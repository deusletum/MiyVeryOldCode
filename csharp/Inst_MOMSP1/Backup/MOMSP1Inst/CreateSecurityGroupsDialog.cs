namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;
    
    public class CreateSecurityGroupsDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public CreateSecurityGroupsDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "ICreateSecurityGroupsDialogControls interface definition"
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface ICreateSecurityGroupsDialogControls
    {
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl MicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperationStaticControl  {get;}
        StaticControl CAMAccountStaticControl  {get;}
        StaticControl CurrentUserAccountStaticControl  {get;}
        StaticControl smxdeangjStaticControl  {get;}
        StaticControl SMXdeangjStaticControl  {get;}
    }
#endregion

    
    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: CreateSecurityGroupsDialog
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
    internal class CreateSecurityGroupsDialog : Dialog, ICreateSecurityGroupsDialogControls
    {

#region "Strings"
        internal class Strings
        {
            public const string DialogTitle = "Microsoft Operations Manager Setup - Create Security Groups";
            public const string Next = "&Next >";
            public const string Back = "< &Back";
            public const string Cancel = "Cancel";
            public const string MicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperation = "Microsoft Operations Manager uses Windows Local Groups to determine who has acces" +
                "s to different Microsoft Operations Manager functions. Setup creates these group" +
                "s and adds \"SMX\\Domain Admins\" group if they do not exist. You may specify acces" +
                "s to Operation";
            public const string CAMAccount = "CAM account:";
            public const string CurrentUserAccount = "Current User account:";
            public const string smxdeangj = "smx\\deangj";
            public const string SMXdeangj = "SMX\\deangj";
        }
#endregion

#region "Control IDs"
        
        internal class ControlIDs
        {
            public const int NextButton = 3;
            public const int BackButton = 4;
            public const int CancelButton = 5;
            public const int MicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperationStaticControl = 7;
            public const int CAMAccountStaticControl = 8;
            public const int CurrentUserAccountStaticControl = 9;
            public const int smxdeangjStaticControl = 10;
            public const int SMXdeangjStaticControl = 11;
        }
#endregion

#region "Member Variables"
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedMicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperationStaticControl;
        protected StaticControl m_cachedCAMAccountStaticControl;
        protected StaticControl m_cachedCurrentUserAccountStaticControl;
        protected StaticControl m_cachedsmxdeangjStaticControl;
        protected StaticControl m_cachedSMXdeangjStaticControl;
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
        internal CreateSecurityGroupsDialog(App app) : 
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
                throw new CreateSecurityGroupsDialogNotFoundException("Create Security Groups Dialog not found");
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
        internal virtual ICreateSecurityGroupsDialogControls Controls
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
        Button ICreateSecurityGroupsDialogControls.NextButton
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
        Button ICreateSecurityGroupsDialogControls.BackButton
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
        Button ICreateSecurityGroupsDialogControls.CancelButton
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
        ///  Exposes access to the MicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperationStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ICreateSecurityGroupsDialogControls.MicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperationStaticControl
        {
            get
            {
                if ((m_cachedMicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperationStaticControl == null))
                {
                    m_cachedMicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperationStaticControl = new StaticControl(this, ControlIDs.MicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperationStaticControl);
                }
                return m_cachedMicrosoftOperationsManagerUsesWindowsLocalGroupsToDetermineWhoHasAccessToDifferentMicrosoftOperationStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the CAMAccountStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ICreateSecurityGroupsDialogControls.CAMAccountStaticControl
        {
            get
            {
                if ((m_cachedCAMAccountStaticControl == null))
                {
                    m_cachedCAMAccountStaticControl = new StaticControl(this, ControlIDs.CAMAccountStaticControl);
                }
                return m_cachedCAMAccountStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the CurrentUserAccountStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ICreateSecurityGroupsDialogControls.CurrentUserAccountStaticControl
        {
            get
            {
                if ((m_cachedCurrentUserAccountStaticControl == null))
                {
                    m_cachedCurrentUserAccountStaticControl = new StaticControl(this, ControlIDs.CurrentUserAccountStaticControl);
                }
                return m_cachedCurrentUserAccountStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the smxdeangjStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ICreateSecurityGroupsDialogControls.smxdeangjStaticControl
        {
            get
            {
                if ((m_cachedsmxdeangjStaticControl == null))
                {
                    m_cachedsmxdeangjStaticControl = new StaticControl(this, ControlIDs.smxdeangjStaticControl);
                }
                return m_cachedsmxdeangjStaticControl;
            }
        }

        
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the SMXdeangjStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 7/22/2003 - Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ICreateSecurityGroupsDialogControls.SMXdeangjStaticControl
        {
            get
            {
                if ((m_cachedSMXdeangjStaticControl == null))
                {
                    m_cachedSMXdeangjStaticControl = new StaticControl(this, ControlIDs.SMXdeangjStaticControl);
                }
                return m_cachedSMXdeangjStaticControl;
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

