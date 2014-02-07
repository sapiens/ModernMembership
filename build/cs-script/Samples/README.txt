Wast majority of the samples are implemented as scripts. If it is required to run/edit the sample files 
with Visual Studio then you will need to right-click the sample (script) file and select in the context menu
CS-Script -> 'Open With VS2012'.

Alternatively you can convert the script into the complete VS project by selecting 'CS-Script' -> 'Convert to' -> 'VS 2012 project'  
-----------------------------------------------------------------------------------------------------
The following are the description of some of the demo scripts. 

 NKill.cs               - Kills processes specified as command-line parameters.
 GetUrl.cs              - Gets URL content and saves it in a file. Capable to go through proxy authentication.
 Debug.cs               - Opens script with temporary C# project for running it under debugger.
 Install.cs             - Installs/Uninstalls 'C# Script' shell extensions that will allow to run/debug any script with mouse right-click.
 ImportData.cs          - Imports data from specified file in SQL Server table.
 ExportData.cs          - Exports data to specified file from SQL Server table.
 RunScript.cs           - Script that runs another script in background with redirection output to the 'parent script'. 
                          Run "cscscript runScript.cs tick.cs" and "cscscript tick.cs" to see it in work.
 ImportTickScript.cs    - Script that imports and execues 'tick.cs' script. 
                          Run "cscscript ImportTickScript.cs" and to see it in work.
 Tick.cs                - Simple script that counts specified number of seconds (used as demo for runScript). 
 SynTime.cs             - Gets time from http://tycho.usno.navy.mil/cgi-bin/timer.pl and synchronises PC systemtime.
 sysFindConfig.cs       - Displays configuration console to adjust system 'FindInFile' options on WindowsXP.
                          This is the answer for WindowsXP BUG described in MSDN articles Q309173 and 
                          "Using the 'A Word or Phrase in the File' Search Criterion May Not Work".
 SMTPMailTo.cs          - Sends e-mail to the specified address.        
 Creditentials.cs       - Prompts and collect user login information.
 GetOptusUsage.cs       - Retreives monthly data usage (applicable only for "Optus Australia" customers).
 Reflect.cs             - Prints assembly reflection info.
 Progressbar.cs         - Shows "continuous" progressbar. This is an example how to draw in GDI+ without flickering.
 CutFile.cs             - Cuts file in pieces and prepares batch file to reassemble the original file.  
 PrintScreen.cs         - Captures screen image and saves it to a file (default file: screen.gif)
 print.cs               - Collection of simple reusable printing routines (the alternative to cmd: notepad.exe /p <file>). 
 google.cs              - Script that performs Google search.   
 googleWebService.cs,
 GoogleSearchService.cs - Script that performs Google search using Google API (WebService). 


