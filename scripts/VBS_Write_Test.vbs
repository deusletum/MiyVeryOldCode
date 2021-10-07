Option Explicit

Sub WriteLog(sMessage, sLog, iExitCode)
    On Error Resume Next
    Const ForAppending = 8

    Dim sLogFile, oFSO, oStream

    sLogFile = sLog
    Set oFSO = CreateObject("Scripting.FileSystemObject")
    Set oStream = oFSO.OpenTextFile(sLogFile, ForAppending, True)
    If Err.Number <> 0 Then
        oStream.Close
        WScript.Echo("Inst_SQL was unable to write a log")
        WScript.Echo(Err.Description)
        Exit Sub
    End If

    oStream.Write(vbCrlf & "*LOG_START*-Inst_SQL")
    oStream.Write(vbCrlf & "Inst_SQL.vbs command line parameter(s) are:")
    oStream.Write(vbCrlf & vbTab & g_sCommandLineParms)
    If iExitCode <> 0 Then
        oStream.Write(vbCrlf & "[" &  FormatDateTime(Date, 2) & " " & _
            FormatDateTime(Time, 4)  & "] ERR Inst_SQL " & sMessage)
    Else
        oStream.Write(vbCrlf & "[" &  FormatDateTime(Date, 2) & " " & _
            FormatDateTime(Time, 4)  & "] Inst_SQL " & sMessage)
    End If
    oStream.Write(vbCrlf & "*LOG_DONE*")
    oStream.Close
    WScript.Echo(sMessage)
    WScript.Quit(iExitCode)

    Set oStream = Nothing
    Set oFSO = Nothing
End Sub

WriteLog "This is a test", "c:\logs\VBS_Write_Test.log" , 1500
