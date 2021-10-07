@echo off
if defined echo (echo %echo%)
REM ----------------------------------------------------------------------
REM Name     : Driver_release.cmd
REM
REM Company  : .
REM

REM
REM Summary  : A file for coping nic drivers to your HappyCD source
REM
REM Usage    : Driver_release.cmd {WINPE_ROOT}
REM
REM History  : 1/16/2003 - deangj - created
REM          : 5/16/2003 - deangj - changed the name from Driver_copy.cmd
REM             to Driver_release.cmd
REM          : 9/19/2003 - deangj - Added HP D530 NIC Drivers
REM ----------------------------------------------------------------------
setlocal
pushd .

if "%1" == "" goto Error:
if "%1" == "/?" goto Error:
if "%1" == "?" goto Error:

SET DROP=%1
SET DRIVERSERVER=.\drivers

REM ***************************************************************************
REM Copy Drivers
REM     if you have added a drivers to happyCD please make sure you add it below
REM     So that is get copied.
REM .inf files go in %DROP%\i386\inf
REM .sys files go in %DROP%\i386\system32\drivers
REM everything else goes into %DROP%\i386\System32
REM ***************************************************************************
REM Netgear 31x Drivers
xcopy %DRIVERSERVER%\Netgear\*.inf %DROP%\i386\inf /H /R /Y /I
xcopy %DRIVERSERVER%\Netgear\*.dos %DROP%\i386\system32 /H /R /Y
xcopy %DRIVERSERVER%\Netgear\*.sys %DROP%\i386\system32\drivers /H /R /Y /I

REM 3Com 5103cBTX
xcopy %DRIVERSERVER%\5103cBTX\*.inf %DROP%\i386\inf /H /R /Y /I
xcopy %DRIVERSERVER%\5103cBTX\*.sys %DROP%\i386\system32\drivers /H /R /Y /I
xcopy %DRIVERSERVER%\5103cBTX\3CSOHO10.0 %DROP%\i386\system32 /H /R /Y
xcopy %DRIVERSERVER%\5103cBTX\3csohob.exe %DROP%\i386\system32 /H /R /Y
xcopy %DRIVERSERVER%\5103cBTX\*.cat %DROP%\i386\system32 /H /R /Y
xcopy %DRIVERSERVER%\5103cBTX\PARTNO %DROP%\i386\system32 /H /R /Y

REM HP D530 Network Drivers
xcopy %DRIVERSERVER%\D530\*.inf %DROP%\i386\inf /H /R /Y /I
xcopy %DRIVERSERVER%\D530\*.sys %DROP%\i386\system32\drivers /H /R /Y /I
xcopy %DRIVERSERVER%\D530\*.cat %DROP%\i386\system32 /H /R /Y

REM other Drivers that are needed
xcopy %DRIVERSERVER%\Other\*.inf %DROP%\i386\inf /H /R /Y /I
xcopy %DRIVERSERVER%\Other\*.sys %DROP%\i386\system32\drivers /H /R /Y /I
xcopy %DRIVERSERVER%\D530\*.cat %DROP%\i386\system32 /H /R /Y

goto :End
REM ***************************************************************************
REM Ussage
REM ***************************************************************************
:ERROR
echo Usage:
echo .
echo Driver_copy.cmd {WINPE_ROOT}
echo Example Driver_copy.cmd d:\winpe\root
echo .
echo WINPE_ROOT = The place where your happyCD root is
goto :End

:End
popd
endlocal