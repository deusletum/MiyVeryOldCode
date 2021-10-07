@echo OFF
if defined ECHO (echo %ECHO%)
set ERRNUM=0
set ERRMSG=
REM ======================================================================
REM Copyright . 2004
REM
REM Module : Inst_MDAC.cmd
REM
REM Summary: Installs MDAC 2.8 and hotfix.
REM
REM History: (09/04/2003) John Heaton - Initial coding
REM          (01/30/2004) Todd Ellis  - Added install for Hotfix Q832483
REM          (01/30/2004) Todd Ellis  - Added more Errorchecking.
REM                                   - Added call to set.vbs if credentials are not present.
REM                                   - Added logging.
REM          (02/05/2004) Jheat       - Added goto :End to make Toddell look like he sucks less.
REM          (03/20/2004) Todd Ellis  - Breaking up initial install and hotfix install into seperate scripts.
REM          (03/22/2004) Todd Ellis  - Adding error logic for errorlevel "3010" (reboot expected).
REM          (04/13/2004) Dean Gjedde - Changed shutdown.exe to boot.vbs /reboot (bug # 3505)
REM
REM Error Codes:
REM               1 = MDAC Install Failure
REM               2 = Invalid User Input
REM
REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" neq "" (goto :Usage)

REM ======================================================================
REM Ensure things are where expected.
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
echo %DATE% - %TIME% : INST_MDAC Starting >%LOGFILE%


REM ======================================================================
REM Set some enviroment variables
REM ======================================================================
REM Set the OS Language information
if not defined OSLANG (call %TOOLSRV%\scripts\oslang.cmd >NUL 2>&1)
if /i "%OSLANG%" equ "ENU" (call %TOOLSRV%\scripts\oslang.cmd 2 >NUL 2>&1)

REM Set the OS Version information
if not defined OSVER (call %TOOLSRV%\scripts\osver.cmd >NUL 2>&1)

if "%OSVER%" neq "W2K" (
    set ERRNUM=0
    set ERRMSG=%OSVER% is not supported.
    echo %DATE% - %TIME% : %OSVER% is not supported. %ERRORLEVEL% >>%LOGFILE%
    goto :Reboot
)

REM set product install location
set SRCLOC=%PRODUCTSRV%\mdac2.8\%OSVER%\%OSLANG%\%PROCESSOR_ARCHITECTURE%

REM ======================================================================
REM Installs MDAC
REM ======================================================================
for /f "tokens=5 delims= " %%x in ('filever.exe %WINDIR%\system32\odbc32.dll') do (set MDACVER=%%x)
if "%MDACVER%" equ "3.525.1022.0" (
    echo %DATE% - %TIME% : MDAC 2.8 was already installed on this machine. >>%LOGFILE%
    set ERRMSG=MDAC 2.8 already installed.
    goto :Reboot
)

REM Install MDAC
echo Ensuring proper credentials are obtained...
call cscript //nologo  c:\keep\set.vbs smx,redmond >NUL 2>&1
echo.
echo Installing MDAC 2.8 from %SRCLOC%\...
echo %DATE% - %TIME% : Beginning install of MDAC 2.8 from %SRCLOC%\... >>%LOGFILE%
rd %SYSTEMDRIVE%\mdac /q/s >NUL 2>&1
call %SRCLOC%\mdac_typ.exe /q /t:%SYSTEMDRIVE%\mdac /c:"setup.exe /QNl"
if "%ERRORLEVEL%" neq "0" (
    if "%ERRORLEVEL%" equ "3010" (
        set ERRNUM=0
        set ERRMSG=MDAC 2.8 install returned: %ERRORLEVEL%, this error is expected, reboot reqiuired.
        echo %DATE% - %TIME% : MDAC 2.8 install returned:%ERRORLEVEL%, this error is expected, reboot reqiuired. >>%LOGFILE%
        goto :Reboot
    )
    set ERRNUM=1
    set ERRMSG=MDAC2.8 installation has failed see %LOGFILE% for details.
    echo %DATE% - %TIME% : MDAC 2.8 installation has failed on this machine. %ERRORLEVEL% >>%LOGFILE%
) else (
    echo %DATE% - %TIME% : MDAC2.8 installation completed successfully. >>%LOGFILE%
    set ERRMSG=MDAC2.8 installation completed successfully.
)

:Reboot
start /wait cmd.exe /c cscript.exe //nologo %SYSTEMROOT%\bricktools\boot.vbs /reboot
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
echo Inst_MDAC.cmd
echo.
echo Example: Inst_MDAC.cmd
echo.
echo Installs MDAC 2.8
echo.

:End
if defined ERRMSG (
    echo Err%ERRNUM% - %ERRMSG%
    echo Err%ERRNUM% - %ERRMSG% >>%LOGFILE%
)
exit /b %ERRNUM%