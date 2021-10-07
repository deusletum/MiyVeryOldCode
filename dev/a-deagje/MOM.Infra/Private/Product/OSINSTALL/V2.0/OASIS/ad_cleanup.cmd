@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=

REM ======================================================================
REM Copyright . 2004
REM
REM Module    : ad_cleanup.cmd
REM
REM Summary   : Cleans up computers machines account and DNS entries
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM
REM History   : (1/20/2004) John Heaton - Initial coding
REM           : (5/27/2004) Mark Yocom - Changed log location to C:\LOGS
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" neq "" (goto :Usage)

REM ======================================================================
REM set local DNS Servers
REM ======================================================================
set LOGFILE=C:\LOGS\ad_cleanup.log
set FILELOC=\\smx.net\tools\oasis
set DNS=smxdc03.smx.net smxdc02.smx.net baldc01.balvenie.smx.net
set USERDNS=SMX.NET BALVENIE.SMX.NET
set MACHINES=%COMPUTERNAME%D %COMPUTERNAME%E

REM ======================================================================
REM do all the work.
REM ======================================================================
if "%SYSTEMDRIVE%" neq "C:" (goto :End)
if exist %LOGFILE% (del %LOGFILE%)
for %%d in (%DNS%) do (
    for %%e in (%MACHINES%) do (
        for %%f in (%USERDNS%) do (
            echo Deleting DNS entries for %%e in %%f from %%d @ %TIME%>>%LOGFILE% 2>&1
            call dnscmd.exe %%d /NodeDelete %%f %%e /f >>%LOGFILE% 2>&1
            echo. >>%LOGFILE%
            echo. >>%LOGFILE%
            echo Deleting machines account for %%e from %%f @ %TIME%>>%LOGFILE% 2>&1
            call cscript //nologo %FILELOC%\delaccount.vbs %%e %%f >>%LOGFILE% 2>&1
            echo. >>%LOGFILE%
            echo. >>%LOGFILE%
        )
    )
)
goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo ad_cleanup.cmd
echo.
echo Example: ad_cleanup.cmd
echo.
set ERRNUM=1

:End
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%