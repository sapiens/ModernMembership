//css_host /version:v4.0 /platform:x86;
using System;  
using System.Runtime.InteropServices; 

class Script
{ 
    static public void Main(string [] args) 
    {
        Console.WriteLine("TragetFramework: " + Environment.Version);
        Console.WriteLine("Platform: " + ((Marshal.SizeOf(typeof(IntPtr)) == 8) ? "x64" : "x32"));
    }
}
