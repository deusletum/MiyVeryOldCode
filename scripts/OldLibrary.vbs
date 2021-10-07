Set objArgs = Wscript.Arguments



Const szDomain = "SMX"
Const szAccount = "asttest"

If objArgs.Count > 3 Then

	WScript.Echo "Machine Library update on server " & objArgs(0)
	WScript.Echo ""
	WScript.Echo "Machine......." & objArgs(1)
	WScript.Echo "Owner........." & objArgs(2)
	WScript.Echo "Location......" & objArgs(3)
	If objArgs.Count > 4 Then
		WScript.Echo "Machine Pools"
		For i = 4 to objArgs.Count - 1
			WScript.Echo "              " & objArgs(i)
		Next
	End If
	WScript.Echo ""
	
	Rem Add the pools. This fails if they're already there, but so what?
	For i = 4 to objArgs.Count - 1
		Set xmlHttpUpdate = CreateObject("Microsoft.XMLHTTP")
		xmlHttpUpdate.Open "POST", "http://" & objArgs(0) & "/MachineUpdate.asp", false 
		xmlHttpUpdate.Send "<root><POOL action=""add"" name=""" & objArgs(i) & """/></root>"
	Next

	Set objCDSUsers = CreateObject("CDSCom.Users")
	Rem Get the password for astdits
	On Error Resume Next
	Do
		szPassword = objCDSUsers.GetPassword(szDomain & "\" & szAccount)
		If Err > 0 Then
			WScript.Echo Err.Description
		End If	
	Loop Until Err = 0
	On Error Goto 0

	Set xmlHttpList = CreateObject("Microsoft.XMLHTTP")
	xmlHttpList.Open "POST", "http://" & objArgs(0) & "/MachineList.asp", false 
	Set xmlHttpGetData = CreateObject("Microsoft.XMLHTTP")
	xmlHttpGetData.Open "POST", "http://" & objArgs(0) & "/MachineData.asp", false 
	Set xmlHttpUpdate = CreateObject("Microsoft.XMLHTTP")
	xmlHttpUpdate.Open "POST", "http://" & objArgs(0) & "/MachineUpdate.asp", false 
	Rem WScript.Echo "Processing machine " & objArgs(1) & "."
	xmlHttpList.Send "<root><MACHINE name=""" & objArgs(1) & """/></root>"
	Set xmlData = xmlHttpList.responseXML
	REM WScript.Echo xmlData.xml
	Set objMachine = xmlData.selectSingleNode("//MACHINE")
	Rem Determine if we REALLY got one
	On Error Resume Next
	name =  objMachine.getAttribute("name")
	if Err = 0 Then
		sAction = "autoupdate"
		WScript.Echo "Library entry for " & objArgs(1) & " will be updated."
		Rem Delete if already there because update ignores some things
		sAction = "add"
		objMachine.setAttribute "action","delete"
		xmlHttpUpdate.Send xmlData
		Set xmlHttpUpdate = CreateObject("Microsoft.XMLHTTP")
		xmlHttpUpdate.Open "POST", "http://" & objArgs(0) & "/MachineUpdate.asp", false 
	Else
		sAction= "add"
		WScript.Echo "Library entry for " & objArgs(1) & " will be added."
	End If
	On Error Goto 0
	REM WScript.Echo "Calling GetMachineData for " & objArgs(1) 
	xmlHttpGetData.Send "<root><MACHINE name=""" & objArgs(1) & """ user="""  & szAccount & """ domain=""" & szDomain & """ password=""" & szPassword & """/></root>"
	Set xmlData = xmlHttpGetData.responseXML
	REM WScript.Echo xmlData.xml
	Set objMachine = xmlData.selectSingleNode("//MACHINE")
	objMachine.setAttribute "action",sAction
	objMachine.setAttribute "owner",objArgs(2)
	objMachine.setAttribute "location",objArgs(3)
	Set objDescription = xmlData.createElement("DESCRIPTION")
	objDescription.Text = ""
	objMachine.AppendChild objDescription
	Set objDescription = xmlData.createElement("NOTES")
	objDescription.Text = "[Not Specified]"
	objMachine.AppendChild objDescription
	For i = 4 to objArgs.Count - 1
		Rem Handle the machine pools
		Set objPool = xmlData.createElement("POOL")
		objPool.Text = objArgs(i)
		objMachine.AppendChild objPool
	Next

	Rem Now look at the disk partitions and mark one for reformatting
	Set objDisks = objMachine.getElementsByTagName("DISK")
	For Each objDisk In ObjDisks
		Rem WScript.Echo "Disk Partition " & objDisk.getAttribute("letter")
		If objDisk.getAttribute("letter") = "D:" Then
			sStatus = "Reformat"
			WScript.Echo "Reformat Disk Partition " & objDisk.getAttribute("letter")
		Else
			sStatus = "Do not use"
		End If
		objDisk.setAttribute "status",sStatus
	Next			
	Rem WScript.Echo "Calling UpdateLibrary for " & objArgs(1) 
	Rem xmlData.Save "C:\Temp\" & objArgs(1) & ".xml"
	xmlHttpUpdate.Send xmlData
	Set xmlData = xmlHttpGetData.responseXML
	REM WScript.Echo xmlData.xml
	Set objMachine = xmlData.selectSingleNode("//MACHINE")
	error = objMachine.getAttribute("error")
 	If Len(error) > 0 Then
		WScript.Echo "UpdateLibrary error for " & objArgs(1) & " " & error
	Else
		WScript.Echo "UpdateLibrary successful for " & objArgs(1) & " " & error
 	End If

Else

	WScript.Echo WScript.ScriptName & " DITS_Server machine_name owner location [pool_name pool_name ...]"
	WScript.Echo ""
	WScript.Echo ""

End if

