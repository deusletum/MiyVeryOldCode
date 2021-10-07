@echo OFF
if defined ECHO (echo %ECHO%)
pushd.

set ERRNUM=0
set ERRMSG=
set DESTDIR=
set SRCLOC=
REM ======================================================================
REM Copyright . 2003
REM
REM Module : inst_dbg.cmd
REM
REM Summary: Install's Latest Blessed Debugger.
REM
REM History: (7/9/2003) John Heaton - Initial coding
REM ======================================================================
REM Error Codes:
REM             1 = Invalid user input or Usage
REM             2 = Invalid Destination Path
REM             3 = Access Denied (Not Used)
REM             4 = Path Not Found (Not Used)
REM             5 = Reboot required (Not Used)
REM             6 = Install Failure
REM ======================================================================

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if {%1} equ "/?" (goto :Usage)
if {%1} equ "?" (goto :Usage)
if {%1} equ "-?" (goto :Usage)

if not defined TOOLSRV (set TOOLSRV=\\smx.net\tools)
REM ======================================================================
REM Set Destination Directory
REM ======================================================================
set DESTDIR=%1
if not defined DESTDIR (set DESTDIR="%PROGRAMFILES%\Debuggers")
if /i {%DESTDIR%} equ {%SYSTEMROOT%\SYSTEM32} (
    set ERRNUM=2
    set ERRMSG=Cannot install to a system directory
    goto :End
)
if /i {%DESTDIR%} equ {%SYSTEMROOT%\SYSWOW64} (
    set ERRNUM=2
    set ERRMSG=Cannot install to a system directory
    goto :End
)
set SRCLOC=%TOOLSRV%\Debugger
set DESTDIR=%DESTDIR:"=%
md "%DESTDIR%" >NUL 2>&1

REM ======================================================================
REM Do the install
REM ======================================================================
call %SRCLOC%\version.cmd
echo Install Debugger Version %DBG_VERSION% to %DESTDIR%
start /wait %SRCLOC%\setup_%PROCESSOR_ARCHITECTURE%.exe /z /q /i "%DESTDIR%"
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=6
    set ERRMSG=Installation Failed
)
cscript //nologo %TOOLSRV%\drop\addpath.vbs PRE "%DESTDIR%"
goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo inst_dbg.cmd [DESTDIR]
echo.
echo Example: inst_dbg.cmd D:\Debuggers
echo.
echo Optional Parameters:
echo    "Destination Directory"
echo.
echo.
set ERRNUM=1
goto :End

:End
popd
if defined ERRMSG (echo Err%ERRNUM% - %ERRMSG%)
exit /b %ERRNUM%