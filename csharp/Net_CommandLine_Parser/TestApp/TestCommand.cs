using System;
using System.Diagnostics;
using System.Reflection;

using MS.CommandLine;

namespace TestApp
{
	/// <summary>
	/// Summary description for TestCommand.
	/// </summary>
    [Command("test",
    //
    // The following optional members are for documentation purposes.  The command infrastructure
    // can automatically generate a help page based on this info.
    //
    BriefUsage = "<options> <testname>",
    BriefDescription="Tests command line libraries",
    GeneralInformation = @"This runs unit tests on the command line library.",
    //
    // By default parameters on the command line not related to options are not allowed.
    // set this attribute to true to allow unnamed parameters
    //
    AllowNoNameOptions = true)]
	public class TestCommand : Command
	{
        string adHocParameter;

        // 
        // An option attribute on a property allows the command infrastructure
        // to automatically set the property if the option is specified on the
        // command line.
        //
        [Option("adhoc",  
             OptionValueType.ValueOptional,
             Description = "-{0} <something>: Something that will be written to standard out when the "+
                           "tests are executed")
        ]
        public string AdHocParameter
        {
            get
            {
                return this.adHocParameter;
            }
            set
            {
                this.adHocParameter = value;
            }
        }

		public TestCommand()
		{
		}

        protected override void RunImplementation()
        {
            if(this.adHocParameter != null)
            {
                this.Output.WriteLine(this.adHocParameter);
            }
            TraceListener testListener = new TestListener();
            Debug.Listeners.Clear();
            Debug.Listeners.Add(testListener);
            
            Trace.Listeners.Clear();
            Trace.Listeners.Add(testListener);

            TestSingleCommandParser();
            TestArrayPropertySet();
            TestMultipleCommand();
            TestCommandAlias();
            TestOptionDelimiter();
        }

        private void TestSingleCommandParser()
        {
            StandardOptionParser parser = new StandardOptionParser("example");
            CommandFactory factory = new CommandFactory(typeof(ExampleCommand).Assembly, parser);
            string[] arguments = new string[0];
            Command command = factory.Create(arguments);

            Debug.Assert(command is ExampleCommand, "command is ExampleCommand");

            command.Dispose();
        }

        private void TestArrayPropertySet()
        {
            StandardOptionParser parser = new StandardOptionParser("example");
            CommandFactory factory = new CommandFactory(typeof(ExampleCommand).Assembly, parser);
            string[] arguments = new string[]{"/r", "lib1", "/r", "lib2", "/r", "lib3"};
            ExampleCommand command = factory.Create(arguments) as ExampleCommand;

            Debug.Assert(command != null, "command != null");
            Debug.Assert(command.Reference.Count == 3, "command.Reference.Length == 3");
            Debug.Assert(command.Reference[0] == "lib1");
            Debug.Assert(command.Reference[1] == "lib2");
            Debug.Assert(command.Reference[2] == "lib3");

            command.Dispose();
        }

        private void TestOptionDelimiter()
        {
            string[][] toTest = new string[][] 
            {
                new string[]{"/user", "abc"},
                new string[]{"-user", "abc"},
                new string[]{"\x2013user", "abc"}
            };
            for (int i = 0; i < toTest.Length; i++)
            {
                StandardOptionParser parser = new StandardOptionParser("example");
                CommandFactory factory = new CommandFactory(typeof(ExampleCommand).Assembly, parser);
                string[] arguments = toTest[i];
                ExampleCommand command = factory.Create(arguments) as ExampleCommand;

                Debug.Assert(command.UserName == toTest[i][1], "command.UserName == toTest[i][1]");

                command.Dispose();

            }            
        }

        private void TestMultipleCommand()
        {
            StandardOptionParser parser = new StandardOptionParser();
            CommandFactory factory = new CommandFactory(Assembly.GetExecutingAssembly(), parser);
            string[] arguments = new string[]{"empty"};
            EmptyCommand command1 = factory.Create(arguments) as EmptyCommand;
            Debug.Assert(command1 != null, "command1 != null");
            command1.Dispose();

            arguments = new string[]{"return", "5"};
            ReturnCommand command2 = factory.Create(arguments) as ReturnCommand;
            Debug.Assert(command2 != null, "command1 != null");
            Debug.Assert(command2.ToReturn == 5, "command2.ToReturn == 5");
            command2.Dispose();
        }

        private void TestReturnCode()
        {
            StandardOptionParser parser = new StandardOptionParser();
            CommandFactory factory = new CommandFactory(Assembly.GetExecutingAssembly(), parser);
            string[] arguments = new string[]{"return", "55"};
            ReturnCommand command = factory.Create(arguments) as ReturnCommand;
            Debug.Assert(command != null, "command != null");
            command.Run();
            Debug.Assert(command.ReturnCode == 55, "command.ReturnCode == 55");
            command.Dispose();
            Debug.Assert(1==0);
        }

        private void TestCommandAlias()
        {
            StandardOptionParser parser = new StandardOptionParser();
            CommandFactory factory = new CommandFactory(this.GetType().Assembly, parser);

            string[] names = new string[]{"help", "/?", "-?"};
            foreach(string name in names)
            {
                string[] arguments = new string[]{name};
                Command command = factory.Create(arguments);
                Debug.Assert(command is TestHelpCommand, "command is TestHelpCommand");
                command.Dispose();
            }
        }

        class TestListener : TraceListener
        {

            public override void Write(string message)
            {

            }

            public override void WriteLine(string message)
            {
                
            }

            public override void Fail(string message, string detailMessage)
            {
                throw new TestException(message + "\r\n" + detailMessage);
            }
        }
	}
}
