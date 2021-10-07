@echo OFF
if defined ECHO (echo %ECHO%)
pushd.

REM ======================================================================
REM Copyright . 2004
REM
REM Module : inst_urt.cmd
REM
REM Summary: Installs .NET Framework and SDK
REM
REM History: (7/15/2003) John Heaton - Initial coding
REM          (2/15/2004) John Heaton - Updating things to make Gavme happy.
REM                                  - Yocom and I rewrote the entire thing
REM                                  - Gavme and Brent watched us.
REM          (2/23/2004) myocom - Added a little more info to error message
REM          (2/23/2004) Todd Ellis  - Added a little fix for above. Seems to not like () in variables..
REM                                  - enclosed in "" and works fine.
REM          (3/20/2004) John Heaton - Added some aditional logging
REM          (4/13/2004) Dean Gjedde - Added support for install AMD64 URT (bug # 3506)
REM          (8/26/2004) Glenn LaVigne - Support URT 2.0 (arb build) installs
REM         (10/7/2004) Glenn LaVigne - Support localized verison of URT 2.0 - new MUI installs (bug 3860)
REM
REM ======================================================================
REM Error Codes:
REM             1 = Invalid user input or Usage
REM             2 = Unable to locate file
REM             3 = Unable to verify language
REM             6 = Install Failure
REM ======================================================================
set LOGFILE=%SYSTEMDRIVE%\LOGS\INST_URT.LOG
md %SYSTEMDRIVE%\LOGS>NUL 2>&1
echo Starting INST_URT @ %DATE% - %TIME%>%LOGFILE%

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" equ "/?" (goto :Usage)
if "%1" equ "?" (goto :Usage)
if "%1" equ "-?" (goto :Usage)
set SRCLOC=

REM ======================================================================
REM Set some enviroment variables
REM ======================================================================
if not defined TOOLSRV (set TOOLSRV=\\smx.net\tools)
if not defined PRODUCTSRV (set PRODUCTSRV=\\smx.net\products)
if not defined OSLANG (call %TOOLSRV%\scripts\oslang.cmd >NUL 2>&1)
if not defined OSLANG (
    set ERRNUM=3
    set ERRMSG=Failed to determine OS Language.
    goto :End
)


REM ======================================================================
REM Figure out build to install based on user input - otherwise use 4322 - URT 1.1
REM Override for AMD64 (should be able to get rid of this in future)
REM ======================================================================


if /i "%PROCESSOR_ARCHITECTURE%" equ "AMD64" (
    set OSLANG=EN
    set BLESSBLD=40405
    set BLESSVER=v2.0
    goto :InstallURT
)

if "%1" equ "" (
    set BLESSBLD=4322
    set BLESSVER=v1.1
    goto :InstallURT
)

set BLESSBLD=%1

if "%BLESSBLD%" equ "4322" (
    set BLESSVER=v1.1
    goto :InstallURT
)

if "%BLESSBLD%" equ "40607" (
    set BLESSVER=v2.0
    goto :InstallURT
)

set ERRMSG=Invalid Build %BLESSBLD% entered
set ERRNUM=1
goto :End


:Installurt
REM ======================================================================
REM Install .NET Framework for X86, IA64 or AMD64
REM ======================================================================

set SRCLOC=%PRODUCTSRV%\URT\%OSLANG%\%BLESSBLD%\%PROCESSOR_ARCHITECTURE%


REM
REM If non-EN version for V2.0+ of URT, it uses MUI install scheme
REM  so you always want to install the EN version of the framework and then a lang pack
REM

if /i "%BLESSVER%" equ "v2.0" (
      set SRCLOC=%PRODUCTSRV%\URT\EN\%BLESSBLD%\%PROCESSOR_ARCHITECTURE%
)

set SRCLOC=%PRODUCTSRV%\URT\%OSLANG%\%BLESSBLD%\%PROCESSOR_ARCHITECTURE%

echo Installing .Net Framework %BLESSVER%.%BLESSBLD% from %SRCLOC% @ %DATE% - %TIME%>>%LOGFILE%
echo Installing .Net Framework %BLESSVER%.%BLESSBLD% from %SRCLOC%

if /i "%PROCESSOR_ARCHITECTURE%" neq "X86" (goto :IA64FrameWork)

if exist %SRCLOC%\dotnetfx.exe (
        copy %SRCLOC%\dotnetfx.exe %TEMP%\dotnetfx.exe /y>NUL 2>&1
) else (
        set ERRMSG=Unable to locate %SRCLOC%\dotnetfx.exe
        set ERRNUM=2
        goto :End
)
call %TEMP%\dotnetfx.exe /q /t:%SYSTEMDRIVE%\net /c:"%SYSTEMDRIVE%\net\install.exe /q"
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=6
    set ERRMSG="%OSLANG% .NET Framework Build %BLESSVER%.%BLESSBLD% installation has failed (returned %ERRORLEVEL%), see %TEMP%\dotnetfx.log for details."
    goto :End
)

set ERRMSG=%OSLANG% .NET Framework %BLESSVER%.%BLESSBLD% installation completed successfully @ %DATE% - %TIME%
set ERRNUM=0
echo Err%ERRNUM% - %ERRMSG%
echo Err%ERRNUM% - %ERRMSG%>>%LOGFILE%
echo.

REM
REM Install lang pack for non-EN version of URT 2.0
REM

if /i "%OSLANG%" neq "EN" (
   if /i "%BLESSVER%" equ "v2.0" (
      set SRCLOC=%PRODUCTSRV%\URT\%OSLANG%\%BLESSBLD%\%PROCESSOR_ARCHITECTURE%
      if exist %SRCLOC%\langpack.exe (
            copy %SRCLOC%\langpack.exe %TEMP%\langpack.exe /y>NUL 2>&1
      ) else (
           set ERRMSG=Unable to locate %SRCLOC%\langpack.exe
           set ERRNUM=2
           goto :End
      )

      REM Give URT a little time to calm down before LangPack applied
      sleep 5
      echo Installing Language pack for %OSLANG% - %SRCLOC% @ %DATE% - %TIME%>>%LOGFILE%
      echo Installing Language pack for %OSLANG% - %SRCLOC%
      call %TEMP%\langpack.exe /q /t:%SYSTEMDRIVE%\net /c:"%SYSTEMDRIVE%\net\install.exe /q"
      if "%ERRORLEVEL%" neq "0" (
          set ERRNUM=6
          set ERRMSG="%OSLANG% .NET Lang pack installation has failed (returned %ERRORLEVEL%), see %TEMP%\langpack.log for details."
          goto :End
      )
      echo Language pack for %OSLANG% successfully installed @ %DATE% - %TIME%>>%LOGFILE%
      echo Language pack for %OSLANG% successfully installed
   )
)

echo Adding .Net Framework %BLESSVER%.%BLESSBLD% to path @ %DATE% - %TIME%>>%LOGFILE%

cscript //nologo %TOOLSRV%\drop\addpath.vbs PRE "%SYSTEMROOT%\Microsoft.NET\Framework\%BLESSVER%.%BLESSBLD%"

goto :SDK

REM ======================================================================
REM Install .NET 64 bit
REM ======================================================================
:IA64FrameWork
if exist %SRCLOC%\netfx64.exe (
    copy %SRCLOC%\netfx64.exe %TEMP%\netfx64.exe /y>NUL 2>&1
) else (
    set ERRMSG=Unable to locate %SRCLOC%\netfx64.exe
    set ERRNUM=2
    goto :End
)
call %TEMP%\netfx64.exe /q /t:%SYSTEMDRIVE%\net /c:"%SYSTEMDRIVE%\net\install.exe /q"
if "%ERRORLEVEL%" neq "0" (
    set ERRNUM=6
    set ERRMSG="%OSLANG% .NET Framework %BLESSVER%.%BLESSBLD% installation has failed (returned %ERRORLEVEL%), see %TEMP%\dotnetfx.log for details."
    goto :End
)
set ERRMSG=%OSLANG% .NET Framework %BLESSVER%.%BLESSBLD% installation completed successfully.
set ERRNUM=0
echo Err%ERRNUM% - %ERRMSG%>>%LOGFILE%
if /i "%PROCESSOR_ARCHITECTURE%" equ "AMD64" (
    cscript //nologo %TOOLSRV%\drop\addpath.vbs PRE "%SYSTEMROOT%\Microsoft.NET\Framework64\%BLESSVER%.%BLESSBLD%"
) else (
    cscript //nologo %TOOLSRV%\drop\addpath.vbs PRE "%SYSTEMROOT%\Microsoft.NET\Framework64\v2.0.30925"

)

REM ======================================================================
REM Install .NET SDK
REM ======================================================================
:SDK
echo.
echo Installing .Net SDK from %SRCLOC%
echo Installing .Net SDK from %SRCLOC% @ %DATE% - %TIME%>>%LOGFILE%

if exist %SRCLOC%\setup.exe (
    copy %SRCLOC%\setup.exe %TEMP%\setup.exe /y>NUL 2>&1
) else (
    set ERRMSG=Unable to locate %SRCLOC%\setup.exe
    set ERRNUM=2
    goto :End
)
call %TEMP%\setup.exe /q /t:%SYSTEMDRIVE%\sdk /c:"%SYSTEMDRIVE%\sdk\install.exe /q"
if "%ERRORLEVEL%" neq "0" (
    if "%ERRORLEVEL%" neq "3010" (
        set ERRNUM=6
        set ERRMSG="%OSLANG% .NET Framework %BLESSVER%.%BLESSBLD% SDK installation has failed (returned %ERRORLEVEL%), see %TEMP%\dotNetFxSDK.log for details."
        goto :End
    )
)

set ERRMSG=%OSLANG% .NET Framework %BLESSVER%.%BLESSBLD% SDK installation completed successfully. @ %DATE% - %TIME%>>%LOGFILE%
set ERRNUM=0
if /i "%PROCESSOR_ARCHITECTURE%" equ "IA64" (
    cscript //nologo %TOOLSRV%\drop\addpath.vbs PRE "%PROGRAMFILES%\Microsoft.NET\SDK\%BLESSVER%\Bin"
) else if /i "%PROCESSOR_ARCHITECTURE%" equ "AMD64" (
    cscript //nologo %TOOLSRV%\drop\addpath.vbs PRE "%PROGRAMFILES%\Microsoft.NET\SDK\%BLESSVER% 64bit\Bin"
) else (
    echo Adding .Net SDK to path @ %DATE% - %TIME%>>%LOGFILE%
    cscript //nologo %TOOLSRV%\drop\addpath.vbs PRE "%PROGRAMFILES%\Microsoft.NET\SDK\%BLESSVER%\Bin"
)
goto :End

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo inst_urt.cmd
echo.
echo Example: inst_urt.cmd [Build] [/?]
echo.
echo If no build specified, defaults to URT build 4322 - version 1.1
echo.
echo Installs the localized version of URT that matches operating system lang
echo.
echo Valid Builds
echo   4322 - URT version 1.1
echo   40607 - URT version 2.0 Beta 1
echo.

set ERRNUM=1
set ERRMSG=Gave user the usage.

:End
popd
echo.
if not defined ERRNUM (set ERRNUM=0)
echo Err%ERRNUM% - %ERRMSG%
echo Err%ERRNUM% - %ERRMSG%>>%LOGFILE%
exit /b %ERRNUM%