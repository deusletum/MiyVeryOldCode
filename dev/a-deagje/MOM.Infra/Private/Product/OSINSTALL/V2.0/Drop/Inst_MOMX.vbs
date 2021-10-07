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
'             4 - path not found or unavailable
'             5 - MSI install failure
'             6 - MSI install requires reboot
'             7 - Coverage binaries install failure
'             8 - SetupContext Error
'             9 - MSVCRTD.DLL copy error
'
' History   : 4/25/2003 - Dean Gjedde - Created
'             4/28/2003 - Dean Gjedde - made changes to script requested
'               in code review (see each section for details)
'             5/20/2003 - Dean Gjedde - Fixed bugs 2768, 2769, 2772
'             5/20/2003 - Dean Gjedde - Fixed DCRs 2747, 2748
'             5/21/2003 - Dean Gjedde - Fixed bugs 2764, 2765, 2766, 2767
'                   , and 2770
'             5/23/2003 - Dean Gjedde - Fixed bugs 2782, 2784, 2785
'             5/27/2003 - Dean Gjedde - Fixed bugs 2801, 2802
'             5/27/2003 - Dean Gjedde - Fixed bug 2809
'             6/05/2003 - Dean Gjedde - Fixed bug 2819
'             6/18/2003 - Dean Gjedde - Fixed bug 2921 and DCR 2926
'             6/18/2003 - Dean Gjedde - Fixed bug 2931
'             6/18/2003 - Dean Gjedde - Fixed DCR 2934
'             8/05/2003 - Dean Gjedde - Fixed bug 3063
'             8/14/2003 - Dean Gjedde - Fixed bug 31399, 3062
'             8/17/2003 - Dean Gjedde - Fixed bug 2764 - Added a call To
'               install the coverage binaries for MOMX Cover build
'             8/26/2003 - Dean Gjedde - Fixed DCR 3113 - changed MSI property SQL_SERVER
'               to MOM_DB_SERVER
'             8/27/2003 - Dean Gjedde - Fixed DCR 3116 - changed MSI
'               property CONSOLIDATOR to MANAGEMENT_SERVER and remove InstallType
'             9/04/2003 - Dean Gjedde - Fixed DCR 3128 - Added MSI properties
'               to reporting Install
'             10/1/2003 - Dean Gjedde - Added PIDKEY Property, this is being pushed by bug 30028
'             10/24/2003 - Dean Gjedde - Changed the way MOM_UI is installed DCR 3217
'               and added REQUIRE_AUTH_COMMN to db install DCR 3216.
'             10/24/2003 - Dean Gjedde - Added SetupContext stuff, DCR 2753
'             11/10/2003 - Dean Gjedde - Removed Cover build install, DCR 2764
'               Alse fixed BUG 3238 - MOMX msi GUID changed
'             11/14/2003 - Dean Gjedde - Added support for install MCF and MMPC, DCR 3224
'             11/25/2003 - Dean Gjedde - Put Coverage support back In
'             12/05/2003 - Dean Gjedde - Enable Watson Exception Handling, DCR 3279
'             12/05/2003 - Dean Gjedde - Set default MOMX Reporing install path to
'               %programfiles%\Microsoft System Center Suite Reporting”
'             02/24/2004 - Dean Gjedde - Add support for installing Webconsole
'                                        Added support for SQL Port
'                                        Added support for writing EnableErrorReporting to config xml
'                                        Now create Inst_MOMX.log in %SYSTEMDRIVE%\logs
'                                        DITS Pauses for errors other than 1
'             03/04/2004 - Dean Gjedde - Add support for writing Rosetta_server, and Reporting_DB (bug 3410)
'             03/18/2004 - Dean Gjedde - Add support for specifying the path to the MOM DB files (bug 3456)
'             03/30/2004 - John Heaton - Updated to support SKU
'             04/22/2004 - Dean Gjedde - Removed DITSPause
'             05/03/2004 - Dean Gjedde - Added coping msvcrtd.dll for Reporting Debug installs bug 44346
'             05/12/2004 - Dean Gjedde - Will not write DAS and ConfigGroup to config.xml when
'                                        installing Express bug 3633
'             05/12/2004 - Dean Gjedde - Fixed UCase and EVAL bugs
'             05/15/2004 - Dean Gjedde - Added MS_MPS MSI property to Express builds bug 3669
'             05/18/2004 - Dean Gjedde - Added support for passing in passwords for users
'             05/19/2004 - Dean Gjedde - Added quotes are the path string to support spaces in path
'             05/25/2004 - John Heaton - Removed PIDKEY unless user puts it in.
'             06/03/2004 - Dean Gjedde - Removed call to GetDefaultAccount when installing reporting bug 3707
'             06/28/2004 - Dean Gjedde - Added UI only install support for Express bug 3778
'             07/09/2004 - Dean Gjedde - Fixed bug with Express not installing webconsole bug 47658
'             07/12/2004 - Dean Gjedde - Changed agent msi path bug 3788
' ----------------------------------------------------------------------
Option Explicit
Const g_iShortDate = 2
Const g_iLongTime = 3
Const g_iShortTime = 4

Dim g_cQuote
Dim g_sCommandLineParms
g_cQuote = Chr(34)

Main()

' -------------------------------------------------------------------
' summary: the RunCMD runs a command and get there errorlevel after the
'       command is run
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
' summary: gets the default account for the domain the user belongs to
' -------------------------------------------------------------------
Sub GetDefaultAccount(ByRef strAccount, strLog)
    On Error Resume Next
    Const iCDSError = 3

    Dim strUser, strPassword, objCDS, intResults

    Set objCDS = WScript.CreateObject("CDSCom.Users")
    If Err.Number <> 0 Then
        WriteLogAndQuit "VAR_ABORT - Unable to create CDSCom object", strLog, iCDSError
    End If
    intResults = objCDS.GetDefaultCredentials(strAccount, "")
    If intResults <> 0 Then
        WriteLogAndQuit "VAR_ABORT - no default account for your domain in CDS", strLog, iCDSError
    End If

    Set objCDS = Nothing
End Sub

' -------------------------------------------------------------------
' summary: gets the password for the specific user account
' -------------------------------------------------------------------
Function GetAccountPassword(strAccount, strLog)
    On Error Resume Next
    Const iCDSError = 3

    Dim objCDS
    Dim strPassword

    Set objCDS = WScript.CreateObject("CDSCom.Users")
    If Err.Number <> 0 Then
        WriteLogAndQuit "VAR_ABORT - Unable to create CDSCom object", strLog, iCDSError
    End If
    strPassword = objCDS.GetPassword(strAccount)
    GetAccountPassword = strPassword

    Set objCDS = Nothing
End Function

' -------------------------------------------------------------------
' summary: gets the build number for the product in strProduct given
'          the build token
' -------------------------------------------------------------------
Function GetBuildNum(strProduct, strBuildToken, strLog)
    On Error Resume Next
    Const iCDSError = 3

    Dim objCDS
    Dim strBuild

    Set objCDS = WScript.CreateObject("CDSCom.Builds")
    If Err.Number <> 0 Then
        WriteLogAndQuit "VAR_ABORT - Unable to create CDSCom object", strLog, iCDSError
    End If
    strBuild = objCDS.GetLatestBuildByToken(strProduct, strBuildToken)
    If strBuild = "" Then
        WriteLogAndQuit "VAR_ABORT - There is no " & strBuildToken _
            & " build for product " & strProduct, strLog , iCDSError
    End If
    GetBuildNum = strBuild

    Set objCDS = Nothing
End Function

' -------------------------------------------------------------------
' summary: Checks for SQL Server Service
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
' summary: the WriteLogAndQuit function will write messages to a log file And
'    report errors to the user.
' -------------------------------------------------------------------
Sub WriteLogAndQuit(strMessage, strLog, intExitCode)
    On Error Resume Next
    Const ForAppending = 8

    Dim strLogFile, objEnv, objFSO, objStream, oShell
    Dim sWINDIR, sArch

    strLogFile = strLog
    Set objFSO = CreateObject("Scripting.FileSystemObject")
    Set oShell = CreateObject("WScript.Shell")
    sWINDIR = oShell.ExpandEnvironmentStrings("%WINDIR%")
    Set objStream = objFSO.OpenTextFile(strLogFile, ForAppending, True)
    If Err.Number <> 0 Then
        objStream.Close
        WScript.Echo("Inst_MOMX was unable to write a log")
        WScript.Echo(Err.Description)
        Exit Sub
    End If

    objStream.Write(vbCrlf & "*LOG_START*-Inst_MOMX")
    objStream.Write(vbCrlf & "Inst_MOMX.vbs command line parameter(s) are:")
    objStream.Write(vbCrlf & g_sCommandLineParms)
    If intExitCode <> 0 Then
        objStream.Write(vbCrlf & "[" &  FormatDateTime(Date, g_iShortDate) & " " & _
            FormatDateTime(Time, g_iShortTime)  & "] ERR Inst_MOMX.vbs " & strMessage)
    Else
        objStream.Write(vbCrlf & "[" &  FormatDateTime(Date, g_iShortDate) & " " & _
            FormatDateTime(Time, g_iShortTime)  & "] Inst_MOMX.vbs " & strMessage)
    End If
    objStream.Write(vbCrlf & "*LOG_DONE*")
    objStream.Close
    WScript.Echo(strMessage)
    WScript.Quit(intExitCode)

    Set objFSO = Nothing
    Set objStream = Nothing
End Sub

' -------------------------------------------------------------------
' summary: uninstalls an msi based install, checks for errors and the Quits the script
' -------------------------------------------------------------------
Sub Uninstall(strVersion, strGUID, strMSILog, bInteractive, strLog)
    Const iMSIError = 5
    Const iNoError = 0

    Dim strCommand
    Dim intError

    If bInteractive = True Then
        strCommand = "msiexec.exe /x " & strGUID & " /lv* " & strMSILog
    Else
        strCommand = "msiexec.exe /x " & strGUID & " /lv* " & strMSILog & " /Qn"
    End If

    intError = RunCMD(strCommand)
    If intError <> 0 Then
        WriteLogAndQuit "VAR_FAIL - Uninstall for " & strVersion _
            & " failed with error number " & intError, strLog, iMSIError
    Else
        WriteLogAndQuit "VAR_PASS - Uninstall for " & strVersion _
            & " passed", strLog, iNoError
    End If

End Sub

' -------------------------------------------------------------------
' summary: installs an msi based install, checks for errors and the Quits the script
' -------------------------------------------------------------------
Sub Install(strVersion, strPath, strMSILog, strParms, bInteractive, strLog)
    Const intReboot = 1641
    Const iMSIReboot = 6
    Const iMSIError =5
    Const iNoError = 0

    Dim strCommand
    Dim intError

    If bInteractive = True Then
        strCommand = "msiexec.exe /i " & strPath & " /lv* " & strMSILog & strParms
    Else
        strCommand = "msiexec.exe /i " & strPath & " /lv* " & strMSILog & strParms & " /Qn"
    End If

    intError = RunCMD(strCommand)
    If intError = intReboot Then
        WriteLogAndQuit "VAR_FAIL - Install for " & strVersion _
            & " passed but requires a reboot", strLog, iMSIReboot
    ElseIf intError <> 0 Then
        WriteLogAndQuit "VAR_FAIL - Install for " & strVersion _
            & " failed with error number " & intError, strLog, iMSIError
    Else
        WriteLogAndQuit "VAR_PASS - Install for " & strVersion _
            & " passed", strLog, iNoError
    End If

End Sub

' -------------------------------------------------------------------
' summary: displays usage
' -------------------------------------------------------------------
Sub Usage()
    WScript.Echo("Usage:")
    WScript.Echo("cscript.exe Inst_MOMX.vbs /BUILD: [/TYPE:] [/VERSION:] [/LANGUAGE:]" _
        & " [/PATH:] [/?] [/SERVER:] [/AMCONTROL] [/DBSERVER:] [/I]")
    WScript.Echo(vbTab & "[/NODEBUG] [/DAS_USER:] [/DBINSTANCE:] " _
        & "[/DESTPATH:] [/CONFIGNAME:] [/Uninstall:] [/PIDKEY:]")
    WScript.Echo(vbTab & "[/REPORTINGDB:] [/REPORTINGDBSIZE:] [/SQLSERVICENAME:] " _
        & "[/SQLAGENTSERVICENAME] [/MOMX_DBNAME] [/ACTION_USER] [/ACTION_USER]")
    WScript.Echo("/BUILD: -  Supports build number, LATEST, or BLESSED")
    WScript.Echo(vbTab & " /BUILD: is not required if /PATH: is used")
    WScript.Echo("/TYPE: -  Supports FRE, CHK, DEBUG, RETAIL, NONOPT, COVER")
    WSCript.Echo(vbTab & "RETAIL is default for /TYPE:")
    WScript.Echo("/SKU: -  Supports SELECT, EXPRESS, and EVAL")
    WSCript.Echo(vbTab & "SELECT is default for /SKU:")
    WScript.Echo("/VERSION: - Supports COMPLETE, MOM_SERVER, MOM_DB, UI, AGENT, REPORTING")
    WScript.Echo(vbTab &"COMPLETE is default for /VERSION:")
    WScript.Echo("/LANGUAGE: - Supports only EN builds, but will support other " _
        & "languages when they become available")
    WScript.Echo(vbTab & "EN is the default for /LANGUAGE:")
    WScript.Echo("/PATH: - Exact path for private build, use /PATH:DEFAULT for default path")
    WScript.Echo(vbTab & "DEFAULT is the default for /PATH:")
    WScript.Echo("/?: Displays usage for inst_momx")
    WScript.Echo("Additional Parameters:")
    WScript.Echo(vbTab & "/SERVER: - Name of MOMX Server if doing an agent install.")
    WScript.Echo(vbTab & "/AMCONTROL: - Scan level for the DCAM if doing an agent install")
    WScript.Echo(vbTab & vbTab & "Use FULL for full contact and NONE if behind a firewall " _
        & "or does not have contact with the DCAM")
    WScript.Echo(vbTab & "/DBSERVER: - Name of the Server where the MOM DB is located if " _
        & "doing a MOM Server, or Reporting install")
    WScript.Echo("Optional:  All parameters after these will be optional.")
    WScript.Echo(vbTab & "/I - Interactive mode")
    WScript.Echo(vbTab & "/DAS_USER: - Name of the user to use for DAS (domain\user)")
    WScript.Echo(vbTab & vbTab & "If not specified default user will be retrieved from CDS")
    WScript.Echo(vbTab & "/DAS_PW: - Password for DAS User")
    WScript.Echo(vbTab & vbTab & "If not specified password will be retrieved from CDS")
    WScript.Echo(vbTab & "/DBINSTANCE: - Name of the SQL Instance to use")
    WScript.Echo(vbTab & vbTab & "If not specified default instance will be used")
    WScript.Echo(vbTab & vbTab & "Note that you only have to provide the instance name not " _
        & "computername\instance name")
    WScript.Echo(vbTab & "/DBSIZE: - Size of the MOMX database in MB")
    WScript.Echo(vbTab & vbTab & "If not specified default size of 500 MB will be used")
    WScript.Echo(vbTab & "/DESTPATH: - Destination path where MOM is going to be installed")
    WScript.Echo(vbTab & vbTab & "If not specified default destination path is used")
    WScript.Echo(vbTab & "/CONFIGNAME: - Name of the MOM configuration group name")
    WScript.Echo(vbTab & vbTab & "If not specified "& g_cQuote & "COMPUTERNAME_SQLINSTANCE_ConfigGroup" _
        & g_cQuote & " will be used")
    WScript.Echo(vbTab & "/UNINSTALL - Uninstall the product specified in /Version")
    WScript.Echo(vbTab & vbTab & "Note: This is a complete uninstall")
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
    WScript.Echo(vbTab & "/ROSETTASERVER: - This is the name of the server that MOMX Reporting will be install on")
    WScript.Echo(vbTab & vbTab & "If not specified the default %computername% is used")
    WScript.Echo(vbTab & "/REPORTINGDBSERVER: - This is the name of the server " _
        & "that will have the Reporting DB installed on")
    WScript.Echo(vbTab & vbTab & "If not specified the default %computername% is used")
    WScript.Echo(vbTab & "/TASK_USER - Name of the user to use for Reporting install task (domain\user)")
    WScript.Echo(vbTab & vbTab & "If not specified default user will be retrieved from CDS")
    WScript.Echo(vbTab & "/TASK_PW: - Password for Task User")
    WScript.Echo(vbTab & vbTab & "If not specified password will be retrieved from CDS")
    WScript.Echo(vbTab & "/ACTION_USER - Name of the user to use to install MOMX (domain\user)")
    WScript.Echo(vbTab & vbTab & "If not specified default user will be retrieved from CDS")
    WScript.Echo(vbTab & "/ACTION_PW: - Password for Action User")
    WScript.Echo(vbTab & vbTab & "If not specified password will be retrieved from CDS")
    WScript.Echo(vbTab & "/REP_USER - Name of the user to use for Reporting service to use (domain\user)")
    WScript.Echo(vbTab & vbTab & "If not specified default user will be retrieved from CDS")
    WScript.Echo(vbTab & "/REP_PW: - Password for Reporting service User")
    WScript.Echo(vbTab & vbTab & "If not specified password will be retrieved from CDS")
    WScript.Echo(vbTab & "/PIDKEY - MOMX Product Installation Key, please enter PID without dashes '-'")
    WScript.Echo(vbTab & "/DISABLE_REQ_AUTH - Disables Mutual Authentication")
    WScript.Echo(vbTab & vbTab & "This is enabled by default")
    WScript.Echo(vbTab & "/INSTALLMCF - Install MCF along with other components, this " _
        & "can only be installed with Complete and MOM_SERVER")
    WScript.Echo(vbTab & vbTab & "This is not installed by default")
    WScript.Echo(vbTab & "/INSTALLMMPC - Install MMPC along with other components, " _
        & "this can only be installed with Complete and MOM_SERVER")
    WScript.Echo(vbTab & vbTab & "This will also install MCF")
    WScript.Echo(vbTab & vbTab & "This is not installed by default")
    WScript.Echo(vbTab & "/INSTALLWEBCONSOLE - Install MOMX Web Console, note this will " _
        & "only be install on MOM Server and Complete installs")
    WScript.Echo(vbTab & vbTab & "This is not installed by default")
    WScript.Echo(vbTab & "/DISABLEWATSON - This will disable Watson exception handling")
    WScript.Echo(vbTab & vbTab & "Watson Exception handling is enabled by default")
    WScript.Echo(vbTab & "/DATADIR: - This is the location of the MOM DB file directory")
    WScript.Echo(vbTab & vbTab & "the default DATA dir will be used if not specified")
    WScript.Echo(vbTab & "/LOGDIR: - This is the location of the MOM DB log file directory")
    WScript.Echo(vbTab & vbTab & "the default LOG dir will be used if not specified")
    WScript.Echo(vbTab & "/SQLPORT: - This is the port number of the SQL instance that MOMX will install on")
    WScript.Echo(vbTab & vbTab & "the default SQL Port will be used if not specified")
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
' summary: the main function gets everything started.
' -------------------------------------------------------------------
Sub Main()
    Const strDefaultPath = "\\smx.net\builds\momx"
    Const sMSVCRTD = "\\smx.net\tools\TOOLBOX\VCSP4_DBG\X86\MSVCRTD.DLL"
    Const strMOMXGUID = "{CDDF3EA1-A765-49D0-9E78-FFF09C005D78}"
    Const strMOMXAgentGUID = "{245E6079-CEE4-4535-BDD6-9755F3E8CE3F}"
    Const strMOMXReportingGUID = "{660C5339-DBA4-4973-8CF4-1A23B79370F4}"
    Const strMOMXMSI = "MOMServer.msi"
    Const strMOMXAgentMSI = "MOMAgent.msi"
    Const strMOMXReportingMSI = "MOMReporting.msi"
    Const IA64 = "IA64"
    Const X86 = "X86"
    Const strCDImage = "CDImage"
    Const iUserError = 1
    Const iSQLError = 2
    Const iPathNotFoundError = 4
    Const iCoverageError = 7
    Const iSetupContextError = 8
    Const strInstalled = "Installed"
    Const strUninstalled = "Uninstalled"
    Const strCOMPONENT_DB = "DB"
    Const strCOMPONENT_SERVER = "Server"
    Const strCOMPONENT_CONSOLE = "Console"
    Const strCOMPONENT_AGENT = "Agent"
    Const strCOMPONENT_REPORTING = "Reporting"
    Const strWatson = " ENABLE_ERROR_REPORTING=1 QUEUE_ERROR_REPORTS=0"
    Const sWebConsole = " MOMWebConsole=1"
    Const iWaitTime = 5000 'This is in milliseconds
    Const iRetries = 3
    Const iMSVCRTDError = 9

    Dim objShell, objEnv, aSplit, aArguments, aDASUser, strArg, strRepUserPassword
    Dim strBuild, strType, strSKU, strVersion, strLanguage, strPath, strServer, strPIDKEY, strMOMXReprotingInstallDir
    Dim strAMControl, strDBServer, strDASUser, strDASUserPassword, bInstallMCF, bInstallMMPC, sRosettaServer
    Dim strDBInstance, strDBSize, strDistPath ,strConfigName, bWebConsole, sReportingDBServer
    Dim strCommand, strComputerName, strReportingDB, strReportingDBSize, aRepUser, sSQLPort, sDATADIR, sLOGDIR
    Dim strSQLServiceName, strSQLAgentServiceName, strMOMXDBNAME, bPathDefault, bEnableWatson
    Dim bUseDist, bDBSize, bInteractive, bUninstall, strRepUser, strFlavor, bDisableAuthComm
    Dim bReportingDB, bReportingDBSize, bSQLServiceName, bSQLAgentServiceName, bMOMXDBName, sWINDIR
    Dim intError,strArch, strParms, objFSO, objStream, strMSIEXECLog, strSystemDrive, strLog
    Dim strMSIEXECUninstallLog, strMSIFile, sSplit, strTaskUser, aTaskUser, strTaskUserPassword
    Dim strTempDBInstance, strActionUser, aActionUser, strActionUserPassword, bActionUser, sArg
    Dim objMachine, objComponent, strProgramFiles, strMOMXDefaultInstallDir, strAddLocalValue, sCopyDLLCMD

    bInteractive = False
    bUninstall = False
    bDBSize = False
    bUseDist = False
    bReportingDB = False
    bReportingDBSize = False
    bSQLServiceName = False
    strSQLAgentServiceName = False
    bMOMXDBName = False
    bPathDefault = True
    bActionUser = False
    bDisableAuthComm = False
    bInstallMCF = False
    bInstallMMPC = False
    bWebConsole = False
    bEnableWatson = True

    Set objShell = WScript.CreateObject("WScript.Shell")
    Set objFSO = WScript.CreateObject("Scripting.FileSystemObject")
    strComputerName = objShell.ExpandEnvironmentStrings("%COMPUTERNAME%")
    strSystemDrive = objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%")
    sWINDIR = objShell.ExpandEnvironmentStrings("%WINDIR%")
    strProgramFiles = objShell.ExpandEnvironmentStrings("%ProgramFiles%")
    strMOMXDefaultInstallDir = strProgramFiles & "\Microsoft Operations Manager 2005"
    strMOMXReprotingInstallDir = strProgramFiles & "\Microsoft System Center Reporting"
    strArch = UCase(objShell.ExpandEnvironmentStrings("%PROCESSOR_ARCHITECTURE%"))
    strLog = strSystemDrive & "\logs\Inst_MOMX.Log"
    If Not objFSO.FolderExists(strSystemDrive & "\logs") Then
        objFSO.CreateFolder(strSystemDrive & "\logs")
    End If
    sCopyDLLCMD = "CMD /C copy " & sMSVCRTD & " " & sWINDIR & "\System32 /Y"

    g_sCommandLineParms = ""
    For Each sArg In WScript.Arguments
        g_sCommandLineParms = g_sCommandLineParms & " " & sArg
    Next

    'Check if inst_momx.vbs is being run under CScript.exe
    If Not((InStr(1, WScript.FullName, "CSCRIPT", vbTextCompare) <> 0)) Then
        WriteLogAndQuit "VAR_ABORT - This script must be run with CScript.exe", strLog, iUserError
    End If

    'If for correct number of arguments
    Set aArguments = WScript.Arguments
    If aArguments.Count < 1 Then
        Usage()
        WriteLogAndQuit "VAR_ABORT - You must specify at least one switch", strLog, iUserError
    End If

    'Create the SetupContext objects
    On Error Resume Next
    Set objMachine = WScript.CreateObject("MOM.Test.Common.CMachine")
    If Err.Number <> 0 Then
        g_sCommandLineParms = g_sCommandLineParms & vbCrlf & "Unable to create SetupContext COM object" _
            & "Inst_MOMX.vbs will not write to the Topology file, make sure that MOM.Test.Setup.SetupContext.dll" _
            & " is registered with regasm /codebase"
        WScript.Echo("Unable to create SetupContext COM object, Inst_MOMX.vbs will not write to the Topology file")
        Err.Clear
    End If
    If Not(objMachine.Product.Exists()) Then
        objMachine.Product.Add()
    End If
    On Error GoTo 0

    'Parse command line arguments
    For Each strArg In WScript.Arguments
        aSplit = Split(strArg, ":", 2)
        If Not UBound(aSplit) = 0 Then
            sSplit = aSplit(1)
        End If
        Select Case UCase(aSplit(0))
            Case "/?"
                Usage()
                WriteLogAndQuit "VAR_ABORT - usage requested", strLog, iUserError
            Case "/BUILD"
                strBuild = sSplit
            Case "/TYPE"
                strType = sSplit
            Case "/VERSION"
                sSplit = UCase(sSplit) 'Doing case-insensitive comparison
                If sSplit = "COMPLETE" Or sSplit = "MOM_SERVER" Or _
                    sSplit = "MOM_DB" Or sSplit = "UI" Or _
                    sSplit = "AGENT" Or sSplit = "REPORTING" Then
                    strVersion = sSplit
                Else
                    WriteLogAndQuit "VAR_ABORT - " & sSplit & " is not a supported /VERSION:", strLog, iUserError
                End If
            Case "/LANGUAGE"
                strLanguage = sSplit
            Case "/SKU"
                sSplit = UCase(sSplit) 'Doing case-insensitive comparison
                If sSplit = "SELECT" Or sSplit = "EXPRESS" Or sSplit = "EVAL" Then
                    strSKU = sSplit
                Else
                    WriteLogAndQuit "VAR_ABORT - /SKU: switch can only take values SELECT, " _
                        & "EXPRESS, and EVAL", strLog, iUserError
                End If
            Case "/PATH"
                strPath = sSplit
                'If /path: is not default check if it exists
                If strPath <> "DEFAULT" Then
                    If Not(objFSO.FolderExists(strPath)) Then
                        WriteLogAndQuit "VAR_ABORT - " & strPath _
                            & " does not exist or is not accessable", strLog, iPathNotFoundError
                    End If
                    bPathDefault = False
                End If
            Case "/SERVER"
                strServer = sSplit
            Case "/AMCONTROL"
                strAMControl = UCase(sSplit)
            Case "/DBSERVER"
                strDBServer = sSplit
            Case "/DAS_USER"
                strDASUser = sSplit
            Case "/DAS_PW"
                strDASUserPassword = sSplit
            Case "/DBINSTANCE"
                If Instr(sSplit,"\") Then
                    WriteLogAndQuit "VAR_ABORT - \ is not a valid char for switch /DBINSTANCE:", strLog, iUserError
                Else
                    strDBInstance = sSplit
                End If
            Case "/DBSIZE"
                strDBSize = sSplit
                bDBSize = True
            Case "/DESTPATH"
                strDistPath = sSplit
                bUseDist = True
            Case "/CONFIGNAME"
                strConfigName = sSplit
            Case "/I"
                bInteractive = True
            Case "/UNINSTALL"
                bUNINSTALL = True
            Case "/REPORTINGDB"
                strReportingDB = sSplit
                bReportingDB = True
            Case "/REPORTINGDBSIZE"
                strReportingDBSize = sSplit
                bReportingDBSize = True
            Case "/SQLSERVICENAME"
                strSQLServiceName = sSplit
                bSQLServiceName = True
            Case "/SQLAGENTSERVICENAME"
                strSQLAgentServiceName = sSplit
                strSQLAgentServiceName = True
            Case "/MOMX_DBNAME"
                strMOMXDBNAME = sSplit
                bMOMXDBName = True
            Case "/ROSETTASERVER"
                sRosettaServer = sSplit
            Case "/REPORTINGDBSERVER"
                sReportingDBServer = sSplit
            Case "/TASK_USER"
                strTaskUser = sSplit
            Case "/TASK_PW"
                strTaskUserPassword = sSplit
            Case "/ACTION_USER"
                strActionUser = sSplit
                bActionUser = True
            Case "/ACTION_PW"
                strActionUserPassword = sSplit
            Case "/REP_USER"
                strRepUser = sSplit
            Case "/REP_PW"
                strRepUserPassword = sSplit
            Case "/PIDKEY"
                strPIDKEY = sSplit
            Case "/DISABLE_REQ_AUTH"
                bDisableAuthComm = True
            Case "/INSTALLMCF"
                bInstallMCF = True
            Case "/INSTALLMMPC"
                bInstallMMPC = True
            Case "/INSTALLWEBCONSOLE"
                bWebConsole = True
            Case "/DISABLEWATSON"
                bEnableWatson = False
            Case "/SQLPORT"
                sSQLPort = sSplit
            Case "/DATADIR"
                sDATADIR = sSplit
            Case "/LOGDIR"
                sLOGDIR = sSplit
            Case Else
                WriteLogAndQuit "VAR_ABORT - " & strArg & " is not a supported argument", strLog, iUserError
                WScript.Quit(1)
        End Select
    Next

    'Check if /BUILD: switch is empty
    If (bPathDefault) Then
        If strBuild = "" Then
            Usage()
            WriteLogAndQuit "VAR_ABORT - /BUILD: switch is empty", strLog, iUserError
        End If
    End If

    'Check if /TYPE:, /VERSION:, /LANGUAGE:, and /PATH: switches are empty
    If strType = "" Then
        strType = "RETAIL"
    End If
    If strSKU = "" Then
        strSKU = "SELECT"
    End If
    If strVersion = "" Then
        strVersion = "COMPLETE"
    End If
    If strLanguage = "" Then
        strLanguage = "EN"
    End If
    If strPath = "" Then
        strPath = "DEFAULT"
        bPathDefault = True
    End If
    strType = UCase(strType)
    strSKU = UCase(strSKU)
    strVersion = UCase(strVersion)
    strLanguage = UCase(strLanguage)
    strPath = UCase(strPath)
    strBuild = UCase(strBuild)

    strMSIEXECLog = strSystemDrive & "\logs\" & strVersion & "_Install.log"
    strMSIEXECUninstallLog = strSystemDrive & "\logs\" & strVersion & "_Uninstall.log"

    'Check if Processor type is ia64
    If strArch = IA64 And strVersion <> "AGENT" Then
        WriteLogAndQuit "VAR_ABORT - /VERSION:" & strVersion & " can only be installed on x86" _
            & " systems, this is an IA64", strLog, iUserError
    End If
    If strArch <> IA64 Then
        strArch = X86
    End If

    'Check if SQL Server is installed
    If strVersion = "COMPLETE" Or strVersion = "MOM_DB" Or strVersion = "REPORTING" Then
        If Not(CheckForSQL) Then
            WriteLogAndQuit "VAR_ABORT - SQL Server must be installed before MOMX can be installed", strLog, iSQLError
        End If
    End If

    'If install MOM_SERVER check for empty /DBSERVER: switch
    If strVersion = "MOM_SERVER" And strDBServer = "" Then
        WriteLogAndQuit "VAR_ABORT - /DBSERVER: switch must be used with /VERSION:MOM_SERVER", strLog, iUserError
    End If

    'If install MOMXUI check for empty /DBSERVER: switch
    If strVersion = "UI" And strServer = "" Then
        WriteLogAndQuit "VAR_ABORT - /SERVER: switch must be used with /VERSION:UI", strLog, iUserError
    End If

    'If install REPORTING check for empty /DBSERVER: switch
    If strVersion = "REPORTING" And strDBServer = "" Then
        strDBServer = strComputerName
    End If

    'Check if Agent install has the required switches
    If strVersion = "AGENT" Then
        If strAMControl <> "FULL" And strAMControl <> "NONE" Then
            WriteLogAndQuit "VAR_ABORT - /AMCONTROL: switch must be either " _
                & "NONE or FULL with /VERSION:AGENT", strLog, iUserError
        End If
        If strServer = "" Then
            WriteLogAndQuit "VAR_ABORT - /SERVER: switch must be used with " _
                & "/VERSION:AGENT", strLog, iUserError
        End If
    End If

    'Check if DAS user is specified, if not get default account and then get the password for that account
    If strVersion <> "AGENT" Then
        If strVersion <> "REPORTING" Then
            If strDASUser = "" Then
                GetDefaultAccount strDASUser, strLog
            End If
            aDASUser = Split(strDASUser, "\", 2)
            If strDASUserPassword = "" Then
                strDASUserPassword = GetAccountPassword(strDASUser, strLog)
            End If
    
            'Check if Action user is specified, if not get it from CDS
            If Not (bActionUser) Then
                GetDefaultAccount strActionUser, strLog
            End If
            aActionUser = Split(strActionUser, "\", 2)
            If strActionUserPassword = "" Then
                strActionUserPassword = GetAccountPassword(strActionUser, strLog)
            End If
        Else
             'Check if Task user is specified, if not set get default account and then get the password for that account
            If strTaskUser = "" Then
                GetDefaultAccount strTaskUser, strLog
            End If
            aTaskUser = Split(strTaskUser, "\", 2)
            If strTaskUserPassword = "" Then
                strTaskUserPassword = GetAccountPassword(strTaskUser, strLog)
            End If
    
            'Check if Rep user is specified, if not set get default acount and the get the password for that account
            If strRepUser = "" Then
                GetDefaultAccount strRepUser, strLog
            End If
            aRepUser = Split(strRepUser, "\", 2)
            If strRepUserPassword = "" Then
                strRepUserPassword = GetAccountPassword(strRepUser, strLog)
            End If
        End If
    End If

    ' Check if DBInstance is empty
    If strDBInstance = "" Then
        strDBInstance = strComputerName
    Else
        If strDBInstance <> strComputerName Then
            strTempDBInstance = strDBInstance
            strDBInstance = strComputerName & "\" & strDBInstance
        End If
    End If

    'check if Config name is empty
    If strConfigName = "" Then
        If strDBInstance <> strComputerName Then
            strConfigName = strComputerName & "_" & strTempDBInstance & "_ConfigGroup"
        Else
            strConfigName = strComputerName & "_DefaultInstance_ConfigGroup"
        End If
    End If

    If (strVersion = "REPORTING") Or (strVersion = "MOM_SERVER") Then
        If strTempDBInstance <> "" Then
            If strDBServer <> strTempDBInstance Then
                strDBServer = strDBServer & "\" & strTempDBInstance
            End If
        End If
    End If

    If strVersion = "REPORTING" Then
        If sRosettaServer = "" Then
            sRosettaServer = strComputerName
        End If
        If sReportingDBServer = "" Then
            sReportingDBServer = strDBServer
        End If
    End If

    If strType = "FRE" Then
        strType = "RETAIL"
    Elseif strType = "CHK" Then
        strType = "DEBUG"
    End If

    Select Case strType
        Case "RETAIL"
            strFlavor = "FRE"
        Case "DEBUG"
            strFlavor = "CHK"
        Case "NONOPT"
            strFlavor = "FRE"
        Case "COVER"
            strFlavor = "FRE"
        Case Else
            WriteLogAndQuit "VAR_ABORT - /TYPE: switch can only take values RETAIL, " _
                & "DEBUG, FRE, CHK, NONOPT, COVER", strLog, iUserError
    End Select

    If (strVersion = "REPORTING") And (strType = "DEBUG") Then
        RunCMD sCopyDLLCMD
        Dim i
        For i = 1 To iRetries
            If Not (objFSO.FileExists(sWINDIR & "\System32\MSVCRTD.DLL")) Then
                RunCMD sCopyDLLCMD
                WScript.Sleep(iWaitTime)
            Else
                Exit For
            End If
        Next
        If Not (objFSO.FileExists(sWINDIR & "\System32\MSVCRTD.DLL")) Then
            WriteLogAndQuit "VAR_ABORT - Unable to copy MSVCRTD.DLL" _
                & " for MOMX Reporting Debug", strLog, iMSVCRTDError
        End If
    End If

    If (strVersion = "REPORTING") And (strSKU = "EXPRESS") Then
        WriteLogAndQuit "VAR_ABORT - /SKU:EXPRESS switch can not be used when installing Reporting", _
            strLog, iUserError
    End If

    If (strSKU = "EXPRESS") Then
        If (strVersion = "REPORTING") Or (strVersion = "MOM_SERVER") Or (strVersion = "MOM_DB") Then
            WriteLogAndQuit "VAR_ABORT - EXPRESS is only supported for Complete and Agent installs", _
            strLog, iUserError
        End If
    End If
    'Build default path
    Select Case strBuild
        Case "LATEST"
            strBuild = GetBuildNum("MOMX", "LATEST", strLog)
        Case "BLESSED"
            strBuild = GetBuildNum("MOMX", "BLESSED", strLog)
    End Select
    strCommand = strDefaultPath & "\" & strLanguage & "\" & strBuild & "\" & strType _
        & "\" & strArch & "\" & strSKU & strCDImage
    If strVersion = "AGENT" Then
        If strArch = "IA64" Then
            strCommand = strCommand & "\" & IA64
        Else
            strCommand = strCommand & "\i386"
        End If
    End If

    'Uninstall If bUninstall is true
    On Error Resume Next
    If (bUninstall) Then
        Select Case strVersion
            Case "COMPLETE"
                If Not(objMachine.Product.Components.Exists(strCOMPONENT_DB)) Then
                    objMachine.Product.Components.Add(strCOMPONENT_DB)
                End If
                If Not(objMachine.Product.Components.Exists(strCOMPONENT_SERVER)) Then
                    objMachine.Product.Components.Add(strCOMPONENT_SERVER)
                End If
                If Not(objMachine.Product.Components.Exists(strCOMPONENT_CONSOLE)) Then
                    objMachine.Product.Components.Add(strCOMPONENT_CONSOLE)
                End If
                For Each objComponent In objMachine.Product.Components
                    If objComponent.Name = strCOMPONENT_DB Then
                        objComponent.InstallState = strUninstalled
                        objComponent.Build = strBuild
                        objComponent.Language = strLanguage
                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                            & " " & FormatDateTime(Time, g_iLongTime)
                    End If
                    If objComponent.Name = strCOMPONENT_SERVER Then
                        objComponent.InstallState = strUninstalled
                        objComponent.Build = strBuild
                        objComponent.Language = strLanguage
                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                            & " " & FormatDateTime(Time, g_iLongTime)
                    End If
                    If objComponent.Name = strCOMPONENT_CONSOLE Then
                        objComponent.InstallState = strUninstalled
                        objComponent.Build = strBuild
                        objComponent.Language = strLanguage
                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                            & " " & FormatDateTime(Time, g_iLongTime)
                    End If
                Next
                objMachine.CommitLocal
                Uninstall strVersion, strMOMXGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "MOM_SERVER"
                If Not(objMachine.Product.Components.Exists(strCOMPONENT_SERVER)) Then
                    objMachine.Product.Components.Add(strCOMPONENT_SERVER)
                End If
                For Each objComponent In objMachine.Product.Components
                    If objComponent.Name = strCOMPONENT_SERVER Then
                        objComponent.InstallState = strUninstalled
                        objComponent.Build = strBuild
                        objComponent.Language = strLanguage
                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                            & " " & FormatDateTime(Time, g_iLongTime)
                    End If
                Next
                objMachine.CommitLocal
                Uninstall strVersion, strMOMXGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "MOM_DB"
                If Not(objMachine.Product.Components.Exists(strCOMPONENT_DB)) Then
                    objMachine.Product.Components.Add(strCOMPONENT_DB)
                End If
                For Each objComponent In objMachine.Product.Components
                    If objComponent.Name = strCOMPONENT_DB Then
                        objComponent.InstallState = strUninstalled
                        objComponent.Build = strBuild
                        objComponent.Language = strLanguage
                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                            & " " & FormatDateTime(Time, g_iLongTime)
                    End If
                Next
                objMachine.CommitLocal
                Uninstall strVersion, strMOMXGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "UI"
                If Not(objMachine.Product.Components.Exists(strCOMPONENT_CONSOLE)) Then
                    objMachine.Product.Components.Add(strCOMPONENT_CONSOLE)
                End If
                For Each objComponent In objMachine.Product.Components
                    If objComponent.Name = strCOMPONENT_CONSOLE Then
                        objComponent.InstallState = strUninstalled
                        objComponent.Build = strBuild
                        objComponent.Language = strLanguage
                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                            & " " & FormatDateTime(Time, g_iLongTime)
                    End If
                Next
                objMachine.CommitLocal
                Uninstall strVersion, strMOMXGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "AGENT"
                If Not(objMachine.Product.Components.Exists(strCOMPONENT_AGENT)) Then
                    objMachine.Product.Components.Add(strCOMPONENT_AGENT)
                End If
                For Each objComponent In objMachine.Product.Components
                    If objComponent.Name = strCOMPONENT_AGENT Then
                        objComponent.InstallState = strUninstalled
                        objComponent.Build = strBuild
                        objComponent.Language = strLanguage
                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                            & " " & FormatDateTime(Time, g_iLongTime)
                    End If
                Next
                objMachine.CommitLocal
                Uninstall strVersion, strMOMXAgentGUID, strMSIEXECUninstallLog, bInteractive, strLog
            Case "REPORTING"
                If Not(objMachine.Product.Components.Exists(strCOMPONENT_REPORTING)) Then
                    objMachine.Product.Components.Add(strCOMPONENT_REPORTING)
                End If
                For Each objComponent In objMachine.Product.Components
                    If objComponent.Name = strCOMPONENT_REPORTING Then
                        objComponent.InstallState = strUninstalled
                        objComponent.Build = strBuild
                        objComponent.Language = strLanguage
                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                            & " " & FormatDateTime(Time, g_iLongTime)
                    End If
                Next
                objMachine.CommitLocal
                Uninstall strVersion, strMOMXReportingGUID, strMSIEXECUninstallLog, bInteractive, strLog
        End Select
    End If

    Dim sDASParm, sActionParm, sPIDParm, sConfigParm, strMS_MPS
    sDASParm = " DAS_ACCOUNT=" & g_cQuote & aDASUser(1) & g_cQuote & " DAS_PASSWORD=" & g_cQuote _
        & strDASUserPassword & g_cQuote & " DAS_DOMAIN=" & g_cQuote & aDASUser(0) & g_cQuote
    sActionParm = " ACTIONSUSER=" & g_cQuote & aActionUser(1) & g_cQuote & " ACTIONSPASSWORD=" _
        & g_cQuote & strActionUserPassword & g_cQuote & " ACTIONSDOMAIN=" & g_cQuote & aActionUser(0) _
        & g_cQuote
    If Not strPIDKEY = "" Then
        sPIDParm = " PIDKEY=" & g_cQuote & strPIDKEY & g_cQuote
    End If
    sConfigParm = " CONFIG_GROUP=" & g_cQuote & strConfigName & g_cQuote
    strMS_MPS = " MS_MPS=" & g_cQuote & strCommand & "\ManagementPacks\Setup" & g_cQuote
    'Build Parameters
    ' Ok there is lots of repented code below this is to work with SetupContext
    'poor design (that is not going to change
    Select Case strVersion
        Case "COMPLETE"
            Dim sCompleteParms
            sCompleteParms = "MOMXDB,MOMXServer,MOMXUI"
            'Check if MCF or MMPC or both needs to be installed
            If (bInstallMMPC) Then
                sCompleteParms = sCompleteParms & ",MMPC"
            End If
            If (bInstallMCF) Then
                sCompleteParms = sCompleteParms & ",MCF"
            End If
            If (bWebConsole) Then
                sCompleteParms = sCompleteParms & ",MOMWebConsole"
            End If
            If strSKU = "EXPRESS" Then
                sCompleteParms = "MOMXDB,MOMXServer,MOMXUI,MOMWebConsole"
            End If
            strParms = " ADDLOCAL=" & g_cQuote & sCompleteParms & g_cQuote
            strParms = strParms & sActionParm & sPIDParm
            strParms = strParms & " SQLSVR_INSTANCE=" & g_cQuote & strDBInstance & g_cQuote
            If strSKU <> "EXPRESS" Then
                strParms = strParms & sDASParm & sConfigParm
            Else
                strParms = strParms & strMS_MPS
            End If
            If (bDisableAuthComm) Then
                strParms = strParms & " REQUIRE_AUTH_COMMN=0"
            End If
            If (bEnableWatson) Then
                strParms = strParms & strWatson
            End If
            If (sSQLPort <> "") Then
                strParms = strParms & " SQL_PORT=" & sSQLPort
            End If
            If (sDATADIR <> "") Then
                strParms = strParms & " DATA_DIR=" & g_cQuote & sDATADIR & g_cQuote
            End If
            If (sLOGDIR <> "") Then
                strParms = strParms & " LOG_DIR=" & g_cQuote & sLOGDIR & g_cQuote
            End If
            'SetupContext stuff
            If Not(objMachine.Product.Components.Exists(strCOMPONENT_DB)) Then
                objMachine.Product.Components.Add(strCOMPONENT_DB)
            End If
            If Not(objMachine.Product.Components.Exists(strCOMPONENT_SERVER)) Then
                objMachine.Product.Components.Add(strCOMPONENT_SERVER)
            End If
            If Not(objMachine.Product.Components.Exists(strCOMPONENT_CONSOLE)) Then
                objMachine.Product.Components.Add(strCOMPONENT_CONSOLE)
            End If
            For Each objComponent In objMachine.Product.Components
                If objComponent.Name = strCOMPONENT_DB Then
                    objComponent.InstallState = strInstalled
                    objComponent.Build = strBuild
                    objComponent.Flavor = strType
                    objComponent.Language = strLanguage
                    objComponent.ActionsDomain = aActionUser(0)
                    objComponent.ActionsUser = aActionUser(1)
                    objComponent.ActionsPassword = strActionUserPassword
                    If strSKU <> "EXPRESS" Then
                        objComponent.DasAccount = aDASUser(1)
                        objComponent.DasDomain = aDASUser(0)
                        objComponent.DasPassword = strDASUserPassword
                        objComponent.ConfigGrpName = strConfigName
                    End If
                    If Not strPIDKEY = "" Then
                        objComponent.PIDKEY = strPIDKEY
                    End If
                    objComponent.InstallState = strInstalled
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.SKU = strSKU
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.MSIExecFile = strMOMXMSI
                    objComponent.SqlServerInstance = strDBInstance
                    If (bDisableAuthComm) Then
                        objComponent.RequireAuthCommn = "false"
                    Else
                        objComponent.RequireAuthCommn = "true"
                    End If
                    If (bEnableWatson) Then
                        objComponent.EnableErrorReporting = "true"
                        objComponent.QueueErrorReports = "false"
                    End If
                    If(bPathDefault) Then
                        objComponent.SourceDir = strCommand
                    Else
                        objComponent.SourceDir = strPath
                    End If
                    If (bUseDist) Then
                        strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
                        objComponent.Location = strDistPath
                    Else
                        objComponent.Location = strMOMXDefaultInstallDir
                    End If
                    If (bDBSize) Then
                        strParms = strParms & " DB_SIZE=" & g_cQuote & strDBSize & g_cQuote
                        objComponent.DatabaseSize = strDBSize
                    End If
                End If
                If objComponent.Name = strCOMPONENT_SERVER Then
                    objComponent.InstallState = strInstalled
                    objComponent.Build = strBuild
                    objComponent.Flavor = strType
                    objComponent.Language = strLanguage
                    objComponent.ActionsDomain = aActionUser(0)
                    objComponent.ActionsUser = aActionUser(1)
                    objComponent.ActionsPassword = strActionUserPassword
                    If strSKU <> "EXPRESS" Then
                        objComponent.DasAccount = aDASUser(1)
                        objComponent.DasDomain = aDASUser(0)
                        objComponent.DasPassword = strDASUserPassword
                        objComponent.ConfigGrpName = strConfigName
                    End If
                    If Not strPIDKEY = "" Then
                        objComponent.PIDKEY = strPIDKEY
                    End If
                    objComponent.InstallState = strInstalled
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.SKU = strSKU
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.MSIExecFile = strMOMXMSI
                    objComponent.SqlServerInstance = strDBInstance
                    If (bEnableWatson) Then
                        objComponent.EnableErrorReporting = "true"
                        objComponent.QueueErrorReports = "false"
                    End If
                    If sSQLPort <> "" Then
                        objComponent.SQLPort = sSQLPort
                    End If
                    If(bPathDefault) Then
                        objComponent.SourceDir = strCommand
                    Else
                        objComponent.SourceDir = strPath
                    End If
                    If (bWebConsole) Then
                        objComponent.WebConsole = "true"
                    End If
                    If (bInstallMCF) Then
                        objComponent.MCF = "true"
                    End If
                    If (bInstallMMPC) Then
                        objComponent.MMPC = "true"
                    End If
                    If (bUseDist) Then
                        objComponent.Location = strDistPath
                    Else
                        objComponent.Location = strMOMXDefaultInstallDir
                    End If
                    If (bDBSize) Then
                        strParms = strParms & " DB_SIZE=" & g_cQuote & strDBSize & g_cQuote
                        objComponent.DatabaseSize = strDBSize
                    End If
                End If
                If objComponent.Name = strCOMPONENT_CONSOLE Then
                    objComponent.InstallState = strInstalled
                    objComponent.Build = strBuild
                    objComponent.Flavor = strType
                    objComponent.Language = strLanguage
                    objComponent.ActionsDomain = aActionUser(0)
                    objComponent.ActionsUser = aActionUser(1)
                    objComponent.ActionsPassword = strActionUserPassword
                    If strSKU <> "EXPRESS" Then
                        objComponent.DasAccount = aDASUser(1)
                        objComponent.DasDomain = aDASUser(0)
                        objComponent.DasPassword = strDASUserPassword
                        objComponent.ConfigGrpName = strConfigName
                    End If
                    If Not strPIDKEY = "" Then
                        objComponent.PIDKEY = strPIDKEY
                    End If
                    objComponent.InstallState = strInstalled
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.SKU = strSKU
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.MSIExecFile = strMOMXMSI
                    objComponent.SqlServerInstance = strDBInstance
                    If (bEnableWatson) Then
                        objComponent.EnableErrorReporting = "true"
                        objComponent.QueueErrorReports = "false"
                    End If
                    If(bPathDefault) Then
                        objComponent.SourceDir = strCommand
                    Else
                        objComponent.SourceDir = strPath
                    End If
                    If (bUseDist) Then
                        objComponent.Location = strDistPath
                    Else
                        objComponent.Location = strMOMXDefaultInstallDir
                    End If
                    If (bDBSize) Then
                        strParms = strParms & " DB_SIZE=" & g_cQuote & strDBSize & g_cQuote
                        objComponent.DatabaseSize = strDBSize
                    End If
                End If
            Next
        Case "MOM_SERVER"
            Dim sServerParms
            sServerParms = "MOMXServer"
            'Check if MCF or MMPC or both needs to be installed
            If (bInstallMMPC) Then
                sServerParms = sServerParms & ",MMPC"
            End If
            If (bInstallMCF) Then
                sServerParms = sServerParms & ",MCF"
            End If
            If (bWebConsole) Then
                sServerParms = sServerParms & ",MOMWebConsole"
            End If
            strParms = " ADDLOCAL=" & g_cQuote & sServerParms & g_cQuote
            strParms = strParms & " MOM_DB_SERVER=" & g_cQuote & strDBServer & g_cQuote
            strParms = strParms & sActionParm & sPIDParm
            strParms = strParms & sDASParm & sConfigParm
            If (sSQLPort <> "" ) Then
                strParms = strParms & " SQL_PORT=" & sSQLPort
            End If
            If (bEnableWatson) Then
                strParms = strParms & strWatson
            End If
            If Not(objMachine.Product.Components.Exists(strCOMPONENT_SERVER)) Then
                objMachine.Product.Components.Add(strCOMPONENT_SERVER)
            End If
            For Each objComponent In objMachine.Product.Components
                If objComponent.Name = strCOMPONENT_SERVER Then
                    objComponent.InstallState = strInstalled
                    objComponent.Build = strBuild
                    objComponent.Flavor = strType
                    objComponent.Language = strLanguage
                    objComponent.ActionsDomain = aActionUser(0)
                    objComponent.ActionsUser = aActionUser(1)
                    objComponent.ActionsPassword = strActionUserPassword
                    objComponent.DasAccount = aDASUser(1)
                    objComponent.DasDomain = aDASUser(0)
                    objComponent.DasPassword = strDASUserPassword
                    objComponent.ConfigGrpName = strConfigName
                    If Not strPIDKEY = "" Then
                        objComponent.PIDKEY = strPIDKEY
                    End If
                    objComponent.InstallState = strInstalled
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.SKU = strSKU
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.MSIExecFile = strMOMXMSI
                    objComponent.Server = strDBServer
                    If (bEnableWatson) Then
                        objComponent.EnableErrorReporting = "true"
                        objComponent.QueueErrorReports = "false"
                    End If
                    If sSQLPort <> "" Then
                        objComponent.SQLPort = sSQLPort
                    End If
                    If(bPathDefault) Then
                        objComponent.SourceDir = strCommand
                    Else
                        objComponent.SourceDir = strPath
                    End If
                    If (bWebConsole) Then
                        objComponent.WebConsole = "true"
                    End If
                    If (bInstallMCF) Then
                        objComponent.MCF = "true"
                    End If
                    If (bInstallMMPC) Then
                        objComponent.MMPC = "true"
                    End If
                    If (bUseDist) Then
                        strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
                        objComponent.Location = strDistPath
                    Else
                        objComponent.Location = strMOMXDefaultInstallDir
                    End If
                End If
            Next
        Case "MOM_DB"
            strParms = " ADDLOCAL=" & g_cQuote & "MOMXDB" & g_cQuote
            strParms = strParms & sActionParm & sPIDParm
            strParms = strParms & " SQLSVR_INSTANCE=" & g_cQuote & strDBInstance & g_cQuote
            strParms = strParms & sDASParm & sConfigParm
            If (bDisableAuthComm) Then
                strParms = strParms & " REQUIRE_AUTH_COMMN=0"
            End If
            If (bEnableWatson) Then
                strParms = strParms & strWatson
            End If
            If (sDATADIR <> "") Then
                strParms = strParms & " DATA_DIR=" & g_cQuote & sDATADIR & g_cQuote
            End If
            If (sLOGDIR <> "") Then
                strParms = strParms & " LOG_DIR=" & g_cQuote & sLOGDIR & g_cQuote
            End If
            If Not(objMachine.Product.Components.Exists(strCOMPONENT_DB)) Then
                objMachine.Product.Components.Add(strCOMPONENT_DB)
            End If
            For Each objComponent In objMachine.Product.Components
                If objComponent.Name = strCOMPONENT_DB Then
                    objComponent.InstallState = strInstalled
                    objComponent.Build = strBuild
                    objComponent.Flavor = strType
                    objComponent.Language = strLanguage
                    objComponent.ActionsDomain = aActionUser(0)
                    objComponent.ActionsUser = aActionUser(1)
                    objComponent.ActionsPassword = strActionUserPassword
                    objComponent.DasAccount = aDASUser(1)
                    objComponent.DasDomain = aDASUser(0)
                    objComponent.DasPassword = strDASUserPassword
                    objComponent.ConfigGrpName = strConfigName
                    If Not strPIDKEY = "" Then
                        objComponent.PIDKEY = strPIDKEY
                    End If
                    objComponent.InstallState = strInstalled
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.SKU = strSKU
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.MSIExecFile = strMOMXMSI
                    objComponent.SqlServerInstance = strDBInstance
                    If (bDisableAuthComm) Then
                        objComponent.RequireAuthCommn = "false"
                    Else
                        objComponent.RequireAuthCommn = "true"
                    End If
                    If (bEnableWatson) Then
                        objComponent.EnableErrorReporting = "true"
                        objComponent.QueueErrorReports = "false"
                    End If
                    If(bPathDefault) Then
                        objComponent.SourceDir = strCommand
                    Else
                        objComponent.SourceDir = strPath
                    End If
                    If (bUseDist) Then
                        strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
                        objComponent.Location = strDistPath
                    Else
                        objComponent.Location = strMOMXDefaultInstallDir
                    End If
                    If (bDBSize) Then
                        strParms = strParms & " DB_SIZE=" & g_cQuote & strDBSize & g_cQuote
                        objComponent.DatabaseSize = strDBSize
                    End If
                End If
            Next
        Case "UI"
            strParms = " ADDLOCAL=" & g_cQuote & "MOMXUI" & g_cQuote
            strParms = strParms & " MOM_SERVER=" & g_cQuote & strServer & g_cQuote
            strParms = strParms & sPIDParm
            strParms = strParms & sDASParm
            If (bEnableWatson) Then
                strParms = strParms & strWatson
            End If
            If Not(objMachine.Product.Components.Exists(strCOMPONENT_CONSOLE)) Then
                objMachine.Product.Components.Add(strCOMPONENT_CONSOLE)
            End If
            For Each objComponent In objMachine.Product.Components
                If objComponent.Name = strCOMPONENT_CONSOLE Then
                    objComponent.InstallState = strInstalled
                    objComponent.Build = strBuild
                    objComponent.Flavor = strType
                    objComponent.Language = strLanguage
                    objComponent.ActionsDomain = aActionUser(0)
                    objComponent.ActionsUser = aActionUser(1)
                    objComponent.ActionsPassword = strActionUserPassword
                    objComponent.DasAccount = aDASUser(1)
                    objComponent.DasDomain = aDASUser(0)
                    objComponent.DasPassword = strDASUserPassword
                    objComponent.ConfigGrpName = strConfigName
                    If Not strPIDKEY = "" Then
                        objComponent.PIDKEY = strPIDKEY
                    End If
                    objComponent.InstallState = strInstalled
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.SKU = strSKU
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.MSIExecFile = strMOMXMSI
                    objComponent.Server = strDBServer
                    If (bEnableWatson) Then
                        objComponent.EnableErrorReporting = "true"
                        objComponent.QueueErrorReports = "false"
                    End If
                    If(bPathDefault) Then
                        objComponent.SourceDir = strCommand
                    Else
                        objComponent.SourceDir = strPath
                    End If
                    If (bUseDist) Then
                        strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
                        objComponent.Location = strDistPath
                    Else
                        objComponent.Location = strMOMXDefaultInstallDir
                    End If
                End If
            Next
        Case "AGENT"
            strParms = " MANAGEMENT_SERVER=" & g_cQuote & strServer & g_cQuote & " CONFIG_GROUP=" & g_cQuote _
                    & strConfigName & g_cQuote & " AM_CONTROL=" & g_cQuote & strAMControl & g_cQuote
            If (bEnableWatson) Then
                strParms = strParms & strWatson
            End If
            If Not(objMachine.Product.Components.Exists(strCOMPONENT_AGENT)) Then
                objMachine.Product.Components.Add(strCOMPONENT_AGENT)
            End If
            For Each objComponent In objMachine.Product.Components
                If objComponent.Name = strCOMPONENT_AGENT Then
                    objComponent.InstallState = strInstalled
                    objComponent.Build = strBuild
                    objComponent.Flavor = strType
                    objComponent.Language = strLanguage
                    objComponent.ConfigGrpName = strConfigName
                    objComponent.InstallState = strInstalled
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.SKU = strSKU
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.MSIExecFile = strMOMXAgentMSI
                    objComponent.ControlLevel = strAMControl
                    If (bEnableWatson) Then
                        objComponent.EnableErrorReporting = "true"
                        objComponent.QueueErrorReports = "false"
                    End If
                    If(bPathDefault) Then
                        objComponent.SourceDir = strCommand
                    Else
                        objComponent.SourceDir = strPath
                    End If
                    If (bUseDist) Then
                        strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
                        objComponent.Location = strDistPath
                    Else
                        objComponent.Location = strMOMXDefaultInstallDir
                    End If
                    If (bActionUser) Then
                        strParms = strParms & " ACTIONSUSER=" & g_cQuote & aActionUser(1) _
                            & g_cQuote & " ACTIONSPASSWORD=" & g_cQuote & strActionUserPassword _
                            & g_cQuote & " ACTIONSDOMAIN=" & g_cQuote & aActionUser(0) _
                            & g_cQuote
                        objComponent.ActionsDomain = aActionUser(0)
                        objComponent.ActionsUser = aActionUser(1)
                        objComponent.ActionsPassword = strActionUserPassword
                    End If
                End If
            Next
        Case "REPORTING"
            strParms = " MOM_DB_SERVER=" & g_cQuote & strDBServer & g_cQuote & " SQLSVR_INSTANCE=" & g_cQuote _
                    & sReportingDBServer & g_cQuote & " TASK_USER_ACCOUNT=" & g_cQuote & aTaskUser(1) & g_cQuote _
                    & " TASK_USER_DOMAIN=" & g_cQuote & aTaskUser(0) & g_cQuote & " TASK_USER_PASSWORD=" _
                    & g_cQuote & strTaskUserPassword & g_cQuote & " REPORTING_USER=" & g_cQuote & aRepUser(1) _
                    & g_cQuote & " REPORTING_DOMAIN=" & g_cQuote & aRepUser(0) & g_cQuote & " REPORTING_PASSWORD=" _
                    & g_cQuote & strRepUserPassword & g_cQuote & " ROSETTA_SERVER=" & g_cQuote & sRosettaServer _
                    & g_cQuote
            If Not(objMachine.Product.Components.Exists(strCOMPONENT_REPORTING)) Then
                objMachine.Product.Components.Add(strCOMPONENT_REPORTING)
            End If
            For Each objComponent In objMachine.Product.Components
                If objComponent.Name = strCOMPONENT_REPORTING Then
                    objComponent.Build = strBuild
                    objComponent.Flavor = strType
                    objComponent.Language = strLanguage
                    objComponent.ConfigGrpName = strConfigName
                    objComponent.InstallState = strInstalled
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.SKU = strSKU
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.MSIExecFile = strMOMXReportingMSI
                    objComponent.RosettaMachineName = sRosettaServer
                    objComponent.ReportingSqlServerInstance = sReportingDBServer
                    If(bPathDefault) Then
                        objComponent.SourceDir = strCommand
                    Else
                        objComponent.SourceDir = strPath
                    End If
                    objComponent.Server = strDBServer
                    objComponent.SqlServerInstance = strDBServer
                    objComponent.TaskUserAccount = aTaskUser(1)
                    objComponent.TaskUserDomain = aTaskUser(0)
                    objComponent.RepUserAccount = aRepUser(1)
                    objComponent.RepUserDomain = aRepUser(0)
                    objComponent.RepUserPassword = strRepUserPassword
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
                    If (bUseDist) Then
                        strParms = strParms & " INSTALLDIR=" & g_cQuote & strDistPath & g_cQuote
                        objComponent.Location = strDistPath
                    Else
                        objComponent.Location = strMOMXReprotingInstallDir
                    End If
                End If
            Next
    End Select
    objMachine.CommitLocal
    On Error Goto 0

    Select Case strVersion
        Case "AGENT"
            strMSIFile = strMOMXAgentMSI
        Case "REPORTING"
            strMSIFile = strMOMXReportingMSI
        Case Else
            strMSIFile = strMOMXMSI
    End Select

    'Install from path specified by the user
    If Not(bPathDefault)Then
        Install strVersion, g_cQuote & strPath & "\" & strMSIFile & g_cQuote, strMSIEXECLog, _
            strParms, bInteractive, strLog
    End If

    'Check if path is correct
    If Not(objFSO.FolderExists(strCommand)) Then
        WriteLogAndQuit "VAR_ABORT - " & strCommand & " does not exist or " _
            & "is not accessible", strLog, iPathNotFoundError
    End If
    strCommand = strCommand & "\" & strMSIFile

    'Install the Product
    Install strVersion, strCommand, strMSIEXECLog, strParms, bInteractive, strLog
End Sub