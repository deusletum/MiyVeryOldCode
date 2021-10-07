@echo off


if NOT EXIST setenv.cmd copy setenv.tpt setenv.cmd
attrib -r setenv.cmd
call setenv.cmd

set COMPILER=%CLRBINDIR%\csc.exe
set ROOTPATH=%~dp0
set COMPILEDEST=%ROOTPATH%bin
set GENERALOPTIONS=/debug:full
set DEBUGOPTIONS=/define:DEBUG

: make sure the bin directory is created
if NOT EXIST bin md bin
if NOT EXIST bin\chk md bin\chk
if NOT EXIST bin\fre md bin\fre

pushd src

echo Building Debug MS.CommandLine.dll
"%COMPILER%" /target:library /out:"%ROOTPATH%bin\chk\MS.CommandLine.dll" %GENERALOPTIONS% %DEBUGOPTIONS% /recurse:*.cs

echo Building Retail MS.CommandLine.dll
"%COMPILER%" /target:library /out:"%ROOTPATH%bin\fre\MS.CommandLine.dll" %GENERALOPTIONS% /recurse:*.cs

popd

pushd TestApp

echo Building Unit tests 
"%COMPILER%" /target:exe /out:%ROOTPATH%bin\chk\TestApp.exe /r:"%ROOTPATH%bin\chk\MS.CommandLine.dll" %GENERALOPTIONS% %DEBUGOPTIONS% /recurse:*.cs

popd

echo Running Unit tests
pushd bin\chk

TestApp.exe
if errorlevel 1 echo TESTS FAILED!!!!!!

popd
:end