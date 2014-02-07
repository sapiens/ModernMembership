using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms; 
using CSScriptLibrary;
using csscript;

public class Script 
{
    static void PrintScriptName(string algorithmName, Func<string> action)
    {
        try
        {
            Console.WriteLine(string.Format("Source script file by {0}: {1}",  algorithmName, action()));
        }
        catch
        {
            Console.WriteLine(string.Format("Source script file by {0}: error", algorithmName));
        }
    }
    public static void Main() 
    {
        var entryAsm = Assembly.GetEntryAssembly();
        var thisAsm = Assembly.GetExecutingAssembly();
        var hostAsm = Assembly.GetCallingAssembly();

        if (thisAsm == entryAsm)
            Console.WriteLine("It is not a script assembly but a fully compiled stand alone application.");

        if (hostAsm == null)
            Console.WriteLine("It is an assembly hosted by " + hostAsm.GetName().Name);

        PrintScriptName("reflection #1    ", ()=>  CSScript.GetScriptName(Assembly.GetExecutingAssembly()));
        PrintScriptName("reflection #2    ", ()=>  Assembly.GetExecutingAssembly().GetScriptName());
        PrintScriptName("reflection #3    ", ()=>  GetScriptName(thisAsm));
        PrintScriptName("CSSEnvironment #1", ()=>  CSSEnvironment.PrimaryScriptFile); //script that was executed from command line or double-clicked
        PrintScriptName("CSSEnvironment #2", ()=>  CSSEnvironment.ScriptFile);		  //script that is currently executed; it may not be a PrimaryScriptFile if it is a pre-execution scripting scenario
        PrintScriptName("Environment Var  ", ()=>  Environment.GetEnvironmentVariable("EntryScript"));
    }

    static public string GetScriptName(Assembly assembly)
    {
        var attr =  (from item in assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), true)
                     select (AssemblyDescriptionAttribute)item)
                     .FirstOrDefault();

        return attr == null ? "" : attr.Description;
    }
}