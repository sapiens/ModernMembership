echo off
echo Generating executable from the script and embedding csws.exe as resource 
echo .
css /e script.cs 
echo .
echo Executing script.exe, which reads embedded resource data and saves it as csws.exe
script.exe
echo .
pause