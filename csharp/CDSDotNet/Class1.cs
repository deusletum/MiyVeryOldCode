using System;

namespace CDSDotNet
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
			//Get CDS User info
			CDSComNET.Users CDS = new CDSComNET.UsersClass();
			string testaccount = @"smx\asttest";
			object account = (object)testaccount;
			string password = (string)CDS.GetPassword(ref account);

			Console.WriteLine("Password is: " + password);
		}
	}
}
