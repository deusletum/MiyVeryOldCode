using System;
using System.Runtime.Serialization;

namespace MS.CommandLine
{
	/// <summary>
	///    Thrown when command line usage is incorrect.
	/// </summary>
	[Serializable] 
	public class UsageException : CommandException
	{
        /// <summary>
        ///    Default constructor.
        /// </summary>
        public UsageException() {}

        /// <summary>
        ///    Create usage exception with specified message.
        /// </summary>
        /// <param name="s">
        ///    Error message.
        /// </param>
        public UsageException(string message): base(message) {}

        /// <summary>
        ///    Create usage exception with inner exception.
        /// </summary>
        /// <param name="s">
        ///    Error message.
        /// </param>
        /// <param name="e">
        ///    Inner exception.
        /// </param>
        public UsageException(string message, System.Exception exception): base(message,exception) {}

        /// <summary>
        ///    Constructor used by serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UsageException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }

	}
}
