using System;
using System.Collections;
using System.Management;

namespace NWmi
{
	/// <summary>
	/// This class provide network adaptor information
	/// such ass ip address, subnet mask, and nic index ID
	/// </summary>
	class NetworkAdaptor
	{
		/// <summary>
		/// Get NIC info and returns it in a hastable
		/// </summary>
		/// <param name="ComputerName">Name of the computer you want nic info for</param>
		/// <returns>hastable with nic info</returns>
		public Hashtable GetNICinfo(string ComputerName)
		{
			char Quote = (char)34;
			string WMIQuery = "Select * From Win32_NetworkAdapterConfiguration Where IPEnabled=" 
				+ Quote + "True" + Quote;
			string WMIScope = @"\\" + ComputerName + @"\root\CimV2";

			ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher
				(WMIScope,WMIQuery);
			
			Hashtable MyHash = new Hashtable();
			foreach (ManagementObject NetCard in WMISearcher.Get())
			{
				if (NetCard["DHCPEnabled"].ToString() != "True")
				{
					MyHash.Add("LBNIC", NetCard["Index"]);
					string[] IPS = (string[])NetCard["IPAddress"];
					string[] Subnets = (string[])NetCard["IPSubnet"];
					
					int i=0;
					while (i < IPS.Length - 1)
					{
						MyHash.Add("STATIC_IP" + i, IPS[i]);
						MyHash.Add("STATIC_SUBNET" + i, Subnets[i]);
						i++;
					}
				}
				else
				{
					MyHash.Add("ManagementNIC", NetCard["Index"]);
				}
			}
			return MyHash;
		}
		/// <summary>
		/// Get NIC info and returns it in a hastable
		/// </summary>
		/// <returns>hastable with nic info</returns>
		public Hashtable GetNICinfo()
		{
			char Quote = (char)34;
			string WMIQuery = "Select * From Win32_NetworkAdapterConfiguration Where IPEnabled=" 
				+ Quote + "True" + Quote;

			ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher
				(WMIQuery);

			Hashtable MyHash = new Hashtable();
			foreach (ManagementObject NetCard in WMISearcher.Get())
			{
				if (NetCard["DHCPEnabled"].ToString() != "True")
				{
					MyHash.Add("LBNIC", NetCard["Index"]);
					string[] IPS = (string[])NetCard["IPAddress"];
					string[] Subnets = (string[])NetCard["IPSubnet"];
					
					int i=0;
					while (i < IPS.Length - 1)
					{
						MyHash.Add("STATIC_IP" + i, IPS[i]);
						MyHash.Add("STATIC_SUBNET" + i, Subnets[i]);
						i++;
					}
				}
				else
				{
					MyHash.Add("ManagementNIC", NetCard["Index"]);
				}
			}
			return MyHash;
		}
	}
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// Displays usage
		/// </summary>
		static void Usage()
		{
			Console.WriteLine("AC Command Line testing");
			Console.WriteLine("This test asumes:");
			Console.WriteLine("that you have two computers");
			Console.WriteLine("that both computers are on the SMX.NET network and domain");
			Console.WriteLine("that both computers have two NIC cards,");
			Console.WriteLine("that both computers Have run ipset get to get static IP Addresses");
			Console.WriteLine("that both computers Have been built using Oasis");
			Console.WriteLine("that AC10 or above is installed on both computers");
			Console.WriteLine();
			Console.WriteLine("Usage:");
			Console.WriteLine("CLTtests.exe computer1 computer2");
			Console.WriteLine("Example:");
			Console.WriteLine("CLTtests.exe deangj02e deangj03e");
			Environment.Exit(0);
		}
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Usage();
			}
			if (args[0] == "/?")
			{
				Usage();
			}
			if (args[0] == "?")
			{
				Usage();
			}
			string Controller = args[0];

			Hashtable MyHash = new Hashtable();
			NetworkAdaptor N = new NetworkAdaptor();

			MyHash = N.GetNICinfo(Controller);
			foreach (DictionaryEntry o in MyHash)
			{
				Console.WriteLine(o.Key + " " + o.Value);	
			}

		}
	}
}
