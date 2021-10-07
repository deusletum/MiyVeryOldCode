@echo OFF
if defined ECHO (echo %ECHO%)
pushd .
setlocal

REM ======================================================================
REM Copyright . 2004
REM
REM Module  : CopyTools.cmd
REM
REM Summary: Copies specified toolserver folders.
REM          Also registers files and adds new location to %ADDPATH%.
REM
REM History: (08/10/03) John Heaton - Initial coding
REM          (10/02/03) Jennla - added header and updated to copy depending on %PROCESSOR_ARCHITECTURE%
REM          (10/17/03) Jennla - fixing for loops for *.dlls
REM          (02/05/04) Jheat  - Updating to take optional flag and remove RO if exist.
REM          (02/06/04) myocom - Added logging, updated usage, removed /s /e /purge from robocopy
REM                              since /MIR implies those switches
REM          (02/06/04) Jheat  - Updating to make sure its all happy.
REM          (05/11/04) Jheat  - Updating to include copying common tools infra bug 3601
REM                            - Also brought up to coding standards
REM          (06/07/04) Jheat  - Updated to copy .NET stuff from common
REM          (06/09/04) Jheat  - Updated to remove /R in registering Dll's
REM ======================================================================

REM ======================================================================
REM Set constants
REM ======================================================================
set CopyTools.ToolSrv=\\SMX.NET\TOOLS
set CopyTools.ValidInputs=BRICKTOOLS IDW MSTOOLS ALL
set CopyTools.LogFile=%SYSTEMDRIVE%\LOGS\CopyTools.Log
md %SYSTEMDRIVE%\LOGS >nul 2>&1
if /i "%PROCESSOR_ARCHITECTURE%" equ "x86" (
    set CopyTools.CopyFlags=/MIR /R:3 /W:5 /LOG+:%SYSTEMDRIVE%\LOGS\ROBOCOPY.LOG /TEE /NP
    set CopyTools.CopyProgram=%CopyTools.ToolSrv%\bricktools\X86\robocopy.exe
) else (
    set CopyTools.CopyFlags=/qrhcye
    set CopyTools.CopyProgram=xcopy.exe
)

REM ======================================================================
REM Ensure proper parameters entered
REM ======================================================================
if "%1" equ "" (goto :Usage)
if "%1" equ "/?" (goto :Usage)
if "%1" equ "?" (goto :Usage)
if "%1" equ "-?" (goto :Usage)
if /i "%2" equ "/NoPath" (
    set CopyTools.AddPath=
    ) else (
        set CopyTools.AddPath=True
    )
)
for %%i in (%CopyTools.ValidInputs%) do (
    if /i "%1" equ "%%i" (
        goto :CheckOSVer
    )
)
goto :Usage

REM ======================================================================
REM Check to determine OSVER information
REM ======================================================================
:CheckOSVer
if not defined OSVER (
    call :LogMsg STDOUT Running OSVER ...
    call %CopyTools.ToolSrv%\scripts\osver.cmd >NUL 2>&1
) else (
    call :LogMsg STDOUT Not running OSVER
    call :LogMsg STDOUT OSVER already defined as %OSVER%
)

REM ======================================================================
REM check user input
REM ======================================================================
:CheckInput
call :LogMsg STDOUT Copytools started
if /i "%1" equ "ALL" (
    set CopyTools.ToCopy=%CopyTools.ValidInputs: ALL=%
) else (
    set CopyTools.ToCopy=%1
)

REM ======================================================================
REM Do the copy
REM ======================================================================
:DoCopy
for %%i in (%CopyTools.ToCopy%) do (
    call :LogMsg STDOUT Copying %%i
    attrib -r /s %SYSTEMROOT%\%%i\*.* >nul 2>&1
    if /i "%%i" equ "BRICKTOOLS" (
        %CopyTools.CopyProgram% %CopyTools.ToolSrv%\BRICKTOOLS\%PROCESSOR_ARCHITECTURE% %SYSTEMROOT%\BRICKTOOLS\ %CopyTools.CopyFlags% >nul 2>&1
        copy %CopyTools.ToolSrv%\bricktools\COMMON\*.* %SYSTEMROOT%\BRICKTOOLS\*.* /y >nul 2>&1
        %CopyTools.CopyProgram% %CopyTools.ToolSrv%\BRICKTOOLS\COMMON\NET %SYSTEMROOT%\BRICKTOOLS\NET\ %CopyTools.CopyFlags% >nul 2>&1
        REM Register DLLs ----
        call :LogMsg STDOUT Registering DLLs
        if exist %SYSTEMROOT%\BRICKTOOLS\COM (
            pushd %SYSTEMROOT%\BRICKTOOLS\COM
            for %%r in (*.dll) do (
                call :LogMsg %%r - Registered
                regsvr32 /s %%r >>%CopyTools.Logfile%
            )
            popd
        )
        if exist %SYSTEMROOT%\BRICKTOOLS\NET (
            pushd %SYSTEMROOT%\BRICKTOOLS\NET
            for %%r in (*.dll) do (
                call :LogMsg %%r
                gacutil.exe /nologo /i %%r >>%CopyTools.Logfile%
            )
            for %%r in (*.dll) do (
                call :LogMsg %%r
                regasm.exe /nologo %%r >>%CopyTools.Logfile%
            )
            popd
        )
        REM End DLL register ---
    ) else (
        %CopyTools.CopyProgram% %CopyTools.ToolSrv%\%%i\%OSVER%\%PROCESSOR_ARCHITECTURE% %SYSTEMROOT%\%%i\ %CopyTools.CopyFlags% >nul 2>&1
    )
    if defined CopyTools.AddPath (
        call :LogMsg STDOUT Running addpath for %%i
        cscript //nologo %CopyTools.ToolSrv%\drop\addpath.vbs PRE "%SYSTEMROOT%\%%i" >nul 2>&1
    )
)
goto :End

REM ======================================================================
REM Logging Section
REM ======================================================================
:LogMsg
if /i "%1" neq "STDOUT" (
    echo [%DATE% %TIME%] %*>>%CopyTools.Logfile%
    goto :EOF
)
set CopyTools.LogTest.Msg=%*
set CopyTools.LogTest.Msg=[%DATE% %TIME%] %CopyTools.LogTest.Msg%
set CopyTools.LogTest.Msg=%CopyTools.LogTest.Msg: STDOUT=%
echo %CopyTools.LogTest.Msg%
echo %CopyTools.LogTest.Msg%>>%CopyTools.Logfile%
goto :EOF

REM ======================================================================
REM Usage
REM ======================================================================
:Usage
echo.
echo Usage:
echo.
echo copytools.cmd TOOLS [/NOPATH]
echo.
echo Example: copytools.cmd IDW
echo.
echo Valid Tools:
echo    %CopyTools.ValidInputs%
echo.
echo Optional Parameters
echo.
echo /NOPATH = Doesn't add tools to path.

:End
call :LogMsg STDOUT CopyTools Complete
endlocal
popd
exit /b 0