namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class StopServicesDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public StopServicesDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IStopServicesDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IStopServicesDialogControls
    {
        Button ContinueButton  {get;}
        TextBox AccountTextBox  {get;}
        StaticControl InOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCanStaticControl  {get;}
        Button CancelButton  {get;}
        StaticControl ServicesStaticControl  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: StopServicesDialog
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
    internal class StopServicesDialog : Dialog, IStopServicesDialogControls
    {

#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup";
            internal const string Continue = "&Continue";
            internal const string InOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCan = "In order for Setup to continue, these services must not be running.\r\n\r\nTo have Se" +
                "tup stop these services, click Continue.\r\n\r\nTo cancel installation, click Cancel" +
                ".";
            internal const string Cancel = "Cancel";
            internal const string Services = "Services:";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            internal const int ContinueButton = 3;
            internal const int AccountTextBox = 4;
            internal const int InOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCanStaticControl = 5;
            internal const int CancelButton = 6;
            internal const int ServicesStaticControl = 7;
        }
#endregion

#region "Member Variables"
        protected Button m_cachedContinueButton;
        protected TextBox m_cachedAccountTextBox;
        protected StaticControl m_cachedInOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCanStaticControl;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedServicesStaticControl;
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
        internal StopServicesDialog(App app) : 
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
                tempWindow = new Window(Strings.DialogTitle, StringMatchSyntax.ExactMatch, WindowClassNames.Alert, StringMatchSyntax.ExactMatch, app, 3000);
            }
            catch (Exceptions.WindowNotFoundException)
            {
                throw new StopServicesDialogNotFoundException("Stop Services Dialog not found");
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
        internal virtual IStopServicesDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control Account
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string AccountText
        {
            get
            {
                return Controls.AccountTextBox.Text;
            }
            set
            {
                Controls.AccountTextBox.Text = value;
            }
        }


        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ContinueButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IStopServicesDialogControls.ContinueButton
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
        ///  Exposes access to the AccountTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IStopServicesDialogControls.AccountTextBox
        {
            get
            {
                if ((m_cachedAccountTextBox == null))
                {
                    m_cachedAccountTextBox = new TextBox(this, ControlIDs.AccountTextBox);
                }
                return m_cachedAccountTextBox;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the InOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCanStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IStopServicesDialogControls.InOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCanStaticControl
        {
            get
            {
                if ((m_cachedInOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCanStaticControl == null))
                {
                    m_cachedInOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCanStaticControl = new StaticControl(this, ControlIDs.InOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCanStaticControl);
                }
                return m_cachedInOrderForSetupToContinueTheseServicesMustNotBeRunningToHaveSetupStopTheseServicesClickContinueToCanStaticControl;
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
        Button IStopServicesDialogControls.CancelButton
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
        ///  Exposes access to the ServicesStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IStopServicesDialogControls.ServicesStaticControl
        {
            get
            {
                if ((m_cachedServicesStaticControl == null))
                {
                    m_cachedServicesStaticControl = new StaticControl(this, ControlIDs.ServicesStaticControl);
                }
                return m_cachedServicesStaticControl;
            }
        }

#endregion

#region "Methods"
        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Continue
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickContinue()
        {
            Controls.ContinueButton.Click();
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

#endregion

    }
}

