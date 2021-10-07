@echo OFF
if defined ECHO (echo %ECHO%)
setlocal
REM ***************************************************************************
REM Module  : acinst.cmd
REM
REM Abstract: This files is intended to install all flavors of AC, ACT and Patches.
REM           Run with no parameters to get parameter list.
REM
REM Creator : John Heaton
REM
REM History : (06/21/01) John Heaton (Jheat) Initial coding
REM           (09/18/01) John Heaton (Jheat) Added support for no MSDE
REM                                          To use just set MSDE=NO
REM              (01/16/02) James Shepard (Jshep) Added the Patch install
REM              (05/06/02) John Heaton (Jheat) Cleaned up and added new dir to copy
REM              (07/01/02) James Shepard (Jshep) Added SMX* install
REM              (07/19/02) James Shepard (Jshep) Added CONFIGGROUP Switch to SMX agent installs.
REM           (09/23/02) James Shepard (Jshep) Added Tracing for SMX (again)
REM           (09/24/02) James Shepard (Jshep) Rolled back Tracing.
REM           (09/24/02) James Shepard (Jshep) Added a call to SMX_trace.cmd
REM           (10/16/02) James Shepard (Jshep) New path for SMX.
REM           (01/17/02) Jennifer LaVigne(Jennla) Infra Bug 2423
REM           (02/19/03) John Heaton (Jheat) Added ACT Support Coding Standard Comply
REM           (03/21/03) Glenn LaVigne (glennlav) Restored support of AC Client-only installs
REM
REM Legend: acinst.cmd PRODNAME PRODBUILD PRODTYPE
REM
REM Copyright . 2004
REM ***************************************************************************

REM ***************************************************************************
REM Let's take a look at the parameters you entered
REM ***************************************************************************

REM This should be product name
if /i "%1" equ "" (goto :Usage)
REM This should be build number
if /i "%2" equ "" (goto :Usage)
REM This should be type (fre, fre.vs, chk, patch, etc)
if /i "%3" equ "" (goto :Usage)

REM ***************************************************************************
REM set some variables to track things easier.
REM ***************************************************************************
set PRODTYPE=%1
set TYPE=%3
set PROCESS_ARCHITECTURE=%PROCESSOR_ARCHITECTURE%
set DROPSRV=\\smx.net\tools\drop

REM ***************************************************************************
REM Check for Build Number, MSDE, Client Info and LOC
REM ***************************************************************************
if /i "%2" neq "LATEST" (set VERSION=%2)
if not defined LOC (
    set LOC=EN
    ) else (
        if /i "%LOC%" equ "JPN" (set LOC=JA)
        if /i "%LOC%" equ "USA" (set LOC=EN)
)
if /i "%MSDE%" equ "NO" (set MSDE=ADDLOCAL=Server,Client)
if /i "%CLIENT%" equ "YES" (set MSDE=ADDLOCAL=Client)

REM ***************************************************************************
REM Step one... setting %4 if it exists
REM ***************************************************************************
if "%4" equ "" (goto :ProdLoc)
if /i "%4" equ "/NODEBUG" (set DEBUG=NO)
if /i "%4" equ "/i" (set INTERACTIVE=YES)

REM ***************************************************************************
REM Step two... setting %5 if it exists
REM ***************************************************************************
if "%5" equ "" (goto :ProdLoc)
if /i "%5" equ "/NODEBUG" (set DEBUG=NO)
if /i "%5" equ "/i" (set INTERACTIVE=YES)

REM ***************************************************************************
REM Lets set the product location.  Based on what you told me.
REM ***************************************************************************
:ProdLoc
if /i "%PRODTYPE%" equ "AC"       (set PRODLOC=\\smx.net\BUILDS\APPSRV)
if /i "%PRODTYPE%" equ "AC10SP1"  (set PRODLOC=\\smx.net\BUILDS\AC10SP1)
if /i "%PRODTYPE%" equ "AC10SP2"  (set PRODLOC=\\smx.net\builds\AC10SP2)
if /i "%PRODTYPE%" equ "ACT"      (set PRODLOC=\\smx.net\BUILDS\ACT)
if not defined PRODLOC (
    echo You entered an invalid product
    goto :Usage
)
if /i "%INTERACTIVE%" neq "YES" (
    set INTERACTIVE=/Q
    ) else (
        set INTERACTIVE=
)
REM ***************************************************************************
REM Let's figure out the build number, unless you told me, then that's easy.
REM ***************************************************************************
if /i "%2" equ "LATEST" (
    if not exist %PRODLOC%\LATESTGOOD.BAT (
    echo Unable to locate %PRODLOC%\LATESTGOOD.BAT
    goto :EOF
    )
    call %PRODLOC%\LATESTGOOD.BAT
)

REM ***************************************************************************
REM Let's look for an old version and get rid of it
REM Just in case this isn't a clean install.
REM ***************************************************************************
if /i "%PRODTYPE%" equ "ACT" (goto :Uninstall_ACT)
if /i "%TYPE%" equ "PATCH" (
    goto :Debugger
    ) else (goto :UninstallAC
)

:UninstallAC
if not exist "%ProgramFiles%\Microsoft Application Center" (goto :INST_PREP)
echo Uninstalling existing version of %1
title Uninstalling existing version of %1
set LOGFILE=%SYSTEMDRIVE%\%1_uninstall.log
call msiexec.exe /x {20F95200-47D6-4CAC-92FF-5F6B29C78F88} /q /lv* %LOGFILE%
if exist "%ProgramFiles%\Microsoft Application Center" (
    rd "%ProgramFiles%\Microsoft Application Center" /Q/S
)

goto :INST_PREP

:Uninstall_ACT
if not exist "%ProgramFiles%\Microsoft ACT" (goto :INST_PREP)
echo Uninstalling existing version of ACT
title Uninstalling existing version of ACT
set LOGFILE=%SYSTEMDRIVE%\act_uninstall.log
call msiexec.exe /x {408C304F-6D69-440E-A977-7CB473989BC9} /q /lv* %LOGFILE% ALLSUSERS=1
if exist "%ProgramFiles%\Microsoft ACT" (
    rd "%ProgramFiles%\Microsoft ACT" /Q/S
)

REM ***************************************************************************
REM set the instsuffix for the product (works for everything but patch)
REM ***************************************************************************
:INST_PREP
set LOGFILE=%SYSTEMDRIVE%\%PRODTYPE%_setup.log
if /i "%PRODTYPE%" equ "ACT" (
    set INSTSUFFIX=%LOC%\%VERSION%\X86\%TYPE%\CDIMAGE\Microsoft Application Center Test.MSI
)
if not defined INSTSUFFIX (
    set INSTSUFFIX=%LOC%\%VERSION%\X86%TYPE%\CDIMAGE\Microsoft Application Center 2000.MSI
)

REM ***************************************************************************
REM Have to figure out the PID, because Chris hates me.
REM ***************************************************************************
if /i "%PRODTYPE%" equ "AC"      (set PID=PIDKEY=H6TWQTQQM8HXJYGD69F7R84VM)
if /i "%PRODTYPE%" equ "AC10SP1" (set PID=PIDKEY=H6TWQTQQM8HXJYGD69F7R84VM)
if /i "%PRODTYPE%" equ "AC10SP2" (set PID=PIDKEY=H6TWQTQQM8HXJYGD69F7R84VM)

REM ***************************************************************************
REM Check to see if the MSI exists. (Changed the go to Fail. Was :End)
REM ***************************************************************************
:MSI_CHK
if not exist "%PRODLOC%\%INSTSUFFIX%" (
    echo "Missing MSI or misspelled product variable (%PRODTYPE%) (%2) or (%TYPE%)."
    goto :EOF
)

REM ***************************************************************************
REM Do you want me to launch the debugger?
REM ***************************************************************************
:Debugger
if /i "%DEBUG%" neq "NO" (call \\smx.net\tools\bricktools\x86\debug.cmd %PRODTYPE% 3)

REM ***************************************************************************
REM if the Patch is installed, we need to set the INSTSUFFIX for it.
REM ***************************************************************************
if /i "%TYPE%" equ "PATCH" (set INSTSUFFIX=%LOC%\%VERSION%\X86%TYPE%)

REM ***************************************************************************
REM Well lets install it then.
REM ***************************************************************************
if not exist %SYSTEMROOT%\ACSTOOLS (MD %SYSTEMROOT%\ACSTOOLS)
if /i "%TYPE%" neq "PATCH" (goto :MAIN_INSTALL)

REM ***************************************************************************
REM setup a log file that can track the installation of the patch.
REM ***************************************************************************
set LOGFILE=%SYSTEMDRIVE%\%PRODTYPE%_setup.log

REM ***************************************************************************
REM Set ACMSI string to the cdimage path.
REM ***************************************************************************
set ACMSI=/Path:\\smx.net\builds\APPSRV\en\RTM\x86fre\cdimage

REM ***************************************************************************
REM Check to see if the path is valid.
REM ***************************************************************************
if not exist "%PRODLOC%\%INSTSUFFIX%" (echo Missing exe file, please check path && goto :EOF)

REM ***************************************************************************
REM Installing the Patch.
REM ***************************************************************************
title Installing %PRODTYPE% Patch from %PRODLOC%\%INSTSUFFIX%
echo Installing %PRODTYPE% Patch from %PRODLOC%\%INSTSUFFIX%
if /i "%PRODTYPE%" equ "AC10SP1" (
    call %PRODLOC%\%INSTSUFFIX%\ACSP1setUP.EXE /UNATTENDED %ACMSI%
    goto :sleep
)
if /i "%PRODTYPE%" equ "AC10SP2" (
    call %PRODLOC%\%INSTSUFFIX%\AC2000_SP2.EXE /Q /C:"ACSP2.exe /UNATTENDED" /R:IS
    goto :ErrorCheck
)

REM ***************************************************************************
REM Added because of infra bug 2320 for the ac10sp1 patch. This allows the Symbols to install.
REM ***************************************************************************
:sleep
sleep 120
goto :ErrorCheck

REM ***************************************************************************
REM Installing ACT, AC, AC10SP1
REM ***************************************************************************
:MAIN_INSTALL
title "Installing %PRODTYPE% from %PRODLOC%\%INSTSUFFIX%"
echo "Installing %PRODTYPE% from %PRODLOC%\%INSTSUFFIX%"
call msiexec /i "%PRODLOC%\%INSTSUFFIX%" /lv* %LOGFILE% %INTERACTIVE% %MSDE% %PID%
:ErrorCheck
if "%ERRORLEVEL%" equ "1641" (goto :Symbols)
if "%ERRORLEVEL%" neq "0" (goto :Fail)

REM ***************************************************************************
REM Installing Symbols
REM ***************************************************************************
:Symbols
echo Copying Symbols ....
title Copying Symbols ....
call \\smxfiles\scratch\toddell\ac\SymbolsPLEASE.CMD %PRODTYPE% %VERSION% %TYPE% ALL

goto :End

REM ***************************************************************************
REM The fail condintion.
REM ***************************************************************************
:Fail
echo.
echo setup failed to install, please see %LOGFILE% for more details.
echo Returned error code is %ERRORLEVEL%
goto :EOF

REM ***************************************************************************
REM So here is how you use it.
REM ***************************************************************************
:Usage
echo.
echo Usage:
echo.
echo acinst.cmd PRODTYPE PRODBUILD TYPE
echo.
echo Example: acinst.cmd AC 0440.3 FRE
echo Example: acinst.cmd AC10SP1 0440.612 FRE
echo Example: acinst.cmd AC10SP2 LATEST FRE
echo Example: acinst.cmd ACT LATEST FRE.VS
echo Example: acinst.cmd AC10SP1 0440.612 PATCH
echo Example: acinst.cmd AC10SP2 LATEST PATCH
echo.
echo Valid Prodtypes:
echo     "AC, AC10SP1, AC10SP2, ACT"
echo.
echo Valid Prodbuild:
echo     "Build number, or Latest"
echo.
echo Valid Type:
echo     FRE, CHK,
echo     FRE.VS, FRE.ENT, CHK.VS, CHK.ENT for ACT Only."
echo.
echo Optional Parameters:
echo     "/i" Interactive
echo     "/Nodebug" Doesn't launch debugger or start tracing on SMX
echo
echo     "set MSDE=NO" This doesn't install MSDE
echo     To set the Language to Japanese "set Loc=ja" before running ACINST.CMD
echo.
echo.
goto :EOF

REM ***************************************************************************
REM Exiting the program
REM ***************************************************************************
:End
title Installation completed successfully
echo Installation completed successfully
endlocal