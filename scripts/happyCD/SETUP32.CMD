@echo off
if defined echo (echo %echo%)
pushd
REM ----------------------------------------------------------------------
REM Name     : setup32.cmd
REM
REM Company  : .
REM
REM
REM Summary  : This script will setup SafeOS on C:
REM
REM Usage    : see :usage
REM
REM History  : 12/11/2002 - Dean Gjedde - created
REM History  : 5/18/2003 - Brent Forman - modified perl file and unattend file for DL360G2(5i) support
REM            (also modified usage section)
REM ----------------------------------------------------------------------

REM ***************************************************************************
REM Check for required parameter
REM ***************************************************************************
if "%1" == "" GOTO Error
if "%1" == "/?" GOTO Error
if "%1" == "?" GOTO Error
set sname=%1>nul

REM ***********
REM User Config
REM ***********
set domain=SMX
set server=\\acstools
set user=asttest
set share=happydisk


REM ***********
REM Format Disk
REM ***********
echo Y | format c: /fs:fat32 /q

REM ***************************************************************************
REM Register CDSCom.dll and get asttest password from CDS
REM ***************************************************************************
md c:\delete>NUL
regsvr32 %SYSTEMDRIVE%\i386\system32\CDSCom.dll /S
CScript.exe //E:VBscript CDSGet.vbs
call c:\delete\password.bat
set password=%AsttestPassword%

REM ***************************************************************************
REM Copy files down.
REM ***************************************************************************
net use x: %SERVER%\%SHARE% /u:%DOMAIN%\%USER% %PASSWORD%>NUL
copy X:\SAFEOS\*.* c:\delete>NUL
copy x:\safeos\autopart\*.* c:\delete>NUL

REM ***************************************************************************
REM Saves some environment variables
REM ***************************************************************************
echo SET DOMAIN=%DOMAIN%>C:\DELETE\DOMAIN.BAT
echo SET USER=%USER%>>C:\DELETE\DOMAIN.BAT
echo SET PASSWORD=%PASSWORD%>>C:\DELETE\DOMAIN.BAT
if "%2" == "" goto domain
echo SET %2=YES>>C:\DELETE\DOMAIN.BAT
if "%3" == "" goto domain
echo SET %3=YES>>C:\DELETE\DOMAIN.BAT

REM ***************************************************************************
REM Adds Domain Info: Domain Name, Password & Account to unattend file
REM ***************************************************************************
:domain
perl.exe -w MUNGEINI.PL c:\delete\setup.scr

REM ***************************************************************************
REM Runs setup
REM ***************************************************************************
x:\NT5\I386\winnt32.exe /s:x:\NT5\I386 /unattend:c:\delete\newsafeos.txt /tempdrive:c: /noreboot

goto :EndAndReboot

REM ***************************************************************************
REM Ussage
REM ***************************************************************************
:Error
echo Usage:
echo .
echo Setup5i32.cmd {machname_name} {optional1} {optional2}
echo Example Setup5i32.cmd office-brick1
echo .
echo Optional:
echo AUTOPART = Automaticly create D and E Drives
echo OASIS-W2K = Automaticly Launch OASIS Install on E Drive
echo OASIS-NET = Automaticly Launch OASIS Install on E Drive
goto :End

:EndAndReboot
popd
exit

:End
popd