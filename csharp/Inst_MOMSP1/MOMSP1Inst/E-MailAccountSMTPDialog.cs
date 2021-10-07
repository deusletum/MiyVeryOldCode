namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

#region "IEMailAccountSMTPDialogControls interface definition"
     
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IEMailAccountSMTPDialogControls
    {
        StaticControl TransportStaticControl  {get;}
        EditComboBox TransportEditComboBox  {get;}
        StaticControl ServerNameStaticControl  {get;}
        TextBox ServerNameTextBox  {get;}
        StaticControl ReturnAddressStaticControl  {get;}
        TextBox ReturnAddressTextBox  {get;}
        StaticControl SMTPPortStaticControl  {get;}
        TextBox SMTPPortTextBox  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl  {get;}
        StaticControl NoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForThStaticControl  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: EMailAccountSMTPDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  TODO: Add dialog functionality description here.
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[deangj] 9/27/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class EMailAccountSMTPDialog : Dialog, IEMailAccountSMTPDialogControls
    {


#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - E-Mail Account";
            internal const string Transport = "&Transport:";
            internal const string ServerName = "Server name";
            internal const string ReturnAddress = "Return address";
            internal const string SMTPPort = "SMTP &Port";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccou = "You can have Consolidators send notifications by e-mail. \r\n\r\nIf you want notifica" +
                "tions sent by e-mail, specify the e-mail account information.  You can skip this" +
                " configuration by leaving the Server and Mailbox fields blank.";
            internal const string NoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForTh = "Note that this option requires you to provide the internet address (not the addre" +
                "ss selected from the Address book) for the operator.";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int TransportStaticControl = 0x3;
            internal const int TransportEditComboBox = 0x4;
            internal const int ServerNameStaticControl = 0x5;
            internal const int ServerNameTextBox = 0x6;
            internal const int ReturnAddressStaticControl = 0x7;
            internal const int ReturnAddressTextBox = 0x8;
            internal const int SMTPPortStaticControl = 0x9;
            internal const int SMTPPortTextBox = 0xA;
            internal const int NextButton = 0xB;
            internal const int BackButton = 0xC;
            internal const int CancelButton = 0xD;
            internal const int YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl = 0xF;
            internal const int NoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForThStaticControl = 0x10;
        }

#endregion

#region "Member Variables"
        protected StaticControl m_cachedTransportStaticControl;
        protected EditComboBox m_cachedTransportEditComboBox;
        protected StaticControl m_cachedServerNameStaticControl;
        protected TextBox m_cachedServerNameTextBox;
        protected StaticControl m_cachedReturnAddressStaticControl;
        protected TextBox m_cachedReturnAddressTextBox;
        protected StaticControl m_cachedSMTPPortStaticControl;
        protected TextBox m_cachedSMTPPortTextBox;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedYouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl;
        protected StaticControl m_cachedNoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForThStaticControl;

#endregion

#region "Constructor and Init function"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  TODO: Add a description for your constructor.
        ///  </summary>
        ///  <param name="app">App object owning the dialog.</param>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal EMailAccountSMTPDialog(App app) : 
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
        /// 	[deangj] 9/27/2003 Created
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
                throw new EMailAccountDialogNotFoundException("Email Account Dialog not found");
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
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual IEMailAccountSMTPDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control Transport
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string TransportText
        {
            get
            {
                return Controls.TransportEditComboBox.Text;
            }
            set
            {
                Controls.TransportEditComboBox.Text = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control ServerName
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
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
        ///  Routine to set/get the text in control ReturnAddress
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string ReturnAddressText
        {
            get
            {
                return Controls.ReturnAddressTextBox.Text;
            }
            set
            {
                Controls.ReturnAddressTextBox.Text = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control SMTPPort
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string SMTPPortText
        {
            get
            {
                return Controls.SMTPPortTextBox.Text;
            }
            set
            {
                Controls.SMTPPortTextBox.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the TransportStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountSMTPDialogControls.TransportStaticControl
        {
            get
            {
                if ((m_cachedTransportStaticControl == null))
                {
                    m_cachedTransportStaticControl = new StaticControl(this, ControlIDs.TransportStaticControl);
                }
                return m_cachedTransportStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the TransportEditComboBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        EditComboBox IEMailAccountSMTPDialogControls.TransportEditComboBox
        {
            get
            {
                if ((m_cachedTransportEditComboBox == null))
                {
                    m_cachedTransportEditComboBox = new EditComboBox(this, ControlIDs.TransportEditComboBox);
                }
                return m_cachedTransportEditComboBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ServerNameStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountSMTPDialogControls.ServerNameStaticControl
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
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IEMailAccountSMTPDialogControls.ServerNameTextBox
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
        ///  Exposes access to the ReturnAddressStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountSMTPDialogControls.ReturnAddressStaticControl
        {
            get
            {
                if ((m_cachedReturnAddressStaticControl == null))
                {
                    m_cachedReturnAddressStaticControl = new StaticControl(this, ControlIDs.ReturnAddressStaticControl);
                }
                return m_cachedReturnAddressStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ReturnAddressTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IEMailAccountSMTPDialogControls.ReturnAddressTextBox
        {
            get
            {
                if ((m_cachedReturnAddressTextBox == null))
                {
                    m_cachedReturnAddressTextBox = new TextBox(this, ControlIDs.ReturnAddressTextBox);
                }
                return m_cachedReturnAddressTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SMTPPortStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountSMTPDialogControls.SMTPPortStaticControl
        {
            get
            {
                if ((m_cachedSMTPPortStaticControl == null))
                {
                    m_cachedSMTPPortStaticControl = new StaticControl(this, ControlIDs.SMTPPortStaticControl);
                }
                return m_cachedSMTPPortStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SMTPPortTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IEMailAccountSMTPDialogControls.SMTPPortTextBox
        {
            get
            {
                if ((m_cachedSMTPPortTextBox == null))
                {
                    m_cachedSMTPPortTextBox = new TextBox(this, ControlIDs.SMTPPortTextBox);
                }
                return m_cachedSMTPPortTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NextButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IEMailAccountSMTPDialogControls.NextButton
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
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IEMailAccountSMTPDialogControls.BackButton
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
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IEMailAccountSMTPDialogControls.CancelButton
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
        ///  Exposes access to the YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountSMTPDialogControls.YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl
        {
            get
            {
                if ((m_cachedYouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl == null))
                {
                    m_cachedYouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl = new StaticControl(this, ControlIDs.YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl);
                }
                return m_cachedYouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForThStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountSMTPDialogControls.NoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForThStaticControl
        {
            get
            {
                if ((m_cachedNoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForThStaticControl == null))
                {
                    m_cachedNoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForThStaticControl = new StaticControl(this, ControlIDs.NoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForThStaticControl);
                }
                return m_cachedNoteThatThisOptionRequiresYouToProvideTheInternetAddressNotTheAddressSelectedFromTheAddressBookForThStaticControl;
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
        /// 	[deangj] 9/27/2003 Created
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
        /// 	[deangj] 9/27/2003 Created
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
        /// 	[deangj] 9/27/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickCancel()
        {
            Controls.CancelButton.Click();
        }

#endregion

    }
}


