@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=

REM ======================================================================
REM Copyright . 2004
REM
REM Module    : regimport.cmd
REM
REM Summary   : Imports all regkeys in a given directory
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM
REM History   : (3/01/2004) John Heaton - Initial coding
REM             (5/26/2004) John Heaton - Trivial Change (goto ENd line 31)
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" neq "" (goto :Usage)

REM ======================================================================
REM Imports regkeys
REM ======================================================================
if not defined TOOLSRV (set TOOLSRV=\\SMX.NET\TOOLS)
for %%i in (%TOOLSRV%\POPS\REGS\*.reg) do (regedit -s %%i)
goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo regimport.cmd
echo.
echo Description: Imports reg files for POPS
set ERRNUM=0

:End
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%