@echo off
if defined ECHO (echo %ECHO%)

echo Starting Networking with DHCP
regsvr32 /s netcfgx.dll
factory -minint
netcfg -v -winpe
net start dhcp
net start nla

chres.exe /W:1024 /H:768 /C:8 /F:1

call oc.bat

Echo Set CDROM Drive to x:
diskpart /s CD.txt

Echo .
Echo .
Echo To delete all partitions and create a 4GB Fat32 partition run clean.cmd
Echo clean.cmd will reboot your system.
Echo .
Echo To Setup Safe OS run Setup32.cmd ComputerName or Setup32.cmd /? to get
Echo optional args.  Setup5i32.cmd has also been added for DL360G2 support.
Echo Setup32.cmd will reboot your system.


Echo Disk Config
diskpart /s disklist.txt