Option Explicit

Const sProBuildFile = "vs_pro_enu_x86_cd.sem"
Const sVSTSBuildFile = "vs_vsts_enu_x86_cd.sem"
Const sNetPath = "\\cpvsbuild\Drops\whidbey\lab22dev\layouts\x86ret\current"
Const iForReading = 1
Const bCreate = False
Const iUnicode = -1
Const sPattern = "revision="

GetBuildNum "PRO"
GetBuildNum "VSTS"

Function GetBuildNum(ByVal sSku)
	Dim oFSO,oShell,sProBuildNum,sVSTSBuildNum,oStream,sContents,oRegEx
	Dim oMatches,oMatch,sFile
	
	sFile = sProBuildFile
	If (UCase(sSku) = "VSTS") Then
		sFile = sVSTSBuildFile
	End If
	
	Set oRegEx = New RegExp
	oRegEx.Pattern = sPattern
	oRegEx.IgnoreCase = True
	
	Set oFSO = WScript.CreateObject("Scripting.FileSystemObject")
	If (oFSO.FileExists(sNetPath & "\" & sFile)) Then
		Set oStream = oFSO.OpenTextFile(sNetPath & "\" & sFile,iForReading)
		Do While Not oStream.AtEndOfStream
			sContents = oStream.ReadLine
			If (oRegEx.Test(sContents)) Then
				sProBuildNum = sContents
			End If
		Loop
		oStream.Close()
		WScript.Echo("Current " & sSku & " Build Number is: " & oRegEx.Replace(sProBuildNum,""))
	Else
		WScript.Echo sNetPath & "\" & sFile & " does not exist"
	End If
End Function