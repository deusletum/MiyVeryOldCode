' ======================================================================
' Copyright . 2004
'
' Module : Import_SqlCov.vbs 
'
' Summary:
'           checks prereqs to ensure that MOM is isntalled
'
'           runs SqlCovReport.exe to merge the local coverage
'           data with the other data for that build / DB in the
'           reporting server.
'           
' Returns:
'          -1 .Run method of shell object returned error
'           0 success
'           1 invalid arguments
'           2 MOM component requested is not installed
'           in the event of external exe failure, return code from .exe
'
' History: (5/6/2004) MARKING - Initial coding
'          (5/7/2004) MARKING - incorporated feedback from code review
'                               updated function headers
'                               updated ParseArgs based on code review feedback
' ======================================================================

Option Explicit
Const VERSIONINFO = "Version 0.9"
Const DESTDIR = "\SqlCoverage"
Const IMPORTCMD = "cmd /c DESTDIR\SqlCovReport merge REPORTDB fromDB LOGGINGDB forBuild COVERDB BUILDNAME > OUTPUTFILE 2>>&1"

'production values
Const IMPORTUSER = "SqlCoverage_UserRW"
Const IMPORTPASS = "Coverage_Write"
Const IMPORTSRV = "SMDATA"
Const REPORTDB = "SqlCoverage"

'test values
'Const IMPORTUSER = "sa"
'Const IMPORTPASS = "SMX#2001"
'Const IMPORTSRV = "ZEUS72E"
'Const REPORTDB = "SqlCoverage"



' Create nice universal objects because I am lazy
Dim g_objFSO        ' File System object
Dim g_objShell      ' Windows Scripting Host Shell object
Dim g_log           ' log file
Dim g_scriptName    ' holds name of script
Dim g_verbose       ' send message to screen?
Dim g_retVal        ' what did Main return?
Dim g_destDir       ' would be a constant, but VBScript doesn't allow code in constant declarations
Dim g_reportDB      ' ditto

Set g_objShell = CreateObject("WScript.Shell")
Set g_objFSO   = CreateObject("Scripting.FileSystemObject")
g_scriptName = g_objFSO.GetBaseName(WScript.ScriptName)
g_verbose = False
g_destDir = g_objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%") & DESTDIR
g_reportDB = IMPORTUSER & ":" & IMPORTPASS & "@"& IMPORTSRV & "." & REPORTDB



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
' Function : ImportCoverageResults
'
' Summary:
'           replaces tokens in IMPORTCMD const with runtime
'           values for directory containing .exe file,
'           the name of the repoting DB,
'           the buildNumber pre-pended by "MOM.",
'           the DB that was analyzed with either "OnePoint", or "SystemCenterReporting",
'           the DB containing the coverage data (currenty the same as analyzed DB),
'           and the name of the log file that will catch all output from the .exe
'
'           runs the resulting command using RunCMD
'           
' Arguments:
'           dir - the location of the .exe file
'           buildNumber - the build number of the component that was analyzed
' Returns:
'           exit code from RunCMD
' ======================================================================
Function ImportCoverageResults (ByVal dir, ByVal buildNumber)
    Dim import
    Dim logFile : logfile = g_objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%") & "\logs\SqlCovReport.log"

    import = IMPORTCMD
    import = Replace(import, "DESTDIR", dir)
    import = Replace(import, "REPORTDB", g_reportDB)
    import = Replace(import, "BUILDNAME", "MOM." & buildNumber)
    If "DB" = UCase(WScript.Arguments.Named.Item("component")) Then
        import = Replace(import, "COVERDB", "OnePoint")
        import = Replace(import, "LOGGINGDB", "OnePoint")
    Else
        import = Replace(import, "COVERDB", "SystemCenterReporting")
        import = Replace(import, "LOGGINGDB", "SystemCenterReporting")
    End If
    import = Replace(import, "OUTPUTFILE", logFile)
    ImportCoverageResults = RunCMD(import)

End Function

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
        LogError "MOM component not installed"
        Exit Function
    Else
        LogMsg "MOM build " & momVersion & " found", g_verbose
    End If

    LogMsg "prereq checking done, proceeding with installation of coverage tools", g_verbose

    Main = ImportCoverageResults (g_destDir, momVersion)

End Function