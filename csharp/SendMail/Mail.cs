using System;
using System.Web.Mail;

namespace SendMail
{
	class usage
	{
		public void DisplayUsage()
		{
			Console.WriteLine("Usage SendMail.exe <to> <from> <subject> <body>");
			Console.WriteLine("<to> the email address of the person or personds you are sending mail to");
			Console.WriteLine("use a comma to sepate the email address");
			Console.WriteLine("<from> your email address");
			Console.WriteLine("<subject> subject of your email address");
			Console.WriteLine("<body> the test of the email");
			Console.WriteLine("Example One");
			Console.WriteLine("SendMail.exe SomeOne@Someplace.com Me@myPlace.com Hello \"How are you?\"");
			Console.WriteLine
				("SendMail.exe SomeOne@Someplace.com,SomeOther@Someplace.com Me@myPlace.com Hi hello");

		}
	}

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Start
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				try
				{
					MailMessage Message = new MailMessage();

					Message.To = args[0];
					Message.From = args[1];
					Message.Subject = args[2];
					Message.Body = args[3];
					try
					{
						SmtpMail.SmtpServer = "smarthost.redmond.corp.microsoft.com";
						SmtpMail.Send(Message);
					}
					catch(System.Web.HttpException ehttp)
					{
						Console.WriteLine("{0}", ehttp.Message);
						Console.WriteLine("Here is the Full Message output");
						Console.Write("{0}", ehttp.ToString());
					}
				}
				catch(IndexOutOfRangeException)
				{
					usage use = new usage();
					use.DisplayUsage();
				}
			}
			catch(System.Exception e)
			{
				Console.WriteLine("Unknown Exception occurred {0}", e.Message);
				Console.WriteLine("Here is the Full Message output");
				Console.WriteLine("{0}", e.ToString());
			}


		}
	}
}
