@echo off

echo Reset ERRORLEVEL to 1 (color 00 sets ERRORLEVEL to 1)
color 00
echo This ERRORLEVEL should be 1: %ERRORLEVEL%

echo.

echo Call Errorlevel tester with 43 as first argument
cscs /nl ErrorLevelTester.cs 43
echo This ERRORLEVEL should be 43: %ERRORLEVEL%

echo.

echo Call Errorlevel tester with 0 as first argument
cscs /nl ErrorLevelTester.cs 0
echo This ERRORLEVEL should be 0: %ERRORLEVEL%

echo.

echo Call Errorlevel tester with no argument (thereby causing the script to throw an "index out of range" exception).
cscs /nl ErrorLevelTester.cs
echo This is the ERRORLEVEL after the .NET exception: %ERRORLEVEL%

pause