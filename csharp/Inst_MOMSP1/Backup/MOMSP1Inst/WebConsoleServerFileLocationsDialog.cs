namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class WebConsoleServerFileLocationsDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public WebConsoleServerFileLocationsDialogNotFoundException(string message): base(message)
        {
        }
    }

    #region "IWebConsoleServerFileLocationsDialogControls interface definition"


    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IWebConsoleServerFileLocationsDialogControls
    {
        TextBox NameTextBox  {get;}
        Button BrowseButton  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl EnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalledStaticControl  {get;}
        StaticControl LocationStaticControl  {get;}
    }

    #endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: WebConsoleServerFileLocationsDialog
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
    internal class WebConsoleServerFileLocationsDialog : Dialog, IWebConsoleServerFileLocationsDialogControls
    {


        #region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Web Console Server File Locations";
            internal const string Browse = "B&rowse ...";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string EnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalled = "Enter the location where the Web Console server files will be installed.";
            internal const string Location = "&Location:";
        }

        #endregion

        #region "Control IDs"
        public class ControlIDs
        {
            public const int NameTextBox = 0x3;
            public const int BrowseButton = 0x4;
            public const int NextButton = 0x5;
            public const int BackButton = 0x6;
            public const int CancelButton = 0x7;
            public const int EnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalledStaticControl = 0x9;
            public const int LocationStaticControl = 0xA;
        }

        #endregion

        #region "Member Variables"
        protected TextBox m_cachedNameTextBox;
        protected Button m_cachedBrowseButton;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedEnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalledStaticControl;
        protected StaticControl m_cachedLocationStaticControl;

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
        internal WebConsoleServerFileLocationsDialog(App app) : 
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
                throw new WebConsoleServerFileLocationsDialogNotFoundException("WebConsole Server File Location Dialog not found"); 
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
        internal virtual IWebConsoleServerFileLocationsDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control Name
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string NameText
        {
            get
            {
                return Controls.NameTextBox.Text;
            }
            set
            {
                Controls.NameTextBox.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NameTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IWebConsoleServerFileLocationsDialogControls.NameTextBox
        {
            get
            {
                if ((m_cachedNameTextBox == null))
                {
                    m_cachedNameTextBox = new TextBox(this, ControlIDs.NameTextBox);
                }
                return m_cachedNameTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the BrowseButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IWebConsoleServerFileLocationsDialogControls.BrowseButton
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
        ///  Exposes access to the NextButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IWebConsoleServerFileLocationsDialogControls.NextButton
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
        Button IWebConsoleServerFileLocationsDialogControls.BackButton
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
        Button IWebConsoleServerFileLocationsDialogControls.CancelButton
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
        ///  Exposes access to the EnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalledStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IWebConsoleServerFileLocationsDialogControls.EnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalledStaticControl
        {
            get
            {
                if ((m_cachedEnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalledStaticControl == null))
                {
                    m_cachedEnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalledStaticControl = new StaticControl(this, ControlIDs.EnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalledStaticControl);
                }
                return m_cachedEnterTheLocationWhereTheWebConsoleServerFilesWillBeInstalledStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the LocationStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IWebConsoleServerFileLocationsDialogControls.LocationStaticControl
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

        #endregion

        #region "Methods"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Browse
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickBrowse()
        {
            Controls.BrowseButton.Click();
        }

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


