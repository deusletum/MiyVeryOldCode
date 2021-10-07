using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace Mp3Joinner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public FileInfo Join(List<FileInfo> Files,string NewFileName)
        {
            FileStream NewFileReader = new FileStream(NewFileName, FileMode.Create);
            FileStream FileReader = null;

            foreach (FileInfo f in Files)
            {
                FileReader = new FileStream(f.FullName, FileMode.Open);
                FileReader.Read(
            }
            return null;
        }
    }
}
