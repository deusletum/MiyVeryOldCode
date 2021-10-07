@echo off
REM ***************************************************************************
REM Check for required parameter
REM ***************************************************************************
IF "%1" == "" GOTO Error
if "%1" == "/?" GOTO Error
if "%1" == "?" GOTO Error

Setup32.cmd %1 autopart oasis-w2k
goto :End

REM ***************************************************************************
REM Ussage
REM ***************************************************************************
:Error
ECHO Usage:
ECHO .
ECHO S.cmd {machname_name}
ECHO Example .cmd office-brick1

:End