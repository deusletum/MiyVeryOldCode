using System;
using System.Runtime.Serialization;

namespace MS.CommandLine
{
	/// <summary>
	///    Exception thrown when there is an error loading the command option specifications.
	/// </summary>
	[Serializable] 
	public class OptionSpecificationException : CommandException
	{
        /// <summary>
        ///    Default constructor.
        /// </summary>
        public OptionSpecificationException()
        {
        }

        /// <summary>
        ///    Create exception with specified message.
        /// </summary>
        /// <param name="s">
        ///    Error message.
        /// </param>
        public OptionSpecificationException(string message): base(message) 
        {
        }

        /// <summary>
        ///    Create command line exception with inner exception.
        /// </summary>
        /// <param name="s">
        ///    Error message.
        /// </param>
        /// <param name="e">
        ///    Inner exception.
        /// </param>
        public OptionSpecificationException(string message, System.Exception exception): base(message, exception) 
        {
        }

        /// <summary>
        ///    Construtor used by serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected OptionSpecificationException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }

	}
}
