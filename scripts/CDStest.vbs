Option Explicit
Dim objShell
Dim strUser
Dim strDomain
Dim objCDS
Dim objCDSBuild

If (LCase(Right(WScript.Fullname,11)) = "WScript.exe") Then
	wscript.echo "this file must be executed using cscript.exe, please rerun."
	wscript.quit 1
End If

Set objShell = WScript.CreateObject("WScript.Shell")
Set objCDS = WScript.CreateObject("CDSCom.Users")
Set objCDSBuild = WScript.CreateObject("CDSCom.Builds")

WScript.Echo("the password for smx\asttest is " & objCDS.GetPassword("smx\asttest"))
WScript.Echo("the password for smx\momdas is " & objCDS.GetPassword("smx\momdas"))
WScript.Echo("the password for smx\momactionact is " & objCDS.GetPassword("smx\momactionact"))
WScript.Echo("the password for smx\momactionactlowpriv is " & objCDS.GetPassword("smx\momactionactlowpriv"))
WScript.Echo("the password for smx\momdts is " & objCDS.GetPassword("smx\momdts"))
WScript.Echo("the password for smx\momreporting is " & objCDS.GetPassword("smx\momreporting"))
WScript.Echo("MOMX latest build is " & objCDSBuild.GetLatestBuildByToken("MOMX", "LATEST"))

Set objCDS = Nothing
Set objShell = Nothing