namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class RegistrationInformationDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public RegistrationInformationDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "IRegistrationInformationDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IRegistrationInformationDialogControls
    {
        StaticControl UserNameStaticControl  {get;}
        TextBox WelcomeToMicrosoftOperationsManager2000SetupTextBox  {get;}
        StaticControl OrganizationStaticControl  {get;}
        TextBox SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox  {get;}
        TextBox InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox  {get;}
        TextBox _TextBox  {get;}
        TextBox _TextBox2  {get;}
        TextBox _TextBox3  {get;}
        TextBox _TextBox4  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl PleaseEnterYourNameAndOrganizationStaticControl  {get;}
        StaticControl InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasStaticControl  {get;}
        StaticControl _StaticControl  {get;}
        StaticControl _StaticControl2  {get;}
        StaticControl _StaticControl3  {get;}
        StaticControl _StaticControl4  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: RegistrationInformationDialog
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
    internal class RegistrationInformationDialog : Dialog, IRegistrationInformationDialogControls
    {

#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Registration Information";
            internal const string UserName = "&User name:";
            internal const string Organization = "&Organization:";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string PleaseEnterYourNameAndOrganization = "Please enter your name and organization:";
            internal const string InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCas = "In the boxes below, type your 25-character CD key. You\'ll find this number on the" +
                " yellow sticker on the back of the CD jewel case. \r\n\r\n&Product key:";
            internal const string _ = "_";
            internal const string _2 = "_";
            internal const string _3 = "_";
            internal const string _4 = "_";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            internal const int UserNameStaticControl = 3;
            internal const int WelcomeToMicrosoftOperationsManager2000SetupTextBox = 4;
            internal const int OrganizationStaticControl = 5;
            internal const int SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox = 6;
            internal const int InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox = 7;
            internal const int _TextBox = 8;
            internal const int _TextBox2 = 9;
            internal const int _TextBox3 = 10;
            internal const int _TextBox4 = 11;
            internal const int NextButton = 12;
            internal const int BackButton = 13;
            internal const int CancelButton = 14;
            internal const int PleaseEnterYourNameAndOrganizationStaticControl = 15;
            internal const int InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasStaticControl = 17;
            internal const int _StaticControl = 18;
            internal const int _StaticControl2 = 19;
            internal const int _StaticControl3 = 20;
            internal const int _StaticControl4 = 21;
        }
#endregion

#region "Member Variables"
        protected StaticControl m_cachedUserNameStaticControl;
        protected TextBox m_cachedWelcomeToMicrosoftOperationsManager2000SetupTextBox;
        protected StaticControl m_cachedOrganizationStaticControl;
        protected TextBox m_cachedSetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox;
        protected TextBox m_cachedInTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox;
        protected TextBox m_cached_TextBox;
        protected TextBox m_cached_TextBox2;
        protected TextBox m_cached_TextBox3;
        protected TextBox m_cached_TextBox4;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedPleaseEnterYourNameAndOrganizationStaticControl;
        protected StaticControl m_cachedInTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasStaticControl;
        protected StaticControl m_cached_StaticControl;
        protected StaticControl m_cached_StaticControl2;
        protected StaticControl m_cached_StaticControl3;
        protected StaticControl m_cached_StaticControl4;
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
        internal RegistrationInformationDialog(App app) : 
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
                throw new RegistrationInformationDialogNotFoundException("Registration Information Dialog not found");
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
        internal virtual IRegistrationInformationDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control WelcomeToMicrosoftOperationsManager2000Setup
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string WelcomeToMicrosoftOperationsManager2000SetupText
        {
            get
            {
                return Controls.WelcomeToMicrosoftOperationsManager2000SetupTextBox.Text;
            }
            set
            {
                Controls.WelcomeToMicrosoftOperationsManager2000SetupTextBox.Text = value;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupClo
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloText
        {
            get
            {
                return Controls.SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox.Text;
            }
            set
            {
                Controls.SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox.Text = value;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCas
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasText
        {
            get
            {
                return Controls.InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox.Text;
            }
            set
            {
                Controls.InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox.Text = value;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control _
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string _Text
        {
            get
            {
                return Controls._TextBox.Text;
            }
            set
            {
                Controls._TextBox.Text = value;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control _2
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string _2Text
        {
            get
            {
                return Controls._TextBox2.Text;
            }
            set
            {
                Controls._TextBox2.Text = value;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control _3
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string _3Text
        {
            get
            {
                return Controls._TextBox3.Text;
            }
            set
            {
                Controls._TextBox3.Text = value;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control _4
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string _4Text
        {
            get
            {
                return Controls._TextBox4.Text;
            }
            set
            {
                Controls._TextBox4.Text = value;
            }
        }


        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the UserNameStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IRegistrationInformationDialogControls.UserNameStaticControl
        {
            get
            {
                if ((m_cachedUserNameStaticControl == null))
                {
                    m_cachedUserNameStaticControl = new StaticControl(this, ControlIDs.UserNameStaticControl);
                }
                return m_cachedUserNameStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the WelcomeToMicrosoftOperationsManager2000SetupTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IRegistrationInformationDialogControls.WelcomeToMicrosoftOperationsManager2000SetupTextBox
        {
            get
            {
                if ((m_cachedWelcomeToMicrosoftOperationsManager2000SetupTextBox == null))
                {
                    m_cachedWelcomeToMicrosoftOperationsManager2000SetupTextBox = new TextBox(this, ControlIDs.WelcomeToMicrosoftOperationsManager2000SetupTextBox);
                }
                return m_cachedWelcomeToMicrosoftOperationsManager2000SetupTextBox;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the OrganizationStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IRegistrationInformationDialogControls.OrganizationStaticControl
        {
            get
            {
                if ((m_cachedOrganizationStaticControl == null))
                {
                    m_cachedOrganizationStaticControl = new StaticControl(this, ControlIDs.OrganizationStaticControl);
                }
                return m_cachedOrganizationStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IRegistrationInformationDialogControls.SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox
        {
            get
            {
                if ((m_cachedSetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox == null))
                {
                    m_cachedSetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox = new TextBox(this, ControlIDs.SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox);
                }
                return m_cachedSetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IRegistrationInformationDialogControls.InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox
        {
            get
            {
                if ((m_cachedInTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox == null))
                {
                    m_cachedInTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox = new TextBox(this, ControlIDs.InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox);
                }
                return m_cachedInTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasTextBox;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _TextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IRegistrationInformationDialogControls._TextBox
        {
            get
            {
                if ((m_cached_TextBox == null))
                {
                    m_cached_TextBox = new TextBox(this, ControlIDs._TextBox);
                }
                return m_cached_TextBox;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _TextBox2 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IRegistrationInformationDialogControls._TextBox2
        {
            get
            {
                if ((m_cached_TextBox2 == null))
                {
                    m_cached_TextBox2 = new TextBox(this, ControlIDs._TextBox2);
                }
                return m_cached_TextBox2;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _TextBox3 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IRegistrationInformationDialogControls._TextBox3
        {
            get
            {
                if ((m_cached_TextBox3 == null))
                {
                    m_cached_TextBox3 = new TextBox(this, ControlIDs._TextBox3);
                }
                return m_cached_TextBox3;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _TextBox4 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IRegistrationInformationDialogControls._TextBox4
        {
            get
            {
                if ((m_cached_TextBox4 == null))
                {
                    m_cached_TextBox4 = new TextBox(this, ControlIDs._TextBox4);
                }
                return m_cached_TextBox4;
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
        Button IRegistrationInformationDialogControls.NextButton
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
        Button IRegistrationInformationDialogControls.BackButton
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
        Button IRegistrationInformationDialogControls.CancelButton
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
        ///  Exposes access to the PleaseEnterYourNameAndOrganizationStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IRegistrationInformationDialogControls.PleaseEnterYourNameAndOrganizationStaticControl
        {
            get
            {
                if ((m_cachedPleaseEnterYourNameAndOrganizationStaticControl == null))
                {
                    m_cachedPleaseEnterYourNameAndOrganizationStaticControl = new StaticControl(this, ControlIDs.PleaseEnterYourNameAndOrganizationStaticControl);
                }
                return m_cachedPleaseEnterYourNameAndOrganizationStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IRegistrationInformationDialogControls.InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasStaticControl
        {
            get
            {
                if ((m_cachedInTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasStaticControl == null))
                {
                    m_cachedInTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasStaticControl = new StaticControl(this, ControlIDs.InTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasStaticControl);
                }
                return m_cachedInTheBoxesBelowTypeYour25characterCDKeyYoullFindThisNumberOnTheYellowStickerOnTheBackOfTheCDJewelCasStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _StaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IRegistrationInformationDialogControls._StaticControl
        {
            get
            {
                if ((m_cached_StaticControl == null))
                {
                    m_cached_StaticControl = new StaticControl(this, ControlIDs._StaticControl);
                }
                return m_cached_StaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _StaticControl2 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IRegistrationInformationDialogControls._StaticControl2
        {
            get
            {
                if ((m_cached_StaticControl2 == null))
                {
                    m_cached_StaticControl2 = new StaticControl(this, ControlIDs._StaticControl2);
                }
                return m_cached_StaticControl2;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _StaticControl3 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IRegistrationInformationDialogControls._StaticControl3
        {
            get
            {
                if ((m_cached_StaticControl3 == null))
                {
                    m_cached_StaticControl3 = new StaticControl(this, ControlIDs._StaticControl3);
                }
                return m_cached_StaticControl3;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the _StaticControl4 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IRegistrationInformationDialogControls._StaticControl4
        {
            get
            {
                if ((m_cached_StaticControl4 == null))
                {
                    m_cached_StaticControl4 = new StaticControl(this, ControlIDs._StaticControl4);
                }
                return m_cached_StaticControl4;
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

