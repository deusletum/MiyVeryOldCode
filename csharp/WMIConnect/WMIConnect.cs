using System;
using System.Collections;
using System.IO;
using System.Management;

namespace WMIConnect
{
	class WMIInfo
	{
		private Hashtable Values;

		/// <summary>
		/// default constructor
		/// </summary>
		public WMIInfo()
		{
		}

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ClassName"></param>
		/// <param name="Properties"></param>
		/// <returns></returns>
		public Hashtable GetValues(string ClassName, string[] Properties)
		{
			string WQL = "SELECT * FROM " + ClassName;
			string Namespace;
			
			if(ComputerName == "")
			{
				Namespace = "\\\\" + ComputerName + "\\root\\CIMv2";
			}
			else
			{
				Namespace = "root\\CIMv2";
			}

			ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher(Namespace, WQL);

			foreach(ManagementObject Info in WMISearcher.Get())
			{
				foreach(string Prop in Properties)
				{
					this.Values.Add(Prop, Info[Prop]);
				}
			}
			return this.Values;
		}

		/// <summary>
		/// Remote Computer Name.
		/// </summary>
		public string ComputerName
		{
			get
			{
				return ComputerName;
			}
			set
			{
				ComputerName = value;
			}
		}
	}
}
