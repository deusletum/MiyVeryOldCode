using System;
using System.DirectoryServices;

namespace ADSI
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
			string Computer = "dev-deangj01";
			try
			{
				DirectoryEntry DirEnt = new DirectoryEntry();
				DirEnt.Path = ("LDAP://redmond.corp.microsoft.com");

				DirectorySearcher DirSearch = new DirectorySearcher(DirEnt);
				DirSearch.Filter = "(objectCategory=computer)";
				DirSearch.PropertiesToLoad.Add("name");

				SearchResultCollection Results = DirSearch.FindAll();

				ResultPropertyCollection ResultCollection;
				ResultCollection = Results;

				foreach (object Result in ResultCollection.PropertyNames)
				{
					foreach (object PropVal in ResultCollection.Properties[Result])
					{
						Console.WriteLine("{0}", PropVal);
					}				
				}
//
//				foreach (string PropName in DirEnt.Properties.PropertyNames)
//				{
//					foreach (object ADSIval in DirEnt.Properties[PropName])
//					{
//						Console.WriteLine("name=" + PropName + "  value=" + ADSIval );
//					}
//				}

//				DirectorySearcher src = new DirectorySearcher
//					(@"LDAP://redmond.corp.microsoft.com", "(objectCategory=computer)");
//				
//				foreach( SearchResult res in src.FindAll())
//				{
//					Console.WriteLine(res.Properties["Name"].Value);
//				}

			}
			catch (SystemException e)
			{
				Console.WriteLine("An Exception occured {0}", e.Message);
				Console.WriteLine("An Exception occured {0}", e.ToString());
			}
		}
	}
}
