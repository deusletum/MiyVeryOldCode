using System;

namespace MS.CommandLine
{
    /// <summary>
    ///    The command attribute is used to mark a class as a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandAttribute : Attribute
    {
        private string name;
        private string briefDescription;
        private string briefUsage;
        private string generalInformation;
        private bool   allowNoNameOptions;

        /// <summary>
        ///    Constructs a command attribute.
        /// </summary>
        /// <param name="name">
        ///    Name of the command.
        /// </param>
        /// <param name="briefUsage">
        ///    One line description of the command.
        /// </param>
        public CommandAttribute(string name)
        {
            this.name = name;
            this.briefUsage = string.Empty;
            this.generalInformation = string.Empty;
        }

        /// <summary>
        ///    Gets and sets the name of the command.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }
        
        /// <summary>
        ///    Gets or sets a brief description of the command.
        /// </summary>
        public string BriefDescription
        {
            get
            {
                return this.briefDescription;
            }
            set
            {
                this.briefDescription = value;
            }
        }

        /// <summary>
        ///    One line description of the command.
        /// </summary>
        public string BriefUsage
        {
            get
            {
                return this.briefUsage;
            }
            set
            {
                this.briefUsage = value;
            }
        }

        /// <summary>
        ///    Gets or sets the detailed usage for the command.
        /// </summary>
        public string GeneralInformation
        {
            get
            {
                return this.generalInformation;
            }
            set
            {
                this.generalInformation = value;
            }
        }

        /// <summary>
        ///    Gets or sets whether unnamed options are allowed.
        /// </summary>
        public bool AllowNoNameOptions
        {
            get
            {
                return this.allowNoNameOptions;
            }
            set
            {
                this.allowNoNameOptions = value;
            }
        }
    }
}
