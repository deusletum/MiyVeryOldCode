using System;

namespace MS.CommandLine
{
	/// <summary>
	///    The command alias attribute is used to indicate that a command
	///    can be referred to by more than one name.  This allows aliasing 
	///    a number of commands to the same command object.
	/// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public sealed class CommandAliasAttribute : Attribute
    {
        private string alias;

        /// <summary>
        ///    Constructs an alias attribute with the specified alias.
        /// </summary>
        /// <param name="alias">
        ///    Alias for command.
        /// </param>
        public CommandAliasAttribute(string alias)
		{
			this.alias = alias;
		}

        /// <summary>
        ///    Gets the alias.
        /// </summary>
        public string Alias
        {
            get
            {
                return this.alias;
            }
        }
	}
}
