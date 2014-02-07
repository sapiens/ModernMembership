// include Fake lib
#r @"fake/FakeLib.dll"
open Fake 

let buildDir = "temp/"
let slnDir="../src/ModernMembership/"
let sln=slnDir + "ModernMembership.sln"


let verbose proj args=
{
 args with 
 Verbosity=MSBuildVerbosity.Quiet
 Targets=[] 
}

Target "Clean" (fun _ ->    
    build (fun p ->
     {
     p with 
     Verbosity = MSBuildVerbosity.Quiet
     }
    ) sln
    
    CleanDir buildDir     
    MSBuild null "Clean" ["Configuration","Release"] [sln]
    |> ignore
)

Target "Local" (fun _ ->
     MSBuildRelease buildDir "Build" [sln]
    |> Log "Compile Output"
)

// Default target
Target "Default" (fun _ ->
    trace "Hello World from FAKE"
    
)

//Dependencies

"Clean"
  ==> "Local"

// start build
RunTargetOrDefault "Local"