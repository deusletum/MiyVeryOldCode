' ============================================================================
' Copyright . 2004
'
' Module : Set.vbs
'
' Summary: Designed to create connections to Server Machines
' Dependencies: addsmxdns.vbs, cdscom.dll
'          (uses either a test team Generic Account or specified user input)
'          ErrorCodes:
'            0 - Nothing is wrong
'            1 - User input is wrong
'            2 - a file/folder could not be located
'            3 - some action could not be executed
'                ...could not contact CDS,could not parse XML file,etc.
'
' History: (08/10/2003) Jennla - Initial coding
'          (12/02/2003) Jheat  - Updated to include legacy servers
'          (01/16/2004) Deangj - fixed problem with copying set.vbs when set.vbs
'                                   already on the box and read only
'          (01/22/2204) Deangj - Changed the Set.log to unicode and changed the
'                                   cds user name and password to cds_ro
'          (01/22/2004) Deangj - Changed the connect to server retry to 2
'          (03/30/2004) Deangj - removed \\arch\archive for redmond list
'          (06/21/2004) Jheat  - Added \\arch\archive for redmond list
' ============================================================================
Option Explicit

' Common Variables
Const sUSERPWR      = "asttest"
Const sPWPWR        = "Mars#01"

' Server Names
Const sTOOLSRV      = "\\smx.net\tools"
Const sSAFEKEEP     = "c:\keep"
Const sSAFEDEL      = "c:\delete"

' Method Argument inputs
Const iWINMIN       = 2
Const iFORWRITING   = 2
Const iFORAPPENDING = 8
Const iSUBSTRINGS   = -1
Const bWINWAIT      = True
Const bCREATEDOC    = True
Const bNOCREATEDOC  = False
Const bOVERWRITE    = True
Const bTRISTATETRUE = True

'================== Create Common Objects ====================================
Dim objFSO
Set objFSO     = CreateObject("Scripting.FileSystemObject")

Dim objShell
Set objShell   = CreateObject("WScript.Shell")

Dim objNetwork
set objNetwork = CreateObject("Wscript.Network")

' Common System Variables
Dim g_SysRoot
g_SysRoot      = UCase(objShell.Environment("process").Item("SystemRoot"))

Dim g_SysProcVar
g_SysProcVar   = UCase(objShell.Environment.Item("Processor_Architecture"))

'================== Ensure this script is Run properly =======================
If (LCase(Right(Wscript.FullName,11)) <> "cscript.exe") Then
    WScript.Echo "This file must be executed using cscript.exe...please rerun."
    QuitProgram(1)     ' script was not executed using cscript
End If

If WScript.Version < CInt(5.0) Then
    WScript.Echo "This file must be executed using WSH 5.0 or greater"
    QuitProgram(1)     ' script was not executed on proper cscript version
End If

If (WScript.Arguments.Count > 4) Then
    WScript.Echo "Error in number of input parameters...please rerun."
    Usage()
    QuitProgram(1)     ' user # if inputs is incorrect
End If

'================== Execution of Main Function ===============================
' Global Variables
Dim g_SetFile

Main()
QuitProgram(0)

Sub Main
    Dim i, j, k, arg
    Dim sUser, sPW, strDomain, sDomainEN, strServer, sNetUseCMD
    Dim aDomain, aServer, aVars
    Dim bPW

    '============== Get variable inputs from user input ======================
'---NO Required variable inputs---
'---Optional variable inputs---
    'aDomain   - domain credentials to join with (SMX,REDMOND,etc...)
    'aServer   - server names/shares to connect to
    'sUser     - user credentials to connect with
    'sPW       - password to establish credentials with
    'default action is to connect to all default servers in aServer for all aDomain

    ' Defaults for flags
    strDomain = "SMX"
    sUser     = sUSERPWR
    sPW       = sPWPWR
    bPW       = False

    ' Set flags based on user inputs (overwrite defaults)
    For Each arg In WScript.Arguments
        If Instr(arg,"?") Then
            Usage()
            QuitProgram(1)     ' user wanted usage
        End If
        aVars = Split(arg,":",2)
        Select Case UCase(aVars(0))
            Case "/USER"
                sUser = aVars(1)
            Case "/PW"
                sPW   = aVars(1)
                bPW   = True
            Case Else
                If (Left(aVars(0),2) = "\\") Then
                    strServer = aVars(0)
                Else
                    strDomain = aVars(0)
                End If
        End Select
    Next
    aDomain = Split(strDomain,",",iSUBSTRINGS)
    ' Copy dependent files and folders - ignore errors
    DependFiles()

    '============== Create connections to Servers ============================
    ' Create log file
    On Error Resume Next
    Set g_SetFile = objFSO.OpenTextFile("c:\logs\set.log",iFORWRITING,bCREATEDOC,bTRISTATETRUE)
    g_SetFile.Close
    If (Err <> 0) Then
        WScript.Echo "Cannot overwrite c:\logs\set.log...please check locks on file."
        QuitProgram(3)
    End If
    On Error goto 0
    EchoAndLog "Started " & WScript.ScriptFullName

    ' Iterate and create connections for all aDomain
    For i = LBound(aDomain) To UBound(aDomain)
        ' Take first part of domain name only (in case of FQDN input)
        aVars = Split(aDomain(i),".",2)
        aDomain(i) = UCase(aVars(0))
        ' Translate names to Unicode characters as neccessary
        Select Case aDomain(i)
            Case "HONDA"     ' Japanese Localized Domain
                aDomain(i) = "****"
                sDomainEN  = "HONDA"
            Case "****"
                sDomainEN  = "HONDA"
            Case "SUPERNET"  ' German Localized Domain
                aDomain(i) = "ÜBERNET"
                sDomainEN  = "SUPERNET"
            Case "ÜBERNET"
                sDomainEN  = "SUPERNET"
            Case "FRENCH"    ' French Localized Domain
                aDomain(i) = "FRANÇAIS"
                sDomainEN  = "FRENCH"
            Case "FRANÇAIS"
                sDomainEN  = "FRENCH"
            Case Else
                sDomainEN = aDomain(i)
        End Select
        ' Assign/Create aServer list
        If (strServer <> Empty) Then
            aServer = Split(strServer,",",iSUBSTRINGS)
        Else
            SrvList aDomain(i),sDomainEN,aServer
        End If
        ' Retrieve sUser password from CDS system
        If (bPW = False) Then
            sPW = CDSPassword(aDomain(i),sDomainEN,sUser)
        End If
        EchoAndLog Space(3) & "Connecting with credentials - " & sDomainEN & "\" & sUser
        EchoAndLog Space(3) & Date & " " & Time

        ' Iterate through the list of Servers...delete/create connections
        For j = LBound(aServer) To UBound(aServer)
            k = 0
            Do While k < 2
                On Error Resume Next
                objNetwork.RemoveNetworkDrive aServer(j)
                On Error goto 0
                WScript.Sleep(500)    ' sleep for 1/2 second
                On Error Resume Next
                objNetwork.MapNetworkDrive "",aServer(j),False,aDomain(i) & "\" & sUser,sPW
                If (Err <> 0) Then
                    EchoAndLog Space(6) & "Failure - connect to " & aServer(j)
                    EchoAndLog Space(8) & Err & ": " & Err.Description
                    k = k + 1
                    WScript.Sleep(5000)    ' sleep for 5 seconds
                Else
                    EchoAndLog Space(8) & "Success - connect to " & aServer(j)
                    Exit Do
                End If
                On Error goto 0
            Loop
        Next
    Next
    ' Copy dependent files and folders...after connections
    DependFiles()
    EchoAndLog "Finished - " & Date & " " & Time
End Sub

'================== Copy files necessary to run ==============================
Sub DependFiles()
    Dim x, y, z, sCopyCMD, aDependFiles, aSafeFiles, sFile, oFile
    aSafeFiles = Array(sSAFEKEEP & "\AddSMXDNS.vbs", sSAFEKEEP & "\CDSCom.dll", _
                        sSAFEKEEP & "\setcred.cmd", sSAFEKEEP & "\Set.vbs")
    aDependFiles = Array(sTOOLSRV & "\bricktools\common" & "\AddSMXDNS.vbs", _
                         sTOOLSRV & "\bricktools\" & g_SysProcVar & "\com\CDSCom.dll", _
                         sTOOLSRV & "\oasis\setcred.cmd", _
                         sTOOLSRV & "\oasis\set.vbs" )

    ' Create the KEEP/DELETE folders...if necessary
    If Not objFSO.FolderExists(sSAFEKEEP) Then
        objFSO.CreateFolder(sSAFEKEEP)
    Else
        For Each sFile In aSafeFiles
            If objFSO.FileExists(sFile) Then
                Set oFile = objFSO.GetFile(sFile)
                If oFile.Attributes And 1 Then
                    oFile.Attributes = oFile.Attributes -1
                End If
            End If
        Next
    End If
    If Not objFSO.FolderExists(sSAFEDEL) Then
        objFSO.CreateFolder(sSAFEDEL)
    End If
    If Not objFSO.FolderExists("c:\logs") Then
        objFSO.CreateFolder("c:\logs")
    End If
    ' Copy the files needed
    On Error Resume Next
    For x = LBound(aDependFiles) To UBound(aDependFiles)
        objFSO.CopyFile aDependFiles(x),sSAFEKEEP & "\",bOVERWRITE
    Next
    ' Register the CDScom.dll - machine is now updated for all possible CDS functions
    If objFSO.FileExists(g_SysRoot & "bricktools\com\cdscom.dll") Then
        objShell.Run "regsvr32 " & g_SysRoot & "\bricktools\com\CDSCom.dll /s",iWINMIN,bWINWAIT
    Else
        objShell.Run "regsvr32 " & sSAFEKEEP & "\CDSCom.dll /s",iWINMIN,bWINWAIT
    End If
    On Error goto 0
End Sub

'================== Get User password from CDS ===============================
' Retrieve User Password from CDS System
' -  32 bit utilizes cdscom.dll; 64 bit uses a database query
Function CDSPassword(sDomain,sDomainEN,sUser)
    If (g_SysProcVar = "X86") Then
        Dim objCDS
        On Error Resume Next
        Set objCDS = WScript.CreateObject("CDSCom.Users")
        CDSPassword = objCDS.GetPassword(sDomain & "\" & sUser)
        ' Deal with any errors from CDS...set default sPassword
        If (objCDS Is Nothing) Or (CDSPassword = Empty) Or (Err <> 0) Then
            CDSPassword = sPWPWR
            EchoAndLog   Space(3) & "Could not retrieve password for " & sDomainEN & _
                         "\" & sUser & " from CDS"
            WScript.Echo Space(6) & Err & ": " & Err.Description
        End If
        On Error Goto 0
        Set objCDS = Nothing
    Else
        Dim sSQLQuery
        Dim dbConnect, dbRecordSet
        Const sConnectStr = "DRIVER={SQL Server};Network=dbmssocn;PROVIDER=sqloledb;SERVER=smdata.smx.net;DATABASE=CommonDataStore;UID=cds_ro;PWD=cds_ro"
        On Error Resume Next
        Set dbConnect   = CreateObject("ADODB.Connection")
        Set dbRecordSet = CreateObject("ADODB.Recordset")
        dbConnect.Open sConnectStr
        sSQLQuery = "exec sp_GetPassword'" & sDomain & "\" & sUser & "';"
        dbRecordSet.Open sSQLQuery, dbConnect
        If Err.Number <> 0 Then
            EchoAndLog   Space(3) & "Could not connect to cds with connection string " & sConnectStr & VbCrLf _
            & Space(3) & "Error number is " & Err.Number & vbTab & "Error message is " & Err.Description
        End If
        ' Deal non-existence of user in database...set default sPassword
        If dbRecordSet.EOF And dbRecordSet.BOF Then
            CDSPassword = sPWPWR
            EchoAndLog   Space(3) & "Could not retrieve password for " & sDomainEN & _
                         "\" & sUser & " from CDS"
            WScript.Echo Space(6) & "64001:'" & sDomain & "\" & sUser & "' record not in common data store."
        Else
            CDSPassword = dbRecordSet("PASSWORD")
        End If
        dbRecordSet.Close
        dbConnect.Close
        Set dbRecordSet.ActiveConnection = Nothing
        Set dbRecordSet                  = Nothing
        Set dbConnect                    = Nothing
    End If
End Function

'================== Create list of default Servers per Domain ================
Sub SrvList(sDomain,sDomainEN,ByRef aServer)
    If (sDomain = "SMX") Then
        '---Default SMX Domain Servers---
        aServer = Array("\\smx.net\tools\"    , _
                        "\\smx.net\products\" , _
                        "\\smx.net\builds\"   , _
                        "\\smx.net\drop\"     , _
                        "\\smx.net\archive\"  , _
                        "\\smx.net\symbols\"  , _
                        "\\smx.net\os"        , _
                        "\\smxtools.smx.net\ipc$")

    Elseif (sDomain = "REDMOND") Then
        '---Default Redmond Domain Servers---
        aServer = Array("\\smxfiles.redmond.corp.microsoft.com\ipc$"    , _
                        "\\smxfiles\ipc$"                               , _
                        "\\archon\ipc$"                                 , _
                        "\\archon.redmond.corp.microsoft.com\ipc$"      , _
                        "\\emdarchon.redmond.corp.microsoft.com\ipc$"   , _
                        "\\emdarchon\ipc$")
    Else
        EchoAndLog sDomainEN & " has no default server list to connect to" & _
                   "...please choose SMX or REDMOND."
        QuitProgram(1)     ' invalid sDomain input for using default strServer list
    End If
End Sub

'================== Show Usage ===============================================
Sub Usage()
    WScript.Echo
    WScript.Echo
    WScript.Echo "cscript //nologo set.vbs [domain] [servers] [/user:] [/pw:]"
    WScript.Echo "Establish credentials connections to servers."
    WScript.Echo "--- Log at C:\logs\Set.log ---"
    WScript.Echo
    WScript.Echo "    domain   = list of user domains to try to establish the connections with (delimited by commas)"
    WScript.Echo "               default: SMX"
    WScript.Echo "    servers  = list of servers to connect to (delimited by commas)" & Chr(34) & "S1,S2,S3" & Chr(34)
    WScript.Echo "               default: SMX core servers"
    WScript.Echo "    /user:   = user name to establish connections with"
    WScript.Echo "               default: asttest"
    WScript.Echo "    /pw:     = user password to establish connections with"
    WScript.Echo "               default: retrieved from CDS based on user name"
    WScript.Echo
    WScript.Echo "Examples:"
    WScript.Echo "    set.vbs"
    WScript.Echo "       Establish credentials to all default SMX servers."
    WScript.Echo
    WScript.Echo "    set.vbs smx,redmond"
    WScript.Echo "       Establish credentials to all default SMX and REDMOND servers."
    WScript.Echo
    WScript.Echo "    set.vbs redmond \\Winbuilds\ipc$,\\Jennla\public"
    WScript.Echo "       Establish credentials to \\Winbuilds\ipc$ and \\Jennla\public"
    WScript.Echo "       using REDMOND\asttest credentials."
    WScript.Echo
    WScript.Echo "    set.vbs segroup,redmond /user:astdits /pw:Goo#22 \\Jennla\public"
    WScript.Echo "       Try to establish credentials to \\Jennla\public using the credentials"
    WScript.Echo "       SEGROUP\astdits and REDMOND\astdits with the password Goo#22."
    WScript.Echo
    WScript.Echo
End Sub

'================== Append info to OASIS file ================================
' Append information to the log file at sSAFEKEEP\OASIS.log
Sub EchoAndLog(sWriteOut)
    Set g_SetFile = objFSO.OpenTextFile("c:\logs\set.log",iFORAPPENDING,bNOCREATEDOC,bTRISTATETRUE)
    WScript.Echo sWriteOut
    g_SetFile.WriteLine sWriteOut
    g_SetFile.Close
End Sub

'================== Quit Program and Close Down ==============================
' Quit script, close objects, and return iError as ErrorLevel
Sub QuitProgram(iError)
    Set objFSO     = Nothing
    Set objShell   = Nothing
    Set objNetwork = Nothing

    WScript.Quit(iError)
End Sub
