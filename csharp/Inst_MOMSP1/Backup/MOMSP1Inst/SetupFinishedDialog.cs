namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class SetupFinishedDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public SetupFinishedDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "ISetupFinishedDialogControls interface definition"
    
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface ISetupFinishedDialogControls
    {
        Button FinishButton  {get;}
        Button CancelButton  {get;}
        StaticControl YouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetupStaticControl  {get;}
        Button BackButton  {get;}
        CheckBox ViewManagementPackInstallationLogAfterSetupExitsCheckBox  {get;}
        StaticControl CompletingTheMicrosoftOperationsManagerSetupWizardStaticControl  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: SetupFinishedDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  TODO: Add dialog functionality description here.
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[deangj] 9/29/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class SetupFinishedDialog : Dialog, ISetupFinishedDialogControls
    {

#region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup";
            internal const string Finish = "Finish";
            internal const string Cancel = "Cancel";
            internal const string YouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetup = "You have successfully completed Microsoft Operations Manager Setup.\r\n";
            internal const string Back = "< &Back";
            internal const string ViewManagementPackInstallationLogAfterSetupExits = "&View Management Pack installation log after Setup exits.";
            internal const string CompletingTheMicrosoftOperationsManagerSetupWizard = "Completing the Microsoft Operations Manager Setup Wizard\r\n";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int FinishButton = 0x3;
            internal const int CancelButton = 0x4;
            internal const int YouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetupStaticControl = 0x6;
            internal const int BackButton = 0x7;
            internal const int ViewManagementPackInstallationLogAfterSetupExitsCheckBox = 0x8;
            internal const int CompletingTheMicrosoftOperationsManagerSetupWizardStaticControl = 0x9;
        }

#endregion

#region "Member Variables"
        protected Button m_cachedFinishButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedYouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetupStaticControl;
        protected Button m_cachedBackButton;
        protected CheckBox m_cachedViewManagementPackInstallationLogAfterSetupExitsCheckBox;
        protected StaticControl m_cachedCompletingTheMicrosoftOperationsManagerSetupWizardStaticControl;

#endregion

#region "Constructor and Init function"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  TODO: Add a description for your constructor.
        ///  </summary>
        ///  <param name="app">App object owning the dialog.</param>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal SetupFinishedDialog(App app) : 
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
        /// 	[deangj] 9/29/2003 Created
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
                throw new SetupFinishedDialogNotFoundException("Setup Finished Dialog not found");
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
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual ISetupFinishedDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox ViewManagementPackInstallationLogAfterSetupExits
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool ViewManagementPackInstallationLogAfterSetupExits
        {
            get
            {
                return Controls.ViewManagementPackInstallationLogAfterSetupExitsCheckBox.Checked;
            }
            set
            {
                Controls.ViewManagementPackInstallationLogAfterSetupExitsCheckBox.Checked = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the FinishButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button ISetupFinishedDialogControls.FinishButton
        {
            get
            {
                if ((m_cachedFinishButton == null))
                {
                    m_cachedFinishButton = new Button(this, ControlIDs.FinishButton);
                }
                return m_cachedFinishButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the CancelButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button ISetupFinishedDialogControls.CancelButton
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
        ///  Exposes access to the YouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetupStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ISetupFinishedDialogControls.YouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetupStaticControl
        {
            get
            {
                if ((m_cachedYouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetupStaticControl == null))
                {
                    m_cachedYouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetupStaticControl = new StaticControl(this, ControlIDs.YouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetupStaticControl);
                }
                return m_cachedYouHaveSuccessfullyCompletedMicrosoftOperationsManagerSetupStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the BackButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button ISetupFinishedDialogControls.BackButton
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
        ///  Exposes access to the ViewManagementPackInstallationLogAfterSetupExitsCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox ISetupFinishedDialogControls.ViewManagementPackInstallationLogAfterSetupExitsCheckBox
        {
            get
            {
                if ((m_cachedViewManagementPackInstallationLogAfterSetupExitsCheckBox == null))
                {
                    m_cachedViewManagementPackInstallationLogAfterSetupExitsCheckBox = new CheckBox(this, ControlIDs.ViewManagementPackInstallationLogAfterSetupExitsCheckBox);
                }
                return m_cachedViewManagementPackInstallationLogAfterSetupExitsCheckBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the CompletingTheMicrosoftOperationsManagerSetupWizardStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ISetupFinishedDialogControls.CompletingTheMicrosoftOperationsManagerSetupWizardStaticControl
        {
            get
            {
                if ((m_cachedCompletingTheMicrosoftOperationsManagerSetupWizardStaticControl == null))
                {
                    m_cachedCompletingTheMicrosoftOperationsManagerSetupWizardStaticControl = new StaticControl(this, ControlIDs.CompletingTheMicrosoftOperationsManagerSetupWizardStaticControl);
                }
                return m_cachedCompletingTheMicrosoftOperationsManagerSetupWizardStaticControl;
            }
        }

#endregion

#region "Methods"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Finish
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickFinish()
        {
            Controls.FinishButton.Click();
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Cancel
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickCancel()
        {
            Controls.CancelButton.Click();
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Back
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/29/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickBack()
        {
            Controls.BackButton.Click();
        }

#endregion

    }
}


