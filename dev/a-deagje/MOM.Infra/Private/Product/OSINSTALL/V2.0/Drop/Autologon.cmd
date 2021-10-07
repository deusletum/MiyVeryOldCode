@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=
md %SYSTEMDRIVE%\LOGS >NUL 2>&1
set AUTOLOGON.LogFile=%SYSTEMDRIVE%\LOGS\autologon.log

REM ======================================================================
REM Copyright . 2004
REM
REM Module    : Autologon.cmd
REM
REM Summary   : sets up auto logon for x86 and ia64
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM             2 - Autologon Error
REM
REM History   : (1/26/2004) Dean Gjedde - Initial coding
REM             (2/25/2004) John Heaton - Updated to not give false positive
REM             (2/28/2004) Todd Ellis  - Fixing mispelling on %SYSTEMDRIVE% so it will actually write to LOGFILE.
REM             (2/28/2004) Todd Ellis  - Added line 38 and removed else statement near line 58.
REM                                     - (was not working on 64 bit)
REM             (3/01/2004) John Heaton - Updating to have a failchk and got rid of all the if statements
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%3" equ "" (goto :Usage)
if "%1" equ "/?" (goto :Usage)
if "%1" equ "?" (goto :Usage)
if "%1" equ "-?" (goto :Usage)
if not defined TOOLSRV (set TOOLSRV=\\smx.net\tools)
set AUTOLOGON.Path=%TOOLSRV%\bricktools\%PROCESSOR_ARCHITECTURE%\autologon.exe
set PASSWORD=%1
set USER=%2
set DOMAINNAME=%3
if not exist %AUTOLOGON.Path% (
    set ERRNUM=2
    set ERRMSG=Unable to locate %AUTOLOGON.Path%
    goto :End
)
REM ======================================================================
REM Chose the the correct auto logon process
REM ======================================================================
if /i "%PROCESSOR_ARCHITECTURE%" equ "X86" (
    goto :Standard
) else (
    goto :64Bit
)

:Standard
REM ======================================================================
REM autologon for X86
REM ======================================================================
call %AUTOLOGON.Path% -xr
call %AUTOLOGON.Path% \\%COMPUTERNAME% -s %PASSWORD% %USER% %DOMAINNAME%
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=2
    set ERRMSG=Autologon Error on First run
)
if defined ERRMSG (
    echo %ERRMSG% - Err - %ERRNUM% >%AUTOLOGON.LogFile%
)
call %AUTOLOGON.Path% \\%COMPUTERNAME% -s %PASSWORD% %USER% %DOMAINNAME%
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=2
    set ERRMSG=Autologon Error on Second run
    goto :End
)
goto :FailChk

:64Bit
REM ======================================================================
REM autologon for AMD and IA64
REM ======================================================================
call cscript.exe //nologo %TOOLSRV%\drop\autologit.vbs %PASSWORD% %USER% %DOMAINNAME%
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=2
    set ERRMSG=Autologon Error on First run
    goto :End
)
goto :FailChk

:FailChk
REM ======================================================================
REM Checks for failures
REM ======================================================================
if "%ERRORLEVEL%" neq "0" (
    set ERRMSG=%ERRORLEVEL% - Unable to write autologon settings using %PASSWORD% %USER% %DOMAINNAME%
    set ERRNUM=2
    goto :End
) else (
    set ERRMSG=%ERRORLEVEL% - Autologon set
    goto :End
)

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo Autologon.cmd PASSWORD USERNAME DOMAINNAME
echo.
echo Example: Autologon.cmd MyPass ASTTEST SMX
echo.
echo Required Parameters:
echo    "PASSWORD, USERNAME, DOMAINNAME"
echo.
set ERRNUM=1
set ERRMSG=Displaying Usage

:End
popd
echo %ERRMSG% - Err - %ERRNUM% >>%AUTOLOGON.LogFile%
echo %ERRMSG% - Err - %ERRNUM%
exit /b %ERRNUM%