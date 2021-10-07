@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=
REM ======================================================================
REM Copyright . 2004
REM
REM Module    : Inst_BITS.cmd
REM
REM Summary   : Install BITS client and Server
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM             2 - Install Failure
REM
REM History   : (1/22/2004) Dean Gjedde - Initial coding
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" equ "" (goto :Usage)
if "%1" equ "/?" (goto :Usage)
if "%1" equ "?" (goto :Usage)
if "%1" equ "-?" (goto :Usage)

if /i "%PROCESSOR_ARCHITECTURE%" neq "X86" (
    set ERRNUM=1
    set ERRMSG=Inst_BITS.cmd only supports install BITS on X86
    goto :End
)
set FILESVR=\\smx.net
set BITS_SOURCE=products\bits\1.5\en

REM ======================================================================
REM Start Server or Client install logic
REM ======================================================================
set TYPE=%1
if /i "%TYPE%" equ "SERVER" (
    goto :Install
)
if /i "%TYPE%" equ "CLIENT" (
    goto :Install
)
set ERRNUM=1
set ERRMSG=%TYPE% not supported
goto :End

REM ======================================================================
REM Install BITS
REM ======================================================================
:Install
echo Installing BITS %TYPE%
start /wait %FILESVR%\%BITS_SOURCE%\%TYPE%\bits_v15_%TYPE%_setup.exe -q -z
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=2
    set ERRMSG=BITS install for %TYPE% failed.
    goto :End
)
echo BITS %TYPE% install succeeded
goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo Inst_BITS.cmd install the Background Intelligent Transfer Service Version 1.5 Client or Server
echo.
echo Usage:
echo.
echo Inst_BITS.cmd TYPE
echo.
echo Example: Inst_BITS.cmd server
echo.
echo Required Parameters:
echo    "TYPE is CLIENT or SERVER"
echo.
set ERRNUM=1

:End
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%