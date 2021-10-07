namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{


    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class IncompleteInstallationDetectedDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public IncompleteInstallationDetectedDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IIncompleteInstallationDetectedDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IIncompleteInstallationDetectedDialogControls
    {
        Button ExitButton  {get;}
        Button ContinueButton  {get;}
        Button RebootButton  {get;}
        TextBox PendingFileRenameInformationTextBox  {get;}
        StaticControl PendingFileRenameInformationStaticControl  {get;}
        StaticControl SetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeReplaStaticControl  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: IncompleteInstallationDetectedDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  TODO: Add dialog functionality description here.
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[asttest] 8/13/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class IncompleteInstallationDetectedDialog : Dialog, IIncompleteInstallationDetectedDialogControls
    {


#region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Incomplete Installation Detected";
            internal const string Exit = "&Exit";
            internal const string Continue = "&Continue";
            internal const string Reboot = "&Reboot";
            internal const string PendingFileRenameInformation = "Pending File Rename Information";
            internal const string SetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeRepla = "Setup detected that there are Pending File Rename Operations for MOMSDK01E. Locke" +
                "d system files are waiting to be replaced on reboot, probably because a previous" +
                " installation program needed to replace a file that was locked.  \r\n\r\nWe strongly" +
                " recommend tha";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int ExitButton = 0x3;
            internal const int ContinueButton = 0x4;
            internal const int RebootButton = 0x5;
            internal const int PendingFileRenameInformationTextBox = 0x6;
            internal const int PendingFileRenameInformationStaticControl = 0x7;
            internal const int SetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeReplaStaticControl = 0x8;
        }

#endregion

#region "Member Variables"
        protected Button m_cachedExitButton;
        protected Button m_cachedContinueButton;
        protected Button m_cachedRebootButton;
        protected TextBox m_cachedPendingFileRenameInformationTextBox;
        protected StaticControl m_cachedPendingFileRenameInformationStaticControl;
        protected StaticControl m_cachedSetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeReplaStaticControl;

#endregion

#region "Constructor and Init function"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  TODO: Add a description for your constructor.
        ///  </summary>
        ///  <param name="app">App object owning the dialog.</param>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal IncompleteInstallationDetectedDialog(App app) : 
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
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        private static Window Init(App app)
        {
            // First check if the dialog is already up.
            Window tempWindow = null;
            try
            {
                tempWindow = new Window(Strings.DialogTitle, StringMatchSyntax.ExactMatch, WindowClassNames.Alert, StringMatchSyntax.ExactMatch, app, 3000);
            }
            catch (Exceptions.WindowNotFoundException)
            {
                throw new IncompleteInstallationDetectedDialogNotFoundException("Incomplete Installation Detected Dialog not found");
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
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual IIncompleteInstallationDetectedDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control PendingFileRenameInformation
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string PendingFileRenameInformationText
        {
            get
            {
                return Controls.PendingFileRenameInformationTextBox.Text;
            }
            set
            {
                Controls.PendingFileRenameInformationTextBox.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ExitButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IIncompleteInstallationDetectedDialogControls.ExitButton
        {
            get
            {
                if ((m_cachedExitButton == null))
                {
                    m_cachedExitButton = new Button(this, ControlIDs.ExitButton);
                }
                return m_cachedExitButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ContinueButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IIncompleteInstallationDetectedDialogControls.ContinueButton
        {
            get
            {
                if ((m_cachedContinueButton == null))
                {
                    m_cachedContinueButton = new Button(this, ControlIDs.ContinueButton);
                }
                return m_cachedContinueButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the RebootButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IIncompleteInstallationDetectedDialogControls.RebootButton
        {
            get
            {
                if ((m_cachedRebootButton == null))
                {
                    m_cachedRebootButton = new Button(this, ControlIDs.RebootButton);
                }
                return m_cachedRebootButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the PendingFileRenameInformationTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IIncompleteInstallationDetectedDialogControls.PendingFileRenameInformationTextBox
        {
            get
            {
                if ((m_cachedPendingFileRenameInformationTextBox == null))
                {
                    m_cachedPendingFileRenameInformationTextBox = new TextBox(this, ControlIDs.PendingFileRenameInformationTextBox);
                }
                return m_cachedPendingFileRenameInformationTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the PendingFileRenameInformationStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IIncompleteInstallationDetectedDialogControls.PendingFileRenameInformationStaticControl
        {
            get
            {
                if ((m_cachedPendingFileRenameInformationStaticControl == null))
                {
                    m_cachedPendingFileRenameInformationStaticControl = new StaticControl(this, ControlIDs.PendingFileRenameInformationStaticControl);
                }
                return m_cachedPendingFileRenameInformationStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeReplaStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IIncompleteInstallationDetectedDialogControls.SetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeReplaStaticControl
        {
            get
            {
                if ((m_cachedSetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeReplaStaticControl == null))
                {
                    m_cachedSetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeReplaStaticControl = new StaticControl(this, ControlIDs.SetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeReplaStaticControl);
                }
                return m_cachedSetupDetectedThatThereArePendingFileRenameOperationsForMOMSDK01ELockedSystemFilesAreWaitingToBeReplaStaticControl;
            }
        }

#endregion

#region "Methods"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Exit
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickExit()
        {
            Controls.ExitButton.Click();
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Continue
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickContinue()
        {
            Controls.ContinueButton.Click();
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Reboot
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[asttest] 8/13/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickReboot()
        {
            Controls.RebootButton.Click();
        }

#endregion

    }
}


