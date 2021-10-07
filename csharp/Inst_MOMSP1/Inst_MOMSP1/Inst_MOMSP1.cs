//-------------------------------------------------------------------
///

///

///
/// <summary>
///    An EXE wrapper for class library MOM10SP1.dll for installing
///     MOMSP1.
///    Error Codes:
///    0 = Install Pass
///    1 = Install Failed
///    2 = CDS failure
/// </summary>
/// 
/// <types>
/// Utilities
/// Install
/// Start
/// </types>
/// 
/// <history>
///     <record date="22-Jul-03" who="deangj">
///     First Creation
///     </record>     
/// </history>
///
//-------------------------------------------------------------------
using System;
using System.IO;
using CDSCom;
using Microsoft.MOM.Test.Operations.MOMSP1Inst;

namespace Inst_MOMSP1
{
    #region Utilities Class
    class Utilities
    {
        /// <summary>
        /// Writes strings to a log file
        /// </summary>
        /// <param name="LogEntry">Text string to be written to the log file</param>
        /// <param name="LogFile">Path and name of log file to write logging information to</param>
        /// <param name="DisplayTime">Log time</param>
        public void Log(string LogEntry, string LogFile, bool DisplayTime)
        {
            try
            {
                bool time = DisplayTime;
                if(time)
                {
                    StreamWriter LogStream = new StreamWriter(LogFile,true);
                    LogStream.WriteLine(System.DateTime.Now + " " + LogEntry);
                    Console.WriteLine(System.DateTime.Now + " " + LogEntry);
                    LogStream.Close();
                }
                else
                {
                    StreamWriter LogStream = new StreamWriter(LogFile,true);
                    LogStream.WriteLine(LogEntry);
                    Console.WriteLine(LogEntry);
                    LogStream.Close();
                }
            }
            catch(FileLoadException e)
            {
                Console.WriteLine("File " + LogFile + " did not load.");
                Console.WriteLine("An Exception occurred" + e.Message);
            }
        }
        /// <summary>
        /// Gets the password for specified user from CDS
        /// </summary>
        /// <param name="User">User name in domain\user syntax</param>
        /// <returns>Returns the password for the specified user</returns>
        public string GetPassword(string User)
        {
            string TestAccount = User;
            string TestPassword;

            //Get CDS User info
            try
            {
                UsersClass CDS = new UsersClass();
                object account = (object)TestAccount;
                TestPassword = (string)CDS.GetPassword(ref account);
                return TestPassword;
            }
            catch(Exception e)
            {
                Console.WriteLine("There was an error creating or connecting to CDS");
                Console.WriteLine("Please make sure that CDScom.dll is registered");
                Console.WriteLine(e.ToString());
                return "CDSERROR";
            }
        }
    }
    #endregion

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Start
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            Install_Typical InstallParams = new Install_Typical();
            InstallParams.InstallFile = @"\\astdrop\builds\MOM10SP1\1300\x86FRE\Retail\Intel\OMSetup.exe";
            InstallParams.DASDomain = "smx";
            InstallParams.DASUser = "asttest";
            InstallParams.DASPassword = "Bacchus#01";
            InstallParams.UseCAMAccount = true;
            InstallParams.CAMDomain = "smx";
            InstallParams.CAMUser = "asttest";
            InstallParams.CAMPassword = "Bacchus#01";
            InstallParams.ConfigurationName = "Inst_MOMSP1 Test";

            Install MOMInstall = new Install();
            MOMInstall.InstallMOMSP1(InstallParams);
		}
	}
}
