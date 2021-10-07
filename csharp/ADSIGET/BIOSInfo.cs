using System;
using System.Management;
using System.Collections;
using System.DirectoryServices;
using System.Data.SqlClient;

namespace GetData
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class BIOSInfo
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			string domain;
			string machine;
			string user;
			string password;
			WbemScripting.SWbemLocator wmiObj;
			WbemScripting.SWbemServices wmiServiceObj;
			SelectQuery q = new SelectQuery("SELECT * FROM Win32_BIOS");	
			ConnectionOptions options = new ConnectionOptions();


			if (args.Length == 4) 
			{
					machine = args[0];
					domain = args[1];
					user = args[2];
					password = args[3];
							
				wmiObj = new WbemScripting.SWbemLocatorClass();
				wmiServiceObj = wmiObj.ConnectServer(machine, "", domain + "\\" + user, password, "", "", 0, null);
				//wmiServiceObj.Security_.ImpersonationLevel = 3;
				options.Username = domain + "\\" + user;
				options.Password = password;
				ManagementScope scope = new ManagementScope("\\\\" + machine + "\\root\\CIMV2");
				EnumerationOptions o = new EnumerationOptions();
				ManagementObjectSearcher s = new ManagementObjectSearcher(scope, q, o);
				ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher

					("Select * From Win32_BIOS");
				

				string txt = "";

           
				foreach (ManagementObject SystemProp in s.Get())

				{

					txt = "Machine= " + machine + "\tSerialNumber= "+ SystemProp["SerialNumber"] + "\t" 
					+ "SMBIOSBIOSVersion= " + SystemProp["SMBIOSBIOSVersion"];
					txt = txt + "\r\n";
					Console.WriteLine(txt);
				}

				Console.Read();
			}
			else
			{
//				Console.WriteLine("Incorrect number of parameters");
				try
			{
				SqlConnection thisConnection = new SqlConnection(
					@"Data Source=SUSIC-DEV;" + 
					"Integrated Security=true;" +
					"Initial Catalog=MachineLib;" +
					"Connect Timeout=5");
				thisConnection.Open();
				SqlCommand thisCommand = thisConnection.CreateCommand();
				thisCommand.CommandText = "Select distinct Name from Machines where Name not like '%JOSH%'";
				SqlDataReader thisReader = thisCommand.ExecuteReader();
				while (thisReader.Read())
				{
					//Console.WriteLine("\t{0}",  thisReader["Name"]);

					machine = (string) thisReader["Name"];
					domain = "smx";
					user = "asttest";
					password = "Cecrops#01";
					options.Username = domain + "\\" + user;
					options.Password = password;
					try 
					{
						ManagementScope scope = new ManagementScope("\\\\" + machine + "\\root\\CIMV2");
						EnumerationOptions o = new EnumerationOptions();
						ManagementObjectSearcher s = new ManagementObjectSearcher(scope, q, o);
						ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher

							("Select * From Win32_BIOS");		

					string txt = "";

           
					foreach (ManagementObject SystemProp in s.Get())

					{

						txt = "Machine= " + machine + "\tSerialNumber= "+ SystemProp["SerialNumber"] + "\t" 
							+ "SMBIOSBIOSVersion= " + SystemProp["SMBIOSBIOSVersion"];
						txt = txt + "\r\n";
						Console.WriteLine(txt);
					}
					}
					catch (Exception e)
					{
						Console.WriteLine (e.Message);
					}

				}
				thisReader.Close();

				thisConnection.Close();

				String str = Console.ReadLine();
			}
				catch (SqlException e)
				{
					Console.WriteLine(e.Message);
				}
			//Console.WriteLine("Usage : getdata <machinename> <domain> <user> <password>");

				Console.Read();

				return;
			}	



		
		}
	}
}





