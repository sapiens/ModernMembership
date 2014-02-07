using System;
using System.Threading;
using CSScriptLibrary;

public interface IScript
{
    void Hello(string greeting);
}

class Host
{
    const string code = @"using System;

                         public class Script : MarshalByRefObject
                         {
                             public void Hello(string greeting)
 	                         {
 	                             Console.WriteLine(greeting);
 	                         }
                         }";

    static void Main()
    {
        ThreadPool.QueueUserWorkItem(x => TestLifeExtension(2, 1, "Expected to pass (0)"));
        ThreadPool.QueueUserWorkItem(x => TestLifeExtension(7, 6, "Expected to pass (1)"));
        ThreadPool.QueueUserWorkItem(x => TestLifeExtension(-1, 6, "Expected to fail (2)"));
        ThreadPool.QueueUserWorkItem(x => TestLifeExtension(0, 6, "Expected to fail (3)"));

        Console.ReadLine();
    }

    static void TestLifeExtension(int extensionMinutes, int callDelayMinutes, string message)
    {
        using (var helper = new AsmHelper(CSScript.CompileCode(code), null, false))
        {
            IScript script = helper.CreateAndAlignToInterface<IScript>("*");

            if (extensionMinutes != -1)
            {
                helper.RemoteObject.ExtendLifeFromMinutes(extensionMinutes); //this line is required if the script methods to be invoked though helper.Invoke(...)
                (script as MarshalByRefObject).ExtendLifeFromMinutes(extensionMinutes);
            }
            try
            {
                script.Hello("");
                Thread.Sleep(1000 * 60 * callDelayMinutes);
                script.Hello(message + ": Hi TestLifeExtension...");
            }
            catch (Exception e)
            {
                Console.WriteLine(message + ": Error in TestLifeExtension - " + e.Message);
            }
        }
    }
}