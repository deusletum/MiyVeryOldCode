' ============================================================================
' Copyright . 2003
'
' Module : DelAccount.vbs
'
' Summary: Designed to delete a computer account from a domain
' Dependencies: run under user w/ permission to remove DNS entry
'          ErrorCodes:
'            0 - Nothing is wrong
'            1 - User input is wrong
'            2 - a file/folder could not be located
'            3 - some action could not be executed
'                ...could not remove account from domain
'
' History: (02/04/2003) Gavme - Initial coding
'          (03/20/2003) Gavme - Modified lines 46 to use "on error resume next"
'                               Added Usage and modified lines 29 and from 63 to 71
'          (08/20/2003) Jennla- revamped to support OASIS 2.0 and new domains
' ============================================================================
Option Explicit

' Method Argument Inputs
Const iSUBSTRINGS = -1

'================== Create Common Objects ====================================
Dim objShell
Set objShell = CreateObject("WScript.Shell")

'================== Ensure this script is Run properly =======================
If (LCase(Right(Wscript.FullName,11)) <> "cscript.exe") Then
    WScript.Echo "This file must be executed using cscript.exe..."
    WScript.Quit(1)     ' script was not executed using cscript
End If

If WScript.Version < CInt(5.0) Then
    WScript.Echo "This file must be executed using WSH 5.0 or greater" & _
                 "...please run Update_WSH.cmd"
    WScript.Quit(1)     ' script was not executed on proper cscript version
End If

If (WScript.Arguments.Count <> 2) Then
    WScript.Echo "Error in number of input parameters..."
    Usage()
    WScript.Quit(1)     ' user # if inputs is incorrect
End If

Dim arg
For Each arg In WScript.Arguments
    If InStr(arg,"?") Then
        Usage()
        WScript.Quit(1)     ' user wants usage
    End If
Next

'================== Execution of Main Function ===============================
Dim iMainReturn
iMainReturn = Main()

Set objShell = Nothing
WScript.Quit(iMainReturn)

Function Main
    Main = 0
    Dim i, j, k
    Dim objCompAccount
    Dim sCompName, sDomainFQDN ,sRunCMD
    Dim aDomain

    sCompName   = UCase(WScript.Arguments(0))
    sDomainFQDN = UCase(WScript.Arguments(1))
    If InStr(sDomainFQDN,".") Then
        aDomain = Split(sDomainFQDN,".",iSUBSTRINGS)
        ' Translate domain to Unicode characters if neccessary
        Select Case aDomain(0)
            Case "HONDA"     ' Japanese Localized Domain
                aDomain(0) = "****"
            Case "SUPERNET"  ' German Localized Domain
                aDomain(0) = "ÜBERNET"
            Case "FRENCH"    ' French Localized Domain
                aDomain(0) = "FRANÇAIS"
        End Select
        ' Build string to delete Machine entry
        sRunCMD = "LDAP://CN=" & sCompName & ",OU=TestOS"
        For i = LBound(aDomain) To UBound(aDomain)
            sRunCMD = sRunCMD & ",DC=" & aDomain(i)
        Next
    Else
        WScript.Echo "You must input a FQDN for the domain input..." & sDomainFQDN
        Main = 1
        Exit Function
    End If

    ' Delete Machine entry from DNS
    On Error Resume Next
    Set objCompAccount = GetObject(sRunCMD)
    If (Err <> 0) Then
        WScript.Echo "Could not remove " & UCase(sCompName) & " from the " & sDomainFQDN & " domain."
        WScript.Echo Space(3) & Err & " : " & Err.Description
        Main = 3
        Exit Function
    End If
    objCompAccount.DeleteObject(0)
    If (Err <> 0) Then
        WScript.Echo "Could not remove " & UCase(sCompName) & " from the " & sDomainFQDN & " domain."
        WScript.Echo Space(3) & Err & " : " & Err.Description
        Main = 3
        Exit Function
    End If

    WScript.Echo "Removed " & UCase(sCompName) & " from the " & sDomainFQDN & " domain."
    On Error goto 0
End Function

'================== Show Usage ===============================================
Sub Usage()
    WScript.Echo
    WScript.Echo
    WScript.Echo "cscript //nologo delaccount.vbs name domain"
    WScript.Echo "Deletes machinename account from DNS entry."
    WScript.Echo
    WScript.Echo "Inputs:"
    WScript.Echo "   name   - computer name account to delete"
    WScript.Echo "            (currently must follow this team's naming standards)"
    WScript.Echo "   domain - FQDN domain to delete computer account from"
    WScript.Echo
    WScript.Echo "Examples:"
    WScript.Echo "   delaccount.vbs Jennlav01E SMX.NET"
    WScript.Echo "   Delete computer account for Jennlav01E in SMX.NET domain."
    WScript.Echo
    WScript.Echo
End Sub