using System;
using System.Management;

namespace WMINicTest
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
			char Quote = (char)34;
			string WMIQuery = "Select * From Win32_NetworkAdapter Where AdapterType=" 
				+ Quote + "Ethernet 802.3" + Quote;
			
			//Console.WriteLine("{0}", WMIQuery);

			ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher
				(WMIQuery);
			
			foreach (ManagementObject NetCard in WMISearcher.Get())
			{
//				foreach (System.Management.PropertyData Prop in NetCard.Properties)
//				{
//					Console.WriteLine("{0}",Prop.Name.ToString());
//					Console.WriteLine("{0}", NetCard[Prop.Name.ToString()]);			
					Console.WriteLine("{0}", NetCard["Caption"]);
					Console.WriteLine("{0}", NetCard["Speed"]);
					Console.WriteLine("{0}", NetCard["Name"]);
//				}
			}
		}
	}
}
