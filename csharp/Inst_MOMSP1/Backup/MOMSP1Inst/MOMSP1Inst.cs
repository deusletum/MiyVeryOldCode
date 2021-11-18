//-------------------------------------------------------------------
///

///

///
/// <summary>
///    A class library to install MOM10SP1 using MAUI UI automation
/// </summary>
/// 
/// <history>
///     <record date="22-Jul-03" who="deangj">
///     First Creation
///     </record>     
/// </history>
///
//-------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Threading;
using System.Text.RegularExpressions;
using Maui.Core;
using Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs;

namespace Microsoft.MOM.Test.Operations.MOMSP1Inst
{
    #region Utilites Class
    /// <summary>
    /// This class contain utility method such as check if SQL Servier is install and getting process ids.
    /// </summary>
    internal class Utilities
    {
        private string ID;
        private int BuildNum;
        private bool IsInstalled;
        
        /// <summary>
        /// Returns a process ID when given a process name
        /// </summary>
        /// <param name="ProcessName">Process Name</param>
        /// <returns>Process ID</returns>
        internal int GetProcessID(string ProcessName)
        {
            string WMIQuery = "SELECT * FROM Win32_Process WHERE Name=\"" 
                + ProcessName + "\"";

            ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher(WMIQuery);
            foreach (ManagementObject Process in WMISearcher.Get())
            {		
                this.ID = Process["ProcessId"].ToString();
            }
            int PID = int.Parse(this.ID);
            return PID;
        }
        /// <summary>
        /// Gets Process ID for Wise Installer sponed process GLB**.tmp in Windows 2000
        /// </summary>
        /// <returns>Process ID</returns>
        internal int GetMOMW2KProcessID()
        {
            string WMIQuery = "SELECT * FROM Win32_Process";

            ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher(WMIQuery);
            foreach (ManagementObject Process in WMISearcher.Get())
            {	
                Regex GLB = new Regex(@"GLB[1-9A-Za-z]{1,3}\.tmp");
                if(GLB.IsMatch(Process["Name"].ToString()))
                {
                    this.ID = Process["ProcessId"].ToString();
                }
            }
            int PID = int.Parse(this.ID);
            return PID;
        }
        /// <summary>
        /// Executes a command or application
        /// </summary>
        /// <param name="Command">Name of the command</param>
        /// <param name="Parms">Parameters for the command</param>
        internal void Exec(string Command, string Parms)
        {
            int ID;
            Process CMD = new Process();
            CMD.StartInfo.FileName = Command;
            CMD.StartInfo.Arguments = Parms;
            CMD.StartInfo.UseShellExecute = false;
            CMD.Start();
            ID = CMD.Id;
        }
        /// <summary>
        /// Get the build number of Windows
        /// </summary>
        /// <returns>Returns the build number of Windows</returns>
        internal int GetOSBuildNum()
        {
            string WMIQuery = "SELECT * FROM Win32_OperatingSystem";

            ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher(WMIQuery);
            foreach (ManagementObject OsInfo in WMISearcher.Get())
            {		
                this.BuildNum = int.Parse(OsInfo["BuildNumber"].ToString());
            }
            return this.BuildNum;
        }
        
        /// <summary>
        /// Checks if SQL is installed
        /// </summary>
        /// <returns>true if SQL is installed and false if not</returns>
        internal bool CheckForSQL()
        {
            string SQLWMIQuery = "SELECT * FROM Win32_Service";
            ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher(SQLWMIQuery);
            foreach( ManagementObject Service in WMISearcher.Get())
            {
                Regex SQL = new Regex("MSSQL");
                if(SQL.IsMatch(Service["Name"].ToString().ToUpper()))
                {
                    this.IsInstalled = true;
                    break;
                }
                else
                {
                    this.IsInstalled = false;
                }
            }
            return this.IsInstalled;
        }
    }
    #endregion

    #region MOMSP1Dialogs Class
    /// <summary>
    /// This class holds the methods to process MOMSP1 dialogs
    /// </summary>
    class MOMSP1Dialogs
    {
        private App MAUIApp;
        private string Exchange = "Microsoft Exchange";
        private string SMTP = "SMTP";
        
        #region Dialogs Constructor
        /// <summary>
        /// Class containing Methods to execute and manipulate MOMSP1 install dialogs. This includes clicking buttons and entering values
        /// </summary>
        /// <param name="app">MAUI App object</param>
        internal MOMSP1Dialogs(App app)
        {
            this.MAUIApp = app;
        }
        #endregion

        #region Welcome Dialog Method
        /// <summary>
        /// Processes the MOMSP1 Welcome Dialog
        /// </summary>
        internal void WelcomeDialog()
        {
            // Process the Welcome Dialog
            try
            {
                WelcomeDialog Welcome = new WelcomeDialog(this.MAUIApp);
                Welcome.ClickNext();
                Thread.Sleep(3000);
            }
            catch (WelcomeDialogNotFoundException)
            {
                Window ForegroundWindow = new Window(WindowType.Foreground);
                if(ForegroundWindow.Caption == "Incomplete Installation Detected")
                {
                    try
                    {
                        IncompleteInstallationDetectedDialog Incomplete = new IncompleteInstallationDetectedDialog(this.MAUIApp);
                        Incomplete.ClickContinue();
                        Thread.Sleep(5000);

                        WelcomeDialog Welcome = new WelcomeDialog(this.MAUIApp);
                        Welcome.ClickNext();
                        Thread.Sleep(3000);
                    }
                    catch(IncompleteInstallationDetectedDialogNotFoundException)
                    {
                        throw new WelcomeDialogNotFoundException("Welecome Dialog not found. Window " + ForegroundWindow.Caption
                            + " is in the foreground.");
                    }
                }
                else
                {
                    this.MAUIApp.BringToForeground();
                    try
                    {
                        WelcomeDialog Welcome = new WelcomeDialog(this.MAUIApp);
                        Welcome.ClickNext();
                        Thread.Sleep(3000);
                    }
                    catch(WelcomeDialogNotFoundException)
                    {
                        throw new WelcomeDialogNotFoundException("Welecome Dialog not found. Window " + ForegroundWindow.Caption
                            + " is in the foreground.");
                    }
                }
            }
        }

        #endregion

        #region Registration Information Dialog Methods
        /// <summary>
        /// Processes the MOMSP1 Registration Informatin Dialog
        /// </summary>
        /// <param name="Keys">MOMSP1 PID Keys</param>
        internal void RegistrationDialog(string[] Keys)
        {
            try
            {
                RegistrationInformationDialog RegDialog = new RegistrationInformationDialog(this.MAUIApp);
                RegDialog.InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasText = Keys[0];
                RegDialog._Text = Keys[1];
                RegDialog._2Text = Keys[2];
                RegDialog._3Text = Keys[3];
                RegDialog._4Text = Keys[4];
                RegDialog.ClickNext();
                Thread.Sleep(5000);
            }
            catch(RegistrationInformationDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    RegistrationInformationDialog RegDialog = new RegistrationInformationDialog(this.MAUIApp);
                    RegDialog.InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasText = Keys[0];
                    RegDialog._Text = Keys[1];
                    RegDialog._2Text = Keys[2];
                    RegDialog._3Text = Keys[3];
                    RegDialog._4Text = Keys[4];
                    RegDialog.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(RegistrationInformationDialogNotFoundException)
                {
                    throw new RegistrationInformationDialogNotFoundException("Registration Information Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        /// <summary>
        /// Processes the MOMSP1 Registration Informatin Dialog
        /// </summary>
        internal void RegistrationDialog()
        {
            try
            {
                RegistrationInformationDialog RegDialog = new RegistrationInformationDialog(this.MAUIApp);
                RegDialog.ClickNext();
                Thread.Sleep(5000);
            }
            catch(RegistrationInformationDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    RegistrationInformationDialog RegDialog = new RegistrationInformationDialog(this.MAUIApp);
                    RegDialog.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(RegistrationInformationDialogNotFoundException)
                {
                    throw new RegistrationInformationDialogNotFoundException("Registration Information Select Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region License Agreement Dialog Method
        /// <summary>
        /// Process MOMSP1 License Agreement Dialog
        /// </summary>
        internal void LicenseDialog()
        {
            try
            {
                LicenseAgreementDialog License = new LicenseAgreementDialog(this.MAUIApp);
                License.ClickIacceptTheTermsInTheLicenseAgreement();
                Thread.Sleep(2000);
                License.ClickNext();
                Thread.Sleep(5000);
            }
            catch(LicenseAgreementDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    LicenseAgreementDialog License = new LicenseAgreementDialog(this.MAUIApp);
                    License.ClickIacceptTheTermsInTheLicenseAgreement();
                    Thread.Sleep(2000);
                    License.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(LicenseAgreementDialogNotFoundException)
                {
                    throw new LicenseAgreementDialogNotFoundException("License Agreement Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Destination Directory Dialog Methods
        /// <summary>
        /// Destination Directory Dialog for MOMSP1
        /// </summary>
        /// <param name="Dir">Destination Directory</param>
        internal void DestinationDirectory(string Dir)
        {
            try
            {
                DestinationDirectoryDialog Dest = new DestinationDirectoryDialog(this.MAUIApp);
                Dest.ClickBrowse();
                Thread.Sleep(2000);
                try
                {
                    SelectDestinationDirectoryDialog SelectDir = new SelectDestinationDirectoryDialog(this.MAUIApp);
                    SelectDir.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesText = Dir;
                    SelectDir.ClickOK();
                    Thread.Sleep(2000);
                }
                catch(SelectDestinationDirectoryDialogNotFoundException)
                {
                    this.MAUIApp.BringToForeground();
                    Window ForegroundWindowTwo = new Window(WindowType.Foreground);
                    try
                    {
                        SelectDestinationDirectoryDialog SelectDir = new SelectDestinationDirectoryDialog(this.MAUIApp);
                        SelectDir.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesText = Dir;
                        SelectDir.ClickOK();
                        Thread.Sleep(2000);
                    }
                    catch(SelectDestinationDirectoryDialogNotFoundException)
                    {
                        throw new SelectDestinationDirectoryDialogNotFoundException("Select Destination Directory Dialog not found. Window "
                            + ForegroundWindowTwo.Caption + " is in the foreground.");
                    }
                }
                Dest.ClickNext();
                Thread.Sleep(5000);
            }
            catch(DestinationDirectoryDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    DestinationDirectoryDialog Dest = new DestinationDirectoryDialog(this.MAUIApp);
                    Dest.ClickBrowse();
                    Thread.Sleep(2000);
                    try
                    {
                        SelectDestinationDirectoryDialog SelectDir = new SelectDestinationDirectoryDialog(this.MAUIApp);
                        SelectDir.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesText = Dir;
                        SelectDir.ClickOK();
                        Thread.Sleep(2000);
                    }
                    catch(SelectDestinationDirectoryDialogNotFoundException)
                    {
                        this.MAUIApp.BringToForeground();
                        Window ForegroundWindowTwo = new Window(WindowType.Foreground);
                        try
                        {
                            SelectDestinationDirectoryDialog SelectDir = new SelectDestinationDirectoryDialog(this.MAUIApp);
                            SelectDir.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesText = Dir;
                            SelectDir.ClickOK();
                            Thread.Sleep(2000);
                        }
                        catch(SelectDestinationDirectoryDialogNotFoundException)
                        {
                            throw new SelectDestinationDirectoryDialogNotFoundException("Select Destination Directory Dialog not found. Window "
                                + ForegroundWindowTwo.Caption + " is in the foreground.");
                        }
                    }
                    Dest.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(DestinationDirectoryDialogNotFoundException)
                {
                    throw new DestinationDirectoryDialogNotFoundException("Destination Directory Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        
        /// <summary>
        /// The Destination Directory for MOMSP1
        /// </summary>
        internal void DestinationDirectory()
        {
            try
            {
                DestinationDirectoryDialog Dest = new DestinationDirectoryDialog(this.MAUIApp);
                Dest.ClickNext();
                Thread.Sleep(5000);
            }
            catch(DestinationDirectoryDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    DestinationDirectoryDialog Dest = new DestinationDirectoryDialog(this.MAUIApp);
                    Dest.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(DestinationDirectoryDialogNotFoundException)
                {
                    throw new DestinationDirectoryDialogNotFoundException("Destination Directory Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Install Type Dialog Method
        /// <summary>
        /// MOMSP1 Install Type Dialog
        /// </summary>
        /// <param name="Type">The MOMSP1 Install type</param>
        internal void InstallType(InstallTypes Type)
        {
            try
            {
                InstallationTypeDialog MOMInstallType = new InstallationTypeDialog(this.MAUIApp);
                switch (Type)
                {
                    case MOMSP1Dialogs.InstallTypes.Typical:
                        MOMInstallType.ClickTypical();
                        break;
                    case MOMSP1Dialogs.InstallTypes.User_Interfaces:
                        MOMInstallType.ClickUserInterfaces();
                        break;
                    case MOMSP1Dialogs.InstallTypes.Custom:
                        MOMInstallType.ClickCustom();
                        break;
                    case MOMSP1Dialogs.InstallTypes.Express:
                        MOMInstallType.ClickExpress();
                        break;
                }
                Thread.Sleep(2000);
                MOMInstallType.ClickNext();
                Thread.Sleep(5000);
            }
            catch(InstallationTypeDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    InstallationTypeDialog MOMInstallType = new InstallationTypeDialog(this.MAUIApp);
                    switch (Type)
                    {
                        case MOMSP1Dialogs.InstallTypes.Typical:
                            MOMInstallType.ClickTypical();
                            break;
                        case MOMSP1Dialogs.InstallTypes.User_Interfaces:
                            MOMInstallType.ClickUserInterfaces();
                            break;
                        case MOMSP1Dialogs.InstallTypes.Custom:
                            MOMInstallType.ClickCustom();
                            break;
                        case MOMSP1Dialogs.InstallTypes.Express:
                            MOMInstallType.ClickExpress();
                            break;
                    }
                    Thread.Sleep(2000);
                    MOMInstallType.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(InstallationTypeDialogNotFoundException)
                {
                    throw new InstallationTypeDialogNotFoundException("Install Type Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Database Instance Dialog Method
        /// <summary>
        /// MOMSP1 Database Instance Dialog
        /// </summary>
        internal void DBInstance(string InstanceName)
        {
            Utilities Tools = new Utilities();
            if(Tools.CheckForSQL())
            {
                try
                {
                    DatabaseInstanceDialog DB = new DatabaseInstanceDialog(this.MAUIApp);
                    DB.AccountText = InstanceName;
                    Thread.Sleep(2000);
                    DB.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(DatabaseInstanceDialogNotFoundException)
                {
                    this.MAUIApp.BringToForeground();
                    Window ForegroundWindow = new Window(WindowType.Foreground);
                    try
                    {
                        DatabaseInstanceDialog DB = new DatabaseInstanceDialog(this.MAUIApp);
                        DB.AccountText = InstanceName;
                        Thread.Sleep(2000);
                        DB.ClickNext();
                        Thread.Sleep(5000);
                    }
                    catch(DatabaseInstanceDialogNotFoundException)
                    {
                        throw new DatabaseInstanceDialogNotFoundException("Database Instance Dialog not found. Window "
                            + ForegroundWindow.Caption + " is in the foreground.");
                    }
                }
            }
            else
            {
                throw new SQLNotInstalledException("SQL Server or MSDE was not found on this computer");
            }
        }
        #endregion

        #region DAS Account Dialog Method
        /// <summary>
        /// The DAS Account Dialog
        /// </summary>
        /// <param name="Account">Account user name</param>
        /// <param name="Domain">Domain name</param>
        /// <param name="Password">Password for user</param>
        /// <param name="UseDAS">To use Account user information for both DAS and CAM.</param>
        internal void DasAccount(string Account, string Domain, string Password, bool UseDAS)
        {
            try
            {
                DASAccountInformationDialog DAS = new DASAccountInformationDialog(this.MAUIApp);
                DAS.AccountText = Account;
                DAS.PasswordText = Password;
                DAS.DomainOrLocalComputerText = Domain;
                if(!UseDAS)
                {
                    DAS.UseThisAccountForConsolidatorAgentManagerComponent = false;
                }
                DAS.ClickNext();
                Thread.Sleep(5000);
                Window AccountValidation = new Window(WindowType.Foreground);
                if(AccountValidation.Caption == "Account verification")
                {
                    throw new DASAccountValidationException("The DAS account information cannot be validated by MOMSP1");
                }
            }
            catch(DASAccountInformationDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    DASAccountInformationDialog DAS = new DASAccountInformationDialog(this.MAUIApp);
                    DAS.AccountText = Account;
                    DAS.PasswordText = Password;
                    DAS.DomainOrLocalComputerText = Domain;
                    if(!UseDAS)
                    {
                        DAS.UseThisAccountForConsolidatorAgentManagerComponent = false;
                    }
                    DAS.ClickNext();
                    Thread.Sleep(5000);
                    Window AccountValidation = new Window(WindowType.Foreground);
                    if(AccountValidation.Caption == "Account verification")
                    {
                        throw new DASAccountValidationException("The DAS account information cannot be validated by MOMSP1");
                    }
                }
                catch(DASAccountInformationDialogNotFoundException)
                {
                    throw new DASAccountInformationDialogNotFoundException("DAS Account Information Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Consolidator Account Dialog Mothod
        /// <summary>
        /// Consolidator Account Dialog
        /// </summary>
        /// <param name="Account">Account user name</param>
        /// <param name="Domain">Domain name</param>
        /// <param name="Password">Password for user</param>
        internal void ConsolidatorAccount(string Account, string Domain, string Password)
        {
            try
            {
                ConsolidatorAccountDetailsDialog Consolidator = new ConsolidatorAccountDetailsDialog(this.MAUIApp);
                Consolidator.AccountText = Account;
                Consolidator.PasswordText = Password;
                Consolidator.DomainOrLocalComputerText = Domain;
                Consolidator.ClickNext();
                Thread.Sleep(5000);
            }
            catch(ConsolidatorAccountDetailsDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    ConsolidatorAccountDetailsDialog Consolidator = new ConsolidatorAccountDetailsDialog(this.MAUIApp);
                    Consolidator.AccountText = Account;
                    Consolidator.PasswordText = Password;
                    Consolidator.DomainOrLocalComputerText = Domain;
                    Consolidator.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(ConsolidatorAccountDetailsDialogNotFoundException)
                {
                    throw new ConsolidatorAccountDetailsDialogNotFoundException("Consolidator Account Details Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Install System Files Dialog Method
        /// <summary>
        /// Install System Files Dialog
        /// </summary>
        internal void InstallSystemFiles()
        {
            try
            {
                InstallSystemFilesDialog InstallSys = new InstallSystemFilesDialog(this.MAUIApp);
                InstallSys.ClickNext();
                Thread.Sleep(5000);
            }
            catch(InstallSystemFilesDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    InstallSystemFilesDialog InstallSys = new InstallSystemFilesDialog(this.MAUIApp);
                    InstallSys.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(InstallSystemFilesDialogNotFoundException)
                {
                    throw new InstallSystemFilesDialogNotFoundException("Install System Files Details Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Create Security Groups Dialog Method
        /// <summary>
        /// Create Security Groups Dialog
        /// </summary>
        internal void CreateSeurityGroups()
        {
            try
            {
                CreateSecurityGroupsDialog SGroups = new CreateSecurityGroupsDialog(this.MAUIApp);
                SGroups.ClickNext();
                Thread.Sleep(15000);
            }
            catch(CreateSecurityGroupsDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    CreateSecurityGroupsDialog SGroups = new CreateSecurityGroupsDialog(this.MAUIApp);
                    SGroups.ClickNext();
                    Thread.Sleep(25000);
                }
                catch(CreateSecurityGroupsDialogNotFoundException)
                {
                    throw new CreateSecurityGroupsDialogNotFoundException("Create Security Groups Details Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Database and Log File Size Dialog Methods
        /// <summary>
        /// Database and Log File Size Dialog
        /// </summary>
        /// <param name="DBSize">Database file size in MB</param>
        /// <param name="LogSize">Log file size in MB</param>
        /// <param name="DBFile">Database file location</param>
        /// <param name="LogFile">Log file location</param>
        internal void DBLogSize(string DatabaseSize, string LogSize, string DBFile, string LogFile)
        {
            try
            {
                DatabaseandLogFileSizeDialog DBSize = new DatabaseandLogFileSizeDialog(this.MAUIApp);
                if(DatabaseSize != null)
                {
                    DBSize.EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinText = DatabaseSize;
                }
                if(LogSize != null)
                {
                    DBSize.SizeText = LogSize;
                }
                if(DBFile != null)
                {
                    DBSize.LocationText = DBFile;
                }
                if(LogFile != null)
                {
                    DBSize.TextBox0Text = LogFile;
                }
                DBSize.ClickNext();
                Thread.Sleep(5000);
            }
            catch(DatabaseandLogFileSizeNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    DatabaseandLogFileSizeDialog DBSize = new DatabaseandLogFileSizeDialog(this.MAUIApp);
                    if(DatabaseSize != null)
                    {
                        DBSize.EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinText = DatabaseSize;
                    }
                    if(LogSize != null)
                    {
                        DBSize.SizeText = LogSize;
                    }
                    if(DBFile != null)
                    {
                        DBSize.LocationText = DBFile;
                    }
                    if(LogFile != null)
                    {
                        DBSize.TextBox0Text = LogFile;
                    }
                    DBSize.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(DatabaseandLogFileSizeNotFoundException)
                {
                    throw new DatabaseandLogFileSizeNotFoundException("Database File and Log File Size Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Start Service Dialog Method
        /// <summary>
        /// Start Service Dialog
        /// </summary>
        /// <param name="AutoStart">Set the OnePoint Service to Auto Start</param>
        /// <param name="StartAfterSetup">Start the Onepoint Service after setup completes</param>
        internal void StartService(bool AutoStart, bool StartAfterSetup)
        {
            try
            {
                StartServiceDialog Start = new StartServiceDialog(this.MAUIApp);
                if(!AutoStart)
                {
                    Start.ClickAutomaticallyStartTheServiceEveryTimeTheComputerStarts();
                }
                if(!StartAfterSetup)
                {
                    Start.ClickStartTheServiceAfterSetupFinishes();
                }
                Start.ClickNext();
                Thread.Sleep(5000);
            }
            catch(StartServiceDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    StartServiceDialog Start = new StartServiceDialog(this.MAUIApp);
                    if(!AutoStart)
                    {
                        Start.ClickAutomaticallyStartTheServiceEveryTimeTheComputerStarts();
                    }
                    if(!StartAfterSetup)
                    {
                        Start.ClickStartTheServiceAfterSetupFinishes();
                    }
                    Start.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(StartServiceDialogNotFoundException)
                {
                    throw new StartServiceDialogNotFoundException("Start Service Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Configuration Group Name Dialog Method
        /// <summary>
        /// Configuration Group Name Dialog
        /// </summary>
        /// <param name="Name">Configuration Group Name</param>
        internal void ConfigGroup(string Name)
        {
            try
            {
                ConfigurationGroupNameDialog Config = new ConfigurationGroupNameDialog(this.MAUIApp);
                Config.MBText = Name;
                Config.ClickNext();
            }
            catch(ConfigurationGroupNameDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    ConfigurationGroupNameDialog Config = new ConfigurationGroupNameDialog(this.MAUIApp);
                    Config.MBText = Name;
                    Config.ClickNext();
                }
                catch(ConfigurationGroupNameDialogNotFoundException)
                {
                    throw new StartServiceDialogNotFoundException("Configuration Group Name Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Web Console Server File Location Dialog Method
        /// <summary>
        /// Web Console Server File Location Dialog
        /// </summary>
        /// <param name="Location">Web Console Server File Location</param>
        internal void WebConsoleLocation(string Location)
        {
            try
            {
                WebConsoleServerFileLocationsDialog Web = new WebConsoleServerFileLocationsDialog(this.MAUIApp);
                Web.NameText = Location;
                Web.ClickNext();
                Thread.Sleep(5000);
            }
            catch(WebConsoleServerFileLocationsDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    WebConsoleServerFileLocationsDialog Web = new WebConsoleServerFileLocationsDialog(this.MAUIApp);
                    Web.NameText = Location;
                    Web.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(WebConsoleServerFileLocationsDialogNotFoundException)
                {
                    throw new WebConsoleServerFileLocationsDialogNotFoundException("Web Console Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        /// <summary>
        /// Web Console Server File Location Dialog
        /// </summary>
        internal void WebConsoleLocation()
        {
            try
            {
                WebConsoleServerFileLocationsDialog Web = new WebConsoleServerFileLocationsDialog(this.MAUIApp);
                Web.ClickNext();
                Thread.Sleep(5000);
            }
            catch(WebConsoleServerFileLocationsDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    WebConsoleServerFileLocationsDialog Web = new WebConsoleServerFileLocationsDialog(this.MAUIApp);
                    Web.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(WebConsoleServerFileLocationsDialogNotFoundException)
                {
                    throw new WebConsoleServerFileLocationsDialogNotFoundException("Web Console Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Update Browscap.ini Dialog Method
        /// <summary>
        /// Update Browscap.ini Dialog
        /// </summary>
        /// <param name="Update">Update Browscap.ini</param>
        internal void UpdateBrowscap(bool Update)
        {
            try
            {
                UpdateBrowscap_iniDialog Browscap = new UpdateBrowscap_iniDialog(this.MAUIApp);
                if(Update)
                {
                    Browscap.RadioGroup0 = RadioGroup0.Yes;
                }
                else
                {
                    Browscap.RadioGroup0 = RadioGroup0.No;
                }
                Browscap.ClickNext();
                Thread.Sleep(5000);
            }
            catch(UpdateBrowscap_iniDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    UpdateBrowscap_iniDialog Browscap = new UpdateBrowscap_iniDialog(this.MAUIApp);
                    if(Update)
                    {
                        Browscap.RadioGroup0 = RadioGroup0.Yes;
                    }
                    else
                    {
                        Browscap.RadioGroup0 = RadioGroup0.No;
                    }
                    Browscap.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(UpdateBrowscap_iniDialogNotFoundException)
                {
                    throw new UpdateBrowscap_iniDialogNotFoundException("Update Browscap.ini Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region E-mail Account Dialog Method
        /// <summary>
        /// E-mail Account Dialog
        /// </summary>
        internal void EmailAccount()
        {
            try
            {
                EMailAccountDialog Email = new EMailAccountDialog(this.MAUIApp);
                Email.ClickNext();
                Thread.Sleep(5000);
            }
            catch(EMailAccountDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    EMailAccountDialog Email = new EMailAccountDialog(this.MAUIApp);
                    Email.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(EMailAccountDialogNotFoundException)
                {
                    throw new EMailAccountDialogNotFoundException("E-mail Account Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        /// <summary>
        /// E-mail Account Dialog, Exchange server email transport
        /// </summary>
        /// <param name="ExchangeServer">Exchange server name</param>
        /// <param name="Mailbox">Exchange server mailbox name</param>
        internal void EmailAccount(string ExchangeServer, string Mailbox )
        {
            try
            {
                EMailAccountDialog Email = new EMailAccountDialog(this.MAUIApp);
                Email.TransportText = this.Exchange;
                Email.ExchangeServerText = ExchangeServer;
                Email.MailboxText = Mailbox;
                Email.ClickNext();
                Thread.Sleep(5000);
            }
            catch(EMailAccountDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    EMailAccountDialog Email = new EMailAccountDialog(this.MAUIApp);
                    Email.TransportText = this.Exchange;
                    Email.ExchangeServerText = ExchangeServer;
                    Email.MailboxText = Mailbox;
                    Email.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(EMailAccountDialogNotFoundException)
                {
                    throw new EMailAccountDialogNotFoundException("E-mail Account Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        /// <summary>
        /// E-mail Account Dialog, SMTP server email transport
        /// </summary>
        /// <param name="ReturnAddress">SMTP Return Address</param>
        /// <param name="ServerName">SMTP Server Name</param>
        /// <param name="SMTPPort">SMTP Port Number</param>
        internal void EmailAccount(string ReturnAddress, string ServerName, string SMTPPort)
        {
            try
            {
                EMailAccountSMTPDialog Email = new EMailAccountSMTPDialog(this.MAUIApp);
                Email.TransportText = this.SMTP;
                Email.ReturnAddressText = ReturnAddress;
                Email.ServerNameText = ServerName;
                Email.SMTPPortText = SMTPPort;
                Email.ClickNext();
                Thread.Sleep(5000);
            }
            catch(EMailAccountDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    EMailAccountSMTPDialog Email = new EMailAccountSMTPDialog(this.MAUIApp);
                    Email.TransportText = this.SMTP;
                    Email.ReturnAddressText = ReturnAddress;
                    Email.ServerNameText = ServerName;
                    Email.SMTPPortText = SMTPPort;
                    Email.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(EMailAccountDialogNotFoundException)
                {
                    throw new EMailAccountDialogNotFoundException("E-mail Account Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Managment Pack Modules Dialog Method
        /// <summary>
        /// Managment Pack Modules Dialog
        /// </summary>
        /// <param name="InstallMPs">Install all Management Packs or None</param>
        internal void ManagmentPackModules(bool InstallMPs)
        {
            try
            {
                ManagementPackModulesDialog Man = new ManagementPackModulesDialog(this.MAUIApp);
                if(!InstallMPs)
                {
                    Man.ClickClearAll();
                }
                Man.ClickNext();
                Thread.Sleep(5000);
            }
            catch(ManagementPackModulesDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    ManagementPackModulesDialog Man = new ManagementPackModulesDialog(this.MAUIApp);
                    if(!InstallMPs)
                    {
                        Man.ClickClearAll();
                    }
                    Man.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(ManagementPackModulesDialogNotFoundException)
                {
                    throw new ManagementPackModulesDialogNotFoundException("Management Pack Modules Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Start Copying Files Dialog Method
        /// <summary>
        /// Start Copying Files Dialog
        /// </summary>
        internal void StartCopying()
        {
            try
            {
                StartCopyingFilesDialog Start = new StartCopyingFilesDialog(this.MAUIApp);
                Start.ClickNext();
                Thread.Sleep(5000);
            }
            catch(StartCopyingFilesDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    StartCopyingFilesDialog Start = new StartCopyingFilesDialog(this.MAUIApp);
                    Start.ClickNext();
                    Thread.Sleep(5000);
                }
                catch(StartCopyingFilesDialogNotFoundException)
                {
                    throw new StartCopyingFilesDialogNotFoundException("Start Copying Files Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Stop Services Dialog Method
        /// <summary>
        /// Stop Services Dialog
        /// </summary>
        internal void StopServices()
        {
            try
            {
                StopServicesDialog Stop = new StopServicesDialog(this.MAUIApp);
                Stop.ClickContinue();
                Thread.Sleep(1200000);
            }
            catch(StopServicesDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    StopServicesDialog Stop = new StopServicesDialog(this.MAUIApp);
                    Stop.ClickContinue();
                    Thread.Sleep(1200000);
                }
                catch(StopServicesDialogNotFoundException)
                {
                    throw new StopServicesDialogNotFoundException("Stop Services Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Setup Finished Dialog Method
        /// <summary>
        /// Setup Finished Dialog
        /// </summary>
        internal void SetupFinished()
        {
            try
            {
                SetupFinishedDialog Finish = new SetupFinishedDialog(this.MAUIApp);
                Finish.ViewManagementPackInstallationLogAfterSetupExits = false;
                Finish.ClickFinish();
                Thread.Sleep(5000);
            }
            catch(SetupFinishedDialogNotFoundException)
            {
                this.MAUIApp.BringToForeground();
                Window ForegroundWindow = new Window(WindowType.Foreground);
                try
                {
                    SetupFinishedDialog Finish = new SetupFinishedDialog(this.MAUIApp);
                    Finish.ViewManagementPackInstallationLogAfterSetupExits = false;
                    Finish.ClickFinish();
                    Thread.Sleep(5000);
                }
                catch(SetupFinishedDialogNotFoundException)
                {
                    throw new SetupFinishedDialogNotFoundException("Setup Finished Dialog not found. Window "
                        + ForegroundWindow.Caption + " is in the foreground.");
                }
            }
        }
        #endregion

        #region Enum InstallTypes
        /// <summary>
        /// MOMSP1 Install Types
        /// </summary>
        internal enum InstallTypes
        {
            Typical = 1,
            User_Interfaces = 2,
            Custom = 3,
            Express = 4
        }
        #endregion
    }
    #endregion

    #region Exceptions
    /// <summary>
    /// Represents an error of MOMSP1 Invalid Product Key
    /// </summary>
    class InvalidProductKeyException: System.Exception
    {
        public InvalidProductKeyException()
        {
        }
        public InvalidProductKeyException(string message): base(message)
        {
        }
        public InvalidProductKeyException(string message, Exception inner): base(message, inner)
        {
        }
    }
    /// <summary>
    /// Represents an error of MOMSP1 unable to validate the DAS account information
    /// </summary>
    class DASAccountValidationException: System.Exception
    {
        public DASAccountValidationException()
        {
        }
        public DASAccountValidationException(string message): base(message)
        {
        }
        public DASAccountValidationException(string message, Exception inner): base(message, inner)
        {
        }
    }
    /// <summary>
    /// Represents an error of MOMSP1 unable to validate the CAM account information
    /// </summary>
    class ConsolidatorAccountValidationException: System.Exception
    {
        public ConsolidatorAccountValidationException()
        {
        }
        public ConsolidatorAccountValidationException(string message): base(message)
        {
        }
        public ConsolidatorAccountValidationException(string message, Exception inner): base(message, inner)
        {
        }
    }
    /// <summary>
    /// Represents an error of SQL not being installed
    /// </summary>
    class SQLNotInstalledException: System.Exception
    {
        public SQLNotInstalledException()
        {
        }
        public SQLNotInstalledException(string message): base(message)
        {
        }
        public SQLNotInstalledException(string message, Exception inner): base(message, inner)
        {
        }
    }
    #endregion

    #region MOMSP1_Install Class
    /// <summary>
    /// Abstract class for MOMSP1 common properties
    /// </summary>
    public abstract class MOMSP1_Install
    {
        #region Private member Variables
        private string IFile;
        private string DUser;
        private string DPassword;
        private string DDomain;
        private bool UseCAM;
        private string CUser;
        private string CPassword;
        private string CDomain;
        private string PID;
        private string Computer;
        private int OSBuild;
        private string W2K3SetupFile;
        private bool Select;
        private string Destinaton;
        private string DBInstance;
        private string ConfigName;
        private bool AllManagementPacks;
        private string DBFileSize;
        private string DBLogFileSize;
        private string DBFileLoc;
        private string DBLogFileLoc;
        private bool UseExchange;
        private string ExchangeServer;
        private string Mailbox;
        private bool UseSMTP;
        private string SMTPServer;
        private string ReturnAddress;
        private string SMTPPort;
        private bool AutoStart;
        private bool StartAfterSetup;
        private string WebConsoleLoc;
        private bool UpdateBrowscapINI;
        #endregion

        #region MOMSP1_Install Class constructor
        protected MOMSP1_Install()
        {
            this.PID = @"QVG72-JKDK4-FR3Q8-JKXGC-FDG7M";
            this.W2K3SetupFile = "MOM_SETUP_DERANDOMIZED.EXE";
            this.UseCAM = false;
            this.Select = false;
            this.UseExchange = false;
            this.UseSMTP = false;
            this.AutoStart = true;
            this.StartAfterSetup = true;
            this.UpdateBrowscapINI = true;
            this.InstallManagementPacks = true;

            Process CurrentProcess = new Process();
            StringDictionary Env = CurrentProcess.StartInfo.EnvironmentVariables;
            this.Computer = Env["computername"];

            Utilities Tools = new Utilities();
            this.OSBuild = Tools.GetOSBuildNum();
        }
        #endregion

        #region "Properties"
        /// <summary>
        /// Update IIS Browscap INI File
        /// </summary>
        public bool UpdateBrowscapINIFile
        {
            get
            {
                return this.UpdateBrowscapINI;
            }
            set
            {
                this.UpdateBrowscapINI = value;
            }
        }
        /// <summary>
        /// MOMSP1 WebConsole Install Location
        /// </summary>
        public string WebConsoleLocation
        {
            get
            {
                return this.WebConsoleLoc;
            }
            set
            {
                this.WebConsoleLoc = value;
            }
        }
        /// <summary>
        /// Start the MOMSP1 Onepoint Service after Setup Finishes
        /// </summary>
        public bool StartServiceAfterSetupFinishes
        {
            get
            {
                return this.StartAfterSetup;
            }
            set
            {
                this.StartAfterSetup = value;
            }
        }
        /// <summary>
        /// Auto Start the MOMSP1 Onepoint Service
        /// </summary>
        public bool AutoStartOnePointService
        {
            get
            {
                return this.AutoStart;
            }
            set
            {
                this.AutoStart = value;
            }
        }
        /// <summary>
        /// SMTP Port Number
        /// </summary>
        public string SMTPPortNumber
        {
            get
            {
                return this.SMTPPort;
            }
            set
            {
                this.SMTPPort = value;
            }
        }
        /// <summary>
        /// SMTP Return Address Name
        /// </summary>
        public string SMTPReturnAddress
        {
            get
            {
                return this.ReturnAddress;
            }
            set
            {
                this.ReturnAddress = value;
            }
        }
        /// <summary>
        /// SMTP Server Name
        /// </summary>
        public string SMTPServerName
        {
            get
            {
                return this.SMTPServer;
            }
            set
            {
                this.SMTPServer = value;
            }
        }
        /// <summary>
        /// Microsoft Exchange Server Mailbox Name
        /// </summary>
        public string ExchangeMailboxName
        {
            get
            {
                return this.Mailbox;
            }
            set
            {
                this.Mailbox = value;
            }
        }
        /// <summary>
        /// Microsoft Exchange Server Name
        /// </summary>
        public string ExchangeServerName
        {
            get
            {
                return this.ExchangeServer;
            }
            set
            {
                this.ExchangeServer = value;
            }
        }
        /// <summary>
        /// Uses Microsoft Exchange Email in MOMSP1
        /// </summary>
        public bool UseExchangeEmail
        {
            get
            {
                return this.UseExchange;
            }
            set
            {
                this.UseExchange = value;
            }
        }
        /// <summary>
        /// Uses SMTP Email in MOMSP1
        /// </summary>
        public bool UseSMTPEmail
        {
            get
            {
                return this.UseSMTP;
            }
            set
            {
                this.UseSMTP = value;
            }
        }
        /// <summary>
        /// MOMSP1 Database Log File Location
        /// </summary>
        public string DatabaseLogFileLocation
        {
            get
            {
                return this.DBLogFileLoc;
            }
            set
            {
                this.DBLogFileLoc = value;
            }
        }
        /// <summary>
        /// MOMSP1 Database File Location
        /// </summary>
        public string DatabaseFileLocation
        {
            get
            {
                return this.DBFileLoc;
            }
            set
            {
                this.DBFileLoc = value;
            }
        }
        /// <summary>
        /// MOMSP1 Database Log File Size
        /// </summary>
        public string DatabaseLogFileSize
        {
            get
            {
                return this.DBLogFileSize;
            }
            set
            {
                this.DBLogFileSize = value;
            }
        }
        /// <summary>
        /// MOMSP1 Database File Size
        /// </summary>
        public string DatabaseFileSize
        {
            get
            {
                return this.DBFileSize;
            }
            set
            {
                this.DBFileSize = value;
            }
        }
        /// <summary>
        /// Path and filename of the MOMSP1 installation file (eg. omsetup.exe)
        /// Throw FileNotFoundException if file does not exist
        /// </summary>
        public string InstallFile
        {
            set
            {
                if(File.Exists(value))
                {
                    IFile = value;
                }
                else
                {
                    throw new FileNotFoundException("MOM10SP1 install file not found", value);
                }
            }
            get
            {
                return this.IFile;
            }
        }
        /// <summary>
        /// DAS usser account name
        /// </summary>
        public string DASUser
        {
            set
            {
                DUser = value;
            }
            get
            {
                return this.DUser;
            }
        }
        /// <summary>
        /// DAS user account password
        /// </summary>
        public string DASPassword
        {
            set
            {
                DPassword = value;
            }
            get
            {
                return this.DPassword;
            }
        }
        /// <summary>
        /// DAS user account domain
        /// </summary>
        public string DASDomain
        {
            set
            {
                DDomain = value;
            }
            get
            {
                return this.DDomain;
            }
        }
        /// <summary>
        /// Specifies that the CAM Account will be used.
        /// Default value is false.
        /// </summary>
        public bool UseCAMAccount
        {
            set
            {
                UseCAM = value;
            }
            get
            {
                return this.UseCAM;
            }
        }
        /// <summary>
        /// CAM user account name
        /// </summary>
        public string CAMUser
        {
            set
            {
                CUser = value;
            }
            get
            {
                return this.CUser;
            }
        }
        /// <summary>
        /// CAM user account password
        /// </summary>
        public string CAMPassword
        {
            set
            {
                CPassword = value;
            }
            get
            {
                return this.CPassword;
            }
        }
        /// <summary>
        /// CAM user account domain
        /// </summary>
        public string CAMDomain
        {
            set
            {
                CDomain = value;
            }
            get
            {
                return this.CDomain;
            }
        }
        /// <summary>
        /// MOMSP1 Product ID. This a 5 sets of five letter and numbers sperated by an '-'
        /// Default value is QVG72-JKDK4-FR3Q8-JKXGC-FDG7M
        /// </summary>
        public string ProductID
        {
            set
            {
                string PIDRegEx = @"[A-Z1-9]{5}?-[A-Z1-9]{5}?-[A-Z1-9]{5}?-[A-Z1-9]{5}?-[A-Z1-9]{5}?";
                Regex RegExObject = new Regex(PIDRegEx);
                if(RegExObject.IsMatch(value))
                {
                    
                    PID = value;
                }
                else
                {
                    throw new InvalidProductKeyException("The MOMSP1 Product Key is not invalid " + value);
                }
            }
            get
            {
                return this.PID;
            }
        }
        /// <summary>
        /// The name of the local computer
        /// </summary>
        public string ComputerName
        {
            get
            {
                return this.Computer;
            }
        }
        /// <summary>
        /// The build number of the local Operating System
        /// </summary>
        public int OSBuildNumber
        {
            get
            {
                return this.OSBuild;
            }
        }
        /// <summary>
        /// This is the process that OMSetup.exe spawns in Windows 2003
        /// </summary>
        public string W2K3SetupFileName
        {
            get
            {
                return this.W2K3SetupFile;
            }
        }
        /// <summary>
        /// The destination directory to install MOMSP1
        /// </summary>
        public string DestinationDirectory
        {
            set
            {
                this.Destinaton = value;
            }
            get
            {
                return this.Destinaton;
            }
        }
        /// <summary>
        /// Specifies if the is a Select build of MOMSP1
        /// </summary>
        public bool IsSelectBuild
        {
            set
            {
                this.Select = value;
            }
            get
            {
                return this.Select;
            }
        }
        /// <summary>
        /// Specifies the Database Instance MOMSP1 will use for the OnePoint Database
        /// </summary>
        public string DatabaseInstance
        {
            set
            {
                this.DBInstance = value;
            }
            get
            {
                return this.DBInstance;
            }
        }
        /// <summary>
        /// MOMSP1 Configuration Name
        /// </summary>
        public string ConfigurationName
        {
            set
            {
                this.ConfigName = value;
            }
            get
            {
                return this.ConfigName;
            }
        }
        /// <summary>
        /// Install all Management Packs or None
        /// </summary>
        public bool InstallManagementPacks
        {
            set
            {
                this.AllManagementPacks = value;
            }
            get
            {
                return this.AllManagementPacks;
            }
        }
        #endregion
    }

    #endregion

    #region Install_Typical Class
    public class Install_Typical: MOMSP1_Install
    {
        public Install_Typical()
        {
        }
    }
    #endregion

    #region Install Class
    /// <summary>
    /// class will be used to install MOMSP1
    /// </summary>
    public class Install
    {
        #region Private member Variables
        const int W2K = 2195;
        private string IFile;
        private string DUser;
        private string DPassword;
        private string DDomain;
        private string CUser;
        private string CPassword;
        private string CDomain;
        private string PID;
        private int OSBuild;
        private int MOMSP1_ProcessID;
        private string W2K3File;
        private bool Select;
        private string Dest;
        private string Computer;
        private string DatabaseInstance;
        private bool UseCAMInfo;
        private string DatabaseSize;
        private string DatabaseLogSize;
        private string DatabaseFileLocation;
        private string DatabaseLogFileLocation;
        private bool AutoStartService;
        private bool StartAfterSetup;
        private string ConfigGroup;
        private string WebConsoleLoc;
        private bool UpdateBrowscapINI;
        private bool UseExchange;
        private string ExchangeServer;
        private string Mailbox;
        private string SMTPServer;
        private string SMTPReturnAddress;
        private string SMTPPort;
        private bool UseSMTP;
        private bool InstallAllMPs;
        #endregion

        #region Methods
        /// <summary>
        /// Installs MOMSP1
        /// </summary>
        /// <param name="InstalParams">This is the Install Parameters object</param>
        public void InstallMOMSP1(Install_Typical InstallParams)
        {
            #region set varables
            this.IFile = InstallParams.InstallFile;
            this.DDomain = InstallParams.DASDomain;
            this.DPassword = InstallParams.DASPassword;
            this.DUser = InstallParams.DASUser;
            this.CDomain = InstallParams.CAMDomain;
            this.CPassword = InstallParams.CAMPassword;
            this.CUser = InstallParams.CAMUser;
            this.PID = InstallParams.ProductID;
            this.OSBuild = InstallParams.OSBuildNumber;
            this.W2K3File = InstallParams.W2K3SetupFileName;
            this.Select = InstallParams.IsSelectBuild;
            this.Dest = InstallParams.DestinationDirectory;
            this.Computer = InstallParams.ComputerName;
            this.DatabaseInstance = InstallParams.DatabaseInstance;
            this.UseCAMInfo = InstallParams.UseCAMAccount;
            this.DatabaseSize = InstallParams.DatabaseFileSize;
            this.DatabaseLogSize = InstallParams.DatabaseLogFileSize;
            this.DatabaseFileLocation = InstallParams.DatabaseFileLocation;
            this.DatabaseLogFileLocation = InstallParams.DatabaseLogFileLocation;
            this.AutoStartService = InstallParams.AutoStartOnePointService;
            this.StartAfterSetup = InstallParams.StartServiceAfterSetupFinishes;
            this.ConfigGroup = InstallParams.ConfigurationName;
            this.WebConsoleLoc = InstallParams.WebConsoleLocation;
            this.UpdateBrowscapINI = InstallParams.UpdateBrowscapINIFile;
            this.UseExchange = InstallParams.UseExchangeEmail;
            this.UseSMTP = InstallParams.UseSMTPEmail;
            this.ExchangeServer = InstallParams.ExchangeServerName;
            this.Mailbox = InstallParams.ExchangeMailboxName;
            this.SMTPServer = InstallParams.SMTPServerName;
            this.SMTPReturnAddress = InstallParams.SMTPReturnAddress;
            this.SMTPPort = InstallParams.SMTPPortNumber;
            this.InstallAllMPs = InstallParams.InstallManagementPacks;

            string Dash = "-";
            string[] Keys = this.PID.Split(Dash.ToCharArray(), 5);
            
            if(InstallParams.UseCAMAccount)
            {
                this.CDomain = InstallParams.CAMDomain;
                this.CPassword = InstallParams.CAMPassword;
                this.CUser = InstallParams.CAMUser;
            }
            #endregion

            #region MAUI startup
            Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;
            Utilities Tools = new Utilities();
            Tools.Exec(this.IFile, "");
            Thread.Sleep(5000);

            // Get the Process ID
            if(this.OSBuild == W2K)
            {
                this.MOMSP1_ProcessID = Tools.GetMOMW2KProcessID();
            }
            else
            {
                this.MOMSP1_ProcessID = Tools.GetProcessID(this.W2K3File);
            }
            
            // Create the MAUI App Object
            App MOMSP1_App = new App(this.MOMSP1_ProcessID, false);
            #endregion

            #region Exec Dialog Methods
            MOMSP1Dialogs Dialogs = new MOMSP1Dialogs(MOMSP1_App);
            Dialogs.WelcomeDialog();
            if(this.Select)
            {
                Dialogs.RegistrationDialog();
            }
            else
            {
                Dialogs.RegistrationDialog(Keys);
            }
            Dialogs.LicenseDialog();
            if(this.Dest != null)
            {
                Dialogs.DestinationDirectory(this.Dest);
            }
            else
            {
                Dialogs.DestinationDirectory();
            }
            Dialogs.InstallType(MOMSP1Dialogs.InstallTypes.Typical);
            if(this.DatabaseInstance != null)
            {
                Dialogs.DBInstance(this.DatabaseInstance);
            }
            else
            {
                Dialogs.DBInstance(this.Computer);
            }
            if(this.UseCAMInfo)
            {
                Dialogs.DasAccount(this.DUser, this.DDomain, this.DPassword, false);
                Dialogs.ConsolidatorAccount(this.CUser, this.CDomain, this.CPassword);
            }
            else
            {
                Dialogs.DasAccount(this.DUser, this.DDomain, this.DPassword, true);
            }
            Dialogs.InstallSystemFiles();
            Dialogs.CreateSeurityGroups();
            Dialogs.DBLogSize(this.DatabaseSize, this.DatabaseLogSize, this.DatabaseFileLocation, this.DatabaseLogFileLocation);
            Dialogs.StartService(this.AutoStartService, this.StartAfterSetup);
            Dialogs.ConfigGroup(this.ConfigGroup);
            if(this.WebConsoleLoc != null)
            {
                Dialogs.WebConsoleLocation(this.WebConsoleLoc);
            }
            else
            {
                Dialogs.WebConsoleLocation();
            }
            if(this.OSBuild == 2195)
            {
                Dialogs.UpdateBrowscap(this.UpdateBrowscapINI);
            }
            if((this.UseExchange) && (!this.UseSMTP))
            {
                Dialogs.EmailAccount(this.ExchangeServer, this.Mailbox);
            }
            else if((this.UseSMTP) && (!this.UseExchange))
            {
                Dialogs.EmailAccount(this.SMTPReturnAddress, this.SMTPServer, this.SMTPPort);
            }
            else
            {
                Dialogs.EmailAccount();
            }
            Dialogs.ManagmentPackModules(this.InstallAllMPs);
            Dialogs.StartCopying();
            Dialogs.StopServices();
            Dialogs.SetupFinished();
            #endregion

        }
        #endregion
    }
    #endregion

}