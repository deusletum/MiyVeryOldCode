@echo OFF
if defined ECHO (echo %ECHO%)
pushd.

set ERRNUM=0
set ERRMSG=
REM ======================================================================
REM Copyright . 2004
REM
REM Module : inst_ie.cmd
REM
REM Summary: Used to install various versions of IE
REM
REM History: (4/25/2003) John Heaton - Initial coding (based on GregBls File)
REM          (4/28/2003) John Heaton - Updated per Myocom's Suggestions.
REM          (8/20/2003) Mark Yocom  - Added additional sleep per jennla's
REM                                    suggestion (doubled all sleep times)
REM          (2/24/2004) John Heaton - Updating to better support W2K3

REM ======================================================================
REM Error Codes:
REM             1 = Invalid user input or Usage
REM             2 = Invalid Destination Path
REM             3 = Access Denied (Not Used)
REM             4 = Path Not Found
REM             5 = Reboot required (Not Used)
REM             6 = Install Failure
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" equ "/?" (goto :Usage)
if "%1" equ "?" (goto :Usage)
if "%1" equ "-?" (goto :Usage)

REM ======================================================================
REM Establish parameter entries
REM ======================================================================
set PRODUCT=%1
set LOGFILE=%SYSTEMDRIVE%\LOGS\INST_IE.LOG
md %SYSTEMDRIVE%\LOGS >NUL 2>&1
del %LOGFILE% >NUL 2>&1
if not defined PRODUCT (set PRODUCT=ie6sp1)
if not defined PRODUCTSRV (set PRODUCTSRV=\\smx.net\products)
if not defined TOOLSRV (set TOOLSRV=\\smx.net\tools)
if not defined OSVER (
    call %TOOLSRV%\scripts\osver.cmd>NUL
)

if not defined SPVER (
    call %TOOLSRV%\scripts\osver.cmd>NUL
)

if not defined OSVER (
    set ERRMSG=Unable to access %TOOLSRV% or you have no OS
    set ERRNUM=4
    goto :End
)

if /i "%OSVER%" neq "W2K" (
    set ERRMSG=Install only supported on W2K
    set ERRNUM=0
    goto :End
)

:Product
if /i "%PRODUCT%" equ "IE55SP2" (goto :IE55SP2)
if /i "%PRODUCT%" equ "IE6" (goto :IE6)
if /i "%PRODUCT%" equ "IE6SP1" (goto :IE6SP1)
goto :Usage

REM ======================================================================
REM Install IE55SP2
REM ======================================================================
:IE55SP2
echo Installing %PRODUCT%>%LOGFILE%
echo Installing %PRODUCT%
if exist %PRODUCTSRV%\ie\%PRODUCT%\ie5setup.exe (
    start /wait %PRODUCTSRV%\ie\%PRODUCT%\ie5setup.exe /q >NUL 2>&1
    goto :FailChk
) else (
    set ERRNUM=4
    set ERRMSG=Unable to locate %PRODUCTSRV%\ie\%PRODUCT%\ie5setup.exe
    goto :End
)

REM ======================================================================
REM Install IE6
REM ======================================================================
:IE6
echo Installing %PRODUCT%>%LOGFILE%
echo Installing %PRODUCT%

if exist %PRODUCTSRV%\ie\%PRODUCT%\ie6setup.exe (
    start /wait %PRODUCTSRV%\ie\%PRODUCT%\ie6setup.exe /q >NUL 2>&1
    goto :FailChk
) else (
    set ERRNUM=4
    set ERRMSG=Unable to locate %PRODUCTSRV%\ie\%PRODUCT%\ie6setup.exe
    goto :End
)

REM ======================================================================
REM Install IE6SP1
REM ======================================================================
:IE6SP1
echo Installing %PRODUCT%>%LOGFILE%
echo Installing %PRODUCT%
if exist %PRODUCTSRV%\ie\%PRODUCT%\ie6setup.exe (
    start /wait %PRODUCTSRV%\ie\%PRODUCT%\ie6setup.exe /q >NUL 2>&1
    goto :FailChk
    goto :End
) else (
    set ERRNUM=4
    set ERRMSG=Unable to locate %PRODUCTSRV%\ie\%PRODUCT%\ie6setup.exe
    goto :End
)

REM ======================================================================
REM Check Failure Information
REM ======================================================================
:FailChk
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=6
    set ERRMSG=%PRODUCT% Failed to install IE ERROR %ERRORLEVEL%
) else (
    set ERRNUM=0
    set ERRMSG=Install completed
)
goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo INST_IE.cmd PRODUCT
echo.
echo Example: INST_IE.cmd IE6SP1
echo.
echo Valid Products:
echo    "IE55SP2, IE6, IE6SP1"
echo.
set ERRNUM=1
set ERRMSG=Displaying usage
goto :End

:End
popd
echo. >>%LOGFILE%
echo %ERRMSG% - Errorlevel %ERRNUM% >>%LOGFILE%
if "%ERRNUM%" equ "0" (
    echo %ERRMSG% - Errorlevel %ERRNUM%
    call shutdown -r -f -t 5
) else (
    if "%ERRNUM%" neq "1" (
        call ditspause.exe "%ERRMSG% - Errorlevel %ERRNUM%"
    )
)
exit /b %ERRNUM%