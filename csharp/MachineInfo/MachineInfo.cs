using System;
using System.IO;
using System.Management;
using System.Net;
using System.Xml;

namespace Microsoft.SMX.MachineInfo
{
	class MachineLib
	{

		private string DitsServer = "acdits";
		private string GetWeb =  "MachineList.asp";
		private string UpdateWeb = "MachineUpdate.asp";
		private char Quote = (char)34;

		/// <summary>
		/// Get machine information from MachineLib
		/// </summary>
		/// <param name="ComputerName">Name of the computer</param>
		/// <returns></returns>
		public XmlDocument GetMachineInfo(string ComputerName)
		{
			string GetWebSite = @"http://" + DitsServer + "/" + GetWeb;

			try
			{
				string XMLArgs = @"<root><MACHINE name=" + Quote + 
					ComputerName + Quote + "@/><root>";
				
				//Talk to the web site
				WebRequest WebList = WebRequest.Create(GetWebSite);
				WebList.Method = "POST";
				WebList.Credentials = CredentialCache.DefaultCredentials;

				StreamWriter StreamList = new StreamWriter(WebList.GetRequestStream());
				StreamList.Write(XMLArgs);
				StreamList.Close();

				//Create a XML Doc from the reponse of the webrequest
				XmlDocument DocList = new XmlDocument();
				DocList.Load(WebList.GetResponse().GetResponseStream());

				return DocList;

			}
			catch (WebException WebExcept)
			{
				string Error = "<error>" + WebExcept.Message + "</error>";
				XmlDocument XMLErr = new XmlDocument();
				XMLErr.LoadXml(Error);
				return XMLErr;
			}
		}
//		public XmlDocument AddMachineInfo(XmlDocument ComputerInfo)
//		{
//		}
//		public XmlDocument AddMachineInfo(XmlDocument ComputerInfo, string Action)
//		{
//		}
	}

	class WMIInfo
	{
//		public XmlDocument GetMachineInfo()
//		{
//		}
//		public XmlDocument GetMachineInfo(string ComputerName, string UserName, String Password)
//		{
//		}
	}

	class MachineInfo
	{

		[STAThread]
		static void Main(string[] args)
		{
			string Computer = "dev-deangj01";
			MachineLib Lib = new MachineLib();
			XmlDocument ListXML = new XmlDocument();
			ListXML = Lib.GetMachineInfo(Computer);

			Console.WriteLine(ListXML.OuterXml);
		}
	}
}
