using System;
using System.Collections;
using System.IO;
using System.Threading;
	/// <summary>
	/// Summary description for class MultiCopy.
	/// </summary>
namespace MultiCopy
{
	class FileArrayList
	{
		private string sourcedir;
		private string destdir;

		public static ArrayList fArray = new ArrayList();
		
		public FileArrayList()
		{
			sourcedir = "c:\\MyDir";
			destdir = "c:\\MyDir";
		}

		public static string SourceDir
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

		public static string DestDir
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

	}
	
	class ThreadCopy
	{
		public void CopyOne()
		{
			//			int One = FileArrayList.fArray.Count/4;
			//			int Two = One + One;
			//			int Three = Two + One;
			//			int Four = Three + FileArrayList.fArray.Count;

			//Array myTargetArray=Array.CreateInstance( typeof(String), 15 );

			int CountOne = FileArrayList.fArray.Count/4;

			//FileArrayList.fArray.CopyTo(0, files, 0, CountOne);

			for(int i=0;i!=CountOne;i++)
			{
				FileInfo fName = new FileInfo(FileArrayList.fArray[i].ToString());
				Console.WriteLine("{0}", fName.FullName);
			}
	
		}
		public void CopyTwo()
		{
			int CountOne = FileArrayList.fArray.Count/4;
			int CountTwo = CountOne + CountOne;
			
			Array fileArray = Array.CreateInstance( typeof(string), CountTwo);

			int InternalCount = CountOne;
			for (int i=0;i!=CountOne;i++)
			{
				fileArray.SetValue( FileArrayList.fArray[InternalCount], i);
				InternalCount++;
			}

			Console.WriteLine("fileArrayTwo Length is {0}", fileArray.Length);
		}

	}
				

	class CopyStuff
	{
		public void Copyfolder(string srcFolderFullName, string destFolderFullName)
		{
			
			try
			{
				DirectoryInfo destDirectoryInfo = new DirectoryInfo(destFolderFullName);

				if(!destDirectoryInfo.Exists)
				{
					//create dest folder if it does not exist.
					destDirectoryInfo.Create();
				}
				
				DirectoryInfo srcDirectoryInfo = new DirectoryInfo(srcFolderFullName);

				foreach(FileInfo srcFile in srcDirectoryInfo.GetFiles())        
				{
					//overwrite existing file at dest
					srcFile.CopyTo(destFolderFullName + @"\" + srcFile.Name, true);
					Console.WriteLine("Coping {0}", srcFolderFullName + "\\" + srcFile.Name);
				}
				foreach(DirectoryInfo srcSubFolder in srcDirectoryInfo.GetDirectories())
				{
					//Here is the Recursion
					Copyfolder(srcSubFolder.FullName, destFolderFullName + @"\" + srcSubFolder.Name);
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("An Unknown Exception occurred: {0}, {1}", e.Message, e.Source);
			}
		}

		public void FillFileArray(string Source, string Dest)
		{

			DirectoryInfo SourceDir = new DirectoryInfo(Source);

			FileInfo[] ArrayFiles = SourceDir.GetFiles();
			
			foreach(FileInfo file in ArrayFiles)
			{
				FileArrayList.fArray.Add(Source + "\\" + file.Name);
			}

			foreach(DirectoryInfo SourceSub in SourceDir.GetDirectories())
			{
				FillFileArray(SourceSub.FullName, Source + "\\" + SourceSub.Name);
			}
		}
	}

	/// <summary>
	/// This is the Start up class which hold 'Main()' the entry poing for the application.
	/// </summary>
	class Start
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		
		[STAThread]
		static void Main(string[] args)
		{
			FileArrayList.SourceDir = args[0];
			FileArrayList.DestDir = args[1];

			CopyStuff c = new CopyStuff();
			//	c.Copyfolder(args[0], args[1]);
			c.FillFileArray(args[0], args[1]);
//			foreach (string file in FileArrayList.fArray)
//			{
//				Console.WriteLine("{0}", file);
//			}
//			Console.WriteLine("fArray Count {0} and fArray Count/4 {1}",
//				FileArrayList.fArray.Count, FileArrayList.fArray.Count/4);
//
			

			ThreadCopy th = new ThreadCopy();
			th.CopyOne();
//			th.CopyTwo();
		}
	}
}
