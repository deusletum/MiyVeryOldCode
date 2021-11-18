namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class CouldNotLocateDatabaseDialogotNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public CouldNotLocateDatabaseDialogotNotFoundException(string message): base(message)
        {
        }
    }

#region "ICouldNotLocateDatabaseDialogControls interface definition"
    
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface ICouldNotLocateDatabaseDialogControls
    {
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifStaticControl  {get;}
        Button NextButton  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: CouldNotLocateDatabaseDialog
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
    internal class CouldNotLocateDatabaseDialog : Dialog, ICouldNotLocateDatabaseDialogControls
    {


#region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Could Not Locate Database";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecif = "Setup reported the following error when attempting to locate the Microsoft Operat" +
                "ions Manager database on the specified computer: DEV-DEANGJ00.  \r\n\r\nThe error re" +
                "ported was: \"Unable to make connection to database on server name given\"\r\n\r\nVeri" +
                "fy that Micros";
            internal const string Next = "&Next >";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int BackButton = 0x3;
            internal const int CancelButton = 0x4;
            internal const int SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifStaticControl = 0x6;
            internal const int NextButton = 0x7;
        }

#endregion

#region "Member Variables"
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedSetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifStaticControl;
        protected Button m_cachedNextButton;

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
        internal CouldNotLocateDatabaseDialog(App app) : 
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
                throw new CouldNotLocateDatabaseDialogotNotFoundException("Could Not Locate Database Dialog not found");
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
        internal virtual ICouldNotLocateDatabaseDialogControls Controls
        {
            get
            {
                return this;
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
        Button ICouldNotLocateDatabaseDialogControls.BackButton
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
        Button ICouldNotLocateDatabaseDialogControls.CancelButton
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
        ///  Exposes access to the SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ICouldNotLocateDatabaseDialogControls.SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifStaticControl
        {
            get
            {
                if ((m_cachedSetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifStaticControl == null))
                {
                    m_cachedSetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifStaticControl = new StaticControl(this, ControlIDs.SetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifStaticControl);
                }
                return m_cachedSetupReportedTheFollowingErrorWhenAttemptingToLocateTheMicrosoftOperationsManagerDatabaseOnTheSpecifStaticControl;
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
        Button ICouldNotLocateDatabaseDialogControls.NextButton
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

#endregion

#region "Methods"
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

#endregion

    }
}


