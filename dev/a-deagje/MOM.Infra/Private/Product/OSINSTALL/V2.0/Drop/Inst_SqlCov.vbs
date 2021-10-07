' ======================================================================
' Copyright . 2004
'
' Module : Inst_SqlCov.vbs
'
' Summary:
'           Installs and configures SQL Code Coverage for MOMx database
'           SQL Code Coverage scripts / binaries come from: 
'           http://ToolBox/details/details.aspx?ToolID=21842
'           contact sqlcvusr for information on coverage tools
'
' Details:
'           Checks prereqs: SQL installed, MOMx DB installed
'           Checks to ensure that OnePoint DB is not already instrumented
'           Copies coverage binaries and sql scripts into place
'           Runs sql scripts to do the following:
'               Register extended sproc dll with sql
'               Register extended sprocs with sql
'               Grant execution privs on extended sprocs to guest and public
'           Executes SqlCodeAnalyze on the MOMx DB to instrument sprocs in OnePoint DB
'
' Returns:
'          -1 .Run method of shell object returned error
'           0 success
'           1 invalid arguments
'           2 MOM component requested is not installed
'           3 could not find SQL install dir
'           4 file copy error
'           5 sql template file not found
'           6 custom sql file cannot be written
'           7 xml template file cannot be found
'           8 custom xml file cannot be written
'           9 dsn reg template cannot be found
'          10 custom dsn reg file cannot be written
'           in the event of external exe failure, return code from .exe
'
' History: (5/6/2004) MARKING - Initial coding
'          (5/7/2004) MARKING - incorporated feedback from code review
'                               updated function headers
'                               updated ParseArgs based on code review feedback
'          (5/10/2004)MARKING - Added code to create System DSN
' ======================================================================

Option Explicit
'WScript.Quit

Const VERSIONINFO = "Version 0.9"
Const SOURCEDIR = "\\smx.net\tools\toolbox\SqlCoverage"
Const DESTDIR = "\SqlCoverage"
Const ANALYZERCMD = "cmd /c DESTDIR\SqlCodeAnalyzer -c CONFIGFILE > OUTPUTFILE 2>>&1"
Const OSQLCMD = "osql -E -i INPUTFILE -o OUTPUTFILE"
Const CONFIG = "config.xml"
Const SQLFILE = "registerExtendedProcs.sql"
Const DSNFILE = "dsn.reg"
Const DSNCMD = "regedit /s INPUTFILE"

Dim g_objFSO        ' File System object
Dim g_objShell      ' Windows Scripting Host Shell object
Dim g_log           ' log file
Dim g_scriptName    ' holds name of script
Dim g_verbose       ' send message to screen?
Dim g_retVal        ' what did Main return?
Dim g_destDir       ' would be a constant, but VBScript doesn't allow code in constant declarations

Set g_objShell = CreateObject("WScript.Shell")
Set g_objFSO   = CreateObject("Scripting.FileSystemObject")
g_scriptName = g_objFSO.GetBaseName(WScript.ScriptName)
g_verbose = False
g_destDir = g_objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%") & DESTDIR


WScript.Echo g_scriptName & " " & VERSIONINFO

g_retVal = Main()
CloseLog
WScript.Quit(g_retVal)



' ======================================================================
'
' Function : InitLog
'
' Summary:
'           initializes log file for writing.  log file name is
'           %SYSTEMDRIVE%\logs\<NameOfScript>.log
' Arguments:
'           none, but relies on global FSO object
' Returns:
'           file object
' ======================================================================
Public Function InitLog ()
    Dim fileName
    Dim overwrite : overwrite = True
    Dim systemDrive

    systemDrive = g_objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%")
    fileName = systemDrive & "\logs\" & g_scriptName & ".log"
    Set InitLog = g_objFSO.CreateTextFile(fileName)
End Function

' ======================================================================
'
' Function : LogMsg
'
' Summary:
'           writes date, time, & message to log file and optionally to screen
' Arguments:
'           Msg - message to write to log file
'           sendToScreen - whether or not to send Msg to screen
'           depends on global file object from InitLog
' Returns:
'           Nothing
' ======================================================================
Public Sub LogMsg (ByVal Msg, ByVal sendToScreen)
    Dim logMsg
    logMsg = "[" & Now & "] " & Msg
    g_log.WriteLine logMsg
    If sendToScreen Then
        Wscript.Echo logMsg
    End If
End Sub

' ======================================================================
'
' Function : LogError
'
' Summary:
'           sends message to log file and screen with ERROR pre-pended
' Arguments:
'           Msg - error message to log
' Returns:
'           Nothing
' ======================================================================
Public Sub LogError (ByVal Msg)
    LogMsg "ERROR - " & Msg, True
End Sub


' ======================================================================
'
' Function : CloseLog
'
' Summary:
'           writes an ending message to log file
' Arguments:
'           none.  relies on global file object from InitLog
' Returns:
' ======================================================================
Public Sub CloseLog ()
    LogMsg g_scriptName & " ended", g_verbose
    g_log.Close
End Sub

' ======================================================================
'
' Function : GetProductInfo
'
' Summary:
'           gets build number for the machine, product,
'           and component passed in.
' Arguments:
'           strComputer  -- computer from which to get information
'           strProduct   -- product for which to get get build number
'           strComponent -- component for which to get build number
' Returns:
'           build number if found
'           "UNKNOWN" on error or missing machine, product, or component
'           "0000" when a matching node for params is found, but buildnumber
'                  is missing.  very rare.
' ======================================================================
Public Function GetProductInfo (ByVal strComputer, ByVal strProduct, ByVal strComponent)
    On Error Resume Next
    Dim objCfg 'object that holds the TAT object
    Dim machine, product, component 'variables that are used for enumeration

    If (UCase(strProduct) = "MOMX") Then
        strProduct = "MOM"
    End If

    GetProductInfo = "UNKNOWN"
    Set objCfg = CreateObject("Infra.TopologyAuthoring.Config")
    objCfg.DeserializeFromFile(g_objFSO.BuildPath(g_objShell.Environment("Process").Item("SystemRoot"),"config.xml"))

    'Walk the list of machines, products, and components to get the version information we're after
    For Each machine in objCfg.Machines
        If (Trim(UCase(machine.Name)) = Trim(UCase(strComputer))) Then
            For Each product in machine.Products
                If (Trim(UCase(product.Name)) = Trim(UCase(strProduct))) Then
                    For Each component in product.Components
                        If (Trim(UCase(component.Name)) = Trim(UCase(strComponent))) Then
                            If (component.Build <> "") Then
                                GetProductInfo = component.Build
                            Else
                                GetProductInfo = "0000"
                            End If
                            Exit Function
                        End If
                    Next 'component
                End If
            Next 'product
        End If
    Next 'machine

End Function

' ======================================================================
'
' Function : GetSqlInstallDir
'
' Summary:
'           looks in the registry to find out where SQL is installed
' Arguments:
'           instance name, although currently only the default unnamed
'           instance is supported due to external tool problems.
' Returns:
'           string representing SQL install directory on success
'           empty string on failure
' ======================================================================
Function GetSqlInstallDir (ByVal instanceName)
    Dim regPath
    Dim retVal

    If "DEFAULT" = UCase(instanceName) Then
        regPath = "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSSQLServer\Setup\SQLPath"
        retVal = g_objShell.RegRead(regPath)
        LogMsg "SQL installed at " & retVal, g_verbose
    Else
        LogError "script only supports unnamed instances at this time"
        retVal = ""
    End If
    GetSqlInstallDir = retVal
End Function

' ======================================================================
'
' Function : CopyFiles
'
' Summary:
'           copies all files from one directory to another
'           optionally will copy subfolders as well
' Arguments:
'           src - directory that has files to be copied
'           dest - directory into which files will be copied
'           wholeTree - flag to indicate whether subfolders should
'                       copied as well.
' Returns:
'           0 on success
'           4 on failure
' ======================================================================
Function CopyFiles(ByVal src, ByVal dest, ByVal wholeTree)
    Dim overwrite : overwrite = True
    Dim file
    On Error Resume Next
    CopyFiles = 0
    g_objFSO.CreateFolder(dest)

    Err.Clear
    If wholeTree Then
        g_objFSO.CopyFolder src & "\*", dest & "\", overwrite
        If 0 <> Err.number Then
            LogError "{" & Err.Description & "," & Err.number & "}"
            CopyFiles = 4
            Exit Function
        End If
    Else
        g_objFSO.CopyFile src & "\*", dest & "\", overwrite
        If 0 <> Err.number Then
            LogError "{" & Err.Description & "," & Err.number & "}"
            CopyFiles = 4
            Exit Function
        End If
    End If
End Function

' ======================================================================
'
' Function : RunCMD
'
' Summary:
'           executes command passed in using .Run method of shell object
' Arguments:
'           strCMD - command line to be executed
'           also relies on global shell object
' Returns:
'           exit code of process specified in strCMD on success
'           -1 if there was an error reported by .Run method
' ======================================================================
Function RunCMD(strCMD)
    On Error Resume Next
    Dim retVal
    Dim waitForIt : waitForIt = True
    Dim minimizedAndInactive : minimizedAndInactive = 7

    LogMsg "Running:" & strCMD, g_verbose
    retVal = g_objShell.Run(strCMD, minimizedAndInactive, waitForIt)
    If 0 <> Err.number Then
        LogError "{" & Err.Description & "," & Err.number & "}"
        RunCMD = -1
        Exit Function
    End If

    RunCMD = retVal
End Function

' ======================================================================
'
' Function : RegisterXProcs
'
' Summary:
'           reads in a templatized .sql file from local disk and
'           replaces DESTINATIONDIR token in file with runtime values
'           of where the xproc dll is stored.
'
'           replaces tokens in OSQLCMD constant with the names of 
'           customized sql file and log file name
'
'           executes osql.exe to run the .sql script
' Arguments:
'           dir - directory containing xproc dll
'           relies on SQL being installed and osql.exe on the PATH
' Returns:
'           5 if template sql file cannot be found
'           6 if customized sql file cannot be written
'           osql exit code on success
' ======================================================================
Function RegisterXProcs (ByVal dir)
    On Error Resume Next
    Dim inputFile
    Dim contents
    Dim overwrite : overwrite = True
    Dim osql
    Dim queryFileName

    Err.Clear
    Set inputFile = g_objFSO.OpenTextFile(g_objFSO.BuildPath(dir, SQLFILE))
    If 0 <> Err.number Then
        LogError "{" & Err.Description & "," & Err.number & "}"
        RegisterXProcs = 5
        Exit Function
    End If

    contents = inputFile.ReadAll
    inputFile.Close

    contents = Replace(contents, "DESTINATIONDIR", dir)
    queryFileName = g_objFSO.BuildPath(g_objShell.ExpandEnvironmentStrings("%TEMP%"), SQLFILE)
    Err.Clear
    Set inputFile = g_objFSO.CreateTextFile(queryFileName, overwrite)
    If 0 <> Err.number Then
        LogError "{" & Err.Description & "," & Err.number & "}"
        RegisterXProcs = 6
        Exit Function
    End If
    inputFile.Write contents
    inputFile.Close

    osql = OSQLCMD
    osql = Replace(osql, "INPUTFILE", queryFileName)
    osql = Replace(osql, "OUTPUTFILE", g_objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%") _
                                & "\logs\" & g_objFSO.GetBaseName(SQLFILE) & ".log")

    RegisterXProcs = RunCMD(osql)
End Function

' ======================================================================
'
' Function : RunAnalyzer
'
' Summary:
'           reads in templatized config.xml file and replaces
'           tokens for server name, build name, and database name
'           with runtime values.
'
'           executes SqlCodeAnalyzer via strCMD
' Arguments:
'           buildNumber - build number of product
' Returns:
'           exit code from RunCMD
' ======================================================================
Function RunAnalyzer (ByVal buildNumber)
    On Error Resume Next
    Dim analyzer
    Dim file
    Dim contents
    Dim logFile : logfile = g_objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%") & "\logs\SqlCodeAnalyzer.log"
    Dim dir : dir = g_objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%") & DESTDIR
    Dim fileName : fileName = g_objShell.ExpandEnvironmentStrings("%TEMP%") & "\config.xml"

    Err.Clear
    Set file = g_objFSO.OpenTextFile(dir & "\" & CONFIG)
    If 0 <> Err.number Then
        LogError "{" & Err.Description & "," & Err.number & "}"
        RunAnalyzer = 7
        Exit Function
    End If

    contents = file.ReadAll
    file.Close

    contents = Replace(contents, "BUILD_NUMBER", "MOM." & buildNumber)
    contents = Replace(contents, "MACHINENAME", g_objShell.ExpandEnvironmentStrings("%COMPUTERNAME%"))
    If "DB" = UCase(WScript.Arguments.Named.Item("component")) Then
        contents = Replace(contents, "DATABASENAME", "OnePoint")
    Else
        contents = Replace(contents, "DATABASENAME", "SystemCenterReporting")
    End If

    Err.Clear
    Set file = g_objFSO.CreateTextFile(fileName)
    If 0 <> Err.number Then
        LogError "{" & Err.Description & "," & Err.number & "}"
        RunAnalyzer = 8
        Exit Function
    End If

    file.Write(contents)
    file.Close

    analyzer = ANALYZERCMD
    analyzer = Replace(analyzer, "OUTPUTFILE", logFile)
    analyzer = Replace(analyzer, "DESTDIR", dir)
    analyzer = Replace(analyzer, "CONFIGFILE", fileName)

    RunAnalyzer = RunCMD(analyzer)

End Function

' ======================================================================
'
' Function : CreateSystemDSN
'
' Summary:
'           reads in templatized dsn.reg file and replaces
'           token for WINDIR with the runtime value.
'
'           executes regedit via strCMD
' Returns:
'           exit code from RunCMD on success
'           9 - could not find DSN template file
'          10 - could not create custom DSN file
' ======================================================================
Function CreateSystemDSN ()
    On Error Resume Next
    Dim dsn
    Dim file
    Dim contents
    Dim dir : dir = g_objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%") & DESTDIR
    Dim fileName : fileName = g_objShell.ExpandEnvironmentStrings("%TEMP%") & "\dsn.reg"
    Dim winDir : winDir = Replace(g_objShell.ExpandEnvironmentStrings("%WINDIR%"), "\", "\\")

    Err.Clear
    Set file = g_objFSO.OpenTextFile(dir & "\" & DSNFILE)
    If 0 <> Err.number Then
        LogError "{" & Err.Description & "," & Err.number & "}"
        CreateSystemDSN = 9
        Exit Function
    End If

    contents = file.ReadAll
    file.Close

    contents = Replace(contents, "WINDIR", winDir)

    Err.Clear
    Set file = g_objFSO.CreateTextFile(fileName)
    If 0 <> Err.number Then
        LogError "{" & Err.Description & "," & Err.number & "}"
        CreateSystemDSN = 10
        Exit Function
    End If

    file.Write(contents)
    file.Close

    dsn = DSNCMD
    dsn = Replace(dsn, "INPUTFILE", fileName)

    CreateSystemDSN = RunCMD(dsn)
End Function

' ======================================================================
'
' Function : InstallAndConfigureSqlCoverage
'
' Summary:
'           'driver' function.  this function calls CopyFiles to get
'           coverage files on the local drive.
'
'           calls RegisterXProcs to get xprocs registered with sql server
'
'           calls RunAnalyzer to execute SqlCodeAnalyzer
' Arguments:
'           destinationDir - directory to which files should be copied
'           buildNumber - build number of the mom component being analyzed
' Returns:
'           0 on success
'           return value from other functions on failure
' ======================================================================
Function InstallAndConfigureSqlCoverage (ByVal destinationDir, ByVal buildNumber)
    Dim retVal
    Dim goDeep : goDeep = False

    retVal = CopyFiles(SOURCEDIR, destinationDir, goDeep)
    If 0 <> retVal Then
        LogError "Could not copy files from " & SOURCEDIR & " to " & destinationDir
        InstallAndConfigureSqlCoverage = retVal
        Exit Function
    End If

    retVal = RegisterXProcs(destinationDir)
    If 0 <> retVal Then
        LogError "Could not register extended sprocs"
        InstallAndConfigureSqlCoverage = retVal
        Exit Function
    End If

    retVal = CreateSystemDSN
    If 0 <> retVal Then
        LogError "regedit returned an error"
        InstallAndConfigureSqlcoverage = retVal
        Exit Function
    End If

    retVal = RunAnalyzer (buildNumber)
    If 0 <> retVal Then
        LogError "sproc analyzer returned an error"
        InstallAndConfigureSqlcoverage = retVal
        Exit Function
    End If
End Function

' ======================================================================
'
' Function : Usage
'
' Summary:
'           displays usage to user
' Arguments:
'           None
' Returns:
'           Nothing
' ======================================================================
Sub Usage
    WScript.Echo "Usage:"
    WScript.Echo
    WScript.Echo g_scriptName & " /component:<db|reporting> [/verbose]"
    WScript.Echo "e.g. " & g_scriptName & " /component:db"
    WScript.Echo "e.g. " & g_scriptName & " /component:reporting /verbose"
End Sub


' ======================================================================
'
' Function : ParseArgs
'
' Summary:
'           parses command line arguments, ensuring that all arguments
'           are named and that at least /component is specified.
'
'           ensures that value of component is legal
'
'           if /verbose is specified, sets global verbose flag
'
'           if any other named parameters are specified, returns error
' Arguments:
'           none, gets data from WScript.Arguments
' Returns:
'           0 on success
'           1 on failure
' ======================================================================
Function ParseArgs
    Dim arg
    Dim NumNamedArgs : NumNamedArgs = WScript.Arguments.Named.Count
    Dim MinArgs : MinArgs = 1
    Dim MaxArgs : MaxArgs = 2

    ParseArgs = 0
    If 0 <> WScript.Arguments.Unnamed.Count Then
        ' No args to parse
        ParseArgs = 1
    ElseIf NumNamedArgs < MinArgs Or NumNamedArgs > MaxArgs Then
        ' Wrong number of args
        ParseArgs = 1
    ElseIf Not WScript.Arguments.Named.Exists("component") Then
        ' Component is a required arg
        ParseArgs = 1
    Else
        For Each arg in WScript.Arguments.Named
            Select Case UCase(arg)
                Case "COMPONENT"
                    Select Case UCase(WScript.Arguments.Named.Item(arg))
                        Case "DB"
                            'Nothing to do, this is a good arg
                        Case "REPORTING"
                            'Nothing to do, this is a good arg
                        Case Else
                            'the component option was neither DB nor REPORTING
                            ParseArgs = 1
                        Exit For
                    End Select
                Case "VERBOSE"
                    g_verbose = True
                Case Else
                    ParseArgs = 1
                    Exit For
            End Select
        Next
    End If
End Function



' ======================================================================
'
' Function : Main
'
' Summary:
'           gets everything started.  initializes log, checks params
'           and checks prereqs (MOM and SQL are installed)
' Arguments:
'           none.
' Returns:
'          -1 .Run method of shell object returned error
'           0 success
'           1 invalid arguments
'           2 MOM component requested is not installed
'           3 could not find SQL install dir
'           4 file copy error
'           5 sql template file not found
'           6 custom sql file cannot be written
'           7 xml template file cannot be found
'           8 custom xml file cannot be written
'           in the event of external exe failure, return code from .exe
' ======================================================================
Function Main ()
    Dim momVersion
    Dim sqlInstallDir
    Dim retVal

    Set g_Log = InitLog
    'anything on the command line means the user wants more info printed to the screen
    LogMsg g_scriptName & " started", g_verbose

    retVal = ParseArgs
    If 0 <> retVal Then
        LogError "Invalid Parameters"
        Usage
        Main = retVal
        Exit Function
    End If

    LogMsg "Checking for MOM installation", g_verbose
    momVersion = GetProductInfo(g_objShell.ExpandEnvironmentStrings("%COMPUTERNAME%"), "MOM", _
                                UCase(WScript.Arguments.Named.Item("component")))
    If "UNKNOWN" = momVersion OR "0000" = momVersion Then
        Main = 2
        LogError "MOM componenet not installed"
        Exit Function
    Else
        LogMsg "MOM build " & momVersion & " found", g_verbose
    End If

    sqlInstallDir = GetSqlInstallDir ("DEFAULT")
    If "" = sqlInstallDir Then
        LogError "could not find SQL installation directory"
        Main = 3
        Exit Function
    End If

    LogMsg "prereq checking done, proceeding with installation of coverage tools", g_verbose

    Main = InstallAndConfigureSqlCoverage (g_destDir, momVersion)

End Function