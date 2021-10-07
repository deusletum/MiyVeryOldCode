using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoDir
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.GetFiles(@"C:\tmp");
            p.copy(@"C:\EPUBS\NoDirs");
        }

        private List<FileInfo> files = new List<FileInfo>();

        public void GetFiles(string Path)
        {
            DirectoryInfo dpath = new DirectoryInfo(Path);
            foreach (FileInfo file in dpath.GetFiles())
            {
                files.Add(file);
            }
            foreach(DirectoryInfo dir in dpath.GetDirectories())
            {
                this.GetFiles(dir.FullName);
            }
        }
        public void copy(string dest)
        {
            if(!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            foreach (FileInfo file in this.files)
            {
                if (file.Name.Length > 240)
                {
                    file.CopyTo(dest + "\\" + file.Name.Substring(0, 240));
                }
                else
                {
                    file.CopyTo(dest + "\\" + file.Name, true);
                }
            }
        }
    }
}
