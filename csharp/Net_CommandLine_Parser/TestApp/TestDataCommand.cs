using System;

using MS.CommandLine;

namespace TestApp
{
	/// <summary>
	/// Summary description for TestDataCommand.
	/// </summary>
    [Command("empty",
         BriefUsage = "",
         BriefDescription="A command that does nothing.",
         GeneralInformation = @"This command does nothing."
     )]
    public class EmptyCommand : Command
	{
        protected override void RunImplementation()
        {
            // do nothing.
        }
	}

    [Command("return",
         BriefUsage = "<return code>",
         BriefDescription="A command that sets the return code.",
         GeneralInformation = @"This command sets the return code.",
         AllowNoNameOptions = true
         )]
    public class ReturnCommand : Command
    {
        private int toReturn;

        [Option(CommandOption.NoName,  
             OptionValueType.ValueRequired,
             Description = "The return code is specified as the first parameter."
        )]
        public int ToReturn
        {
            get
            {
                return this.toReturn;
            }
            set
            {
                this.toReturn = value;
            }
        }

        protected override void RunImplementation()
        {
            this.ReturnCode = toReturn;
        }

    }
}
