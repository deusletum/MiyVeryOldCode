using System;

namespace MS.CommandLine
{
	/// <summary>
	///    The option attribute is used to identify command line options on command
	///    objects.
	/// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple=true)]
    public sealed class OptionAttribute : Attribute
	{
        string name;
        OptionValueType valueType;
        bool isMultipleValue;
        string defaultValue;
        bool isFinalOption;
        string description;
        Type collectionType;

        /// <summary>
        ///    Constructor for option attribute.
        /// </summary>
        /// <param name="name">
        ///    Name of option.
        /// </param>
        /// <param name="valueType">
        ///    Kind of value supplied to the option.
        /// </param>
        /// <param name="isMultipleValue">
        ///    Whether or not there can be multiple instances of this option on the command line.
        /// </param>
		public OptionAttribute(string name, OptionValueType valueType)
		{
			this.name = name;
            this.valueType = valueType;
		}

        /// <summary>
        ///    Name of the option.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        ///    The kind of value that is associated with the command line switch.
        /// </summary>
        public OptionValueType ValueType
        {
            get
            {
                return this.valueType;
            }
        }

        /// <summary>
        ///    Gets or sets whether there can be multiple instances of this option
        ///    on the command line.
        /// </summary>
        public bool IsMultipleValue
        {
            get
            {
                return this.isMultipleValue;
            }
            set
            {
                this.isMultipleValue = value;
            }
        }

        /// <summary>
        ///    Gets or sets the default value of the option.
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
            set
            {
                this.defaultValue = value;
            }
        }

        /// <summary>
        ///    If an option is marked as a final option it means the remaining options
        ///    after the option switch are values of this option and are not parsed
        ///    by the command line parser.
        /// </summary>
        public bool IsFinalOption
        {
            get
            {
                return this.isFinalOption;
            }
            set
            {
                this.isFinalOption = value;
            }
        }

        /// <summary>
        ///    The description of the option.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <summary>
        ///    If the option is a collection then this specifies
        ///    the type of objects in that collection.
        /// </summary>
        public Type CollectionType
        {
            get
            {
                return this.collectionType;
            }
            set
            {
                this.collectionType = value;
            }
        }
	}
}
