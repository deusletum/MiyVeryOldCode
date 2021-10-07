
namespace MS.CommandLine
{
    using System;
    using System.IO ;

    /// <summary>
    ///    Base class for all commands.
    /// </summary>
    public abstract class Command : IDisposable
    {
        #region Fields

        private TextWriter commandError;
        private TextWriter commandOut;
        private int returnCode; 
        private CommandSpecification specification;

        #endregion

        #region Constructor/Destructor
		
        /// <summary>
        ///    Finalizer for command object.  Calls
        ///    dispose.
        /// </summary>
        ~Command()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        ///    Gets the return code for the command.
        /// </summary>
        public int ReturnCode
        {
            get
            {
                return this.returnCode ;
            }
            set
            {
                this.returnCode = value;
            }
        }

        /// <summary>
        ///    Gets or sets the command output.  Output of the command is written to this text writer.
        /// </summary>
        public TextWriter Output
        {
            get
            {
                return this.commandOut ;
            }
            set
            {
                this.commandOut = value ;
            }
        }

        /// <summary>
        ///    Gets or sets the command error stream.  Errors are output to this.
        /// </summary>
        public TextWriter Error
        {
            get
            {
                return commandError ;
            }
            set
            {
                commandError = value ;
            }
        }

        /// <summary>
        ///    Gets or sets the commands specification.
        /// </summary>
        public CommandSpecification Specification
        {
            get
            {
                return this.specification;
            }
            set
            {
                this.specification = value;
            }
        }

        #endregion

        /// <summary>
        ///    Runs command given the command line options.
        /// </summary>
        public void Run() 
        {
            if (this.commandOut == null)
            {
                throw new CommandException( "No output writer has been given for the command." ) ;
            }

            RunImplementation();
        }


        /// <summary>
        ///    Load the command from settings on the command line.
        /// </summary>
        /// <param name="options"></param>
        public void Load(CommandOptionCollection options)
        {
            // if not displaying help load the implementation.
            //
            LoadImplementation(options);
        }

        /// <summary>
        ///    Subclasses imlement this method for the run implementation.
        /// </summary>
        protected abstract void RunImplementation() ;

        /// <summary>
        ///    Subclasses implement this method for the load implementation.
        /// </summary>
        /// <param name="options">
        ///    Command line options.
        /// </param>
        protected virtual void LoadImplementation(CommandOptionCollection options)
        {
        }

        /// <summary>
        ///    Dispose method used by finalizer and Dispose method.
        /// </summary>
        /// <param name="suppressFinalize"></param>
        protected virtual void Dispose(bool suppressFinalize)
        {
            if(suppressFinalize)
            {
                GC.SuppressFinalize(this);
            }
        }

        #region IDisposable Members

        /// <summary>
        ///    Dispose method used to implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
