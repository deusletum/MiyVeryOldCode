@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=

REM ======================================================================
REM Copyright . 2004
REM
REM Module    : inst_ac10sp2.cmd
REM
REM Summary   : Based on old ACINST but less crap
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM
REM History   : (4/15/2004) John Heaton - Initial coding
REM           : (7/01/2004) John Heaton - Fixed Infra Bug 3781
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if /i "%4" equ "" (goto :Usage)
if "%1" equ "/?" (goto :Usage)
if "%1" equ "?" (goto :Usage)
if "%1" equ "-?" (goto :Usage)

if /i "%1" equ "latest" (
    set inst_ac10sp2.Build=0780
) else (
    set inst_ac10sp2.Build=%1
)

if /i "%2" equ "EN" (
    set inst_ac10sp2.LOC=EN
    goto :LANGOK
)
if /i "%2" equ "JA" (
    set inst_ac10sp2.LOC=JA
    goto :LANGOK
)
set ERRNUM=1
set ERRMSG=Language choice must be EN or JA
goto :Usage
:LANGOK

if /i "%3" equ "FRE" (
    set inst_ac10sp2.Type=X86FRE
    goto :CHOICEOK
)
if /i "%3" equ "CHK" (
    set inst_ac10sp2.Type=X86CHK
    goto :CHOICEOK
)

set ERRNUM=1
set ERRMSG=Type choice must be FRE or CHK
goto :Usage
:CHOICEOK

if /i "%4" equ "Server" (
    set inst_ac10sp2.Component=
    goto :COMPOK
)
if /i "%4" equ "Client" (
    set inst_ac10sp2.Component=ADDLOCAL=Client
    goto :COMPOK
)

set ERRNUM=1
set ERRMSG=Component choice must be Server or Client
goto :Usage
:COMPOK

if /i "%5" equ "/I" (
    set inst_ac10sp2.Interactive=
) else (
    set inst_ac10sp2.Interactive=/Q
)

REM ======================================================================
REM set needed parameters
REM ======================================================================
set inst_ac10sp2.ProdPath=\\smx.net\builds\ac10sp2
set inst_ac10sp2.MSIPath=CDIMAGE\Microsoft Application Center 2000.MSI
set inst_ac10sp2.PID=PIDKEY=H6TWQTQQM8HXJYGD69F7R84VM
set inst_ac10sp2.LogFile=%SYSTEMDRIVE%\Logs\inst_ac10sp2.log

REM ======================================================================
REM Install AC
REM ======================================================================
REM Install is as follows
REM \\SERVER\SHARE\LOC\BUILD\TYPE\MSI LOGFILE SERVER or CLIENT PIDKEY
echo Installing Application Center 2000 SP2 from "%inst_ac10sp2.ProdPath%\%inst_ac10sp2.LOC%\%inst_ac10sp2.Build%\%inst_ac10sp2.Type%\%inst_ac10sp2.MSIPath%"
echo.
call msiexec /i "%inst_ac10sp2.ProdPath%\%inst_ac10sp2.LOC%\%inst_ac10sp2.Build%\%inst_ac10sp2.Type%\%inst_ac10sp2.MSIPath%" /lv* %inst_ac10sp2.LogFile% %inst_ac10sp2.Component% %inst_ac10sp2.Interactive% %inst_ac10sp2.PID%
set ERRMSG=%ERRORLEVEL% - Application Center 2000 SP2 instalation is now complete.
set ERRNUM=0
goto :End
REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo inst_ac10sp2.cmd BUILD LANG TYPE COMPONENTS
echo.
echo Example: inst_ac10sp2.cmd 0780 EN FRE SERVER
echo.
echo Required Parameters:
echo    Build      : latest or number
echo    Language   : EN, or JA
echo    Type       : FRE, or CHK
echo    Components : Server, or Client
echo.
set ERRNUM=1

:End
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%