using System;
using System.Diagnostics;
using System.Reflection;

namespace MS.CommandLine
{
	/// <summary>
	///    This is a base class that applications can derive from to get a "help" 
	///    command.
	/// </summary>
	public class HelpCommandBase : Command
	{
        CommandFactory factory;
        string commandName;

        /// <summary>
        ///    Default constructor.
        /// </summary>
        public HelpCommandBase()
            : this(null)
        {
        }

        /// <summary>
        ///    Create a help command that provides help for command objects in
        ///    the specified assembly.  If the assembly is null then the assembly
        ///    of the help object will be used.
        /// </summary>
        /// <param name="commandAssembly">
        ///    Assembly that contains commands.
        /// </param>
        public HelpCommandBase(Assembly commandAssembly)
        {
            if (commandAssembly != null)
            {
                factory = new CommandFactory(commandAssembly, new StandardOptionParser());
            }
            else
            {
                // use the assembly for this object.  If it is a derived class it will be the assembly of the 
                // derived class.
                //
                factory = new CommandFactory(this.GetType().Assembly, new StandardOptionParser());
            }
            
            Debug.Assert(factory != null, "factory != null");
        }

        /// <summary>
        ///    This is a specific command that will provide help for.
        /// </summary>
        [Option(CommandOption.NoName, OptionValueType.NoValue)]
        public string CommandName
        {
            get
            {
                return this.commandName;
            }
            set
            {
                this.commandName = value;
            }
        }

        /// <summary>
        ///    Implementation help command.  If a specific command is specified then help for 
        ///    that command will be output.  If not command is specified then a list of all commands
        ///    will be written to the output.
        /// </summary>
        protected override void RunImplementation()
        {
            Output.WriteLine();
            if(commandName != null && factory.Commands[this.commandName] != null)
            {
                factory.Commands[this.commandName].PrintFullUsage(Output);
            }
            else
            {
                if (this.commandName != null && factory.Commands[this.commandName] == null)
                {
                    Output.WriteLine("Unknown command '{0}'", this.commandName);
                    Output.WriteLine();
                }
                Output.WriteLine("The following commands are available");
                Output.WriteLine();
                //
                // Sort command names.
                //
                string[] names = new string[factory.Commands.Count];
                for(int i=0 ; i < names.Length ; i++)
                {
                    names[i] = factory.Commands[i].Name;
                }
                Array.Sort(names);

                foreach(string name in names)
                {
                    CommandSpecification spec = factory.Commands[name];
                    Output.WriteLine("  {0} - {1}", spec.Name, spec.BriefDescription);
                }
            }
        }
	}
}
