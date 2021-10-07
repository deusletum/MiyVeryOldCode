using System ;
using System.Collections.Specialized ;
using System.Collections ;
using System.Diagnostics ;

namespace MS.CommandLine
{

    /// <summary>
    ///    A command line option.
    /// </summary>
    public class CommandOption
    {
        string name ;
        private StringCollection values = new StringCollection();

        /// <summary>
        ///    Name used for options that are not associated with an option. 
        /// </summary>
        public const string NoName = "";

        /// <summary>
        ///    Constructs option with the specified name.
        /// </summary>
        /// <param name="n">
        ///    Option name.
        /// </param>
        public CommandOption(string name)
        {
            this.name = name ;
        }
		
        /// <summary>
        ///    Gets option name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name ;
            }
        }

        /// <summary>
        ///    Adds value to option.
        /// </summary>
        /// <param name="val">
        ///    Value to add.
        /// </param>
        public void Add(string optionValue)
        {
            this.values.Add(optionValue);
        }
		
        /// <summary>
        ///    Gets option value.  First option value if option
        ///    has more than one value.
        /// </summary>
        public string Value
        {
            get
            {
                if( this.values.Count > 0 )
                {
                    return this.values[0] ;
                }
                else
                {
                    return string.Empty ;
                }
            }
        }

        /// <summary>
        ///    Gets all values for option.
        /// </summary>
        public StringCollection Values
        {
            get
            {
                return this.values;
            }
        }

        /// <summary>
        ///    Gets value at the specified index.
        /// </summary>
        public string this[int index]
        {
            get
            {
                return (string)this.values[index] ;
            }
        }
    }


}
