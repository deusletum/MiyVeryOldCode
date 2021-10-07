Option Explicit

Function WriteFile(strLogTxt, strFile)

	'On Error Resume Next
	
	Dim objScriptShell
	Dim objFileSystem
	Dim objCreateFile
	Dim objFileStream
	
	Const intOpenAs = 2
	Const intASCII = "-2"
	
	'<comment>Create WScript Filesystem Object</comment>
	Set objFileSystem = CreateObject("Scripting.FileSystemObject")
	
	'<comment>Create the log file</comment>
	If Not objFileSystem.FileExists(strFile) Then
		Set objCreateFile = objFileSystem.CreateTextFile(strFile, True)
		objCreateFile.Close
	Else
		objFileSystem.DeleteFile(strFile)
		Set objCreateFile = objFileSystem.CreateTextFile(strFile, True)
		objCreateFile.Close
	End If			
	
	'<comment>Set the file for writing and append</comment>
	Set objFileStream = objFileSystem.OpenTextFile(strFile, intOpenAs, True, intASCII)
	If Err.Number <> 0 Then
		WriteFile = 1
		objFileStream.Close
		WScript.Echo "There was an error in WriteFile"
		WScript.Echo "strLogTxt is " & strLogTxt
		WScript.Echo "strFile is " & strFile
		WScript.Echo "Error Info: " & err.Description & " err.number " & err.Number & " err.source " & err.Source 
		err.Clear	
	Else
		objFileStream.Write (strLogTxt)
		objFileStream.Close
		WriteFile = 0	
	End If
	
	Set objScriptShell = Nothing
	Set objFileSystem = Nothing
	Set objCreateFile = Nothing
	Set objFileStream = Nothing

End Function

'******************************************************************************
' Main
'******************************************************************************

Dim objShell
Dim strUser
Dim strDomain
Dim objCDS
Dim strPassword

If (LCase(Right(WScript.Fullname,11)) = "WScript.exe") Then
	wscript.echo "this file must be executed using cscript.exe, please rerun."
	wscript.quit 1
End If

Set objShell = WScript.CreateObject("WScript.Shell")

Set objCDS = WScript.CreateObject("CDSCom.Users")

strPassword = objCDS.GetPassword("smx\asttest")

WriteFile  "Set AsttestPassword=" & strPassword, "c:\delete\password.bat"

Set objCDS = Nothing
Set objShell = Nothing