namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class DatabaseInstanceDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public DatabaseInstanceDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IDatabaseInstanceDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IDatabaseInstanceDialogControls
    {
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl YouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostTStaticControl  {get;}
        StaticControl ServerNameStaticControl  {get;}
        EditComboBox AccountEditComboBox  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: DatabaseInstanceDialog
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
    internal class DatabaseInstanceDialog : Dialog, IDatabaseInstanceDialogControls
    {

#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Database Instance";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string YouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostT = "You have selected to install Microsoft Operations Manager database. \r\n\r\nSelect th" +
                "e name of the SQL Server instance to host the Microsoft Operations Manager 2000 " +
                "database.";
            internal const string ServerName = "&Server name:";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            internal const int NextButton = 3;
            internal const int BackButton = 4;
            internal const int CancelButton = 5;
            internal const int YouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostTStaticControl = 7;
            internal const int ServerNameStaticControl = 8;
            internal const int AccountEditComboBox = 9;
        }
#endregion

#region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedYouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostTStaticControl;
        protected StaticControl m_cachedServerNameStaticControl;
        protected EditComboBox m_cachedAccountEditComboBox;
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
        internal DatabaseInstanceDialog(App app) : 
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
                throw new DatabaseInstanceDialogNotFoundException("Database Instance Dialog not found"); 
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
        internal virtual IDatabaseInstanceDialogControls Controls
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
                return Controls.AccountEditComboBox.Text;
            }
            set
            {
                Controls.AccountEditComboBox.Text = value;
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
        Button IDatabaseInstanceDialogControls.NextButton
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
        Button IDatabaseInstanceDialogControls.BackButton
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
        Button IDatabaseInstanceDialogControls.CancelButton
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
        ///  Exposes access to the YouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostTStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseInstanceDialogControls.YouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostTStaticControl
        {
            get
            {
                if ((m_cachedYouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostTStaticControl == null))
                {
                    m_cachedYouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostTStaticControl = new StaticControl(this, ControlIDs.YouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostTStaticControl);
                }
                return m_cachedYouHaveSelectedToInstallMicrosoftOperationsManagerDatabaseSelectTheNameOfTheSQLServerInstanceToHostTStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ServerNameStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseInstanceDialogControls.ServerNameStaticControl
        {
            get
            {
                if ((m_cachedServerNameStaticControl == null))
                {
                    m_cachedServerNameStaticControl = new StaticControl(this, ControlIDs.ServerNameStaticControl);
                }
                return m_cachedServerNameStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the AccountEditComboBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        EditComboBox IDatabaseInstanceDialogControls.AccountEditComboBox
        {
            get
            {
                if ((m_cachedAccountEditComboBox == null))
                {
                    m_cachedAccountEditComboBox = new EditComboBox(this, ControlIDs.AccountEditComboBox);
                }
                return m_cachedAccountEditComboBox;
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

