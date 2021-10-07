@echo OFF
if defined ECHO (echo %ECHO%)
pushd.

set ERRNUM=0
set ERRMSG=
set SRCLOC=
set BUILDNUM=
REM ======================================================================
REM Copyright . 2004
REM
REM Module : update_wsh.cmd
REM
REM Summary: Updates the Windows Script Host to the version on the tool share
REM             Current VBS Version 5.6
REM
REM History: (01/11/02) Glenn LaVigne - Initial coding
REM          (10/15/03) Jennla - modify for smx.net domain
REM          (02/17/04) Jheat  - Updated sleep and removed OS VER it did
REM ======================================================================
REM Error Codes:
REM             1 = Invalid user input or Usage
REM             2 = Not Used
REM             3 = Not Used
REM             4 = Not Used
REM             5 = Not Used
REM             6 = Install Failure
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" neq "" (goto :Usage)

if not defined TOOLSRV (set TOOLSRV=\\smx.net\tools)
if not defined PRODUCTSRV (set PRODUCTSRV=\\smx.net\products)
set COPYFLAGS=/qrhyc
set SRCLOC=%PRODUCTSRV%\wsh

REM ======================================================================
REM Get OS Build Number and version info.
REM ======================================================================
if not defined OSVER (call %TOOLSRV%\SCRIPTS\OSVER.CMD)
if not defined OSVER (
    set ERRMSG=Unable to determine OS Version
    set ERRNUM=6
    goto :End
)

REM ======================================================================
REM Upgrade WSH - on W2K systems only
REM ======================================================================
if "%OSVER%" equ "W2K" (
    REM hack to get around random popup that occurs about reboot
    echo %SYSTEMROOT%\IDW\sleep.exe 60 > %SYSTEMDRIVE%\temp.bat
    echo %SYSTEMROOT%\IDW\kill -f scripten.exe>> %SYSTEMDRIVE%\temp.bat

    echo Updating to WSH 5.6 ...
    start /min cmd.exe /c "%SYSTEMDRIVE%\temp.bat"
    call %SRCLOC%\scripten.exe /Q
    if "%ERRORLEVEL%" neq "0" (
        set ERRMSG=Install failure %ERRORLEVEL%
        set ERRNUM=6
    )
    del %SYSTEMDRIVE%\temp.bat /f/q >NUL 2>&1
) else (
    set ERRMSG=WSH 5.6 or greater is already installed on XP and W2K3.
    set ERRNUM=0
    goto :End
)
goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo update_wsh.cmd
echo.
echo Example: update_wsh.cmd [/?]
echo.
set ERRNUM=1
set ERRMSG=Gave you the usage

:End
popd
if defined ERRMSG (echo Err%ERRNUM% - %ERRMSG%)
exit /b %ERRNUM%