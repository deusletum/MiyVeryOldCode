namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class DestinationDirectoryDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public DestinationDirectoryDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IDestinationDirectoryDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IDestinationDirectoryDialogControls
    {
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl SetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFilesStaticControl  {get;}
        Button BrowseButton  {get;}
        StaticControl EMicrosoftOperationsManager2000StaticControl  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: DestinationDirectoryDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  TODO: Add dialog functionality description here.
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[deangj] 7/22/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class DestinationDirectoryDialog : Dialog, IDestinationDirectoryDialogControls
    {

#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Destination Directory";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string SetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFiles = "Setup is ready to begin installation of Microsoft Operations Manager files in the" +
                " following location.\r\n\r\nTo install files in the folder shown, click Next.\r\n\r\nTo " +
                "select a different installation folder, click Browse.";
            internal const string Browse = "B&rowse...";
            internal const string EMicrosoftOperationsManager2000 = "E:\\...\\Microsoft Operations Manager 2000";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            internal const int NextButton = 3;
            internal const int BackButton = 4;
            internal const int CancelButton = 5;
            internal const int SetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFilesStaticControl = 7;
            internal const int BrowseButton = 9;
            internal const int EMicrosoftOperationsManager2000StaticControl = 10;
        }
#endregion

#region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedSetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFilesStaticControl;
        protected Button m_cachedBrowseButton;
        protected StaticControl m_cachedEMicrosoftOperationsManager2000StaticControl;
#endregion

#region "Constructor and Init function"
        /// -----------------------------------------------------------------------------        ///  <summary>
        ///  TODO: Add a description for your constructor.
        ///  </summary>
        ///  <param name="app">App object owning the dialog.</param>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal DestinationDirectoryDialog(App app) : 
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
        /// 	[deangj] 7/22/2003 Created
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
                throw new DestinationDirectoryDialogNotFoundException("Destinatin Directory Dialog not found");
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
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual IDestinationDirectoryDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NextButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDestinationDirectoryDialogControls.NextButton
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
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDestinationDirectoryDialogControls.BackButton
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
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDestinationDirectoryDialogControls.CancelButton
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
        ///  Exposes access to the SetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFilesStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDestinationDirectoryDialogControls.SetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFilesStaticControl
        {
            get
            {
                if ((m_cachedSetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFilesStaticControl == null))
                {
                    m_cachedSetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFilesStaticControl = new StaticControl(this, ControlIDs.SetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFilesStaticControl);
                }
                return m_cachedSetupIsReadyToBeginInstallationOfMicrosoftOperationsManagerFilesInTheFollowingLocationToInstallFilesStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the BrowseButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDestinationDirectoryDialogControls.BrowseButton
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
        ///  Exposes access to the EMicrosoftOperationsManager2000StaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDestinationDirectoryDialogControls.EMicrosoftOperationsManager2000StaticControl
        {
            get
            {
                if ((m_cachedEMicrosoftOperationsManager2000StaticControl == null))
                {
                    m_cachedEMicrosoftOperationsManager2000StaticControl = new StaticControl(this, ControlIDs.EMicrosoftOperationsManager2000StaticControl);
                }
                return m_cachedEMicrosoftOperationsManager2000StaticControl;
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
        /// 	[deangj] 7/22/2003 Created
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
        /// 	[deangj] 7/22/2003 Created
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
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickCancel()
        {
            Controls.CancelButton.Click();
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Browse
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickBrowse()
        {
            Controls.BrowseButton.Click();
        }

#endregion

    }
}

