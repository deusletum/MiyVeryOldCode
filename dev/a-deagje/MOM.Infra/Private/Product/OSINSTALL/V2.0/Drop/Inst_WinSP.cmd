@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=

REM ======================================================================
REM Copyright . 2003
REM
REM Module : Inst_WinSP.cmd
REM
REM Summary: Installs Windows Service Packs
REM
REM History: (7/2/2003) John Heaton - Initial coding (based on Winspinst.cmd)
REM ======================================================================
REM Error Codes:
REM             1 = Invalid user input or Usage
REM             2 = Undefined (Not Used)
REM             3 = Access Denied
REM             4 = Path Not Found
REM             5 = Install Failure
REM             6 = Reboot required
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" equ "" (goto :Usage)
if "%1" equ "/?" (goto :Usage)
if "%1" equ "?" (goto :Usage)
if "%1" equ "-?" (goto :Usage)
if "%2" equ "" (goto :Usage)

REM ======================================================================
REM Set some enviroment variables
REM ======================================================================
set OSTYPE=%1
set SPVER=%2
set FILESRV=SMX.NET\OS
if not defined LOC (set LOC=EN)
if /i "%3" equ "/NOREBOOT" (set NOREBOOT=TRUE)
set INSTLOC=\\%FILESRV%\%OSTYPE%\%LOC%\SVCPACK\%SPVER%\I386\UPDATE
set INSTFLAGS=-u -o -z

REM ======================================================================
REM Installs Windows 2000 Service Packs
REM ======================================================================
if /i "%OSTYPE%" neq "W2K" (goto :W2K3)
start /wait %INSTLOC%\UPDATE.EXE %INSTFLAGS%
set ERRNUM=%ERRORLEVEL%
call :CHK_ERR
if not defined NOREBOOT (sshutdown.exe -r -f -t 3)
goto :End

REM ======================================================================
REM Installs Windows 2003 Service Packs
REM ======================================================================
:W2K3
if /i "%OSTYPE%" neq "W2K3" (goto :XP)
echo W2K3
\\%FILESRV%\%OSTYPE%\%LOC%\SVCPACK\%SPVER%\I386\UPDATE\UPDATE.EXE -u -o -z
if not defined NOREBOOT (sshutdown.exe -r -f -t 3)
goto :End

REM ======================================================================
REM Installs Windows XP Service Packs
REM ======================================================================
:XP
if /i "%OSTYPE%" neq "XP" (goto :Usage)
echo XP
\\%FILESRV%\%OSTYPE%\%LOC%\SVCPACK\%SPVER%\I386\UPDATE\UPDATE.EXE -u -o -z
if not defined NOREBOOT (sshutdown.exe -r -f -t 3)
goto :End

REM ======================================================================
REM Checks Error Message
REM ======================================================================
:CHK_ERR
if not exist {%INSTLOC%} (
    set ERRMSG=Path not found %INSTLOC%
    set ERRNUM=4
goto :End
)
if "%ERRNUM%" equ "5" (
    set ERRMSG=Access Denied
    set ERRNUM=3
    goto :End
)



REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo inst_winsp.cmd OSTYPE SRVPACK
echo.
echo Example: inst_winsp.cmd W2K SP4
echo.
echo Valid OS Types:
echo    "W2K, W2K3, XP"
echo.
echo Valid Service Pack Version:
echo    "W2K  = SP4"
echo    "W2K3 = SP1"
echo    "XP   = SP1"
echo.
echo Optional Parameter:
echo    "/NOREBOOT"
echo.
set ERRNUM=1
set ERRMSG=Invalid Parameter(s) entered

:End
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%