' ============================================================================
' Copyright . 2004
'
' Module : Pops_gl.vbs
'
' Summary: Designed to customize setup on a testO/S
' Dependencies: safeOS bricktools folder; connections to sTOOLSRV and sPRODUCTSRV; MSXML 3.X
'          ErrorCodes:
'            0 - Nothing is wrong
'            1 - User input is wrong
'            2 - a file/folder could not be located
'            3 - some action could not be executed
'                ...could not parse XML file,etc.
'            4 - could not find xml file.
'
' History: (08/10/2003) Jennla - Initial coding
'          (01/12/2004) Deangj - Changed symbols path to \\smx.net\symbols\symbols
'          (01/21/2004) Jheat  - Updated Symbol Path again.
'          (02/05/2004) Jheat  - Updated to support 64 bit.
'          (03/26/2004) Deangj - Added retry to check of pops xml file exist
'          (05/11/2004) Jheat  - Added FQDN variable to system.
'          (05/20/2004) Deangj - Added sleep POPS_Relaunch.cmd to accommodate a WMI problem in AMD64 W2K3
' ============================================================================
Option Explicit

' Common Variables
Const sFAILALIAS    = "SMXOASIS"
Const SYM_VAR       = "_NT_SYMBOL_PATH"  ' Symbols Var.
Const TOOL_VAR      = "ToolSrv"          ' Tool Server Var.
Const OSLANG_VAR    = "OSLang"           ' OS langauge Var.
Const OSVER_VAR     = "OSVer"            ' OS version Var.
Const FQDN_VAR      = "FQDN"             ' FQDN Var.

' Server Names
Const sTOOLSRV      = "\\smx.net\tools"
Const sPRODUCTSRV   = "\\smx.net\products"
Const sSAFEDRIVE    = "C:"
Const sSAFEKEEP     = "c:\keep"
Const sSAFEDEL      = "c:\delete"

' Method Argument inputs
Const iWINMIN       = 2
Const iFORWRITING   = 2
Const iFORAPPENDING = 8
Const iSUBSTRINGS   = -1
Const bWINWAIT      = True
Const bNOWINWAIT    = False
Const bCREATEDOC    = True
Const bNOCREATEDOC  = False

'================== Create Common Objects ====================================
Dim objFSO
Set objFSO   = CreateObject("Scripting.FileSystemObject")

Dim objShell
Set objShell = CreateObject("WScript.Shell")

' Common System Variables
Dim g_SysDrive
g_SysDrive   = UCase(objShell.Environment("process").Item("SystemDrive"))

Dim g_SysRoot
g_SysRoot    = UCase(objShell.Environment("process").Item("SystemRoot"))

Dim g_SysDomain
g_SysDomain  = UCase(objShell.Environment("process").Item("UserDomain"))

Dim g_SysProcVar
g_SysProcVar = UCase(objShell.Environment.Item("Processor_Architecture"))

Dim sSAFETOOLS
If (g_SysProcVar = "IA64") Then
    sSAFETOOLS = "c:\windows\bricktools"
Else
    sSAFETOOLS = "c:\winnt\bricktools"
End If

'================== Ensure this script is Run properly =======================
If (LCase(Right(Wscript.FullName,11)) <> "cscript.exe") Then
    WScript.Echo "This file must be executed using cscript.exe...please rerun."
    QuitProgram(1)     ' script was not executed using cscript
End If

If WScript.Version < CInt(5.0) Then
    WScript.Echo "This file must be executed using WSH 5.0 or greater"
    QuitProgram(1)     ' script was not executed on proper cscript version
End If

If (g_SysDrive = sSAFEDRIVE) Then
    Wscript.Echo "Cannot use POPS 2.0 to modify your SafeOS...please boot to a testOS."
    QuitProgram(1)     ' safeOS drive selected for modification
End If

'================== Execution of Main Function ===============================
' Global Variables
Dim objXML, g_XMLRoot
Dim g_POPSFile
Dim g_sPFile
Set g_XMLRoot = Nothing

Main()
QuitProgram(0)

Sub Main
    Const iSLEEPTIME = 15000
    Const iRETRIES = 3

    Dim i, j, k, arg
    Dim objSysEnv, objSysInfo
    Dim strRun, strNoRun, sContinueAction
    Dim aRun, aNoRun, aVars
    Dim bExecActions, f, bXMLExist
    Dim iOSVer, iOSLang
    Dim XMLEntryNodes, XMLActionNode

    strRun          = Empty
    strNoRun        = Empty
    sContinueAction = Empty
    g_sPFile        = Empty

    '============== Create or Append to Logging File =========================
    If Not objFSO.FolderExists(g_SysDrive & "\logs") Then
        objFSO.CreateFolder(g_SysDrive & "\logs")
    End If
    On Error Resume Next
    If Not objFSO.FileExists(g_SysDrive & "\logs\pops.log") Then
        Set g_POPSFile = objFSO.OpenTextFile(g_SysDrive & "\logs\pops.log",iFORWRITING,bCREATEDOC)
        g_POPSFile.Close
    Else
        EchoAndLog ""
    End If
    If (Err <> 0) Then
        WScript.Echo Err & " " & Err.Description
        WScript.Echo "Cannot overwrite " & g_SysDrive & "\logs\pops.log...please check locks on file."
        QuitProgram(3)
    End If
    On Error goto 0
    EchoAndLog "Started " & WScript.ScriptFullName & " - " & Date & " " & Time

    '============== Get variable inputs from user input ======================
    For Each arg In WScript.Arguments
        If Instr(arg,"?") Then
            Usage()
            QuitProgram(0)
        End If
        aVars = Split(arg,":",2)
        Select Case UCase(aVars(0))
            Case "/XML"
                g_sPFile        = UCase(aVars(1))
            Case "/RUN"
                strRun          = UCase(aVars(1))
            Case "/NORUN"
                strNoRun        = UCase(aVars(1))
            Case "/CONT"
                sContinueAction = UCase(aVars(1))
        End Select
    Next
    ' Delete any existing link to this file in startup menu
    Link sSAFEDEL & "\POPS_Relaunch.cmd","STARTUP","delete"

    ' Make sure this is good to run things
    If (strNoRun <> Empty) And (strRun <> Empty) Then
        EchoAndLog Space(3) & "You cannot use the '/run' and '/norun' flags together...please choose one."
        QuitProgram(1)     ' can only use '/run' or '/norun' independently
    End If
    If (g_SysDomain <> "SMX") Then
        EchoAndLog Space(3) & "Establishing Credentials to Necessary Servers"
        objShell.Run "cscript //nologo " & sSAFEKEEP & "\set.vbs smx,redmond",iWINMIN,bWINWAIT
    End If

    '============== Run Basic O/S setup steps =================================
    EchoAndLog Space(3) & "Creating Icons and Environmental Variables..."
    ' Create shortcut(s) to file(s)
    Link sSAFEKEEP & "\Refresh_" & Left(g_SysDrive,1) & ".cmd","Desktop","create"
    ' Figure out basic stats of System
    Dim oLocator, oServices, oInfoSet
    Set oLocator = CreateObject("Wbemscripting.SWbemlocator")
    Set oServices = oLocator.ConnectServer(".", "root\CIMv2")
    Set oInfoSet = oServices.InstancesOf("Win32_OperatingSystem")

    For Each i In oInfoSet
    'For Each i In GetObject("winmgmts:").InstancesOf("Win32_OperatingSystem")
        iOSVer      = Left(i.Version,3)
        iOSLang     = i.OSLanguage
    Next

    ' Add env. vars to System
    On Error Resume Next
    Set objSysEnv = objShell.Environment("SYSTEM")
    Set objSysInfo = CreateObject("ADSystemInfo")
    objSysEnv(FQDN_VAR) = objSysInfo.DomainDNSName
    Set objSysInfo = Nothing
    objSysEnv(TOOL_VAR) = sTOOLSRV
    On Error Goto 0
    Select Case iOSVer
        Case "5.0"
            objSysEnv(OSVER_VAR)  = "W2K"
            objSysEnv(SYM_VAR)    = "SRV*\\smx.net\symbols\symbols;SRV*\\symbols\symbols"
        Case "5.1"
            objSysEnv(OSVER_VAR)  = "WINXP"
            objSysEnv(SYM_VAR)    = "SRV*\\smx.net\symbols\symbols;SRV*\\symbols\symbols"
        Case "5.2"
            objSysEnv(OSVER_VAR)  = "W2K3"
            objSysEnv(SYM_VAR)    = "SRV*\\smx.net\symbols\symbols;SRV*\\symbols\symbols"
        Case Else
            EchoAndLog Space(3) & "Something has gone wrong...could not find an O/S."
            QuitProgram(3)     ' version of sOS returned is invalid...stop execution
    End Select
    ' http://www.microsoft.com/globaldev/reference/winxp/xp-lcid.mspx
    Select Case CInt(iOSLang)
        Case 1046                          'Brazilian
            objSysEnv(OSLANG_VAR) = "BR"
        Case 2052                          'Chinese (Simplified)
            objSysEnv(OSLANG_VAR) = "CN"
        Case 1028                          'Chinese (Traditional/Taiwan)
            objSysEnv(OSLANG_VAR) = "TW"
        Case 3076                          'Chinese (Hong Kong)
            objSysEnv(OSLANG_VAR) = "HK"
        Case 1029                          'Czech
            objSysEnv(OSLANG_VAR) = "CS"
        Case 1033                          'English
            objSysEnv(OSLANG_VAR) = "EN"
        Case 1036                          'French
            objSysEnv(OSLANG_VAR) = "FR"
        Case 1031                          'German
            objSysEnv(OSLANG_VAR) = "DE"
        Case 1038                          'Hungarian
            objSysEnv(OSLANG_VAR) = "HU"
        Case 1040                          'Italian
            objSysEnv(OSLANG_VAR) = "IT"
        Case 1041                          'Japanese
            objSysEnv(OSLANG_VAR) = "JA"
        Case 1042                          'Korean
            objSysEnv(OSLANG_VAR) = "KO"
        Case 1045                          'Polish
            objSysEnv(OSLANG_VAR) = "PL"
        Case 2070                          'Portuguese
            objSysEnv(OSLANG_VAR) = "PT"
'       Case 10XX                          'Pseudo
'           objSysEnv(OSLANG_VAR) = "PS"
        Case 1049                          'Russian
            objSysEnv(OSLANG_VAR) = "RU"
        Case 1034                          'Spanish
            objSysEnv(OSLANG_VAR) = "ES"
        Case 1053                          'Swedish
            objSysEnv(OSLANG_VAR) = "SV"
        Case 1055                          'Turkish
            objSysEnv(OSLANG_VAR) = "TR"
    End Select

    ' Quit if they didn't want to run any XML commands
    If (g_sPFile = Empty) Then
        QuitProgram(0)
    End If
    ' Ensure XML exists
    For f = 1 To iRETRIES
        If Not objFSO.FileExists(g_sPFile) Then
            bXMLExist = False
            WScript.Sleep(iSLEEPTIME)
        Else
            bXMLExist = True
            Exit For
        End If
    Next
    If Not(bXMLExist) Then
        EchoAndLog Space(3) & "Cannot find " & g_sPFile & " to execute POPS 2.0" & _
        "...please verify file existence."
        QuitProgram(4)     ' could not find file g_sPFile
    End If
    ' Ensure that the user input an XML file
    If (Right(g_sPFile,4) <> ".XML") Then
        EchoAndLog Space(3) & "POPS 2.0 can only execute *.xml files..." & _
                   "please select another input (" & g_sPFile & ")."
        QuitProgram(1)     ' g_sPFile is not an XML file
    End If

    '============== Load/Verify XML file ======================================
    '- Note: in *.xml all Attribute Entries should be LCase
    '- Note: in *.xml all Action Node values must be UCase

    ' Update MSXML for XML Parsing...if necessary
    On Error Resume Next
    Set objXML = CreateObject("msxml2.DOMDocument")
    If (Err <> 0) Then
        EchoAndLog Space(3) & "Updating MSXML for - " & Err & ": " & Err.Description
        objShell.Run sPRODUCTSRV & "\msxml\msxml3sp1.exe /Q /C:" & _
                     Chr(34) & "msiexec /I msxml3.msi /qn" & Chr(34),iWINMIN,bWINWAIT
        WScript.Sleep(2000)     ' sleep 2 seconds
        Set objXML = CreateObject("msxml2.DOMDocument")
    End If
    On Error goto 0

    EchoAndLog Space(3) & "Parsing/Modifying XML file..." & g_sPFile
    ' Ensure XML is completely loaded before continuing
    objXML.async = False
    objXML.load(g_sPFile)
    If (objXML.parseError.errorcode <> 0) Then
        EchoAndLog Space(3) & "Error in parsing " & g_sPFile & "...please contact " & sFAILALIAS & "."
        QuitProgram(3)     ' XML code is bad
    End If
    ' Get base heirarchy from XML file
    Set g_XMLRoot = objXML.documentElement

    EchoAndLog Space(3) & "Putting user input into XML..."
    ' For each input argument, update the XML as appropriate
    For Each arg In WScript.Arguments
        aVars = Split(arg,":",2)
        If (UCase(aVars(0)) <> "/XML") And (UCase(aVars(0)) <> "/CONT") And _
           (UCase(aVars(0)) <> "/RUN") and (UCase(aVars(0)) <> "/NORUN") Then
            On Error Resume Next
            If (aVars(1) = Empty) Then
                EchoAndLog Space(6) & "You must put in some sort of parameter for the " & UCase(aVars(0)) & " flag."
                QuitProgram(1)     ' user input to flag contains no data
            End If
            On Error goto 0
            UpdateXMLParam aVars(0),aVars(1)
        End If
    Next

    '============== Start executing XML Action(s) =============================
    EchoAndLog Space(3) & "Running XML Actions..."
    Set XMLEntryNodes = g_XMLRoot.selectNodes("entry")
    ' Parse user input list of Actions to Run; turn off all and then reenable only chosen actions
    If (strRun <> Empty) Then
        aRun = Split(strRun,",",iSUBSTRINGS)
        For i = 0 To (XMLEntryNodes.length - 1)
            XMLEntryNodes.item(i).attributes.getNamedItem("enable").text = "off"
        Next
        For i = LBound(aRun) To UBound(aRun)
            Set XMLActionNode = g_XMLRoot.selectSingleNode("entry[@action='" & aRun(i) & "']")
            If (XMLActionNode Is Nothing) Then
                EchoAndLog Space(6) & "Could not locate Entry/Action " & aRun(i) & " in " & g_sPFile
                Usage()
                QuitProgram(1)
            End If
            XMLActionNode.attributes.getNamedItem("enable").text = "on"
        Next
    End If
    ' Parse user input list of Actions NOT to Run; turn off selected actions
    If (strNoRun <> Empty) Then
        aNoRun = Split(strNoRun,",",iSUBSTRINGS)
        For i = LBound(aNoRun) To UBound(aNoRun)
            Set XMLActionNode = g_XMLRoot.selectSingleNode("entry[@action='" & aNoRun(i) & "']")
            If (XMLActionNode Is Nothing) Then
                EchoAndLog Space(6) & "Could not locate Entry/Action " & aNoRun(i) & " in " & g_sPFile
                Usage()
                QuitProgram(1)
            End If
            XMLActionNode.attributes.getNamedItem("enable").text = "off"
        Next
    End If

    ' Find XMLEntryNode in XML and run function to execute it. (bExecAction keeps track of continue point - if returning from reboot)
    bExecActions = (sContinueAction = Empty)
    For i = 0 To (XMLEntryNodes.length - 1)
        If (bExecActions = False) Then
            ' Set bExecActions to True, if the current Action was the continuation point (so that the next Action is executed)
            bExecActions = (sContinueAction = XMLEntryNodes.item(i).attributes.getNamedItem("action").text)
        Else
            ' Execute Action if it is supposed to be run
            ExecuteXMLAction(XMLEntryNodes.item(i))
        End If
    Next

    ' Setup shortcuts for some basic Actions
    Link g_SysRoot & "\bricktools\background.cmd","STARTUP","create"
    Link g_SysRoot & "\bricktools\remotecmd.cmd","STARTUP","create"
End Sub

'================== Execute the XML Action specified ==========================
' - Create a CMD line from the XML info and execute it
Sub ExecuteXMLAction(XMLEntryNode)
    Dim x, y, z, arg
    Dim REPOPSFile, objSysInfo
    Dim XMLEnable, XMLDesc, XMLAction, XMLReboot
    Dim XMLCmdNodes, XMLParameterNodes, XMLKey, XMLValue, XMLErrorChk
    Dim sTaskCMD, sOSDomain, sOSDomainFQDN
    Dim aVars
    Dim iErrorCode

    XMLEnable = LCase(XMLEntryNode.attributes.getNamedItem("enable").text)
    XMLDesc   = LCase(XMLEntryNode.attributes.getNamedItem("desc").text)
    XMLAction = LCase(XMLEntryNode.attributes.getNamedItem("action").text)
    XMLReboot = LCase(XMLEntryNode.attributes.getNamedItem("reboot").text)

    If (XMLEnable = "on") Then
        EchoAndLog Space(6) & "[" & UCase(XMLAction) & "] --> " & XMLDesc
        If (XMLReboot = "yes") Then
            ' Action will reboot - create link file in startup to continue running pops
            arg = "cscript //nologo " & WScript.ScriptFullName
            For Each x In WScript.Arguments
                aVars = Split(x,":",2)
                If (UCase(aVars(0)) <> "/CONT") Then
                    arg = arg & " " & aVars(0) & ":" & Chr(34) & UCase(aVars(1)) & Chr(34)
                End If
            Next
            arg = arg & " /CONT:" & XMLAction
            Set REPOPSFile = objFSO.OpenTextFile(sSAFEDEL & "\POPS_Relaunch.cmd",iFORWRITING,bCREATEDOC)
            REPOPSFile.WriteLine "@echo OFF"
            REPOPSFile.WriteLine "if defined ECHO (echo %ECHO%)"
            REPOPSFile.WriteLine "set iCOUNT=1"
            REPOPSFile.WriteLine
            REPOPSFile.WriteLine ":Loop"
            REPOPSFile.WriteLine "if not exist " & sTOOLSRV & "\pops\pops.vbs ("
            REPOPSFile.WriteLine "    cscript //nologo " & sSAFEKEEP & "\set.vbs smx"
            REPOPSFile.WriteLine "    if {%iCOUNT%} geq {3} ("
            REPOPSFile.WriteLine "        echo Could not reach %TOOLSRV%\pops\pops.vbs file to execute it. " & _
                                          ">> %SYSTEMDRIVE%\logs\pops.log"
            REPOPSFile.WriteLine "    ) else ("
            REPOPSFile.WriteLine "        set /a iCOUNT=%iCOUNT%+1"
            REPOPSFile.WriteLine "        goto :Loop)"
            REPOPSFile.WriteLine ") else ("
            REPOPSFile.WriteLine "    sleep 45" 'This is to accommodate a WMI problem in AMD64 W2K3
            REPOPSFile.WriteLine "    " & arg
            REPOPSFile.WriteLine "    goto :EOF)"
            REPOPSFile.Close
            Link sSAFEDEL & "\POPS_Relaunch.cmd","STARTUP","create"
        End If

        Set XMLCmdNodes = XMLEntryNode.SelectNodes("command")
        ' Construct a CMDline string for each Command Node (one Action can have multiple commands)
        For x = 0 To (XMLCmdNodes.length - 1)
            WScript.Sleep(1500)     ' sleep 1 1/2 seconds
            ' Check current Domain name
            Set objSysInfo = CreateObject("ADSystemInfo")
            On Error Resume Next
            sOSDomain      = objSysInfo.DomainShortName
            sOSDomainFQDN  = objSysInfo.DomainDNSName
            Set objSysInfo = Nothing

            XMLValue    = XMLCmdNodes.item(x).attributes.getNamedItem("value").text
            XMLKey      = XMLCmdNodes.item(x).attributes.getNamedItem("key").text
            On Error Resume Next
            XMLErrorChk = LCase(XMLCmdNodes.item(x).attributes.getNamedItem("errorchk").text)
            On Error goto 0
            Set XMLParameterNodes = XMLCmdNodes.item(x).SelectNodes("parameter")
            ' Use Command Node key value to determine first part of cmd string
            Select Case LCase(XMLKey)
                Case "vbs"
                    If (g_SysProcVar = "AMD64") Then
                        sTaskCMD = g_SysRoot & "\SysWOW64\cmd.exe /c cscript //nologo "
                    Else
                        sTaskCMD = "cmd.exe /c cscript //nologo "
                    End If
                Case "wsh"
                    If (g_SysProcVar = "AMD64") Then
                        sTaskCMD = g_SysRoot & "\SysWOW64\cmd.exe /c cscript //nologo "
                    Else
                        sTaskCMD = "cmd.exe /c cscript //nologo "
                    End If
                Case "wsf"
                    If (g_SysProcVar = "AMD64") Then
                        sTaskCMD = g_SysRoot & "\SysWOW64\cmd.exe /c cscript //nologo "
                    Else
                        sTaskCMD = "cmd.exe /c cscript //nologo "
                    End If
                Case "exe"
                    If (g_SysProcVar = "AMD64") Then
                        sTaskCMD = g_SysRoot & "\SysWOW64\cmd.exe /c "
                    Else
                        sTaskCMD = "cmd.exe /c "
                    End If
                Case "cmd"
                    If (g_SysProcVar = "AMD64") Then
                        sTaskCMD = g_SysRoot & "\SysWOW64\cmd.exe /c "
                    Else
                        sTaskCMD = "cmd.exe /c "
                    End If
                Case "bat"
                    If (g_SysProcVar = "AMD64") Then
                        sTaskCMD = g_SysRoot & "\SysWOW64\cmd.exe /c "
                    Else
                        sTaskCMD = "cmd.exe /c "
                    End If
                Case Else
                    EchoAndLog Space(3) & UCase(XMLKey) & " is an unrecognized command type" & _
                               "...please check your file extension."
                    QuitProgram(1)     ' invalid file type identifier in sPOPS XML
            End Select
            ' Append Command Node value to cmd string
            sTaskCMD = sTaskCMD & XMLValue
            ' Append Parameter Node values to cmd string
            For y = 0 To (XMLParameterNodes.length - 1)
                sTaskCMD = sTaskCMD & " " & XMLParameterNodes.item(y).attributes.getNamedItem("value").text
            Next
            ' Create a Regular Expression to replace %*%
            sTaskCMD = ReplaceInLine(sTaskCMD,"%TOOLSRV%",sTOOLSRV)
            sTaskCMD = ReplaceInLine(sTaskCMD,"%PRODUCTSRV%",sPRODUCTSRV)
            sTaskCMD = ReplaceInLine(sTaskCMD,"%OSSRV%","\\smx.net\os")
            sTaskCMD = ReplaceInLine(sTaskCMD,"%DOMAIN%",sOSDomain)
            sTaskCMD = ReplaceInLine(sTaskCMD,"%DOMAINFQDN%",sOSDomainFQDN)
            EchoAndLog Space(9) & sTaskCMD

            ' Execute created CMD line; DITSpause- iErrorCode not zero for *.exe & Err not zero for *.else
            On Error Resume Next
            Err.Clear
            iErrorCode = objShell.Run(sTaskCMD,iWINMIN,bWINWAIT)
            If (Err <> 0) Or ((iErrorCode <> 0) And (iErrorCode <> 5)) Then
                If (XMLErrorChk <> "off") Then
                    EchoAndLog Space(9) & Err & " ErrorLevel -    " & iErrorCode & " ErrorCode"
                    objShell.Run sSAFETOOLS & "\ditspause.exe " & _
                                 Chr(34) & "POPS error running - " & sTaskCMD & Chr(34),iWINMIN,bWINWAIT
                End If
            End If
            On Error goto 0
        Next
        If (XMLReboot = "yes") Then
        ' Action will reboot - shutdown POPS; await reboot for retrigger
            EchoAndLog Space(3) & "Closing down POPS - awaiting ReBoot (" & XMLAction & ")"
            WScript.Sleep 5000
            QuitProgram(0)
        End If
    End If
End Sub

'================== Modify and Input CMD line =================================
Function ReplaceInLine(sLine,sPattern,sReplace)
    Dim RegEx

    Set RegEx        = New RegExp
    RegEx.Pattern    = sPattern
    RegEx.IgnoreCase = True
    RegEx.Global     = True
    ReplaceInLine = RegEx.Replace(sLine,sReplace)
End Function

'================== Change XML parameters info ================================
' - Integrate the User input to change Action parameters as allowed in the XML (pwrite value only)
Sub UpdateXMLParam(sEntry,sParameters)
    Dim XMLActionNode, XMLParameterNode

    ' Strip off leading '/' and tailing ':' from Action input
    sEntry = UCASE(Mid(sEntry,2))
    Set XMLActionNode = g_XMLRoot.selectSingleNode("entry[@action='" & sEntry & "']")
    If (XMLActionNode Is Nothing) Then
        EchoAndLog Space(6) & "Could not locate Entry/Action " & sEntry & " in " & g_sPFile
        Usage()
        QuitProgram(1)     ' invalid Action request from user to run from g_sPFile
    End If

    Set XMLParameterNode = XMLActionNode.selectSingleNode("command/parameter[@key='pwrite']")
    If (XMLParameterNode Is Nothing) Then
        EchoAndLog Space(6) & sEntry & " action does not allow parameter changes..."
    Else
        XMLParameterNode.attributes.getNamedItem("value").text = sParameters
        EchoAndLog Space(6) & "Updated action " & sEntry & " with data: " & sParameters
    End If
End Sub

'================== Create/delete ShortCut Link to File ==================
' Create a shortcut link to another file
Sub Link(sFileSrc,sFileDest,sFileAction)
    Dim objFileLink
    Dim sFileBaseName

    ' Retrieve name of file (no directory path or extension info)
    sFileBaseName = objFSO.GetBaseName(sFileSrc)
    If Not(objFSO.FileExists(sFileSrc)) And (UCase(sFileAction) = "CREATE") Then
        EchoAndLog Space(3) & "Could not create link to - " & sFileSrc
        Exit Sub
    End If
    ' Determine the path to desired link location
    Select Case UCase(sFileDest)
        Case "DESKTOP"
            sFileDest = objShell.SpecialFolders("AllUsersDesktop")
        Case "STARTUP"
            sFileDest = objShell.SpecialFolders("AllUsersStartUp")
        Case "STARTMENU"
            sFileDest = objShell.SpecialFolders("AllUsersPrograms")
    End Select
    If Not(objFSO.FolderExists(sFileDest)) And (UCase(sFileAction) = "CREATE") Then
        EchoAndLog Space(3) & "Cannot find the folder - " & sFileDest
        QuitProgram(2)     ' could not find the folder to create the link in
    End If

    ' Create shortcut file at desired link location
    If (UCase(sFileAction) = "CREATE") Then
        Set objFileLink = objShell.CreateShortcut(sFileDest & "\" & sFileBaseName & ".lnk")
        objFileLink.TargetPath = sFileSrc
        objFileLink.WindowStyle = 1
        objFileLink.Save
    End If
    ' Delete shortcut file from desired link location
    If (UCase(sFileAction) = "DELETE") Then
        sFileSrc = Chr(34) & sFileDest & "\" & sFileBaseName & ".lnk" & Chr(34)
        objShell.Run "cmd.exe /c del /q /a " & sFileSrc,iWINMIN,bWINWAIT
    End If
End Sub

'================== Show Usage ===========================================
Sub Usage()
    Dim x, y, z
    Dim XMLEntryNodes, XMLDescNode, XMLActionNode

    WScript.Echo
    WScript.Echo
    WScript.Echo "cscript //nologo pops.vbs [/xml:] [/run:] [/cont:]"
    WScript.Echo "--- Log at c:\logs\pops.log ---"
    WScript.Echo
    WScript.Echo "    No required paramters."
    WScript.Echo "    Note: this script will automatically set itself up for continued"
    WScript.Echo "          execution following a reboot (when reboot attribute is set in the XML)"
    WScript.Echo "    Examples:"
    WScript.Echo "      pops.vbs /xml:" & sTOOLSRV & "\pops\test_env.xml"
    WScript.Echo "         Run all tasks listed in " & sTOOLSRV & "\pops\test_env.xml."
    WScript.Echo
    WScript.Echo "      pops.vbs /xml:c:\delete\mine.xml"
    WScript.Echo "               /run:foo,goo,boo"
    WScript.Echo "         Run tasks foo,goo,and boo from the XML file Mine.xml."
    WScript.Echo
    WScript.Echo "      pops.vbs /xml:" & sTOOLSRV & "\pops\test_env.xml"
    WScript.Echo "               /cont:copyidw"
    WScript.Echo "               /bf:" & Chr(34) & "/add:/3gb,/debug2" & Chr(34)
    WScript.Echo "               /norun:logon,addadmin,urtsdk"
    WScript.Echo "         Do not run tasks logon, addadmin, and urtsdk from the test_env.xml file."
    wscript.Echo "         Run all other tasks that occur after CopyIDW in the test_env.xml file."
    WScript.Echo "         Use the parameter input" & Chr(34) & "/add:/3gb,/debug2" & Chr(34)
    WScript.Echo "         when running the BF task."
    WScript.Echo
    WScript.Echo
    WScript.Echo "Returned ErrorCodes:"
    WScript.Echo "   0 - Nothing is wrong"
    WScript.Echo "   1 - User input is wrong"
    WScript.Echo "   2 - a file/folder could not be located"
    WScript.Echo "   3 - some action could not be executed"
    WScript.Echo "       ...could not contact CDS,could not parse XML file,etc."
    WScript.Echo
    WScript.Echo "Inputs:"
    WScript.Echo "   /xml:        - xml file to read/Execute from"
    WScript.Echo "                  default:" & sTOOLSRV & "\pops\test_env.xml"
    WScript.Echo "   /run:        - comma delimited list of specific tasks to run from XML file"
    WScript.Echo "                  no default"
    WScript.Echo "   /norun:      - comma delimited list of specific tasks NOT to run from XML file"
    WScript.Echo "                  no default"
    WScript.Echo "   /cont:       - task to continue action execution from after a reboot"
    WScript.Echo "                  Note: script resumes after reboot at the XML entry following task specified"
    WScript.Echo "                  no default"
    WScript.Echo "   /%TaskName%: - quoted string to add input parameter(s) to a task"
    WScript.Echo "                  ** availability based on XML construction; no default"
    ' Gather info from the XML and write out to the CMD window- for Usage purposes
    ' -  only operates when the XML file has been loaded into memory
    If Not(g_XMLRoot Is Nothing) Then
        WScript.Echo Space(18) & "=== " & UCase(g_sPFile) & " Specific ==="
        Set XMLEntryNodes = g_XMLRoot.selectNodes("entry")
        For x = 0 To (XMLEntryNodes.length - 1)
            XMLDescNode = XMLEntryNodes.item(x).attributes.getNamedItem("desc").text
            XMLActionNode = XMLEntryNodes.item(x).attributes.getNamedItem("action").text
            WScript.Echo Space(22) & XMLActionNode & vbtab & "- " & XMLDescNode
        Next
        WScript.Echo Space(18) & "=== " & UCase(g_sPFile) & " Specific ==="
    End If
    WScript.Echo
    WScript.Echo
End Sub

'================== Append info to POPS file =================================
' Append information to the log file at sSAFEKEEP\POPS.log
Sub EchoAndLog(sWriteOut)
    Set g_POPSFile = objFSO.OpenTextFile(g_SysDrive & "\logs\pops.log",iFORAPPENDING,bNOCREATEDOC)
    WScript.Echo sWriteOut
    g_POPSFile.WriteLine sWriteOut
    g_POPSFile.Close
End Sub

'================== Quit Program and Close Down ==============================
' Quit script, close objects, and return iError as ErrorLevel
Sub QuitProgram(iError)
    Set objFSO        = Nothing
    Set objShell      = Nothing
    Set objXML        = Nothing

    WScript.Quit(iError)
End Sub