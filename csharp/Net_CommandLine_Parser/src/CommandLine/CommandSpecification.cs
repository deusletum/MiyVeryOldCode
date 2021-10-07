using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace MS.CommandLine
{
    /// <summary>
    ///    The CommandSpecification is an object that describes a command.  It is
    ///    constructed from the attributes on the Command object.
    /// </summary>
    public class CommandSpecification
    {
        CommandAttribute commandAttribute;
        OptionSpecificationCollection optionSpecs;
        Type commandType;
        StringCollection commandAliases;

        /// <summary>
        ///    Constructs a command specification based on the type.
        /// </summary>
        /// <param name="type">
        ///    Type of command.
        /// </param>
        public CommandSpecification(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            this.commandType = type;

            object[] attributes = this.commandType.GetCustomAttributes(typeof(CommandAttribute), false);
            Debug.Assert(attributes != null, "attributes != null");
            Debug.Assert(attributes.Length > 0, "attributes.Length > 0");

            this.commandAttribute = attributes[0] as CommandAttribute;

            this.optionSpecs = new OptionSpecificationCollection();
            this.optionSpecs.LoadFromType(this.commandType);
            //
            // Add any aliases of the command.
            //
            this.commandAliases = new StringCollection();
            object[] aliasAttributes = this.commandType.GetCustomAttributes(typeof(CommandAliasAttribute), true);
            foreach(CommandAliasAttribute aliasAttribute in aliasAttributes)
            {
                commandAliases.Add(aliasAttribute.Alias);
            }
        }

        /// <summary>
        ///    Gets whether this command allows unnamed options.
        /// </summary>
        public bool AllowNoNameOptions
        {
            get
            {
                return this.commandAttribute.AllowNoNameOptions;
            }
        }

        /// <summary>
        ///    Command name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.commandAttribute.Name;
            }
        }

        /// <summary>
        ///    Usage for command.
        /// </summary>
        public string GeneralInformation
        {
            get
            {
                return this.commandAttribute.GeneralInformation;
            }
        }

        public string BriefDescription
        {
            get
            {
                return this.commandAttribute.BriefDescription;
            }
        }

        /// <summary>
        ///    Gets one line description of usage.
        /// </summary>
        public string BriefUsage
        {
            get
            {
                return this.commandAttribute.BriefUsage;
            }
        }

        /// <summary>
        ///    Specification of command line options.
        /// </summary>
        public OptionSpecificationCollection OptionSpecifications
        {
            get
            {
                return this.optionSpecs;
            }
        }

        /// <summary>
        ///    Type of command.
        /// </summary>
        public Type CommandType
        {
            get
            {
                return this.commandType;
            }
        }

        /// <summary>
        ///    Gets the aliases of the command.
        /// </summary>
        public StringCollection CommandAliases
        {
            get
            {
                return this.commandAliases;
            }
        }

        /// <summary>
        ///    Prints full usage to the supplied text writer.
        /// </summary>
        /// <param name="writer">
        ///    Output stream to write usage to.
        /// </param>
        public void PrintFullUsage(TextWriter writer)
        {
            if(writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            writer.WriteLine(" {0} - {1}", this.Name, this.BriefDescription);
            writer.WriteLine();
            writer.WriteLine(FormatUtility.FormatStringForWidth(this.GeneralInformation, 2, 0, 80));
            writer.WriteLine(" Usage: {0}", this.BriefUsage);
            writer.WriteLine();
            if(this.OptionSpecifications.Count > 0)
            {
                writer.WriteLine(" Options:");
                writer.WriteLine();
                //
                // Sort option names.
                //
                string[] names = new string[this.OptionSpecifications.Count];
                for(int i=0 ; i < names.Length ; i++)
                {
                    names[i] = this.OptionSpecifications[i].OptionName;
                }
                Array.Sort(names);

                foreach (string name in names)
                {
                    OptionSpecification spec = this.OptionSpecifications[name];
                    if(spec.Description != null)
                    {
                        string description = string.Format(CultureInfo.CurrentCulture, spec.Description, spec.OptionName);
                        writer.WriteLine(FormatUtility.FormatStringForWidth(description, 2, 2, 80));
                    }
                }
            }
            
        }
    }
}
