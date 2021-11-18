namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class ManagementPackModulesDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public ManagementPackModulesDialogNotFoundException(string message): base(message)
        {
        }
    }


#region "IManagementPackModulesDialogControls interface definition"
    
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IManagementPackModulesDialogControls
    {
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeStaticControl  {get;}
        StaticControl ManagementPackModulesStaticControl  {get;}
        ListBox ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeListBox  {get;}
        StaticControl DescriptionStaticControl  {get;}
        TextBox DescriptionTextBox  {get;}
        Button ClearAllButton  {get;}
        CheckBox BackupAllSelectedManagementPacksRecommendedCheckBox  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: ManagementPackModulesDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  TODO: Add dialog functionality description here.
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[deangj] 9/16/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class ManagementPackModulesDialog : Dialog, IManagementPackModulesDialogControls
    {


#region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Management Pack Modules";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManageme = "Management Pack Modules contain rules and scripts for supporting applications and" +
                " environments. Select the Management Pack Modules you want to install.";
            internal const string ManagementPackModules = "&Management Pack Modules:";
            internal const string Description = "Description:";
            internal const string ClearAll = "Clear All";
            internal const string BackupAllSelectedManagementPacksRecommended = "Backup All Selected Management Packs (Recommended)";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int NextButton = 0x3;
            internal const int BackButton = 0x4;
            internal const int CancelButton = 0x5;
            internal const int ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeStaticControl = 0x7;
            internal const int ManagementPackModulesStaticControl = 0x8;
            internal const int ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeListBox = 0x9;
            internal const int DescriptionStaticControl = 0xA;
            internal const int DescriptionTextBox = 0xB;
            internal const int ClearAllButton = 0xC;
            internal const int BackupAllSelectedManagementPacksRecommendedCheckBox = 0xE;
        }

#endregion

#region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeStaticControl;
        protected StaticControl m_cachedManagementPackModulesStaticControl;
        protected ListBox m_cachedManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeListBox;
        protected StaticControl m_cachedDescriptionStaticControl;
        protected TextBox m_cachedDescriptionTextBox;
        protected Button m_cachedClearAllButton;
        protected CheckBox m_cachedBackupAllSelectedManagementPacksRecommendedCheckBox;

#endregion

#region "Constructor and Init function"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  TODO: Add a description for your constructor.
        ///  </summary>
        ///  <param name="app">App object owning the dialog.</param>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal ManagementPackModulesDialog(App app) : 
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
        /// 	[deangj] 9/16/2003 Created
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
                throw new ManagementPackModulesDialogNotFoundException("Managment Pack Modules Dialog not found"); 
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
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual IManagementPackModulesDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox BackupAllSelectedManagementPacksRecommended
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool BackupAllSelectedManagementPacksRecommended
        {
            get
            {
                return Controls.BackupAllSelectedManagementPacksRecommendedCheckBox.Checked;
            }
            set
            {
                Controls.BackupAllSelectedManagementPacksRecommendedCheckBox.Checked = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control Description
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string DescriptionText
        {
            get
            {
                return Controls.DescriptionTextBox.Text;
            }
            set
            {
                Controls.DescriptionTextBox.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NextButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IManagementPackModulesDialogControls.NextButton
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
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IManagementPackModulesDialogControls.BackButton
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
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IManagementPackModulesDialogControls.CancelButton
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
        ///  Exposes access to the ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IManagementPackModulesDialogControls.ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeStaticControl
        {
            get
            {
                if ((m_cachedManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeStaticControl == null))
                {
                    m_cachedManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeStaticControl = new StaticControl(this, ControlIDs.ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeStaticControl);
                }
                return m_cachedManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ManagementPackModulesStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IManagementPackModulesDialogControls.ManagementPackModulesStaticControl
        {
            get
            {
                if ((m_cachedManagementPackModulesStaticControl == null))
                {
                    m_cachedManagementPackModulesStaticControl = new StaticControl(this, ControlIDs.ManagementPackModulesStaticControl);
                }
                return m_cachedManagementPackModulesStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeListBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        ListBox IManagementPackModulesDialogControls.ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeListBox
        {
            get
            {
                if ((m_cachedManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeListBox == null))
                {
                    m_cachedManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeListBox = new ListBox(this, ControlIDs.ManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeListBox);
                }
                return m_cachedManagementPackModulesContainRulesAndScriptsForSupportingApplicationsAndEnvironmentsSelectTheManagemeListBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the DescriptionStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IManagementPackModulesDialogControls.DescriptionStaticControl
        {
            get
            {
                if ((m_cachedDescriptionStaticControl == null))
                {
                    m_cachedDescriptionStaticControl = new StaticControl(this, ControlIDs.DescriptionStaticControl);
                }
                return m_cachedDescriptionStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the DescriptionTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IManagementPackModulesDialogControls.DescriptionTextBox
        {
            get
            {
                if ((m_cachedDescriptionTextBox == null))
                {
                    m_cachedDescriptionTextBox = new TextBox(this, ControlIDs.DescriptionTextBox);
                }
                return m_cachedDescriptionTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ClearAllButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IManagementPackModulesDialogControls.ClearAllButton
        {
            get
            {
                if ((m_cachedClearAllButton == null))
                {
                    m_cachedClearAllButton = new Button(this, ControlIDs.ClearAllButton);
                }
                return m_cachedClearAllButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the BackupAllSelectedManagementPacksRecommendedCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IManagementPackModulesDialogControls.BackupAllSelectedManagementPacksRecommendedCheckBox
        {
            get
            {
                if ((m_cachedBackupAllSelectedManagementPacksRecommendedCheckBox == null))
                {
                    m_cachedBackupAllSelectedManagementPacksRecommendedCheckBox = new CheckBox(this, ControlIDs.BackupAllSelectedManagementPacksRecommendedCheckBox);
                }
                return m_cachedBackupAllSelectedManagementPacksRecommendedCheckBox;
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
        /// 	[deangj] 9/16/2003 Created
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
        /// 	[deangj] 9/16/2003 Created
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
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickCancel()
        {
            Controls.CancelButton.Click();
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button ClearAll
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickClearAll()
        {
            Controls.ClearAllButton.Click();
        }

#endregion

    }
}


