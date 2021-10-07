Option Explicit
const ForAppending = 8

' -------------------------------------------------------------------
' summary:
'    the IsCscript function checks to ensure that the host 
'    is CScript.exe
'
' returns:
'    True if the host is CScript.exe, False if it is WScript.exe
'
' history:
'    13-Sep-01 marking First Creation
'    11-Nov-02 Changed name per code review
' -------------------------------------------------------------------
Function IsCscript ()
    If (InStr(1, WScript.FullName, "CSCRIPT", vbTextCompare) <> 0) Then
        IsCscript = True
    Else
        IsCscript = False
    End If
End Function

' -------------------------------------------------------------------
' summary:
'    the Usage function will display usage information and Set
'    %ERRORLEVEL% to 1 which means that the command line arguments
'    are not correct
'
' returns:
'    Nothing
'
' history:
'    05-Nov-02 deangj First Creation
'    13-Nov-02 deangj remove % from all input parms
'        removed the "." and replace with blank line
'        changed WHS requirenment to 5.1
'    12-Dec-03 deangj Changed usage to be displayed on the one line
' -------------------------------------------------------------------
Sub Usage
    Dim strUsageMsg
    strUsageMsg = "MachineInfo.vbs will collect computer information " & _
    "and store it in Machinelib" & vbCrlf & "MachineInfo.vbs must be run " & _ 
    "under cscript.exe and" & vbCrlf & vbTab & "requires Windows Script " & _ 
    "Host 5.1" & vbCrlf & "Usage:" & vbCrlf & "" & vbCrlf  & "cscript.exe " & _ 
    "MachineInfo.vbs asset location owner purpose" & vbCrlf & "asset is a 6 " & _ 
    "digit Microsoft Asset Tag number located on the computer " & vbCrlf & _ 
    "location is the location of the computer" & vbCrlf & "owner is your " & _ 
    "alias, asttest will not be accepted" & vbCrlf & "purpose must be one " & _ 
    "of these values:" & vbCrlf & vbTab & "DevMachine, TestMachine, " & _ 
    "MailMachine, CoreServer, Server" & vbCrlf & "Example:" & vbCrlf & _ 
    vbTab & "cscript.exe MachineInfo.vbs 587904 44/1019 deangj TestMachine"
    WScript.Echo(strUsageMsg)
    WScript.Quit(1)
End Sub

' -------------------------------------------------------------------
' summary:
'    the GetMachineLibInfo function will Get machine info for the
'    computer named in strMachineName
'
' param:
'    param name="strMachineName" dir="IN" 
'    a computer name
'
' returns:
'    Returns XML DOMDocument which contains XML machine info for the computer
'    if it exist in machinelib If GetMachineLibInfo DOMDocument.xml is
'    "<root></root>" then there is no entry for the computer in Machinelib
'
' history:
'    05-Nov-02 deangj First Creation
'    13-Nov-02 deangj Changed var prefix to xml for xml obj and web
'       web obj
'    12-Dec-03 deangj Changed the web server name to smdits
' -------------------------------------------------------------------
Function GetMachineLibInfo(strMachineName)
    'On Error Resume Next
    Const GetInfoWeb = "http://acdits/MachineList.asp"
    
    Dim webHttpGetData
    Dim xmlData
    
    Set webHttpGetData = CreateObject("MSXML2.XMLHTTP")

    
    webHttpGetData.Open "POST", GetInfoWeb, False
    webHttpGetData.Send "<root><MACHINE name=""" & strMachineName & """/></root>"

    Set xmlData = webHttpGetData.responseXML

    Set GetMachineLibInfo = xmlData
    
    Set xmlData = Nothing
    Set webHttpGetData = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    the UpdateMachine function will submit the contents of 
'    strXMLData to MachineLib
'
' param:
'    param name="strXMLData" dir="IN" 
'    this is a string of XML Data that is the computers information
'
' returns:
'    Returns XML DOMDocument. This DOMDocument should be parsed for an Error
'    attribute in the machine element
'
' history:
'    05-Nov-02 deangj First Creation
'    13-Nov-02 deangj Changed var prefix to xml for xml obj and web
'       web obj
'       Combinded the function DeleteMachine into UpdateMachine
'    12-Dec-03 deangj Changed the web server name to smdits
' -------------------------------------------------------------------
Function UpdateMachine(strXMLData)
    'On Error Resume Next
    Const UpdateWeb = "http://acdits/MachineUpdate.asp"
    
    Dim webHttpUpdate
    Dim xmlData
    
    Set webHttpUpdate = CreateObject("MSXML2.XMLHTTP")    

    webHttpUpdate.Open "POST", UpdateWeb, False
    webHttpUpdate.Send strXMLData
    
    Set xmlData = webHttpUpdate.responseXML
    Set UpdateMachine = xmlData
    
    Set xmlData = Nothing
    Set webHttpUpdate = Nothing
End Function

' -------------------------------------------------------------------
' summary:
'    the main function gets everything started.
'
'returns:
'    Nothing
'
' history:
'    05-Nov-02 deangj First Creation
'    13-Nov-02 deangj Changed if <> statments to if Not
'       in if Not statments I put the smaller code first to save excution time
'    12-Dec-03 deangj Changed the cases in select case to Uppercase
'    12-Dec-03 deangj Corrected the spelling of already
' -------------------------------------------------------------------
Sub Main()
    On Error Resume Next

    Dim strMessage
    Dim objEnv
    Dim CMDArgs
    Dim XMLData
    Dim bCorrectArgs
    Dim strCompName
    Dim objXMLData
    Dim strXMLData
    Dim objMachNode
    Dim objXMLError
    Dim strWMIData
    
    ' ------------------------------------------------------------------------------------------
    ' Comment:
    '    Check if script engine is cscript. If not cscript exit.
    ' ------------------------------------------------------------------------------------------
    If (Not IsCscript()) Then
        Usage
    End If
    
    If WScript.Version < "5.1" Then
        Usage
    End If
        
    ' ------------------------------------------------------------------------------------------
    ' comment:
    '    Parse Command Line arguments
    ' ------------------------------------------------------------------------------------------
    Set CMDArgs = WScript.Arguments
    If CMDArgs.count <> 1 Then
        Usage
    End If
    
    strCompName = WScript.Arguments(0)    
         
	' -----------------------------------------------------------------------------------
	' comment: 
	'	Delete the entry
	' -----------------------------------------------------------------------------------
	
	Set objXMLData = UpdateMachine("<root><MACHINE name=""" & strCompName & _
		""" action=""delete""/></root>")
	If Instr(Ucase(objXMLData.xml), "ERROR") Then
		Set objMachNode = objXMLData.selectSingleNode("//MACHINE")
		XMLError = objMachNode.getAttribute("error")
		If Len(XMLError) > 0 Then
	   		WriteLog("There was an error: " & XMLError)
	   		WScript.Quit(4)
		End If
	End If
	WScript.Echo("Computer: " & strCompName & " entry was deleted from MachineLib")    
	
	Set objXMLData = Nothing
	Set objMachNode = Nothing

End Sub

Call Main()