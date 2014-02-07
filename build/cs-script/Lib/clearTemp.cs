using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class Script
{
    const string usage = "Usage: cscscript clearTemp\n" +
                         "Deletes all temporary files created by the script engine.\n";

    static public void Main(string[] args)
    {
        if (args.Length == 1 && (args[0] == "?" || args[0] == "/?" || args[0] == "-?" || args[0].ToLower() == "help"))
        {
            Console.WriteLine(usage);
        }
        else
        {
            //delete temporary Visusl Studio projects
            string baseDir = Path.Combine(Path.GetTempPath(), "CSSCRIPT");
            if (Directory.Exists(baseDir))
            {
                foreach (string projectDir in Directory.GetDirectories(baseDir))
                {
                    string pidFile = Path.Combine(projectDir, "host.pid");

                    try
                    {
                        int id = int.Parse(File.ReadAllText(pidFile));
                        if (Process.GetProcessById(id) != null)
                            continue; //the process using this project is still active
                    }
                    catch { }

                    DeleteDir(projectDir);
                }

                //delete all DLLs; any dll that is currently in use will not be deleted (try...catch);
                string[] files = Directory.GetFiles(baseDir);

                Thread.Sleep(3000); //allow just created files to be loaded (get locked)

                foreach (string file in files)
                {
                    try
                    {
                        if (Path.GetFileName(file).EndsWith("_recent.txt"))
                            File.Delete(file);
                    }
                    catch { }
                }
            }
        }
    }

    static private void DeleteDir(string dir)
    {
        //deletes folder recursively
        try
        {
            string infoFile = Path.Combine(dir, "css_info.txt");

            string sourceDirectory = "";

            try
            {
                sourceDirectory = File.ReadAllLines(infoFile)[1]; //second line
            }
            catch { }

            foreach (string file in Directory.GetFiles(dir))
                try
                {
                    if (!file.EndsWith("_recent.txt") && file != infoFile)
                    {
                        string srcFile = Path.GetFileNameWithoutExtension(file) + ".*"; //name of the potential script file that was used to produce the compiled assembly 

                        if (Directory.GetFiles(sourceDirectory, srcFile).Length > 0)
                            continue; //script still exists

                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                }
                catch
                {
                }

            foreach (string subDir in Directory.GetDirectories(dir))
                DeleteDir(subDir);

            if (File.Exists(infoFile) && Directory.GetFiles(dir).Length == 1 && Directory.GetDirectories(dir).Length == 0)
                File.Delete(infoFile);

            if (Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
                Directory.Delete(dir);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}