' ======================================================================
' Copyright . 2004
'
' Module : EnableBrowserService.vbs
'
' Summary: Enables Browser Service if machine is joined to Bowmore domain
'
' Exit Codes: 1 - Incorrect parameter/Description
'             2 - Failed to write reg key
'             3 - Failed to set Browser service to auto start
'             4 - Failed to start Browser service
'
' History: (6/30/2004) Dean Gjedde - Initial coding bug 3679
' ======================================================================
Option Explicit
Const sTargetDomain = "BOWMORE"

Dim sLogInfo, iError
iError = Main()
WriteLog(sLogInfo & vbCrlf & "Exit code: " & iError)
WScript.Quit(iError)

Function Main()
    Const sKey = "HKLM\SYSTEM\CurrentControlSet\Services\Browser\Parameters\MaintainServerList"
    Const sKeyValue = "Yes"
    Const iW2K3 = 3790
    Const iUserError = 1
    Const iRegWriteError = 2
    Const iAutoStartError = 3
    Const iStartService = 4

    Dim oLocator, oServices, oInfoSet, oInfo, sDomain
    Dim oShell, iErr

    'Check if script is being run under CScript.exe
    If Not((InStr(1, WScript.FullName, "CSCRIPT", vbTextCompare) <> 0)) Then
        WScript.Echo "This script must be run with CScript.exe"
        Main = iUserError
        Exit Function
    End If

    If (WScript.Arguments.Count <> 0)Then
        Usage()
        Main = iUserError
        Exit Function
    End If

    'Get the domain the computer is joined to
    Set oLocator = CreateObject("Wbemscripting.SWbemlocator")
    Set oServices = oLocator.ConnectServer(".", "root\CIMv2")
    Set oInfoSet = oServices.InstancesOf("Win32_ComputerSystem")
    For Each oInfo In oInfoSet
        sDomain = UCase(oInfo.Domain)
        sLogInfo = "Computer's domain is: " & sDomain
    Next

    If sDomain <> sTargetDomain Then
        Main = 0
        sLogInfo = sLogInfo & vbCrlf & "Computer domain is not " & sTargetDomain
        Exit Function
    End If

    'Write to registy to enable browser
    Set oShell = WScript.CreateObject("WScript.Shell")
    iErr = oShell.RegWrite(sKey,sKeyValue)
    If iErr <> 0 Then
        sLogInfo = sLogInfo & vbCrlf & "Failed to write reg key"
        Main = iRegWriteError
        Exit Function
    End If

    'set browser service to autostart
    Set oInfoSet = oServices.ExecQuery("SELECT * FROM Win32_Service WHERE Name = 'browser'")
    For Each oInfo In oInfoSet
        If Not UCase(oInfo.startMode) = "AUTOMATIC" Then
            iError = oInfo.ChangeStartMode("AUTOMATIC")
        End If
        If iError <> 0 Then
            sLogInfo = sLogInfo & vbCrlf & "Failed to set browser service to autostart"
            Main = iAutoStartError
            Exit Function
        End If

        'start browser service
        If Not UCase(oInfo.state) = "RUNNING" Then
            iError = oInfo.StartService()
        End If
        If iError <> 0 Then
            sLogInfo = sLogInfo & vbCrlf & "Failed to start browser service"
            Main = iStartService
            Exit Function
        End If
    Next

    sLogInfo = sLogInfo & vbCrlf & "EnableBrowserService.vbs completed successfully"
    Main = 0
    Set oShell = Nothing
    Set oInfoSet = Nothing
    Set oServices = Nothing
    Set oLocator = Nothing
End Function

' Writes a log file
Sub WriteLog(sMessage)
    'On Error Resume Next
    Const iShortDate = 2
    Const iForAppending = 8

    Dim sLogFile, oEnv, oFSO, oStream, oShell
    Dim sSystemRoot, sArch

    Set oFSO = CreateObject("Scripting.FileSystemObject")
    Set oShell = CreateObject("WScript.Shell")
    sSystemRoot = oShell.ExpandEnvironmentStrings("%SYSTEMROOT%")
    sLogFile = sSystemRoot & "\Logs\EnableBrowserService.log"
    If Not(oFSO.FolderExists(sSystemRoot & "\Logs")) Then
        oFSO.CreateFolder sSystemRoot & "\Logs"
    End If
    Set oStream = oFSO.OpenTextFile(sLogFile, iForAppending, True)
    If Err.Number <> 0 Then
        oStream.Close
        WScript.Echo("EnableBrowserService.vbs was unable to write a log")
        WScript.Echo(Err.Description)
        Exit Sub
    End If

    oStream.Write(vbCrlf & "*LOG_START*-EnableBrowserService")
    oStream.Write(vbCrlf & "[" &  FormatDateTime(Date, iShortDate) & " " & _
        FormatDateTime(Time, iShortDate)  & "] EnableBrowserService.vbs "  & vbCrlf & sMessage)
    oStream.Write(vbCrlf & "*LOG_DONE*")
    oStream.Close
    WScript.Echo(sMessage)

    Set oFSO = Nothing
    Set oStream = Nothing
End Sub

'Displays usage
Sub Usage()
    WScript.Echo "Usage:"
    WScript.Echo ""
    WScript.Echo "EnableBrowserService.vbs will enable and start the Browser service on a"
    WScript.Echo "  computer joined to the " & sTargetDomain & " domain"
End Sub