namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class StartServiceDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public StartServiceDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IStartServiceDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IStartServiceDialogControls
    {
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl TheAgentManagerAndConsolidatorRunAsWindowsServicesStaticControl  {get;}
        CheckBox AutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox  {get;}
        CheckBox StartTheServiceAfterSetupFinishesCheckBox  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: StartServiceDialog
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
    internal class StartServiceDialog : Dialog, IStartServiceDialogControls
    {

#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Start Service";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string TheAgentManagerAndConsolidatorRunAsWindowsServices = "The Agent Manager and Consolidator run as Windows services.";
            internal const string AutomaticallyStartTheServiceEveryTimeTheComputerStarts = "&Automatically start the service every time the computer starts";
            internal const string StartTheServiceAfterSetupFinishes = "&Start the service after Setup finishes";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            internal const int NextButton = 3;
            internal const int BackButton = 4;
            internal const int CancelButton = 5;
            internal const int TheAgentManagerAndConsolidatorRunAsWindowsServicesStaticControl = 7;
            internal const int AutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox = 8;
            internal const int StartTheServiceAfterSetupFinishesCheckBox = 9;
        }
#endregion

#region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedTheAgentManagerAndConsolidatorRunAsWindowsServicesStaticControl;
        protected CheckBox m_cachedAutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox;
        protected CheckBox m_cachedStartTheServiceAfterSetupFinishesCheckBox;
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
        internal StartServiceDialog(App app) : 
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
                throw new StartServiceDialogNotFoundException("Start Service Dialog not found");
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
        internal virtual IStartServiceDialogControls Controls
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
        Button IStartServiceDialogControls.NextButton
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
        Button IStartServiceDialogControls.BackButton
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
        Button IStartServiceDialogControls.CancelButton
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
        ///  Exposes access to the TheAgentManagerAndConsolidatorRunAsWindowsServicesStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IStartServiceDialogControls.TheAgentManagerAndConsolidatorRunAsWindowsServicesStaticControl
        {
            get
            {
                if ((m_cachedTheAgentManagerAndConsolidatorRunAsWindowsServicesStaticControl == null))
                {
                    m_cachedTheAgentManagerAndConsolidatorRunAsWindowsServicesStaticControl = new StaticControl(this, ControlIDs.TheAgentManagerAndConsolidatorRunAsWindowsServicesStaticControl);
                }
                return m_cachedTheAgentManagerAndConsolidatorRunAsWindowsServicesStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the AutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IStartServiceDialogControls.AutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox
        {
            get
            {
                if ((m_cachedAutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox == null))
                {
                    m_cachedAutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox = new CheckBox(this, ControlIDs.AutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox);
                }
                return m_cachedAutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the StartTheServiceAfterSetupFinishesCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IStartServiceDialogControls.StartTheServiceAfterSetupFinishesCheckBox
        {
            get
            {
                if ((m_cachedStartTheServiceAfterSetupFinishesCheckBox == null))
                {
                    m_cachedStartTheServiceAfterSetupFinishesCheckBox = new CheckBox(this, ControlIDs.StartTheServiceAfterSetupFinishesCheckBox);
                }
                return m_cachedStartTheServiceAfterSetupFinishesCheckBox;
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
        ///  Routine to click on button AutomaticallyStartTheServiceEveryTimeTheComputerStarts
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickAutomaticallyStartTheServiceEveryTimeTheComputerStarts()
        {
            Controls.AutomaticallyStartTheServiceEveryTimeTheComputerStartsCheckBox.Click();
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button StartTheServiceAfterSetupFinishes
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickStartTheServiceAfterSetupFinishes()
        {
            Controls.StartTheServiceAfterSetupFinishesCheckBox.Click();
        }

#endregion

    }
}

