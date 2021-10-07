@echo off
copy D:\sqlvault\sqlmain\testtools\CommandLine\src\sstools.snk D:\sqlvault\sqlmain\testtools\CommandLine\src\obj\Debug
if errorlevel 1 goto CSharpReportError
goto CSharpEnd
:CSharpReportError
echo Project error: A tool returned an error code from the build event
exit 1
:CSharpEnd