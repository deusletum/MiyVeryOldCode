//-------------------------------------------------------------------
///
/// <summary>
///    This command line console app copies directorys with 4 threads.
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
//-------------------------------------------------------------------
using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace MCopy
{
    //<!----------------------------------------------------------------!>
    ///
    /// <summary>
    ///    This class holds the fills two arraylists with source and destination
    ///    information for coping files. It also has four methods for the thread
    ///    object to call from Start.Main.
    /// <summary>
    ///
    /// <list type="property">SourceDir</list>
    /// <list type="property">DestDir</list>
    /// <list type="property">OverRide</list>
    /// <list type="method">FillSourceArray</list>
    /// <list type="method">FillDestArray</list>
    /// <list type="method">CopyOne</list>
    /// <list type="method">CopyTwo</list>
    /// <list type="method">CopyThree</list>
    /// <list type="method">CopyFour</list>
    ///
    /// <history>
    ///     <record date="05-Feb-02" who="a-deangj">
    ///     First Creation
    ///     </record>
    /// </history>
    ///
    //<!----------------------------------------------------------------!>
    class CopyFiles
    {

        private string sourcedir;
        private string destdir;
        private bool over;

        /// <summary>
        ///  fArray is the Source files with full path.
        /// </summary>
        public static ArrayList fArray = new ArrayList();

        /// <summary>
        /// fArrayDest is the Destination Directory path.
        /// </summary>
        public static ArrayList fArrayDest = new ArrayList();

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
        /// This method uses Recursion to fill fArray with Source Directory and file full Path
        /// </summary>
        /// <param name="Source">Source Directory, passed in as args[0] for class start</param>
        /// <param name="Dest">Destination Directory, passed in as args[1] for class start</param>
        public void FillSourceArray(string Source, string Dest)
        {
            try
            {
                DirectoryInfo SourceDir = new DirectoryInfo(Source);

                FileInfo[] ArrayFiles = SourceDir.GetFiles();

                foreach (FileInfo file in ArrayFiles)
                {
                    fArray.Add(Source + "\\" + file.Name);
                }

                foreach (DirectoryInfo SourceSub in SourceDir.GetDirectories())
                {
                    FillSourceArray(SourceSub.FullName, Source + "\\" + SourceSub.Name);
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The directory {0} was not found", Source);
            }
        }

        /// <summary>
        /// This method uses Recursion to fill fArrayDest with Destination Directory full Path to
        /// perserve path information when coping files
        /// </summary>
        /// <param name="Source">Source Directory, passed in as args[0] for class start</param>
        /// <param name="Dest">Destination Directory, passed in as args[1] for class start</param>
        public void FillDestArray(string Source, string Dest)
        {
            try
            {
                DirectoryInfo SourceDir = new DirectoryInfo(Source);

                DirectoryInfo destDirectoryInfo = new DirectoryInfo(Dest);

                if (!destDirectoryInfo.Exists)
                {
                    //create dest folder if it does not exist.
                    destDirectoryInfo.Create();
                }

                try
                {
                    FileInfo[] ArrayFiles = SourceDir.GetFiles();

                    foreach (FileInfo file in ArrayFiles)
                    {
                        fArrayDest.Add(Dest + "\\" + file.Name);
                    }

                    foreach (DirectoryInfo SourceSub in SourceDir.GetDirectories())
                    {
                        FillDestArray(SourceSub.FullName, Dest + "\\" + SourceSub.Name);
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (DirectoryNotFoundException)
            {
                //Console.WriteLine("The directory {0} was not found", Source);
            }
        }

        /// <summary>
        ///	This method takes the first 1/4 of fArray and fArrayDest and uses them to copy files.
        /// </summary>
        public void CopyOne()
        {
            int CountOne = fArray.Count / 4;

            for (int i = 0; i != CountOne; i++)
            {
                FileInfo fName = new FileInfo(fArray[i].ToString());
                try
                {
                    fName.CopyTo(fArrayDest[i].ToString(), OverRide);
                    Console.WriteLine("{0}", fArrayDest[i].ToString());
                }
                catch (IOException IOExecpt)
                {
                    Console.WriteLine("File {0} is in uses, unable to copy", fArray[i].ToString());
                    Console.WriteLine("{0}", IOExecpt.Message);
                }
            }

        }
        /// <summary>
        /// This method takes the Second 1/4 of fArray and fArrayDest and uses them to copy files.
        /// </summary>
        public void CopyTwo()
        {
            int CountOne = fArray.Count / 4;
            int CountTwo = CountOne + CountOne;

            for (int i = CountOne; i != CountTwo; i++)
            {
                FileInfo fName = new FileInfo(fArray[i].ToString());
                try
                {
                    fName.CopyTo(fArrayDest[i].ToString(), OverRide);
                    Console.WriteLine("{0}", fArrayDest[i].ToString());
                }
                catch (IOException IOExecpt)
                {
                    Console.WriteLine("File {0} is in uses, unable to copy", fArray[i].ToString());
                    Console.WriteLine("{0}", IOExecpt.Message);
                }
            }
        }

        /// <summary>
        /// This method takes the Third 1/4 of fArray and fArrayDest and uses them to copy files.
        /// </summary>
        public void CopyThree()
        {
            int CountOne = fArray.Count / 4;
            int CountTwo = CountOne + CountOne;
            int CountThree = CountTwo + CountOne;

            for (int i = CountTwo; i != CountThree; i++)
            {
                FileInfo fName = new FileInfo(fArray[i].ToString());
                try
                {
                    fName.CopyTo(fArrayDest[i].ToString(), OverRide);
                    Console.WriteLine("{0}", fArrayDest[i].ToString());
                }
                catch (IOException IOExecpt)
                {
                    Console.WriteLine("File {0} is in uses, unable to copy", fArray[i].ToString());
                    Console.WriteLine("{0}", IOExecpt.Message);
                }
            }
        }

        /// <summary>
        /// This method takes the forth 1/4 of fArray and fArrayDest and uses them to copy files.
        /// </summary>
        public void CopyFour()
        {
            int CountOne = fArray.Count / 4;
            int CountTwo = CountOne + CountOne;
            int CountThree = CountTwo + CountOne;
            int CountFour = fArray.Count;

            for (int i = CountThree; i != CountFour; i++)
            {
                FileInfo fName = new FileInfo(fArray[i].ToString());
                try
                {
                    fName.CopyTo(fArrayDest[i].ToString(), OverRide);
                    Console.WriteLine("{0}", fArrayDest[i].ToString());
                }
                catch (IOException IOExecpt)
                {
                    Console.WriteLine("File {0} is in uses, unable to copy", fArray[i].ToString());
                    Console.WriteLine("{0}", IOExecpt.Message);
                }
            }
        }
    }

    ///<!----------------------------------------------------------------!>
    ///
    /// <summary>
    ///    This class houses Main
    /// <summary>
    ///
    /// <remarks>
    ///    Main pares the command line arguments, it then passes those
    ///    arguments to the properties and methods in class CopyFiles.
    ///    After that is creates four instances of System.Thread with
    ///    the four Copy methods from class CopyFiles and starts these
    ///    threads
    /// </remarks>
    ///
    /// <list type="methond">Main</list>
    ///
    /// <history>
    ///     <record date="05-Feb-02" who="a-deangj">
    ///     First Creation
    ///     </record>
    /// </history>
    ///
    ///<!----------------------------------------------------------------!>
    class Start
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main(string[] args)
        {
            bool OverWrite = false;

            usage use = new usage();
            try
            {
                if (args[0] == "/?")
                {
                    use.DisplayUage();
                }

                if (args[0] == "?")
                {
                    use.DisplayUage();
                }

                if (args[0] == "help")
                {
                    use.DisplayUage();
                }

                if (args[1] == "")
                {
                    use.DisplayUage();
                }
                try
                {
                    if (args[2] == "TRUE")
                    {
                        OverWrite = true;
                    }

                    if (args[2] == "FALSE")
                    {
                        OverWrite = false;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    OverWrite = false;
                }


                CopyFiles Copy = new CopyFiles();

                Copy.SourceDir = args[0];
                Copy.DestDir = args[1];
                Copy.OverRide = OverWrite;

                Copy.FillSourceArray(args[0], args[1]);

                Copy.FillDestArray(args[0], args[1]);

                Thread ThreadOne = new Thread(new ThreadStart(Copy.CopyOne));
                //Copy.CopyOne();
                Thread ThreadTwo = new Thread(new ThreadStart(Copy.CopyTwo));
                //Copy.CopyTwo();
                Thread ThreadThree = new Thread(new ThreadStart(Copy.CopyThree));
                //Copy.CopyThree();
                Thread ThreadFour = new Thread(new ThreadStart(Copy.CopyFour));
                //Copy.CopyFour();

                ThreadOne.Start();
                ThreadTwo.Start();
                ThreadThree.Start();
                ThreadFour.Start();
            }
            catch (IndexOutOfRangeException)
            {
                use.DisplayUage();
            }
        }
    }

    ///<!----------------------------------------------------------------!>
    ///
    /// <summary>
    ///    Usage Object
    /// <summary>
    ///
    /// <remarks>
    ///    Displays usage information
    /// </remarks>
    ///
    /// <list type="method">DisplayUsage</list>
    ///
    /// <history>
    ///     <record date="05-Feb-02" who="a-deangj">
    ///     First Creation
    ///     </record>
    /// </history>
    ///
    ///<!----------------------------------------------------------------!>
    class usage
    {
        /// <summary>
        /// Displays Usage Information
        /// </summary>
        public void DisplayUage()
        {
            Console.WriteLine("Usage for MCopy.exe");
            Console.WriteLine("MCopy.exe <SourceDir> <DestDir> <OverRide>");
            Console.WriteLine("<SourceDir> = Source Directory to be copied");
            Console.WriteLine("<DestDir> = Destination Directory for the File to be copied to");
            Console.WriteLine("<OverRide> = TRUE or FALSE to Over Ride files in the Destination Directory, FALSE is the Default");
        }
    }
}

