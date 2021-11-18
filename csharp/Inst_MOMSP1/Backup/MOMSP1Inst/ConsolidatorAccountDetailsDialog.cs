namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class ConsolidatorAccountDetailsDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public ConsolidatorAccountDetailsDialogNotFoundException(string message): base(message)
        {
        }
    }


#region "IConsolidatorAccountDetailsDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IConsolidatorAccountDetailsDialogControls
    {
        StaticControl AccountStaticControl  {get;}
        TextBox AccountTextBox  {get;}
        StaticControl PasswordStaticControl  {get;}
        TextBox PasswordTextBox  {get;}
        StaticControl DomainOrLocalComputerStaticControl  {get;}
        TextBox DomainOrLocalComputerTextBox  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl AnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistributeStaticControl  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: ConsolidatorAccountDetailsDialog
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
    internal class ConsolidatorAccountDetailsDialog : Dialog, IConsolidatorAccountDetailsDialogControls
    {

#region "Strings"
        internal class Strings
        {
            public const string DialogTitle = "Microsoft Operations Manager Setup - Consolidator Account Details";
            public const string Account = "&Account:";
            public const string Password = "&Password:";
            public const string DomainOrLocalComputer = "&Domain or Local Computer:";
            public const string Next = "&Next >";
            public const string Back = "< &Back";
            public const string Cancel = "Cancel";
            public const string AnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistribute = "An Agent Manager evaluates rules to determine which computers it needs to install" +
                " agents on. A Consolidator distributes rules and collects data from remote agent" +
                "s, performs any central automated response to detected conditions, and inserts c" +
                "ollected data ";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            public const int AccountStaticControl = 3;
            public const int AccountTextBox = 4;
            public const int PasswordStaticControl = 5;
            public const int PasswordTextBox = 6;
            public const int DomainOrLocalComputerStaticControl = 7;
            public const int DomainOrLocalComputerTextBox = 8;
            public const int NextButton = 9;
            public const int BackButton = 10;
            public const int CancelButton = 11;
            public const int AnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistributeStaticControl = 12;
        }
#endregion

#region "Member Variables"
        protected StaticControl m_cachedAccountStaticControl;
        protected TextBox m_cachedAccountTextBox;
        protected StaticControl m_cachedPasswordStaticControl;
        protected TextBox m_cachedPasswordTextBox;
        protected StaticControl m_cachedDomainOrLocalComputerStaticControl;
        protected TextBox m_cachedDomainOrLocalComputerTextBox;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedAnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistributeStaticControl;
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
        internal ConsolidatorAccountDetailsDialog(App app) : 
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
                throw new ConsolidatorAccountDetailsDialogNotFoundException("Consolidator Account Details Dialog not found");
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
        internal virtual IConsolidatorAccountDetailsDialogControls Controls
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
        ///  Routine to set/get the text in control Password
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string PasswordText
        {
            get
            {
                return Controls.PasswordTextBox.Text;
            }
            set
            {
                Controls.PasswordTextBox.Text = value;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control DomainOrLocalComputer
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string DomainOrLocalComputerText
        {
            get
            {
                return Controls.DomainOrLocalComputerTextBox.Text;
            }
            set
            {
                Controls.DomainOrLocalComputerTextBox.Text = value;
            }
        }


        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the AccountStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IConsolidatorAccountDetailsDialogControls.AccountStaticControl
        {
            get
            {
                if ((m_cachedAccountStaticControl == null))
                {
                    m_cachedAccountStaticControl = new StaticControl(this, ControlIDs.AccountStaticControl);
                }
                return m_cachedAccountStaticControl;
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
        TextBox IConsolidatorAccountDetailsDialogControls.AccountTextBox
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
        ///  Exposes access to the PasswordStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IConsolidatorAccountDetailsDialogControls.PasswordStaticControl
        {
            get
            {
                if ((m_cachedPasswordStaticControl == null))
                {
                    m_cachedPasswordStaticControl = new StaticControl(this, ControlIDs.PasswordStaticControl);
                }
                return m_cachedPasswordStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the PasswordTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IConsolidatorAccountDetailsDialogControls.PasswordTextBox
        {
            get
            {
                if ((m_cachedPasswordTextBox == null))
                {
                    m_cachedPasswordTextBox = new TextBox(this, ControlIDs.PasswordTextBox);
                }
                return m_cachedPasswordTextBox;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the DomainOrLocalComputerStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IConsolidatorAccountDetailsDialogControls.DomainOrLocalComputerStaticControl
        {
            get
            {
                if ((m_cachedDomainOrLocalComputerStaticControl == null))
                {
                    m_cachedDomainOrLocalComputerStaticControl = new StaticControl(this, ControlIDs.DomainOrLocalComputerStaticControl);
                }
                return m_cachedDomainOrLocalComputerStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the DomainOrLocalComputerTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IConsolidatorAccountDetailsDialogControls.DomainOrLocalComputerTextBox
        {
            get
            {
                if ((m_cachedDomainOrLocalComputerTextBox == null))
                {
                    m_cachedDomainOrLocalComputerTextBox = new TextBox(this, ControlIDs.DomainOrLocalComputerTextBox);
                }
                return m_cachedDomainOrLocalComputerTextBox;
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
        Button IConsolidatorAccountDetailsDialogControls.NextButton
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
        Button IConsolidatorAccountDetailsDialogControls.BackButton
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
        Button IConsolidatorAccountDetailsDialogControls.CancelButton
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
        ///  Exposes access to the AnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistributeStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IConsolidatorAccountDetailsDialogControls.AnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistributeStaticControl
        {
            get
            {
                if ((m_cachedAnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistributeStaticControl == null))
                {
                    m_cachedAnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistributeStaticControl = new StaticControl(this, ControlIDs.AnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistributeStaticControl);
                }
                return m_cachedAnAgentManagerEvaluatesRulesToDetermineWhichComputersItNeedsToInstallAgentsOnAConsolidatorDistributeStaticControl;
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

#endregion

    }
}

