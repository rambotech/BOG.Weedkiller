@echo off

REM Sample Batch file for running WeedKillerWorker.exe
REM A log file is created by redirecting output to captured.txt in the local folder.
REM You only need to add code to process success and failure conditions below.

echo %DATE% %TIME% -- Launching > captured.txt
echo.> captured.txt

WeedKillerWorker.exe "\\SERVER\ConfigFiles$\WeedKiller_DATACENTER1.xml" >> captured.txt 2>>&1
set EL=%ERRORLEVEL%
echo %DATE% %TIME% -- Exit code: %EL% >> captured.txt

if %EL% GEQ 1 goto SUCCESS

echo %DATE% %TIME% -- FAILURE >> captured.txt
echo.> captured.txt

REM ... anything to do here when a failure occurs.
goto END

:SUCCESS

echo %DATE% %TIME% -- SUCCESS >> captured.txt
echo.> captured.txt

REM ... anything to do here when success occurs.
goto END

:END


