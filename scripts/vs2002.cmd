@echo off
if defined ECHO (echo %ECHO%)
REM ----------------------------------------------------------------------
REM Name     : <vs2002.cmd>
REM
REM Company  : .
REM

REM
REM Summary  : 
REM
REM Usage    : 
REM
REM History  : 4/7/2003 - Dean Gjedde - created
REM ----------------------------------------------------------------------
setlocal
set FILE=%1

if not defined FILE (
    echo You must specify a file name
    echo example: vs2002.cmd myfile.txt
    goto :EOF
)

pushd .
pushd>%temp%\tmp.txt
set /P DIR=<%temp%\tmp.txt

set VS2002=C:\Program Files\Microsoft Visual Studio .NET\Common7\IDE
set FILE=%1
set PATH=%PATH%;%VS2002%
CMD /C devenv.exe "%DIR%\%FILE%"

:EOF
endlocal