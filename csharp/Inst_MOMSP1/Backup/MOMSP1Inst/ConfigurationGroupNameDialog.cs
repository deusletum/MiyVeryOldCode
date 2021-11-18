namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{

    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class ConfigurationGroupNameDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public ConfigurationGroupNameDialogNotFoundException(string message): base(message)
        {
        }
    }

    #region "IConfigurationGroupNameDialogControls interface definition"


    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IConfigurationGroupNameDialogControls
    {
        TextBox MBTextBox  {get;}
        Button NextButton  {get;}
        Button BackButton  {get;}
        Button CancelButton  {get;}
        StaticControl NameStaticControl  {get;}
        StaticControl TypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheSStaticControl  {get;}
    }

    #endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: ConfigurationGroupNameDialog
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
    internal class ConfigurationGroupNameDialog : Dialog, IConfigurationGroupNameDialogControls
    {


        #region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Configuration Group Name";
            internal const string Next = "&Next >";
            internal const string Back = "< &Back";
            internal const string Cancel = "Cancel";
            internal const string Name = "N&ame";
            internal const string TypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheS = "Type a unique name for this configuration group. A configuration group is compris" +
                "ed of the components you selected, the single database they use, and the rules a" +
                "pplying to that use.\r\n\r\nAfter Setup is finished, you will not be able to change " +
                "the name of th";
        }

        #endregion

        #region "Control IDs"
        internal class ControlIDs
        {
            internal const int MBTextBox = 0x3;
            internal const int NextButton = 0x4;
            internal const int BackButton = 0x5;
            internal const int CancelButton = 0x6;
            internal const int NameStaticControl = 0x8;
            internal const int TypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheSStaticControl = 0x9;
        }

        #endregion

        #region "Member Variables"
        protected TextBox m_cachedMBTextBox;
        protected Button m_cachedNextButton;
        protected Button m_cachedBackButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedNameStaticControl;
        protected StaticControl m_cachedTypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheSStaticControl;

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
        internal ConfigurationGroupNameDialog(App app) : 
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
                throw new ConfigurationGroupNameDialogNotFoundException("Configuration Group Name Dialog not found");
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
        internal virtual IConfigurationGroupNameDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control MB
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string MBText
        {
            get
            {
                return Controls.MBTextBox.Text;
            }
            set
            {
                Controls.MBTextBox.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the MBTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox IConfigurationGroupNameDialogControls.MBTextBox
        {
            get
            {
                if ((m_cachedMBTextBox == null))
                {
                    m_cachedMBTextBox = new TextBox(this, ControlIDs.MBTextBox);
                }
                return m_cachedMBTextBox;
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
        Button IConfigurationGroupNameDialogControls.NextButton
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
        Button IConfigurationGroupNameDialogControls.BackButton
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
        Button IConfigurationGroupNameDialogControls.CancelButton
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
        ///  Exposes access to the NameStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IConfigurationGroupNameDialogControls.NameStaticControl
        {
            get
            {
                if ((m_cachedNameStaticControl == null))
                {
                    m_cachedNameStaticControl = new StaticControl(this, ControlIDs.NameStaticControl);
                }
                return m_cachedNameStaticControl;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the TypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheSStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/20/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl IConfigurationGroupNameDialogControls.TypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheSStaticControl
        {
            get
            {
                if ((m_cachedTypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheSStaticControl == null))
                {
                    m_cachedTypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheSStaticControl = new StaticControl(this, ControlIDs.TypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheSStaticControl);
                }
                return m_cachedTypeAUniqueNameForThisConfigurationGroupAConfigurationGroupIsComprisedOfTheComponentsYouSelectedTheSStaticControl;
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


