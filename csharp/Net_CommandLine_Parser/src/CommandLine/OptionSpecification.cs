using System;
using System.Collections.Specialized;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace MS.CommandLine
{
    /// <summary>
    ///    This class reprsents an option specification.  This class
    ///    is used to determine whether a command line option takes a
    ///    parameter or not.  Can also be used to make sure commands aren't
    ///    using the same parameter for different things.
    /// </summary>
    public class OptionSpecification
    {
        #region Fields

        private OptionAttribute attribute;
        private PropertyInfo relatedProperty;

        #endregion

        #region Constructors

        /// <summary>
        ///    Constructs an option spec.
        /// </summary>
        /// <param name="n">
        ///    Name of option.
        /// </param>
        /// <param name="multiValue">
        ///    If true then option can take multiple values.
        /// </param>
        /// <param name="vt">
        ///    What type of value is accepted for option.
        /// </param>
        public OptionSpecification(
            OptionAttribute attribute, 
            PropertyInfo relatedProperty)
        {
            this.attribute = attribute;
            this.relatedProperty = relatedProperty;
        }

        #endregion

        #region Properties

        /// <summary>
        ///    Returns the name of the option.
        /// </summary>
        public string OptionName
        {
            get 
            {
                return this.attribute.Name;
            }
        }

        /// <summary>
        ///    Get whether he command option takes a value with it.
        /// </summary>
        public OptionValueType ValueType 
        {
            get
            {
                return this.attribute.ValueType ;
            }
        }

        /// <summary>
        ///    Gets whether the option is the last option specified on the command line.  Everything
        ///    after it is its value(s).
        /// </summary>
        public bool IsFinalOption
        {
            get
            {
                return this.attribute.IsFinalOption;
            }
        }

        /// <summary>
        ///    Gets wether the option can be specified on the command line multiple times.
        /// </summary>
        public bool IsMultipleValue 
        {
            get
            {
                return this.attribute.IsMultipleValue ;
            }
        }

        /// <summary>
        ///    If the option is bound to a property this is the method info for that property.
        /// </summary>
        public PropertyInfo RelatedProperty
        {
            get
            {
                return this.relatedProperty;
            }
        }

        /// <summary>
        ///    Gets the default value of the option.  If the default is null then the option
        ///    has no default.
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return this.attribute.DefaultValue;
            }
        }

        /// <summary>
        ///    Gets a description of the option.
        /// </summary>
        public string Description
        {
            get
            {
                return this.attribute.Description;
            }
        }

        /// <summary>
        ///    Gets the type of the collection items if on a collection.
        /// </summary>
        public Type CollectionType
        {
            get
            {
                return this.attribute.CollectionType;
            }
        }

        #endregion
    }

}
