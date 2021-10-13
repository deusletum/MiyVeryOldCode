using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;

namespace GetDirsAndFiles
{
    class Program
    {
        /// <summary>
        /// string list of files
        /// </summary>
        private List<string> FileList = new List<string>();

        /// <summary>
        /// Static main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program p = new Program();
            List<string> files = p.GetFiles("c:\\Users");
            foreach (string f in files)
            {
                Console.WriteLine(f);
            }
        }

        /// <summary>
        /// Returns a string list of all files and files in subdirectories from a string directory path
        /// </summary>
        /// <param name="Dir">string list file names</param>
        /// <returns></returns>
        public List<string> GetFiles(string Dir)
        {
            if (Directory.Exists(Dir))
            {
                DirectoryInfo TempDir = new DirectoryInfo(Dir);
                try
                {
                    foreach (FileInfo f in TempDir.GetFiles())
                    {
                        this.FileList.Add(f.FullName);
                    }
                    foreach (DirectoryInfo d in TempDir.GetDirectories())
                    {
                        this.GetFiles(d.FullName);
                    }
                }
                catch (UnauthorizedAccessException ue)
                {
                    this.FileList.Add(ue.Message);
                }
            }
            else
            {
                return null;
            }
            return this.FileList;
        }
    }
}
