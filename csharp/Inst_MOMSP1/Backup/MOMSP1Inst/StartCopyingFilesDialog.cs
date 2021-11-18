namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class StartCopyingFilesDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public StartCopyingFilesDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IStartCopyingFilesDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IStartCopyingFilesDialogControls
    {
        Button NextButton  {get;}
        Button CancelButton  {get;}
        Button BackButton  {get;}
        StaticControl SetupIsReadyToStartCopyingProgramFilesClickNextToContinueStaticControl  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: StartCopyingFilesDialog
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
    internal class StartCopyingFilesDialog : Dialog, IStartCopyingFilesDialogControls
    {

#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Start Copying Files";
            internal const string Next = "&Next >";
            internal const string Cancel = "Cancel";
            internal const string Back = "< &Back";
            internal const string SetupIsReadyToStartCopyingProgramFilesClickNextToContinue = "Setup is ready to start copying program files.\r\n\r\nClick Next to continue. \r\n\r\n";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            internal const int NextButton = 3;
            internal const int CancelButton = 4;
            internal const int BackButton = 6;
            internal const int SetupIsReadyToStartCopyingProgramFilesClickNextToContinueStaticControl = 7;
        }
#endregion

#region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedCancelButton;
        protected Button m_cachedBackButton;
        protected StaticControl m_cachedSetupIsReadyToStartCopyingProgramFilesClickNextToContinueStaticControl;
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
        internal StartCopyingFilesDialog(App app) : 
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
                throw new StartCopyingFilesDialogNotFoundException("Start Copying Files Dialog not found");
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
        internal virtual IStartCopyingFilesDialogControls Controls
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
        Button IStartCopyingFilesDialogControls.NextButton
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
        ///  Exposes access to the CancelButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IStartCopyingFilesDialogControls.CancelButton
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
        ///  Exposes access to the BackButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IStartCopyingFilesDialogControls.BackButton
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
        ///  Exposes access to the SetupIsReadyToStartCopyingProgramFilesClickNextToContinueStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IStartCopyingFilesDialogControls.SetupIsReadyToStartCopyingProgramFilesClickNextToContinueStaticControl
        {
            get
            {
                if ((m_cachedSetupIsReadyToStartCopyingProgramFilesClickNextToContinueStaticControl == null))
                {
                    m_cachedSetupIsReadyToStartCopyingProgramFilesClickNextToContinueStaticControl = new StaticControl(this, ControlIDs.SetupIsReadyToStartCopyingProgramFilesClickNextToContinueStaticControl);
                }
                return m_cachedSetupIsReadyToStartCopyingProgramFilesClickNextToContinueStaticControl;
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

#endregion

    }
}

