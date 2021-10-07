using System;
using System.Reflection;

namespace MS.CommandLine
{
	/// <summary>
	///    Base class for what parses the command line.
	/// </summary>
	public abstract class OptionParser
	{
        /// <summary>
        ///    Gets the command name based on command line arguments.
        /// </summary>
        /// <param name="arguments">
        ///    Command line arguments.
        /// </param>
        /// <returns>
        ///    Command name.
        /// </returns>
        public abstract string ParseCommandName(string[] arguments);

        /// <summary>
        ///    Parses the command line based on the options specifications.
        /// </summary>
        /// <param name="arguments">
        ///    Command line arguments.
        /// </param>
        /// <param name="optionSpecifications">
        ///    Option specifications.
        /// </param>
        /// <returns></returns>
		public abstract CommandOptionCollection Parse(string[] arguments, OptionSpecificationCollection optionSpecifications);

        /// <summary>
        ///    This is called to have the parser set the value of an option on a command.
        /// </summary>
        /// <param name="command">
        ///    Command.
        /// </param>
        /// <param name="option">
        ///    Option.
        /// </param>
        /// <param name="optionSpecification">
        ///    Option specification.
        /// </param>
        public abstract void SetOptionProperty(object command, OptionSpecification optionSpecification, CommandOption option);
        
	}
}
