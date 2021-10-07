'********************************************************************
'*
'* File:           Exec.Vbs
'* Created:        March 1999
'* Version:        1.0
'*
'*  Main Function:  Executes a command.
'*
'*  Exec.vbs  [/e <Command> | /x <ProcessID>] [/S <server>] [/U <username>] [/W <password>] 
'*            [/O <outputfile>]
'*
'* Copyright (C) 1999 oration
'*
'********************************************************************

OPTION EXPLICIT

    'Define constants
    CONST CONST_ERROR                   = 0
    CONST CONST_WSCRIPT                 = 1
    CONST CONST_CSCRIPT                 = 2
    CONST CONST_SHOW_USAGE              = 3
    CONST CONST_EXECUTE                 = 4


    'Declare variables
    Dim intOpMode, i, intProcessID
    Dim strServer, strUserName, strPassword, strOutputFile, strCommand
    Dim res

    'Make sure the host is csript, if not then abort
    VerifyHostIsCscript()

    'Parse the command line
    intOpMode = intParseCmdLine(strServer     ,  _
                                strUserName   ,  _
                                strPassword   ,  _
                                strOutputFile ,  _
                                strCommand    ,  _
                                intProcessID     )


    Select Case intOpMode

        Case CONST_SHOW_USAGE
            Call ShowUsage()

        Case CONST_EXECUTE
            res = RemoteExecute(strCommand, _
                                strServer    , _
                                strUserName   , _
                                strPassword)
            WScript.Echo "result=" & CStr(res)
            Call WScript.Quit(res)

        Case CONST_ERROR
            'Do Nothing

        Case Else                    'Default -- should never happen
            Call Wscript.Echo("Error occurred in passing parameters.")

    End Select
    Call WScript.Quit (1)


'********************************************************************
'*
'* Sub ExecuteCmd()
'*
'* Purpose: List the desktop properties on a system.
'*
'* Input:   strServer           a machine name
'*          strOutputFile       an output file name
'*          strUserName         the current user's name
'*          strPassword         the current user's password
'*          strCommand          a valid WBEM command
'*
'* Output:  Results are either printed on screen or saved in strOutputFile.
'*
'********************************************************************
Function RemoteExecute(DCPA_Command, _
                       DCPA_Remote_Host, _
                       DCPA_Username, _
                       DCPA_Password)

    Dim DCPA_Locator, DCPA_Namespace, DCPA_Instance
    Dim DCPA_Result, DCPA_Process_ID
    Dim er

    On Error Resume Next

    ' Create Locator object to connect to remote CIM object manager
    Set DCPA_Locator = CreateObject("WbemScripting.SWbemLocator")
    if Err.Number then
        on error goto 0
        RemoteExecute = -1
        exit function
    end if

    ' Connect to the namespace which is either local or remote
    Set DCPA_Namespace = DCPA_Locator.ConnectServer(DCPA_Remote_Host, "root/cimv2", DCPA_Username, DCPA_Password)
    if Err.Number then
        WScript.Echo "DCPA_Locator.ConnectServer(""" & DCPA_Remote_Host & """, ""root/cimv2"", """ &  _
                        DCPA_Username& """, """ & DCPA_Password & """): " & _
                        "0x" & Hex(Err.Number) & " : " & Err.Description
        RemoteExecute = Err.Number
        on error goto 0
        exit function
    end if

    DCPA_Namespace.Security_.impersonationlevel = 3
    if Err.Number then
        WScript.Echo  "impersonationlevel = 3: 0x" & Hex(Err.Number) & " : " & Err.Description
        RemoteExecute = Err.Number
        on error goto 0
        exit function
    end if

    ' Do something
    Set DCPA_Instance = DCPA_Namespace.Get("Win32_Process")
    if Err.Number or (DCPA_Instance Is Nothing) then
        WScript.Echo  "DCPA_Namespace.Get(""Win32_Process""): 0x" & Hex(Err.Number) & " : " & Err.Description
        RemoteExecute = Err.Number
        on error goto 0
        exit function
    end if

    ' Create the process
    DCPA_Result = DCPA_Instance.Create(DCPA_Command, Null, Null, DCPA_Process_ID)
    if DCPA_Result <> 0 then
        on error goto 0
        RemoteExecute = DCPA_Result
        exit function
    end if

    ' Fix the Process ID
    If DCPA_Process_ID < 0 Then DCPA_Process_ID = DCPA_Process_ID + 4294967296

    on error goto 0

    ' Return true.
    RemoteExecute = 0
end function


'********************************************************************
'*
'* Function intParseCmdLine()
'*
'* Purpose: Parses the command line.
'* Input:   
'*
'* Output:  strServer         a remote server ("" = local server")
'*          strUserName       the current user's name
'*          strPassword       the current user's password
'*          strOutputFile     an output file name
'*
'********************************************************************
Private Function intParseCmdLine( ByRef strServer,        _
                                  ByRef strUserName,      _
                                  ByRef strPassword,      _
                                  ByRef strOutputFile,    _
                                  ByRef strCommand,       _
                                  ByRef intProcessID      )

    ON ERROR RESUME NEXT

    Dim strFlag
    Dim intState, intArgIter
    Dim objFileSystem

    If Wscript.Arguments.Count > 0 Then
        strFlag = Wscript.arguments.Item(0)
    End If

    'Check if the user is asking for help or is just confused
    If (strFlag="help") OR (strFlag="/h") OR (strFlag="\h") OR (strFlag="-h") _
        OR (strFlag = "\?") OR (strFlag = "/?") OR (strFlag = "?") _ 
        OR (strFlag="h") Then
        intParseCmdLine = CONST_SHOW_USAGE
        Exit Function
    End If

    'Retrieve the command line and set appropriate variables
     intArgIter = 0
    Do While intArgIter <= Wscript.arguments.Count - 1
        Select Case Left(LCase(Wscript.arguments.Item(intArgIter)),2)
  
            Case "/s"
                If Not blnGetArg("Server", strServer, intArgIter) Then
                    intParseCmdLine = CONST_ERROR
                    Exit Function
                End If
                intArgIter = intArgIter + 1

            Case "/o"
                If Not blnGetArg("Output File", strOutputFile, intArgIter) Then
                    intParseCmdLine = CONST_ERROR
                    Exit Function
                End If
                intArgIter = intArgIter + 1

            Case "/u"
                If Not blnGetArg("User Name", strUserName, intArgIter) Then
                    intParseCmdLine = CONST_ERROR
                    Exit Function
                End If
                intArgIter = intArgIter + 1

            Case "/w"
                If Not blnGetArg("User Password", strPassword, intArgIter) Then
                    intParseCmdLine = CONST_ERROR
                    Exit Function
                End If
                intArgIter = intArgIter + 1

            Case "/e"
                intPArseCMdLine = CONST_EXECUTE
                If Not blnGetArg("Command", strCommand, intArgIter) Then
                    intParseCmdLine = CONST_ERROR
                    Exit Function
                End If
                intArgIter = intArgIter + 1

            Case Else 'We shouldn't get here
                Call Wscript.Echo("Invalid or misplaced parameter: " _
                   & Wscript.arguments.Item(intArgIter) & vbCRLF _
                   & "Please check the input and try again," & vbCRLF _
                   & "or invoke with '/?' for help with the syntax.")
                Wscript.Quit

        End Select

    Loop '** intArgIter <= Wscript.arguments.Count - 1


    If IsEmpty(intParseCmdLine) Then _
        intParseCmdLine = CONST_ERROR

End Function


'********************************************************************
'*
'* Sub ShowUsage()
'*
'* Purpose: Shows the correct usage to the user.
'*
'* Input:   None
'*
'* Output:  Help messages are displayed on screen.
'*
'********************************************************************
Private Sub ShowUsage()

    Wscript.Echo ""
    Wscript.Echo "Executes a command."
    Wscript.Echo ""
    Wscript.Echo "SYNTAX:"
    Wscript.Echo "  Exec.vbs [/e <Command> "
    Wscript.Echo "           [/S <server>] [/U <username>]" _
                &" [/W <password>]"
    Wscript.Echo "  [/O <outputfile>]"
    Wscript.Echo ""
    Wscript.Echo "PARAMETER SPECIFIERS:"
    Wscript.Echo "   command       A valid command. " _
               & "(The entire command string must"
    Wscript.Echo "                 be enclosed in quotes if " _
               & "it contains empty spaces.)"
    Wscript.Echo "   ProcessID     The Number of a currently running Process."
    Wscript.Echo "   server        A machine name."
    Wscript.Echo "   username      The current user's name."
    Wscript.Echo "   password      Password of the current user."
    Wscript.Echo "   outputfile    The output file name."
    Wscript.Echo ""
    Wscript.Echo "EXAMPLE:"
    Wscript.Echo "1.  cscript Exec.vbs /e notepad /s MyMachine2"
    Wscript.Echo "    Starts notepad on MyMachine2."
    Wscript.Echo ""
    Wscript.Echo "Note:"
    Wscript.Echo "    Use ps.vbs to obtain process ID numbers"
    Wscript.Echo ""

End Sub

'********************************************************************
'* General Routines
'********************************************************************

'********************************************************************
'* 
'*  Function blnGetArg()
'*
'*  Purpose: Helper to intParseCmdLine()
'* 
'*  Usage:
'*
'*     Case "/s" 
'*       blnGetArg ("server name", strServer, intArgIter)
'*
'********************************************************************
Private Function blnGetArg ( ByVal StrVarName,   _
                             ByRef strVar,       _
                             ByRef intArgIter) 

    blnGetArg = False 'failure, changed to True upon successful completion

    If Len(Wscript.Arguments(intArgIter)) > 2 then
        If Mid(Wscript.Arguments(intArgIter),3,1) = ":" then
            If Len(Wscript.Arguments(intArgIter)) > 3 then
                strVar = Right(Wscript.Arguments(intArgIter), _
                         Len(Wscript.Arguments(intArgIter)) - 3)
                blnGetArg = True
                Exit Function
            Else
                intArgIter = intArgIter + 1
                If intArgIter > (Wscript.Arguments.Count - 1) Then
                    Call Wscript.Echo( "1Invalid " & StrVarName & ".")
                    Call Wscript.Echo( "Please check the input and try again.")
                    Exit Function
                End If

                strVar = Wscript.Arguments.Item(intArgIter)
                If Err.Number Then
                    Call Wscript.Echo( "2Invalid " & StrVarName & ".")
                    Call Wscript.Echo( "Please check the input and try again.")
                    Exit Function
                End If

                blnGetArg = True 'success
            End If
        Else
            strVar = Right(Wscript.Arguments(intArgIter), _
                     Len(Wscript.Arguments(intArgIter)) - 2)
            blnGetArg = True 'success
            Exit Function
        End If
    Else
        intArgIter = intArgIter + 1
        If intArgIter > (Wscript.Arguments.Count - 1) Then
            Call Wscript.Echo( "3Invalid " & StrVarName & ".")
            Call Wscript.Echo( "Please check the input and try again.")
            Exit Function
        End If

        strVar = Wscript.Arguments.Item(intArgIter)
        If Err.Number Then
            Call Wscript.Echo( "4Invalid " & StrVarName & ".")
            Call Wscript.Echo( "Please check the input and try again.")
            Exit Function
        End If

        blnGetArg = True 'success
    End If
End Function

'********************************************************************
'*
'* Sub      VerifyHostIsCscript()
'*
'* Purpose: Determines which program is used to run this script.
'*
'* Input:   None
'*
'* Output:  If host is not cscript, then an error message is printed 
'*          and the script is aborted.
'*
'********************************************************************
Sub VerifyHostIsCscript()

    ON ERROR RESUME NEXT

    Dim strFullName, strCommand, i, j, intStatus

    strFullName = WScript.FullName

    If Err.Number then
        Call Wscript.Echo( "Error 0x" & CStr(Hex(Err.Number)) & " occurred." )
        If Err.Description <> "" Then
            Call Wscript.Echo( "Error description: " & Err.Description & "." )
        End If
        intStatus =  CONST_ERROR
    End If

    i = InStr(1, strFullName, ".exe", 1)
    If i = 0 Then
        intStatus =  CONST_ERROR
    Else
        j = InStrRev(strFullName, "\", i, 1)
        If j = 0 Then
            intStatus =  CONST_ERROR
        Else
            strCommand = Mid(strFullName, j+1, i-j-1)
            Select Case LCase(strCommand)
                Case "cscript"
                    intStatus = CONST_CSCRIPT
                Case "wscript"
                    intStatus = CONST_WSCRIPT
                Case Else       'should never happen
                    Call Wscript.Echo( "An unexpected program was used to " _
                                       & "run this script." )
                    Call Wscript.Echo( "Only CScript.Exe or WScript.Exe can " _
                                       & "be used to run this script." )
                    intStatus = CONST_ERROR
                End Select
        End If
    End If

    If intStatus <> CONST_CSCRIPT Then
        Call WScript.Echo( "Please run this script using CScript." & vbCRLF & _
             "This can be achieved by" & vbCRLF & _
             "1. Using ""CScript Exec.vbs arguments"" for Windows 95/98 or" _
             & vbCRLF & "2. Changing the default Windows Scripting Host " _
             & "setting to CScript" & vbCRLF & "    using ""CScript " _
             & "//H:CScript //S"" and running the script using" & vbCRLF & _
             "    ""Exec.vbs arguments"" for Windows NT/2000." )
        WScript.Quit
    End If

End Sub

'********************************************************************
'* 
'* Function blnErrorOccurred()
'*
'* Purpose: Reports error with a string saying what the error occurred in.
'*
'* Input:   strIn       string saying what the error occurred in.
'*
'* Output:  displayed on screen 
'* 
'********************************************************************
Private Function blnErrorOccurred (ByVal strIn)

    If Err.Number Then
        Call Wscript.Echo( "Error 0x" & CStr(Hex(Err.Number)) & ": " & strIn)
        If Err.Description <> "" Then
            Call Wscript.Echo( "Error description: " & Err.Description)
        End If
        Err.Clear
        blnErrorOccurred = True
    Else
        blnErrorOccurred = False
    End If

End Function

'********************************************************************
'* 
'* Function blnOpenFile
'*
'* Purpose: Opens a file.
'*
'* Input:   strFileName     A string with the name of the file.
'*
'* Output:  Sets objOpenFile to a FileSystemObject and setis it to 
'*            Nothing upon Failure.
'* 
'********************************************************************
Private Function blnOpenFile(ByVal strFileName, ByRef objOpenFile)

    ON ERROR RESUME NEXT

    Dim objFileSystem

    Set objFileSystem = Nothing

    If IsEmpty(strFileName) OR strFileName = "" Then
        blnOpenFile = False
        Set objOpenFile = Nothing
        Exit Function
    End If

    'Create a file object
    Set objFileSystem = CreateObject("Scripting.FileSystemObject")
    If blnErrorOccurred("Could not create filesystem object.") Then
        blnOpenFile = False
        Set objOpenFile = Nothing
        Exit Function
    End If

    'Open the file for output
    Set objOpenFile = objFileSystem.OpenTextFile(strFileName, 8, True)
    If blnErrorOccurred("Could not open") Then
        blnOpenFile = False
        Set objOpenFile = Nothing
        Exit Function
    End If
    blnOpenFile = True

End Function

'********************************************************************
'*                                                                  *
'*                           End of File                            *
'*                                                                  *
'********************************************************************