@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=

REM ======================================================================
REM Copyright . 2004
REM
REM Module    : copy_vcfiles.cmd
REM
REM Summary   : Temp File used to copy VC Files for Whidbey compiler
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM
REM History   : (11/3/2004) John Heaton - Initial coding
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" equ "/?" (goto :Usage)
if "%1" equ "?" (goto :Usage)
if "%1" equ "-?" (goto :Usage)

REM ======================================================================
REM
REM ======================================================================
xcopy \\smxfiles\privates\ianjirka\v3_machine_prep\vc_dlls\%PROCESSOR_ARCHITECTURE% %SYSTEMROOT%\SYSTEM32 /E /S /y

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo copy_vcfiles.cmd
echo.
echo Example: copy_vcfiles.cmd
echo.
set ERRNUM=1

:End
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%