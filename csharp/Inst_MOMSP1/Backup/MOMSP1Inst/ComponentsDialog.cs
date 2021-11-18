namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{


    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class ComponentsDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public ComponentsDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IComponentsDialogControls interface definition"
    
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IComponentsDialogControls
    {
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl SelectTheComponentsYouWantToInstallStaticControl  {get;}
        TextBox SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox  {get;}
        StaticControl DiskSpaceRequiredStaticControl  {get;}
        StaticControl DiskSpaceRemainingStaticControl  {get;}
        StaticControl _29013582KStaticControl  {get;}
        StaticControl _69149KStaticControl  {get;}
        CheckBox DatabaseCheckBox  {get;}
        StaticControl _2920KStaticControl  {get;}
        CheckBox DataAccessServerDASCheckBox  {get;}
        StaticControl _11436KStaticControl  {get;}
        CheckBox ConsolidatorAndAgentManagerCAMCheckBox  {get;}
        StaticControl _74097KStaticControl  {get;}
        CheckBox MOMAdministratorConsoleCheckBox  {get;}
        StaticControl _30713KStaticControl  {get;}
        CheckBox MOMReportingCheckBox  {get;}
        StaticControl _45065KStaticControl  {get;}
        CheckBox WebConsoleServerCheckBox  {get;}
        StaticControl _10735KStaticControl  {get;}
        CheckBox ManagementPackModulesCheckBox  {get;}
        StaticControl _28KStaticControl  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: ComponentsDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  TODO: Add dialog functionality description here.
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[deangj] 9/11/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class ComponentsDialog : Dialog, IComponentsDialogControls
    {


#region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Components";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string SelectTheComponentsYouWantToInstall = "Select the components you want to install:";
            internal const string DiskSpaceRequired = "Disk space required:";
            internal const string DiskSpaceRemaining = "Disk space remaining:";
            internal const string _29013582K = "29013582 k";
            internal const string _69149K = "69149 k";
            internal const string Database = "Database";
            internal const string _2920K = "2920 k";
            internal const string DataAccessServerDAS = "Data Access Server (DAS)";
            internal const string _11436K = "11436 k";
            internal const string ConsolidatorAndAgentManagerCAM = "Consolidator and Agent Manager (CAM)";
            internal const string _74097K = "74097 k";
            internal const string MOMAdministratorConsole = "MOM Administrator Console";
            internal const string _30713K = "30713 k";
            internal const string MOMReporting = "MOM Reporting";
            internal const string _45065K = "45065 k";
            internal const string WebConsoleServer = "Web Console Server";
            internal const string _10735K = "10735 k";
            internal const string ManagementPackModules = "Management Pack Modules";
            internal const string _28K = "28 k";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int NextButton = 0x3;
            internal const int BackButton = 0x4;
            internal const int CancelButton = 0x5;
            internal const int SelectTheComponentsYouWantToInstallStaticControl = 0x6;
            internal const int SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox = 0x7;
            internal const int DiskSpaceRequiredStaticControl = 0x8;
            internal const int DiskSpaceRemainingStaticControl = 0xA;
            internal const int _29013582KStaticControl = 0xB;
            internal const int _69149KStaticControl = 0xC;
            internal const int DatabaseCheckBox = 0xE;
            internal const int _2920KStaticControl = 0xF;
            internal const int DataAccessServerDASCheckBox = 0x10;
            internal const int _11436KStaticControl = 0x11;
            internal const int ConsolidatorAndAgentManagerCAMCheckBox = 0x12;
            internal const int _74097KStaticControl = 0x13;
            internal const int MOMAdministratorConsoleCheckBox = 0x14;
            internal const int _30713KStaticControl = 0x15;
            internal const int MOMReportingCheckBox = 0x16;
            internal const int _45065KStaticControl = 0x17;
            internal const int WebConsoleServerCheckBox = 0x18;
            internal const int _10735KStaticControl = 0x19;
            internal const int ManagementPackModulesCheckBox = 0x1A;
            internal const int _28KStaticControl = 0x1B;
        }

#endregion

#region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedSelectTheComponentsYouWantToInstallStaticControl;
        protected TextBox m_cachedSetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox;
        protected StaticControl m_cachedDiskSpaceRequiredStaticControl;
        protected StaticControl m_cachedDiskSpaceRemainingStaticControl;
        protected StaticControl m_cached_29013582KStaticControl;
        protected StaticControl m_cached_69149KStaticControl;
        protected CheckBox m_cachedDatabaseCheckBox;
        protected StaticControl m_cached_2920KStaticControl;
        protected CheckBox m_cachedDataAccessServerDASCheckBox;
        protected StaticControl m_cached_11436KStaticControl;
        protected CheckBox m_cachedConsolidatorAndAgentManagerCAMCheckBox;
        protected StaticControl m_cached_74097KStaticControl;
        protected CheckBox m_cachedMOMAdministratorConsoleCheckBox;
        protected StaticControl m_cached_30713KStaticControl;
        protected CheckBox m_cachedMOMReportingCheckBox;
        protected StaticControl m_cached_45065KStaticControl;
        protected CheckBox m_cachedWebConsoleServerCheckBox;
        protected StaticControl m_cached_10735KStaticControl;
        protected CheckBox m_cachedManagementPackModulesCheckBox;
        protected StaticControl m_cached_28KStaticControl;

#endregion

#region "Constructor and Init function"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  TODO: Add a description for your constructor.
        ///  </summary>
        ///  <param name="app">App object owning the dialog.</param>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal ComponentsDialog(App app) : 
                base(app, Init(app))
        {
            // TODO: Add Constructor logic here. 
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  This function will attempt to find a showing instance of the dialog.
        ///  </summary>
        ///  <returns>The dialog's Window</returns>
        ///  <param name="app">App owning the dialog.</param>)
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        private static Window Init(App app)
        {
            // First check if the dialog is already up.
            Window tempWindow = null;
            try
            {
                tempWindow = new Window(Strings.DialogTitle, StringMatchSyntax.ExactMatch, WindowClassNames.WiseInstallerDialog, StringMatchSyntax.ExactMatch, app, 3000);
            }
            catch (Exceptions.WindowNotFoundException)
            {
                throw new ComponentsDialogNotFoundException("Components Dialog not found");
            }
            return tempWindow;
        }

#endregion

#region "Properties"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the raw controls for this dialog
        ///  </summary>
        ///  <value>An interface that groups all of the dialog's control properties together</value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual IComponentsDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox Database
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool Database
        {
            get
            {
                return Controls.DatabaseCheckBox.Checked;
            }
            set
            {
                Controls.DatabaseCheckBox.Checked = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox DataAccessServerDAS
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool DataAccessServerDAS
        {
            get
            {
                return Controls.DataAccessServerDASCheckBox.Checked;
            }
            set
            {
                Controls.DataAccessServerDASCheckBox.Checked = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox ConsolidatorAndAgentManagerCAM
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool ConsolidatorAndAgentManagerCAM
        {
            get
            {
                return Controls.ConsolidatorAndAgentManagerCAMCheckBox.Checked;
            }
            set
            {
                Controls.ConsolidatorAndAgentManagerCAMCheckBox.Checked = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox MOMAdministratorConsole
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool MOMAdministratorConsole
        {
            get
            {
                return Controls.MOMAdministratorConsoleCheckBox.Checked;
            }
            set
            {
                Controls.MOMAdministratorConsoleCheckBox.Checked = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox MOMReporting
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool MOMReporting
        {
            get
            {
                return Controls.MOMReportingCheckBox.Checked;
            }
            set
            {
                Controls.MOMReportingCheckBox.Checked = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox WebConsoleServer
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool WebConsoleServer
        {
            get
            {
                return Controls.WebConsoleServerCheckBox.Checked;
            }
            set
            {
                Controls.WebConsoleServerCheckBox.Checked = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox ManagementPackModules
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool ManagementPackModules
        {
            get
            {
                return Controls.ManagementPackModulesCheckBox.Checked;
            }
            set
            {
                Controls.ManagementPackModulesCheckBox.Checked = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecif
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifText
        {
            get
            {
                return Controls.SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox.Text;
            }
            set
            {
                Controls.SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NextButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IComponentsDialogControls.NextButton
        {
            get
            {
                if ((m_cachedNextButton == null))
                {
                    m_cachedNextButton = new Button(this, ControlIDs.NextButton);
                }
                return m_cachedNextButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the BackButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IComponentsDialogControls.BackButton
        {
            get
            {
                if ((m_cachedBackButton == null))
                {
                    m_cachedBackButton = new Button(this, ControlIDs.BackButton);
                }
                return m_cachedBackButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the CancelButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IComponentsDialogControls.CancelButton
        {
            get
            {
                if ((m_cachedCancelButton == null))
                {
                    m_cachedCancelButton = new Button(this, ControlIDs.CancelButton);
                }
                return m_cachedCancelButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SelectTheComponentsYouWantToInstallStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls.SelectTheComponentsYouWantToInstallStaticControl
        {
            get
            {
                if ((m_cachedSelectTheComponentsYouWantToInstallStaticControl == null))
                {
                    m_cachedSelectTheComponentsYouWantToInstallStaticControl = new StaticControl(this, ControlIDs.SelectTheComponentsYouWantToInstallStaticControl);
                }
                return m_cachedSelectTheComponentsYouWantToInstallStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IComponentsDialogControls.SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox
        {
            get
            {
                if ((m_cachedSetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox == null))
                {
                    m_cachedSetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox = new TextBox(this, ControlIDs.SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox);
                }
                return m_cachedSetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the DiskSpaceRequiredStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls.DiskSpaceRequiredStaticControl
        {
            get
            {
                if ((m_cachedDiskSpaceRequiredStaticControl == null))
                {
                    m_cachedDiskSpaceRequiredStaticControl = new StaticControl(this, ControlIDs.DiskSpaceRequiredStaticControl);
                }
                return m_cachedDiskSpaceRequiredStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the DiskSpaceRemainingStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls.DiskSpaceRemainingStaticControl
        {
            get
            {
                if ((m_cachedDiskSpaceRemainingStaticControl == null))
                {
                    m_cachedDiskSpaceRemainingStaticControl = new StaticControl(this, ControlIDs.DiskSpaceRemainingStaticControl);
                }
                return m_cachedDiskSpaceRemainingStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _29013582KStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls._29013582KStaticControl
        {
            get
            {
                if ((m_cached_29013582KStaticControl == null))
                {
                    m_cached_29013582KStaticControl = new StaticControl(this, ControlIDs._29013582KStaticControl);
                }
                return m_cached_29013582KStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _69149KStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls._69149KStaticControl
        {
            get
            {
                if ((m_cached_69149KStaticControl == null))
                {
                    m_cached_69149KStaticControl = new StaticControl(this, ControlIDs._69149KStaticControl);
                }
                return m_cached_69149KStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the DatabaseCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IComponentsDialogControls.DatabaseCheckBox
        {
            get
            {
                if ((m_cachedDatabaseCheckBox == null))
                {
                    m_cachedDatabaseCheckBox = new CheckBox(this, ControlIDs.DatabaseCheckBox);
                }
                return m_cachedDatabaseCheckBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _2920KStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls._2920KStaticControl
        {
            get
            {
                if ((m_cached_2920KStaticControl == null))
                {
                    m_cached_2920KStaticControl = new StaticControl(this, ControlIDs._2920KStaticControl);
                }
                return m_cached_2920KStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the DataAccessServerDASCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IComponentsDialogControls.DataAccessServerDASCheckBox
        {
            get
            {
                if ((m_cachedDataAccessServerDASCheckBox == null))
                {
                    m_cachedDataAccessServerDASCheckBox = new CheckBox(this, ControlIDs.DataAccessServerDASCheckBox);
                }
                return m_cachedDataAccessServerDASCheckBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _11436KStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls._11436KStaticControl
        {
            get
            {
                if ((m_cached_11436KStaticControl == null))
                {
                    m_cached_11436KStaticControl = new StaticControl(this, ControlIDs._11436KStaticControl);
                }
                return m_cached_11436KStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ConsolidatorAndAgentManagerCAMCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IComponentsDialogControls.ConsolidatorAndAgentManagerCAMCheckBox
        {
            get
            {
                if ((m_cachedConsolidatorAndAgentManagerCAMCheckBox == null))
                {
                    m_cachedConsolidatorAndAgentManagerCAMCheckBox = new CheckBox(this, ControlIDs.ConsolidatorAndAgentManagerCAMCheckBox);
                }
                return m_cachedConsolidatorAndAgentManagerCAMCheckBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _74097KStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls._74097KStaticControl
        {
            get
            {
                if ((m_cached_74097KStaticControl == null))
                {
                    m_cached_74097KStaticControl = new StaticControl(this, ControlIDs._74097KStaticControl);
                }
                return m_cached_74097KStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the MOMAdministratorConsoleCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IComponentsDialogControls.MOMAdministratorConsoleCheckBox
        {
            get
            {
                if ((m_cachedMOMAdministratorConsoleCheckBox == null))
                {
                    m_cachedMOMAdministratorConsoleCheckBox = new CheckBox(this, ControlIDs.MOMAdministratorConsoleCheckBox);
                }
                return m_cachedMOMAdministratorConsoleCheckBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _30713KStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls._30713KStaticControl
        {
            get
            {
                if ((m_cached_30713KStaticControl == null))
                {
                    m_cached_30713KStaticControl = new StaticControl(this, ControlIDs._30713KStaticControl);
                }
                return m_cached_30713KStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the MOMReportingCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IComponentsDialogControls.MOMReportingCheckBox
        {
            get
            {
                if ((m_cachedMOMReportingCheckBox == null))
                {
                    m_cachedMOMReportingCheckBox = new CheckBox(this, ControlIDs.MOMReportingCheckBox);
                }
                return m_cachedMOMReportingCheckBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _45065KStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls._45065KStaticControl
        {
            get
            {
                if ((m_cached_45065KStaticControl == null))
                {
                    m_cached_45065KStaticControl = new StaticControl(this, ControlIDs._45065KStaticControl);
                }
                return m_cached_45065KStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the WebConsoleServerCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IComponentsDialogControls.WebConsoleServerCheckBox
        {
            get
            {
                if ((m_cachedWebConsoleServerCheckBox == null))
                {
                    m_cachedWebConsoleServerCheckBox = new CheckBox(this, ControlIDs.WebConsoleServerCheckBox);
                }
                return m_cachedWebConsoleServerCheckBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _10735KStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls._10735KStaticControl
        {
            get
            {
                if ((m_cached_10735KStaticControl == null))
                {
                    m_cached_10735KStaticControl = new StaticControl(this, ControlIDs._10735KStaticControl);
                }
                return m_cached_10735KStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ManagementPackModulesCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IComponentsDialogControls.ManagementPackModulesCheckBox
        {
            get
            {
                if ((m_cachedManagementPackModulesCheckBox == null))
                {
                    m_cachedManagementPackModulesCheckBox = new CheckBox(this, ControlIDs.ManagementPackModulesCheckBox);
                }
                return m_cachedManagementPackModulesCheckBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _28KStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IComponentsDialogControls._28KStaticControl
        {
            get
            {
                if ((m_cached_28KStaticControl == null))
                {
                    m_cached_28KStaticControl = new StaticControl(this, ControlIDs._28KStaticControl);
                }
                return m_cached_28KStaticControl;
            }
        }

#endregion

#region "Methods"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Next
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickNext()
        {
            Controls.NextButton.Click();
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Back
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickBack()
        {
            Controls.BackButton.Click();
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Cancel
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickCancel()
        {
            Controls.CancelButton.Click();
        }

#endregion

    }
}


