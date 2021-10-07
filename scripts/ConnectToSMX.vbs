Option Explicit

Const sUserName = "smx\asttest"
Dim sPassword, oCDS, oNetwork

If (LCase(Right(WScript.Fullname,11)) = "WScript.exe") Then
	wscript.echo "this file must be executed using cscript.exe, please rerun."
	wscript.quit 1
End If

Set oNetwork = WScript.CreateObject("Wscript.Network")
Set oCDS = WScript.CreateObject("CDSCom.Users")

sPassword = oCDS.GetPassword(sUserName)
oNetwork.MapNetworkDrive "","\\smx.net\tools", False, sUserName, sPassword
oNetwork.MapNetworkDrive "","\\smx.net\builds", False, sUserName, sPassword
oNetwork.MapNetworkDrive "","\\smx.net\drop", False, sUserName, sPassword
oNetwork.MapNetworkDrive "","\\smx.net\products", False, sUserName, sPassword
oNetwork.MapNetworkDrive "","\\smx.net\Logs", False, sUserName, sPassword