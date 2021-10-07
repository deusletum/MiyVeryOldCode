using System;
using System.IO;
using System.Collections.Specialized;

namespace MS.CommandLine
{
	// 
    // The command attribute marks the class as a command.  If a class is
    // not marked with this attribute, even if derives from Command it will
    // not be available.
    //
    [Command
     (   
         "example",
         //
         // The following optional members are for documentation purposes.  The command infrastructure
         // can automatically generate a help page based on this info.
         //
         BriefUsage = "example -o <outfile> <other options>",
         BriefDescription="An example command",
         GeneralInformation = @"It's always a good idea to add a general description
            of what the command does in the command attribute.  You don't need to worry
            about what it looks like here because all descriptions are formated to look
            nice on the command line.",
         //
         // By default parameters on the command line not related to options are not allowed.
         // set this attribute to true to allow unnamed parameters
         //
         AllowNoNameOptions = true
     )]
    //
    // The command alias attributes indicates that the same command might be referred to with different
    // names.  
    //
    [CommandAlias("eg")]
    //
    // Options specified on the class level are passed in to the LoadImplementation() method.
    //
    [Option(ExampleCommandOption.OutFile,  OptionValueType.ValueRequired, Description = "-{0} <outfile> : Write all output to a file")]
    [Option
        (ExampleCommandOption.Pass,  
         OptionValueType.ValueRequired,
         Description = "-{0} <options>: all of these options will be passed through to the process",
         //
         // If IsFinalOption is true then all command line arguments after the option are
         // treated as the value of this option.  This can be useful if you need to pass
         // parameters straight through to another process.
         //
         IsFinalOption = true)
    ]
    /// <summary>
    ///    This command is used as an example of how to write a command.  All commands
    ///    must derive from the Command base class.
    /// </summary>
    public class ExampleCommand : Command
	{
        private bool trusted;
        private string userName;
        private StringCollection reference = new StringCollection();
        private StringCollection pass;
        private StringCollection nonameOptions;
        
        /// <summary>
        ///    You need a default constructor for all commands.
        /// </summary>
		public ExampleCommand()
		{
		}

        // 
        // An option attribute on a property allows the command infrastructure
        // to automatically set the property if the option is specified on the
        // command line.
        //
        [Option(ExampleCommandOption.UserName,  
             OptionValueType.ValueRequired,
             Description = "-{0} <user name>: The user name to connect as.",
             //
             // A default value can be supplied.  This will
             // be used if the option is not specified on the command lien.
             //
             DefaultValue="me")]
        /// <summary>
        ///    An option at
        /// </summary>
        public string UserName
        {
            get
            {
                return this.userName;
            }
            //
            // If you attribute a property you need to make sure it has a set method.
            //
            set
            {
                this.userName = value;
            }
        }

        [Option(ExampleCommandOption.Trusted,  
             //
             // NoValue indicates the option does not take a parameter.
             //
             OptionValueType.NoValue,
             Description = "-{0}: if specified a trusted connection will be used.")]
        //
        // The parser does the value conversion when setting properties so it converts
        // /opt, /opt+ and /opt- appropriately if the property is boolean
        //
        public bool Trusted
        {
            get
            {
                return this.trusted;
            }
            set
            {
                this.trusted = value;
            }
        }

        // 
        // The automatic property set logic can tell that the property is an array and set
        // its value properly if the option can be specified multiple times on the command line.
        //
        [Option
             (ExampleCommandOption.Reference,  
             OptionValueType.ValueRequired,
             //
             // You can have a description for each option.  This will be used when displaying help to the user.
             // It is a format string and {0} gets replaced with the option name before it is printed.
             //
             Description = "-{0} <dll name>: Reference a dll",
             //
             // You can indicate that an option can be specified more than once on a command line
             // by setting IsMultipleValue = true
             //
             IsMultipleValue = true,
             CollectionType = typeof(string))
        ]
        public StringCollection Reference
        {
            get
            {
                return this.reference;
            }
        }

        /// <summary>
        ///    You don't have to implement this method if you get all your
        ///    command line data through properties.
        /// </summary>
        /// <param name="options">
        ///    These are the options on the class that were specified
        ///    on the command line.
        /// </param>
        protected override void LoadImplementation(CommandOptionCollection options)
        {
            if(options.Contains(ExampleCommandOption.OutFile))
            {
                try
                {
                    string filename = options[ExampleCommandOption.OutFile].Value;
                    this.Output = new StreamWriter(filename);
                }
                catch(IOException e)
                {
                    // If you detect invalid usage you can throw a usage exception.
                    //
                    throw new UsageException("Invalid file specified", e);
                }
                this.Error = this.Output;
            }
            if(options.Contains(ExampleCommandOption.Pass))
            {
                this.pass = options[ExampleCommandOption.Pass].Values;
            }
            //
            // NoName options are those that aren't related to any option.
            //
            if(options.Contains(CommandOption.NoName))
            {
                this.nonameOptions = options[CommandOption.NoName].Values;
            }
        }

        protected override void RunImplementation()
        {
            // Do all the work for the command in this 
            // The Output and Error properties are set with the output and error
            // streams respectively.
            //
            Output.WriteLine("UserName = '{0}' Trusted = {1}", this.userName, this.trusted);
            Output.WriteLine();
            if(this.pass != null)
            {
                Output.WriteLine("Passthrough options:");

                foreach(string s in this.pass)
                {
                    Output.WriteLine(s);
                }

                Output.WriteLine();
            }

            if(this.reference != null)
            {
                Output.WriteLine("Reference options:");
                foreach(string s in this.reference)
                {
                    Output.WriteLine(s);
                }

                Output.WriteLine();
            }

            if(this.nonameOptions != null)
            {
                Output.WriteLine("No name options:");
                foreach(string s in this.nonameOptions)
                {
                    Output.WriteLine(s);
                }

                Output.WriteLine();
            }
            Error.WriteLine("Whoops! something bad happened (just kidding)");
            //
            // Finally set the process exit code for the command.
            //
            this.ReturnCode = 1;
        }


	}

    /// <summary>
    ///    Its always a good idea to put your string constants in another class.  That
    ///    way you don't have to worry about mispelling them.  The compiler will catch
    ///    any errors.
    /// </summary>
    public sealed class ExampleCommandOption
    {
        public const string OutFile = "outfile";
        public const string UserName = "user";
        public const string Trusted = "trusted";
        public const string Reference = "ref";
        public const string Pass = "pass";
    }

    //
    // If you are using "foo.exe <command> <options>" style command line parsing you will
    // most likely want "foo.exe help" or "foo.exe /?" to display syntax for all the commands.  By
    // deriving from HelpCommandBase you can easily add this functionality to your command line tool.
    // Put the following definition in the same assembly as all your other commands to get this behavior.
    //
    [Command
         (
         "help", 
         BriefDescription = "Show help for commands", 
         BriefUsage = @"help <command name>",
         AllowNoNameOptions = true
         )]
    [CommandAlias("/?")]
    [CommandAlias("-?")]
    public class HelpCommand : HelpCommandBase
    {
    }
}
