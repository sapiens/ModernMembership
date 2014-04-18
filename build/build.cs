//uncomment to include reference to other assembly you might need (one directive per each)
//#r "mycustom.dll"
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CSake;
using NuGet;   //comment it out if you don't create nuget packs or manipulate nuspec files

//don't use namespaces
//any class you create will be considered an inner class of the CSakeWrapper class

const string SlnFile=@"../src/ModernMembership.sln";

const string SlnDir=@"../src";

const string TempDir=@"temp";

//for nuget
static string PackageDir = Path.GetFullPath("temp/package");

static string CurrentDir=Path.GetFullPath("./");

static string NugetExe=Path.Combine(CurrentDir,"libs","nuget.exe");
static string[] Projects=new[]{"ModernMembership","ModernMembership.Web","ModernMembership.Web.Mvc","ModernMembership.Authorization","ModernMembership.SqlFu"};

static bool Built=false;

[SkipIf("Built",true)]
public static void CleanUp()
{
    TempDir.CleanupDir();
    SlnFile.MsBuildClean();         
}


[Depends("CleanUp")] 
[SkipIf("Built",true)]
public static void Build()
{
    SlnFile.MsBuildRelease();    
    Built=true;
}

[Default]
[Depends("Build")] 
public static void Local()
{
    foreach(var project in Projects)
    {
        var releaseDir=Path.GetFullPath(Path.Combine(SlnDir,project,"bin/Release"));
        "robocopy".Exec(releaseDir,TempDir,"/E","/XN","/NS","/NC","/NJH","/NJS");
    }
}

[Depends("Build")]
public static void Pack()
{
  

}


static string PackedNugetName;

static void Pack(string project,string[] deps=null)
{
    PackageDir.MkDir();
    var nuspecFile=Path.Combine(CurrentDir,project+".nuspec");
    
    Dictionary<string,string> depsVersions=new Dictionary<string,string>();
    if (deps!=null)
    {
        foreach(var dep in deps)
        {
            depsVersions[dep]=GetVersion(dep);            
        }
    }
    
    UpdateVersion(nuspecFile,project,depsVersions);
    BuildNuget(project,Path.Combine(SlnDir,project));
}

//------------------------------ Utils ----------------

//updates version in nuspec file
static void UpdateVersion(string nuspecFile,string assemblyName,Dictionary<string,string> localDeps=null)
{
    var nuspec=nuspecFile.AsNuspec();   
    nuspec.Metadata.Version=GetVersion(assemblyName);
    if (localDeps!=null)
    {
        foreach(var kv in localDeps)
        {
            nuspec.AddDependency(kv.Key,kv.Value);
        }
    }
    nuspec.Save(TempDir);    
}

//basePath= relative path for package files source. Usually is the project dir.
static void BuildNuget(string nuspecFile,string basePath)
{
    //if (!nuspecFile.EndsWith(".nuspec"))
    //{
      var project=nuspecFile;  
        nuspecFile+=".nuspec";
    //}
    Path.Combine(TempDir,nuspecFile).CreateNuget(basePath,PackageDir);    
}

static string GetVersion(string asmName)
{
    return Path.Combine(GetReleaseDir(asmName),asmName+".dll").GetAssemblyVersion().ToSemanticVersion().ToString();
}

static string GetReleaseDir(string project)
{
 return Path.Combine(SlnDir,project,"bin","Release");
}
