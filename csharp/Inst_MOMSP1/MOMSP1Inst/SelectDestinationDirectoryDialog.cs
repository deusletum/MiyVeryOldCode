namespace Microsoft.MOM.Test.Operations.MOMSP1Inst.MAUIDialogs
{
    using Maui.Core;
    using Maui.Core.WinControls;
    using Maui.Core.Utilities;
    using System.ComponentModel;

    public class SelectDestinationDirectoryDialogNotFoundException : Window.Exceptions.WindowNotFoundException
    {
        public SelectDestinationDirectoryDialogNotFoundException(string message): base(message)
        {
        }
    }

#region "ISelectDestinationDirectoryDialogControls interface definition"
    
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface ISelectDestinationDirectoryDialogControls
    {
        TextBox BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox  {get;}
        
        // TODO: Rename this interface method.  Intuitive name not found by Class Maker Tool.
        ListBox ListBox0  {get;}
        
        // TODO: Rename this interface method.  Intuitive name not found by Class Maker Tool.
        EditComboBox EditComboBox1  {get;}
        Button OKButton  {get;}
        Button CancelButton  {get;}
        StaticControl BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesStaticControl  {get;}
    }

#endregion

    /// -----------------------------------------------------------------------------
    /// Project		: Maui
    /// Class		: SelectDestinationDirectoryDialog
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
    internal class SelectDestinationDirectoryDialog : Dialog, ISelectDestinationDirectoryDialogControls
    {


#region "Strings"
        // TODO: Remove unused definitions.
        internal class Strings
        {
            internal const string DialogTitle = "Microsoft Operations Manager Setup - Select Destination Directory";
            internal const string ListBox0 = "C:\\Program Files";
            internal const string OK = "OK";
            internal const string Cancel = "Cancel";
            internal const string BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFiles = "Browse for Installation Folder:\r\n\r\nEnter the location where Setup will install Mi" +
                "crosoft Operations Manager files.";
        }

#endregion

#region "Control IDs"
        internal class ControlIDs
        {
            internal const int BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox = 0x3;
            
            // TODO: You may wish to rename this ID.  Intuitive name not found by Dialog Class Maker
            internal const int ListBox0 = 0x4;
            
            // TODO: You may wish to rename this ID.  Intuitive name not found by Dialog Class Maker
            internal const int EditComboBox1 = 0x5;
            internal const int OKButton = 0x7;
            internal const int CancelButton = 0x8;
            internal const int BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesStaticControl = 0x9;
        }

#endregion

#region "Member Variables"
        protected TextBox m_cachedBrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox;
        protected ListBox m_cachedListBox0;
        protected EditComboBox m_cachedEditComboBox1;
        protected Button m_cachedOKButton;
        protected Button m_cachedCancelButton;
        protected StaticControl m_cachedBrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesStaticControl;

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
        internal SelectDestinationDirectoryDialog(App app) : 
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
                tempWindow = new Window(Strings.DialogTitle, StringMatchSyntax.ExactMatch, WindowClassNames.Alert, StringMatchSyntax.ExactMatch, app, 3000);
            }
            catch (Exceptions.WindowNotFoundException)
            {
                throw new SelectDestinationDirectoryDialogNotFoundException("Select Destination Directory Dialog not found");
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
        internal virtual ISelectDestinationDirectoryDialogControls Controls
        {
            get
            {
                return this;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFiles
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesText
        {
            get
            {
                return Controls.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox.Text;
            }
            set
            {
                Controls.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox.Text = value;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to set/get the text in control EditComboBox1
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual string EditComboBox1Text
        {
            get
            {
                return Controls.EditComboBox1.Text;
            }
            set
            {
                Controls.EditComboBox1.Text = value;
            }
        }


        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        TextBox ISelectDestinationDirectoryDialogControls.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox
        {
            get
            {
                if ((m_cachedBrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox == null))
                {
                    m_cachedBrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox = new TextBox(this, ControlIDs.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox);
                }
                return m_cachedBrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesTextBox;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the ListBox0 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        ListBox ISelectDestinationDirectoryDialogControls.ListBox0
        {
            get
            {
                if ((m_cachedListBox0 == null))
                {
                    m_cachedListBox0 = new ListBox(this, ControlIDs.ListBox0);
                }
                return m_cachedListBox0;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the EditComboBox1 control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        EditComboBox ISelectDestinationDirectoryDialogControls.EditComboBox1
        {
            get
            {
                if ((m_cachedEditComboBox1 == null))
                {
                    m_cachedEditComboBox1 = new EditComboBox(this, ControlIDs.EditComboBox1);
                }
                return m_cachedEditComboBox1;
            }
        }

        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Exposes access to the OKButton control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        Button ISelectDestinationDirectoryDialogControls.OKButton
        {
            get
            {
                if ((m_cachedOKButton == null))
                {
                    m_cachedOKButton = new Button(this, ControlIDs.OKButton);
                }
                return m_cachedOKButton;
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
        Button ISelectDestinationDirectoryDialogControls.CancelButton
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
        ///  Exposes access to the BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesStaticControl control
        ///  </summary>
        ///  <value></value>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        StaticControl ISelectDestinationDirectoryDialogControls.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesStaticControl
        {
            get
            {
                if ((m_cachedBrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesStaticControl == null))
                {
                    m_cachedBrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesStaticControl = new StaticControl(this, ControlIDs.BrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesStaticControl);
                }
                return m_cachedBrowseForInstallationFolderEnterTheLocationWhereSetupWillInstallMicrosoftOperationsManagerFilesStaticControl;
            }
        }

#endregion

#region "Methods"
        /// -----------------------------------------------------------------------------
        ///  <summary>
        ///  Routine to click on button OK
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        /// 	[deangj] 9/11/2003 Created
        ///  </history>
        /// -----------------------------------------------------------------------------
        internal virtual void ClickOK()
        {
            Controls.OKButton.Click();
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


