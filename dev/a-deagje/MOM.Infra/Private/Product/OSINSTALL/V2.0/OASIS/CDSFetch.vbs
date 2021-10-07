'==========================================================================
'
' NAME: CDSFetch
'
' AUTHOR: AS Test Operations Lab , MS
' DATE  : 2/26/2002
'
' COMMENT: retrieve info from the CDS system based on a user input
'
'==========================================================================
OPTION EXPLICIT

CONST ForWriting = 2
IF (LCASE(RIGHT(Wscript.FullName,11)) = "wscript.exe") THEN
	WScript.Echo "This file must be executed using cscript.exe, please rerun."
	WScript.Quit 1
END IF											' ensure this file is run using cscript



'================== Get variable inputs from user input ==================
IF WScript.Arguments.Count <> 1 THEN			' make sure that there is one input
	Usage()
	WScript.Quit 1
END IF

DIM CDSSearch
'---Required variable inputs---
'CDSSearch  - parameter to search the CDS for
CDSSearch = WScript.Arguments(0)



'================== Create Objects and system Environmental Variables ====
' Create Objects
DIM objFSObject
	SET objFSObject = CREATEOBJECT("Scripting.FileSystemObject")
	
DIM objWshShell
	SET objWshShell = CREATEOBJECT("WScript.Shell")



'================== Get User password from CDS ===========================
DIM CDSPassword

ON ERROR RESUME NEXT							' allow us to deal with any errors from CDS
DIM objCDSUsers									' create CDS object
	SET objCDSUsers = WScript.CreateObject("CDSCom.Users")

CDSPassword = objCDSUsers.GetPassword(CDSSearch)' get info from the CDS

IF (Err <> 0) OR (CDSPassword = "") THEN
	WScript.Echo
	WScript.Echo Err.Description
	IF objCDSUsers IS NOTHING THEN				' CDScom.dll is not registered or on the box
		WScript.Echo "Please make sure CDSCOM.DLL is on the system and registered."
	ELSE										' CDS system is not functioning properly
		WScript.Echo "Please inquire about the CDS system info...SMTWEBREL and SMTEXEC"
	END IF
	WScript.Echo
	WScript.Quit 1
END IF
ON ERROR goto 0


'================== Write out Password to a log file =====================
DIM InfoFile									'write out temp file to store the info we retrieved
	SET InfoFile = objFSObject.OpenTextFile("c:\keep\var.log",ForWriting,TRUE)
	
InfoFile.WriteLine CDSPassword
InfoFile.Close

SET objCDSUsers = NOTHING



'================== Sub process Usage ====================================
SUB Usage()
	WScript.Echo
	WScript.Echo "Retrieves passswords from the CDS..."
	WScript.Echo
	WScript.Echo "Input is Domain\username"
	WScript.Echo "  ex: cscript CDSFetch.vbs smx\asttest"
	WScript.Echo
END SUB
