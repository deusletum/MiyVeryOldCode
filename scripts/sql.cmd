@echo off
setlocal
set COMMAND=%1

if not defined COMMAND (
    echo You must specify "start" or "stop"
    echo example: sql.cmd start
    goto :EOF
)

if /i "%1" equ "start" ( net start mssqlserver )
if /i "%1" equ "stop" (net stop mssqlserver )

:EOF
endlocal