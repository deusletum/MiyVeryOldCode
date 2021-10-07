using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace MS.CommandLine
{
    /// <summary>
    ///    Base exception class for all command exceptions.
    /// </summary>
    [Serializable] 
    public class CommandException : Exception
    {
        private Command command;

        /// <summary>
        ///    Default constructor.
        /// </summary>
        public CommandException()
        {
        }

        /// <summary>
        ///    Create exception with specified message.
        /// </summary>
        /// <param name="s">
        ///    Error message.
        /// </param>
        public CommandException(string message): base(message) 
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
        public CommandException(string message, Exception exception): base(message, exception) 
        {
        }

        /// <summary>
        ///    Create exception with serialization info.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CommandException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }

        /// <summary>
        ///     Create a command exception for the specified command.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="command"></param>
        public CommandException(string message, Command command) : base(message)
        {
            this.command = command;
        }

        /// <summary>
        ///    Gets or sets the command associated with this exception.
        /// </summary>
        public Command Command
        {
            get
            {
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        /// <summary>
        ///    Gets object for serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData (info, context);
        }

    }
}
