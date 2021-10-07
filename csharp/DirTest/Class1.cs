using System;
using System.IO;

namespace DirTest
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
			Console.WriteLine(GetDirectorySize("c:\\windows"));
		}
        static long GetDirectorySize(string dir)
        {
            long lDirSize = 0;

            string[] files = System.IO.Directory.GetFiles(dir);
            foreach (string filename in files) 
            {
                FileInfo info = new FileInfo(filename);
                lDirSize += info.Length;
            }

            string[] childDir = System.IO.Directory.GetDirectories(dir);
            foreach(string child in childDir)
            {
                lDirSize += GetDirectorySize(child);
            }	
            return lDirSize;
        }
	}
}
