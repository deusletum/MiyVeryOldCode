@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
set Inst_Etrust.ErrNum=0
set Inst_Etrust.ErrMsg=

REM ======================================================================
REM Copyright . 2004
REM
REM Module    : Inst_Etrust.cmd
REM
REM Summary   : Install's Etrust
REM
REM Exit codes: 0 - Success
REM             1 - Incorrect parameter/Usage
REM
REM History   : (6/14/2004) John Heaton - Initial coding
REM ======================================================================

REM ======================================================================
REM set constants
REM ======================================================================
set Inst_Etrust.FileSrv=\\smx.net\products\etrust
set Inst_Etrust.LOGFILE=%SYSTEMDRIVE%\LOGS\Inst_Etrust.log

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" neq "" (goto :Usage)

REM ======================================================================
REM Temporary Check for AMD64
REM ======================================================================
if /i "%PROCESSOR_ARCHITECTURE%" equ "AMD64" (
    set Inst_Etrust.ErrMsg=Etrust for AMD64 not supported at this time
    set Inst_Etrust.ErrNum=1
    goto :End
)

REM ======================================================================
REM check for existance of setup.exe
REM ======================================================================
call :LogMsg STDOUT Checking for Etrust @ %Inst_Etrust.FileSrv%\%PROCESSOR_ARCHITECTURE%
if not exist %Inst_Etrust.FileSrv%\%PROCESSOR_ARCHITECTURE%\setup.exe (
    set Inst_Etrust.ErrMsg=Unable to Locate SETUP.EXE for Etrust @ %Inst_Etrust.FileSrv%\%PROCESSOR_ARCHITECTURE%
    set Inst_Etrust.ErrNum=1
    goto :End
) else (
    call :LogMsg STDOUT Installing Etrust from %Inst_Etrust.FileSrv%\%PROCESSOR_ARCHITECTURE%
    call %Inst_Etrust.FileSrv%\%PROCESSOR_ARCHITECTURE%\SETUP.EXE /S /N
    call :LogMsg STDOUT Waiting for Etrust Install to Complete
    REM This is because Setup.exe terminates and spawns another process.
    sleep 90
    set Inst_Etrust.ErrMsg=Etrust Install Complete
    set Inst_Etrust.ErrNum=0
    goto :End
)


REM ======================================================================
REM Logging Section
REM ======================================================================
:LogMsg
if /i "%1" neq "STDOUT" (
    echo [%DATE% %TIME%] %*>>%Inst_Etrust.Logfile%
    goto :EOF
)
set Inst_Etrust.LogTest.Msg=%*
set Inst_Etrust.LogTest.Msg=[%DATE% %TIME%] %Inst_Etrust.LogTest.Msg%
set Inst_Etrust.LogTest.Msg=%Inst_Etrust.LogTest.Msg: STDOUT=%
echo %Inst_Etrust.LogTest.Msg%
echo %Inst_Etrust.LogTest.Msg%>>%Inst_Etrust.Logfile%
goto :EOF



REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo Inst_Etrust.cmd
echo.
echo Example: Inst_Etrust.cmd
echo.
set Inst_Etrust.ErrNum=1
set Inst_Etrust.ErrMsg=Usage

:End
popd
if defined Inst_Etrust.ErrMsg (
    call :LogMsg STDOUT %Inst_Etrust.ErrMsg% - %Inst_Etrust.ErrNum%
) else (
    call :LogMsg STDOUT Etrust Install Complete
)
exit /b %Inst_Etrust.ErrNum%