///<!------------------------------------------------------------------->
///

///

///
/// <summary>
///    
/// </summary>
/// 
/// <types>
/// CopyFiles
/// Start
/// </types>
/// 
/// <history>
///     <record date="05-Feb-02" who="a-deangj">
///     First Creation
///     </record>     
/// </history>
///
///<!------------------------------------------------------------------->
using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace WinBuildsCopy
{


	class Copy
	{
			
		private string sourcedir;
		private string destdir;
		private bool over;
					
		/// <summary>
		///  Property for holding the string value for the Source Directory
		/// </summary>
		public string SourceDir
		{
			get
			{
				return sourcedir;
			}
			set
			{
				sourcedir = value;
			}
		}
			
		/// <summary>
		/// Property for holding the string value for the Destination Directory
		/// </summary>
		public string DestDir
		{
			get
			{
				return destdir;
			}
			set
			{
				destdir = value;
			}
		}
			
		/// <summary>
		/// Property for holding the bool value for the Over writing files
		/// in the Destination Directory
		/// </summary>
		public bool OverRide
		{
			get
			{
				return over;
			}
			set
			{
				over = value;
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="SourcePath"></param>
		/// <param name="DestPath"></param>
		/// <param name="OverWrite"></param>
		public void Copy(string SourcePath, string DestPath, bool OverWrite)
		{		
			try
			{
				DirectoryInfo destDirectoryInfo = new DirectoryInfo(DestPath);
				if(!destDirectoryInfo.Exists)
				{
					//create dest folder if it does not exist.
					destDirectoryInfo.Create();
				}
				
				DirectoryInfo srcDirectoryInfo = new DirectoryInfo(SourcePath);
				foreach(FileInfo srcFile in srcDirectoryInfo.GetFiles())        
				{
					//overwrite existing file at dest
					srcFile.CopyTo(destFolderFullName + @"\" + srcFile.Name, OverWrite);
				}

				foreach(DirectoryInfo srcSubFolder in srcDirectoryInfo.GetDirectories())
				{
					Copyfolder(srcSubFolder.FullName, destFolderFullName + @"\" + srcSubFolder.Name);
				}
			}
			catch(IOException IOExcept)
			{
				Console.WriteLine("There was an exception");
				Console.WriteLine("{0}", IOExcept.Message);
			}
		}
	}
	/// <summary>
	/// 
	/// </summary>
	class GetBuilds
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filepath">Path to the </param>
		/// <returns></returns>
		public int ParseLatestBuildFile(string filepath)
		{
			try
			{
				Console.WriteLine("{0}",filepath);
				FileInfo FileOpen = new FileInfo(filepath);
				StreamReader FileReader = FileOpen.OpenText();
				string text = "";
				string alllines = "";

				do
				{
					text =  FileReader.ReadLine();
					alllines = alllines + "\n" + text;
				} while (text != null);

				FileReader.Close();
				
				char eql = char.Parse("=");
				char[] chars = new char[] {eql};

				string[] NumArray = new String[2];
				NumArray = (string[])alllines.Split(chars);

				int BuildNum = int.Parse(NumArray[1]);
				return BuildNum;

				
			}
			catch(DirectoryNotFoundException)
			{
				Console.WriteLine("The Directory was not found");
				return 0;
			}
			catch(FileNotFoundException f)
			{
				Console.WriteLine("The file {0} was not found", f.FileName.ToString());
				return 0;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		public void WriteLog()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public void WriteFile()
		{

		}
		/// <summary>
		/// This method gets the the Build number of a windows build
		/// </summary>
		/// <param name="strType">Build type: IDW, IDS, TEST</param>
		/// <param name="strLang">The language you want the build number for</param>
		/// <returns>Build Number</returns>
		//public int GetCurrentBuildNum(string strType, string strLang)
		public int GetCurrentBuildNum(string strType, string strLang)
		{
			IDictionary Langs = (IDictionary) ConfigurationSettings.GetConfig("Languages");
			ICollection LangsRay = Langs.Values;

			IDictionary BuildTypes = (IDictionary) ConfigurationSettings.GetConfig("Directories");
			string IDW = (string)BuildTypes["IDWDir"];
			string IDS = (string)BuildTypes["IDSDir"];
			string TEST = (string)BuildTypes["TestDir"];

			Settings Config = new Settings();
			
			string FilePath = Config.WinbuildsRoot;

			FilePath = FilePath + strLang;

			if (strType == "IDW")
			{
				FilePath = FilePath + IDW + Config.CheckOrFree + Config.Enterprise + Config.VersionFile;
			}
			
			if (strType == "IDS")
			{
				FilePath = FilePath + IDS + Config.CheckOrFree + Config.Enterprise + Config.VersionFile;
			}

			if (strType == "TST")
			{
				FilePath = FilePath + TEST + Config.CheckOrFree + Config.Enterprise + Config.VersionFile;
			}
			
			Console.WriteLine("FilePath is: {0}", FilePath);

			try
			{
				FileVersionInfo fileNfo = FileVersionInfo.GetVersionInfo(FilePath);

				return fileNfo.ProductBuildPart;
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("The file {0} was not found", FilePath);
				return 0;
			}
		}
	}


	/// <summary>
	/// This class acts as the config storage class for this app.
	/// </summary>
	public class Settings
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Settings()
		{
			#region Constructor
			IDictionary ConfigFile = (IDictionary) ConfigurationSettings.GetConfig("Constants");
			///<summary>
			///Current \\winbuilds root path to the lang builds of .Net Server
			///</summary>
			WinBuilds = (string)ConfigFile["WinbuildsRoot"];
			
			///<summary>
			///Full path to the $OEM$ directory that holds drivers that are not part of
			///the windows binaries
			///</summary>
			OEMPath = (string)ConfigFile["OEMRoot"];
			
			///<summary>
			///file to get file version info from to get build number
			///</summary>
			VerFile = (string)ConfigFile["Verfile"];
			
			/// <summary>
			/// constants that should not change
			/// </summary>
			FreeOrCheck = "\\x86fre";
			OEMDir = "\\$OEM$";
			ProcType = "\\i386";
			Ent = "\\ads";
			Serv = "\\srv";
			Symbol = "\\sym";
			
			/// <summary>
			/// The root path for files to be copied to
			/// </summary>
			FileSrv = (string)ConfigFile["SMFilesRootPath"];

			/// <summary>
			/// The root path for the Log file
			/// </summary>
			logDir = (string)ConfigFile["SMLogDir"];

			/// <summary>
			/// The log file name
			/// </summary>
			logFile = (string)ConfigFile["SMLogFile"];
			#endregion
		}

		/// <summary>
		/// All these properties corresponds to the varibles they use for get and set
		/// </summary>
		public string WinbuildsRoot
		{
			get
			{
				return WinBuilds;
			}
			set
			{
				WinBuilds = value;
			}
		}
		public string OEMRootPath
		{
			get
			{
				return OEMPath;
			}
			set
			{
				OEMPath = value;
			}
		}
		public string VersionFile
		{
			get
			{
				return VerFile;
			}
			set
			{
				VerFile = value;
			}
		}
		public string CheckOrFree
		{
			get
			{
				return FreeOrCheck;
			}
			set
			{
				FreeOrCheck = value;
			}
		}
		public string OEM
		{
			get
			{
				return OEMDir;
			}
			set
			{
				OEMDir = value;
			}
		}
		public string ProcessorType
		{
			get
			{
				return ProcType;
			}
			set
			{
				ProcType = value;
			}
		}
		public string Enterprise
		{
			get
			{
				return Ent;
			}
			set
			{
				Ent = value;
			}
		}
		public string Server
		{
			get
			{
				return Serv;
			}
			set
			{
				Serv = value;
			}
		}
		public string SymPath
		{
			get
			{
				return Symbol;
			}
			set
			{
				Symbol = value;
			}
		}
		public string FileServer
		{
			get
			{
				return FileSrv;
			}
			set
			{
				FileSrv = value;
			}
		}
		public string LogDirRoot
		{
			get
			{
				return logDir;
			}
			set
			{
				logDir = value;
			}
		}
		public string LogFile
		{
			get
			{
				return logFile;
			}
			set
			{
				logFile = value;
			}
		}

		#region Private Memeber Variables
		private string WinBuilds;
		private string OEMPath;
		private string VerFile;
		private string FreeOrCheck;
		private string OEMDir;
		private string ProcType;
		private string Ent;
		private string Serv;
		private string Symbol;
		private string FileSrv;
		private string logDir;
		private string logFile;
		#endregion
	}
	

	/// <summary>
	/// Summary description for Start.
	/// </summary>
	class Start
	{

		public static int Num;
		public static int BuildNum;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			IDictionary Langs = (IDictionary) ConfigurationSettings.GetConfig("Languages");
			ICollection LangsRay = Langs.Values;

			IDictionary BuildFiles = (IDictionary) ConfigurationSettings.GetConfig("Files");
			ICollection BuildFilesRay = BuildFiles.Values;

			IDictionary BuildDirs = (IDictionary) ConfigurationSettings.GetConfig("Directories");
			ICollection BuildDirsRay = BuildDirs.Values;

			Settings s = new Settings();

			GetBuilds b = new GetBuilds();

			Copy c = new Copy();

			///<--- Start Foreach 1 --->
			foreach (string Lang in LangsRay)
			{
				string ServePath;
				string BuildsPath;

				///<--- Start Foreach 2 --->
				foreach (string File in BuildFilesRay)
				{

					ServePath = s.FileServer + Lang.ToString() + File.ToString();
					Start.Num = b.ParseLatestBuildFile(ServePath);
					Console.WriteLine("The build number in file: {0} is: {1}", ServePath, Num);


				}///<--- End Foreach 2 --->

				///<--- Start Foreach 3 --->
				foreach (string Dir in BuildDirsRay)
				{
					BuildsPath = s.WinbuildsRoot + Lang.ToString() + Dir.ToString() + s.CheckOrFree + s.Enterprise;

					char dot = char.Parse(".");
					char[] chars = new char[] {dot};

					string[] BuildType = new string[1];
					//Console.WriteLine("Current Dir is: {0}", Dir.ToString());
					BuildType = (string[])Dir.ToString().Split(chars);
					
					//Console.WriteLine("Build Type {0}", BuildType[1]);
					Start.BuildNum = b.GetCurrentBuildNum(BuildType[1], Lang.ToString());
					if (Start.BuildNum == 0)
					{
						break;
					}
					if (Start.BuildNum != Start.Num)
					{
						string adspath = s.WinbuildsRoot + Lang.ToString() + Dir.ToString() + s.CheckOrFree + s.Enterprise;
						string srvpath = s.WinbuildsRoot + Lang.ToString() + Dir.ToString() + s.CheckOrFree + s.Server;
						string oemadspath = s.WinbuildsRoot + Lang.ToString() + Dir.ToString() + s.CheckOrFree + s.Enterprise + s.OEM;
						string oemsrvpath = s.WinbuildsRoot + Lang.ToString() + Dir.ToString() + s.CheckOrFree + s.Server + s.OEM;
						string fservadspath = s.FileServer + Lang.ToString() + "\\" + Start.BuildNum + s.Enterprise;
						string fservsrvpath = s.FileServer + Lang.ToString() + "\\" + Start.BuildNum + s.Server;
						string baseoempath = s.OEMRootPath;

						c.Copy(adspath, fservadspath, true);
					}
				}///<--- End Foreach 3 --->

			}///<--- End Foreach 1 --->
		}
	}
}
#region MCopy
//using System;
//using System.Collections;
//using System.IO;
//using System.Threading;
//
//namespace MCopy
//{
//	//-------------------------------------------------------------------
//	///
//	/// <summary>
//	///    This class holds the fills two arraylists with source and destination
//	///    information for coping files. It also has four methods for the thread
//	///    object to call from Start.Main.
//	/// <summary>
//	/// 
//	/// <list type="property">SourceDir</list>
//	/// <list type="property">DestDir</list>
//	/// <list type="property">OverRide</list>
//	/// <list type="method">FillSourceArray</list>
//	/// <list type="method">FillDestArray</list>
//	/// <list type="method">CopyOne</list>
//	/// <list type="method">CopyTwo</list>
//	/// <list type="method">CopyThree</list>
//	/// <list type="method">CopyFour</list>
//	/// 
//	/// <history>
//	///     <record date="05-Feb-02" who="a-deangj">
//	///     First Creation
//	///     </record>     
//	/// </history>
//	///
//	//-------------------------------------------------------------------
//	class CopyFiles
//	{
//
//		private string sourcedir;
//		private string destdir;
//		private bool over;
//
//		/// <summary>
//		///  fArray is the Source files with full path.
//		/// </summary>
//		public static ArrayList fArray = new ArrayList();
//
//		/// <summary>
//		/// fArrayDest is the Destination Directory path.
//		/// </summary>
//		public static ArrayList fArrayDest = new ArrayList();
//		
//		/// <summary>
//		///  Property for holding the string value for the Source Directory
//		/// </summary>
//		public string SourceDir
//		{
//			get
//			{
//				return sourcedir;
//			}
//			set
//			{
//				sourcedir = value;
//			}
//		}
//
//		/// <summary>
//		/// Property for holding the string value for the Destination Directory
//		/// </summary>
//		public string DestDir
//		{
//			get
//			{
//				return destdir;
//			}
//			set
//			{
//				destdir = value;
//			}
//		}
//
//		/// <summary>
//		/// Property for holding the bool value for the Over writing files
//		/// in the Destination Directory
//		/// </summary>
//		public bool OverRide
//		{
//			get
//			{
//				return over;
//			}
//			set
//			{
//				over = value;
//			}
//		}
//
//		/// <summary>
//		/// This method uses Recursion to fill fArray with Source Directory and file full Path
//		/// </summary>
//		/// <param name="Source">Source Directory, passed in as args[0] for class start</param>
//		/// <param name="Dest">Destination Directory, passed in as args[1] for class start</param>
//		public void FillSourceArray(string Source, string Dest)
//		{
//
//			DirectoryInfo SourceDir = new DirectoryInfo(Source);
//
//			FileInfo[] ArrayFiles = SourceDir.GetFiles();
//			
//			foreach(FileInfo file in ArrayFiles)
//			{
//				fArray.Add(Source + "\\" + file.Name);
//			}
//
//			foreach(DirectoryInfo SourceSub in SourceDir.GetDirectories())
//			{
//				FillSourceArray(SourceSub.FullName, Source + "\\" + SourceSub.Name);
//			}
//		}
//
//		/// <summary>
//		/// This method uses Recursion to fill fArrayDest with Destination Directory full Path to
//		/// perserve path information when coping files
//		/// </summary>
//		/// <param name="Source">Source Directory, passed in as args[0] for class start</param>
//		/// <param name="Dest">Destination Directory, passed in as args[1] for class start</param>
//		public void FillDestArray(string Source, string Dest)
//		{
//
//			DirectoryInfo SourceDir = new DirectoryInfo(Source);
//
//			DirectoryInfo destDirectoryInfo = new DirectoryInfo(Dest);
//
//			if(!destDirectoryInfo.Exists)
//			{
//				//create dest folder if it does not exist.
//				destDirectoryInfo.Create();
//			}
//
//			FileInfo[] ArrayFiles = SourceDir.GetFiles();
//			
//			foreach(FileInfo file in ArrayFiles)
//			{
//				fArrayDest.Add(Dest + "\\" + file.Name);
//			}
//
//			foreach(DirectoryInfo SourceSub in SourceDir.GetDirectories())
//			{
//				FillDestArray(SourceSub.FullName, Dest + "\\" + SourceSub.Name);
//			}
//		}
//		
//		/// <summary>
//		///	This method takes the first 1/4 of fArray and fArrayDest and uses them to copy files.
//		/// </summary>
//		public void CopyOne()
//		{
//			int CountOne = fArray.Count/4;
//
//			for(int i=0;i!=CountOne;i++)
//			{
//				FileInfo fName = new FileInfo(fArray[i].ToString());
//				try
//				{
//					fName.CopyTo(fArrayDest[i].ToString(), OverRide);
//					Console.WriteLine("{0}", fArrayDest[i].ToString());
//				}
//				catch (IOException IOExecpt)
//				{
//					Console.WriteLine("File {0} is in uses, unable to copy", fArray[i].ToString());
//					Console.WriteLine("{0}", IOExecpt.Message);
//				}
//			}
//	
//		}
//		/// <summary>
//		/// This method takes the Second 1/4 of fArray and fArrayDest and uses them to copy files.
//		/// </summary>
//		public void CopyTwo()
//		{
//			int CountOne = fArray.Count/4;
//			int CountTwo = CountOne + CountOne;
//			
//			for(int i=CountOne;i!=CountTwo;i++)
//			{
//				FileInfo fName = new FileInfo(fArray[i].ToString());
//				try
//				{
//					fName.CopyTo(fArrayDest[i].ToString(), OverRide);
//					Console.WriteLine("{0}", fArrayDest[i].ToString());
//				}
//				catch (IOException IOExecpt)
//				{
//					Console.WriteLine("File {0} is in uses, unable to copy", fArray[i].ToString());
//					Console.WriteLine("{0}", IOExecpt.Message);
//				}
//			}
//		}
//
//		/// <summary>
//		/// This method takes the Third 1/4 of fArray and fArrayDest and uses them to copy files.
//		/// </summary>
//		public void CopyThree()
//		{
//			int CountOne = fArray.Count/4;
//			int CountTwo = CountOne + CountOne;
//			int CountThree = CountTwo + CountOne;
//			
//			for(int i=CountTwo;i!=CountThree;i++)
//			{
//				FileInfo fName = new FileInfo(fArray[i].ToString());
//				try
//				{
//					fName.CopyTo(fArrayDest[i].ToString(), OverRide);
//					Console.WriteLine("{0}", fArrayDest[i].ToString());
//				}
//				catch (IOException IOExecpt)
//				{
//					Console.WriteLine("File {0} is in uses, unable to copy", fArray[i].ToString());
//					Console.WriteLine("{0}", IOExecpt.Message);
//				}
//			}
//		}
//
//		/// <summary>
//		/// This method takes the forth 1/4 of fArray and fArrayDest and uses them to copy files.
//		/// </summary>
//		public void CopyFour()
//		{
//			int CountOne = fArray.Count/4;
//			int CountTwo = CountOne + CountOne;
//			int CountThree = CountTwo + CountOne;
//			int CountFour = fArray.Count;
//			
//			for(int i=CountThree;i!=CountFour;i++)
//			{
//				FileInfo fName = new FileInfo(fArray[i].ToString());
//				try
//				{
//					fName.CopyTo(fArrayDest[i].ToString(), OverRide);
//					Console.WriteLine("{0}", fArrayDest[i].ToString());
//				}
//				catch (IOException IOExecpt)
//				{
//					Console.WriteLine("File {0} is in uses, unable to copy", fArray[i].ToString());
//					Console.WriteLine("{0}", IOExecpt.Message);
//				}
//			}
//		}
//	}
//
//	//-------------------------------------------------------------------
//	///
//	/// <summary>
//	///    This class houses Main
//	/// <summary>
//	///
//	/// <remarks>
//	///    Main pares the command line arguments, it then passes those
//	///    arguments to the properties and methods in class CopyFiles.
//	///    After that is creates four instances of System.Thread with
//	///    the four Copy methods from class CopyFiles and starts these
//	///    threads
//	/// </remarks>
//	/// 
//	/// <list type="methond">Main</list>
//	/// 
//	/// <history>
//	///     <record date="05-Feb-02" who="a-deangj">
//	///     First Creation
//	///     </record>     
//	/// </history>
//	///
//	//-------------------------------------------------------------------
//	class Start
//	{
//		/// <summary>
//		/// The main entry point for the application.
//		/// </summary>
//		//[STAThread]
//		static void Main(string[] args)
//		{
//			bool OverWrite = false;
//
//			usage use = new usage();
//			try
//			{
//				if (args[0] == "/?")
//				{
//					use.DisplayUage();
//				}
//
//				if (args[0] == "?")
//				{
//					use.DisplayUage();
//				}
//				
//				if (args[0] == "help")
//				{
//					use.DisplayUage();
//				}
//
//				if (args[1] == "")
//				{
//					use.DisplayUage();
//				}
//				try
//				{
//					if (args[2] == "TRUE")
//					{
//						OverWrite = true;
//					}
//					
//					if (args[2] == "FALSE")
//					{
//						OverWrite = false;
//					}
//				}
//				catch(IndexOutOfRangeException)
//				{
//					OverWrite = false;
//				}
//
//
//				CopyFiles Copy = new CopyFiles();
//			
//				Copy.SourceDir = args[0];
//				Copy.DestDir = args[1];
//				Copy.OverRide = OverWrite;
//
//				Copy.FillSourceArray(args[0], args[1]);
//
//				Copy.FillDestArray(args[0], args[1]);
//
//				Thread ThreadOne = new Thread(new ThreadStart(Copy.CopyOne));
//				//Copy.CopyOne();
//				Thread ThreadTwo = new Thread(new ThreadStart(Copy.CopyTwo));
//				//Copy.CopyTwo();
//				Thread ThreadThree = new Thread(new ThreadStart(Copy.CopyThree));
//				//Copy.CopyThree();
//				Thread ThreadFour = new Thread(new ThreadStart(Copy.CopyFour));
//				//Copy.CopyFour();
//
//				ThreadOne.Start();
//				ThreadTwo.Start();
//				ThreadThree.Start();
//				ThreadFour.Start();
//			}
//			catch(IndexOutOfRangeException)
//			{
//				use.DisplayUage();
//			}
//		}
//	}
//
//	//-------------------------------------------------------------------
//	///
//	/// <summary>
//	///    Usage Object
//	/// <summary>
//	///
//	/// <remarks>
//	///    Displays usage information
//	/// </remarks>
//	/// 
//	/// <list type="method">DisplayUsage</list>
//	/// 
//	/// <history>
//	///     <record date="05-Feb-02" who="a-deangj">
//	///     First Creation
//	///     </record>     
//	/// </history>
//	///
//	//-------------------------------------------------------------------
//	class usage
//	{
//		/// <summary>
//		/// Displays Usage Information
//		/// </summary>
//		public void DisplayUage()
//		{
//			Console.WriteLine("Usage for CopyWin.exe");
//			Console.WriteLine("CopyWin.exe <SourceDir> <DestDir> <OverRide>");
//			Console.WriteLine("<SourceDir> = Source Directory to be copied");
//			Console.WriteLine("<DestDir> = Destination Directory for the File to be copied to");
//			Console.WriteLine("<OverRide> = TRUE or FALSE to Over Ride files in the Destination Directory, FALSE is the Default");
//		}
//	}
//}
#endregion