namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{


    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class EMailAccountDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public EMailAccountDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IEMailAccountDialogControls interface definition"
    
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IEMailAccountDialogControls
    {
        StaticControl TransportStaticControl  {get;}
        EditComboBox TransportEditComboBox  {get;}
        StaticControl ExchangeServerStaticControl  {get;}
        TextBox ExchangeServerTextBox  {get;}
        StaticControl MailboxStaticControl  {get;}
        TextBox MailboxTextBox  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: EMailAccountDialog
    ///  Copyright (C) 2002, oration
    /// -----------------------------------------------------------------------------
    ///  <summary>
    ///  TODO: Add dialog functionality description here.
    ///  </summary>
    ///  <remarks></remarks>
    ///  <history>
    /// 	[deangj] 9/20/2003 Created
    ///  </history>
    /// -----------------------------------------------------------------------------
    internal class EMailAccountDialog : Dialog, IEMailAccountDialogControls
    {


#region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - E-Mail Account";
            internal const string Transport = "&Transport:";
            internal const string ExchangeServer = "Exchange server";
            internal const string Mailbox = "Mailbox";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccou = "You can have Consolidators send notifications by e-mail. \r\n\r\nIf you want notifica" +
                "tions sent by e-mail, specify the e-mail account information.  You can skip this" +
                " configuration by leaving the Server and Mailbox fields blank.";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int TransportStaticControl = 0x3;
            internal const int TransportEditComboBox = 0x4;
            internal const int ExchangeServerStaticControl = 0x5;
            internal const int ExchangeServerTextBox = 0x6;
            internal const int MailboxStaticControl = 0x7;
            internal const int MailboxTextBox = 0x8;
            internal const int NextButton = 0xB;
            internal const int BackButton = 0xC;
            internal const int CancelButton = 0xD;
            internal const int YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl = 0xF;
        }

#endregion

#region "Member Variables"
        protected StaticControl m_cachedTransportStaticControl;
        protected EditComboBox m_cachedTransportEditComboBox;
        protected StaticControl m_cachedExchangeServerStaticControl;
        protected TextBox m_cachedExchangeServerTextBox;
        protected StaticControl m_cachedMailboxStaticControl;
        protected TextBox m_cachedMailboxTextBox;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedYouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl;

#endregion

#region "Constructor and Init function"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  TODO: Add a description for your constructor.
        ///  </summary>
        ///  <param name="app">App object owning the dialog.</param>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal EMailAccountDialog(App app) : 
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
        /// 	[deangj] 9/20/2003 Created
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual IEMailAccountDialogControls Controls
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
        /// 	[deangj] 9/20/2003 Created
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
        ///  Routine to set/get the text in control ExchangeServer
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string ExchangeServerText
        {
            get
            {
                return Controls.ExchangeServerTextBox.Text;
            }
            set
            {
                Controls.ExchangeServerTextBox.Text = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control Mailbox
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string MailboxText
        {
            get
            {
                return Controls.MailboxTextBox.Text;
            }
            set
            {
                Controls.MailboxTextBox.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the TransportStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountDialogControls.TransportStaticControl
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        EditComboBox IEMailAccountDialogControls.TransportEditComboBox
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
        ///  Exposes access to the ExchangeServerStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountDialogControls.ExchangeServerStaticControl
        {
            get
            {
                if ((m_cachedExchangeServerStaticControl == null))
                {
                    m_cachedExchangeServerStaticControl = new StaticControl(this, ControlIDs.ExchangeServerStaticControl);
                }
                return m_cachedExchangeServerStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ExchangeServerTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IEMailAccountDialogControls.ExchangeServerTextBox
        {
            get
            {
                if ((m_cachedExchangeServerTextBox == null))
                {
                    m_cachedExchangeServerTextBox = new TextBox(this, ControlIDs.ExchangeServerTextBox);
                }
                return m_cachedExchangeServerTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the MailboxStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountDialogControls.MailboxStaticControl
        {
            get
            {
                if ((m_cachedMailboxStaticControl == null))
                {
                    m_cachedMailboxStaticControl = new StaticControl(this, ControlIDs.MailboxStaticControl);
                }
                return m_cachedMailboxStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the MailboxTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IEMailAccountDialogControls.MailboxTextBox
        {
            get
            {
                if ((m_cachedMailboxTextBox == null))
                {
                    m_cachedMailboxTextBox = new TextBox(this, ControlIDs.MailboxTextBox);
                }
                return m_cachedMailboxTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NextButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IEMailAccountDialogControls.NextButton
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IEMailAccountDialogControls.BackButton
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button IEMailAccountDialogControls.CancelButton
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IEMailAccountDialogControls.YouCanHaveConsolidatorsSendNotificationsByEmailIfYouWantNotificationsSentByEmailSpecifyTheEmailAccouStaticControl
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

#endregion

#region "Methods"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button Next
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
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
        /// 	[deangj] 9/20/2003 Created
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
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickCancel()
        {
            Controls.CancelButton.Click();
        }

#endregion

    }
}


