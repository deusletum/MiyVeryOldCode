@echo off
if defined echo (echo %echo%)
REM ----------------------------------------------------------------------
REM Name     : buildWinPE.cmd
REM
REM Company  : .
REM

REM
REM Summary  : buildWinPE.cmd will build a winpe iso image with the current HappyCD files
REM                 included
REM
REM Usage    : buildWinPE.cmd WINPE_ROOT WINPE_TOOLS ISO_PATH HAPPYCD_FILES
REM
REM History  : 1/16/2003 - deangj - created
REM History  : 5/18/2003 - brentf - modified for DL360G2(5i) support
REM 		(calls happycd_release-5i.cmd and names .iso file, Happycd-5i.iso)
REM ----------------------------------------------------------------------
pushd
setlocal

REM ***************************************************************************
REM Check for required parameter
REM ***************************************************************************
if "%1" == "" goto Error:
if "%1" == "/?" goto Error:
if "%1" == "?" goto Error:
if "%2" == "" goto Error:
if "%3" == "" goto Error:
if "%4" == "" goto Error:

set XP_PATH="c:\xpsp2"
set WINPE_TOOLSPATH=c:\bin\Funsfuff\opk\winpe
set WINPE_SERVER=c:\winpe_base
set WINPE_ROOT=%1
set WINPE_TOOLS=%2
set ISO_PATH=%3
set HAPPYCD_FILES=%4

REM ***************************************************************************
REM Delete Winpe_root and winpe_tools dir if exist and then create them
REM ***************************************************************************
if exist %WINPE_ROOT% (rd %WINPE_ROOT% /S /Q)
md %WINPE_ROOT%
if exist %WINPE_TOOLS% (rd %WINPE_TOOLS% /S /Q)
md %WINPE_TOOLS%
if not exist %ISO_PATH% (md %ISO_PATH%)

REM ***************************************************************************
REM Copy winpe root and winpe tools
REM ***************************************************************************
xcopy %WINPE_SERVER% %WINPE_ROOT% /e /H /R /Y
xcopy %WINPE_TOOLSPATH% %WINPE_TOOLS% /e /H /R /Y

REM ***************************************************************************
REM Add WSH, ADO, and HTA support to winpe
REM ***************************************************************************
call %HAPPYCD_FILES%\buildoptionalcomponents.vbs -S:%XP_PATH% /D:%WINPE_ROOT% /ADO /WSH /Q

REM ***************************************************************************
REM Copy Perl to %Winpe_root%\perl
REM ***************************************************************************
REM if not exist %WINPE_ROOT% (md %WINPE_ROOT%\Perl)
REM xcopy %PERL_PATH% %WINPE_ROOT%\Perl /e /H /R /Y /I

REM ***************************************************************************
REM Copy HappyCD Files
REM ***************************************************************************
call %HAPPYCD_FILES%\happycd_release-5i.cmd %WINPE_ROOT% %HAPPYCD_FILES%

REM ***************************************************************************
REM Copy NIC Drivers
REM ***************************************************************************
call %HAPPYCD_FILES%\Driver_release.cmd %WINPE_ROOT%

REM ***************************************************************************
REM Create the ISO image
REM ***************************************************************************
REM call %WINPE_TOOLS%\oscdimg.exe -n -b%WINPE_TOOLS%\etfsboot.com %WINPE_ROOT% %ISO_PATH%\HappyCD-5i.iso
Echo %WINPE_TOOLS%\oscdimg.exe -n -b%WINPE_TOOLS%\etfsboot.com %WINPE_ROOT% %ISO_PATH%\HappyCD-5i.iso

goto :END

REM ***************************************************************************
REM Ussage
REM ***************************************************************************
:Error
echo buildWinPE.cmd will build a winpe iso image with the current HappyCD files included
echo .
echo Usage:
echo .
echo buildWinPE.cmd WINPE_ROOT WINPE_TOOLS ISO_PATH HAPPYCD_FILES
echo Example setup32.cmd d:\winpe\root d:\winpe\tools d:\winpe\iso
echo .
echo WINPE_ROOT = The place you want winpe to be copied to
echo WINPE_TOOLS = The place you want winpe tools to be copied to
echo ISO_PATH = The place you want winpe iso image copied to to be copied to
echo HAPPYCD_FILES = The place where the HappyCD files are located
echo This might be a share (\\myserver\myshare) or in you source depot

:END
endlocal
popd
