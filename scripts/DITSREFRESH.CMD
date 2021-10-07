@echo off
if "%echo%" neq "" echo %echo%
setlocal
REM ======================================================================
REM Module  : DITSREFRESH.CMD
REM
REM Abstract: Refreshes a machine to a specified OS
REM
REM Creator : John Heaton (Jheat)
REM
REM History : (10/02/02) John Heaton (Jheat) - Started Initial coding
REM
REM Copyright . 2002

REM ======================================================================
REM Check for required parameter
REM ======================================================================
if "%1" equ "" (goto :Usage)
if "%2" equ "" (goto :Usage)
if "%3" equ "" (goto :Usage)
if "%4" equ "" (goto :Usage)
if "%5" equ "" (goto :Usage)

REM ======================================================================
REM Set Variables for what we are going to do
REM ======================================================================
set OS=%1
set OSB=%2
set OSV=%3
set OSL=%4 
set MACHINE=%5
set SCRIPTPATH=cscript //nologo \\acstools\toolbox\DITSUtils\ACJobs.wsf

REM ======================================================================
REM Set DITS Specific Variables
REM ======================================================================
set SERVER=SMDITS
set TITLE="Refreshing %MACHINE% to %OS% %OSB% FRE.%OSV% %OSL% %6 %7"

REM ======================================================================
REM Launch Refresh
REM ======================================================================
%SCRIPTPATH% /S %SERVER% /U %USERNAME% /J %TITLE% /M " started %DATE%" /E 95 /OS %OS% /OSV %OSV% /OSB %OSB% %6 %7 /OSL %OSL% %MACHINE%

goto :End
REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo Usage:
echo.
echo You must specifcy the following parameters:
echo OS - This is the OS that you want install.
echo OSV - Version of the OS.
echo OSB - Build of the OS you wish to install.
echo OSL - Language of the OS to install.
echo Machine - Machine you want to refresh.
echo Valid OS:
echo         "W2K, XP, NET"
echo.
echo Valid OS Build:
echo         "latest, latest.tst, latest.idw, latest.ids, latest.sp2, latest.sp3, or number "
echo.
echo Valid OS Version:
echo         "PRO, SRV, DTC, ENT, ADS"
echo.
echo Valid OS Language:
echo         "ara,br,chh,chs,cht,cs,da,el,es,fr,fe,fi,ger,heb,hu,it,jpn,kor,mir,nl,no,pl,psu,pt,ru,sv,tr,usa"
echo.
echo.
echo Optional Parameter:
echo      /osd %domain%, if not defined specified machines will refresh to previous domain.
echo.
echo.
echo Example: DitsRefresh.CMD W2K latest.sp3 ENT USA Jheat-test1e 
echo Example: DitsRefresh.CMD W2K latest.sp3 ENT USA Jheat-test1e /osd smx
:End
endlocal