' ============================================================================
' Copyright . 2004
'
' Module : Oasis.vbs
'
' Summary: Designed to refresh various testOS on a machine
' Dependencies: local bricktools folder; set.vbs; connections to sTOOLSRV and sOSSRV; MSXML 3.X
'          ErrorCodes:
'            0 - Nothing is wrong
'            1 - User input is wrong
'            2 - a file/folder could not be located
'            3 - some action could not be executed
'                ...could not contact CDS,could not parse XML file,etc.
'
' History: (08/10/2003) Jennla   - Initial coding
'          (11/04/2003) Jennla   - Changed the way default POPS files work
'          (11/18/2003) GlennLaV - sleep in refresh file - extended more
'          (11/24/2003) Deangj   - Added ASPNET as default installed component
'          (12/08/2003) Jheat    - General Cleanup
'          (01/22/2004) Deangj   - General Cleanup for IA64 support
'          (01/31/2004) Deangj   - Added call to WriteUnicodeDomain.exe to support
'                                  localized domains, removed calling Pops.vbs with the /chkdomin:
'          (02/05/2004) Jheat    - Updated to support NT 4 Domain & Delete Machine account info correctly.
'                                  Also updated a bunch of coding standards stuff, etc.
'          (02/10/2004) deangj   - Made sure that DITS resume script always runs And
'                                  added support for Upgrades running DITS resume script
'          (02/13/2004) deangj   - On upgrade Oasis does not write Refresh_*.cmd, and changed the retry count
'                                  in Refresh_*.cmd
'          (03/02/2004) deangj   - Added an if to see if the version is pro and if so set the upgrade os to winxp
'          (03/09/2004) deangj   - Added Create of HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Setup
'                                  SourcePath and ServicePackSourcePath with sSrcPath as the value
'          (03/10/2004) deangj   - Install BITS W2K3 Components by default
'          (03/16/2004) deangj   - Install MSXML3 if not present
'                                  If MSMQ is not an OS component, tell POPS not to run MSMQ config.
'          (03/29/2004) deangj   - delete c:\keep\cdscom.dll
'          (04/28/2004) deangj   - added support for new 5i Smart array drivers bug # 3426
'          (05/14/2004) pkong    - Add support for Dell Perc 4 array controller
'          (05/22/2004) Jheat    - Removed logic surrounding skus for IA64 and AMD64 as they are all the same PID.
'          (07/26/2004) PKong    - Add support for Promise FastTrak TX2 array controller
' ============================================================================
Option Explicit

' Common Variables
Const sUSERPWR      = "asttest"
Const sADMNUSER     = "goback"
Const sPWPWR        = "Triton#01"
Const sPWADMIN      = "SMX#2001"
Const sFAILALIAS    = "SMXOASIS"

' Server/Folder Names
Const sTOOLSRV      = "\\smx.net\tools"
Const sOSSRV        = "\\smx.net\os"
Const sSAFEDRIVE    = "C:"
Const sSAFEKEEP     = "c:\keep"
Const sSAFEDEL      = "c:\delete"

' Method Argument Inputs
Const iFORREADING   = 1
Const iFORWRITING   = 2
Const iFORAPPENDING = 8
Const iWINMIN       = 2
Const iSUBSTRINGS   = -1
Const bTRISTATETRUE = True
Const bCREATEDOC    = True
Const bNOCREATEDOC  = False
Const bWINWAIT      = True
Const bOVERWRITE    = True
Const bDELETEREADONLY = True
Const iWAIT         = 30000

'================== Create Common Objects ====================================
Dim objShellLink, iErr, g_RegSourcePath

Dim objFSO
Set objFSO   = CreateObject("Scripting.FileSystemObject")

Dim objShell
Set objShell = CreateObject("WScript.Shell")

On Error Resume Next
Dim objXML
Set objXML   = CreateObject("msxml2.DOMDocument")
If (Err <> 0) Then
    Dim XMLCMD
    XMLCMD = "\\smx.net\products\msxml\msxml3sp1.exe /Q /C:" & Chr(34) & "msiexec.exe /I msxml3.msi /qn" & Chr(34)
    iErr = objShell.Run(XMLCMD, iWINMIN, bWINWAIT)
    If iErr <> 0 Then
        WScript.Echo "Unable to install MSXML 3.0"
        QuitProgram(1)
    End If
    Set objXML   = CreateObject("msxml2.DOMDocument")
End If
On Error goto 0

Dim RE
Set RE = New RegExp
RE.IgnoreCase = True

' Common System Variables
Dim g_SysDrive
g_SysDrive   = UCase(objShell.Environment("process").Item("SystemDrive"))

Dim g_SysRoot
g_SysRoot    = UCase(objShell.Environment("process").Item("SystemRoot"))

Dim g_SysName
g_SysName    = UCase(objShell.Environment("process").Item("ComputerName"))

Dim g_SysDomain
g_SysDomain  = UCase(objShell.Environment("process").Item("UserDomain"))

Dim g_SysProcVar, g_SysProcLegacy
g_SysProcVar = UCase(objShell.Environment.Item("Processor_Architecture"))
Select Case g_SysProcVar
    Case "X86"
        g_SysProcLegacy = "I386"
    Case "IA64"
        g_SysProcLegacy = "IA64"
    Case "AMD64"
        g_SysProcLegacy = "AMD64"
End Select

'================== Ensure this script is Run properly =======================
If (LCase(Right(Wscript.FullName,11)) <> "cscript.exe") Then
    WScript.Echo "This file must be executed using cscript.exe...please rerun."
    QuitProgram(1)     ' script was not executed using cscript
End If

If WScript.Version < CInt(5.0) Then
    WScript.Echo "This file must be executed using WSH 5.0 or greater"
    QuitProgram(1)     ' script was not executed on proper cscript version
End If

'================== Execution of Main Function ===============================
' Global Variables
Dim g_XMLRoot
Dim g_OasisFile, g_sCompXML, g_sOasisXML
g_sCompXML    = sTOOLSRV & "\oasis\oscomp.xml"
g_sOasisXML   = sTOOLSRV & "\pops\base_env.xml"
Set g_XMLRoot = Nothing

If (WScript.Arguments.Count = 0) Then
    WScript.Echo "Error in number of input parameters...please rerun."
    Usage()
    QuitProgram(1)     ' user # if inputs is incorrect
End If

Dim g_sTestOS, g_sOSDrive, g_sOSFormat ,g_sOSName, g_sOSDomain, g_sOSLang, g_sOSType, g_sOSVersion, g_sOSAdd
Dim g_sOSDomainEN, g_sOSDomainLOC, g_sOSDomainFQDN, g_sOUContainer, g_sOSBuildOrig, g_sSP, g_strOSComp
Dim g_iOSBuild, g_bOSUpgrade, g_bOSComp, g_bOSAdd, g_bWkgp

Main()
QuitProgram(0)

Sub Main
    Dim i, j, k, arg
    Dim RefreshFile, TempFile
    Dim sSrcPath, sPidKey, sDesktop, sPW, sLoc_ID, sLang_Grp, sDnsCMD
    Dim aOSComp, aVars
    Dim iError
    Const sSAFESTARTUP = "C:\Documents and Settings\All Users\Start Menu\Programs\Startup"

    '============== Create or Append to Logging File =========================
    If objFSO.FileExists("c:\logs\oasis.log") Then
        Set g_OasisFile = objFSO.OpenTextFile("c:\logs\oasis.log",iFORREADING,bNOCREATEDOC,bTRISTATETRUE)
        Do While g_OasisFile.AtEndOfStream <> True
            arg = g_OasisFile.ReadLine
        Loop
    End If
    ' Append to file if continuing same OASIS run after a reboot...else create a new file
    On Error Resume Next
    If InStr(LCase(arg),"rebooting to safeos") Then
        EchoAndLog ""
    Else
        Set g_OasisFile = objFSO.OpenTextFile("c:\logs\oasis.log",iFORWRITING,bCREATEDOC,bTRISTATETRUE)
        g_OasisFile.Close
    End If
    If (Err <> 0) Then
        WScript.Echo "Cannot overwrite c:\logs\oasis.log...please check locks on file."
        QuitProgram(3)
    End If
    On Error goto 0

    EchoAndLog "Started " & WScript.ScriptFullName
    EchoAndLog Date & " " & Time
    If (g_SysDomain <> "SMX") Then
        EchoAndLog Space(3) & "Establishing Credentials to Necessary Servers"
        objShell.Run "cscript //nologo " & sSAFEKEEP & "\set.vbs smx,redmond",iWINMIN,bWINWAIT
    End If

    '============== Get variable inputs from user input ======================
'---Required variable inputs---
    'g_sTestOS    - the OS to install (W2K,XP,WINXP,W2K3)
    'g_bOSUpgrade - signify if this is an OS upgrade (not a full OS install)
'---Optional variable inputs---
    'g_sOSDrive   - testOS drive to refresh
    'g_sOSFormat  - testOS drive format (NTFS,FAT,FAT32)
    'g_sOSName    - name of testOS
    'g_sOSDomain  - domain that the testOS will be joined to
    'g_sOSLang    - language of testOS
    'g_iOSBuild   - OS build number
    'g_sOSType    - type of testOS (FRE,CHK)
    'g_sOSVersion - version of testOS (ADS,SRV,DTC,PRO,BLA)
    'g_strOSComp  - component installation data for oasis.txt
    'g_sOSAdd     - location of file to run after testOS install complete

    ' Defaults for all flags
    g_bOSUpgrade = False
    g_bOSComp    = False
    g_bOSAdd     = False
    g_sOSDrive   = UCase(g_SysDrive)
    g_sOSFormat  = Empty
    g_sOSName    = UCase(g_SysName)
    g_sOSDomain  = Empty
    g_sOSLang    = Empty
    g_iOSBuild   = "BLESSED"
    g_sOSType    = Empty
    g_sOSVersion = Empty
    g_strOSComp  = Empty
    g_sOSAdd     = Empty

    ' Set flags based on user inputs (overwrite defaults)
    ' - on Empty input resume default...except for '/comp:' and '/add:' flags
    For Each arg In WScript.Arguments
        If Instr(arg,"?") Then
            Usage()
            QuitProgram(0)
        End If
        aVars = Split(arg,":",2)
        Select Case UCase(aVars(0))
            Case "/DRIVE"
                g_sOSDrive   = UCase(aVars(1))
            Case "/FORMAT"
                g_sOSFormat  = UCase(aVars(1))
            Case "/NAME"
                g_sOSName    = UCase(aVars(1))
            Case "/DOMAIN"
                g_sOSDomain  = UCase(aVars(1))
            Case "/LANG"
                g_sOSLang    = UCase(aVars(1))
            Case "/LANGUAGE"
                g_sOSLang    = UCase(aVars(1))
            Case "/BUILD"
                g_iOSBuild   = UCase(aVars(1))
            Case "/TYPE"
                g_sOSType    = UCase(aVars(1))
            Case "/VER"
                g_sOSVersion = UCase(aVars(1))
            Case "/VERSION"
                g_sOSVersion = UCase(aVars(1))
            Case "/COMP"
                g_strOSComp  = UCase(aVars(1))
                g_bOSComp    = True
            Case "/COMPONENT"
                g_strOSComp  = UCase(aVars(1))
                g_bOSComp    = True
            Case "/ADD"
                g_sOSAdd     = UCase(aVars(1))
                g_bOSAdd     = True
            Case "/ADDITIONAL"
                g_sOSAdd     = UCase(aVars(1))
                g_bOSAdd     = True
            Case Else
                ' Assume the non-flagged input is the g_sTestOS
                If (InStr(aVars(0),"/") = False) Then
                    g_sTestOS = UCase(aVars(0))
                Else
                    EchoAndLog Space(3) & "Cannot find variable " & arg & "...please check spelling."
                    QuitProgram(1)     ' OASIS input flag invalid
                End If
        End Select
    Next

    If (g_SysDrive <> sSAFEDRIVE) And (g_bOSUpgrade = False) Then
        ' Use Patron system to check-out machine for OASIS during refresh (if not already checked-out)
        ' - Patron components must be initialized AFTER a URT install
        On Error Resume Next
        objShell.Run "gacutil.exe /i /nologo" & _
                     " %WINDIR%\bricktools\net\Infra.LibraryPatron.dll",iWINMIN,bWINWAIT
        objShell.Run "regasm.exe /nologo" & _
                     " %WINDIR%\bricktools\net\Infra.LibraryPatron.dll",iWINMIN,bWINWAIT
        objShell.Run "cscript //nologo %WINDIR%\bricktools\librarypatron.vbs " & _
                     "Checkout " & sFAILALIAS & " 1 " & _
                     Chr(34) & g_SysName & " - Refreshing TestO/S(" & g_sOSDrive & ") via OASIS 2.0" & _
                     Chr(34),iWINMIN,bWINWAIT
        On Error goto 0
    End If

    '============== Load Component XML file/ Verify Inputs =======================
    If Not objFSO.FileExists(g_sCompXML) Then
        EchoAndLog Space(3) & "Cannot find " & g_sCompXML & " to complete this task" & _
                   "...please verify connection to " & sTOOLSRV & "."
        QuitProgram(2)     ' could not find file g_sCompXML
    End If

    ' Ensure XML is completely loaded before continuing
    objXML.async = False
    objXML.load(g_sCompXML)
    If (objXML.parseError.errorcode <> 0) Then
        EchoAndLog Space(3) & "Error in parsing " & g_sCompXML & "...please contact " & sFAILALIAS & "."
        QuitProgram(3)     ' XML code is bad
    End If
    ' Convert memory loaded XML to lower case (create case insensitive env.)
    i = objXML.xml
    i = LCase(i)
    objXML.loadXML i
    ' Get base heirarchy from XML file
    Set g_XMLRoot = objXML.documentElement

    ' Validate Inputs
    VerifyInputs sSrcPath,sPidKey,sLang_Grp,sLoc_ID,aOSComp

    '============== Create Refresh File ======================================
    If Not(g_bOSUpgrade) Then
        EchoAndLog Space(3) & "Creating Refresh Icon w/ confirmed Inputs"
        Set RefreshFile = objFSO.OpenTextFile(sSAFEKEEP & "\Refresh_" _
            & Left(g_sOSDrive,1) & ".cmd",iFORWRITING,bCREATEDOC)
        RefreshFile.WriteLine "@echo OFF"
        RefreshFile.WriteLine "if defined ECHO (echo %ECHO%)"
        RefreshFile.WriteLine
        RefreshFile.WriteLine "set TestOS="    & g_sTestOS
        RefreshFile.WriteLine "set OSDrive="   & g_sOSDrive
        RefreshFile.WriteLine "set OSFormat="  & g_sOSFormat
        RefreshFile.WriteLine "set OSName="    & g_sOSName
        RefreshFile.WriteLine "set OSDomain="  & UCase(g_sOSDomain)
        RefreshFile.WriteLine "set OSLang="    & g_sOSLang
        RefreshFile.WriteLine "set OSBuild="   & g_sOSBuildOrig
        RefreshFile.WriteLine "set OSType="    & g_sOSType
        RefreshFile.WriteLine "set OSVersion=" & g_sOSVersion
        RefreshFile.WriteLine "set OSComp="    & Chr(34) & g_strOSComp & Chr(34)
        RefreshFile.WriteLine "set OSAdd="     & Chr(34) & g_sOSAdd & Chr(34)
        RefreshFile.WriteLine "REM        Variables used by ML 3.0: please do not change."
        If (g_SysDrive = sSAFEDRIVE) Then
            RefreshFile.WriteLine "REM        set MLsafeos=" & g_SysName
            RefreshFile.WriteLine "REM        set MLbuild=" & Left(g_iOSBuild,Len(g_iOSBuild) - Len(g_sSP))
            On Error Resume Next
            RefreshFile.WriteLine "REM        set MLsp=" & Right(g_sSP,1)
            On Error goto 0
        End If
        RefreshFile.WriteLine
        RefreshFile.WriteLine "echo Starting Refresh"
        RefreshFile.WriteLine "set iCOUNT=1"
        RefreshFile.WriteLine ":Loop"
        RefreshFile.WriteLine "if not exist " & sTOOLSRV & "\oasis\oasis.vbs ("
        RefreshFile.WriteLine "    cscript //nologo " & sSAFEKEEP & "\set.vbs smx,redmond"
        RefreshFile.WriteLine "        %WINDIR%\idw\sleep.exe 15"
        RefreshFile.WriteLine "    if {%iCOUNT%} geq {3} ("
        RefreshFile.WriteLine "        echo Could not reach " & sTOOLSRV & "\oasis\oasis.vbs file to execute it. " & _
                                       ">> c:\logs\oasis.log"
        RefreshFile.WriteLine "    ) else ("
        RefreshFile.WriteLine "        set /a iCOUNT=%iCOUNT%+1"
        RefreshFile.WriteLine "        %WINDIR%\idw\sleep.exe 10"
        RefreshFile.WriteLine "        echo Could not reach " & sTOOLSRV _
            & "\oasis\oasis.vbs - retrying >> c:\logs\oasis.log"
        RefreshFile.WriteLine "        goto :Loop)"
        RefreshFile.WriteLine ") else ("
        RefreshFile.WriteLine "    cscript //nologo " & WScript.ScriptFullName & _
                                                    " %TestOS%" & _
                                                    " /Drive:%OSDrive%" & _
                                                    " /Format:%OSFormat%" & _
                                                    " /Name:%OSName%" & _
                                                    " /Domain:%OSDomain%" & _
                                                    " /Lang:%OSLang%" & _
                                                    " /Build:%OSBuild%" & _
                                                    " /Type:%OSType%" & _
                                                    " /Ver:%OSVersion%" & _
                                                    " /Comp:%OSComp%" & _
                                                    " /Add:%OSAdd%"
        RefreshFile.WriteLine "    goto :EOF)"
        RefreshFile.Close
    End If
    ' Delete Refresh links in the safeOS StartUp Menu
    objShell.Run "cmd.exe /c del /q /a " & Chr(34) & sSAFESTARTUP & "\Refresh_*" & Chr(34) & ".cmd",iWINMIN,bWINWAIT

    '============== Operate/Modify TestOS ====================================
    If (g_SysDrive <> sSAFEDRIVE) And (g_bOSUpgrade = False) Then
        EchoAndLog Space(3) & "Rebooting to SafeOS (from " & g_SysDrive & ") to continue Installation"
        ' Create Refresh shortcut in C:\...StartUp Menu
        Link sSAFEKEEP & "\Refresh_" & Left(g_sOSDrive,1) & ".cmd",sSAFESTARTUP
        WScript.Sleep 10
        If Not objFSO.FileExists(sSAFESTARTUP & "\Refresh_" & Left(g_sOSDrive,1) & ".lnk") Then
            objShell.Run "%WINDIR%\bricktools\ditspause.exe " & Chr(34) & "SafeOS File Missing",iWINMIN,bWINWAIT
        End If
        ' Suspend DITS
        On Error Resume next
        objShell.Run "cscript //nologo " & "%WINDIR%\bricktools\SuspendDITS.wsf",iWINMIN,bWINWAIT
        On Error goto 0

        ' Reboot machine to safeOS
        objShell.Run "cscript //nologo " & sTOOLSRV & "\bricktools\common" & _
                     "\boot.vbs /boot:SafeOS /time:5 /reboot",iWINMIN,bWINWAIT
        QuitProgram(0)     ' natural quit due to testOS shutdown
    End If

    '============== Operate/Modify SafeOS ====================================
    If (g_SysDrive = sSAFEDRIVE) Then
        On Error Resume Next
        WScript.Sleep(iWAIT)
        objShell.Run "CMD /C regsvr32.exe " & sSAFEKEEP & "\cdscom.dll /u /s",iWINMIN,bWINWAIT
        objFSO.DeleteFile sSAFEKEEP & "\cdscom.dll",bDELETEREADONLY
        On Error goto 0
    End If
    ' Copy Tools and set default actions
    If (g_bOSUpgrade = False) Then
        EchoAndLog Space(3) & "Operating on SafeOS"
        On Error Resume Next
        objShell.Run sTOOLSRV & "\drop\copytools.cmd all /nopath",iWINMIN,bWINWAIT
        objFSO.CopyFile sTOOLSRV & "\oasis\set.vbs",sSAFEKEEP & "\",bOVERWRITE
        objFSO.CopyFile sTOOLSRV & "\oasis\setcred.cmd",sSAFEKEEP & "\",bOVERWRITE
        objShell.Run sTOOLSRV & "\bricktools\" & g_SysProcVar & "\chres.exe" & _
                     " /W:1024 /H:768 /C:16 /F:70",iWINMIN,bWINWAIT
        On Error goto 0
        objShell.Run "cscript //H:cscript //nologo //S",iWINMIN,bWINWAIT
    End If
    sPW = CDSPassword(g_sOSDomainLOC,sUSERPWR,sPWPWR)

    ' Create Refresh & folder/file manipulation
    Link sSAFEKEEP & "\Refresh_" & Left(g_sOSDrive,1) & ".cmd","Desktop"
    ' Delete/Create the c:\delete folder and startup refresh links...ignore errors
    On Error Resume Next
    objFSO.DeleteFolder(sSAFEDEL)
    objFSO.CreateFolder(sSAFEDEL)
    objFSO.DeleteFile objFSO.BuildPath(sSAFESTARTUP,"\Refresh_*.lnk"),True
    On Error Goto 0

    ' Delete old TestOS computer account name from domain(s)
    On Error Resume Next
    EchoAndLog Space(3) & "Cleaning up old computer account and DNS settings"
    sPW = CDSPassword("SMX",sADMNUSER,sPWPWR)
    sDnsCMD = "%WINDIR%\bricktools\RunasPwd.exe -u:" & sADMNUSER & "@" & "SMX" & ".NET" & _
              " -P:" & sPW & _
              " -e:" & Chr(34) & "cmd /c " & sTOOLSRV & "\oasis\ad_cleanup.cmd"
    objShell.Run sDnsCMD,iWINMIN,bWINWAIT
    On Error Goto 0

    ' Use Patron system to check-out machine for OASIS during refresh (if not already checked-out)
    ' - Patron components must be initialized AFTER a URT install
    On Error Resume Next
    EchoAndLog Space(3) & "Checking machine out from ML via Patron"
    objShell.Run "gacutil.exe /i /nologo" & _
                 " %WINDIR%\bricktools\net\Infra.librarypatron.dll",iWINMIN,bWINWAIT
    objShell.Run "regasm.exe /nologo" & _
                 " %WINDIR%\bricktools\net\Infra.librarypatron.dll",iWINMIN,bWINWAIT
    objShell.Run "cscript //nologo %WINDIR%\bricktools\librarypatron.vbs " & _
                 "Checkout " & sFAILALIAS & " 1 " & _
                 Chr(34) & g_SysName & " - Refreshing TestO/S(" & g_sOSDrive & ") via OASIS 2.0" & _
                 Chr(34),iWINMIN,bWINWAIT
    On Error goto 0

    '============== Generate Unattended Text file ============================
    EchoAndLog Space(3) & "Creating OASIS.txt file"

    ' Modify XML in memory based on Inputs
    sPW = CDSPassword(g_sOSDomainLOC,sUSERPWR,sPWPWR)
    ChangeXML "GUIUnattended","AdminPassword",sPWADMIN,"on"
    ChangeXML "UserData","ComputerName",g_sOSName,"on"
    ChangeXML "UserData","ProductID",Chr(34) & sPidKey & Chr(34),"on"
    ChangeXML "IEHardening","LocalIntranetSites",_
              Chr(34) & "file://smx.net/;file://smxfiles/;;http://" & g_sOSName & Chr(34),""
    ' Modify XML based on O/S install scenario
    If (g_bOSUpgrade = True) Then
        ChangeXML "Unattended","NTUpgrade","yes","on"
    Else
        ChangeXML "Unattended","NTUpgrade","no","on"
        ChangeXML "Unattended","OEMPreInstall","yes","on"
        ChangeXML "Unattended","OverwriteOEMfilesonUpgrade","yes","on"
        ChangeXML "RegionalSettings","LanguageGroup",sLang_Grp,"on"
        ChangeXML "RegionalSettings","InputLocale",sLoc_ID,"on"
        ' Determine and modify appropriate Domain info
        If (g_bWkgp = True) Then
            ChangeXML "Identification","JoinWorkGroup",g_sOSDomainLOC,"on"
        Else
            ChangeXML "Identification","JoinDomain",g_sOSDomainLOC & g_sOSDomainFQDN,"on"
            ChangeXML "Identification","CreateComputerAccountInDomain","Yes","on"
            ChangeXML "Identification","DomainAdmin",g_sOSDomainLOC & "\" & sUSERPWR,"on"
            ChangeXML "Identification","DomainAdminPassword",sPW,"on"
            ChangeXML "Identification","MachineObjectOU",Chr(34) & g_sOUContainer & Chr(34),"on"
        End If
    End If
    DetermineDrivers()

    ' Modify XML based on O/S Component Inputs
    If (g_strOSComp <> Empty) Then
        ' Iterate through the list of Component and 'turn them on' in the XML in memory
        ' - ignore errors since existence was already validated in VerifyInputs
        For i = LBound(aOSComp) To UBound(aOSComp)
            ChangeXML "NetOptionalComponents",aOSComp(i),"","on"
            ChangeXML "Components",aOSComp(i),"","on"
        Next
    End If

    ' Setup machine for g_sOSAdd execution after testO/S installation
    EchoAndLog Space(3) & "Setting up Additional files to execute after O/S installation."
    'Create temp file to add to GUIRunOnce Input (establish credentials and start PostOASIS execution)
    Set TempFile = objFSO.OpenTextFile(sSAFEDEL & "\temp.cmd",iFORWRITING,bCREATEDOC)
    Dim sRegPath
    sRegPath = sTOOLSRV & "\bricktools\" & g_SysProcVar & "\reg.exe"
    arg = sRegPath & " add HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Setup /v SourcePath" _
        & " /t REG_SZ /d " & Chr(34) & g_RegSourcePath & Chr(34) & " /f" & VbCrLf
    arg = arg & "   " & sRegPath & " add HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Setup /v" _
        & " ServicePackSourcePath /t REG_SZ /d " & Chr(34) & g_RegSourcePath & Chr(34) & " /f" & VbCrLf
    arg = arg & "    cscript //nologo " & sTOOLSRV & "\pops\pops.vbs"
    If (g_bOSUpgrade) Then
        arg = arg & " /xml:" & g_sOasisXML & " /norun:pops,patron_in,background,remotecmd" & _
                    ",MSMQ,URTSDK,ADDADMIN,LOGON,RBT"
    End If
    If (g_sOSAdd <> Empty) Then
        If Instr(LCase(g_sOSAdd),LCase(g_sOasisXML)) Then
            arg = arg & " /xml:" & g_sOSAdd
            If Not InStr(UCase(g_strOSComp), "MSMQ") Then
                arg = arg & " /norun:pops,enable_iis,MSMQ"
            Else
                arg = arg & " /norun:pops,enable_iis"
            End If
        Else
            arg = arg & " /xml:" & g_sOasisXML & _
                        " /pops:" & Chr(34) & "/xml:" & g_sOSAdd & Chr(34)
            If Not InStr(UCase(g_strOSComp), "MSMQ") Then
                arg = arg & " /norun:patron_in,video,background,remotecmd,enable_iis,MSMQ"
            Else
                arg = arg & " /norun:patron_in,video,background,remotecmd,enable_iis"
            End If
        End If
        If (g_bWkgp = False) Then
            arg = arg & " /logon:" & Chr(34) & sPW & " " & sUSERPWR & Chr(34) & _
                        " /addadmin:" & sUSERPWR & "-1"
        Else
            arg = arg & ",addadmin,logon"
        End If
    Else
        'Make sure that DITS resume script is run
        arg = arg & " /xml:" & g_sOasisXML & " /norun:pops,patron_in,video,background,remotecmd" & _
                    ",MSMQ,URTSDK,ADDADMIN,LOGON,RBT,REG_ICF,REG_IEHARDENING"
    End If
    TempFile.WriteLine "@echo OFF"
    TempFile.WriteLine "if defined ECHO (echo %ECHO%)"
    TempFile.WriteLine "set iCOUNT=1"
    TempFile.WriteLine
    TempFile.WriteLine ":Loop"
    TempFile.WriteLine "if not exist " & sTOOLSRV & "\pops\pops.vbs ("
    TempFile.WriteLine "    cscript //nologo " & sSAFEKEEP & "\set.vbs smx,redmond"
    TempFile.WriteLine "    if {%iCOUNT%} geq {3} ("
    TempFile.WriteLine "        echo Could not reach %TOOLSRV%\pops\pops.vbs file to execute it. " & _
                                ">> %SYSTEMDRIVE%\logs\pops.log"
    TempFile.WriteLine "    ) else ("
    TempFile.WriteLine "        set /a iCOUNT=%iCOUNT%+1"
    TempFile.WriteLine "        goto :Loop)"
    TempFile.WriteLine ") else ("
    TempFile.WriteLine "    " & arg
    TempFile.WriteLine "    goto :EOF)"
    TempFile.Close
    ChangeXML "GuiRunOnce","Command0",Chr(34) & sSAFEDEL & "\temp.cmd" & Chr(34),"on"
    EchoAndLog Space(6) & "Verified input 'Additional'..." & g_sOSAdd

    ' Write out modified XML into Oasis.txt
    CreateUnattendFile()

    'call WriteUnicodeDomain.exe to write the the Unicode domain correctly
    If Not objFSO.FileExists(sSAFEKEEP & "\WriteUnicodeDomain.exe ") Then
        objShell.Run "CMD /C xcopy " & sTOOLSRV & "\bricktools\" & g_SysProcVar _
            & "\NET\WriteUnicodeDomain.exe " & sSAFEKEEP & " /YR"
    End If
    Select Case UCase(g_sOSDomainEN)
        Case "JAPANESE"
            objShell.Run sSAFEKEEP & "\WriteUnicodeDomain.exe " & sSAFEDEL & "\Oasis.txt " & g_sOSDomainEN
        Case "GERMAN"
            objShell.Run sSAFEKEEP & "\WriteUnicodeDomain.exe " & sSAFEDEL & "\Oasis.txt " & g_sOSDomainEN
        Case "FRENCH"
            objShell.Run sSAFEKEEP & "\WriteUnicodeDomain.exe " & sSAFEDEL & "\Oasis.txt " & g_sOSDomainEN
    End Select

    '============== All Components Verified...Final Actions ==================
    ' - do not format drives if running an Upgrade scenario
    If (g_bOSUpgrade = False) Then
        WScript.Echo
        EchoAndLog   Space(3) & "About to Format TestOS " & g_sOSDrive
        WScript.Echo Space(3) & "hit CTRL+C to quit format..."
        WScript.Sleep(7000)     ' sleep 7 seconds

        ' Format TestOS Drive Partition - attempt 3 times before resort to manual intervention
        EchoAndLog Space(6) & "Formatting TestOS " & g_sOSDrive
        k = 0
        Do While (k < 3)
            iError = objShell.Run("cmd.exe /c (ECHO. && ECHO y)|label " & g_sOSDrive,iWINMIN,bWINWAIT)
            iError = iError + _
                     objShell.Run("cmd.exe /c (ECHO. && ECHO y)|format " & g_sOSDrive & " /q/u/x /FS:" & _
                                  g_sOSFormat & " /V:",iWINMIN,bWINWAIT)
            If (iError = 0) Then
                Exit Do
            End If
            k = k + 1
        Loop
        If (iError <> 0) Then
            EchoAndLog Space(6) & "Could not format " & g_sOSDrive & _
                       "...please format this drive manually and continue."
            objShell.Run "%WINDIR%\bricktools\ditspause.exe " & _
                         Chr(34) & "Could not format " & g_sOSDrive & _
                         "...please format this drive manually and continue." & Chr(34),iWINMIN,bWINWAIT
        End If     ' pause for investigation/fix...testOS partition format failed
    End If

    ' Create CMD line to Install O/S & Run
    WScript.Echo
    EchoAndLog   Space(3) & "About to Install TestOS from " & sSrcPath
    WScript.Echo Space(3) & "hit CTRL+C to quit TestOS install..."
    WScript.Sleep(5000)     ' sleep 5 seconds

    ' Suspend DITS
    On Error Resume next
    objShell.Run "cscript //nologo " & "%WINDIR%\bricktools\SuspendDITS.wsf",iWINMIN,bWINWAIT
    On Error goto 0

    EchoAndLog Date & " " & Time
    If (g_bOSUpgrade = True) Then
        objShell.Run sSrcPath & "\winnt32.exe" & _
                     " /unattended:" & sSAFEDEL & "\oasis.txt",iWINMIN,bWINWAIT
    Else
        objShell.Run sSrcPath & "\winnt32.exe" & _
                     " /s:" & sSrcPath & _
                     " /s:" & sSrcPath & _
                     " /s:" & sSrcPath & _
                     " /s:" & sSrcPath & _
                     " /s:" & sSrcPath & _
                     " /s:" & sSrcPath & _
                     " /s:" & sSrcPath & _
                     " /s:" & sSrcPath & _
                     " /unattend:" & sSAFEDEL & "\oasis.txt" & _
                     " /tempdrive:" & g_sOSDrive & _
                     " /copysource:lang",iWINMIN,bWINWAIT
    End If
End Sub

'================== Verify Inputs ============================================
' Verify user inputs
' - seperate into sections for actions to be performed inbetween checking
Sub VerifyInputs(ByRef sSrcPath,ByRef sPidKey,ByRef sLang_Grp,ByRef sLoc_ID,aOSComp)
    Dim x, y, z, arg
    Dim BuildFile
    Dim XMLEntryNodes, XMLEntryKey, XMLHeaderNode
    Dim sNoValid
    Dim bValid
    Dim aXMLHeaderNodes, aVars

    EchoAndLog Space(3) & "Verifying User Inputs"
    sSrcPath = LCase(sOSSRV)

' Verify g_sTestOS Input
    Select Case g_sTestOS
        Case "W2K"
        Case "WINXP"
            g_sOSVersion = "PRO"
        Case "W2K3"
        Case "UPGRADE"
            g_sTestOS    = Empty
            g_bOSUpgrade = True
        Case Else
            EchoAndLog Space(3) & "*" & g_sTestOS & "*  Invalid OS choice...please choose - W2K,WINXP,W2K3, or Upgrade"
            QuitProgram(1)     ' g_sTestOS input invalid
    End Select
    ' Determine if this is an Upgrade scenario
    If (g_bOSUpgrade = True) Then
        If (g_sOSDrive = g_SysDrive) And _
           (g_sOSName = g_SysName) And _
           (g_sTestOS = Empty) And _
           (g_sOSFormat = Empty) And _
           (g_sOSLang = Empty) And _
           (g_sOSType = Empty) And _
           (g_sOSVersion = Empty) Then
            UpgradeOS ()
        Else
            EchoAndLog Space(3) & "Invalid input flag for Upgrade scenario...see usage."
            Usage()
            QuitProgram(1)     ' invalid flag input for Upgrade
        End If
    End If
    If Not objFSO.FolderExists(sSrcPath & "\" & g_sTestOS) Then
        EchoAndLog Space(3) & "Cannot find " & LCase(sSrcPath & "\" & g_sTestOS) & _
                   "...please check connection to " & UCase(sOSSRV) & "."
        QuitProgram(2)     ' g_sTestOS folder does not exist in sSrcPath
    End If
    EchoAndLog Space(6) & "Verified input 'TestOS'" & vbtab & "..." & g_sTestOS
    sSrcPath = sSrcPath & "\" & LCase(g_sTestOS)

    ' Setup Default Values for non-upgrade scenario (after obtaining defaults for upgrade)
    If (g_bOSUpgrade = False) Then
        If (g_sOSDomain = Empty) Then
            g_sOSDomain = "SMX.NET"
        End If
        If (g_sOSVersion = Empty) Then
            g_sOSVersion = "SRV"
        End If
        If (g_sOSLang = Empty) Then
            g_sOSLang = "EN"
        End If
        If (g_sOSType = Empty) And (g_bOSUpgrade = False) Then
            g_sOSType = "FRE"
        End If
        If (g_strOSComp = Empty) And (g_bOSComp = False) Then
            g_strOSComp = "SNMP,NETMONTOOLS,IIS_COMMON,IIS_FTP,IIS_INETMGR,IIS_NNTP," _
                          & "IIS_NNTP_DOCS,IIS_SMTP,IIS_WWW,MSMQ,MSMQ_CORE," _
                          & "MSMQ_LOCALSTORAGE,TSENABLE,TSWEBCLIENT,ASPNET," _
                          & "BITSSERVEREXTENSIONSISAPI,BITSSERVEREXTENSIONSMANAGER"
        End If
    End If

' Verify g_sOSDrive Input - (append ':' if user did not)
    If (Len(g_sOSDrive) = 1) Then
        g_sOSDrive = g_sOSDrive & ":"
    End If
    ' Can not refresh safeOS
    If (g_sOSDrive = sSAFEDRIVE) Then
        EchoAndLog Space(3) & "Cannot use OASIS to refresh your SafeOS...please use the '/drive:' flag."
        QuitProgram(1)     ' safeOS drive selected for refresh
    End If
    ' g_sOSDrive partition does not exist
    If Not objFSO.DriveExists(g_sOSDrive) Then
        EchoAndLog Space(3) & g_sOSDrive & " does not exist...please select another drive."
        QuitProgram(1)     ' g_sOSDrive does not exist on machine
    End If
    ' Determine what drive type the g_sOSDrive is...only continue if 'fixed'
    Select Case objFSO.GetDrive(g_sOSDrive).DriveType
        Case 0
            arg = "Unknown"
        Case 1
            arg = "Removable"
        Case 2
            arg = "Fixed"
        Case 3
            arg = "Network"
        Case 4
            arg = "CD-ROM"
        Case 5
            arg = "RAM Disk"
    End Select
    If Not(arg = "Fixed") Then
        EchoAndLog Space(3) & "You may not use OASIS on the " & g_sOSDrive & _
                   " because it is a " & arg & " drive...please select another drive."
        QuitProgram(1)     ' drive type of g_sOSDrive will not support a testOS installation
    End If
    EchoAndLog Space(6) & "Verified input 'Drive'" & vbtab & "..." & g_sOSDrive

' Verify g_sOSFormat Input
    If (g_sOSFormat = Empty) Then
        g_sOSFormat = "NTFS"
    End If
    Select Case UCase(g_sOSFormat)
        Case "NTFS"
        Case "FAT"
        Case "FAT32"
        Case Else
            EchoAndLog Space(3) & g_sOSFormat & " is an invalid format...please choose - NTFS,FAT,FAT32"
            QuitProgram(1)     ' g_sOSFormat input invalid
    End Select
    EchoAndLog Space(6) & "Verified input 'Format'" & vbtab & "..." & g_sOSFormat

' Verify g_sOSName Input - only operates if no user input provided
    If (g_sOSName = g_SysName) Then
        ' OASIS started from safeOS...name corrected to include testOS drive letter
        If (g_SysDrive = sSAFEDRIVE) Then
            g_sOSName = g_SysName & Left(g_sOSDrive,1)
        Else
            ' OASIS started from testOS to refresh...name corrected to %machinename%
            If (g_SysDrive = g_sOSDrive) Then
                g_sOSName = g_SysName
            Else
            ' OASIS started on alternate testOS than specified to refresh
                EchoAndLog Space(3) & "You must input a testOS name to refresh an alternate testOS" &_
                           "...please use the '/name:' flag"
                QuitProgram(1)     ' started from different testOS than refreshing...must input '/name:' parameter
            End If
        End If
    End If
    EchoAndLog Space(6) & "Verified input 'Name'" & vbtab & "..." & g_sOSName

' Verify g_sOSDomain Input
    aVars = Split(g_sOSDomain,".",2)
    g_sOSDomainLOC  = aVars(0)
    On Error Resume Next
    g_sOSDomainFQDN = aVars(1)
    On Error goto 0
    If (g_sOSDomainFQDN = Empty) Then
        If (g_sOSDomainLOC = "SMX") Then
            g_sOSDomainFQDN = "NET"
        Else
            If (g_sOSDomainLOC = "BOWMORE") Then
                g_sOSDomainFQDN = ""
            Else
            ' Joining a Workgroup
                g_bWkgp = True
            End If
        End If
        g_sOSDomainEN = g_sOSDomainLOC
    Else
        ' Joining a qualified domain (translate names to/from Unicode characters as neccessary)
        g_bWkgp = False
        If (g_sOSDomainLOC <> "BOWMORE") Then
            g_sOSDomainFQDN = "." & g_sOSDomainFQDN
        Else
            g_sOSDomainEN   = g_sOSDomainLOC
        End If
        'Create line for input to OU container on DC
        aVars = Split(g_sOSDomainLOC & g_sOSDomainFQDN,".",iSUBSTRINGS)
        g_sOUContainer = "OU=TestOS"
        For x = LBound(aVars) To UBound(aVars)
            g_sOUContainer = g_sOUContainer & "," & "DC=" & aVars(x)
        Next
    End If
    EchoAndLog Space(6) & "Verified input 'Domain'" & vbtab & "..." & g_sOSDomain

' Verify g_sOSLang Inputs
    ' http://www.microsoft.com/globaldev/reference/winxp/xp-lcid.mspx
    ' http://www.microsoft.com/globaldev/reference/win2k/setup/localsupport.mspx
    Select Case g_sOSLang
        Case "BR"                         'Brazilian
            sLoc_ID   = "0416:00000416"
            sLang_Grp = "1"
        Case "CN"                         'Chinese (Simplified)
            sLoc_ID   = "0804:e0010804"
            sLang_Grp = "10"
        Case "TW"                         'Chinese (Traditional/Taiwan)
            sLoc_ID   = "0404:e0010404"
            sLang_Grp = "9"
        Case "HK"                         'Chinese (Hong Kong)
            sLoc_ID   = "0c04:e0080404"
            sLang_Grp = "9"
        Case "CS"                         'Czech
            sLoc_ID   = "0405:00000405"
            sLang_Grp = "2"
        Case "EN"                         'English
            sLoc_ID   = "0409:00000409"
            sLang_Grp = "1"
        Case "FR"                         'French
            sLoc_ID   = "040c:0000040c"
            sLang_Grp = "1"
        Case "DE"                         'German
            sLoc_ID   = "0407:00000407"
            sLang_Grp = "1"
        Case "HU"                         'Hungarian
            sLoc_ID   = "040e:0000040e"
            sLang_Grp = "2"
        Case "IT"                         'Italian
            sLoc_ID   = "0410:00000410"
            sLang_Grp = "1"
        Case "JA"                         'Japanese
            sLoc_ID   = "0411:e0010411"
            sLang_Grp = "7"
        Case "KO"                         'Korean
            sLoc_ID   = "0412:e0010412"
            sLang_Grp = "8"
        Case "PL"                         'Polish
            sLoc_ID   = "0415:00010415"
            sLang_Grp = "2"
        Case "PT"                         'Portuguese
            sLoc_ID   = "0816:00000816"
            sLang_Grp = "1"
        Case "PS"                         'Pseudo
            sLoc_ID   = "0409:00000409"
            sLang_Grp = "1"
        Case "RU"                         'Russian
            sLoc_ID   = "0419:00000419"
            sLang_Grp = "5"
        Case "ES"                         'Spanish
            sLoc_ID   = "040a:0000040a"
            sLang_Grp = "1"
        Case "SV"                         'Swedish
            sLoc_ID   = "041d:0000041d"
            sLang_Grp = "1"
        Case "TR"                         'Turkish
            sLoc_ID   = "041f:0000041f"
            sLang_Grp = "6"
        Case Else
            EchoAndLog Space(3) & g_sOSLang & " is an unsupported language choice...please rerun."
            QuitProgram(1)     ' g_sOSLang input invalid
    End Select
    If Not objFSO.FolderExists(sSrcPath & "\" & g_sOSLang) Then
        EchoAndLog Space(3) & "Cannot find language " & g_sOSLang & " at " & sSrcPath & _
                   "...please contact " & sFAILALIAS & "."
        QuitProgram(2)     ' g_sOSLang folder does not exist in sSrcPath
    End If
    ' Modify inputs to force East Asian LangPack installation for every language; and EN defaults
    sLoc_ID   = "0409:00000409," & sLoc_ID & ",0411:e0010411,0412:e0010412,0404:e0010404,0804:e0010804"
    sLang_Grp = "1," & sLang_Grp & ",7,8,9,10"
    EchoAndLog Space(6) & "Verified input 'Language'" & vbtab & "..." & g_sOSLang
    sSrcPath = sSrcPath & "\" & LCase(g_sOSLang)

' Verify g_iOSBuild # Input
    g_sOSBuildOrig = g_iOSBuild
    RE.Pattern = ".SP[0-9]"    ' set pattern to find SP info
    ' Seperate SP info from Build Number
    If RE.Test(Right(g_iOSBuild,4)) Then
        g_sSP = Right(g_iOSBuild,4)
        g_iOSBuild = Left(g_iOSBuild,Len(g_iOSBuild) - 4)
    End If
    ' g_iOSBuild is flag, not a 'real' build number
    If Not IsNumeric(g_iOSBuild) Then
        Select Case g_iOSBuild
            Case "LATEST"
            Case "BLESSED"
            Case Else
                EchoAndLog Space(3) & g_iOSBuild & " is an invalid build/flag choice" & _
                           "...please choose - LATEST or BLESSED"
                QuitProgram(1)     ' g_iOSBuild input invalid
        End Select
        ' Establish latest Build # for released products
        Select Case g_sTestOS
            Case "W2K"
                g_iOSBuild="2195"
                If (g_sSP = Empty) Then
                    g_sSP = ".SP4"
                End If
            Case "WINXP"
                g_iOSBuild="2600"
                If (g_sSP = Empty) Then
                    g_sSP = ".SP2"
                End If
            Case "W2K3"
                g_iOSBuild="3790"
        End Select
    End If
    ' Use g_iOSBuild to determine PID for the OS
    If (g_SysProcVar = "X86") Then
        If (g_iOSBuild = 2195) Then                       ' W2K
            If (g_sOSVersion = "PRO") Then
                sPidKey = "VXKC4-2B3YF-W9MFK-QB3DB-9Y7MB"
            Elseif (g_sOSVersion = "DTC") Then
                sPidKey = "RM233-2PRQQ-FR4RH-JP89H-46QYB"
            Else
                sPidKey = "H6TWQ-TQQM8-HXJYG-D69F7-R84VM"
            End If
        ElseIf (g_iOSBuild = 2600) Then                   ' WINXP
            sPidKey = "T47XK-46FXT-MB6PX-7JHXC-4V3K8"
        ElseIf (g_iOSBuild = 3790) Then                   ' W2K3
            If (g_sOSVersion = "ADS") Then
                sPidKey = "XQJW7-QC7YP-W24GB-R4YRW-4864D"
            Elseif (g_sOSVersion = "SRV") Then
                sPidKey = "VKG2H-BJ26K-TPKKT-4V8KB-3JVXW"
            Elseif (g_sOSVersion = "DTC") Then
                sPidKey = "TCY37-3Y4CD-BGG8K-7PYGY-3DVY6"
            Elseif (g_sOSVersion = "BLA") Then
                sPidKey = "PDHPG-3XPG6-BX3JG-DMGVY-7YCGD"
            Else sPidKey = "VKG2H-BJ26K-TPKKT-4V8KB-3JVXW"
            End If
        End If
    End If
    If (g_SysProcVar = "IA64") Then
        If (g_iOSBuild = 2600) Then                       ' WINXP
            sPidKey = "GCB4F-KWXJC-QF7H8-6RBCX-GQDT8"
        ElseIf (g_iOSBuild = 3790) Then                   ' W2K3
            sPidKey = "KV9B7-TYRVP-G74HT-YYBXT-4YG63"
        End If
    End If
    If (g_SysProcVar = "AMD64") Then
        If (g_iOSBuild = 3790) Then                   ' W2K3
            ' Old PID Key
            'sPidKey = "CKY24-Q8QRH-X3KMR-C6BCY-T847Y"
            sPidKey = "GXBMC-84YGV-FGW84-Q88V7-WPX63" 'W2K3
        End If
    End If
    If (sPidKey = Empty) Then
        EchoAndLog Space(3) & "Could not locate a PID for build " & g_iOSBuild & _
                   "...please contact " & sFAILALIAS & "."
        QuitProgram(3)     ' could not find PID in build # range to assign
    End If
    g_iOSBuild = g_iOSBuild & g_sSP
    If Not objFSO.FolderExists(sSrcPath & "\" & g_iOSBuild) Then
        EchoAndLog Space(3) & "Cannot find build " & g_iOSBuild & " at " & sSrcPath & _
                   "...please contact " & sFAILALIAS & "."
        QuitProgram(2)     ' g_iOSBuild folder does not exist in sSrcPath
    End If
    EchoAndLog Space(6) & "Verified input 'Build'" & vbtab & "..." & g_iOSBuild
    sSrcPath = sSrcPath & "\" & LCase(g_iOSBuild)

' Verify g_sOSType Input
    Select Case g_sOSType
        Case "FRE"
        Case "CHK"
        Case Else
            EchoAndLog Space(3) & g_sOSType & " is an invalid OS type...please choose - FRE,CHK"
            QuitProgram(1)     ' g_sOSType input invalid
    End Select
    If Not objFSO.FolderExists(sSrcPath & "\" & g_SysProcVar & g_sOSType) Then
        EchoAndLog Space(3) & "Cannot find type " & g_SysProcVar & g_sOSType & " at " & sSrcPath & _
                   "...please contact " & sFAILALIAS & "."
        QuitProgram(2)     ' g_SysProcVar & g_sOSType folder does not exist in sSrcPath
    End If
    EchoAndLog Space(6) & "Verified input 'Type'" & vbtab & "..." & g_sOSType
    sSrcPath = sSrcPath & "\" & LCase(g_SysProcVar & g_sOSType)

' Verify g_sOSVersion Input
    If Not objFSO.FolderExists(sSrcPath & "\" & g_sOSVersion) Then
        EchoAndLog Space(3) & "Cannot find version " & g_sOSVersion & " at " & sSrcPath & _
                   "...please contact " & sFAILALIAS & "."
        QuitProgram(2)     ' g_sOSVersion folder does not exist in sSrcPath
    End If
    EchoAndLog Space(6) & "Verified input 'Version'" & vbtab & "..." & g_sOSVersion
    sSrcPath = sSrcPath & "\" & LCase(g_sOSVersion)

' Verify g_strOSComp Input
    aOSComp = Split(g_strOSComp,",",iSUBSTRINGS)
    aXMLHeaderNodes = Array("NetOptionalComponents","Components")
    ' Iterate through all OS Component Inputs and verify existence in g_sCompXML
    For x = LBound(aOSComp) To UBound(aOSComp)
        bValid = False
        For y = LBound(aXMLHeaderNodes) To UBound(aXMLHeaderNodes)
            Set XMLHeaderNode = g_XMLRoot.SelectSingleNode("header[@name='" & LCase(aXMLHeaderNodes(y)) & "']")
            ' Search for EntryNode requested (break loop when found, and then modify its entries)
            Set XMLEntryNodes = XMLHeaderNode.SelectNodes("entry")
            For z = 0 To (XMLEntryNodes.length - 1)
                XMLEntryKey = XMLEntryNodes.item(z).attributes.getNamedItem("key").text
                If (XMLEntryKey = LCase(aOSComp(x))) Then
                    bValid = True
                    Exit For
                End If
            Next
        Next
        If (bValid = False) Then
            sNoValid = sNoValid & " " & UCase(aOSComp(x))
        End If
    Next
    If (sNoValid <> Empty) Then
        EchoAndLog Space(3) & sNoValid & ": are invalid OS Component(s)."
        Usage()
        QuitProgram(1)     ' g_strOSComp input(s) invalid
    End If
    EchoAndLog Space(6) & "Verified input 'Component'..." & g_strOSComp

' Final verification of sSrcPath
    If Not objFSO.FileExists(sSrcPath & "\" & g_SysProcLegacy & "\winnt32.exe") Then
        EchoAndLog Space(3) & "Cannot find " & sSrcPath & "\" & g_SysProcLegacy & "\winnt32.exe" & _
                   "...please contact " & sFAILALIAS & "."
        QuitProgram(2)     ' could not locate executable file to start OS installation
    End If
    g_RegSourcePath = sSrcPath
    sSrcPath = sSrcPath & "\" & LCase(g_SysProcLegacy)
End Sub

'================== Modify Variable values for Upgrade Scenario ==============
' http://support.microsoft.com/default.aspx?scid=kb;en-us;186336
Sub UpgradeOS()
' - Ignore g_iOSBuild...based on normal flow and gets checked in VerifyInputs
' - Ignore g_strOSComp...not relevant, take as is and if component already installed should NOT hurt anything
' - Ignore g_sOSAdd...not relevant, take as is and add to oasis.txt
    Dim x, y, z
    Dim objSysInfo
    Dim sTestOS, sVersion, bType, iLang

' Gather g_sOSDrive/g_sOSName data
    g_sOSDrive = g_SysDrive
    g_sOSName  = g_SysName

' Gather g_sOSFormat data (WMI query)
    For Each x In GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_LogicalDisk WHERE DeviceID='" _
        & g_sOSDrive & "'")
        g_sOSFormat = UCase(x.FileSystem)
    Next

' Gather g_sOSDomain data
    Set objSysInfo = CreateObject("ADSystemInfo")
    g_sOSDomain    = UCase(objSysInfo.DomainDNSName)
    Set objSysInfo = Nothing

' Gather g_sTestOS/g_sOSVersion/g_sOSType/g_sOSLang data (WMI query)
    For Each x In GetObject("winmgmts:").InstancesOf("Win32_OperatingSystem")
        sTestOS  = Left(x.Version,3)
        sVersion = UCase(x.Caption)
        bType    = x.Debug
        iLang    = x.OSLanguage
    Next

    ' Assign g_sOSVersion
    If (InStr(sVersion,"PROFESSIONAL") <> 0) Then
        ' XP and W2K have Professional in caption
        g_sOSVersion = "PRO"
    Elseif (InStr(sVersion,"WINDOWS 2000 SERVER") <> 0) Or (InStr(sVersion,"STANDARD") <> 0) Then
        ' Search explicitly for Windows 2000 Server or Standard; all W2K3 have Server in caption
        g_sOSVersion = "SRV"
    Elseif (InStr(sVersion,"WINDOWS 2000 ADVANCED SERVER") <> 0) Or (InStr(sVersion, "ENTERPRISE") <> 0) Then
        ' Search explicitly for Windows 2000 Advanced Server or 2003 Enterprise; all W2K3 have Enterprise in caption
        g_sOSVersion = "ADS"
    Elseif (InStr(sVersion,"WEB") <> 0) Then
        ' Only W2K3 has a Web Blade type
        g_sOSVersion = "BLA"
    Elseif (InStr(sVersion, "DATACENTER") <> 0) Then
        ' W2K and W2K3 have Datacenter in caption
        g_sOSVersion = "DTC"
    Else
        EchoAndLog Space(3) & "Unable to obtain O/S verion (ADS,SRV,etc)...please contact " & sFAILALIAS & "."
        QuitProgram(3)     ' WMI query did not return a valid g_sOSVersion
    End If

    ' Assign Upgrade of g_sTestOS to OS one above
    Select Case sTestOS
        Case "5.0"
            If g_sOSVersion = "PRO" Then
                g_sTestOS = "WINXP"
                EchoAndLog Space(3) & "Running Upgrade scenario from W2K PRO to WINXP PRO...."
            Else
                g_sTestOS = "W2K3"
                EchoAndLog Space(3) & "Running Upgrade scenario from W2K to W2K3...."
            End If
        'Case "5.1"    'TBD
        'Case "5.2"    'TBD
        Case Else
            EchoAndLog Space(3) & "Unsupported upgrade scenario...cannot upgrade current test O/S."
            QuitProgram(1)     ' cannot upgrade current test O/S
    End Select

    ' Assign g_sOSType...(based on previous WMI query)
    If (bType = True) Then
        g_sOSType = "CHK"
    Else
        g_sOSType = "FRE"
    End If

    ' Assign g_sOSLang
    ' http://www.microsoft.com/globaldev/reference/winxp/xp-lcid.mspx
    Select Case CInt(iLang)
        Case 1046            'Brazilian
            g_sOSLang = "BR"
        Case 2052            'Chinese (Simplified)
            g_sOSLang = "CN"
        Case 1028            'Chinese (Traditional/Taiwan)
            g_sOSLang = "TW"
        Case 3076            'Chinese (Hong Kong)
            g_sOSLang = "HK"
        Case 1029            'Czech
            g_sOSLang = "CS"
        Case 1033            'English
            g_sOSLang = "EN"
        Case 1036            'French
            g_sOSLang = "FR"
        Case 1031            'German
            g_sOSLang = "DE"
        Case 1038            'Hungarian
            g_sOSLang = "HU"
        Case 1040            'Italian
            g_sOSLang = "IT"
        Case 1041            'Japanese
            g_sOSLang = "JA"
        Case 1042            'Korean
            g_sOSLang = "KO"
        Case 1045            'Polish
            g_sOSLang = "PL"
        Case 2070            'Portuguese
            g_sOSLang = "PT"
'       Case 10XX            'Pseudo
'           g_sOSLang = "PS"
        Case 1049            'Russian
            g_sOSLang = "RU"
        Case 1034            'Spanish
            g_sOSLang = "ES"
        Case 1053            'Swedish
            g_sOSLang = "SV"
        Case 1055            'Turkish
            g_sOSLang = "TR"
    End Select
End Sub

'================== Create ShortCut Link to File =============================
' Create a shortcut link to another file
Sub Link(sFileSrc,sFileDest)
    Dim objFileLink
    Dim sFileBaseName

    ' Retrieve name of file (no directory path or extension info)
    sFileBaseName = objFSO.GetBaseName(sFileSrc)
    If Not objFSO.FileExists(sFileSrc) Then
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
    If Not objFSO.FolderExists(sFileDest) Then
        EchoAndLog Space(3) & "Cannot find the folder - " & sFileDest
        QuitProgram(2)     ' could not find the folder to create the link in
    End If

    ' Create shortcut file at desired link location
    Set objFileLink = objShell.CreateShortcut(sFileDest & "\" & sFileBaseName & ".lnk")
    objFileLink.TargetPath = sFileSrc
    objFileLink.WindowStyle = 1
    objFileLink.Save
End Sub

'================== Determine Driver(s) Changes ==============================
' Gather data from machine and determine hardware specific drivers are needed
Sub DetermineDrivers()
    Dim x, y, z, strInfo
    Dim objLocator, objServices
    Set objLocator  = CreateObject("Wbemscripting.SWbemlocator")

    ' Iterate through all device ID's...match with specific driver entries
    ' - only needs to operate if O/S is W2K for now
    Set objServices = objLocator.ConnectServer(".", "root\CIMv2")
    Set strInfo     = objServices.InstancesOf("Win32_PnPEntity")
    Const ID5I = "PCI\VEN_0E11&DEV_B178&SUBSYS_40800E11&REV_01\3&13C0B0C5&0&20"       '5i MassStorage Drivers
    Const ID5Ib = "PCI\VEN_0E11&DEV_B178&SUBSYS_40800E11&REV_01\3&267A616A&0&20"      '5i MassStorage Drivers, Different rev
    Const ID5Ic = "PCI\VEN_0E11&DEV_B178&SUBSYS_40800E11&REV_01\3&13C0B0C5&0&08"      '5i MassStorage Drivers, Different rev
    Const ID5Id = "PCI\VEN_0E11&DEV_B178&SUBSYS_40800E11&REV_01\3&13C0B0C5&0&18"      '5i MassStarage Drivers, Different rev
    Const ID6400 = "PCI\VEN_0E11&DEV_0046&SUBSYS_409C0E11&REV_01\4&392398EF&0&2010"   '6400 MassStorage Drivers
    Const IDPERC3DI = "PCI\VEN_1028&DEV_000A&SUBSYS_011B1028&REV_01\3&1070020&0&41"   'Perc3Di Drivers
    Const IDPERC2= "PCI\VEN_1011&DEV_0046&SUBSYS_13649005&REV_01\3&29E81982&0&08"     'Perc2 Drivers
    Const IDPERC4DI = "PCI\VEN_1028&DEV_000F&SUBSYS_014A1028&REV_02\3&474B838&0&18"   'Perc4/Di Drivers
    Const IDTX2 = "PCI\VEN_105A&DEV_6268&SUBSYS_4D68105A&REV_02\3&291BF6FF&0&50"      'Promise FastTrak TX2 (Fujitsu Blade Servers)
    Const sMassStorageDrivers = "MassStorageDrivers"
    Const sOEMBootFiles = "OEMBootFiles"
    Const sXMLOn = "on"

    Const sCompaqW2KDisk = "Compaq Smart Array 5i Driver Diskette for Windows 2000"
    Const sSmartArray5300 = "Smart Array 5300 Controller"
    Const sSmartArray5i = "Smart Array 5i, 532, 5312 Controllers"
    Const sSmartArray6 = "Smart Array 641, 642, 6400, 6400EM Controllers"
    Const sIDECdrom = "IDE CD-ROM (ATAPI 1.2)/PCI IDE Controller"
    Const sDellPerc = "Dell PERC 2, 2/Si, 3/Si, 3/Di RAID Controllers"
    Const sPerc4W2KStr = "DELL PERC 4/Di RAID Driver for Windows 2000 "
    Const sPerc4W2K3Str = "DELL PERC 4/Di RAID On Motherboard Driver"
    Const sTX2Str = "Promise FastTrak100 (tm) LP/TX2/TX4 Controller"

    Const sTXTSetup = "TXTSETUP.OEM"
    Const sCPQCissmDLL = "cpqcissm.dll"
    Const sCPQCissmSys = "CPQCISSM.SYS"
    Const sCPQCissmCat = "cpqcissm.cat"
    Const sCPQCissmInf = "cpqcissm.inf"
    Const sPerc2Sys = "PERC2.SYS"
    Const sPerc2HIBSys = "PERC2HIB.SYS"

    Const sPerc4W2KOEMFILE1 = "mraid2k.cat"
    Const sPerc4W2KOEMFILE2 = "MRAID2K.sys"
    Const sPerc4W2KOEMFILE3 = "nodev.inf"
    Const sPerc4W2KOEMFILE4 = "oemsetup.inf"

    Const sPerc4W2K3OEMFILE1= "mraid35x.cat"
    Const sPerc4W2K3OEMFILE2= "Mraid35x.sys"
    Const sPerc4W2K3OEMFILE3= "nodev.inf"
    Const sPerc4W2K3OEMFILE4= "oemsetup.inf"

    Const sTX2OEMFILE1 = "FASTTRAK.CAT"
    Const sTX2OEMFILE2 = "fasttrak.inf"
    Const sTX2OEMFILE3 = "fasttrak.sys"

    For Each x In strInfo
        If (x.PNPDeviceID = ID5I) Or (x.PNPDeviceID = ID5Ib) Or (x.PNPDeviceID = ID5Ic) _
            Or (x.PNPDeviceID = ID6400) Or (x.PNPDeviceID = ID5Id) Then
            ChangeXML sMassStorageDrivers, _
                      Chr(34) & sCompaqW2KDisk & Chr(34),"",sXMLOn
            ChangeXML sMassStorageDrivers, _
                      Chr(34) & sSmartArray5300 & Chr(34),"",sXMLOn
            ChangeXML sMassStorageDrivers, _
                      Chr(34) & sSmartArray5i & Chr(34),"",sXMLOn
            ChangeXML sMassStorageDrivers, _
                      Chr(34) & sSmartArray6 & Chr(34),"",sXMLOn
            ChangeXML sMassStorageDrivers, _
                      Chr(34) & sIDECdrom & Chr(34),"",sXMLOn
            ChangeXML sOEMBootFiles,sTXTSetup,"",sXMLOn
            ChangeXML sOEMBootFiles,sCPQCissmDLL,"",sXMLOn
            ChangeXML sOEMBootFiles,sCPQCissmSys,"",sXMLOn
            ChangeXML sOEMBootFiles,sCPQCissmCat,"",sXMLOn
            ChangeXML sOEMBootFiles,sCPQCissmInf,"",sXMLOn
            EchoAndLog Space(6) & "5I drive array (W2K/W2K3)...assigning drivers."
        End If

        If (x.PNPDeviceID = IDTX2) Then
            ChangeXML sMassStorageDrivers, Chr(34) & sTX2Str & Chr(34),"",sXMLOn
            ChangeXML sMassStorageDrivers, Chr(34) & sIDECdrom & Chr(34),"",sXMLOn
            ChangeXML sOEMBootFiles,sTXTSetup,"",sXMLOn
            ChangeXML sOEMBootFiles,sTX2OEMFILE1,"",sXMLOn
            ChangeXML sOEMBootFiles,sTX2OEMFILE2,"",sXMLOn
            ChangeXML sOEMBootFiles,sTX2OEMFILE3,"",sXMLOn
            EchoAndLog Space(6) & "Promise TX2 (W2K/W2K3)...assigning drivers."
        End If

        If (g_sTestOS = "W2K") Then
            If (x.PNPDeviceID = IDPERC3DI) Or (x.PNPDeviceID = IDPERC2) Then
                ChangeXML sMassStorageDrivers, Chr(34) & sDellPerc & Chr(34),"","on"
                ChangeXML sMassStorageDrivers, Chr(34) & sIDECdrom & Chr(34),"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc2Sys,"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc2HIBSys,"",sXMLOn
                ChangeXML sOEMBootFiles,sTXTSetup,"",sXMLOn
                EchoAndLog Space(6) & "DELL PERC Array (W2K)...assigning drivers."
            End If
            If (x.PNPDeviceID = IDPERC4DI) Then
                ChangeXML sMassStorageDrivers, Chr(34) & sPerc4W2KStr & Chr(34),"","on"
                ChangeXML sMassStorageDrivers, Chr(34) & sIDECdrom & Chr(34),"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc4W2KOEMFILE1,"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc4W2KOEMFILE2,"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc4W2KOEMFILE3,"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc4W2KOEMFILE4,"",sXMLOn
                ChangeXML sOEMBootFiles,sTXTSetup,"",sXMLOn
                EchoAndLog Space(6) & "DELL PERC 4 Array (W2K)...assigning drivers."
            End If
        End If

        If (g_sTestOS = "W2K3") Then
            If (x.PNPDeviceID = IDPERC4DI) Then
                ChangeXML sMassStorageDrivers, Chr(34) & sPerc4W2K3Str & Chr(34),"","on"
                ChangeXML sMassStorageDrivers, Chr(34) & sIDECdrom & Chr(34),"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc4W2K3OEMFILE1,"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc4W2K3OEMFILE2,"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc4W2K3OEMFILE3,"",sXMLOn
                ChangeXML sOEMBootFiles,sPerc4W2K3OEMFILE4,"",sXMLOn
                ChangeXML sOEMBootFiles,sTXTSetup,"",sXMLOn
                EchoAndLog Space(6) & "DELL PERC 4 Array (W2K3)...assigning drivers."
            End If
        End If
    Next
    Set objLocator = Nothing
End Sub

'================== Change XML in Memory =====================================
' Modify/find Component values from the g_sCompXML in memory
Sub ChangeXML(sHeaderName,sEntryKey,sEntryValue,sEntryEnable)
    Dim x, y, z
    Dim XMLHeaderNodes, XMLHeaderNode, XMLHeaderName, XMLEntryNodes, XMLEntryNode, XMLEntryKey
    Set XMLHeaderNode = Nothing
    Set XMLEntryNode = Nothing
    Set XMLHeaderNodes = g_XMLRoot.selectNodes("header")

    ' Search XMLHeaderNodes and ensure requested sHeaderName exists in XML(break loop when found)
    For x = 0 To (XMLHeaderNodes.length - 1)
        XMLHeaderName = XMLHeaderNodes.item(x).attributes.getNamedItem("name").text
        If (XMLHeaderName = LCase(sHeaderName)) Then
            Set XMLHeaderNode = XMLHeaderNodes.item(x)
            Exit For
        End If
    Next

    ' Search for sEntryKey requested (break loop when found)
    If Not(XMLHeaderNode Is Nothing) and (sEntryKey <> Empty) Then
        Set XMLEntryNodes = XMLHeaderNode.selectNodes("entry")
        For x = 0 To (XMLEntryNodes.length - 1)
            XMLEntryKey = XMLEntryNodes.item(x).attributes.getNamedItem("key").text
            If (XMLEntryKey = LCase(sEntryKey)) Then
                Set XMLEntryNode = XMLEntryNodes.item(x)
                Exit For
            End If
        Next
        ' Found sEntryNode...modify its parameters
        If Not(XMLEntryNode Is Nothing) Then
            ' Change Value parameter
            If (sEntryValue <> Empty) Then
                XMLEntryNode.attributes.getNamedItem("value").text = sEntryValue
            End If
            ' Change Enable parameter
            If (sEntryEnable <> Empty) Then
                XMLEntryNode.attributes.getNamedItem("enable").text = sEntryEnable
            End If
        End If
    End If
End Sub

'================== Create & Write Oasis.txt =================================
' Create unattended text file for test O/S installation
Sub CreateUnattendFile()
    Dim x, y, z
    Dim CompFile
    Dim XMLHeaderNodes, XMLHeaderName, XMLEntryNodes, XMLSubNodes, XMLSubName
    Dim sCompOutput
    Dim aHeaders, aEntrys, aValues
    Dim objDictHeader
    Set objDictHeader = CreateObject("Scripting.Dictionary")
    Set XMLHeaderNodes = g_XMLRoot.selectNodes("header")

    ' Parse Modified XML & create Dictionary Objects to hold data found
    For x = 0 To (XMLHeaderNodes.length - 1)
        XMLHeaderName = XMLHeaderNodes.item(x).attributes.getNamedItem("name").text
        ' Add XMLHeaderName/EntryDictionary to HeaderDictionary (if not already exist)
        If Not objDictHeader.Exists(XMLHeaderName) Then
            objDictHeader.Add XMLHeaderName,CreateObject("Scripting.Dictionary")
        End If
        ' Add EntryNode Key/Value to EntryDictionary (if enable="on" or "always")
        Set XMLEntryNodes = XMLHeaderNodes.item(x).selectNodes("entry")
        For y = 0 To (XMLEntryNodes.length - 1)
            If (XMLEntryNodes.item(y).attributes.getNamedItem("enable").text = "always") Or _
               (XMLEntryNodes.item(y).attributes.getNamedItem("enable").text = "on") Then
                objDictHeader.Item(XMLHeaderName).Item(XMLEntryNodes.item(y).attributes.getNamedItem("key").text) _
                                   = XMLEntryNodes.item(y).attributes.getNamedItem("value").text
                ' Check for XMLSubNodes per EntryNode
                Set XMLSubNodes = XMLEntryNodes.item(y).selectNodes("sub")
                If Not(XMLSubNodes Is Nothing) Then
                    ' Add XMLSubName/EntryDictionary to HeaderDictionary (if not already exist) for dependency purposes
                    For z = 0 To (XMLSubNodes.length - 1)
                        XMLSubName = XMLSubNodes.item(z).attributes.getNamedItem("name").text
                        If Not objDictHeader.Exists(XMLSubName) Then
                            objDictHeader.Add XMLSubName,CreateObject("Scripting.Dictionary")
                        End If
                        objDictHeader.Item(XMLSubName).Item(XMLSubNodes.item(z).attributes.getNamedItem("key").text) _
                                           = XMLSubNodes.item(z).attributes.getNamedItem("value").text
                    Next
                End If
            End If
        Next
    Next

    ' Create unattended installation file w/ info from dictionary objects
    Set CompFile = objFSO.OpenTextFile(sSAFEDEL & "\oasis.txt",iFORWRITING,bCREATEDOC,bTRISTATETRUE)
    aHeaders = objDictHeader.Keys
    ' Write out data sections for each XMLHeaderName in HeaderDictionary
    For x = 0 To objDictHeader.Count - 1
        CompFile.WriteLine
        CompFile.WriteLine "[" & aHeaders(x) & "]"
        ' Array of Entrys and Values from EntryDictionary per XMLHeaderName
        aEntrys = objDictHeader.Item(aHeaders(x)).Keys
        aValues = objDictHeader.Item(aHeaders(x)).Items
        ' Write each Key/Value pair to CompFile
        For y = 0 To objDictHeader.Item(aHeaders(x)).Count - 1
            sCompOutput = aEntrys(y)
            If (aValues(y) <> Empty) Then
                sCompOutput = sCompOutput & "=" & aValues(y)
            End If
            CompFile.WriteLine sCompOutput
        Next
    Next
    CompFile.Close
    Set objDictHeader = Nothing
End Sub

'================== Get User password from CDS ===============================
' Retrieve User Password from CDS System
' -  32 bit utilizes cdscom.dll; 64 bit uses a database query
Function CDSPassword(sDomain,sUser,sPW)
    If (g_SysProcVar = "X86") Then
        Dim objCDS
        On Error Resume Next
        Set objCDS = WScript.CreateObject("CDSCom.Users")
        CDSPassword = objCDS.GetPassword(sDomain & "\" & sUser)
        ' Deal with any errors from CDS...set default sPassword
        If (objCDS Is Nothing) Or (CDSPassword = Empty) Or (Err <> 0) Then
            CDSPassword = sPW
            EchoAndLog   Space(3) & "Could not retrieve password for " & UCase(g_sOSDomainEN) & _
                         "\" & UCase(sUser) & " from CDS"
            WScript.Echo Space(6) & Err & ": " & Err.Description
        End If
        On Error Goto 0
        Set objCDS = Nothing
    Else
        Dim sSQLQuery
        Dim dbConnect, dbRecordSet
        Const sConnectStr = "DRIVER={SQL Server};Network=dbmssocn;PROVIDER=sqloledb;SERVER=smdata.smx.net;DATABASE=CommonDataStore;UID=cds_ro;PWD=cds_ro"
        On Error Resume Next
        Set dbConnect   = CreateObject("ADODB.Connection")
        Set dbRecordSet = CreateObject("ADODB.Recordset")
        dbConnect.Open sConnectStr
        If Err.Number <> 0 Then
            EchoAndLog   Space(3) & "Could not connect to cds with connection string " & sConnectStr & VbCrLf _
            & Space(3) & "Error number is " & Err.Number & vbTab & "Error message is " & Err.Description
        End If
        sSQLQuery = "exec sp_GetPassword'" & sDomain & "\" & sUser & "';"
        dbRecordSet.Open sSQLQuery, dbConnect
        ' Deal non-existence of user in database...set default sPassword
        If dbRecordSet.EOF And dbRecordSet.BOF Then
            CDSPassword = sPW
            EchoAndLog   Space(3) & "Could not retrieve password for " & UCase(g_sOSDomainEN) & _
                         "\" & UCase(sUser) & " from CDS"
            WScript.Echo Space(6) & "64001:'" & sDomain & "\" & sUser & "' record not in common data store."
        Else
            CDSPassword = dbRecordSet("PASSWORD")
        End If
        dbRecordSet.Close
        dbConnect.Close
        Set dbRecordSet.ActiveConnection = Nothing
        Set dbRecordSet                  = Nothing
        Set dbConnect                    = Nothing
    End If
End Function

'================== Show Usage ===============================================
Sub Usage()
    Dim x, y, z
    Dim XMLEntryNodes, XMLKeyNode
    Dim aXMLUsage

    WScript.Echo
    WScript.Echo
    WScript.Echo "cscript //nologo oasis.vbs testOS [/drive:] [/format:] [/build:] [/domain:]"
    WScript.Echo "                           [/lang:] [/name:] [/type:] [/ver:] [/comp:] [/add:]"
    WScript.Echo "--- Log at c:\logs\oasis.log ---"
    WScript.Echo
    WScript.Echo "    Only the testOS paramter is required (w/ certain exceptions..."
    WScript.Echo "    that will prompt you for the correct additional flag)."
    WScript.Echo "    Examples:"
    WScript.Echo "      oasis.vbs w2k3"
    WScript.Echo "         Install EN Blessed ADS FRE build of W2K3 on current drive partition,"
    WScript.Echo "         with current OS name and common OS components."
    WScript.Echo "         Run no additional parameter on testOS."
    WScript.Echo
    WScript.Echo "      oasis.vbs w2k /comp:iis_common,msmq /add:\\smx.net\tools\pops\test_env.xml /lang:ja /ver:srv"
    WScript.Echo "         Install JA Blessed SRV FRE build of W2K on current drive partition,"
    WScript.Echo "         with current OS name and IIS_Common & MSMQ components installed."
    WScript.Echo "         Run POPS 2.0 on testOS."
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
    WScript.Echo "   ** Invalid option when using UPGRADE **"
    WScript.Echo
    WScript.Echo "   testOS   - (required) OS to install or upgrade scenario"
    WScript.Echo "              W2K,WINXP,W2K3,UPGRADE"
    WScript.Echo "              no default: required input"
    WScript.Echo
    WScript.Echo "   /drive:  - drive on which the new OS will be installed"
    WScript.Echo "   **         (required if not started from testOS)"
    WScript.Echo "              default: %SYSTEMDRIVE%"
    WScript.Echo
    WScript.Echo "   /format: - the format onto which the OS is installed"
    WScript.Echo "   **         NTFS,FAT32,FAT"
    WScript.Echo "              default: NTFS"
    WScript.Echo
    WScript.Echo "   /build:  - build of the new OS"
    WScript.Echo "              Explicit Build Number (####)"
    WScript.Echo "              BLESSED,LATEST"
    WScript.Echo "              .sp# - add to any tag or build# for slipstream OS w/ SP"
    WScript.Echo "              default: BLESSED"
    WScript.Echo
    WScript.Echo "   /domain: - FQDN for domain that the computer joins in new OS"
    WScript.Echo "              otherwise...establishes machine in workgroup"
    WScript.Echo "              default: SMX.NET"
    WScript.Echo
    WScript.Echo "   /lang:   - two letter abbreviation of Language for new OS"
    WScript.Echo "   **         BR   (Brazilian)"
    WScript.Echo "              CN   (Chinese - Simplified)"
    WScript.Echo "              TW   (Chinese - Traditional/Taiwan)"
    WScript.Echo "              HK   (Chinese - Hong Kong)"
    WScript.Echo "              CS   (Czech)"
    WScript.Echo "              EN   (English)"
    WScript.Echo "              FR   (French)"
    WScript.Echo "              DE   (German)"
    WScript.Echo "              HU   (Hungarian)"
    WScript.Echo "              IT   (Italian)"
    WScript.Echo "              JA   (Japanese)"
    WScript.Echo "              KO   (Korean)"
    WScript.Echo "              PL   (Polish)"
    WScript.Echo "              PT   (Portuguese)"
    WScript.Echo "              PS   (Pseudo)"
    WScript.Echo "              RU   (Russian)"
    WScript.Echo "              ES   (Spanish)"
    WScript.Echo "              SV   (Swedish)"
    WScript.Echo "              TR   (Turkish)"
    WScript.Echo "              default: EN"
    WScript.Echo
    WScript.Echo "   /name:   - name of the machine under the new OS"
    WScript.Echo "   **         standard is safeO/S name plus testO/S drive letter appended"
    WScript.Echo "              default: %COMPUTERNAME% + testos drive letter"
    WScript.Echo
    WScript.Echo "   /type:   - type of OS"
    WScript.Echo "   **         FRE,CHK"
    WScript.Echo "              default: FRE"
    WScript.Echo
    WScript.Echo "   /ver:    - version of OS"
    WScript.Echo "   **         PRO,BLA,SRV,DTC,ADS"
    WScript.Echo "              default: SRV"
    WScript.Echo
    WScript.Echo "   /add:    - any XML file (w/ parameters) to be executed after installation"
    WScript.Echo "              for a default test brick please input " & sTOOLSRV & "\pops\test_env.xml"
    WScript.Echo "              default: none"
    WScript.Echo
    WScript.Echo "   /comp:   - OS components that can be installed"
    WScript.Echo "              default: SNMP,NETMONTOOLS,IIS_COMMON,IIS_FTP,IIS_INETMGR,IIS_NNTP,"
    WScript.Echo "                       IIS_NNTP_DOCS,IIS_SMTP,IIS_WWW,MSMQ,MSMQ_CORE,"
    WScript.Echo "                       MSMQ_LOCALSTORAGE,TSENABLE,TSWEBCLIENT,ASPNET,"
    WScript.Echo "                       BITSSERVEREXTENSIONSISAPI,BITSSERVEREXTENSIONSMANAGER"
    ' Gather info from the XML and write out to the CMD window- for Usage purposes
    ' -  only operates when the XML file has been loaded into memory
    If Not(g_XMLRoot Is Nothing) Then
        aXMLUsage = Array("components","netoptionalcomponents")
        For x = LBound(aXMLUsage) to UBound(aXMLUsage)
            WScript.Echo Space(14) & "=== " & UCase(aXMLUsage(x)) & " ==="
            Set XMLEntryNodes = g_XMLRoot.SelectNodes("header[@name='" & LCase(aXMLUsage(x)) & "']/entry")
            For y = 0 To (XMLEntryNodes.Length - 1)
                XMLKeyNode = XMLEntryNodes.item(y).attributes.getNamedItem("key").text
                WScript.Echo Space(18) & XMLKeyNode
            Next
        Next
    End If
    WScript.Echo
    WScript.Echo
End Sub

'================== Append info to OASIS file ================================
' Append information to the log file at sSAFEKEEP\OASIS.log
Sub EchoAndLog(sWriteOut)
    Set g_OasisFile = objFSO.OpenTextFile("c:\logs\oasis.log",iFORAPPENDING,bNOCREATEDOC,bTRISTATETRUE)
    WScript.Echo sWriteOut
    g_OasisFile.WriteLine sWriteOut
    g_OasisFile.Close
End Sub

'================== Quit Program and Close Down ==============================
' Quit script, close objects, and return iError as ErrorLevel
Sub QuitProgram(iError)
    Set objFSO   = Nothing
    Set objShell = Nothing
    Set objXML   = Nothing

    WScript.Quit(iError)
End Sub