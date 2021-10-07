@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=

REM ======================================================================
REM Copyright . 2004
REM
REM Module    : IEHotfix.cmd
REM
REM Summary   : Installs IE hotfix MS04-004 on W2K
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM             2 - Unable to get OS version
REM             6 - Install Failure
REM
REM History   : (02/16/2004) Dean Gjedde - Initial coding
REM             (03/08/2004) Dean Gjedde - Added HotFix Q823559
REM             (08/02/2004) Dean Gjedde - Added HotFix 867801
REM             (12/7/2004) John Heaton  - Updated for Hotfix 889669
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" neq "" (goto :Usage)

REM ======================================================================
REM Set vars and check params
REM ======================================================================
if not defined PRODUCTSRV (set PRODUCTSRV=\\smx.net\products)
if not defined OSVER (
    if exist \\smx.net\tools\scripts\osver.cmd (
        call \\smx.net\tools\scripts\osver.cmd
    ) else (
        set ERRMSG=Unable to run \\smx.net\tools\scripts\osver.cmd
        set ERRNUM=2
        goto :End
    )
)
if /i "%PROCESSOR_ARCHITECTURE%" neq "X86" (
    set ERRMSG=IEHotfix.cmd is not supported on %PROCESSOR_ARCHITECTURE%
    goto :End
)
if /i "%OSVER%" neq "W2K" (
    set ERRMSG=IEHotfix.cmd is not supported on %OSVER%
    goto :End
)
REM /Q set silent install and /R:N forces no reboot
set PARAMS=/Q /R:N
REM This is the error reported when ie needs a reboot
set IEREBOOT=3010

REM ======================================================================
REM Get IE Version
REM ======================================================================
for /f "skip=4 tokens=3" %%i in ('reg query "HKLM\software\microsoft\internet explorer" /v Version 2^>nul') do set IEVER=%%i
set IE6SP1VER=6.0.2800.1106
set IE55SP2VER=5.50.4807.2300
if /i "%IEVER%" equ "%IE6SP1VER%" (
    set PRODUCT=IE6SP1
    goto :Hotfix
)
if /i "%IEVER%" equ "%IE55SP2VER%" (
    set PRODUCT=IE55SP2
    goto :Hotfix
)
set ERRMSG=%IEVER% is not supported by IEHotfix.cmd
goto :End

REM ======================================================================
REM Check Failure Information
REM ======================================================================
:HotFix
call "%PRODUCTSRV%\ie\IE_HotFix\%PRODUCT% hotfix\MS04-004\X86\W2K\Q832894.exe" %PARAMS%
set ERRORNUMBER=%ERRORLEVEL%
if "%ERRORNUMBER%" neq "0" (
    if "%ERRORNUMBER%" neq "%IEREBOOT%" (
        set ERRNUM=6
        set ERRMSG=%PRODUCT% Failed to install
        goto :End
    )
)


set Hotfix.File=IE6.0sp1-KB889669-Windows-2000-XP-x86
set HOTFIX.Dir=com_microsoft.Q889669_IE6_SP1
set HOTFIX.Server=\\Smx.net\os\ossupport\hotfixes\WU\Software\%OSLANG%\com_microsoft.internetexplorer6x\x86win2k\
if /i "%OSLANG%" equ "CN" (
    set SUBLANG=CHS
    set HOTFIX.Server=\\Smx.net\os\ossupport\hotfixes\WU\Software\zh%OSLANG%\com_microsoft.internetexplorer6x\x86win2k\
) else if /i "%OSLANG%" equ "TW" (
    set SUBLANG=CHT
    set HOTFIX.Server=\\Smx.net\os\ossupport\hotfixes\WU\Software\zh%OSLANG%\com_microsoft.internetexplorer6x\x86win2k\
) else if /i "%OSLANG%" equ "EN" (
    set SUBLANG=ENU
) else if /i "%OSLANG%" equ "JA" (
    set SUBLANG=JPN
) else if /i "%OSLANG%" equ "DE" (
    set SUBLANG=DEU
) else if /i "%OSLANG%" equ "FR" (
    set SUBLANG=FRA
) else if /i "%OSLANG%" equ "KO" (
    set SUBLANG= KOR
) else if /i "%OSLANG%" equ "ES" (
    set SUBLANG=ESN
) else if /i "%OSLANG%" equ "IT" (
    set SUBLANG=ITA
) else if /i "%OSLANG%" equ "RU" (
    set SUBLANG=RUS
) else if /i "%OSLANG%" equ "HE" (
    set SUBLANG=HEB
) else (
    set SUBLANG=ENU
)
call %HOTFIX.Server%\%HOTFIX.Dir%\%HOTFIX.File%-%SUBLANG%.exe -z -q
set ERRORNUMBER=%ERRORLEVEL%
if "%ERRORNUMBER%" neq "0" (
    if "%ERRORNUMBER%" neq "%IEREBOOT%" (
        set ERRNUM=6
        set ERRMSG=%PRODUCT% Failed to install
        goto :End
    )
)

shutdown.exe /r /f /t 10
set ERRMSG=IEHotfix.cmd completed successfully
goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Installs IE hotfix MS04-004 on W2K for IE55SP2 or IE6SP1
echo and installs IE Hotfix MS04-040 for W2K on IE6SP1
echo.
echo Usage:
echo IEHotfix.cmd
echo.
echo IEHotfix.cmd will reboot after install of Hotfixes
set ERRNUM=1

:End
echo %TIME%>>%SYSTEMDRIVE%\logs\IEHotfix.log
echo %ERRMSG%>>%SYSTEMDRIVE%\logs\IEHotfix.log

popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%