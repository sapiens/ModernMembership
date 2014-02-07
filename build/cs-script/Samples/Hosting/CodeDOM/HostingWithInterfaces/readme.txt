The Host.cs script is an example of the simplified script hosting model when the host application and the script share the same run-time assembly set.

This means that all public types of the host assembly are available from the script code and vice versa.

This particular example though is different to the one contained in the cs-script\Samples\Hosting\HostingSimplified folder. It utilizes interfaces to esteblish contract between the host and the script. Interfaces allows you to avoid using reflection based invocation (AsmHelper.Invoke()) and use direct methods calls instead.