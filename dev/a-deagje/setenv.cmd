REM
REM This is where you add your private build environment settings.
REM Usually, this only consists of your favorite editor settings.
REM
REM If you want source depot to use your editor for change pages (notepad is
REM the default), set the SDEDITOR macro to your editor name.
REM
REM You may have to add some entries to the path to find your editor.
REM
REM Note: Whatever you add to the path will have NO effect on the build
REM       tools used when you call nmake or build in a razzle window.
REM
REM If you're tempted to put other stuff here to significantly modify the
REM build environment, first look at the what razzle does already.
REM
REM     razzle help
REM 
REM from a build window will show the current available options.
REM Hopefully your requirement is already there.
REM
REM When you're done editing this file, save it and exit.  Then at
REM your earliest convience, add
REM      C:\dd\developer\a-deagje\setenv.cmd
REM to source control.
REM
REM E&C is nice, but causes linker warnings
REM set EDIT_AND_CONTINUE=
REM set BUILD_ALLOW_LINKER_WARNINGS=
REM
REM -i hides dep. scanning warnings
set BUILD_DEFAULT=-i %BUILD_DEFAULT%
REM
REM resolver is nice for merges, but requires SDFORMEDITOR to be set to something else
REM set SDEDITOR=Resolver.exe
REM set SDFORMEDITOR=SDForms.exe
REM set SDFORMEDITOR=notepad
REM
REM sdpager is nice for viewing diffs.  Try 'sd diff | sd pager -cp'
REM set SDPAGER=sdpager -cp
REM
REM paths for sdpager / resolve / sdforms
REM set PATH=%PATH%;\\ddfiles\sdtools\
REM
REM SDVRAID and SDVBRANCHPREFIX make SDV work better
if "%SDVRAID%" == "" set SDVRAID=http://bugcheck/bugs/vswhidbey/#.asp
if "%SDVBRANCHPREFIX%" == "" set SDVBRANCHPREFIX=devdiv
REM
REM Include useful info in the razzle prompt.  Clearly an aquired taste.
REM prompt [%BUILD_PRODUCT%:%_BuildType%] $P$G 
REM If you have multiple machines, add another COMPUTERNAME test as below
REM
if "%COMPUTERNAME%" == "A-DEAGJE01" goto DoA-DEAGJE01
REM
echo %COMPUTERNAME% is unknown - Please update %INIT%\setenv.cmd
goto :eof
REM
:DoA-DEAGJE01
REM
REM *** Add your private environment settings for computer: A-DEAGJE01 here ***
REM path=%path%;<**Your path here**> 
REM set SDEDITOR=<**Your editor name here**> 
REM
goto :eof
