' ============================================================================
' Copyright . 2004
'
' Module : autologit.vbs
'
'          ErrorCodes:
'            0 - Nothing is wrong
'            1 - Incorrect parameter/Usage
'            2 - Failed to write registry entries
'
' Summary: Setup autologon for lab machines
'
' History: (08/10/2003) unknown  - First Creation
'          (01/26/2004) Deangj   - General Code Cleanup
' ============================================================================
Option Explicit
Const sKey = "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon"
Const sRegType = "REG_SZ"
Dim sPassword, sUser, sDomain

If (LCase(Right(Wscript.FullName,11)) <> "cscript.exe") Then
    WScript.Echo "This file must be executed using cscript.exe...please rerun."
    WScript.Quit(1)     ' script was not executed using cscript
End If

If WScript.Version < CInt(5.0) Then
    WScript.Echo "This file must be executed using WSH 5.0 or greater"
    WScript.Quit(1)     ' script was not executed on proper cscript version
End If

If WScript.Arguments(0) = "/?" Then
    Usage
    WScript.Quit(1)
End If

If WScript.Arguments.Count = 2 Then
    Usage
    WScript.Quit(1)
End If

sPassword = WScript.Arguments(0)
sUser = WScript.Arguments(1)
sDomain = WScript.Arguments(2)

RegWrite sKey & "\AutoAdminLogon", 1, sRegType
RegWrite sKey & "\DefaultUserName", sUser, sRegType
RegWrite sKey & "\DefaultDomainName", sDOMAIN, sRegType
RegWrite sKey & "\DefaultPassword", sPassword, sRegType

' ============================================================================
' RegWrite rights registry entries
' ============================================================================
Sub RegWrite(sKey, sValue, sRegType)
    On Error Resume Next
    Dim oShell

    Set oShell = CreateObject("WScript.Shell")
    oShell.RegWrite sKey, sValue, sRegType
    If Err.Number <> 0 Then
        WScript.Echo("Failed to write " & sKey & " with value: " & sValue)
        WScript.Quit(2)
    End If
End Sub

' ============================================================================
' RegWrite rights registry entries
' ============================================================================
Sub Usage()
    WScript.Echo ""
    WScript.Echo "Usage:"
    WScript.Echo ""
    WScript.Echo "cscript.exe //nologo autologit.vbs PASSWORD USER FQDNDOMAIN"
    WScript.Echo ""
    WScript.Echo "Example: cscript.exe //nologo Autologon.cmd MyPass ASTTEST SMX.NET"
    WScript.Echo ""
    WScript.Echo "Required Parameters:"
    WScript.Echo vbTab & "PASSWORD, USERNAME, FQDNDOMAIN"
End Sub