echo off

IF EXIST %windir%\SysWOW64 goto x64 

echo Installing shell extension...
start %windir%\System32\regsvr32 "%CSSCRIPT_DIR%\Lib\ShellExtensions\CS-Script\ShellExt.cs.{25D84CB0-7345-11D3-A4A1-0080C8ECFED4}.dll"
goto exit

:x64
echo ------------
echo ATTENTION
echo If executed on Vista you must modify this file to include
echo absolute path for all DLLs and run the modifiet batch file as 
echo an administrator 
echo ------------
echo Installing x32 and x64 shell extension...
start %windir%\System32\regsvr32 "%CSSCRIPT_DIR%\Lib\ShellExtensions\CS-Script\ShellExt64.cs.{25D84CB0-7345-11D3-A4A1-0080C8ECFED4}.dll"
start %windir%\SysWOW64\regsvr32 "%CSSCRIPT_DIR%\Lib\ShellExtensions\CS-Script\ShellExt.cs.{25D84CB0-7345-11D3-A4A1-0080C8ECFED4}.dll"


:exit