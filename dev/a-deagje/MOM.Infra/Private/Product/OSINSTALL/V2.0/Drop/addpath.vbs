'=============================================================================
'
' NAME: AddPath.vbs
'
' AUTHOR: Glenn LaVigne- glennlav
' DATE  : 06/25/2002   - glennlav created
' COMMENT: Designed to add to or overwrite the PATH system variable
'          (either add the beginning or end of the PATH string)
'
' Returns the following ExitCodes
'     0 - Nothing is wrong
'     1 - User input is wrong
'     3 - some action could NOT be executed
'
' Modified  : 03/31/2003  - Jennla: updated to meet w/ current coding standards
'                                   added check to not add location that is already in %PATH%
'
'=============================================================================
Option Explicit


'================== Ensure this script is Run properly =======================
If (LCase(Right(Wscript.FullName,11)) = "wscript.exe") Then
    WScript.Echo "This file must be executed using cscript.exe...please rerun."
    WScript.Quit(1)      ' script was not executed using cscript
End If

If WScript.Arguments.Count <> 2 Then
    WScript.Echo "Error in number of set input parameters..."
    Usage()
    WScript.Quit(1)      ' user input incorrect # of parameters
End If

Dim arg
For Each arg In WScript.Arguments
    If (arg = "/?") Or (arg = "?") Then
        Usage()
        WScript.Quit(1)     ' user wants usage
    End If
Next


'================== Create Objects and system Environmental Variables ========
' Create Objects
Dim objShell
Set objShell = CreateObject("WScript.Shell")


'================== Execution of Main Function ===============================
Dim MainReturn
MainReturn = Main()

Set objShell = Nothing

WScript.Quit(MainReturn)



Function Main
    Main = 0
    Const Env_Var = "PATH"

    '============== Get variable inputs from user input ======================
    Dim sAction,sPath
    '---Required variable inputs---
    'sAction   - where/how to modify the PATH variable
    'sPath     - location that you want added/overwritten to the PATH variable
    sAction = UCase(WScript.Arguments(0))
    sPath = WScript.Arguments(1)

    ' Create variables to hold the PATH data
    Dim objSysEnv
    Set objSysEnv = objShell.Environment("SYSTEM")

    ' Overwrite current PATH with sPath
    If sAction = "OVERWRITE" Then
        objSysEnv(Env_Var) = sPath
        WScript.Echo "Set " & Env_Var & " to a value of " & sPath
        Main = 0     ' successfully overwrote sPath to PATH
        Exit Function
    End If

    ' Check that sPath is not already added to PATH (apply to PRE and POST sAction only)
    If InStr(objShell.ExpandEnvironmentStrings(objSysEnv(Env_Var)),sPath & ";") Or _
       InStr(objShell.ExpandEnvironmentStrings(objSysEnv(Env_Var)),sPath & "\;") Then
        WScript.Echo sPath & " is already added to " & Env_Var
        Main = 3     ' did NOT add sPath because it is already in PATH
        Exit Function
    End If

    ' Add sPath to end of PATH
    If sAction = "POST" Then
        objSysEnv(Env_Var) = objShell.ExpandEnvironmentStrings(objSysEnv(Env_Var)) & ";" & sPath
        WScript.Echo "Added " & sPath & " to end of " & Env_Var
        Main = 0     ' sPath successfully added to end of PATH
        Exit Function
    End If

    ' Add sPath to start of PATH
    If sAction = "PRE" Then
        objSysEnv(Env_Var) = sPath & ";" & objShell.ExpandEnvironmentStrings(objSysEnv(Env_Var))
        WScript.Echo "Added " & sPath & " to start of " & Env_Var
        Main = 0     ' sPath successfully added to start of PATH
        Exit Function
    End If

End Function


'================== Show Usage ===============================================
Sub Usage()
    WScript.Echo
    WScript.Echo
    WScript.Echo "cscript //nologo addpath.vbs [action] [PATH]"
    WScript.Echo " Note - these effects are on a SYSTEM-Wide level and will not apply to the"
    WScript.Echo "        current command window/process"
    WScript.Echo
    WScript.Echo "    ACTION   = the location or modification action you wish to perform"
    WScript.Echo "               on the PATH environmental variable"
    WScript.Echo "               PRE       : add to start of %PATH%"
    WScript.Echo "               POST      : add to end of %PATH%"
    WScript.Echo "               OVERWRITE : replace current %PATH% with second parameter input"
    WScript.Echo "    PATH     = the location that you want to add/replace to %PATH%"
    WScript.Echo
    WScript.Echo
    WScript.Echo "Examples:"
    WScript.Echo "    cscript //nologo addpath.vbs pre e:\winnt\bricktools\"
    WScript.Echo "    PATH = e:\winnt\bricktools\;...;e:\winnt\"
    WScript.Echo
    WScript.Echo "    cscript //nologo addpath.vbs overwrite " & Chr(34) & "e:\winnt\idw\;" & _
                      "e:\winnt\program files;e:\winnt\bricktools" & Chr(34)
    WScript.Echo "    PATH = e:\winnt\idw;e:\winnt\program files;e:\winnt\bricktools"
    WScript.Echo
    WScript.Echo
End Sub