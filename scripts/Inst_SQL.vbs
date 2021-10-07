' ======================================================================
' Copyright . 2003
'
' Module : Inst_SQL.vbs
'
' Summary: This script will install SQL Server 2000 and MSDE siliently
'
' ErrorCodes: 1 - bad user input
'             2 - Write File error
'             3 - Unable to create CDScom object
'             4 - Reg write error
'             5 - SQL install failure
'             6 - SQL install pass requires reboot
'             7 - Unable to change service startup type
'             8 - Unable to start or stop service
'             9 - Path not found
'
' History: (7/8/2003) Dean Gjedde - Initial coding
'          (10/23/2003) Dean Gjedde - changed shares to smx.net
'          (12/09/2009) Dean Gjedde - Change usage example - trival change
' ======================================================================
Option Explicit
Dim g_sCommandLineParms

Main()
' -------------------------------------------------------------------
' summary:
'    the RunCMD runs a command and get there errorlevel after the
'       command is run
'
' returns:
'    retruns the errorlevel of the command
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Function RunCMD(sCMD)
    Dim oShell, iExitCode

    Set oShell = WScript.CreateObject("WScript.Shell")
    WScript.Echo("Running:")
    WScript.Echo(sCMD)
    iExitCode = oShell.Run(sCMD,, True)

    RunCMD = iExitCode

    Set oShell = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    create the SQL Server 2000 reg key install hack for W2K3
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Function WriteSQLReg(sLog)
    Const SQLRegKey = "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags"
    Const SQLReg = "{ff25f0b5-c894-45f4-a29d-1bdd0c7926cd}"
    Const SQLRegValue = 1
    Const SQLRegType = "REG_DWORD"
    Const iError = 4

    Dim oShell, sRegValue
    Set oShell = WScript.CreateObject("WScript.Shell")
    oShell.RegWrite SQLRegKey & "\" & SQLReg, SQLRegValue, SQLRegType
    sRegValue = oShell.RegRead(SQLRegKey & "\" & SQLReg)
    If sRegValue <> SQLRegValue Then
        WriteLog "VAR_ABORT - Failed to write SQL Reg Key", sLog, iError
    End If

    Set oShell = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    Get the build number for the windows install
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Function GetWindowsBuild()
    Dim oLocator, oServices, oInfoSet, oInfo, sBuildNum

    Set oLocator = CreateObject("Wbemscripting.SWbemlocator")
    Set oServices = oLocator.ConnectServer(".", "root\CIMv2")
    Set oInfoSet = oServices.InstancesOf("Win32_OperatingSystem")

    For Each oInfo In oInfoSet
        sBuildNum = oInfo.BuildNumber
    Next

    GetWindowsBuild = sBuildNum

    Set oInfoSet = Nothing
    Set oServices = Nothing
    Set oLocator = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    Checks for SQL Server Service
'
'returns:
'    Returns true if SQL Server Service is on the box and fale if not
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Function CheckForSQL()
    Dim oLocator, oServices, oInfoSet, oInfo

    Set oLocator = CreateObject("Wbemscripting.SWbemlocator")
    Set oServices = oLocator.ConnectServer(".", "root\CIMv2")
    Set oInfoSet = oServices.InstancesOf("Win32_Service")

    For Each oInfo In oInfoSet
        Dim sService
        sService = oInfo.Name
        If UCase(oInfo.Name) = "MSSQLSERVER" Then
            CheckForSQL = True
            Exit Function
        ElseIf UCase(InStr(oInfo.Name, "MSSQL$")) Then
            CheckForSQL = True
            Exit Function
        Else
            CheckForSQL = False
        End If
    Next

    Set oInfoSet = Nothing
    Set oServices = Nothing
    Set oLocator = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    the WriteLog function will write messages to a log file And
'    report errors to the user.
'
' returns:
'    Nothing
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Sub WriteLog(sMessage, sLog, iExitCode)
    On Error Resume Next
    Const ForAppending = 8

    Dim sLogFile, oFSO, oStream

    sLogFile = sLog
    Set oFSO = CreateObject("Scripting.FileSystemObject")
    Set oStream = oFSO.OpenTextFile(sLogFile, ForAppending, True)
    If Err.Number <> 0 Then
        oStream.Close
        WScript.Echo("Inst_SQL was unable to write a log")
        WScript.Echo(Err.Description)
        Exit Sub
    End If

    oStream.Write(vbCrlf & "*LOG_START*-Inst_SQL")
    oStream.Write(vbCrlf & "Inst_SQL.vbs command line parameter(s) are:")
    oStream.Write(vbCrlf & vbTab & g_sCommandLineParms)
    If iExitCode <> 0 Then
        oStream.Write(vbCrlf & "[" &  FormatDateTime(Date, 2) & " " & _
            FormatDateTime(Time, 4)  & "] ERR Inst_SQL " & sMessage)
    Else
        oStream.Write(vbCrlf & "[" &  FormatDateTime(Date, 2) & " " & _
            FormatDateTime(Time, 4)  & "] Inst_SQL " & sMessage)
    End If
    oStream.Write(vbCrlf & "*LOG_DONE*")
    oStream.Close
    WScript.Echo(sMessage)
    WScript.Quit(iExitCode)

    Set oStream = Nothing
    Set oFSO = Nothing
End Sub

' -------------------------------------------------------------------
' summary:
'    the ReplaceFileTxt function will replace text in a file and write
'   it to the new file
'
' returns:
'    Nothing
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Sub ReplaceFileTxt(sFile, sNewFile, sRegEx, sReplaceTxt, sLog)
    On Error Resume Next
    Const ForWriting = 2
    Const ForReading = 1
    Const CreateFile = True
    Const DontCreateFile = False
    Const iError = 4

    Dim oEnv, oFSO, oStream, oRegEx, sTxt, sNewTxt

    Set oRegEx = New RegExp
    oRegEx.Pattern = sRegEx
    oRegEx.IgnoreCase = False

    Set oFSO = CreateObject("Scripting.FileSystemObject")
    Set oStream = oFSO.OpenTextFile(sFile, ForReading, DontCreateFile)
    If Err.Number <> 0 Then
        oStream.Close
        WriteLog "Inst_SQL was unable to read file " & sFile, sLog, iExitCode
        Exit Sub
    End If

    sTxt = oStream.ReadAll
    oStream.Close
    Set oStream = Nothing
    sNewTxt = oRegEx.Replace(sTxt, sReplaceTxt)

    Set oStream = oFSO.OpenTextFile(sNewFile, ForWriting, CreateFile)
    If Err.Number <> 0 Then
        oStream.Close
        WriteLog "Inst_SQL was unable to write file " & sFile, sLog, iExitCode
        Exit Sub
    End If
    oStream.Write(sNewTxt)
    oStream.Close

    Set oRegEx = Nothing
    Set oStream = Nothing
    Set oFSO = Nothing
End Sub

' -------------------------------------------------------------------
' summary:
'    gets the password for the specific user account
'
'returns:
'    returns a string containing the password
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Function GetAccountPassword(strAccount, strLog)
    On Error Resume Next
    Const iError = 3
    Dim oCDS, sPassword

    Set oCDS = WScript.CreateObject("CDSCom.Users")
    If Err.Number <> 0 Then
        WriteLog "VAR_ABORT - Unable to create CDSCom object", strLog, iError
    End If
    sPassword = oCDS.GetPassword(strAccount)
    GetAccountPassword = sPassword

    Set oCDS = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    sets a service's startup type
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Function ServiceStartupType(sServiceName, sStartupType, sLog)
    On Error Resume Next
    Const iExitCode = 7
    Dim oLocator, oServices, oInfoSet, oInfo, iError

    Set oLocator = CreateObject("Wbemscripting.SWbemlocator")
    Set oServices = oLocator.ConnectServer(".", "root\CIMv2")
    Set oInfoSet = oServices.ExecQuery("SELECT * FROM Win32_Service WHERE Name = '" & sServiceName & "'")

    For Each oInfo In oInfoSet
        If Not UCase(oInfo.startMode) = sStartupType Then
            iError = oInfo.ChangeStartMode(sStartupType)
        End If
    Next
    If iError <> 0 Then
        WriteLog "VAR_ABORT - Unable to change " & sServiceName & " startup type", sLog, iExitCode
    End If

    Set oInfoSet = Nothing
    Set oServices = Nothing
    Set oLocator = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    stops or starts a service
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Function ChangeServiceStatus(sServiceName, sStatus, sLog)
    On Error Resume Next
    Const iExitCode = 8
    Dim oLocator, oServices, oInfoSet, oInfo, sServiceStatus, iError

    sServiceStatus = UCase(sStatus)
    Set oLocator = CreateObject("Wbemscripting.SWbemlocator")
    Set oServices = oLocator.ConnectServer(".", "root\CIMv2")
    Set oInfoSet = oServices.ExecQuery("SELECT * FROM Win32_Service WHERE Name = '" & sServiceName & "'")

    For Each oInfo In oInfoSet
        Select Case sServiceStatus
            Case "START"
                If Not UCase(oInfo.state) = "RUNNING" Then
                    iError = oInfo.StartService()
                End If
            Case "STOP"
                If Not UCase(oInfo.state) = "STOPPED" Then
                    iError = oInfo.StopService()
                End If
        End Select
    Next
    If iError <> 0 Then
        WScript.Echo(iError)
        WriteLog "VAR_ABORT - Unable to " & sServiceStatus & " service " & sServiceName, sLog, iExitCode
    End If

    Set oInfoSet = Nothing
    Set oServices = Nothing
    Set oLocator = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    displays usage
'
'returns:
'    Nothing
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Sub Usage()
    WScript.Echo("Usage:")
    WScript.Echo("cscript.exe Inst_SQL.vbs /VERSION: [/TYPE:] [/LANGUAGE:]" _
        & " [/?] [/SQLINSTANCE:] [/SAPASSWORD] [/PORTNUMBER:] [/COLLATION]")
    WScript.Echo(vbTab & "[/INSTDIR] [/INSTDATADIR:]")
    WScript.Echo("/VERSION: - Supports ENT (Enterprise), STD (Standard), DEV (Developer), PER (Personal), and MSDE")
    WScript.Echo(vbTab & "Note: Only ENT and DEV are available for IA64")
    WScript.Echo("/TYPE: - Supports x86, ia64. %PROCESSOR_ARCHITECTURE% is the default value for /Type:")
    WScript.Echo("/LANGUAGE: - Supports EN (English), DE (German), FR (French)" _
        & ", JA (Japanese), KO (Korean), ES (Spanish),")
    WScript.Echo(vbTab & "CN (Simplified Chinese), TW (Traditional Chinese, and IT (Italian) builds")
    WScript.Echo(vbTab & "EN is default for /Language:")
    WScript.Echo("/?: Displays usage for inst_momx")
    WScript.Echo("/SQLINSTANCE: - SQL named instance, %COMPUTERNAME% is the default for /SqlInstance:")
    WScript.Echo("/SAPASSWORD: - SQL SA password, If not specified the default SA password will be retrieved from CDS")
    WScript.Echo("/PORTNUMBER: - The TCP/IP port number that SQL Service will communicate on. 1433 " _
        & "is the default value for /PortNumber:")
    WScript.Echo(vbTab & vbTab & "This option is not available for IA64 or MSDE installs")
    WScript.Echo("/COLLATION: -  The collation for which the database will store characters")
    WScript.Echo(vbTab & "SQL_Latin1_General_Cp1_CI_AS is the default value for /Collation on EN")
    WScript.Echo(vbTab & "Japanese_CI_AS is the default value for /Collation on JA")
    WScript.Echo(vbTab & "Latin1_General_CI_AS is the default value for /Collation on DE")
    WScript.Echo(vbTab & "French_CI_AS is the default value for /Collation on FR")
    WScript.Echo(vbTab & "Italian_CI_AS is the default value for /Collation on IT")
    WScript.Echo(vbTab & "Korean_Wansung_CI_AS is the default value for /Collation on KO")
    WScript.Echo(vbTab & "Modern_Spanish_CI_AS is the default value for /Collation on ES")
    Wscript.Echo(vbTab & "Chinese_PRC_CI_AS is the default value for /Collation on CN")
    WScript.Echo(vbTab & "Chinese_Taiwan_Stroke_CI_AS is the default value for /Collation on TW")
    WScript.Echo(vbTab & vbTab & "Note this is also where you would specify case-sensitive or not")
    WScript.Echo(vbTab & vbTab & "Please see http://msdn.microsoft.com/library/default.asp?url=/library" _
        & "/en-us/tsqlref/ts_ca-co_5ell.asp for more information")
    WScript.Echo("/INSTDIR: - The location of the SQL Server files. %PROGRAMFILES%\Microsoft SQL Server is" _
        & " the default value for /InstDIR:.")
    WScript.Echo("/INSTDATADIR: The location of the SQL Server data files. %PROGRAMFILES%\Microsoft SQL Server" _
        & " is the default value for /InstDATADIR:.")
    WScript.Echo("Examples:")
    WScript.Echo("Install SQL Enterprise default:")
    WScript.Echo(vbTab & "cscript.exe inst_sql.vbs /version:ent")
    WScript.Echo("Install SQL Enterprise case sensitive:")
    WScript.Echo(vbTab & "cscript.exe inst_sql.vbs  /version:ent /Collation: SQL_Latin1_General_Cp1_CS_AS")
    WScript.Echo("Install MSDE named instance:")
    WScript.Echo(vbTab & "cscript.exe Inst_sql.vbs /version:MSDE /SqlInstance:""MSDE-TEST""")
End Sub

' -------------------------------------------------------------------
' summary:
'    the main function gets everything started.
'
' returns:
'    Nothing
'
' History: (7/8/2003) Dean Gjedde - Initial coding
' -------------------------------------------------------------------
Sub Main()
    'Const sDefaultPath = "\\smx.net\products\SQL\2000.80.194.0\X86"
    Const sDefaultPath = "\\momfiles05\SQL\2000.80.194.0\X86"
    Const sSQLRTMFile = "setupsql.exe"
    Const sSQLIA64 = "setup.exe"
    Const sSQLRTM = "sqlins.iss"
    Const sSQLSP3 = "sql2kdef.iss"
    Const sSQLCustom = "sqlins_cust.iss"
    Const X86 = "X86"
    Const IA64 = "IA64"
    Const sLog = "Inst_SQL.Log"
    Const sRegExPortNum = "^[0-9]{1,6}$"
    Const sRegExSQLInstance = "^[\w-]{1,16}$"
    Const sDefaultENCollation = "SQL_Latin1_General_Cp1_CI_AS"
    Const sDefaultJACollation = "Japanese_CI_AS"
    Const sDefaultFRCollation = "French_CI_AS"
    Const sDefaultDECollation = "Latin1_General_CI_AS"
    Const sDefaultITCollation = "Italian_CI_AS"
    Const sDefaultKOCollation = "Korean_Wansung_CI_AS"
    Const sDefaultESCollation = "Modern_Spanish_CI_AS"
    Const sDefaultCNCollation = "Chinese_PRC_CI_AS"
    Const sDefaultTWCollation = "Chinese_Taiwan_Stroke_CI_AS"
    Const sDefaultSQLInstanceName = "MSSQLSERVER"
    Const bOverRide = True
    Const sSQLUser = "SQL\SA"
    Const W2K3 = "3790"
    Const sDefaultAutoStart = "AutoStart=15"
    Const sAutoStart = "AutoStart=0"
    Const sDefaultLicense = "LicenseLimit=5"
    Const sLicense = "LicenseLimit=10000"
    Const sDefaultCollation = "collation_name=[\w\' ]+"
    Const sDefaultInstance = "InstanceName=MSSQLSERVER"
    Const sDefaultInstDir = "szDir=%PROGRAMFILES%\Microsoft SQL Server"
    Const sDefaultInstDataDir = "szDataDir=%PROGRAMFILES%\Microsoft SQL Server"
    Const sDefaultPW = "EnterPwd="
    Const sDefaultConfirmPW = "ConfirmPwd="
    Const sDefaultUpgrade = "UpgradeMSSearch=0"
    Const sNewUpgrade = "UpgradeMSSearch=1"
    Const sDefaultInstallType = "TYPE=1"
    Const sNewInstallType = "Type=32"
    Const sErrorReporting = "EnableErrorReporting=0"
    Const sNewErrorReporting = "EnableErrorReporting=1"
    Const sDefaultTCPPort = "TCPPort=1433"
    Const sNewCustomTCPPort = "TCPPort=0"
    Const sDefaultNMPipe = "NMPPipeName=\\\\\.\\pipe\\sql\\query"
    Const sFlags = "-s -SMS -f1"
    Const iNeedRebootError = 3010
    Const iPendingFileOperationError = 50071
    Const iMSINeedRebootError = 1641
    Const iMSIInstallFailure = 1603
    Const iUserInputError = 1
    Const iPathNotFoundError = 9
    Const iInstallReboot = 6
    Const iInstallFailureError = 5
    Const iNoError = 0
    Const sPIDKey = "K7J7MQY3XY9PDBW74FCRFVM3J"

    Dim oShell, oEnv, aSplit, aArguments, aDASUser, aCAMUser, sArg
    Dim sBuild, sType, sVersion, sLanguage, sPath, sSQLInstance '''''
    Dim sSAPassword, sPortNumber, sCollation, sInstDir, sInstDataDir
    Dim sCommand, sComputerName, sArch, oRegEx, oFSO, sSQLRTMCustIssFile
    Dim sSystemDrive, sMSIEXECLog, sParms, sSplit, sTempDir, sIssPath
    Dim sSQLRTMIssFile, sSQLSP3IssFile, sSPCommand, iInstallError, sSPParms

    g_sCommandLineParms = ""
    For Each sArg In WScript.Arguments
        g_sCommandLineParms = g_sCommandLineParms & " " & sArg
    Next
    sSQLRTMIssFile = sDefaultPath & "\EN\SQL2000a\X86\ENT\" & sSQLRTM
    sSQLSP3IssFile = sDefaultPath & "\EN\ServicePack\X86\SP3\" & sSQLSP3
    sSQLRTMCustIssFile = sDefaultPath & "\EN\SQL2000a\X86\ENT\" & sSQLCustom

    'Check if inst_momx.vbs is being run under CScript.exe
    If Not((InStr(1, WScript.FullName, "CSCRIPT", vbTextCompare) <> 0)) Then
        WriteLog "VAR_ABORT - This script must be run with CScript.exe", sLog, iUserInputError
    End If

    'If for correct number of arguments
    Set aArguments = WScript.Arguments
    If aArguments.Count < 1 Then
        Usage()
        WriteLog "VAR_ABORT - You must specify switch /VERSION:", sLog, iUserInputError
    End If

    Set oShell = WScript.CreateObject("WScript.Shell")
    Set oFSO = CreateObject("Scripting.FileSystemObject")
    sComputerName = oShell.ExpandEnvironmentStrings("%COMPUTERNAME%")
    sSystemDrive = oShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%")
    sTempDir = oShell.ExpandEnvironmentStrings("%TEMP%")
    sArch = UCase(oShell.ExpandEnvironmentStrings("%PROCESSOR_ARCHITECTURE%"))

    'Parse command line arguments
    For Each sArg In WScript.Arguments
        aSplit = Split(UCase(sArg), ":", 2)
        If Not UBound(aSplit) = 0 Then
            sSplit = aSplit(1)
        End If
        Select Case aSplit(0)
            Case "/?"
                Usage()
                WriteLog "VAR_ABORT - usage requested", sLog, iUserInputError
            Case "/TYPE"
                sType = sSplit
            Case "/VERSION"
                If sSplit = "ENT" Or sSplit = "STD" Or _
                    sSplit = "PER" Or sSplit = "DEV" Or _
                    sSplit = "MSDE" Then
                    sVersion = sSplit
                Else
                    WriteLog "VAR_ABORT - " & sSplit & " is not a supported /VERSION:", sLog, iUserInputError
                End If
            Case "/LANGUAGE"
                sLanguage = sSplit
            Case "/SQLINSTANCE"
                Set oRegEx = New RegExp
                oRegEx.Pattern = sRegExSQLInstance
                oRegEx.IgnoreCase = False
                If Not(oRegEx.Test(sSplit)) Then
                    WriteLog "VAR_ABORT - " & sSplit & " is an invalid SQL Instance name", sLog, iUserInputError
                End If
                sSQLInstance = sSplit
            Case "/SAPASSWORD"
                sSAPassword = sSplit
            Case "/PORTNUMBER"
                Set oRegEx = New RegExp
                oRegEx.Pattern = sRegExPortNum
                oRegEx.IgnoreCase = False
                If Not(oRegEx.Test(sSplit)) Then
                    WriteLog "VAR_ABORT - " & sSplit & " is an invalid portnumber", sLog, iUserInputError
                End If
                sPortNumber = sSplit
            Case "/COLLATION"
                sCollation = sSplit
            Case "/INSTDIR"
                sInstDir = sSplit
            Case "/INSTDATADIR"
                sInstDataDir = sSplit
            Case Else
                WriteLog "VAR_ABORT - " & sArg & " is not a supported argument", sLog, iUserInputError
        End Select
    Next

    If sVersion = "" Then
        Usage()
        WriteLog "VAR_ABORT - /VERSION: switch is empty", sLog, iUserInputError
    End If

    If sType = "" Then
        sType = sArch
    End If
    If sLanguage = "" Then
        sLanguage = "EN"
    End If

    sMSIEXECLog = sSystemDrive & "\logs\" & sVersion & "_Install.log"
    If Not (oFSO.FolderExists(sSystemDrive & "\logs\")) Then
        oFSO.CreateFolder(sSystemDrive & "\logs\")
    End If

    If sArch <> sType Then
        WriteLog "VAR_ABORT - You cannot install " & sType & " on a " & sArch & " computer", sLog , iUserInputError
    End If

    If sType = IA64 Then
        Select Case sVersion
            Case "MSDE"
                WriteLog "VAR_ABORT - You cannot install MSDE on an " & sType & " computer", sLog, iUserInputError
            Case "PER"
                WriteLog "VAR_ABORT - You cannot install SQL Server Persional on an " & sType & " computer", sLog, iUserInputError
            Case "STD"
                WriteLog "VAR_ABORT - You cannot install SQL Server Standard on an " & sType & " computer", sLog, iUserInputError
        End Select
    End If

    If sVersion = "MSDE" And sPortNumber <> "" Then
        WriteLog "VAR_ABORT - /PORTNUMBER: switch cannot be used with /VERSION:MSDE", strLog, iUserInputError
    End If

    If sSAPassword = "" Then
        sSAPassword = GetAccountPassword(sSQLUser, sLog)
    End If

    If sCollation = "" Then
        Select Case sLanguage
            Case "EN"
                sCollation = sDefaultENCollation
            Case "DE"
                sCollation = sDefaultDECollation
            Case "FR"
                sCollation = sDefaultFRCollation
            Case "JA"
                sCollation = sDefaultJACollation
            Case "IT"
                sCollation = sDefaultITCollation
            Case "KO"
                sCollation = sDefaultKOCollation
            Case "ES"
                sCollation = sDefaultESCollation
            Case "CN"
                sCollation = sDefaultCNCollation
            Case "TW"
                sCollation = sDefaultTWCollation
            Case Else
                sCollation = sDefaultENCollation
        End Select
    End If

    If sType = IA64 Or sVersion = "MSDE" Then
        'Build Parameters
        sParms = " PERPROCESSOR=10000 AGTAUTOSTART=1 SQLAUTOSTART=1 SECURITYMODE=SQL SAPWD=""" & sSAPassword & "" _
            & """ COLLATION=""" & sCollation & "" & """ PIDKEY=" & sPIDKey
        If Not sSQLInstance = "" Then
            sParms = sParms & " INSTANCENAME=""" & sSQLInstance & """"
        End If
        If Not sInstDir = "" Then
            sParms = sParms & " INSTALLSQLDIR=""" & sInstDir & """"
        End If
        If Not sInstDataDir = "" Then
            sParms = sParms & " INSTALLSQLDATADIR="""& sInstDataDir & """"
        End If
        sParms = sParms & " /qn /l*v " & sMSIEXECLog
    Else
        If oFSO.FileExists(sSQLRTMIssFile) Then
            oFSO.CopyFile sSQLRTMIssFile, sTempDir & "\", bOverRide
        Else
            WriteLog "VAR_ABORT - " & sSQLRTMIssFile & " does not exist or is not accessable", sLog, iPathNotFoundError
        End If
        If oFSO.FileExists(sSQLSP3IssFile) Then
            oFSO.CopyFile sSQLSP3IssFile, sTempDir & "\", bOverRide
        Else
            WriteLog "VAR_ABORT - " & sSQLSP3IssFile & " does not exist or is not accessable", sLog, iPathNotFoundError
        End If
        If(GetWindowsBuild = W2K3) Then
            WriteSQLReg(sLog)
        End If
        'write entries to the SQL RTM custom iss file to install more than one instance on the computer
        If(CheckForSQL) Then
            If oFSO.FileExists(sSQLRTMCustIssFile) Then
                oFSO.CopyFile sSQLRTMCustIssFile, sTempDir & "\", bOverRide
            Else
                WriteLog "VAR_ABORT - " & sSQLRTMCustIssFile & " does not exist or is not accessable", sLog, iPathNotFoundError
            End If
            ReplaceFileTxt sTempDir & "\" & sSQLCustom, sTempDir & "\" & sSQLCustom, sDefaultAutoStart, sAutoStart, sLog
            ReplaceFileTxt sTempDir & "\" & sSQLCustom, sTempDir & "\" & sSQLCustom, sDefaultLicense, sLicense, sLog
            Dim sNewCollation
            sNewCollation = "collation_name=" & sCollation
            ReplaceFileTxt sTempDir & "\" & sSQLCustom, sTempDir & "\" & sSQLCustom, sDefaultCollation, sNewCollation , sLog
            If Not sSQLInstance = "" Then
                Dim sNewInstance
                sNewInstance = "InstanceName=" & sSQLInstance
                ReplaceFileTxt sTempDir & "\" & sSQLCustom, sTempDir & "\" & sSQLCustom, sDefaultInstance, sNewInstance, sLog
                Dim sNewCustomNMPipe
                sNewCustomNMPipe = "NMPPipeName=\\.\pipe\MSSQL$" & sSQLInstance & "\sql\query"
                ReplaceFileTxt sTempDir & "\" & sSQLCustom, sTempDir & "\" & sSQLCustom, sDefaultNMPipe, sNewCustomNMPipe, sLog
            End If
            If Not sPortNumber = "" Then
                Dim sNewPort
                sNewPort = "TCPPort=" & sPortNumber
                ReplaceFileTxt sTempDir & "\" & sSQLCustom, sTempDir & "\" & sSQLCustom, sDefaultTCPPort, sNewPort, sLog
                ReplaceFileTxt sTempDir & "\" & sSQLSP3, sTempDir & "\" & sSQLSP3, sDefaultTCPPort, sNewPort, sLog
            Else
                ReplaceFileTxt sTempDir & "\" & sSQLCustom, sTempDir & "\" & sSQLCustom, sDefaultTCPPort, sNewCustomTCPPort, sLog
                ReplaceFileTxt sTempDir & "\" & sSQLSP3, sTempDir & "\" & sSQLSP3, sDefaultTCPPort, sNewCustomTCPPort, sLog
            End If
            If Not sInstDir = "" Then
                Dim sNewInstDir
                sNewInstDir = "szDir=" & sInstDir
                ReplaceFileTxt sTempDir & "\" & sSQLCustom, sTempDir & "\" & sSQLCustom, sDefaultInstDir, sNewInstDir, sLog
            End If
            If Not sInstDataDir = "" Then
                Dim sNewInstDataDir
                sNewInstDataDir = "szDataDir=" & sInstDataDir
                ReplaceFileTxt sTempDir & "\" & sSQLCustom, sTempDir & "\" & sSQLCustom, sDefaultInstDir, sNewInstDataDir, sLog
            End If
        End If
        'write entries to the SQL RTM iss file
        ReplaceFileTxt sTempDir & "\" & sSQLRTM, sTempDir & "\" & sSQLRTM, sDefaultAutoStart, sAutoStart, sLog
        ReplaceFileTxt sTempDir & "\" & sSQLRTM, sTempDir & "\" & sSQLRTM, sDefaultLicense, sLicense, sLog
        sNewCollation = "collation_name=" & sCollation
        ReplaceFileTxt sTempDir & "\" & sSQLRTM, sTempDir & "\" & sSQLRTM, sDefaultCollation, sNewCollation , sLog
        If Not sSQLInstance = "" Then
            sNewInstance = "InstanceName=" & sSQLInstance
            ReplaceFileTxt sTempDir & "\" & sSQLRTM, sTempDir & "\" & sSQLRTM, sDefaultInstance, sNewInstance, sLog
            sNewCustomNMPipe = "NMPPipeName=\\.\pipe\MSSQL$" & sSQLInstance & "\sql\query"
            ReplaceFileTxt sTempDir & "\" & sSQLRTM, sTempDir & "\" & sSQLRTM, sDefaultNMPipe, sNewCustomNMPipe, sLog
        End If
        If Not sPortNumber = "" Then
            sNewPort = "TCPPort=" & sPortNumber
            ReplaceFileTxt sTempDir & "\" & sSQLRTM, sTempDir & "\" & sSQLRTM, sDefaultTCPPort, sNewPort, sLog
        End If
        If Not sInstDir = "" Then
            sNewInstDir = "szDir=" & sInstDir
            ReplaceFileTxt sTempDir & "\" & sSQLRTM, sTempDir & "\" & sSQLRTM, sDefaultInstDir, sNewInstDir, sLog
        End If
        If Not sInstDataDir = "" Then
            sNewInstDataDir = "szDataDir=" & sInstDataDir
            ReplaceFileTxt sTempDir & "\" & sSQLRTM, sTempDir & "\" & sSQLRTM, sDefaultInstDir, sNewInstDataDir, sLog
        End If
        'write entries to the SQL SP3 iss file
        ReplaceFileTxt sTempDir & "\" & sSQLSP3, sTempDir & "\" & sSQLSP3, sDefaultUpgrade, sNewUpgrade, sLog
        Dim sNewPW
        sNewPW = "EnterPwd=" & sSAPassword
        ReplaceFileTxt sTempDir & "\" & sSQLSP3, sTempDir & "\" & sSQLSP3, sDefaultPW, sNewPW, sLog
        Dim sNewConfirmPW
        sNewConfirmPW = "ConfirmPwd=" & sSAPassword
        ReplaceFileTxt sTempDir & "\" & sSQLSP3, sTempDir & "\" & sSQLSP3, sDefaultConfirmPW, sNewConfirmPW, sLog
        ReplaceFileTxt sTempDir & "\" & sSQLSP3, sTempDir & "\" & sSQLSP3, sErrorReporting, sNewErrorReporting, sLog
        If Not sSQLInstance = "" Then
            sNewInstance = "InstanceName=" & sSQLInstance
            ReplaceFileTxt sTempDir & "\" & sSQLSP3, sTempDir & "\" & sSQLSP3, sDefaultInstance, sNewInstance, sLog
            sNewCustomNMPipe = "NMPPipeName=\\.\pipe\MSSQL$" & sSQLInstance & "\sql\query"
            ReplaceFileTxt sTempDir & "\" & sSQLSP3, sTempDir & "\" & sSQLSP3, sDefaultNMPipe, sNewCustomNMPipe, sLog
        End If
        If (CheckForSQL) Then
            sParms = sTempDir & "\" & sSQLCustom
        Else
            sParms = sTempDir & "\" & sSQLRTM
        End If
        sSPParms = sTempDir & "\" & sSQLSP3
    End If

    Select Case sVersion
        Case "MSDE"
            sCommand = sDefaultPath & "\" & sLanguage & "\MSDEa\" & sType & "\SP3\" & sSQLIA64
        Case Else
            If sType = IA64 Then
                sCommand = sDefaultPath & "\" & sLanguage & "\SQL2000a\" & sType & "\" & sVersion & "\" & sSQLIA64
            Else
                sCommand = sDefaultPath & "\" & sLanguage & "\SQL2000a\" & sType & "\" & sVersion & "\x86\setup\" & sSQLRTMFile
                sSPCommand = sDefaultPath & "\" & sLanguage & "\ServicePack\" & sType & "\SP3\x86\setup\" & sSQLRTMFile
            End If
    End Select

    'Check if path is correct
    If Not(oFSO.FileExists(sCommand)) Then
        WriteLog "VAR_ABORT - " & sCommand & " does not exist or is not accessable", sLog, iPathNotFoundError
    End If
    If Not sSPCommand = "" Then
        If Not(oFSO.FileExists(sSPCommand)) Then
            WriteLog "VAR_ABORT - " & sSPCommand & " does not exist or is not accessable", sLog, iPathNotFoundError
        End If
    End If

    ' 'Install from path specified by the user
    If sType = IA64 Or sVersion = "MSDE" Then
        ChangeServiceStatus "SNMP", "STOP", sLog
        iInstallError = RunCMD(sCommand & sParms)
        ChangeServiceStatus "SNMP", "START", sLog
        Select Case iInstallError
            Case 0
                WriteLog "VAR_PASS - Install for " & sVersion & " " & sType & " passed", sLog, 0
            Case iMSINeedRebootError
                WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " passed but requres a reboot", sLog, iInstallReboot
            Case iMSIInstallFailure
                WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " failed with error number " & iMSIInstallFailure, sLog, iInstallFailureError
            Case Else
                WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " failed with error number " & iInstallError, sLog, iInstallFailureError
        End Select
    Else
        ChangeServiceStatus "SNMP", "STOP", sLog
        iInstallError = RunCMD(sCommand & " " & sFlags & " " & sParms)
        Select Case iInstallError
            Case 0
                ChangeServiceStatus "SNMP", "STOP", sLog
                iInstallError = RunCMD(sSPCommand & " " & sFlags & " " & sSPParms)
                Select Case iInstallError
                    Case 0
                        ChangeServiceStatus "SNMP", "START", sLog
                        If Not sSQLInstance = "" Then
                            ServiceStartupType "MSSQL$" & sSQLInstance, "AUTOMATIC", sLog
                            ServiceStartupType "SQLAGENT$" & sSQLInstance, "AUTOMATIC", sLog
                            ChangeServiceStatus "MSSQL$" & sSQLInstance, "START", sLog
                            ChangeServiceStatus "SQLAGENT$" & sSQLInstance, "START", sLog
                        Else
                            ServiceStartupType "MSSQLSERVER", "AUTOMATIC", sLog
                            ServiceStartupType "SQLSERVERAGENT", "AUTOMATIC", sLog
                            ChangeServiceStatus "MSSQLSERVER", "START", sLog
                            ChangeServiceStatus "SQLSERVERAGENT", "START", sLog
                        End If
                        WriteLog "VAR_PASS - Install for " & sVersion & " " & sType & " passed", sLog, iNoError
                    Case iNeedRebootError
                        ChangeServiceStatus "SNMP", "START", sLog
                        If Not sSQLInstance = "" Then
                            ServiceStartupType "MSSQL$" & sSQLInstance, "AUTOMATIC", sLog
                            ServiceStartupType "SQLAGENT$" & sSQLInstance, "AUTOMATIC", sLog
                            ChangeServiceStatus "MSSQL$" & sSQLInstance, "START", sLog
                            ChangeServiceStatus "SQLAGENT$" & sSQLInstance, "START", sLog
                        Else
                            ServiceStartupType "MSSQLSERVER", "AUTOMATIC", sLog
                            ServiceStartupType "SQLSERVERAGENT", "AUTOMATIC", sLog
                            ChangeServiceStatus "MSSQLSERVER", "START", sLog
                            ChangeServiceStatus "SQLSERVERAGENT", "START", sLog
                        End If
                        WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " passed but requres a reboot", sLog, iInstallReboot
                    Case iPendingFileOperationError
                        WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " passed but requres a reboot to flush the pending file operation flag " & iMSIInstallFailure, sLog, iInstallFailureError
                    Case Else
                        WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " failed with error number " & iInstallError, sLog, iInstallFailureError
                End Select
            Case iNeedRebootError
                ChangeServiceStatus "SNMP", "STOP", sLog
                iInstallError = RunCMD(sSPCommand & " " & sFlags & " " & sSPParms)
                Select Case iInstallError
                    Case 0
                        ChangeServiceStatus "SNMP", "START", sLog
                        If Not sSQLInstance = "" Then
                            ServiceStartupType "MSSQL$" & sSQLInstance, "AUTOMATIC", sLog
                            ServiceStartupType "SQLAGENT$" & sSQLInstance, "AUTOMATIC", sLog
                            ChangeServiceStatus "MSSQL$" & sSQLInstance, "START", sLog
                            ChangeServiceStatus "SQLAGENT$" & sSQLInstance, "START", sLog
                        Else
                            ServiceStartupType "MSSQLSERVER", "AUTOMATIC", sLog
                            ServiceStartupType "SQLSERVERAGENT", "AUTOMATIC", sLog
                            ChangeServiceStatus "MSSQLSERVER", "START", sLog
                            ChangeServiceStatus "SQLSERVERAGENT", "START", sLog
                        End If
                        WriteLog "VAR_PASS - Install for " & sVersion & " " & sType & " passed", sLog, iNoError
                    Case iNeedRebootError
                        If Not sSQLInstance = "" Then
                            ServiceStartupType "MSSQL$" & sSQLInstance, "AUTOMATIC", sLog
                            ServiceStartupType "SQLAGENT$" & sSQLInstance, "AUTOMATIC", sLog
                            ChangeServiceStatus "MSSQL$" & sSQLInstance, "START", sLog
                            ChangeServiceStatus "SQLAGENT$" & sSQLInstance, "START", sLog
                        Else
                            ServiceStartupType "MSSQLSERVER", "AUTOMATIC", sLog
                            ServiceStartupType "SQLSERVERAGENT", "AUTOMATIC", sLog
                            ChangeServiceStatus "MSSQLSERVER", "START", sLog
                            ChangeServiceStatus "SQLSERVERAGENT", "START", sLog
                        End If
                        WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " passed but requres a reboot", sLog, iInstallReboot
                    Case iPendingFileOperationError
                        WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " passed but requres a reboot to flush the pending file operation flag " & iMSIInstallFailure, sLog, iInstallFailureError
                    Case Else
                        WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " failed with error number " & iInstallError, sLog, iInstallFailureError
                End Select
                WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " passed but requres a reboot", sLog, iInstallReboot
            Case iPendingFileOperationError
                WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " passed but requres a reboot to flush the pending file operation flag " & iMSIInstallFailure, sLog, iInstallFailureError
            Case Else
                WriteLog "VAR_ABORT - Install for " & sVersion & " " & sType & " failed with error number " & iInstallError, sLog, iInstallFailureError
        End Select
    End If
End Sub