namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class DASAccountInformationDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public DASAccountInformationDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IDASAccountInformationDialogControls interface definition"  
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IDASAccountInformationDialogControls
    {
        StaticControl AccountStaticControl  {get;}
        TextBox AccountTextBox  {get;}
        StaticControl PasswordStaticControl  {get;}
        TextBox PasswordTextBox  {get;}
        StaticControl DomainOrLocalComputerStaticControl  {get;}
        TextBox DomainOrLocalComputerTextBox  {get;}
        CheckBox UseThisAccountForConsolidatorAgentManagerComponentCheckBox  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl TheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWitStaticControl  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: DASAccountInformationDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  TODO: Add dialog functionality description here.
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[deangj] 10/1/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class DASAccountInformationDialog : Dialog, IDASAccountInformationDialogControls
    {


#region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - DAS Account Information";
            internal const string Account = "&Account:";
            internal const string Password = "&Password:";
            internal const string DomainOrLocalComputer = "&Domain or Local Computer:";
            internal const string UseThisAccountForConsolidatorAgentManagerComponent = "&Use this account for Consolidator/Agent Manager component";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string TheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWit = "The Data Access Server (DAS) provides centralized database access logic. The DAS " +
                "computer must log on using an account with Administrator privileges both on the " +
                "DAS and on the computer containing the database. \r\n\r\nIf you want to use this acc" +
                "ount for the C";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int AccountStaticControl = 0x3;
            internal const int AccountTextBox = 0x4;
            internal const int PasswordStaticControl = 0x5;
            internal const int PasswordTextBox = 0x6;
            internal const int DomainOrLocalComputerStaticControl = 0x7;
            internal const int DomainOrLocalComputerTextBox = 0x8;
            internal const int UseThisAccountForConsolidatorAgentManagerComponentCheckBox = 0x9;
            internal const int NextButton = 0xA;
            internal const int BackButton = 0xB;
            internal const int CancelButton = 0xC;
            internal const int TheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWitStaticControl = 0xE;
        }

#endregion

#region "Member Variables"
        protected StaticControl m_cachedAccountStaticControl;
        protected TextBox m_cachedAccountTextBox;
        protected StaticControl m_cachedPasswordStaticControl;
        protected TextBox m_cachedPasswordTextBox;
        protected StaticControl m_cachedDomainOrLocalComputerStaticControl;
        protected TextBox m_cachedDomainOrLocalComputerTextBox;
        protected CheckBox m_cachedUseThisAccountForConsolidatorAgentManagerComponentCheckBox;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedTheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWitStaticControl;

#endregion

#region "Constructor and Init function"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  TODO: Add a description for your constructor.
        ///  </summary>
        ///  <param name="app">App object owning the dialog.</param>
        ///  <history>
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal DASAccountInformationDialog(App app) : 
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
        /// 	[deangj] 10/1/2003 Created
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
                throw new DASAccountInformationDialogNotFoundException("DAS Account Information Dialog not found");
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual IDASAccountInformationDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Property to handle checkbox UseThisAccountForConsolidatorAgentManagerComponent
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual bool UseThisAccountForConsolidatorAgentManagerComponent
        {
            get
            {
                return Controls.UseThisAccountForConsolidatorAgentManagerComponentCheckBox.Checked;
            }
            set
            {
                Controls.UseThisAccountForConsolidatorAgentManagerComponentCheckBox.Checked = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control Account
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 10/1/2003 Created
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
        /// 	[deangj] 10/1/2003 Created
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
        /// 	[deangj] 10/1/2003 Created
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDASAccountInformationDialogControls.AccountStaticControl
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IDASAccountInformationDialogControls.AccountTextBox
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDASAccountInformationDialogControls.PasswordStaticControl
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IDASAccountInformationDialogControls.PasswordTextBox
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDASAccountInformationDialogControls.DomainOrLocalComputerStaticControl
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IDASAccountInformationDialogControls.DomainOrLocalComputerTextBox
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
        ///  Exposes access to the UseThisAccountForConsolidatorAgentManagerComponentCheckBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        CheckBox IDASAccountInformationDialogControls.UseThisAccountForConsolidatorAgentManagerComponentCheckBox
        {
            get
            {
                if ((m_cachedUseThisAccountForConsolidatorAgentManagerComponentCheckBox == null))
                {
                    m_cachedUseThisAccountForConsolidatorAgentManagerComponentCheckBox = new CheckBox(this, ControlIDs.UseThisAccountForConsolidatorAgentManagerComponentCheckBox);
                }
                return m_cachedUseThisAccountForConsolidatorAgentManagerComponentCheckBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NextButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDASAccountInformationDialogControls.NextButton
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDASAccountInformationDialogControls.BackButton
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDASAccountInformationDialogControls.CancelButton
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
        ///  Exposes access to the TheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWitStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDASAccountInformationDialogControls.TheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWitStaticControl
        {
            get
            {
                if ((m_cachedTheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWitStaticControl == null))
                {
                    m_cachedTheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWitStaticControl = new StaticControl(this, ControlIDs.TheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWitStaticControl);
                }
                return m_cachedTheDataAccessServerDASProvidesCentralizedDatabaseAccessLogicTheDASComputerMustLogOnUsingAnAccountWitStaticControl;
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
        /// 	[deangj] 10/1/2003 Created
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
        /// 	[deangj] 10/1/2003 Created
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
        /// 	[deangj] 10/1/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickCancel()
        {
            Controls.CancelButton.Click();
        }

#endregion

    }
}


