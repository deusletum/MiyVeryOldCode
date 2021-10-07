namespace MS.CommandLine
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    ///    Implementation of the standard option parser.
    /// </summary>
    public class StandardOptionParser : OptionParser
    {
        private const string NameGroup = "name";
        private const string PlusMinusGroup = "plusminus";

        // This regex includes the long dash that MS Word "autocorrects" the regular dash.
        // This is the cause of many copy/past errors from emails.  We treat this dash as a
        // regular dash so users don't have to worry about it.
        //
        private static readonly Regex optionRegex = new Regex(@"^(\/|\-|" + "\x2013" + @")(?<name>[^\+\-]+)(?<plusminus>\+|\-)?$");

        private string commandName;
        
        /// <summary>
        ///    Creates a standard option parser that treats the first argument as the 
        ///    name of the command to use.
        /// </summary>
        public StandardOptionParser() : this(null)
        {
        }

        /// <summary>
        ///    Creates a standard option parser that parses the command line for 
        ///    the specified command.
        /// </summary>
        /// <param name="commandName">
        ///    Name of the command.
        /// </param>
        public StandardOptionParser(string commandName)
        {
            this.commandName = commandName;
        }

        /// <summary>
        ///    Command name for multicommand parser is the first argument.
        /// </summary>
        /// <param name="arguments">
        ///    Command line arguments.
        /// </param>
        /// <returns>
        ///    Name
        /// </returns>
        public override string ParseCommandName(string[] arguments)
        {
            if(this.commandName == null)
            {
                // the first argument is the command name
                //
                if(arguments == null || arguments.Length == 0)
                {
                    throw new UsageException("No command is specified");
                }
                return arguments[0];
            }
            else
            {
                // this parser only parses for one type of command.
                //
                return this.commandName;
            }
        }

        /// <summary>
        ///    Parses command line.
        /// </summary>
        public override CommandOptionCollection Parse(string[] arguments, OptionSpecificationCollection optionSpecifications)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            if (optionSpecifications == null)
            {
                throw new ArgumentNullException("arguments");
            }

            CommandOptionCollection result = new CommandOptionCollection() ;
            // 
            // If no commandName is null then the first argument is the command name so skip it.
            //
            int startArgument = this.commandName==null ? 1 : 0;
            //
            // Loop through all arguments skipping the first because that is the command name.
            //
            for (int i = startArgument ; i < arguments.Length ; i++)
            {
                string optName = null ;
                string optValue = null ;
                OptionSpecification optSpec = null ;
                Match m = optionRegex.Match(arguments[i]);
                if (m.Success)
                {
                    string partialName = m.Groups[NameGroup].Value;
                    string plusMinus = (m.Groups[PlusMinusGroup].Success ? m.Groups[PlusMinusGroup].Value : null);
                    optName = null;
                    optValue = null ;

                    OptionSpecification[] matchingSpecs = optionSpecifications.GetPartial(partialName);
                    if (matchingSpecs.Length == 0)
                    {
                        throw new UsageException(string.Format(CultureInfo.CurrentCulture, "{0} is not a valid option.", arguments[i]));
                    }
                    else if (matchingSpecs.Length > 1)
                    {
                        throw new UsageException(string.Format(CultureInfo.CurrentCulture, "{0} is an ambiguous option.", arguments[i]));
                    }
                    else
                    {
                        Debug.Assert(matchingSpecs.Length == 1, "matchingSpecs.Length == 1");
                        optSpec = matchingSpecs[0] ;
                        optName = optSpec.OptionName;
                    }

                    if (optSpec.IsFinalOption)
                    {
                        Debug.Assert(!result.Contains(optSpec.OptionName), "!result.Contains(optSpec.OptionName)");
                        //
                        // all remaining options should be interpreted as the values of this option.  This
                        // logic short circuits the loop here by incrementing i and calling continue.
                        //
                        CommandOption newOption = new CommandOption(optSpec.OptionName);
                        for (i++ ; i < arguments.Length ; i++)
                        {
                            newOption.Add(arguments[i]);  
                        }
                        result.Add(newOption);
                        continue;
                    }
                    else if (optSpec.ValueType == OptionValueType.NoValue)
                    {
                        // if no value then it may be a +/- option
                        //
                        if (plusMinus != null)
                        {
                            optValue = plusMinus;
                        }
                    }
                    else if (i+1 < arguments.Length)
                    {
                        // Not at the end of the argument list.  Also account for MS Word 
                        // long dash here.
                        //
                        if (arguments[i+1].StartsWith("-") || 
                            arguments[i+1].StartsWith("/") ||
                            arguments[i+1].StartsWith("\x2013"))
                        {
                            // Next argument is not a value.
                            //
                            if (optSpec.ValueType == OptionValueType.ValueOptional)
                            {
                                // Value is optional so it is not an error to omit it.
                                //
                            }
                            else
                            {
                                // value is required but we haven't found one.
                                //
                                throw new UsageException(string.Format(CultureInfo.CurrentCulture, "No value specified for option {0}", arguments[i]));
                            }
                        }
                        else
                        {
                            // next argument is a value
                            Debug.Assert(optSpec.ValueType != OptionValueType.NoValue, "optSpec.ValueType != OptionValueType.NoValue");
                            optValue = arguments[i+1] ;
                            i++ ; // eat the next argument
                        }
                    }
                    else
                    {
                        // There are no more arguments. if a value was required it is an error.
                        // 
                        if (optSpec.ValueType == OptionValueType.ValueRequired)
                        {
                            throw new UsageException(string.Format(CultureInfo.CurrentCulture, "No value specified for option {0}", arguments[i]));
                        }
                    }	
                }
                else
                {
                    // Option is not related to a command line option.  Put it in the no-name category.
                    //
                    optName = CommandOption.NoName;
                    optValue = arguments[i] ;
                }

                Debug.Assert(optName != null, "optName != null" ) ;
                Debug.Assert(optName.Length > 0 || (optName.Length == 0 && optSpec == null),
                    "Option spec is specified for no-name options" ) ;

                CommandOption opt = null;
                opt = result[optName] ;
                if( opt == null )
                {
                    // This option has not been seen on the command line yet so
                    // add it.
                    //
                    opt = new CommandOption(optName) ;
                    result.Add(opt) ;
                }
                else if( optSpec != null && !optSpec.IsMultipleValue )
                {
                    // the option already exists in the option collection but
                    // there is an option spec that says the option should not have
                    // multiple values so throw an error.
                    //
                    throw new UsageException(
                        string.Format(CultureInfo.CurrentCulture,  "-{0} is specified more than once.", optName ) ) ;
                }
                //
                // set the value
                //
                if( optValue != null )
                {
                    opt.Add( optValue ) ;
                }
            }
            return result ;
        }

        public override void SetOptionProperty(object command, OptionSpecification optionSpecification, CommandOption option)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
			
            if (optionSpecification == null)
            {
                throw new ArgumentNullException("optionSpecification");
            }

            if (option == null)
            {
                throw new ArgumentNullException("option");
            }

            PropertyInfo propertyInfo = optionSpecification.RelatedProperty;
            Type targetType = optionSpecification.RelatedProperty.PropertyType;

            if(typeof(bool).IsAssignableFrom(targetType))
            {
                // special case handling for boolean properties.
                //
                if(option.Value == null || option.Value.Length == 0 || option.Value == "+")
                {
                    propertyInfo.SetValue(command, true, null);
                }
                else if(option.Value == "-")
                {
                    propertyInfo.SetValue(command, false, null);
                }
            }
            else if(typeof(IList).IsAssignableFrom(targetType))
            {
                IList list = optionSpecification.RelatedProperty.GetValue(command, null) as IList;
                if (list == null)
                {
                    throw new CommandException(string.Format(CultureInfo.CurrentCulture, "The collection property {0} needs to be initialized in the class constructor.", optionSpecification.RelatedProperty.Name));
                }
                // create an array of the correct type and convert each member.
                //
                Type elementType = optionSpecification.CollectionType;
                for(int i=0; i < option.Values.Count ; i++)
                {
                    object toAdd = Convert.ChangeType(option.Values[i], elementType, CultureInfo.CurrentCulture);
                    list.Add(toAdd);
                }
            }
            else
            {
                // Default is to use convert to change the type.
                //
                object optionValue = Convert.ChangeType(option.Value, targetType, CultureInfo.CurrentCulture);
                propertyInfo.SetValue(command, optionValue, null);

            }
        }
    }
}
