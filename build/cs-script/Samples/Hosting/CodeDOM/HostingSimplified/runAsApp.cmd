echo off
css /e host.cs
copy %CSSCRIPT_DIR%\Lib\CSScriptLibrary.dll CSScriptLibrary.dll
host.exe

pause