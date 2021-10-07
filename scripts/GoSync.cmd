@echo off
set ROOT=c:\dd
set DIRS=

pushd %ROOT%\dds
call sd.exe sync ...
FOR /F "eol=; tokens=2,3* delims=, " %i in (myfile.txt) do @echo %i %j %k