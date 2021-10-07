using System;
using System.Diagnostics;
using System.Management;
using System.Threading;

namespace MOM10SP1_Install
{
    class Utilities
    {
        private string ID;

        public int GetProcessID(string ProcessName)
        {
            char Quote = (char)34;
            string WMIQuery = "Select * From Win32_Process Where Name=" 
                + Quote + ProcessName + Quote;

            ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher(WMIQuery);
			foreach (ManagementObject Process in WMISearcher.Get())
            {		
                this.ID = Process["ProcessId"].ToString();
                Console.WriteLine("Name: " + Process["Name"] + " PID: " + Process["ProcessId"]); 
            }
            int PID = int.Parse(this.ID);
            return PID;
        }

        public void Exec(string Command, string Parms)
        {
            int ID;
            Process CMD = new Process();
            CMD.StartInfo.FileName = Command;
            CMD.StartInfo.Arguments = Parms;
            CMD.StartInfo.UseShellExecute = false;
            CMD.Start();
            ID = CMD.Id;
        }
    }
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
            string KeyOne = "QVG72";
            string KeyTwo = "JKDK4";
            string KeyThree = "FR3Q8";
            string KeyFour = "JKXGC";
            string KeyFive = "FDG7M";

            try
            {
                string MOM10SP1 = @"D:\Retail\Intel\OMSetup.exe";
                Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;

                Utilities U = new Utilities();
                U.Exec(MOM10SP1, "");
  
                Thread.Sleep(5000);

                int id = U.GetProcessID("MOM_SETUP_DERANDOMIZED.EXE");

                Process P = Process.GetProcessById(id);
                
                Console.WriteLine("Windows Title: " + P.MainWindowTitle);
                Console.WriteLine("Main Window Handle: " + P.MainWindowHandle);
                Console.WriteLine("Handle: " + P.Handle);
                Console.WriteLine("Process Name: " + P.ProcessName);
                Console.WriteLine("Process ID: " + P.Id);

                Maui.OS.App app = new Maui.OS.App(id, false);
                Console.WriteLine(app.IsRunning);

                MicrosoftOperationsManagerSetupWelcomeDialog Welcome = new MicrosoftOperationsManagerSetupWelcomeDialog(app);
                Welcome.ClickNext();

                Thread.Sleep(3000);

                MicrosoftOperationsManagerSetupRegistrationInformationDialog Register = new MicrosoftOperationsManagerSetupRegistrationInformationDialog(app);
                Register.Intheboxesbelowtypeyour25characterCDkeyYoullfindthisnumberontheyellowstickeronthebackoftheCDjewelcasText = KeyOne;
                Register._Text = KeyTwo;
                Register._2Text = KeyThree;
                Register._3Text = KeyFour;
                Register._4Text = KeyFive;
                Register.ClickNext();

                Thread.Sleep(3000);

                MicrosoftOperationsManagerSetupLicenseAgreementDialog License = new MicrosoftOperationsManagerSetupLicenseAgreementDialog(app);
                License.ClickIacceptthetermsinthelicenseagreement();
                Thread.Sleep(2000);
                License.ClickNext();

                Thread.Sleep(5000);

                MicrosoftOperationsManagerSetupDestinationDirectoryDialog Destination = new MicrosoftOperationsManagerSetupDestinationDirectoryDialog(app);
                Destination.ClickNext();

                Thread.Sleep(5000);
                
                MicrosoftOperationsManagerSetupInstallationTypeDialog InstallType = new MicrosoftOperationsManagerSetupInstallationTypeDialog(app);
                InstallType.ClickTypical();
                InstallType.ClickNext();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Source);
            }
		}
	}
}
