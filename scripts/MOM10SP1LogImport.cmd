@ECHO OFF
@IF "%ECHO%" NEQ "" ECHO %ECHO%
setlocal
REM ======================================================================
REM Module  : MOM10SP1LogImport.CMD
REM
REM Abstract: Imports log files for MOM10SP1
REM
REM Creator : Dean Gjedde (deangj)
REM
REM History : (10/22/02) Dean Gjedde (deangj) - Created
REM
REM Copyright . 2002
REM
REM Credit : Most code leveaged for John Heaton (jheat)
REM
REM ======================================================================

REM ======================================================================
REM Check for required parameters
REM ======================================================================
if /i "%1" equ "" (goto Usage:)
if /i "%2" equ "" (goto Usage:)
if /i "%3" equ "" (goto Usage:)

SET SRCPATH=\\SRVDEANGJ\Tools\Lab
REM ======================================================================
REM Figure out the build number just in case you said latest
REM ======================================================================
IF /I "%1" EQU "LATEST" (
	REM CALL \\ACSTOOLS\OASIS\SETCRED.CMD
	CALL \\ASTDROP\BUILDS\MOM10SP1\LATEST.BAT
	)
IF "%VERSION%" EQU "" (SET VERSION=%1)

REM ======================================================================
REM Set parameters based on input
REM ======================================================================
SET RUNID=1
IF /I "%2" EQU ".NET" (SET OSTYPE=7)
IF /I "%2" EQU "W2K" (SET OSTYPE=6)
SET LogLoc=C:\LogImport\TestResults.xml
SET NOTES=%4---%3

If /I "%5" EQU "" (SET TESTYPE=BVT) Else (SET TESTTYPE=%5)

REM ======================================================================
REM Get LogFile, VBS Script and other needed files
REM ======================================================================
ECHO.
ECHO Gathering needed files ....
if not exist C:\LogImport (MD C:\LogImport)

if not exist %SYSTEMROOT%\SYSTEM32\CDSCom.dll (
    if not exist C:\LogImport\CDSCom.dll (COPY %SRCPATH%\CDSCom.dll C:\LogImport /Y>NUL)
    call regsvr32 C:\LogImport\CDSCom.dll /S
)

copy %SRCPATH%\LogImporter.dll C:\LogImport /Y>NUL
copy %SRCPATH%\ComLog.dll C:\LogImport /Y>NUL
copy %SRCPATH%\MOM10SP1LogImport.vbs C:\LogImport /Y>NUL
copy %SRCPATH%\strcon.exe C:\LogImport /Y>NUL
copy \\ASTFILES\LOGS\MOM10SP1\%VERSION%\%TESTTYPE%\%3_EN_x86fre\TestResults.xml C:\LogImport /Y>NUL
call REGSVR32 C:\LogImport\LogImporter.dll /S
call REGSVR32 C:\LogImport\ComLog.dll /S

REM ======================================================================
REM Call the VBS script passing in the parameters.
REM ======================================================================
PUSHD C:\LogImport
CALL C:\LogImport\strcon.exe %LogLoc% %LogLoc% /UNICODE
CALL CSCRIPT //Nologo MOM10SP1LogImport.vbs %version% %ostype% %LogLoc% %notes%
POPD
IF EXIST C:\LogImport\CDSCom.dll (REGSVR32 C:\LogImport\CDSCom.dll /U /S)
REGSVR32 C:\LogImport\LogImporter.dll /U /S
CALL REGSVR32 C:\LogImport\ComLog.dll /U /S
RD C:\LogImport /Q/S
goto :EOF

REM ======================================================================
REM Ussage
REM ======================================================================
:Usage
ECHO USAGE:
ECHO Required Parameters: Build # OS Type Machine Name Description Test Type
ECHO OS Type must be W2K or .NET
ECHO Test Type must be BVT or FUNC Default is BVT
ECHO.
ECHO EXAMPLE: MOM10SP1LogImport.cmd 1234 W2K SMXDPBVT03D "W2K SP3 EN" BVT
ECHO EXAMPLE: MOM10SP1LogImport.cmd 1234 .NET SMXDPBVT03D ".NET 3699 ADS" BVT

:EOF
endlocal