@echo OFF
if defined ECHO (echo %ECHO%)
setlocal
REM ======================================================================
REM Module  : offinst.cmd
REM
REM Abstract: Installs Office, or just Outlook or Access
REM
REM Creator : Todd Ellis (ToddEll)
REM
REM History : (07/23/02) Todd Ellis (Toddell) - Started Initial coding
REM           (04/01/03) Todd Ellis (Toddell) - Fix for 2659 (usage always displays)
REM                                           - Conform to coding standards
REM                                           - Changed usage to reflect USA for valid language
REM                                           - Changes usage to support /NOREBOOT option
REM           (08/12/03) Dean Gjedde (deangj) - Changed to language standard, made work with french bug # 3074
REM           (11/21/03) Dean Gjedde (deangj) - Added support for Office2k3, Fixed DCR 3231
REM           (01/09/04) Dean Gjedde (deangj) - Changed the path echoed to the user, trival change
REM           (07/30/04) John Heaton (Jheat)  - Updated to leverage OSLANG variable

REM Copyright . 2004

REM ======================================================================
REM Check for required parameters
REM ======================================================================
if /i "%1" equ "" (goto :Usage)
if /i "%2" equ "" (goto :Usage)
if /i "%1" equ "/?" (goto :Usage)
if not defined LOC (
    if defined OSLANG (
        set LOC=%OSLANG%
    ) else (
        set LOC=EN
    )
)

REM ======================================================================
REM Set some required stuff.
REM ======================================================================
set PRODTYPE=%1
set PLATFORM=%2
set FLAGS=/Q
set LOG=/lv* %SYSTEMDRIVE%\logs\Install_%PRODTYPE%%PLATFORM%.log
if /i "%PLATFORM%" equ "XP" (
    set SRCLOC=\\smx.net\products\office\OFFICEXP
) else if /i "%PLATFORM%" equ "2K3" (
    set SRCLOC=\\smx.net\products\office\OFFICE2K3
) else (
    set SRCLOC=\\smx.net\products\office\OFFICE2K
)
if /i "%LOC%" equ "FR" (
    set SETUPFILE=INSTALL.EXE
) else (
    set SETUPFILE=SETUP.EXE
)
if /i "%PLATFORM%" equ "2K3" (
    set SETUPFILE=SETUP.EXE
    if /i "%LOC%" equ "FR" (
        set SETUPFILE=SETUPPRO.EXE
    )
)

REM ======================================================================
REM Install based on prod type
REM ======================================================================
if /i "%PRODTYPE%" equ "Outlook" goto :InstallComponent
if /i "%PRODTYPE%" equ "Access" goto :InstallComponent
if /i "%PRODTYPE%" equ "Office" goto :InstallOffice
goto :Usage

:InstallComponent
echo Preparing to install %PRODTYPE% %PLATFORM%...
set TRANSFORM=TRANSFORMS=%SRCLOC%\%PRODTYPE%%PLATFORM%.MST
if /i "%PLATFORM%" equ "XP" (
    if /i "%LOC%" equ "FR" (
        set TRANSFORM=TRANSFORMS=%SRCLOC%\%PRODTYPE%%PLATFORM%FR.MST
    )
    if /i "%LOC%" equ "DE" (
        set TRANSFORM=TRANSFORMS=%SRCLOC%\%PRODTYPE%%PLATFORM%FR.MST
    )
)

if /i "%PLATFORM%" equ "2K3" (
    echo Installing %PRODTYPE% %PLATFORM% from %SRCLOC%\%LOC%\Office2003
    start /wait %SRCLOC%\%LOC%\Office2003\%SETUPFILE% %TRANSFORM% %LOG% %FLAGS%
) else (
    echo Installing %PRODTYPE% %PLATFORM% from %SRCLOC%\CD1
    start /wait %SRCLOC%\%LOC%\CD1\%SETUPFILE% %TRANSFORM% %LOG% %FLAGS%
)
if /i "%PLATFORM%" equ "2K" (sleep.exe 150)
goto :InstallDone

:InstallOffice
echo Preparing to install %PRODTYPE% %PLATFORM%...
if /i "%PLATFORM%" equ "2K3" (
    echo Installing %PRODTYPE% %PLATFORM% from %SRCLOC%\%LOC%\Office2003
    start /wait %SRCLOC%\%LOC%\Office2003\%SETUPFILE% %PID% %LOG% %FLAGS%
) else (
    echo Installing %PRODTYPE% %PLATFORM% from %SRCLOC%\CD1
    start /wait %SRCLOC%\%LOC%\cd1\%SETUPFILE% %LOG% %FLAGS%
)
if /i "%PLATFORM%" equ "2K" (SLEEP.EXE 360)
goto :InstallDone

:InstallDone
if /i "%3" neq "/NOREBOOT" (shutdown -r -f -t 3)
goto :EOF

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo offinst.cmd PRODTYPE PLATFORM [/NOREBOOOT]
echo.
echo Valid Prodtype:
echo "Office, Outlook, or Access"
echo.
echo Valid Platform:
echo "2K3, XP, or 2K"
echo.
echo Valid Languages:
echo "EN, FR, DE, JA"
echo To set the Language just "set LOC=Vaild language"
echo.
echo Optional Switches:
echo "/NOREBOOT"
echo To install without rebooting specify this as the last perameter.
goto :EOF

endlocal