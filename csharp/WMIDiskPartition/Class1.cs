using System;
using System.Management;

namespace WMIDiskPartition
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
			ManagementObjectSearcher diskDriveSearcher = new ManagementObjectSearcher(@"root\cimv2", 
					"select * from win32_diskdrive", null);

			ManagementObjectSearcher partitionSearcher = new ManagementObjectSearcher();
					partitionSearcher.Scope = new ManagementScope(@"root\cimv2");

			foreach (ManagementObject wmiDiskDrive in diskDriveSearcher.Get())
			{
				Console.WriteLine("Partitions of " + wmiDiskDrive["deviceid"] + ":");
				partitionSearcher.Query = new ObjectQuery("WQL", "associators of {" +
				wmiDiskDrive.Path.RelativePath + "} where assocclass = win32_diskdrivetodiskpartition");

				foreach (ManagementObject wmiPartition in partitionSearcher.Get())
				{
					Console.WriteLine("  " + wmiPartition["deviceid"]);
				}
			}
		}
	}
}
