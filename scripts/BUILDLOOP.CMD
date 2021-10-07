@echo off
if defined ECHO (echo %ECHO%)

if "%1" equ "/?" (goto :usage)
if /I "%1" equ "" (goto :usage)
TITLE BUILDLOOP.CMD (%1) RUNNING....

:loop
echo Build %1 has not dropped yet.. Will keep looking...
sleep 30
goto :execution

:execution
if not exist \\astdrop\drop\momx\%1\%1end (
             goto :loop ) else ( 
                   call \\smxfiles\scratch\jheat\momx\MOMXBVTS.CMD %1 both /norefresh 
                   echo BVT JOBS HAVE BEEN SUBMITTED, Please see http:\\smxditsrv01 for job tracking.
                   goto :eof
                 )
                                                     

:usage
echo BUILDLOOP.CMD
echo.
echo Used to constantly look for a build to drop and execute BVTs when it does.
echo.
echo Example : BUILDLOOP.CMD 2078
echo.
echo           Make sure all Machines for MOMX BVTS are in a happy state... MOMTEST01-32.
echo.
goto :eof

:eof