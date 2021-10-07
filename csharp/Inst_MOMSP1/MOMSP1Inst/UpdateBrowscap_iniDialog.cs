namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class UpdateBrowscap_iniDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public UpdateBrowscap_iniDialogNotFoundException(string message): base(message)
        {
        }
    }

    #region "IUpdateBrowscap_iniDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IUpdateBrowscap_iniDialogControls
    {
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl TheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgramStaticControl  {get;}
        RadioButton YesRadioButton  {get;}
        RadioButton NoRadioButton  {get;}
        StaticControl BackupTheExistingBrowscapiniFileAndUpdateItStaticControl  {get;}
        StaticControl DoNotUpdateTheExistingBrowscapiniFileStaticControl  {get;}
    }

    #endregion


    #region "Enums for RadioButton groups"


    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  Enum for radio group RadioGroup0
    ///  </summary>
    ///  <history>
    /// 	[deangj] 9/20/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal enum RadioGroup0
    {
        Yes = 0,
        No = 1,
    }

    #endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: UpdateBrowscap_iniDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  TODO: Add dialog functionality description here.
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[deangj] 9/20/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class UpdateBrowscap_iniDialog : Dialog, IUpdateBrowscap_iniDialogControls
    {


        #region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Update Browscap.ini";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string TheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgram = "The local Browscap.ini file does not contain information to recognize Internet Ex" +
                "plorer 6.0 browsers.\r\n\r\nThe setup program can backup the existing Browscap.ini f" +
                "ile to E:\\WINNT\\system32\\inetsrv\\Browscap.old, and update it with a version cont" +
                "aining Interne";
            internal const string Yes = "&Yes";
            internal const string No = "N&o";
            internal const string BackupTheExistingBrowscapiniFileAndUpdateIt = "Backup the existing Browscap.ini file, and update it.";
            internal const string DoNotUpdateTheExistingBrowscapiniFile = "Do not update the existing Browscap.ini file.";
        }

        #endregion

        #region "Control IDs"
        internal class ControlIDs
        {
            internal const int NextButton = 0x3;
            internal const int BackButton = 0x4;
            internal const int CancelButton = 0x5;
            internal const int TheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgramStaticControl = 0x7;
            internal const int YesRadioButton = 0x8;
            internal const int NoRadioButton = 0x9;
            internal const int BackupTheExistingBrowscapiniFileAndUpdateItStaticControl = 0xA;
            internal const int DoNotUpdateTheExistingBrowscapiniFileStaticControl = 0xB;
        }

        #endregion

        #region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedTheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgramStaticControl;
        protected RadioButton m_cachedYesRadioButton;
        protected RadioButton m_cachedNoRadioButton;
        protected StaticControl m_cachedBackupTheExistingBrowscapiniFileAndUpdateItStaticControl;
        protected StaticControl m_cachedDoNotUpdateTheExistingBrowscapiniFileStaticControl;

        #endregion

        #region "Constructor and Init function"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  TODO: Add a description for your constructor.
        ///  </summary>
        ///  <param name="app">App object owning the dialog.</param>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal UpdateBrowscap_iniDialog(App app) : 
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
        /// 	[deangj] 9/20/2003 Created
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
                throw new UpdateBrowscap_iniDialogNotFoundException(" Update Browscap.ini Dialog not found");
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual IUpdateBrowscap_iniDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Functionality for radio group RadioGroup0
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual RadioGroup0 RadioGroup0
        {
            get
            {
                if ((Controls.YesRadioButton.ButtonState == ButtonState.Checked))
                    return RadioGroup0.Yes;

                if ((Controls.NoRadioButton.ButtonState == ButtonState.Checked))
                    return RadioGroup0.No;

                throw new RadioButton.Exceptions.CheckFailedException("No radio button selected.");
            }
            set
            {
                if ((value == RadioGroup0.Yes))
                    Controls.YesRadioButton.ButtonState = ButtonState.Checked;
                else if ((value == RadioGroup0.No))
                    Controls.NoRadioButton.ButtonState = ButtonState.Checked;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NextButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IUpdateBrowscap_iniDialogControls.NextButton
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IUpdateBrowscap_iniDialogControls.BackButton
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IUpdateBrowscap_iniDialogControls.CancelButton
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
        ///  Exposes access to the TheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgramStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IUpdateBrowscap_iniDialogControls.TheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgramStaticControl
        {
            get
            {
                if ((m_cachedTheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgramStaticControl == null))
                {
                    m_cachedTheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgramStaticControl = new StaticControl(this, ControlIDs.TheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgramStaticControl);
                }
                return m_cachedTheLocalBrowscapiniFileDoesNotContainInformationToRecognizeInternetExplorer60BrowsersTheSetupProgramStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the YesRadioButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        RadioButton IUpdateBrowscap_iniDialogControls.YesRadioButton
        {
            get
            {
                if ((m_cachedYesRadioButton == null))
                {
                    m_cachedYesRadioButton = new RadioButton(this, ControlIDs.YesRadioButton);
                }
                return m_cachedYesRadioButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NoRadioButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        RadioButton IUpdateBrowscap_iniDialogControls.NoRadioButton
        {
            get
            {
                if ((m_cachedNoRadioButton == null))
                {
                    m_cachedNoRadioButton = new RadioButton(this, ControlIDs.NoRadioButton);
                }
                return m_cachedNoRadioButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the BackupTheExistingBrowscapiniFileAndUpdateItStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IUpdateBrowscap_iniDialogControls.BackupTheExistingBrowscapiniFileAndUpdateItStaticControl
        {
            get
            {
                if ((m_cachedBackupTheExistingBrowscapiniFileAndUpdateItStaticControl == null))
                {
                    m_cachedBackupTheExistingBrowscapiniFileAndUpdateItStaticControl = new StaticControl(this, ControlIDs.BackupTheExistingBrowscapiniFileAndUpdateItStaticControl);
                }
                return m_cachedBackupTheExistingBrowscapiniFileAndUpdateItStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the DoNotUpdateTheExistingBrowscapiniFileStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IUpdateBrowscap_iniDialogControls.DoNotUpdateTheExistingBrowscapiniFileStaticControl
        {
            get
            {
                if ((m_cachedDoNotUpdateTheExistingBrowscapiniFileStaticControl == null))
                {
                    m_cachedDoNotUpdateTheExistingBrowscapiniFileStaticControl = new StaticControl(this, ControlIDs.DoNotUpdateTheExistingBrowscapiniFileStaticControl);
                }
                return m_cachedDoNotUpdateTheExistingBrowscapiniFileStaticControl;
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
        /// 	[deangj] 9/20/2003 Created
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
        /// 	[deangj] 9/20/2003 Created
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickCancel()
        {
            Controls.CancelButton.Click();
        }

        #endregion

    }
}


