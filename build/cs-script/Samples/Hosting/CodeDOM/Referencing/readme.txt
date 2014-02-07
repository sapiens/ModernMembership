The Host.cs script is an example of hosting the script which requires an external assembly at run-time. The assembly which is not part of the host application assembly set.
Such assembly can be referenced by one of the following techniques: 
	1. directly from the script by using "//css_ref...;" directive or "using statement" 
	2. external assembly can be specified by the host application. 

Host.cs demonstrates technique #2. It passes the location of the ExternalAsm.dll to the script engine as the last parameter with the CSScript.Load() and CSScript.Compile() method.

VS2008 project directory contains the same code sample packed to the Visual Studio 2008 project.