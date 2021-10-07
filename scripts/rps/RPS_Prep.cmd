@if not defined echo echo off
REM ======================================================================
REM
REM NAME: RPS_Prep.cmd
REM
REM AUTHOR: Dean Gjedde
REM DATE  : 1/6/2005
REM
REM COMMENT: Set all needed RPS settings
REM
REM ======================================================================

REM ======================================================================
REM Set CScript.exe as default
REM ======================================================================
CScript.exe //H:CScript

REM ======================================================================
REM Disable System Restore
REM ======================================================================
REG ADD "HKLM\SYSTEM\CurrentControlSet\Services\sr" /v Start /t REG_DWORD /d 4 /f
REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore" /v DisableSR /t REG_DWORD /d 1 /f

REM ======================================================================
REM Disable Perf unfriendly services
REM ======================================================================
REM sc.exe config HELPSVC start= disabled
REM sc.exe config WZCSVC start= disabled
REM sc.exe config WUAUSERV start= disabled
REM sc.exe config BITS start= disabled
REM sc.exe config CISVC start= disabled
REM sc.exe config WSCSVC start= disabled

REM ======================================================================
REM Disable Remote Assistance
REM ======================================================================
REG ADD "HKLM\SYSTEM\CurrentControlSet\Control\Terminal Server" /v fAllowToGetHelp  /t REG_DWORD /d 0 /f

REM ======================================================================
REM Disable Prefetch
REM ======================================================================
REG ADD "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters" /v EnablePrefetcher  /t REG_DWORD /d 0 /f

REM ======================================================================
REM Delete prefetch files
REM ======================================================================
if exist %WINDIR\prefetch (
    del %WINDIR\prefetch\*.* /F /Q
)

REM ======================================================================
REM Enable Security Tab when not joined to the domain
REM ======================================================================
REG ADD "HKLM\SYSTEM\CurrentControlSet\Control\Lsa" /v forceguest  /t REG_DWORD /d 0 /f

REM ======================================================================
REM Delete DLL Cashe
REM ======================================================================
if exist  %WINDIR%\system32\dllcashe (
    del %WINDIR%\system32\dllcashe\*.* /F /Q
)

REM ======================================================================
REM Disable the XP Tour
REM ======================================================================
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Applets\Tour" /v RunCount  /t REG_DWORD /d 0 /f

REM ======================================================================
REM Change background color to black
REM ======================================================================
REG ADD "HKU\.DEFAULT\Control Panel\Colors" /v Background  /t REG_SZ /d "0 0 0" /f

REM ======================================================================
REM Make mouse DSView friendly for all users
REM ======================================================================
REM REG ADD "HKU\.DEFAULT\Control Panel\Mouse" /v MouseSpeed  /t REG_DWORD /d 0 /f
REM REG ADD "HKU\.DEFAULT\Control Panel\Mouse" /v MouseThreshold1  /t REG_DWORD /d 0 /f
REM REG ADD "HKU\.DEFAULT\Control Panel\Mouse" /v MouseThreshold2  /t REG_DWORD /d 0 /f

REM ======================================================================
REM Disable Screensaver
REM ======================================================================
REG ADD "HKU\.DEFAULT\Control Panel\Desktop" /v SCRNSAVE.EXE  /t REG_SZ /d "" /f

REM ======================================================================
REM Disable Desktop ICON cleanup
REM     Note: This is a test
REM ======================================================================
REG ADD "HKU\.DEFAULT\Software\Microsoft\Windows\CurrentVersion\Explorer\Desktop\CleanupWizard" /v NoRun  /t REG_DWORD /d 1 /f

REM ======================================================================
REM Enable remote desktop
REM ======================================================================
REG ADD "HKLM\SYSTEM\CurrentControlSet\Control\Terminal Server" /v fDenyTSConnection /t REG_DWORD /d 0 /f

REM ======================================================================
REM Disable WPA warning balloon
REM ======================================================================
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\RRWPA" /v RemoveReminder /t REG_DWORD /d 1 /f

REM ======================================================================
REM Disable Hibernate
REM ======================================================================
powercfg.exe /HIBERNATE OFF

REM ======================================================================
REM Enable Auto loggon
REM ======================================================================
REM C:\misc\alogon.exe -xr
REM c:\misc\alogon.exe -s "vb_1193" "administrator" "%COMPUTERNAME%"
REM REG DELETE "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" /v AutoLogonCount /f
REM REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultUserName  /t REG_SZ /d "administrator" /f
REM REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultPassword /t REG_SZ /d "vb_1193" /f

:End
REM restart the computer
shutdown.exe -r -f -t 10
exit