@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=

REM ======================================================================
REM Copyright . 2004
REM
REM Module    : MSMQ_Config.txt
REM
REM Summary   : Configures MSMQ
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM
REM History : (08/10/03) Jennifer LaVigne - Initial coding
REM           (03/16/04) John Heaton      - Updated to run MSMQ.TXT from share
REM           (04/13/04) John Heaton      - Bug # 3508 (adding start /wait)
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" neq "" (goto :Usage)
if not defined TOOLSRV (set TOOLSRV=\\smx.net\tools)

REM ======================================================================
REM Do the install
REM ======================================================================
if not exist %TOOLSRV%\DROP\MSMQ.TXT (
    set ERRNUM=2
    set ERRMSG=Unable to locate MSMQ.TXT on %TOOLSRV%\DROP
    goto :End
)

start /wait sysocmgr.exe /i:sysoc.inf /u:%TOOLSRV%\DROP\MSMQ.TXT

goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo msmq_config.cmd
echo.
echo Example: msmq_config [/?]
echo.
set ERRNUM=1
goto :End

:End
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%