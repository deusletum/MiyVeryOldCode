using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Web.Mail;

namespace TestApp
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmEmail : System.Windows.Forms.Form
    {
        private System.Windows.Forms.RichTextBox rtxtBody;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.TextBox txtSMTPSrv;
        private System.Windows.Forms.Label lblsmtpSrv;
        private System.Windows.Forms.Label lblRemail;
        private System.Windows.Forms.TextBox txtRemail;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblBody;
        private System.Windows.Forms.Button cmdFile;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.TextBox txtFrom;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmEmail()
        {
            InitializeComponent();
        }

        public void send()
        {
            try
            {
                MailMessage Message = new MailMessage();

                Message.To = this.txtRemail.Text;
                //Message.To = "smarthost.redmond.corp.microsoft.com";
                Message.From = this.txtFrom.Text;
                Message.Subject = this.txtSubject.Text;
                Message.Body = this.rtxtBody.Text;
                try
                {
                    SmtpMail.SmtpServer = this.txtSubject.Text;
                    SmtpMail.Send(Message);
                }
                catch (System.Web.HttpException ehttp)
                {
                    this.rtxtBody.Text = ehttp.Message + "\r\n" + ehttp.ToString();
                }
            }
            catch (System.Exception e)
            {
                this.rtxtBody.Text = e.Message;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmEmail));
            this.rtxtBody = new System.Windows.Forms.RichTextBox();
            this.cmdSend = new System.Windows.Forms.Button();
            this.txtSMTPSrv = new System.Windows.Forms.TextBox();
            this.lblsmtpSrv = new System.Windows.Forms.Label();
            this.lblRemail = new System.Windows.Forms.Label();
            this.txtRemail = new System.Windows.Forms.TextBox();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.lblSubject = new System.Windows.Forms.Label();
            this.cmdFile = new System.Windows.Forms.Button();
            this.lblBody = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // rtxtBody
            //
            this.rtxtBody.Location = new System.Drawing.Point(128, 160);
            this.rtxtBody.Name = "rtxtBody";
            this.rtxtBody.Size = new System.Drawing.Size(256, 208);
            this.rtxtBody.TabIndex = 0;
            this.rtxtBody.Text = "";
            //
            // cmdSend
            //
            this.cmdSend.Location = new System.Drawing.Point(264, 16);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.TabIndex = 1;
            this.cmdSend.Text = "&Send";
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            //
            // txtSMTPSrv
            //
            this.txtSMTPSrv.Location = new System.Drawing.Point(128, 16);
            this.txtSMTPSrv.Name = "txtSMTPSrv";
            this.txtSMTPSrv.TabIndex = 2;
            this.txtSMTPSrv.Text = "";
            //
            // lblsmtpSrv
            //
            this.lblsmtpSrv.Location = new System.Drawing.Point(8, 16);
            this.lblsmtpSrv.Name = "lblsmtpSrv";
            this.lblsmtpSrv.TabIndex = 3;
            this.lblsmtpSrv.Text = "SMTP Server";
            //
            // lblRemail
            //
            this.lblRemail.Location = new System.Drawing.Point(8, 56);
            this.lblRemail.Name = "lblRemail";
            this.lblRemail.TabIndex = 4;
            this.lblRemail.Text = "Receive Email @";
            //
            // txtRemail
            //
            this.txtRemail.Location = new System.Drawing.Point(128, 56);
            this.txtRemail.Name = "txtRemail";
            this.txtRemail.TabIndex = 5;
            this.txtRemail.Text = "";
            //
            // txtSubject
            //
            this.txtSubject.AutoSize = false;
            this.txtSubject.Location = new System.Drawing.Point(128, 128);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(256, 20);
            this.txtSubject.TabIndex = 6;
            this.txtSubject.Text = "";
            //
            // lblSubject
            //
            this.lblSubject.Location = new System.Drawing.Point(8, 128);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.TabIndex = 7;
            this.lblSubject.Text = "Subject";
            //
            // cmdFile
            //
            this.cmdFile.Location = new System.Drawing.Point(264, 56);
            this.cmdFile.Name = "cmdFile";
            this.cmdFile.TabIndex = 8;
            this.cmdFile.Text = "&Attach File";
            //
            // lblBody
            //
            this.lblBody.Location = new System.Drawing.Point(8, 168);
            this.lblBody.Name = "lblBody";
            this.lblBody.TabIndex = 9;
            this.lblBody.Text = "Body";
            //
            // lblFrom
            //
            this.lblFrom.Location = new System.Drawing.Point(8, 88);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "Sender Email @";
            //
            // txtFrom
            //
            this.txtFrom.Location = new System.Drawing.Point(128, 88);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.TabIndex = 11;
            this.txtFrom.Text = "";
            //
            // frmEmail
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(400, 382);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
            this.txtFrom,
            this.lblFrom,
            this.lblBody,
            this.cmdFile,
            this.lblSubject,
            this.txtSubject,
            this.txtRemail,
            this.lblRemail,
            this.lblsmtpSrv,
            this.txtSMTPSrv,
            this.cmdSend,
            this.rtxtBody});
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEmail";
            this.Text = "Simple Email";
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new frmEmail());
        }
        protected void cmdSend_Click(object sender, System.EventArgs e)
        {
            this.send();
        }
    }
}