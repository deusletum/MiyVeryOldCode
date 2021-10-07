' ----------------------------------------------------------------------
' Name      : Inst_Wsm.vbs
'
' Company   : oration
'

'
' Summary   : Installs Wsm product, with latest or users supplied build and SKU
' Usage     : See usage Function
' History   : 11/18/04  kerrymi  added uninstall functionality and setupcontext
'                                to write component info to config.xml
'-----------------------------------------------------------------------

Main()

' -------------------------------------------------------------------
' summary: EntryPoint for the Script
' -------------------------------------------------------------------
Sub Main()

    Const strDefaultPath = "\\smx.net\builds\wsm\en\"
    Const strCDImage = "\x86\WsmCDImage\MOMWssMP.msi"
    Const strLanguage = "EN"
    Const strSku = "SELECT"
    Const strCOMPONENT_WSMFull = "WSM"
    Const strCOMPONENT_WSMUI = "WSMUI"
    Const strCOMPONENT_WSMServer = "WSMServer"
    Const strCOMPONENT_WSMAgent = "WSMAgent"
    Dim strBuildNum
    Dim strBuildType
    Dim strLocation
    Dim objMachine
    Dim strInstall
    Dim strInstalled
    Set oArguments = WScript.Arguments

    strInstall = "/i "
    strInstalled = "Installed"

    If oArguments.Count < 1 Or oArguments.Count > 4 Then
        WScript.Echo("Incorrect Number of Parameters")
        ShowUsage()
    End If

    If oArguments(0) ="/?" Or oArguments(0) = "-?" Or oArguments(0) = "" Then
        ShowUsage()
    End If

    If oArguments(0) = "latest" Then
        strBuildNum = GetBuildNum("WSM","Latest")
    Else
        strBuildNum = oArguments(0)
    End If

    If oArguments(1) = "retail" Or oArguments(1)= "RETAIL" Then
        strBuildType = "retail"
    Else
        If oArguments(1) = "nonopt" Or oArguments(1)= "NONOPT" Then
            strBuildType = "nonopt"
        Else
            If oArguments(1) = "debug" Or oArguments(1)= "DEBUG" Then
                strBuildType = "debug"
            End If
        End If
    End If

    strParams = "ADDLOCAL=ALL"

    strPath = strDefaultPath & strBuildNum & "\" & strBuildType & strCDImage
    Set objShell = WScript.CreateObject("WScript.Shell")
    strSystemDrive = objShell.ExpandEnvironmentStrings("%SYSTEMDRIVE%")
    strProgramFiles = objShell.ExpandEnvironmentStrings("%PROGRAMFILES%")
    strMSIEXECLog = strSystemDrive & "\logs\" & strBuildNum & "_Install.log "
    strLocation = strProgramFiles & "\Microsoft Web Sites and Services MP"

    'Create the SetupContext objects
    On Error Resume Next
    Set objMachine = WScript.CreateObject("MOM.Test.Common.CMachine")
    'Set objComponent = WScript.CreateObject("MOM.Test.Common.CMachine.Product.Component")
    If Err.Number <> 0 Then
        WScript.Echo("Unable to create SetupContext COM object, Inst_wsm.vbs will not write to the Topology file")
        Err.Clear
    End If
    If Not(objMachine.Product.Exists()) Then
        objMachine.Product.Add()
    End If
    On Error GoTo 0

    If oArguments.Count = 4 Then
        If oArguments(3) = "uninstall" Then
            strInstall = "/x "
            strInstalled = "Uninstalled"
            For Each objComponent In objMachine.Product.Components
                If objComponent.Name = strCOMPONENT_WSMFull Or objComponent.Name = strCOMPONENT_WSMAgent Or objComponent.Name = strCOMPONENT_WSMUI Or objComponent.Name = strCOMPONENT_WSMServer Then
                    objComponent.InstallState = strInstalled
                    objComponent.Build = strBuildNum
                    objComponent.Language = strLanguage
                    objComponent.Flavor = strBuildType
                    objComponent.Sku = strSku
                    objComponent.Location = strLocation
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.SourceDir = strPath
                End If
            Next
        End If
    Else
        If oArguments.Count = 3 Then
            If oArguments(2) = "UI" Or oArguments(2) = "ui" Or oArguments(2) = "Ui" Then
                strParams = "ADDLOCAL=WssMPClient"
                If Not(objMachine.Product.Components.Exists(strCOMPONENT_WSMUI)) Then
                    objMachine.Product.Components.Add(strCOMPONENT_WSMUI)
                End If
                For Each objComponent In objMachine.Product.Components
                    If objComponent.Name = strCOMPONENT_WSMUI Then
                        objComponent.InstallState = strInstalled
                        objComponent.Build = strBuildNum
                        objComponent.Language = strLanguage
                        objComponent.Flavor = strBuildType
                        objComponent.Sku = strSku
                        objComponent.Location = strLocation
                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                        & " " & FormatDateTime(Time, g_iLongTime)
                        objComponent.MSILogPath = strMSIEXECLog
                        objComponent.SourceDir = strPath
                    End If
                Next
            Else
                If oArguments(2) = "AGENT" Or oArguments(2) = "Agent" Or oArguments(2) = "agent" Then
                    strParams = "ADDLOCAL=WssMPAgentConfig"
                    If Not(objMachine.Product.Components.Exists(strCOMPONENT_WSMAgent)) Then
                        objMachine.Product.Components.Add(strCOMPONENT_WSMAgent)
                    End If
                    For Each objComponent In objMachine.Product.Components
                        If objComponent.Name = strCOMPONENT_WSMAgent Then
                            objComponent.InstallState = strInstalled
                            objComponent.Build = strBuildNum
                            objComponent.Language = strLanguage
                            objComponent.Flavor = strBuildType
                            objComponent.Sku = strSku
                            objComponent.Location = strLocation
                            objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                            & " " & FormatDateTime(Time, g_iLongTime)
                            objComponent.MSILogPath = strMSIEXECLog
                            objComponent.SourceDir = strPath
                        End If
                    Next
                Else
                    If oArguments(2) = "SERVER" Or oArguments(2) = "Server" Or oArguments(2) = "server" Then
                        strParams = "ADDLOCAL=WssMPServerConfig"
                        If Not(objMachine.Product.Components.Exists(strCOMPONENT_WSMServer)) Then
                            objMachine.Product.Components.Add(strCOMPONENT_WSMServer)
                        End If
                        For Each objComponent In objMachine.Product.Components
                            If objComponent.Name = strCOMPONENT_WSMServer Then
                                objComponent.InstallState = strInstalled
                                objComponent.Build = strBuildNum
                                objComponent.Language = strLanguage
                                objComponent.Flavor = strBuildType
                                objComponent.Sku = strSku
                                objComponent.Location = strLocation
                                objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                                & " " & FormatDateTime(Time, g_iLongTime)
                                objComponent.MSILogPath = strMSIEXECLog
                                objComponent.SourceDir = strPath
                            End If
                        Next
                    Else
                        If oArguments(2) = "FULL" Or oArguments(2) = "Full" Or oArguments(2) = "full" Then
                            strParams = "ADDLOCAL=ALL"
                            If Not(objMachine.Product.Components.Exists(strCOMPONENT_WSMFull)) Then
                                objMachine.Product.Components.Add(strCOMPONENT_WSMFull)
                            End If
                            For Each objComponent In objMachine.Product.Components
                                If objComponent.Name = strCOMPONENT_WSMFull Then
                                    objComponent.InstallState = strInstalled
                                    objComponent.Build = strBuildNum
                                    objComponent.Language = strLanguage
                                    objComponent.Flavor = strBuildType
                                    objComponent.Sku = strSku
                                    objComponent.Location = strLocation
                                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                                    & " " & FormatDateTime(Time, g_iLongTime)
                                    objComponent.MSILogPath = strMSIEXECLog
                                    objComponent.SourceDir = strPath
                                End If
                            Next
                        Else
                            If oArguments(2) = "uninstall" Then
                                strInstall = "/x "
                                strInstalled = "Uninstalled"
                                For Each objComponent In objMachine.Product.Components
                                    If objComponent.Name = strCOMPONENT_WSMFull Or objComponent.Name = strCOMPONENT_WSMAgent Or objComponent.Name = strCOMPONENT_WSMUI Or objComponent.Name = strCOMPONENT_WSMServer Then
                                        objComponent.InstallState = strInstalled
                                        objComponent.Build = strBuildNum
                                        objComponent.Language = strLanguage
                                        objComponent.Flavor = strBuildType
                                        objComponent.Sku = strSku
                                        objComponent.Location = strLocation
                                        objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                                        & " " & FormatDateTime(Time, g_iLongTime)
                                        objComponent.MSILogPath = strMSIEXECLog
                                        objComponent.SourceDir = strPath
                                    End If
                                Next
                            Else
                                ShowUsage()
                            End If
                        End If
                    End If
                End If
            End If
        Else
            strParams = "ADDLOCAL=ALL"
            If Not(objMachine.Product.Components.Exists(strCOMPONENT_WSMFull)) Then
                objMachine.Product.Components.Add(strCOMPONENT_WSMFull)
            End If
            For Each objComponent In objMachine.Product.Components
                If objComponent.Name = strCOMPONENT_WSMFull Then
                    objComponent.InstallState = strInstalled
                    objComponent.Build = strBuildNum
                    objComponent.Language = strLanguage
                    objComponent.Flavor = strBuildType
                    objComponent.Sku = strSku
                    objComponent.Location = strLocation
                    objComponent.InstallTime = FormatDateTime(Date, g_iShortDate) _
                    & " " & FormatDateTime(Time, g_iLongTime)
                    objComponent.MSILogPath = strMSIEXECLog
                    objComponent.SourceDir = strPath
                End If
            Next
        End If
    End If

    objMachine.CommitLocal

    Install strBuildNum,strPath,strMSIEXECLog,strParams,strInstall

End Sub

' -------------------------------------------------------------------
' summary: gets the build number for the product in strProduct given
'          the build token
' -------------------------------------------------------------------
Function GetBuildNum(strProduct,strBuildToken)
    On Error Resume Next

    Dim oCDS
    Dim strBuild

    Set oCDS = WScript.CreateObject("CDSCom.Builds")
    if Err.Number <> 0 Then
        WScript.Echo("Error while accessing CDSCom cannot get build number")
        WScript.Quit(1)
    End If

    strBuild = oCDS.GetLatestBuildByToken(strProduct,strBuildToken)

    If strBuild = "" Then
        WScript.Echo("Error while accessing CDSCom cannot get build number")
        WScript.Quit(1)
    End If

    GetBuildNum = strBuild

    Set oCDS = Nothing

End Function


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
' summary: installs an msi based install, checks for errors and the Quits the script
' -------------------------------------------------------------------
Sub Install(strVersion, strPath, strMSILog, strParms, strInstall)
    Const intReboot = 1641
    Const iMSIReboot = 6
    Const iMSIError =5
    Const iNoError = 0

    Dim strCommand
    Dim intError

    strCommand = "msiexec.exe " & strInstall & strPath & " /lv* " & strMSILog & strParms & " /Qn"

    intError = RunCMD(strCommand)
    If intError = intReboot Then
        WScript.Echo( "VAR_FAIL - Install for " & strVersion _
            & " passed but requires a reboot")
    ElseIf intError <> 0 Then
        WScript.Echo("VAR_FAIL - Install for " & strVersion _
            & " failed with error number " & intError)
    Else
        WScript.Echo ("VAR_PASS - Install for " & strVersion _
            & " passed")
    End If

End Sub


' -------------------------------------------------------------------
' summary: Displays the USAGE to user
' -------------------------------------------------------------------

Sub ShowUsage()

    WScript.Echo("Usage:")
    Wscript.Echo()
    WScript.Echo("cscript.exe Inst_WSM.vbs [Build Number] [Build Type] {Component} {uninstall}")
    Wscript.Echo()
    Wscript.Echo("where...")
    WScript.Echo("   Build Number = 2810..or latest")
    WScript.Echo("   Build Type = retail, nonopt, debug")
    Wscript.Echo()
    WScript.Echo("   (If Only 2 parameters are provided, WSM Complete (Full) is installed)")
    Wscript.Echo()
    WScript.Echo("Component (optional) = Full, Server, Agent, or UI")
    WScript.Echo("   UI --> Installs WSM Client Only")
    WScript.Echo("   Agent --> Install WSM Agent Component")
    WScript.Echo("   Server --> Installs WSM Server Component")
    WScript.Echo("   Full --> Installs WSM Complete (all 3 components)")
    Wscript.Echo()
    WScript.Echo("uninstall (optional), if provided, must be last argument ...")
    WScript.Echo("  i.e.")
    WScript.Echo("   if you have 2 args, uninstall can be a 3rd arg")
    WScript.Echo("   if you have 3 args, uninstall can be a 4th arg")

    WScript.Quit(1)
End Sub
