@echo off
if defined ECHO (echo %ECHO%)
REM ----------------------------------------------------------------------
REM Name     : happycd_release.cmd
REM
REM Company  : .
REM

REM
REM Summary  : A file for coping HappyCD files to your HappyCD source
REM
REM Usage    : happyCD_release.cmd DROP HAPPYDIR
REM
REM History  : 1/16/2003 myocom - created
REM History  : 5/18/2003 - brentf - modified for DL360G2(5i) support
REM		(added xcopy of mungeini5i.pl, winpe-5i.bmp and setup5i32.cmd)
REM ----------------------------------------------------------------------
SETLOCAL
PUSHD

IF "%1" == "" GOTO Error:
If "%1" == "/?" GOTO Error:
If "%1" == "?" GOTO Error:
If "%2" == "" GOTO Error:

SET DROP=%1
SET HAPPYDIR=%2

REM ***************************************************************************
REM Copy happyCD files.
REM     If you have added a file to happyCD please make sure you add it below
REM     So that is get copied
REM ***************************************************************************

REM ***************************************************************************
REM Files to be copied to %Drop%\i386\System32 dir on HappyCD Source
REM ***************************************************************************
SET SYS32_DIR=%DROP%\i386\System32

XCOPY %HAPPYDIR%\CDSCom.dll %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\CDSGet.vbs %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\chres.exe %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\clean.cmd %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\DISKLIST.txt %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\diskpart.txt %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\mungeini.pl %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\s.cmd %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\Setup32.cmd %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\startnet.cmd %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\happycd-5i\winpe.bmp %SYS32_DIR% /H /R /Y
XCOPY %HAPPYDIR%\CD.txt %SYS32_DIR% /H /R /Y 
XCOPY %HAPPYDIR%\mungeini5i.pl %SYS32_DIR% /H /R /Y 
XCOPY %HAPPYDIR%\setup5i32.cmd %SYS32_DIR% /H /R /Y 

goto :END
REM ***************************************************************************
REM Ussage
REM ***************************************************************************
:ERROR
ECHO Usage:
ECHO .
ECHO happyCD_release.cmd DROP HAPPYDIR
ECHO Example Setup32.cmd d:\winpe\root d:\happycd
ECHO .
ECHO DROP = The winpe root directory
ECHO HAPPYDIR = Place where happyCD files live

:END
POPD
ENDLOCAL