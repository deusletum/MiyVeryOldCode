' ----------------------------------------------------------------------
' Name      : domain.vbs
'
' Company   : oration
' 

'
' Summary   : Get the users domain and writes it to c:\delete\domain.bat
'               returns a 1 if there is an error writing the file.
'
' Usage     : cscript.exe //Nologo domain.vbs
'
' History   : 3/14/2003 - deangj - Created
' ----------------------------------------------------------------------
Option Explicit
On Error Resume Next

Const strTxtFile = "c:\delete\domain.bat"

Dim objLocator
Dim objServices
Dim objFSO
Dim objStream
Dim infoSet
Dim info
Dim strDomain

Set ObjLocator = WScript.CreateObject("Wbemscripting.SWbemlocator")
Set ObjServices = ObjLocator.ConnectServer(".", "root\CIMv2")
Set InfoSet = objServices.InstancesOf("Win32_ComputerSystem")

For Each Info In InfoSet
    strDomain = "set DOMAIN=" & Info.Domain
Next

Set objFSO = WScript.CreateObject("Scripting.FileSystemObject")
Set objStream = objFSO.OpenTextFile(strTxtFile, 2, True)
If Err.Number <> 0 Then
    objStream.Close
    WScript.Echo("Unable to write " & strTxtFile )
    WScript.Echo(Err.Description)
    WScript.Quit(1)
End If

objStream.Write(strDomain)
objStream.Close

Set objLocator = Nothing
Set objServices = Nothing
Set objFSO = Nothing
Set objStream = Nothing