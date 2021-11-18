namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class WelcomeDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public WelcomeDialogNotFoundException(string message): base(message)
        {
        }
    }
    

#region "IWelcomeDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IWelcomeDialogControls
    {
        Button NextButton  {get;}
        Button CancelButton  {get;}
        StaticControl WelcomeToMicrosoftOperationsManager2000SetupStaticControl  {get;}
        StaticControl SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloStaticControl  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: WelcomeDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  MOM10SP1 Welcome Dialog
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[deangj] 7/22/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class WelcomeDialog : Dialog, IWelcomeDialogControls
    {

#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Welcome";
            internal const string Next = "&Next >";
            internal const string Cancel = "Cancel";
            internal const string WelcomeToMicrosoftOperationsManager2000Setup = "Welcome to Microsoft Operations Manager 2000 Setup";
            internal const string SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupClo = "Setup will install one or more components for Microsoft Operations Manager 2000 o" +
                "n this computer.  \r\n\r\nTo continue Setup, close all other open programs and then " +
                "click Next.";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            internal const int NextButton = 3;
            internal const int CancelButton = 4;
            internal const int WelcomeToMicrosoftOperationsManager2000SetupStaticControl = 6;
            internal const int SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloStaticControl = 7;
        }
#endregion

#region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedWelcomeToMicrosoftOperationsManager2000SetupStaticControl;
        protected StaticControl m_cachedSetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloStaticControl;
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
        internal WelcomeDialog(App app) : 
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
                throw new WelcomeDialogNotFoundException("Welcome Dialog Not Found.");    
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
        internal virtual IWelcomeDialogControls Controls
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
        Button IWelcomeDialogControls.NextButton
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
        Button IWelcomeDialogControls.CancelButton
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
        ///  Exposes access to the WelcomeToMicrosoftOperationsManager2000SetupStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IWelcomeDialogControls.WelcomeToMicrosoftOperationsManager2000SetupStaticControl
        {
            get
            {
                if ((m_cachedWelcomeToMicrosoftOperationsManager2000SetupStaticControl == null))
                {
                    m_cachedWelcomeToMicrosoftOperationsManager2000SetupStaticControl = new StaticControl(this, ControlIDs.WelcomeToMicrosoftOperationsManager2000SetupStaticControl);
                }
                return m_cachedWelcomeToMicrosoftOperationsManager2000SetupStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IWelcomeDialogControls.SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloStaticControl
        {
            get
            {
                if ((m_cachedSetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloStaticControl == null))
                {
                    m_cachedSetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloStaticControl = new StaticControl(this, ControlIDs.SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloStaticControl);
                }
                return m_cachedSetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloStaticControl;
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

#endregion

    }
}

