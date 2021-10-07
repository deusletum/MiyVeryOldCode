using System;
using System.Collections;
using System.IO;
using System.Management;


namespace ProcessMON
{
	class WMIClasses
	{
		public void Get()
		{
			ManagementClass newClass = new ManagementClass();
			EnumerationOptions options = new EnumerationOptions();
			options.EnumerateDeep = false;
		
			foreach(ManagementObject SubClass in newClass.GetSubclasses(options)) 
			{
				Console.WriteLine("Class = " + SubClass["__Class"]);

				try
				{
					string ClassName = SubClass["_Class"];
					ManagementObject ManObj = new ManagementObject(ClassName);
					PropertyDataCollection PropData = ManObj.Properties;
					foreach(PropertyData Prop in PropData)
					{
						Console.WriteLine("Property = " + Prop.Name);
					}
				}
				catch(ManagementException e)
				{
//					if(e.Message.ToString() == "Not found")
//					{
//						Console.WriteLine("There are no propeties for Class " + SubClass["_Class"]);
//					}
					Console.WriteLine(e.Message);
				}

			}
	}
}
	/// <summary>
	/// Connect to WMI
	/// </summary>
	class Connect
	{
		/// <summary>
		/// default constructor
		/// </summary>
		public Connect()
		{
		}

		/// <summary>
		/// Overloaded constructor
		/// </summary>
		/// <param name="Scope">System.Management.ManagementScope Object</param>
		/// <param name="Query">System.Management.ObjectQuery Object</param>
		public Connect(ManagementScope Scope, ObjectQuery Query)
		{
			this.Scope = Scope;
			this.Query = Query;
		}
		
		// Private member vars
		private ManagementScope Scope;
		private ObjectQuery Query;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Namespace">WMI NameSpace. Example: "root\cimv2"
		///	</param>
		///	<example>"root\cimv2"</example>
		/// <param name="WMIQuery">WQL WMI Query. Example: "SELECT * FROM WIN32_DISKDRIVE"
		///	</param>
		///	<example>"select * from win32_diskdrive"</example>
		/// <returns>ManagementObjectSearcher object</returns>
		public ManagementObjectSearcher ConectToWMI(string Namespace, string WMIQuery)
		{
			ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher(Namespace, WMIQuery);
			return WMISearcher;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns>ManagementObjectSearcher object</returns>
		public ManagementObjectSearcher ConectToWMI()
		{
			ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher(Scope, Query);
			return WMISearcher;
		}
	
	}
	/// <summary>
	/// 
	/// </summary>
	class Start
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
//			Connect con = new Connect();
//			ManagementObjectSearcher WMI = con.ConectToWMI("root/cimv2", "SELECT * FROM WIN32_DISKDRIVE");
//			foreach (ManagementObject disk in WMI.Get())
//			{
//				Console.WriteLine("{0}", disk["Caption"]);
//			}
			WMIClasses ClassInfo = new WMIClasses();

			ClassInfo.Get();

			
		}
	}
}
