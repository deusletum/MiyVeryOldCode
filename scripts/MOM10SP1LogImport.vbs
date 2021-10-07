' ======================================================================
' Module  : mom10sp1_logimp.vbs
'
' Abstract: Creates Test Run, and imports logs for MOM10SP1
'
' Creator : Dean Gjedde (deangj)
'
' History : (10/22/02) Dean Gjedde (deangj) - Created
'
' Copyright . 2002
'
' Credit : Most code leveaged for John Heaton (jheat)
'
' ======================================================================

' ======================================================================
' Set Database and User Information for Test Run
' ======================================================================
Option Explicit

Const csConnectUser = "redmond\astinfra"
Const csDBConnect = "Driver={SQL Server};Server=SMDATA;Database=RUNS2_MOM10SP1;UID=RUNS2_MOM10SP1_userrw;PASSWORD=SMX#2001"
Const csSiteName = "MOM10SP1"

' ======================================================================
' Define variables to be used
' ======================================================================
Dim objLogImp 
Dim objCDSCOM 
Dim Args
Dim dbCon 
Dim dbRs
Dim strPwd
Dim strXml
Dim strSql
Dim intRunID
DIM intMOMBuild
Dim strMOMBuild
Dim intBuildTypeID
Dim strNotes
Dim Quote

' ======================================================================
' Check for required parameters
' ======================================================================
Quote = Chr(34)

Set Args = WScript.Arguments
If Args.Count <> 4 then
	Wscript.Echo "Ussage:" 
	Wscript.Echo "cscript mom10sp1_logimp.vbs %BUILD#% %OSTYPE% %XMLLocation% %NOTES%"
	Wscript.Echo "Example: cscript mom10sp1_logimp.vbs 1234 W2K C:\logs\TestResults.xml " & Quote & "W2K SP3 EN" & Quote
Else
	Set dbCon = CreateObject("ADODB.Connection")
	Set dbRs = CreateObject("ADODB.Recordset")
'End If

' ======================================================================
' Declare values from inputed information
' ======================================================================
intMOMBuild = "1"
strMOMBuild = Args.Item(0)
intBuildTypeID = Args.Item(1) 
strXml = Args.Item(2)
strNotes = Args.Item(3)

' ======================================================================
' Creates test run based on information inputed
' ======================================================================
	'Connect to the database
	Wscript.Echo "Connecting to the MOM10SP1 Test Runs Database"
	dbCon.Open csDBConnect
	
	'Creates TestRun with proper information.
	Wscript.Echo "Creating new test run record."
	strSql = "Insert RUNS(ACBUILD_ID, ACBUILD, BUILDTYPE_ID, NOTES) VALUES(" & intMOMBuild & ",'" & strMOMBuild & "', " & intBuildTypeID & ",'" & strNotes & "')"
	dbCon.Execute strSql
	
	'Close database connection
	dbCon.Close
	
' ======================================================================
' Connect to database and select top (newly created) run.
' ======================================================================
	'Connects to database and selects top run
	dbCon.Open csDBConnect
	strSql = "Select Top 1 RUN_ID From Runs Order By RUN_ID Desc"
	dbRs.Open strSql, dbCon
	intRunID = dbRs("RUN_ID")
	dbRs.Close
	
' ======================================================================
' Set run as modified despite fact that no information has been put in yet
' Then closes database connection
' ======================================================================	
	strSql = "Update RUNS Set MODIFIED_ON = getdate() Where RUN_ID=" & intRunID
	dbCon.Execute strSql
	
	'Close database connection
	dbCon.Close
	
	'Clear out procedures
	Set dbRs = Nothing
	Set dbCon = Nothing
	
' ======================================================================
' Import the log XML file
' ======================================================================	
	'Set username and password info
	Set objCDSCOM = CreateObject("CDSCOM.Users")
	strPwd = objCDSCOM.GetPassword(csConnectUser)	
		
	'Define object to call into logimporter.dll
	Set objLogImp = CreateObject("LogImporter2.LogImp")
	
	Wscript.Echo "Importing log files ..."
	objLogImp.ImportLogs strXml, csConnectUser, strPwd, intRunID, csSiteName, csDBConnect
	Wscript.Echo "Log files imported"
end if