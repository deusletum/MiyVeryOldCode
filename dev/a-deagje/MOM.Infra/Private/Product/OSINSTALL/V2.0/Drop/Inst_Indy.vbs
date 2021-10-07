' ----------------------------------------------------------------------
' Name      : Inst_Indy.vbs
'
' Company   : oration
'

'
' Summary   : Installs Indy product
'
' Usage     : See usage Function
'
' ErrorCodes: 1 - bad user input
'             3 - Unable to create CDScom object
'             4 - path not found or unavailable
'             5 - MSI install failure
'             6 - MSI install requires reboot
'
' History   : 8/27/2004 - Glenn LaVigne - Created based on Inst_* template
'           : 10/5/2004 - pramodg - Modified for Indy's Setup.msi
' ----------------------------------------------------------------------

Option Explicit

Const iNoError = 0
Const iUserError = 1
Const iCDSError = 3
Const iPathNotFoundError = 4
Const iMSIError = 5
Const iMSIReboot = 6

' Objects that are used by many routines
Dim objShell, objFSO
Set objShell = CreateObject("WScript.Shell")
Set objFSO = CreateObject("Scripting.FileSystemObject")


Dim g_cQuote
Dim g_sCommandLineParms
g_cQuote = Chr(34)

Main()

' -------------------------------------------------------------------
' summary: the main function gets everything started.
' -------------------------------------------------------------------
Sub Main()
    Const strDefaultPath = "\\smx.net\builds\indy"


    Const strIndyGUID = "{1C22ED4E-5E63-46A1-AB92-10C2B9752751}"

    Const strIndyMSI = "Setup.msi"

    Const IA64 = "IA64"
    Const X86 = "X86"

    Const strCDImage = ""

    ' Deal with parsing input command line arguments
    Dim aSplit, aArguments, strArg, sArg, sSplit, strParms

    ' Store the various configuration values
    Dim strBuild, strType, strSKU, strVersion, strLanguage, strPath, strPIDKEY, strFlavor, strGUID
    Dim bPathDefault, bUseDist,  bInteractive, bUninstall

    ' Location to log output from this program
    Dim strLog

    ' Various MSI locations
    Dim strMSIFile, strMSIEXECUninstallLog, strMSIEXECLog

    ' Various local computer locations and information
    Dim  strProgramFiles, strIndyDefaultInstallDir, strSystemDrive, strDistPath, strArch

    bInteractive = False
    bUninstall = False
    bUseDist = False
    bPathDefault = True

    strProgramFiles = objShell.ExpandEnvironmentStrings("%ProgramFiles%")
    strIndyDefaultInstallDir = strProgramFiles & "\Indy"
    strArch = UCase(objShell.ExpandEnvironmentStrings("%PROCESSOR_ARCHITECTURE%"))

    strSystemDrive = objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%")
    strLog = strSystemDrive & "\logs\Inst_Indy.Log"

    If Not objFSO.FolderExists(strSystemDrive & "\logs") Then
        objFSO.CreateFolder(strSystemDrive & "\logs")
    End If

    g_sCommandLineParms = ""
    For Each sArg In WScript.Arguments
        g_sCommandLineParms = g_sCommandLineParms & " " & sArg
    Next

    'Check if Inst_indy is being run under CScript.exe
    If Not((InStr(1, WScript.FullName, "CSCRIPT", vbTextCompare) <> 0)) Then
        WriteLog "VAR_ABORT - This script must be run with CScript.exe", strLog, iUserError, true
    End If

    'If for correct number of arguments
    Set aArguments = WScript.Arguments
    If aArguments.Count < 1 Then
        Usage()
        WriteLog "VAR_ABORT - You must specify at least one switch", strLog, iUserError, true
    End If

     'Parse command line arguments
    For Each strArg In WScript.Arguments
        aSplit = Split(strArg, ":", 2)
        If Not UBound(aSplit) = 0 Then
            sSplit = aSplit(1)
        End If
        Select Case UCase(aSplit(0))
            Case "/?"
                Usage()
                WriteLog "VAR_ABORT - usage requested", strLog, iUserError, true
            Case "/BUILD"
                strBuild = sSplit
            Case "/TYPE"
                strType = sSplit
            Case "/VERSION"
                sSplit = UCase(sSplit) 'Doing case-insensitive comparison
                If sSplit = "COMPLETE" Then
                    strVersion = sSplit
                Else
                    WriteLog "VAR_ABORT - " & sSplit & " is not a supported /VERSION:", strLog, iUserError, true
                End If
            Case "/LANGUAGE"
                strLanguage = sSplit
            Case "/SKU"
                sSplit = UCase(sSplit) 'Doing case-insensitive comparison
                If sSplit = "FULL" Then
                    strSKU = sSplit
                Else
                    WriteLog "VAR_ABORT - " & sSplit & " is not a supported /SKU:", strLog, iUserError, true
                End If
            Case "/PATH"
                strPath = sSplit
                'If /path: is not default check if it exists
                If strPath <> "DEFAULT" Then
                    If Not(objFSO.FolderExists(strPath)) Then
                        WriteLog "VAR_ABORT - User input path " & strPath _
                            & " does not exist or is not accessable", strLog, iPathNotFoundError, true
                    End If
                    bPathDefault = False
                End If
            Case "/DESTPATH"
                strDistPath = sSplit
                bUseDist = True
            Case "/I"
                bInteractive = True
            Case "/UNINSTALL"
                bUNINSTALL = True
            Case "/GUID"
                strGUID = sSplit
            Case "/PIDKEY"
                strPIDKEY = sSplit
            Case Else
                WriteLog "VAR_ABORT - " & strArg & " is not a supported argument", strLog, iUserError, true
                WScript.Quit(iUserError)
        End Select
    Next

    'Check if /BUILD: switch is empty only if it is for install
    If NOT(bUninstall) AND bPathDefault Then
        If strBuild = "" Then
            Usage()
            WriteLog "VAR_ABORT - /BUILD: switch is empty", strLog, iUserError, true
        End If
    End If

    If bUninstall Then
        If strGUID = "" AND strIndyGUID = "" Then
            Usage()
            WriteLog "VAR_ABORT - /UNINSTALL: Guid is empty", strLog, iUserError, true
        End If
    End If

    'Check if /TYPE:, /VERSION:, /LANGUAGE:, and /PATH: switches are empty
    If strType = "" Then
        strType = "RETAIL"
    End If
    If strSKU = "" Then
        strSKU = "FULL"
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

    If strGUID = "" Then
        strGUID = strIndyGUID
    End If

    strType = UCase(strType)
    strSKU = UCase(strSKU)
    strVersion = UCase(strVersion)
    strLanguage = UCase(strLanguage)
    strPath = UCase(strPath)
    strBuild = UCase(strBuild)

    strMSIEXECLog = strSystemDrive & "\logs\" & "Install.log"
    strMSIEXECUninstallLog = strSystemDrive & "\logs\" & "Uninstall.log"

    If strArch <> IA64 Then
        strArch = X86
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
            WriteLog "VAR_ABORT - /TYPE: switch can only take values RETAIL, " _
                & "DEBUG, FRE, CHK, NONOPT, COVER", strLog, iUserError, true
    End Select

    'Build default path
    Select Case strBuild
        Case "LATEST"
            strBuild = GetBuildNum("Indy", "LATEST", strLog)
        Case "BLESSED"
            strBuild = GetBuildNum("Indy", "BLESSED", strLog)
    End Select


    strMSIFile = strDefaultPath & "\" & strLanguage & "\" & strBuild & "\" & strType _
        & "\" & strArch

    'Uninstall If bUninstall is true
    On Error Resume Next
    If (bUninstall) Then
        Select Case strVersion
            Case "COMPLETE"
                Uninstall strVersion, strGUID, strMSIEXECUninstallLog, bInteractive, strLog
        End Select
    End If

    Select Case strVersion
        Case "COMPLETE"
            strParms = strParms & sActionParm & sPIDParm
            If (bUseDist) Then
                strParms = strParms & " TARGETDIR=" & g_cQuote & strDistPath & g_cQuote
            End If
    End Select

    On Error Goto 0

    'Install from path specified by the user
    If Not(bPathDefault) Then
        WriteLog "MSI File Path is " & strPath & "\" & strIndyMSI, strLog, iNoError, false
        Install strVersion, g_cQuote & strPath & "\" & strIndyMSI & g_cQuote, strMSIEXECLog, _
            strParms, bInteractive, strLog
    End If

    'Check if path is correct
    If Not(objFSO.FolderExists(strMSIFile)) Then
        WriteLog "VAR_ABORT - " & strMSIFile & " does not exist or " _
            & "is not accessible", strLog, iPathNotFoundError, true
    End If

    'Install the Product
    strMSIFile = strMSIFile & "\" & strIndyMSI
    WriteLog "MSI File Path is " & strMSIFile, strLog, iNoError, false
    Install strVersion, strMSIFile, strMSIEXECLog, strParms, bInteractive, strLog
End Sub

' -------------------------------------------------------------------
' summary: the RunCMD runs a command and get there errorlevel after the
'       command is run
' -------------------------------------------------------------------
Function RunCMD(strCMD)
    Dim objShell

    Set objShell = WScript.CreateObject("WScript.Shell")

    WScript.Echo("Running:")
    WScript.Echo(strCMD)

    RunCMD = objShell.Run(strCMD,,True)

End Function


' -------------------------------------------------------------------
' summary: gets the build number for the product in strProduct given
'          the build token
' -------------------------------------------------------------------
Function GetBuildNum(strProduct, strBuildToken, strLog)
    On Error Resume Next

    Dim objCDS
    Dim strBuild

    Set objCDS = WScript.CreateObject("CDSCom.Builds")
    If Err.Number <> 0 Then
        WriteLog "VAR_ABORT - Unable to create CDSCom object", strLog, iCDSError
    End If
    strBuild = objCDS.GetLatestBuildByToken(strProduct, strBuildToken)
    If strBuild = "" Then
        WriteLog "VAR_ABORT - There is no " & strBuildToken _
            & " build for product " & strProduct, strLog , iCDSError
    End If
    GetBuildNum = strBuild

    Set objCDS = Nothing
End Function


' -------------------------------------------------------------------
' summary: the WriteLog function will write messages to a log file And
'    optionally quit and report errors to the user.
' -------------------------------------------------------------------
Sub WriteLog(strMessage, strLog, intExitCode, bQuit)
    On Error Resume Next
    Const ForAppending = 8

    Dim strLogFile
    Dim objStream

    strLogFile = strLog

    Set objStream = objFSO.OpenTextFile(strLogFile, ForAppending, True)

    If Err.Number <> 0 Then
        objStream.Close
        WScript.Echo("Inst_Indy was unable to write a log")
        WScript.Echo(Err.Description)
        Exit Sub
    End If

    objStream.Write(vbCrlf & "*LOG_START*-Inst_Indy")
    objStream.Write(vbCrlf & "Inst_Indy.vbs command line parameter(s) are:")
    objStream.Write(vbCrlf & g_sCommandLineParms)
    If intExitCode <> 0 Then
        objStream.Write(vbCrlf & "[" &  FormatDateTime(Date, VbShortDate) & " " & _
            FormatDateTime(Time, VbShortTime)  & "] ERR Inst_Indy.vbs " & strMessage)
    Else
        objStream.Write(vbCrlf & "[" &  FormatDateTime(Date, VbShortDate) & " " & _
            FormatDateTime(Time, VbShortTime)  & "] Inst_Indy.vbs " & strMessage)
    End If
    objStream.Write(vbCrlf & "*LOG_DONE*")
    objStream.Close

    WScript.Echo(strMessage)

    if (bQuit) Then
        WScript.Quit(intExitCode)
    End if

End Sub

' -------------------------------------------------------------------
' summary: uninstalls an msi based install, checks for errors and the Quits the script
' -------------------------------------------------------------------
Sub Uninstall(strVersion, strGUID, strMSILog, bInteractive, strLog)

    Dim strCommand
    Dim intError

    If bInteractive = True Then
        strCommand = "msiexec.exe /x " & strGUID & " /lv* " & strMSILog
    Else
        strCommand = "msiexec.exe /x " & strGUID & " /lv* " & strMSILog & " /Qn"
    End If

    intError = RunCMD(strCommand)
    If intError <> 0 Then
        WriteLog "VAR_FAIL - Uninstall for " & strVersion _
            & " failed with error number " & intError, strLog, intError, true
    Else
        WriteLog "VAR_PASS - Uninstall for " & strVersion _
            & " passed", strLog, iNoError, true
    End If

    WScript.Quit(intError)

End Sub

' -------------------------------------------------------------------
' summary: installs an msi based install, checks for errors and the Quits the script
' -------------------------------------------------------------------
Sub Install(strVersion, strPath, strMSILog, strParms, bInteractive, strLog)
    Const intReboot = 1641

    Dim strCommand
    Dim intError

    If bInteractive = True Then
        strCommand = "msiexec.exe /i " & strPath & " /lv* " & strMSILog & strParms
    Else
        strCommand = "msiexec.exe /i " & strPath & " /lv* " & strMSILog & strParms & " /Qn"
    End If

    WriteLog "Trying to Install with " & strCommand, strLog, 0, false

    intError = RunCMD(strCommand)

    If intError = intReboot Then
        WriteLog "VAR_FAIL - Install for " & strVersion _
            & " passed but requires a reboot", strLog, iMSIReboot, true
    ElseIf intError <> 0 Then
        WriteLog "VAR_FAIL - Install for " & strVersion _
            & " failed with error number " & intError, strLog, intError, true
    Else
        WriteLog "VAR_PASS - Install for " & strVersion _
            & " passed", strLog, iNoError, true
    End If

    WScript.Quit(intError)

End Sub

' -------------------------------------------------------------------
' summary: displays usage
' -------------------------------------------------------------------
Sub Usage()
    WScript.Echo("Usage:")
    WScript.Echo("cscript.exe Inst_Indy.vbs /BUILD: [/TYPE:] [/VERSION:] [/SKU:] [/LANGUAGE:]" _
        & "  [/PATH:] ")
    WScript.Echo(vbTab & "[/DESTPATH:] [/UNINSTALL:] [/PIDKEY:] [/I] [/?] ")
    WScript.Echo


    WScript.Echo("/BUILD: -  Supports build number, LATEST, or BLESSED")
    WScript.Echo(vbTab & " /BUILD: is not required if /PATH: is used")
    WScript.Echo

    WScript.Echo("/TYPE: -  Supports FRE, CHK, DEBUG, RETAIL, NONOPT, COVER")
    WSCript.Echo(vbTab & "RETAIL is default for /TYPE:")
    WScript.Echo

    WScript.Echo("/VERSION: - Supports COMPLETE - will support additonal components in future")
    WScript.Echo(vbTab &"COMPLETE is default for /VERSION:")
    WScript.Echo

    WScript.Echo("/SKU: -  Supports FULL - will support any additional future SKUs")
    WSCript.Echo(vbTab & "FULL is default for /SKU:")
    WScript.Echo

    WScript.Echo("/LANGUAGE: - Supports only EN builds, but will support other " _
        & "languages when they become available")
    WScript.Echo(vbTab & "EN is the default for /LANGUAGE:")
    WScript.Echo

    WScript.Echo("/PATH: - Path to private MSI to install build from - use /PATH:DEFAULT for official builds")
    WScript.Echo(vbTab & "DEFAULT is the default for /PATH:")
    WScript.Echo

    WScript.Echo("/DESTPATH: - Destination path where Indy is going to be installed")
    WScript.Echo(vbTab & vbTab & "If not specified default destination path is used")
    WScript.Echo

    WScript.Echo("/UNINSTALL - Uninstall the product specified in /Version")
    WScript.Echo(vbTab & vbTab & "Note: This is a complete uninstall")
    WScript.Echo

    WScript.Echo("/GUID - The Indy product's GUID")
    WScript.Echo

    WScript.Echo("/PIDKEY - Indy Product Installation Key, please enter PID without dashes '-'")
    WScript.Echo

    WScript.Echo("/I: Run install/uninstall in interactive mode")
    WScript.Echo

    WScript.Echo("/?: Displays usage for inst_Indy")

    WScript.Echo
    WScript.Echo("Examples:")
    WScript.Echo("Indy Full of latest build:")
    WScript.Echo(vbTab & "cscript.exe Inst_Indy.vbs /BUILD:LATEST /TYPE:RETAIL")
    WScript.Echo("Indy Uninstall:")
    WScript.Echo(vbTab & "cscript.exe Inst_Indy.vbs /UNINSTALL")

End Sub