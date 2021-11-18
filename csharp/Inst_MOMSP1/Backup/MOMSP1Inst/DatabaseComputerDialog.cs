namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class DatabaseComputerDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public DatabaseComputerDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IDatabaseComputerDialogControls interface definition"
    
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IDatabaseComputerDialogControls
    {
        StaticControl ServerNameStaticControl  {get;}
        TextBox ServerNameTextBox  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl YouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNamStaticControl  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: DatabaseComputerDialog
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
    internal class DatabaseComputerDialog : Dialog, IDatabaseComputerDialogControls
    {


#region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Database Computer";
            internal const string ServerName = "&Server name:";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string YouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNam = "You have selected to install a component that requires an existing Microsoft Oper" +
                "ations Manager database. \r\n\r\nType the name of the SQL Server computer that hosts" +
                " the Microsoft Operations Manager 2000 database. If the database is on a non-def" +
                "ault instance ";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int ServerNameStaticControl = 0x3;
            internal const int ServerNameTextBox = 0x4;
            internal const int NextButton = 0x5;
            internal const int BackButton = 0x6;
            internal const int CancelButton = 0x7;
            internal const int YouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNamStaticControl = 0x9;
        }

#endregion

#region "Member Variables"
        protected StaticControl m_cachedServerNameStaticControl;
        protected TextBox m_cachedServerNameTextBox;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedYouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNamStaticControl;

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
        internal DatabaseComputerDialog(App app) : 
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
                throw new DatabaseComputerDialogNotFoundException("Database Computer Dialog not found");
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
        internal virtual IDatabaseComputerDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control ServerName
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string ServerNameText
        {
            get
            {
                return Controls.ServerNameTextBox.Text;
            }
            set
            {
                Controls.ServerNameTextBox.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ServerNameStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseComputerDialogControls.ServerNameStaticControl
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
        ///  Exposes access to the ServerNameTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IDatabaseComputerDialogControls.ServerNameTextBox
        {
            get
            {
                if ((m_cachedServerNameTextBox == null))
                {
                    m_cachedServerNameTextBox = new TextBox(this, ControlIDs.ServerNameTextBox);
                }
                return m_cachedServerNameTextBox;
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
        Button IDatabaseComputerDialogControls.NextButton
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
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IDatabaseComputerDialogControls.BackButton
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
        Button IDatabaseComputerDialogControls.CancelButton
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
        ///  Exposes access to the YouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNamStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IDatabaseComputerDialogControls.YouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNamStaticControl
        {
            get
            {
                if ((m_cachedYouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNamStaticControl == null))
                {
                    m_cachedYouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNamStaticControl = new StaticControl(this, ControlIDs.YouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNamStaticControl);
                }
                return m_cachedYouHaveSelectedToInstallAComponentThatRequiresAnExistingMicrosoftOperationsManagerDatabaseTypeTheNamStaticControl;
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
        /// 	[deangj] 9/11/2003 Created
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

#endregion

    }
}


