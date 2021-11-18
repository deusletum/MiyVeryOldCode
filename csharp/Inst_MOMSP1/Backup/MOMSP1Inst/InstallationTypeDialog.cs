namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class InstallationTypeDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public InstallationTypeDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IInstallationTypeDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IInstallationTypeDialogControls
    {
        StaticControl TypeOfInstallationStaticControl  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        RadioButton TypicalRadioButton  {get;}
        RadioButton UserInterfacesRadioButton  {get;}
        RadioButton CustomRadioButton  {get;}
        RadioButton ExpressRadioButton  {get;}
        TextBox AccountTextBox  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: InstallationTypeDialog
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
    internal class InstallationTypeDialog : Dialog, IInstallationTypeDialogControls
    {

#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Installation Type";
            internal const string TypeOfInstallation = "Type of installation:";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string Typical = "&Typical";
            internal const string UserInterfaces = "&User Interfaces";
            internal const string Custom = "&Custom";
            internal const string Express = "&Express";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            internal const int TypeOfInstallationStaticControl = 3;
            internal const int NextButton = 4;
            internal const int BackButton = 5;
            internal const int CancelButton = 6;
            internal const int TypicalRadioButton = 8;
            internal const int UserInterfacesRadioButton = 9;
            internal const int CustomRadioButton = 10;
            internal const int ExpressRadioButton = 11;
            internal const int AccountTextBox = 12;
        }
#endregion

#region "Member Variables"
        protected StaticControl m_cachedTypeOfInstallationStaticControl;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected RadioButton m_cachedTypicalRadioButton;
        protected RadioButton m_cachedUserInterfacesRadioButton;
        protected RadioButton m_cachedCustomRadioButton;
        protected RadioButton m_cachedExpressRadioButton;
        protected TextBox m_cachedAccountTextBox;
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
        internal InstallationTypeDialog(App app) : 
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
                throw new InstallationTypeDialogNotFoundException("Installation Type Dialog not found");
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
        internal virtual IInstallationTypeDialogControls Controls
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
        ///  Exposes access to the TypeOfInstallationStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IInstallationTypeDialogControls.TypeOfInstallationStaticControl
        {
            get
            {
                if ((m_cachedTypeOfInstallationStaticControl == null))
                {
                    m_cachedTypeOfInstallationStaticControl = new StaticControl(this, ControlIDs.TypeOfInstallationStaticControl);
                }
                return m_cachedTypeOfInstallationStaticControl;
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
        Button IInstallationTypeDialogControls.NextButton
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
        Button IInstallationTypeDialogControls.BackButton
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
        Button IInstallationTypeDialogControls.CancelButton
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
        ///  Exposes access to the TypicalRadioButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        RadioButton IInstallationTypeDialogControls.TypicalRadioButton
        {
            get
            {
                if ((m_cachedTypicalRadioButton == null))
                {
                    m_cachedTypicalRadioButton = new RadioButton(this, ControlIDs.TypicalRadioButton);
                }
                return m_cachedTypicalRadioButton;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the UserInterfacesRadioButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        RadioButton IInstallationTypeDialogControls.UserInterfacesRadioButton
        {
            get
            {
                if ((m_cachedUserInterfacesRadioButton == null))
                {
                    m_cachedUserInterfacesRadioButton = new RadioButton(this, ControlIDs.UserInterfacesRadioButton);
                }
                return m_cachedUserInterfacesRadioButton;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the CustomRadioButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        RadioButton IInstallationTypeDialogControls.CustomRadioButton
        {
            get
            {
                if ((m_cachedCustomRadioButton == null))
                {
                    m_cachedCustomRadioButton = new RadioButton(this, ControlIDs.CustomRadioButton);
                }
                return m_cachedCustomRadioButton;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ExpressRadioButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        RadioButton IInstallationTypeDialogControls.ExpressRadioButton
        {
            get
            {
                if ((m_cachedExpressRadioButton == null))
                {
                    m_cachedExpressRadioButton = new RadioButton(this, ControlIDs.ExpressRadioButton);
                }
                return m_cachedExpressRadioButton;
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
        TextBox IInstallationTypeDialogControls.AccountTextBox
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
        ///  Routine to click on button Typical
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickTypical()
        {
            Controls.TypicalRadioButton.Click();
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button UserInterfaces
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickUserInterfaces()
        {
            Controls.UserInterfacesRadioButton.Click();
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Custom
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickCustom()
        {
            Controls.CustomRadioButton.Click();
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Express
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickExpress()
        {
            Controls.ExpressRadioButton.Click();
        }

#endregion

    }
}

