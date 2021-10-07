@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=
REM ======================================================================
REM Copyright . 2004
REM
REM Module    : debug.cmd
REM
REM Summary   :
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM
REM History   : (01/01/01) John Heaton - Initial coding
REM             (05/08/01) Patrick Kong - Add reference to c:\delete
REM                 & %file_svr%
REM             (1/10/02) Glenn LaVigne - take out dependancies, make
REM                 it work with current systems
REM             (3/05/02) Glenn LaVigne - added support for NONE product,
REM                 fixed usage and made more user friendly
REM             (6/17/02) Jennifer LaVigne - more formatting and took out
REM                 call to setcred.cmd, kill previous debug windows first
REM             (4/22/03) Dean Gjedde - Fixed bug 2682 and made compliant
REM                 with coding standards
REM             (7/28/03) Jennla - infra bug 3025
REM             (10/13/03) Dean Gjedde - un-remed line 103, this is
REM                 needed for stress
REM             (01/09/04) Dean Gjedde - Added reg entry for MOMX
REM                 tracelevel, updated paths for \\smx.net
REM                 brought up to coding standards
REM ======================================================================
setlocal

if /i "%1" equ "/?" (goto :Usage)
echo %0 is Running...

set TOOLSRV=\\smx.net
set COPYFLAGS=/qrhyc

REM Kill any current versions of debugview
kill -f dbgview.exe >NUL

REM ***************************************************************************
REM Set defaults - can be overridden with user input
REM ***************************************************************************
set DBG_PRODUCT=MOMX
set DBG_LEVEL=3
set DBG_LOGFILE=%SYSTEMDRIVE%\dbmon.log
set DBG_MAXFILELOG=1000

REM ***************************************************************************
REM Setting parameters based on input.
REM ***************************************************************************
if "%1" neq "" (set DBG_PRODUCT=%1)
if "%2" neq "" (set DBG_LEVEL=%2)
if "%3" neq "" (set DBG_LOGFILE=%SYSTEMDRIVE%\%3_dbmon.log)

REM ***************************************************************************
REM Copy essential files to run this.
REM ***************************************************************************
md %SYSTEMROOT%\bricktools >NUL 2>NUL
xcopy %TOOLSRV%\tools\bricktools\%PROCESSOR_ARCHITECTURE%\dbgview.exe %SYSTEMROOT%\bricktools\ %COPYFLAGS% >NUL
if "%ERRORLEVEL%" neq "0" (
    set ERRUM=1
    set ERRMSG=FATAL ERROR - Unable to copy %TOOLSRV%\tools\bricktools\%PROCESSOR_ARCHITECTURE%\dbgview.exe
    goto :End
)
xcopy %TOOLSRV%\tools\bricktools\%PROCESSOR_ARCHITECTURE%\dbgview.reg %SYSTEMROOT%\bricktools\ %COPYFLAGS% >NUL
if "%ERRORLEVEL%" neq "0" (
    set ERRUM=1
    set ERRMSG=FATAL ERROR - Unable to copy %TOOLSRV%\tools\bricktools\%PROCESSOR_ARCHITECTURE%\dbgview.reg
    goto :End
)

REM ***************************************************************************
REM Add to registry based on product input.
REM ***************************************************************************
if /i "%DBG_PRODUCT%" equ "NONE" (goto :DebugLaunch)

set AC_DBG_REGROOT="HKLM\software\microsoft\windows NT\currentversion\tracing\ACS"
if /i "%DBG_PRODUCT%" equ "AC" (goto :ACEntries)
if /i "%DBG_PRODUCT%" equ "AC10SP1" (goto :ACEntries)
if /i "%DBG_PRODUCT%" equ "AC10SP2" (goto :ACEntries)
if /i "%DBG_PRODUCT%" equ "MOM" (goto :MOMEntries)
if /i "%DBG_PRODUCT%" equ "MOMSDK3" (goto :MOMEntries)

set MOMX_DBG_REGROOT="HKLM\software\mission critical software"
if /i "%DBG_PRODUCT%" equ "MOMX" (
    %SYSTEMROOT%\bricktools\Reg.exe add %MOMX_DBG_REGROOT% /v TraceLevel /t REG_DWORD /d %DBG_LEVEL% /f >NUL
    %SYSTEMROOT%\bricktools\Reg.exe add %MOMX_DBG_REGROOT% /v TraceCircularLines /t REG_DWORD /d 0x000f4240 /f >NUL
    goto :DebugLaunch
)

set ERRMSG=FATAL ERROR - %DBG_PRODUCT% is not recognized as a valid product, see Usage (%0 /?)
set ERRNUM=1
goto :End

REM ***************************************************************************
REM Create debug entries for AC products in registry
REM ***************************************************************************
:ACEntries
%SYSTEMROOT%\bricktools\Reg.exe add %AC_DBG_REGROOT% /v AlwaysODS /t REG_DWORD /d 00000001 /f >NUL
%SYSTEMROOT%\bricktools\Reg.exe add %AC_DBG_REGROOT% /v Level /t REG_DWORD /d %DBG_LEVEL% /f >NUL
%SYSTEMROOT%\bricktools\Reg.exe add %AC_DBG_REGROOT% /v ControlFlags /t REG_DWORD /d 65535 /f >NUL
%SYSTEMROOT%\bricktools\Reg.exe add %AC_DBG_REGROOT% /v Active /t REG_DWORD /d 00000001 /f >NUL
goto :DebugLaunch

REM ***************************************************************************
REM Create debug entries for MOM products in registry
REM ***************************************************************************
:MOMEntries
set MOM_DBG_REGROOT="HKLM\software\mission critical software"
%SYSTEMROOT%\bricktools\Reg.exe add %MOM_DBG_REGROOT% /v TraceLevel /t REG_DWORD /d %DBG_LEVEL% /f >NUL
goto :DebugLaunch


REM ***************************************************************************
REM Add to registry based on product input.
REM ***************************************************************************
:DebugLaunch

REM Merge in reg entries for default config and start up debugview
%SYSTEMROOT%\regedit -s %SYSTEMROOT%\bricktools\dbgview.reg
if "%ERRORLEVEL%" neq "0" (
    set ERRUM=1
    set ERRMSG=FATAL ERROR - Unable to import %SYSTEMROOT%\bricktools\dbgview.reg
    goto :End
)
start %SYSTEMROOT%\bricktools\dbgview.exe /t /l %DBG_LOGFILE% /m %DBG_MAXFILELOG%
if "%ERRORLEVEL%" neq "0" (
    set ERRUM=1
    set ERRMSG=FATAL ERROR - Unable to start %SYSTEMROOT%\bricktools\dbgview.exe
    goto :End
)

echo.
echo Started up an instance of DebugView - log file at %DBG_LOGFILE%
if /i "%DBG_PRODUCT%" neq "NONE" (
    echo Turned on tracing for %DBG_PRODUCT% at debug level %DBG_LEVEL%
)

goto :End

:Usage
echo.
echo -------------------------------------------------------------------------
echo.
echo debug.cmd [Product] [Tracing]
echo.
echo Example: debug
echo          debug ac10sp2
echo          debug mom 2
echo.
echo PRODUCT:
echo Currently supports: NONE, AC, AC10SP1, AC10SP2, MOM, MOMX, and MOMSDK
echo    NONE - just launches DebugView without altering any tracing information
echo    Default value is MOMX
echo.
echo TRACING:
echo You must specify a product to be able to specify the trace level
echo Default value is 3
echo.
echo   Note - this batch file kills any currently running instances of DebugView
echo.
echo -------------------------------------------------------------------------
echo.

:End
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%
endlocal