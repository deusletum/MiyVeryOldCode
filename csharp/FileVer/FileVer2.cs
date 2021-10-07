using System;
using System.Diagnostics;
using System.IO;

namespace FileVer2
{
	/// <summary>
	/// Takes comandling args for path and file name.
	/// It then gets the file version infomations.
	/// </summary>
 
	class Usage
	{
		public void DisplayUsage()
		{
			Console.WriteLine("Usage FileVer2.exe <filename>");
			Console.WriteLine("<filename> if file not in current directory use full path and file name");
			Console.WriteLine("Examples:");
			Console.WriteLine("FileVer2.exe c:\\mydir\\myapp.exe");
			Console.WriteLine("FileVer2.exe \\\\myserver\\myshare\\myapp.exe");
		}
	}
	class FileVersonInformation
	{
		public string FileVersion(string FileName)
		{
			FileVersionInfo fileNfo = FileVersionInfo.GetVersionInfo(FileName);
			return fileNfo.ToString();
		}
		
		public string Get_Path()
		{
			string DirPath;
			DirPath = Directory.GetCurrentDirectory();
			return DirPath.ToString();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			string Ver;
			string fPath;
			string fName;
			string Slash = "\\";

			FileVersonInformation f = new FileVersonInformation();

			try
			{
				Ver = f.FileVersion(args[0]);
				Console.WriteLine("This is the file Info {0}", Ver);
			}
			catch(System.IndexOutOfRangeException)
			{
				Usage u = new Usage();
				u.DisplayUsage();
			}
			catch(System.IO.FileNotFoundException fExcept)
			{
				Console.WriteLine("File was not found");
				Usage u = new Usage();
				u.DisplayUsage();
			}
			catch(System.ArgumentException ArgExcept)
			{
				if (ArgExcept.Message == "Absolute path information is required.")
				{
					fPath = f.Get_Path();
					fName = args[0];
					fPath = fPath + Slash + fName;
				
					Ver = f.FileVersion(fPath);
					Console.WriteLine("This is the file Info {0}", Ver);
				}
				if (ArgExcept.Message == "Illegal characters in path.")
				{
					Usage u = new Usage();
					u.DisplayUsage();
				}
			}
			catch(System.Exception Except)
			{
				Console.WriteLine("Unknown Exception Occurred: {0}", Except.Message);
			}
		}
	}
}
