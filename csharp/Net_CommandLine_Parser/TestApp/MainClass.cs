using System;
using System.Diagnostics;
using System.Reflection;
using MS.CommandLine;

namespace TestApp
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainClass
	{
        const int FatalExitCode = 1;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] arguments)
		{
            Command command = null ;
            try
            {
                CommandFactory factory = new CommandFactory(Assembly.GetExecutingAssembly(), new StandardOptionParser("test"));

                try
                {
                    // create the command
                    //
                    command = factory.Create(arguments) ;
                }
                catch(UsageException e)
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: " + e.Message);
                    Console.WriteLine();
                    if(e.Command != null)
                    {
                        e.Command.Specification.PrintFullUsage(Console.Out);
                    }
                    return FatalExitCode;
                }
                //
                // Run the command.
                //
                Debug.Assert(command != null, "command != null");
                command.Run();
                command.Dispose();
                return command.ReturnCode;
            }
            catch(Exception e)
            {
                if(command != null)
                {
                    command.Dispose();
                }
                Console.Error.WriteLine("Error running tests: {0}", e) ;
                return FatalExitCode;
            }		
        }
	}
}
