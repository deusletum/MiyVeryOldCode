namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class DatabaseandLogFileSizeNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public DatabaseandLogFileSizeNotFoundException(string message): base(message)
        {
        }
    }

    #region "IDatabaseandLogFileSizeDialogControls interface definition"


    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IDatabaseandLogFileSizeDialogControls
    {
        StaticControl SizeStaticControl  {get;}
        TextBox EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox  {get;}
        StaticControl LocationStaticControl  {get;}
        TextBox LocationTextBox  {get;}
        StaticControl SizeStaticControl2  {get;}
    
        // TODO: Rename this interface method.  Intuitive name not found by Class Maker Tool.
        TextBox TextBox0  {get;}
        StaticControl LocationStaticControl2  {get;}
        TextBox SizeTextBox  {get;}
        Button BrowseButton  {get;}
        Button BrowseButton2  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinStaticControl  {get;}
        StaticControl MBStaticControl  {get;}
        StaticControl MBStaticControl2  {get;}
    }

    #endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: DatabaseandLogFileSizeDialog
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
    internal class DatabaseandLogFileSizeDialog : Dialog, IDatabaseandLogFileSizeDialogControls
    {


        #region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Database and Log File Size ";
            internal const string Size = "&Size:";
            internal const string Location = "&Location:";
            internal const string Size2 = "Si&ze:";
            internal const string Location2 = "L&ocation:";
            internal const string Browse = "&Browse ...";
            internal const string Browse2 = "B&rowse ...";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatin = "Enter the size and location for the database file and the log file that will be c" +
                "reated. For information about calculating the space required, refer to your prod" +
                "uct documentation.";
            internal const string MB = "MB";
            internal const string MB2 = "MB";
        }

        #endregion

        #region "Control IDs"
        internal class ControlIDs
        {
            internal const int SizeStaticControl = 0x3;
            internal const int EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox = 0x4;
            internal const int LocationStaticControl = 0x5;
            internal const int LocationTextBox = 0x6;
            internal const int SizeStaticControl2 = 0x7;
        
            // TODO: You may wish to rename this ID.  Intuitive name not found by Dialog Class Maker
            internal const int TextBox0 = 0x8;
            internal const int LocationStaticControl2 = 0x9;
            internal const int SizeTextBox = 0xA;
            internal const int BrowseButton = 0xB;
            internal const int BrowseButton2 = 0xC;
            internal const int NextButton = 0xD;
            internal const int BackButton = 0xE;
            internal const int CancelButton = 0xF;
            internal const int EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinStaticControl = 0x11;
            internal const int MBStaticControl = 0x14;
            internal const int MBStaticControl2 = 0x15;
        }

        #endregion

        #region "Member Variables"
        protected StaticControl m_cachedSizeStaticControl;
        protected TextBox m_cachedEnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox;
        protected StaticControl m_cachedLocationStaticControl;
        protected TextBox m_cachedLocationTextBox;
        protected StaticControl m_cachedSizeStaticControl2;
        protected TextBox m_cachedTextBox0;
        protected StaticControl m_cachedLocationStaticControl2;
        protected TextBox m_cachedSizeTextBox;
        protected Button m_cachedBrowseButton;
        protected Button m_cachedBrowseButton2;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedEnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinStaticControl;
        protected StaticControl m_cachedMBStaticControl;
        protected StaticControl m_cachedMBStaticControl2;

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
        internal DatabaseandLogFileSizeDialog(App app) : 
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
                throw new DatabaseandLogFileSizeNotFoundException("Database and Log file Size Dialog not found");
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
        internal virtual IDatabaseandLogFileSizeDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatin
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinText
        {
            get
            {
                return Controls.EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox.Text;
            }
            set
            {
                Controls.EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox.Text = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control Location
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string LocationText
        {
            get
            {
                return Controls.LocationTextBox.Text;
            }
            set
            {
                Controls.LocationTextBox.Text = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control TextBox0
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string TextBox0Text
        {
            get
            {
                return Controls.TextBox0.Text;
            }
            set
            {
                Controls.TextBox0.Text = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control Size
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string SizeText
        {
            get
            {
                return Controls.SizeTextBox.Text;
            }
            set
            {
                Controls.SizeTextBox.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SizeStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseandLogFileSizeDialogControls.SizeStaticControl
        {
            get
            {
                if ((m_cachedSizeStaticControl == null))
                {
                    m_cachedSizeStaticControl = new StaticControl(this, ControlIDs.SizeStaticControl);
                }
                return m_cachedSizeStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IDatabaseandLogFileSizeDialogControls.EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox
        {
            get
            {
                if ((m_cachedEnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox == null))
                {
                    m_cachedEnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox = new TextBox(this, ControlIDs.EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox);
                }
                return m_cachedEnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the LocationStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseandLogFileSizeDialogControls.LocationStaticControl
        {
            get
            {
                if ((m_cachedLocationStaticControl == null))
                {
                    m_cachedLocationStaticControl = new StaticControl(this, ControlIDs.LocationStaticControl);
                }
                return m_cachedLocationStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the LocationTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IDatabaseandLogFileSizeDialogControls.LocationTextBox
        {
            get
            {
                if ((m_cachedLocationTextBox == null))
                {
                    m_cachedLocationTextBox = new TextBox(this, ControlIDs.LocationTextBox);
                }
                return m_cachedLocationTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SizeStaticControl2 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseandLogFileSizeDialogControls.SizeStaticControl2
        {
            get
            {
                if ((m_cachedSizeStaticControl2 == null))
                {
                    m_cachedSizeStaticControl2 = new StaticControl(this, ControlIDs.SizeStaticControl2);
                }
                return m_cachedSizeStaticControl2;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the TextBox0 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IDatabaseandLogFileSizeDialogControls.TextBox0
        {
            get
            {
                if ((m_cachedTextBox0 == null))
                {
                    m_cachedTextBox0 = new TextBox(this, ControlIDs.TextBox0);
                }
                return m_cachedTextBox0;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the LocationStaticControl2 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseandLogFileSizeDialogControls.LocationStaticControl2
        {
            get
            {
                if ((m_cachedLocationStaticControl2 == null))
                {
                    m_cachedLocationStaticControl2 = new StaticControl(this, ControlIDs.LocationStaticControl2);
                }
                return m_cachedLocationStaticControl2;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SizeTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IDatabaseandLogFileSizeDialogControls.SizeTextBox
        {
            get
            {
                if ((m_cachedSizeTextBox == null))
                {
                    m_cachedSizeTextBox = new TextBox(this, ControlIDs.SizeTextBox);
                }
                return m_cachedSizeTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the BrowseButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDatabaseandLogFileSizeDialogControls.BrowseButton
        {
            get
            {
                if ((m_cachedBrowseButton == null))
                {
                    m_cachedBrowseButton = new Button(this, ControlIDs.BrowseButton);
                }
                return m_cachedBrowseButton;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the BrowseButton2 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDatabaseandLogFileSizeDialogControls.BrowseButton2
        {
            get
            {
                if ((m_cachedBrowseButton2 == null))
                {
                    m_cachedBrowseButton2 = new Button(this, ControlIDs.BrowseButton2);
                }
                return m_cachedBrowseButton2;
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
        Button IDatabaseandLogFileSizeDialogControls.NextButton
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
        Button IDatabaseandLogFileSizeDialogControls.BackButton
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
        Button IDatabaseandLogFileSizeDialogControls.CancelButton
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
        ///  Exposes access to the EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseandLogFileSizeDialogControls.EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinStaticControl
        {
            get
            {
                if ((m_cachedEnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinStaticControl == null))
                {
                    m_cachedEnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinStaticControl = new StaticControl(this, ControlIDs.EnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinStaticControl);
                }
                return m_cachedEnterTheSizeAndLocationForTheDatabaseFileAndTheLogFileThatWillBeCreatedForInformationAboutCalculatinStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the MBStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseandLogFileSizeDialogControls.MBStaticControl
        {
            get
            {
                if ((m_cachedMBStaticControl == null))
                {
                    m_cachedMBStaticControl = new StaticControl(this, ControlIDs.MBStaticControl);
                }
                return m_cachedMBStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the MBStaticControl2 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseandLogFileSizeDialogControls.MBStaticControl2
        {
            get
            {
                if ((m_cachedMBStaticControl2 == null))
                {
                    m_cachedMBStaticControl2 = new StaticControl(this, ControlIDs.MBStaticControl2);
                }
                return m_cachedMBStaticControl2;
            }
        }

        #endregion

        #region "Methods"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Browse
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickBrowse()
        {
            Controls.BrowseButton.Click();
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Browse2
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/16/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickBrowse2()
        {
            Controls.BrowseButton2.Click();
        }

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

        #endregion

    }
}


