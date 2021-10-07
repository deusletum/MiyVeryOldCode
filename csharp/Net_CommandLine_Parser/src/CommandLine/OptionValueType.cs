using System;

namespace MS.CommandLine
{
    /// <summary>
    ///    Specifies the type of value a command line option can take.
    /// </summary>
    public enum OptionValueType
    {
        /// <summary>
        ///    Option cannot take a value.
        /// </summary>
        NoValue,
        /// <summary>
        ///    Value is required.
        /// </summary>
        ValueRequired,
        /// <summary>
        ///    Value is optional.
        /// </summary>
        ValueOptional
    }

}
