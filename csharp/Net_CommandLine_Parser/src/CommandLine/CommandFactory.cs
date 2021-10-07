using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace MS.CommandLine
{
	/// <summary>
	///    The CommandFactory is used to create Command objects.
	/// </summary>
	public class CommandFactory
	{
        #region Fields

        private CommandSpecificationCollection commands;
        private OptionParser argumentParser;

        #endregion

        #region Constructors

        /// <summary>
        ///    Creates a command factory to create commands contained in the
        ///    specified assembly.
        /// </summary>
        /// <param name="commandAssembly">
        ///    Assembly containing commands.
        /// </param>
        public CommandFactory(Assembly commandAssembly, OptionParser parser)
            : this(new Assembly[]{commandAssembly}, parser)
        {
        }

        /// <summary>
        ///    Creates a command factory that creates commands contained in the
        ///    specified assemblies.
        /// </summary>
        /// <param name="commandAssemblies">
        ///    Assemblies containing commands.
        /// </param>
        public CommandFactory(Assembly[] commandAssemblies, OptionParser parser)
        {
            this.argumentParser = parser;
            LoadCommandSpecification(commandAssemblies);

        }

        #endregion

        #region Properties

        /// <summary>
        ///    Gets the commands available in the factory.
        /// </summary>
        public CommandSpecificationCollection Commands
        {
            get
            {
                return this.commands;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///    Creates a command given the command line arguments.
        /// </summary>
        /// <param name="arguments">
        ///    Command line arguments to the command.
        /// </param>
        /// <returns>
        ///    The newly created command.
        /// </returns>
        public Command Create(string[] arguments)
        {
            Command command = null;
            //
            // The first argument is the command name.
            //
            string commandName = this.argumentParser.ParseCommandName(arguments);
            CommandSpecification specification = this.Commands[commandName];
            if(specification == null)
            {
                throw new UsageException(string.Format(CultureInfo.CurrentCulture, "Unknown command '{0}'", commandName));
            }
            try
            {
                //
                // Create the command
                //
                Debug.Assert(typeof(Command).IsAssignableFrom(specification.CommandType), "typeof(Command).IsAssignableFrom(specification.CommandType)");
                command = Activator.CreateInstance(specification.CommandType) as Command;
                command.Specification = specification;
                //
                // Parse the command line options
                //
                CommandOptionCollection options = this.argumentParser.Parse(arguments, specification.OptionSpecifications);
                //
                // Instantiate defaults if the options were not specified in the command line.
                //
                foreach (OptionSpecification spec in specification.OptionSpecifications)
                {
                    if (spec.DefaultValue != null && !options.Contains(spec.OptionName))
                    {
                        CommandOption defaultOption = new CommandOption(spec.OptionName);
                        defaultOption.Add(spec.DefaultValue);
                        options.Add(defaultOption);
                    }
                }
                //
                // Enforce whether the command allows noname arguments.
                //
                if (!command.Specification.AllowNoNameOptions && options[CommandOption.NoName] != null)
                {
                    throw new UsageException(string.Format(CultureInfo.CurrentCulture, "Invalid option '{0}'", options[CommandOption.NoName].Value));
                }
                //
                // Set property options automatically and remove them from the option collection.
                //
                foreach(OptionSpecification spec in specification.OptionSpecifications)
                {
                    if(spec.RelatedProperty != null && options.Contains(spec.OptionName))
                    {
                        this.argumentParser.SetOptionProperty(command, spec, options[spec.OptionName]);
                        options.Remove(spec.OptionName);
                    }
                }
                //
                // Let the command interpret the remaining options.
                //
                command.Load(options);
                //
                // If command streams were not set by the load method use the stdout and stderr.
                //
                if(command.Output == null)
                {
                    command.Output = Console.Out ;
                }
                if(command.Error == null)
                {
                    command.Error = Console.Error ;
                }
            }
            catch(CommandException commandException)
            {
                if (commandException.Command == null)
                {
                    // make sure the command property of exception is set.
                    //
                    commandException.Command = command;
                }
                throw;
            }

            Debug.Assert(command != null, "command != null");
            return command;
        }
        
        /// <summary>
        ///    Loads all classes that have been marked as commands.
        /// </summary>
        /// <param name="commandAssemblies">
        ///    Array of assemblies to search.
        /// </param>
        private void LoadCommandSpecification(Assembly[] commandAssemblies)
        {
            this.commands = new CommandSpecificationCollection();

            foreach(Assembly assembly in commandAssemblies)
            {
                foreach(Type t in assembly.GetTypes())
                {
                    object[] attributes= t.GetCustomAttributes(typeof(CommandAttribute), false);
                    if(attributes.Length > 0)
                    {
                        CommandSpecification specification = new CommandSpecification(t);
                        this.Commands.Add(specification);
                    }
                }
            }
        }

        #endregion
	}
}
