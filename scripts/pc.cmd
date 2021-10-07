@echo off
if defined ECHO (echo %ECHO%)
REM ----------------------------------------------------------------------
REM Name     : PS.CMD
REM
REM Company  : .
REM
REM Summary  : will start PrimalScript with the requested file
REM
REM Usage    : PS.CMD myfile.txt
REM
REM History  : 2/17/2003 - deangj - created
REM ----------------------------------------------------------------------
setlocal
set FILE=%1

if not defined FILE (
    echo You must specify a file name
    echo example: PS.CMD myfile.txt
    goto :EOF
)

pushd .
pushd>%temp%\tmp.txt
set /P DIR=<%temp%\tmp.txt

set SAPIEN=C:\Program Files\SAPIEN\PrimalCode
set FILE=%1
set PATH=%PATH%;%SAPIEN%
CMD /C SendToPc.exe "%DIR%\%FILE%"

:EOF
endlocal