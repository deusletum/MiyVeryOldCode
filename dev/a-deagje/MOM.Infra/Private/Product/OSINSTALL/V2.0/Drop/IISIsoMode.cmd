REM
REM This turns on/off the Worker Process Isolation Mode in IIS 6 (IIS 5 Compat mode)
REM
REM Putting in a 1 as the parameter turns it on
REM Putting in anything else turns it off
REM

net stop w3svc
mdutil set w3svc -prop 9203 -value %1
net start w3svc
