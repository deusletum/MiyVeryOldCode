@echo off
REM ======================================================================
REM
REM CMD File -- Created with SAPIEN Technologies PrimalSCRIPT(TM)
REM
REM NAME: clean.cmd
REM
REM AUTHOR: Microsoft Employee , Microsoft
REM DATE  : 12/18/2002
REM
REM COMMENT: <comment>
REM
REM ======================================================================
diskpart /s diskpart.txt
echo Y | format c: /fs:fat32 /q
Exit