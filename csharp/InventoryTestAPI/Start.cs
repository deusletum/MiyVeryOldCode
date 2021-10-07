using System;
using System.IO;
using System.Net;
using System.Xml;

namespace InventoryTestAPI
{
	/// <summary>
	/// Summary description for Start.
	/// </summary>
	class Start
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
//			bool InDatabase;

			if (args.Length != 1)
			{
				Console.WriteLine("Don't Forget to put usage here!");
				Environment.Exit(1);
			}

			char Quote = (char)34;
			//string Domain = "SMX";
			//string Account = "asttest";

			object UserAccount = (object)@"SMX\asttest";
//			object CDSPassword;
//			string Password;

			string ComputerName = args[0];//"deangj02d";
			string DitsList = @"http://acdits/MachineList.asp";
//			string DitsUpdate = @"http://acdits/MachineUpdate.asp";

			try
			{

//				Password = (string)CDSPassword;
				string XMLArgList = @"<root><MACHINE name=" + Quote + 
					ComputerName + Quote + @"/></root>";

				//Find out if the computer is already in the database
				WebRequest WebList = WebRequest.Create(DitsList);
				WebList.Method = "POST";
				WebList.Credentials = CredentialCache.DefaultCredentials; 
			
				StreamWriter StreamList = new StreamWriter(WebList.GetRequestStream());
				StreamList.Write(XMLArgList);
				StreamList.Close();

				XmlDocument DocList = new XmlDocument();
				DocList.Load(WebList.GetResponse().GetResponseStream());

				Console.WriteLine("{0}", DocList.OuterXml);

				XmlNodeList ListNode;
				XmlDocument XMLTemp = new XmlDocument();

				try
				{
					ListNode = DocList.SelectNodes(@"root/MACHINE/POOL");
				
					foreach(XmlNode Node in ListNode)
					{
						Console.WriteLine("{0}", Node.OuterXml);
					}
//					Start S = new Start();
//					InDatabase = true;
				}
				catch (NullReferenceException e)
				{
					Console.WriteLine("{0}", e.ToString());
					Console.WriteLine("Computer is not in Database");
//					InDatabase = false;
				}
								
//				try
//				{
//
//					Start s = new Start();
//					if (InDatabase == true)
//					{
//						Console.WriteLine("{0}", InDatabase.ToString());
//						ListNode = DocList.SelectSingleNode(@"//MACHINE");
//
//						Console.WriteLine("InDatabase = {0}", DocList.OuterXml);
//
//						XmlDocument DocUpdate = new XmlDocument();
//						DocUpdate.LoadXml(ListNode.OuterXml.ToString());
//
//						DocUpdate.DocumentElement.SetAttribute("action", "delete");
//
//						string NewXML = "<root>" + DocUpdate.OuterXml.ToString() + "</root>";
//
//						WebRequest WebUpdate = WebRequest.Create(DitsUpdate);
//						WebUpdate.Method = "POST";
//						WebUpdate.Credentials = CredentialCache.DefaultCredentials; 
//			
//						StreamWriter StreamUpdate = new StreamWriter(WebUpdate.GetRequestStream());
//						StreamUpdate.Write(NewXML);
//						StreamUpdate.Close();
//
//						XmlDocument DocUpdateRep = new XmlDocument();
//						DocUpdateRep.Load(WebUpdate.GetResponse().GetResponseStream());
//						
//					} 
//					else 
//					{
//						XmlDocument XMLUpdate = new XmlDocument();
//
//						XMLUpdate.LoadXml(	
//							"<MACHINE></MACHINE>");
//
//						XMLUpdate.DocumentElement.SetAttribute("name", ComputerName);
//						XMLUpdate.DocumentElement.SetAttribute("user", Account);
//						XMLUpdate.DocumentElement.SetAttribute("domain", Domain);
//						XMLUpdate.DocumentElement.SetAttribute("password", Password);
//						XMLUpdate.DocumentElement.SetAttribute("action", "add");
//						XMLUpdate.DocumentElement.SetAttribute("owner", "deangj");
//						XMLUpdate.DocumentElement.SetAttribute("location", "1019");
//						string XMLConplete = "<root>" + XMLUpdate.OuterXml + "</root>";
//											
//						Console.WriteLine("{0}", XMLConplete);
//						
//						WebRequest WebUpdate = WebRequest.Create(DitsUpdate);
//						WebUpdate.Method = "POST";
//						WebUpdate.Credentials = CredentialCache.DefaultCredentials; 
//			
//						StreamWriter StreamUpdate = new StreamWriter(WebUpdate.GetRequestStream());
//						StreamUpdate.Write(XMLConplete);
//						StreamUpdate.Close();
//
//						XmlDocument DocUpdate = new XmlDocument();
//						DocUpdate.Load(WebUpdate.GetResponse().GetResponseStream());
//					}
//				}
//				catch (NullReferenceException e)
//				{
//					Console.WriteLine("GetMachineInfo");
//					Console.WriteLine("{0}", e.ToString());
//				}
//				
		}
			catch(System.Runtime.InteropServices.COMException e)
			{
				Console.WriteLine("{0}" ,e.ToString());
			}
		}
		static void Usage()
		{

		}
	}
}
