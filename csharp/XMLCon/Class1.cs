using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace XMLCon
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				XPathDocument doc = new XPathDocument(@"C:\Documents and Settings\deangj\My Documents\Visual Studio Projects\XMLCon\bin\Debug\Expense_Reports.xml");
				XPathNavigator nav = doc.CreateNavigator();
				XPathNodeIterator iter = nav.Select("/Expense_Reports/Fixed_Asset");

				while(iter.MoveNext())
				{
					Console.WriteLine(" {0} ", iter.Current.Value);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("There was an error: {0}",e.ToString());
			}

			
		}
	}
}
