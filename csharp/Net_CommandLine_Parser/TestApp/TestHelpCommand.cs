using System;
using MS.CommandLine;

namespace TestApp
{
	/// <summary>
	///    An example help command.
	/// </summary>
    [Command
         (
         "help", 
         BriefDescription = "Show help for commands", 
         BriefUsage = @"help <command name>",
         AllowNoNameOptions = true
         )]
    [CommandAlias("/?")]
    [CommandAlias("-?")]
    public class TestHelpCommand : HelpCommandBase
	{
	}
}
