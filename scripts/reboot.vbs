Option Explicit

Dim oLocator,oServices,oItemSet,oItem,aSplit,sServer,sUser,sPWD
sServer = "vsperfmd05"
sUser = "redmond\vsqa1"
sPWD = "jan05-vs"

' If WScript.Arguments.Count = 0 Then
' End If

Set oLocator = CreateObject("WbemScripting.SWbemLocator")
Set oServices = oLocator.ConnectServer(sServer,"root\CIMV2",sUser,sPWD)
Set oItemSet = oServices.ExecQuery("Select * from Win32_OperatingSystem")
For Each oItem In oItemSet
	oItem.Reboot
Next