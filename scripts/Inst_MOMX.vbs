' ----------------------------------------------------------------------
' Name      : Inst_MOMX.vbs
'
' Company   : oration
' 
'
' Summary   : Installs MOMX products: MOMX, MOMX DB, MOMX Server
'               , UI, Agent, and Reporting
'
' Usage     : See usage Function
'
' ErrorCodes: 1 - bad user input
'             2 - SQL Server Not installed
'             3 - Unable to create CDScom object
'             4 - path not found or unavalible
'             5 - MSI install failure
'
' History   : 4/25/2003 - Dean Gjedde - Created
'             4/28/2003 - Dean Gjedde - made changes to script reQuested
'               in code review (see each section for details)
' ----------------------------------------------------------------------
Option Explicit
Dim g_cQuote
g_cQuote = Chr(34)

Main()

' -------------------------------------------------------------------
' summary:
'    the RunCMD runs a command and get there errorlevel after the
'       command is run
'
' param:
'    param name="strCMD" dir="IN" 
'    command to be executed
'
' returns:
'    retruns the errorlevel of the command
'
' history:
'   04/17/20 deangj First Creation
' -------------------------------------------------------------------
Function RunCMD(strCMD)
    Dim objShell
    Dim ExitCode
    
    Set objShell = WScript.CreateObject("WScript.Shell")
    WScript.Echo("Running:")
    WScript.Echo(strCMD)
    ExitCode = objShell.Run(strCMD,, True)
    
    RunCMD = ExitCode
End Function

' -------------------------------------------------------------------
' summary:
'    gets the default account for the domain the user belongs to
'
' param:
'    param name="strAccount" dir="IN" 
'    a variable to hold the account info pass in by refference
'
' param:
'    param name="strLog" dir="IN" 
'    Log file name
'
' returns:
'    retruns nothing
'
' history:
'   04/29/20 deangj First Creation
' -------------------------------------------------------------------
Function GetDefaultAccount(byref strAccount, strLog)
    Dim strUser
    Dim strPassword
    Dim objCDS
    Dim intResults
    
    Set objCDS = WScript.CreateObject("CDSCom.Users")
    If Err.Number <> 0 Then
        WriteLog "VAR_ABORT - Unable to create CDSCom object", strLog, 3
    End If
    intResults = objCDS.GetDefaultCredentials(strAccount, "")
    If intResults <> 0 Then
        WriteLog "VAR_ABORT - no default account for your domain in CDS", strLog, 3
    End If
    
    Set objCDS = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    gets the password for the specific user account
'
' param:
'    param name="strAccount" dir="IN" 
'    account to get password for
'
' param:
'    param name="strLog" dir="IN" 
'    name of the log file
'
'returns:
'    returns a string containing the password
'
' history:
'    04/17/2003 deangj First Creation
'    04/28/2003 deangj added parm strLog and check if unable To
'       create CDSCom
' -------------------------------------------------------------------
Function GetAccountPassword(strAccount, strLog)
    On Error Resume Next
    Dim objCDS
    Dim strPassword
    
    Set objCDS = WScript.CreateObject("CDSCom.Users")
    If Err.Number <> 0 Then
        WriteLog "VAR_ABORT - Unable to create CDSCom object", strLog, 3
    End If
    strPassword = objCDS.GetPassword(strAccount)
    GetAccountPassword = strPassword
    
    Set objCDS = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    gets the latest build number for the product in strProduct
'
' param:
'    param name="strProduct" dir="IN" 
'    name of the product to get latest build number
'
' param:
'    param name="strLog" dir="IN" 
'    name of the log file
'
'returns:
'    returns a build number
'
' history:
'    04/17/2003 deangj First Creation
'    04/28/2003 deangj added parm strLog and check if unable To
'       create CDSCom
' -------------------------------------------------------------------
Function GetLatest(strProduct, strLog)
    Dim objCDS
    Dim strBuild
    
    Set objCDS = WScript.CreateObject("CDSCom.Builds")
    If Err.Number <> 0 Then
        WriteLog "VAR_ABORT - Unable to create CDSCom object", strLog, 3
    End If
    strBuild = objCDS.GetLatestBuildByToken(strProduct, "Latest")
    GetLatest = strBuild
 
    Set objCDS = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    Checks for SQL Server Service
'
'returns:
'    Returns true if SQL Server Service is on the box and fale if not
'
' history:
'    04/17/2003 deangj First Creation
' -------------------------------------------------------------------
Function CheckForSQL()
    Dim objLocator
    Dim objServices
    Dim InfoSet
    Dim Info
    
    Set objLocator = CreateObject("Wbemscripting.SWbemlocator")
    Set objServices = ObjLocator.ConnectServer(".", "root\CIMv2")
    Set InfoSet = objServices.InstancesOf("Win32_Service")
    
    For Each Info In InfoSet
        Dim strService
        strService = Info.Name
        If UCase(Info.Name) = "MSSQLSERVER" Then
            CheckForSQL = True
            Exit Function
        ElseIf UCase(InStr(Info.Name, "MSSQL$")) Then
            CheckForSQL = True
            Exit Function
        Else
            CheckForSQL = False
        End If
    Next
End Function

' -------------------------------------------------------------------
' summary:
'    the WriteLog function will write messages to a log file And
'    report errors to the user.
'
' param:
'    param name="strMessage" dir="IN" 
'    the message to be writen to the log
'
' param:
'    param name="strLob" dir="IN" 
'    name of the log file
'
' param:
'    param name="intExitCode" dir="IN" 
'    errorlevel number
'
' returns:
'    Nothing
'
' history:
'    05-Nov-02 deangj First Creation
'    12-Nov-02 deangj Added Datetime stamp to log and put all Error
'        into WriteLog
'    12-Dec-03 deangj Corrected the spelling of Description
'    04/28/03 deangj added parm intExitCode
' -------------------------------------------------------------------
Sub WriteLog(strMessage, strLog, intExitCode)
    On Error Resume Next
    Const ForAppending = 8
    
    Dim strLogFile
    Dim objEnv
    Dim objFSO
    Dim objStream
    
    strLogFile = strLog
    Set objFSO = CreateObject("Scripting.FileSystemObject")
    Set objStream = objFSO.OpenTextFile(strLogFile, ForAppending, True)
    If Err.Number <> 0 Then
        objStream.Close
        WScript.Echo("Inst_MOMX was unable to write a log")
        WScript.Echo(Err.Description)
        Exit Sub
    End If
    
    objStream.Write(vbCrlf & "*LOG_START*-Inst_MOMX")
    objStream.Write(vbCrlf & "[" &  FormatDateTime(Date, 2) & " " & _
        FormatDateTime(Time, 4)  & "] ERR Inst_MOMX " & strMessage)
    objStream.Write(vbCrlf & "*LOG_DONE*")
    objStream.Close
    WScript.Echo(strMessage)
    WScript.Quit(intExitCode)
    
    Set objFSO = Nothing
    Set objStream = Nothing
End Sub

' -------------------------------------------------------------------
' summary:
'    uninstalls an msi based install, checks for errors and the Quits the script
'
' param:
'    param name="strVersion" dir="IN" 
'    name of the product being installed
'
' param:
'    param name="strGUID" dir="IN" 
'    the GUID of the installed product
'
' param:
'    param name="strMSILog" dir="IN" 
'    name of the log file
'
' param:
'    param name="bSilent" dir="IN" 
'    if you want silent unstall set to True
'
' param:
'    param name="strLog" dir="IN" 
'    name of the log file
'
'
'returns:
'    Nothing
'
' history:
'    04/17/2003 deangj First Creation
' -------------------------------------------------------------------
Function Uninstall(strVersion, strGUID, strMSILog, bInteractive, strLog)
    Dim strCommand
    Dim intError
    
    If bInteractive = True Then
        strCommand = "msiexec.exe /x " & strGUID & " /lv* " & strMSILog
    Else
        strCommand = "msiexec.exe /x " & strGUID & " /lv* " & strMSILog & " /Qn"
    End If
    
    intError = RunCMD(strCommand)
    If intError <> 0 Then
        WriteLog "VAR_FAIL - Uninstall for " & strVersion & " failed with error number " & intError, strLog, 5
    Else
        WriteLog "VAR_PASS - Uninstall for " & strVersion & " passed", strLog, 0
    End If

End Function

' -------------------------------------------------------------------
' summary:
'    installs an msi based install, checks for errors and the Quits the script
'
' param:
'    param name="strVersion" dir="IN" 
'    name of the product being installed
'
' param:
'    param name="strPath" dir="IN" 
'    path to the product being installed including the msi install file
'
' param:
'    param name="strMSILog" dir="IN" 
'    name of the log file
'
' param:
'    param name="strParms" dir="IN" 
'    MSI Parameters
'
' param:
'    param name="bSilent" dir="IN" 
'    if you want silent unstall set to true
'
' param:
'    param name="strLog" dir="IN" 
'    name of the log file
'
'returns:
'    Nothing
'
' history:
'    04/17/2003 deangj First Creation
' -------------------------------------------------------------------
Function Install(strVersion, strPath, strMSILog, strParms, bInteractive, strLog)
    Dim strCommand
    Dim intError
    
    If bInteractive = True Then
        strCommand = "msiexec.exe /i " & strPath & " /lv* " & strMSILog & strParms
    Else
        strCommand = "msiexec.exe /i " & strPath & " /lv* " & strMSILog & strParms & " /Qn"
    End If
    
    intError = RunCMD(strCommand)
    If intError <> 0 Then
        WriteLog "VAR_FAIL - Install for " & strVersion & " failed with error number " & intError, strLog, 5
    Else
        WriteLog "VAR_PASS - Install for " & strVersion & " passed", strLog, 0
    End If

End Function

' -------------------------------------------------------------------
' summary:
'    displays usage
'
'returns:
'    Nothing
'
' history:
'    04/17/2003 deangj First Creation
'    04/29/2003 deangj made usage a sub and replaced Q with g_cQuote
' -------------------------------------------------------------------
Sub Usage()
    WScript.Echo("Usage:")
    WScript.Echo("cscript.exe Inst_MOMX.vbs /BUILD: /TYPE: /VERSION: /LANGUAGE:" _
        & " /PATH: [/?] [/SERVER:] [/AMCONTROL] [/DBSERVER:] [/I]")
    WScript.Echo(vbTab & "[/NODEBUG] [/DAS_USER:] [/CAM_USER:] [/DBINSTANCE:] " _
        & "[/DISTPATH:] [/CONFIGNAME:] [/Uninstall]")
    WScript.Echo(vbTab & "[/REPORTINGDB:] [/REPORTINGDBSIZE:] [/SQLSERVICENAME:] " _
        & "[/SQLAGENTSERVICENAME] [/MOMX_DBNAME]")
    WScript.Echo("/BUILD: -  Supports build number, LATEST, or BLESSED")
    WScript.Echo("/TYPE: -  Supports FRE, CHK, DEBUG, RETAIL")
    WScript.Echo("/VERSION: - Supports COMPLETE, MOM_SERVER, MOM_DB, UI, AGENT, REPORTING")
    WScript.Echo("/LANGUAGE: - Supports only EN builds, but will support other " _
        & "languages when they become available.")
    WScript.Echo("/PATH: - Exact path for private build, use /PATH:DEFAULT for default path")
    WScript.Echo("/?: Displays usage for inst_momx")
    WScript.Echo("Additional Parameters:")
    WScript.Echo(vbTab & "/SERVER: - Name of MOMX Server if doing an agent install.")
    WScript.Echo(vbTab & "/AMCONTROL: - Scan level for the DCAM if doing an agent install")
    WScript.Echo(vbTab & vbTab & "Use FULL for full contact and NONE if behind a firewall " _
        & "or does not have contact with the DCAM")
    WScript.Echo(vbTab & "/DBSERVER: - Name of the Server where the MOM DB is located if " _
        & "doing a MOM Server, UI, or Reporting install")
    WScript.Echo("Optional:  All parameters after these will be optional.")
    WScript.Echo(vbTab & "/I - Interactive mode")
    WScript.Echo(vbTab & "/NODEBUG - Doesn't start debug view")
    WScript.Echo(vbTab & "/DAS_USER: - Name of the user to use for DAS (domain\user)")
    WScript.Echo(vbTab & vbTab & "If not specified default user will be retrieved from CDS")
    WScript.Echo(vbTab & "/CAM_USER: - Name of the user to use for CAM (domain\user)")
    WScript.Echo(vbTab & vbTab & "If not specified DAS_USER will be used")
    WScript.Echo(vbTab & "/DBINSTANCE: - Name of the SQL Instance to use")
    WScript.Echo(vbTab & vbTab & "If not specified default instance will be used")
    WScript.Echo(vbTab & "/DBSIZE: - Size of the MOMX database in MB")
    WScript.Echo(vbTab & vbTab & "If not specified default size of 500 MB will be used")
    WScript.Echo(vbTab & "/DISTPATH: - Destination path where MOM is going to be installed")
    WScript.Echo(vbTab & vbTab & "If not specified default destination path is used")
    WScript.Echo(vbTab & "/CONFIGNAME: - Name of the MOM configuration group name")
    WScript.Echo(vbTab & vbTab & "If not specified "& g_cQuote & "MOMX_Test" & g_cQuote & " will be used")
    WScript.Echo(vbTab & "/UNINSTALL - Uninstall the product specified in /Version")
    WScript.Echo(vbTab & "/REPORTINGDB - Name of the Reporting database")
    WScript.Echo(vbTab & vbTab & "If not specified the default ReportsServer will be used")
    WScript.Echo(vbTab & "/REPORTINGDBSIZE - Size of the Reporting database in MB")
    WScript.Echo(vbTab & vbTab & "If not specified the default 500 MB will be used")
    WScript.Echo(vbTab & "/SQLSERVICENAME - Name of the SQL Service")
    WScript.Echo(vbTab & vbTab & "If not specified the default MSSQLSERVER will be used")
    WScript.Echo(vbTab & "/SQLAGENTSERVICENAME - Name of the SQL Agent Service")
    WScript.Echo(vbTab & vbTab & "If not specified the default SQLSERVERAGENT well be used")
    WScript.Echo(vbTab & "/MOMX_DBNAME - Name of the MOMX database")
    WScript.Echo(vbTab & vbTab & "If not specified the default OnePoint will be used")
    WScript.Echo("Examples:")
    WScript.Echo("MOMX DB only:")
    WScript.Echo(vbTab & "cscript.exe Inst_MOMX.vbs /BUILD:LATEST /TYPE:RETAIL /VERSION:MOM_DB" _
        & " /LANGUAGE:EN /PATH:DEFAULT /DBINSTANCE:MOMXDB /CONFIGNAME:TEST")
    WScript.Echo("MOMX Complete:")
    WScript.Echo(vbTab & "cscript.exe Inst_MOMX.vbs /BUILD:LATEST /TYPE:RETAIL /VERSION:COMPLETE" _
        & " /LANGUAGE:EN /PATH:DEFAULT /CONFIGNAME:" & g_cQuote & "MOMX test group" & g_cQuote _
        & " /DAS_USER:smx\asttest")
    WScript.Echo("MOMX MOM_Server:")
    WScript.Echo(vbTab & "cscript.exe Inst_MOMX.vbs /Build:latest /Type:retail /Version:MOM_Server" _
        & " /Language:EN /Path:default /DBSERVER:myserver")
    WScript.Echo("MOMX Agent:")
    WScript.Echo("cscript.exe Inst_MOMX.vbs /Build:latest /Type:retail /Version:Agent /Language:EN" _
        & " /Path:default /Server:MOMSERVER /AMCONTROL:FULL")
End Sub

' -------------------------------------------------------------------
' summary:
'    the main function gets everything started.
'
' returns:
'    Nothing
'
' note: line 820 is the last line of execution, this script never
'   exits Sub Main
'
' history:
'    04/17/2003 deangj First Creation
'    04/29/2003 deangj change %70 of this sub
' -------------------------------------------------------------------
Sub Main()
    Const strDefaultPath = "\\astdrop\builds\momx"
    Const strMOMXGUID = "{A759C7F5-209F-45D1-BD6F-0A50679EE66E}"
    Const strMOMXAgentGUID = "{203A4E60-7F96-4410-B026-DEC46DF0185F}"
    Const strMOMXReportingGUID = "{CFE51487-E604-44A2-86E1-86149B910E27}"
    Const strMOMXMSI = "MOMServer.msi"
    Const strMOMXAgentMSI = "MOMAgent.msi"
    Const strMOMXReportingMSI = "MOMReporting.msi"
    Const IA64 = "IA64"
    Const X86 = "X86"
    Const strLog = "Inst_MOMX.Log"
    Const strDebugPath = "\\acstools\tools"
    Const strDebugCMD = "debug.cmd MOMX"
    Const strCDImage
    
    Dim objShell, objEnv, aSplit, aArguments, aDASUser, aCAMUser, strArg
    Dim strBuild, strType, strVersion, strLanguage, strPath, strServer
    Dim strAMControl, strDBServer, strDASUser, strDASUserPassword, strCAMUser
    Dim strCAMUserPassword, strDBInstance, strDBSize, strDistPath ,strConfigName
    Dim strCommand, strComputerName, strReportingDB, strReportingDBSize
    Dim strSQLServiceName, strSQLAgentServiceName, strMOMXDBNAME, bPathDefault
    Dim bUseDist, bDBSize, bInteractive, bNoDebug, bUninstall, bUseCAM
    Dim bReportingDB, bReportingDBSize, bSQLServiceName, bSQLAgentServiceName, bMOMXDBName
    Dim intError,strArch, strParms, objFSO, objStream, strMSIEXECLog, strSystemDrive
    Dim strMSIEXECUninstallLog
    
    bInteractive = False
    bNoDebug = False
    bUninstall = False
    bUseCAM = False
    bDBSize = False
    bUseDist = False
    bReportingDB = False
    bReportingDBSize = False
    bSQLServiceName = False
    strSQLAgentServiceName = False
    bMOMXDBName = False
    
    'Check if inst_momx.vbs is being run under CScript.exe
    If Not((InStr(1, WScript.FullName, "CSCRIPT", vbTextCompare) <> 0)) Then
        WriteLog "VAR_ABORT - This script must be run with CScript.exe", strLog, 1
    End If
    
    'If for correct number of arguments
    Set aArguments = WScript.Arguments
    If aArguments.Count < 5 Then
        Usage()
        WriteLog "VAR_ABORT - Wrong number of arguments", strLog, 1
    End If
    
    If WScript.Arguments(0) = "/?" Then
        Usage()
        WriteLog "VAR_ABORT - Usage was reQuest with the /? switch", strLog, 1 
    End If
        
    'Parse command line arguments
    For Each strArg In WScript.Arguments
        aSplit = Split(strArg, ":", 2)
        Select Case Ucase(aSplit(0))
            Case "/?"
                Usage()
                WriteLog "VAR_ABORT - usage requested", strLog, 1
            Case "/BUILD"
                strBuild = aSplit(1)          
            Case "/TYPE"
                strType = aSplit(1)
            Case "/VERSION"
                strVersion = aSplit(1)
            Case "/LANGUAGE"
                strLanguage = aSplit(1)
            Case "/PATH"
                strPath = aSplit(1)
            Case "/SERVER"
                strServer = aSplit(1)
            Case "/AMCONTROL"
                strAMControl = aSplit(1)
            Case "/DBSERVER"
                strDBServer = aSplit(1)
            Case "/DAS_USER"
                strDASUser = aSplit(1)
            Case "/CAM_USER"
                strCAMUser = aSplit(1)
                bUseCAM = True
            Case "/DBINSTANCE"
                strDBInstance = aSplit(1)
            Case "/DBSIZE"
                strDBSize = aSplit(1)
                bDBSize = True
            Case "/DISTPATH"
                strDistPath = aSplit(1)
                bUseDist = True
            Case "/CONFIGNAME"
                strConfigName = aSplit(1)
            Case "/I"
                bInteractive = True
            Case "/NODEBUG"
                bNoDebug = True
            Case "/UNINSTALL"
                bUNINSTALL = True
            Case "/REPORTINGDB"
                strReportingDB = aSplit(1)
                bReportingDB = True
            Case "/REPORTINGDBSIZE"
                strReportingDBSize = aSplit(1)
                bReportingDBSize = True
            Case "/SQLSERVICENAME"
                strSQLServiceName = aSplit(1)
                bSQLServiceName = True
            Case "/SQLAGENTSERVICENAME"
                strSQLAgentServiceName = aSplit(1)
                strSQLAgentServiceName = True
            Case "/MOMX_DBNAME"
                strMOMXDBNAME = aSplit(1)
                bMOMXDBName = True
            Case Else
                WriteLog "VAR_ABORT - " & strArg & " is not a supported argument", strLog, 1
                WScript.Quit(1)
        End Select
    Next
    
    Set objShell = WScript.CreateObject("WScript.Shell")
    strComputerName = objShell.ExpandEnvironmentStrings("%COMPUTERNAME%")
    strSystemDrive = objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%")
    strArch = objShell.ExpandEnvironmentStrings("%PROCESSOR_ARCHITECTURE%") 
    strMSIEXECLog = strSystemDrive & "\MOMX_Install.log"
    strMSIEXECUninstallLog = strSystemDrive & "\MOMX_Uninstall.log"
    Set objFSO = CreateObject("Scripting.FileSystemObject")
    
    'Check if Processor type is ia64
    If UCase(strArch) = IA64 And UCase(strVersion) <> "AGENT" Then
        WriteLog "VAR_ABORT - /VERSION:" & strVersion & " can only be installed on x86" _
            & " systems, this is an IA64", strLog, 1
    Else
        strArch = "i386"
    End If
        
    'Check if /BUILD:, /TYPE:, /VERSION:, /LANGUAGE:, and /PATH: switches are empty 
    If strBuild = "" And strType = "" And strVersion = "" And strLanguage = "" And strPath = "" Then
        Usage()
        WriteLog "VAR_ABORT - /BUILD:, /TYPE:, /VERSION:, /LANGUAGE:, and /PATH: switches are empty", strLog, 1
    End If
    
    'Check if SQL Server is installed
    If UCase(strVersion) = "COMPLETE" Or UCase(strVersion) = "MOM_DB" Or UCase(strVersion) = "REPORTING" Then
        If Not(CheckForSQL) Then
            WriteLog "VAR_ABORT - SQL Server must be installed before install MOMX", strLog, 2
        End If
    End If
    
    'If /path: is not default check if it exists
    If UCase(strPath) <> "DEFAULT" Then
        If Not(PathExist(strPath)) Then
            WriteLog "VAR_ABORT - " & strPath & " does not exist or is not accessable", strLog, 4
        End If
        bPathDefault = False
    Else
        bPathDefault = True
    End If
    
    'If install MOM_SERVER check for empty /DBSERVER: switch
    If UCase(strVersion) = "MOM_SERVER" And UCase(strDBServer) = "" Then
        WriteLog "VAR_ABORT - /DBSERVER: switch must be used with /VERSION:MOM_SERVER", strLog, 1
    End If
    
    'If install MOMXUI check for empty /DBSERVER: switch
    If UCase(strVersion) = "UI" And UCase(strDBServer) = "" Then
        WriteLog "VAR_ABORT - /DBSERVER: switch must be used with /VERSION:UI", strLog, 1
    End If
    
    'If install REPORTING check for empty /DBSERVER: switch
    If UCase(strVersion) = "REPORTING" And UCase(strDBServer) = "" Then
        WriteLog "VAR_ABORT - /DBSERVER: switch must be used with /VERSION:REPORTING", strLog, 1
    End If
    
    'Check if Agent install has the required switches
    If UCase(strVersion) = "AGENT" Then
        If UCase(strAMControl) <> "FULL" And UCase(strAMControl) <> "NONE" Then
            WriteLog "VAR_ABORT - /AMCONTROL: switch must be either NONE or FULL /VERSION:AGENT", strLog, 1
        End If
        If strServer = "" Then
            WriteLog "VAR_ABORT - /SERVER: switch must be used with /VERSION:AGENT", strLog, 1
        End If
    End If

    'Check if DAS user is specified, if not set to smx\asttest and then get the password for that account
    If strDASUser = "" Then
        Dim intErr
        GetDefaultAccount strDASUser, strLog
    End If
    aDASUser = Split(strDASUser, "\", 2)
    strDASUserPassword = GetAccountPassword(strDASUser, strLog)
    
    'Check if CAM user is specified, if not use DAS account
    If (bUseCAM) Then 
        strCAMUserPassword = GetAccountPassword(strCAMUser, strLog)
        aCAMUser = Split(strCAMUser, "\", 2)
    End If
    
    'Check if DBInstance is empty
    If strDBInstance = "" Then
        strDBInstance = strComputerName
    End If
    
    'check if Config name is empty
    If strConfigName = "" Then
        strConfigName = "MOMX_Test"
    End If
    
    If UCase(strType) = "FRE" Then
        strType = "RETAIL"
    Elseif UCase(strType) = "CHK" Then
        strType = "DEBUG"
    End If
    
    'Check if /TYPE: has the correct values
    If UCase(strType) <> "RETAIL" And UCase(strType) <> "DEBUG" Then
        WriteLog "VAR_ABORT - /TYPE: switch can only take values RETAIL, DEBUG, FRE, or CHK", strLog, 1
        WScript.Quit(1)
    End If
     
    'Enable debugging
    If Not(bNoDebug) Then
        If (objFSO.FolderExists(strDebugPath)) Then
            intError = RunCMD(strDebugPath & "\" & strDebugCMD)
            If intError <> 0 Then
                WriteLog "VAR_ABORT - Unable to execute " & strDebugPath & "\" & strDebugCMD, 1
            End If
        Else
            WriteLog "VAR_ABORT - " & strDebugPath & " does not exist or is not accessable", strLog, 4 
            WScript.Quit(1)
        End If
    End If
    
    'Uninstall If bUninstall is true
    If (bUninstall) Then
        Select Case UCase(strVersion)
            Case "COMPLETE"
                Uninstall strVersion, strMOMXGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "MOM_SERVER"
                Uninstall strVersion, strMOMXGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "MOM_DB"
                Uninstall strVersion, strMOMXGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "UI"
                Uninstall strVersion, strMOMXGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "AGENT"
                Uninstall strVerions, strMOMXAgentGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "REPORTING"
                Uninstall strVersion, strMOMXReportingGUID, strMSIEXECUninstallLog, bInteractive, strLog
        End Select
    End If
    
    'Build Parameters
    Select Case UCase(strVersion)   
        Case "COMPLETE"
            strParms = " DAS_ACCOUNT=" & g_cQuote & aDASUser(1) & g_cQuote & " DAS_PASSWORD=" & g_cQuote _ 
                & strDASUserPassword & g_cQuote & " DAS_DOMAIN=" & g_cQuote & aDASUser(0) & g_cQuote
            If (bUseCAM) Then
                strParms = strParms & " CAM_ACCOUNT=" & g_cQuote & aCAMUser(1) & g_cQuote & " CAM_PASSWORD=" _
                    & g_cQuote & strCAMUserPassword & g_cQuote & " CAM_DOMAIN=" & g_cQuote & aCAMUser(0) & g_cQuote
            End If
            strParms = strParms & " SQLSVR_INSTANCE=" & g_cQuote & strDBInstance & g_cQuote & " CONFIG_GROUP=" _ 
                & g_cQuote & strConfigName & g_cQuote
            If (bUseDist) Then
                strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
            End If
            If (bDBSize) Then
                strParms = strParms & " DB_SIZE=" & g_cQuote & strDBSize & g_cQuote
            End If
        Case "MOM_SERVER"
            strParms = " InstallType=" & g_cQuote & "Custom" & g_cQuote & " ADDLOCAL=" & g_cQuote & "MOMXServer" _
                & g_cQuote
            strParms = strParms & " DAS_ACCOUNT=" & g_cQuote & aDASUser(1) & g_cQuote & " DAS_PASSWORD=" _
                & g_cQuote & strDASUserPassword & g_cQuote & " DAS_DOMAIN=" & g_cQuote & aDASUser(0) & g_cQuote
            If (bUseCAM) Then
                strParms = strParms & " CAM_ACCOUNT=" & Q & aCAMUser(1) & g_cQuote &" CAM_PASSWORD=" & g_cQuote _
                    & strCAMUserPassword & g_cQuote & " CAM_DOMAIN=" & g_cQuote & aCAMUser(0) & g_cQuote
            End If
            strParms = strParms & " SQL_SERVER=" & g_cQuote & strDBServer & g_cQuote & " CONFIG_GROUP=" & g_cQuote _
                & strConfigName & g_cQuote
            If (bUseDist) Then
                strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
            End If
        Case "MOM_DB"
            strParms = " InstallType=" & g_cQuote & "Custom" & g_cQuote & " ADDLOCAL=" & g_cQuote & "MOMXDB" _ 
                & g_cQuote
            strParms = strParms & " DAS_ACCOUNT=" & g_cQuote & aDASUser(1) & g_cQuote & " DAS_PASSWORD=" _
                & g_cQuote & strDASUserPassword & g_cQuote & " DAS_DOMAIN=" & g_cQuote & aDASUser(0) & g_cQuote
            If (bUseCAM) Then
                strParms = strParms & " CAM_ACCOUNT=" & g_cQuote & aCAMUser(1) & g_cQuote &" CAM_PASSWORD=" _
                    & g_cQuote & strCAMUserPassword & g_cQuote & " CAM_DOMAIN=" & g_cQuote & aCAMUser(0) & g_cQuote
            End If
            strParms = strParms & " SQLSVR_INSTANCE=" & g_cQuote & strDBInstance & g_cQuote & " CONFIG_GROUP=" _
                & g_cQuote & strConfigName & g_cQuote
            If (bUseDist) Then
                strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
            End If
            If (bDBSize) Then
                strParms = strParms & " DB_SIZE=" & g_cQuote & strDBSize & g_cQuote
            End If
        Case "UI"
            strParms = " InstallType=" & g_cQuote & "Custom" & g_cQuote & " ADDLOCAL=" & g_cQuote & "MOMXUI" _
                & g_cQuote
            strParms = strParms & " DAS_ACCOUNT=" & g_cQuote & aDASUser(1) & g_cQuote & " DAS_PASSWORD=" _
                & g_cQuote & strDASUserPassword & g_cQuote & " DAS_DOMAIN=" & g_cQuote & aDASUser(0) & g_cQuote
            If (bUseCAM) Then
                strParms = strParms & " CAM_ACCOUNT=" & g_cQuote & aCAMUser(1) & g_cQuote &" CAM_PASSWORD=" _
                    & g_cQuote & strCAMUserPassword & g_cQuote & " CAM_DOMAIN=" & g_cQuote & aCAMUser(0) & g_cQuote
            End If
            strParms = strParms & " SQL_SERVER=" & g_cQuote & strDBServer & g_cQuote
            If (bUseDist) Then
                strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
            End If
        Case "AGENT"
            strParms = " CONSOLIDATOR=" & g_cQuote & strServer & g_cQuote & " CONFIG_GROUP=" & g_cQuote _
                & strConfigName & g_cQuote & " AM_CONTROL=" & g_cQuote & strAMControl & g_cQuote
            If (bUseDist) Then
                strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
            End If
        Case "REPORTING"
            strParms = " SQL_SERVER=" & g_cQuote & strDBServer & g_cQuote & " SQLSVR_INSTANCE=" & g_cQuote _
                & strDBInstance & g_cQuote
            If (bMOMXDBName) Then
                strParms = strParms & " MOMX_DBNAME=" & g_cQuote & strMOMXDBNAME & g_cQuote
            End If
            If (bReportingDB) Then
                strParms = strParms & " MOMX_REPORTING_DB=" & g_cQuote & strReportingDB & g_cQuote
            End If
            If (bReportingDBSize) Then
                strParms = strParms & " DB_SIZE=" & g_cQuote & strReportingDBSize & g_cQuote
            End If
            If (bSQLServiceName) Then
                strParms = strParms & " SQL_SERVER_SERVICE_NAME=" & g_cQuote & strSQLServiceName & g_cQuote
            End If
            If (bSQLAgentServiceName) Then
                strParms = strParms & " SQL_AGENT_SERVICE_NAME=" & g_cQuote & strSQLAgentServiceName & g_cQuote
            End If             
    End Select
    
    'Install from path specified by the user
    If Not(bPathDefault)Then
        Install strVersion, strPath & "\" & strMOMXMSI, strMSIEXECLog, strParms, bInteractive, strLog
    End If
    
    'Build default path
    strCommand = strDefaultPath & "\" & strLanguage & "\"
    If UCase(strBuild) = "LATEST" Then
        strCommand = strCommand & GetLatest("MOMX") & "\" & strType & "\" & strArch & "\" & strCDImage
    Else
        strCommand = strCommand & strBuild & "\" & strType & "\" & strArch & "\" & strCDImage
    End If
    
    'Check if path is correct
    If Not(objFSO.FolderExists(strCommand)) Then
        WriteLog "VAR_ABORT - " & strCommand & " does not exist or is not accessable", strLog
        WScript.Quit(1)
    End If

    
    Select Case UCase(strVersion)
        Case "COMPLETE"
            strCommand = strCommand & "\" & strMOMXMSI
        Case "MOM_SERVER"
            strCommand = strCommand & "\" & strMOMXMSI
        Case "MOM_DB"
            strCommand = strCommand & "\" & strMOMXMSI
        Case "UI"
            strCommand = strCommand & "\" & strMOMXMSI
        Case "AGENT"
            strCommand = strCommand & "\" & strMOMXAgentMSI
        Case "REPORTING"
            strCommand = strCommand & "\" & strMOMXReportingMSI
    End Select
            
    'Install the Product
    Install strVersion, strCommand, strMSIEXECLog, strParms, bInteractive, strLog
            
End Sub