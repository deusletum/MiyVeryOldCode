@echo off
if defined ECHO (echo %ECHO%)
pushd .
set ERRNUM=0
set ERRMSG=
REM ======================================================================
REM Copyright . 2004
REM
REM Module    : SNMP-WMIProv.cmd
REM
REM Summary   : Installs the SNMP WMI PRovider
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM
REM History   : (09/09/2002) Jennifer LaVigne - Initial coding
REM             (01/14/2004) Dean Gjedde - Changed paths, and brough up to
REM                 coding standards
REM ======================================================================
setlocal

if /i {%1} equ {/?} (goto :Usage)
set TOOLSRV=\\smx.net\tools
set COPYFLAGS=/qrhyc

REM ***************************************************************************
REM Setting common parameters.
REM ***************************************************************************
set SRCSRV=%TOOLSRV%\toolbox\SNMP-WMIProv
set WMI_DIR=%WINDIR%\System32\Wbem

REM ***************************************************************************
REM Copy files for installation.
REM ***************************************************************************
xcopy  %SRCSRV%\snmp*  %WMI_DIR%\ %COPYFLAGS%
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=1
    set ERRMSG=Unable to copy %SRCSRV%\snmp*
    goto :End
)
xcopy  %SRCSRV%\smi*  %WMI_DIR%\snmp\ %COPYFLAGS%
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=1
    set ERRMSG=Unable to copy %SRCSRV%\snmp*
    goto :End
)

REM ***************************************************************************
REM Install and register files copied.
REM ***************************************************************************
mofcomp %WINDIR%\System32\Wbem\snmpreg.mof
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=1
    set ERRMSG=Could not mofcomp %WINDIR%\System32\Wbem\snmpreg.mof
    goto :End
)
mofcomp %WINDIR%\System32\Wbem\snmpsmir.mof
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=1
    set ERRMSG=Could not mofcomp %WINDIR%\System32\Wbem\snmpsmir.mof
    goto :End
)

regsvr32 /s %WINDIR%\System32\Wbem\snmpincl.dll
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=1
    set ERRMSG=Could not register %WINDIR%\System32\Wbem\snmpincl.dll
    goto :End
)
regsvr32 /s %WINDIR%\System32\Wbem\snmpsmir.dll
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=1
    set ERRMSG=Could not register %WINDIR%\System32\Wbem\snmpsmir.dll
    goto :End
)
goto :End

:Usage
echo.
echo -------------------------------------------------------------------------
echo.
echo SNMP-WMIProv.cmd
echo.
echo Example: SNMP-WMIProv
echo.
echo  Copies the necessary Wmi Snmp Files to the
echo  Wbem Folder on the local machine.
echo.
echo -------------------------------------------------------------------------
echo.
set ERRNUM=1

:End
popd
if defined ERRMSG echo %ERRMSG%
exit /b %ERRNUM%
endlocal