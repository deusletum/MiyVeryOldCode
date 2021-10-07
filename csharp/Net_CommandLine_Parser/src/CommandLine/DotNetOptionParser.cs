using System;
using System.Reflection;

namespace MS.CommandLine
{
	/// <summary>
	///    Parses a command line using dot-net style parameters.
	/// </summary>
	public class DotNetOptionParser : OptionParser
	{
        private string commandName;

		public DotNetOptionParser(string commandName)
		{
			this.commandName = commandName;
		}

        public override CommandOptionCollection Parse(string[] arguments, OptionSpecificationCollection optionSpecifications)
        {
            CommandOptionCollection result = new CommandOptionCollection() ;
            for( int i = 0 ; i < arguments.Length ; i++ )
            {
                if( arguments[i][0]=='/' || arguments[i][0] == '-' )
                {
                    int colonIndex = arguments[i].IndexOf(':') ;
                    string optionName = null ;
                    string optionValue = null ;
                    if( colonIndex >= 0 )
                    {
                        optionName = arguments[i].Substring(1, colonIndex-1) ;
                        optionValue = arguments[i].Substring(colonIndex+1) ;
                    }
                    else
                    {
                        optionName = arguments[i].Substring(1) ;
                        optionValue = String.Empty ;
                    }
                    CommandOption opt = null ;
                    opt = result[optionName] ;
                    if( opt == null )
                    {
                        opt = new CommandOption(optionName) ;
                        result.Add(opt) ;
                    }
                    opt.Add(optionValue) ;
                }
            }
            return result ;
        }

        public override string ParseCommandName(string[] arguments)
        {
            return this.commandName;
        }

        public override void SetOptionProperty(object command, OptionSpecification optionSpecification, CommandOption option)
        {
            
        }
	}
}
