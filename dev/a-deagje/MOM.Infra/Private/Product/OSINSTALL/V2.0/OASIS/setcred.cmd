@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=

REM ======================================================================
REM Copyright . 2003
REM
REM Module : setcred.cmd
REM
REM Summary: Just adding header info, etc.
REM
REM History: (8/27/2003) John Heaton - Brought to coding standards.
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" equ "/?" (goto :Usage)
if "%1" equ "?" (goto :Usage)
if "%1" equ "-?" (goto :Usage)

REM ======================================================================
REM Run Set.vbs
REM ======================================================================
attrib -r c:\keep\*>NUL
if exist "C:\KEEP\SET.VBS" (CSCRIPT //NOLOGO C:\KEEP\SET.VBS SMX,REDMOND)

goto :End
REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo setcred.cmd
echo.
echo Example: setcred.cmd
echo.
set ERRNUM=1

:End
set LOGFILE=
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%
