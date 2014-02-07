//css_pre res(%CSSCRIPT_DIR%\csws.exe, script.resources); //generates script.resources 
//css_res script.resources;		                          //embedds script.resources into assembly

using System;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

class Script
{
	[STAThread]
	static public void Main(string[] args)
	{
        var data = new ResourceManager("script", Assembly.GetExecutingAssembly())
				            .GetObject("csws.exe");
						   
        File.WriteAllBytes("csws.exe", (byte[])data);
	}
}

