@echo OFF
if defined ECHO (echo %ECHO%)
set ERRNUM=0
set ERRMSG=

REM ======================================================================
REM Copyright . 2004
REM
REM Module : MDAC_HOTFIX.cmd
REM
REM Summary: Installs MDAC hotfix Q832483.
REM
REM History: (03/23/2004) Todd Ellis - Initial coding
REM
REM Error Codes:  1 - MDAC Hotfix Q832483 installation has failed
REM               2 - Invalid User Input
REM
REM

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" neq "" (goto :Usage)

REM ======================================================================
REM Make sure things are setup as expected.
REM ======================================================================
if not defined TOOLSRV (
    set TOOLSRV=\\smx.net\tools
)
if not defined PRODUCTSRV (
    set PRODUCTSRV=\\smx.net\products
)
if not exist %SYSTEMDRIVE%\logs (
    md %SYSTEMDRIVE%\logs
)
set LOGFILE=%SYSTEMDRIVE%\logs\Inst_MDAC.log
echo %DATE% - %TIME% : MDAC_HOTFIX Starting >>%LOGFILE%

REM ======================================================================
REM Set some enviroment variables
REM ======================================================================
if not defined OSLANG (call %TOOLSRV%\scripts\oslang.cmd >NUL 2>&1)
if /i "%OSLANG%" equ "ENU" (call %TOOLSRV%\scripts\oslang.cmd 2 >NUL 2>&1)
if not defined OSVER (call %TOOLSRV%\scripts\osver.cmd >NUL 2>&1)
if "%OSVER%" neq "W2K" (
    set ERRNUM=0
    set ERRMSG=%OSVER% is not supported.
    echo %DATE% - %TIME% : %OSVER% is not supported. %ERRORLEVEL% >>%LOGFILE%
    goto :End
)

REM set product install location
set FIXLOC=%PRODUCTSRV%\mdac2.8\%OSVER%\%OSLANG%\%PROCESSOR_ARCHITECTURE%\HOTFIX

REM ======================================================================
REM Install HOTFIX Q832483
REM ======================================================================
echo Installing HOTFIX Q832483 from %FIXLOC%\...
call %TOOLSRV%\scripts\oslang.cmd 3 >NUL 2>&1
echo %DATE% - %TIME% : Beginning install of MDAC HOTFIX Q832483 from %FIXLOC%\... >>%LOGFILE%
rd %SYSTEMDRIVE%\Q832483 /q /s >NUL 2>&1
md %SYSTEMDRIVE%\Q832483 >NUL 2>&1
start cmd.exe /c %FIXLOC%\%OSLANG%_Q832483_MDAC_%PROCESSOR_ARCHITECTURE%.EXE /q /t:%SYSTEMDRIVE%\Q832483 /c:"dahotfix.exe /q"

REM Sleeping here to wait for reboot popup.
%SYSTEMROOT%\IDW\sleep.exe 10

REM Killing reboot popup, reboot will occur later in POPS execution.
%SYSTEMROOT%\IDW\kill.exe -f dahotfix.exe

findstr /c:"Setup was successful." %WINDIR%\dahotfix.log>>%LOGFILE%
copy /y %WINDIR%\dahotfix.log %SYSTEMDRIVE%\logs >NUL 2>&1
if "%ERRORLEVEL%" neq "0" (
    findstr /c:"Setup was NOT successful." %WINDIR%\dahotfix.log>>%LOGFILE%
    set ERRNUM=1
    set ERRMSG=MDAC Hotfix Q832483 installation has failed see %LOGFILE% for details.
    echo %DATE% - %TIME% : MDAC Hotfix Q832483 installation has failed on this machine. >>%LOGFILE%
) else (
    set ERRMSG=MDAC Hotfix Q832483 installation completed successfully.
    echo %DATE% - %TIME% : MDAC Hotfix Q832483 installation completed successfully. >>%LOGFILE%
)
goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
set ERRNUM=2
set ERRMSG=Invalid User Input
echo.
echo Usage:
echo.
echo MDAC_HOTFIX.cmd
echo.
echo Example: MDAC_HOTFIX.cmd
echo.
echo Installs MDAC Hotfix Q832483
echo.

:End
if defined ERRMSG (
    echo Err%ERRNUM% - %ERRMSG%
    echo Err%ERRNUM% - %ERRMSG% >>%LOGFILE%
)
exit /b %ERRNUM%