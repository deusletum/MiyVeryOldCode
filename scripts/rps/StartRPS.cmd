@echo off
REM ======================================================================
REM
REM AUTHOR: Dean Gjedde (a-deagje) , Microsoft
REM DATE  : 1/11/2005
REM ======================================================================
set ERRNUM=0

if /i "%1" equ "" (
    echo You must suppy a VS build number
    echo     eg: 41231.00
    set ERRNUM=1
    goto :Exit
)
start cmd.exe /K "\\cpvsbuild\drops\whidbey\lab22dev\raw\%1\sources\ddsuites\src\vs\perf\tools\private\lab22dev %1 a-deagje"

:Exit
exit /b %ERRNUM%