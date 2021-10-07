namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class LicenseAgreementDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public LicenseAgreementDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "ILicenseAgreementDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface ILicenseAgreementDialogControls
    {
        Button NextButton  {get;}
        Button CancelButton  {get;}
        TextBox SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox  {get;}
        Button BackButton  {get;}
        StaticControl PleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbeStaticControl  {get;}
        StaticControl NameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7MStaticControl  {get;}
        RadioButton IacceptTheTermsInTheLicenseAgreementRadioButton  {get;}
        RadioButton IdoNotAcceptTheTermsInTheLicenseAgreementRadioButton  {get;}
        StaticControl MicrosoftOperationsManager2000LicenseAndSupportInformationStaticControl  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: LicenseAgreementDialog
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
    internal class LicenseAgreementDialog : Dialog, ILicenseAgreementDialogControls
    {

#region "Strings"
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - License Agreement";
            internal const string Next = "&Next >";
            internal const string Cancel = "Cancel";
            internal const string Back = "< &Back";
            internal const string PleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbe = "Please write down your Product ID number. If you need to call Microsoft Technical" +
                " Support, you will be asked for this number.";
            internal const string NameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7M = "Name:            Server Manager Team\r\nOrganization:  Microsoft\r\nProduct ID:     R" +
                "7MPM-R36DT-F38FC-RPPCX-XJG7M";
            internal const string IacceptTheTermsInTheLicenseAgreement = "I &accept the terms in the license agreement";
            internal const string IdoNotAcceptTheTermsInTheLicenseAgreement = "I &do not accept the terms in the license agreement";
            internal const string MicrosoftOperationsManager2000LicenseAndSupportInformation = "Microsoft Operations Manager 2000 License and Support Information.";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            internal const int NextButton = 3;
            internal const int CancelButton = 4;
            internal const int SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox = 6;
            internal const int BackButton = 7;
            internal const int PleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbeStaticControl = 8;
            internal const int NameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7MStaticControl = 9;
            internal const int IacceptTheTermsInTheLicenseAgreementRadioButton = 10;
            internal const int IdoNotAcceptTheTermsInTheLicenseAgreementRadioButton = 11;
            internal const int MicrosoftOperationsManager2000LicenseAndSupportInformationStaticControl = 12;
        }
#endregion

#region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedCancelButton;
        protected TextBox m_cachedSetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox;
        protected Button m_cachedBackButton;
        protected StaticControl m_cachedPleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbeStaticControl;
        protected StaticControl m_cachedNameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7MStaticControl;
        protected RadioButton m_cachedIacceptTheTermsInTheLicenseAgreementRadioButton;
        protected RadioButton m_cachedIdoNotAcceptTheTermsInTheLicenseAgreementRadioButton;
        protected StaticControl m_cachedMicrosoftOperationsManager2000LicenseAndSupportInformationStaticControl;
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
        internal LicenseAgreementDialog(App app) : 
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
                throw new LicenseAgreementDialogNotFoundException("License Agreement Dialog not found");
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
        internal virtual ILicenseAgreementDialogControls Controls
        {
            get
            {
                return this;
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
        ///  Exposes access to the NextButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button ILicenseAgreementDialogControls.NextButton
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
        Button ILicenseAgreementDialogControls.CancelButton
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
        ///  Exposes access to the SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox ILicenseAgreementDialogControls.SetupWillInstallOneOrMoreComponentsForMicrosoftOperationsManager2000OnThisComputerToContinueSetupCloTextBox
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
        ///  Exposes access to the BackButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button ILicenseAgreementDialogControls.BackButton
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
        ///  Exposes access to the PleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbeStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ILicenseAgreementDialogControls.PleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbeStaticControl
        {
            get
            {
                if ((m_cachedPleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbeStaticControl == null))
                {
                    m_cachedPleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbeStaticControl = new StaticControl(this, ControlIDs.PleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbeStaticControl);
                }
                return m_cachedPleaseWriteDownYourProductIDNumberIfYouNeedToCallMicrosoftTechnicalSupportYouWillBeAskedForThisNumbeStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the NameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7MStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ILicenseAgreementDialogControls.NameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7MStaticControl
        {
            get
            {
                if ((m_cachedNameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7MStaticControl == null))
                {
                    m_cachedNameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7MStaticControl = new StaticControl(this, ControlIDs.NameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7MStaticControl);
                }
                return m_cachedNameServerManagerTeamOrganizationMicrosoftProductIDR7MPMR36DTF38FCRPPCXXJG7MStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the IacceptTheTermsInTheLicenseAgreementRadioButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        RadioButton ILicenseAgreementDialogControls.IacceptTheTermsInTheLicenseAgreementRadioButton
        {
            get
            {
                if ((m_cachedIacceptTheTermsInTheLicenseAgreementRadioButton == null))
                {
                    m_cachedIacceptTheTermsInTheLicenseAgreementRadioButton = new RadioButton(this, ControlIDs.IacceptTheTermsInTheLicenseAgreementRadioButton);
                }
                return m_cachedIacceptTheTermsInTheLicenseAgreementRadioButton;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the IdoNotAcceptTheTermsInTheLicenseAgreementRadioButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        RadioButton ILicenseAgreementDialogControls.IdoNotAcceptTheTermsInTheLicenseAgreementRadioButton
        {
            get
            {
                if ((m_cachedIdoNotAcceptTheTermsInTheLicenseAgreementRadioButton == null))
                {
                    m_cachedIdoNotAcceptTheTermsInTheLicenseAgreementRadioButton = new RadioButton(this, ControlIDs.IdoNotAcceptTheTermsInTheLicenseAgreementRadioButton);
                }
                return m_cachedIdoNotAcceptTheTermsInTheLicenseAgreementRadioButton;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the MicrosoftOperationsManager2000LicenseAndSupportInformationStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ILicenseAgreementDialogControls.MicrosoftOperationsManager2000LicenseAndSupportInformationStaticControl
        {
            get
            {
                if ((m_cachedMicrosoftOperationsManager2000LicenseAndSupportInformationStaticControl == null))
                {
                    m_cachedMicrosoftOperationsManager2000LicenseAndSupportInformationStaticControl = new StaticControl(this, ControlIDs.MicrosoftOperationsManager2000LicenseAndSupportInformationStaticControl);
                }
                return m_cachedMicrosoftOperationsManager2000LicenseAndSupportInformationStaticControl;
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
        ///  Routine to click on button IacceptTheTermsInTheLicenseAgreement
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickIacceptTheTermsInTheLicenseAgreement()
        {
            Controls.IacceptTheTermsInTheLicenseAgreementRadioButton.Click();
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button IdoNotAcceptTheTermsInTheLicenseAgreement
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickIdoNotAcceptTheTermsInTheLicenseAgreement()
        {
            Controls.IdoNotAcceptTheTermsInTheLicenseAgreementRadioButton.Click();
        }

#endregion

    }
}

