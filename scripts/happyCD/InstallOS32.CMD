@echo off
if defined echo (echo %echo%)
pushd

REM ***************************************************************************
REM Check for required parameter
REM ***************************************************************************
if "%1" == "/?" GOTO Error
if "%1" == "?" GOTO Error

REM ***********
REM User Config
REM ***********
set OSROOT=%SYSTEMDRIVE%\OSInstall
set OS=w2k
set VER=pro
set DRIVE=c:

REM ***************************************************************************
REM Runs setup
REM ***************************************************************************
if exist %OSROOT%\%OS%\%VER%\i386\winnt32.exe (
REM x:\NT5\I386\winnt32.exe /s:x:\NT5\I386 /unattend:c:\delete\newsafeos.txt /tempdrive:c: /noreboot
	%OSROOT%\%OS%\%VER%\i386\winnt32.exe /tempdrive:%DRIVE% /noreboot
)

goto :EndAndReboot

REM ***************************************************************************
REM Ussage
REM ***************************************************************************
:Error
echo Usage:
echo .
echo InstallOS32.cmd {OS} {VER} {DRIVE}
echo Example InstallOS32.cmd office-brick1
echo .
echo Optional:
echo OS = OS to install W2K, W2K3, XP - W2K is default
echo VER = Version to install SRV, ENT, PRO, Home - PRO is default
echo DRIVE = Drive you want the OS installed on - C: is default
goto :End

:EndAndReboot
popd
REM exit

:End
popd