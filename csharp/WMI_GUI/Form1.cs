using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Management;

namespace WMI_GUI
{
	public class WMIstuff
	{
		public void StartWMI(System.Windows.Forms.RichTextBox rtxtbox)
		{
			ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher
				("Select * From Win32_Process");

			ArrayList lines = new ArrayList();
			string txt = "";
			
			foreach (ManagementObject Process in WMISearcher.Get())
			{
				lines.Add("Process Name"+ "\t" + Process["Name"] + "\t" 
					+ "Process ID" + "\t" + Process["ProcessID"]);
			}

			foreach (string line in lines)
			{
				txt = txt + line + "\r\n";
			}

			rtxtbox.Text = txt;
		}
		public void ProcessName(System.Windows.Forms.RichTextBox rtxtbox)
		{
			ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher
				("Select * From Win32_Process");

			ArrayList lines = new ArrayList();
			string txt = "";
			
			foreach (ManagementObject Process in WMISearcher.Get())
			{
				lines.Add(Process["Name"]);
			}

			foreach (string line in lines)
			{
				txt = txt + line + "\r\n";
			}

			rtxtbox.Text = txt;
		}

		public void ProcessID(System.Windows.Forms.RichTextBox rtxtbox)
		{

		}

		public string GetName()
		{
			try
			{
				string name = "";

				ManagementObjectSearcher CompSys = new ManagementObjectSearcher
					("Select * From Win32_ComputerSystem");

				foreach (ManagementObject Comp in CompSys.Get())
				{
					name = (string)Comp["Name"];
				}
				
				return name;

			}	
			catch (IndexOutOfRangeException IE)
			{
				//MessageBox Msgbox = MessageBox.Show();
				MessageBox.Show(IE.ToString());
				return "Error";
			}
		}
	}
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox rtxtMain;
		private System.Windows.Forms.RichTextBox rtxtPID;
		private System.Windows.Forms.RichTextBox rtxtPName;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.rtxtMain = new System.Windows.Forms.RichTextBox();
			this.rtxtPID = new System.Windows.Forms.RichTextBox();
			this.rtxtPName = new System.Windows.Forms.RichTextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// rtxtMain
			// 
			this.rtxtMain.Name = "rtxtMain";
			this.rtxtMain.Size = new System.Drawing.Size(264, 472);
			this.rtxtMain.TabIndex = 0;
			this.rtxtMain.Text = "";
			this.rtxtMain.WordWrap = false;
			// 
			// rtxtPID
			// 
			this.rtxtPID.Location = new System.Drawing.Point(384, 40);
			this.rtxtPID.Name = "rtxtPID";
			this.rtxtPID.TabIndex = 1;
			this.rtxtPID.Text = "";
			// 
			// rtxtPName
			// 
			this.rtxtPName.Location = new System.Drawing.Point(280, 40);
			this.rtxtPName.Name = "rtxtPName";
			this.rtxtPName.TabIndex = 2;
			this.rtxtPName.Text = "";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(280, 8);
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = "Process Name";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(384, 8);
			this.textBox2.Name = "textBox2";
			this.textBox2.TabIndex = 4;
			this.textBox2.Text = "Process ID";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(832, 486);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBox2,
																		  this.textBox1,
																		  this.rtxtPName,
																		  this.rtxtPID,
																		  this.rtxtMain});
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			WMIstuff WMI = new WMIstuff();
			WMI.StartWMI(this.rtxtMain);
			WMI.ProcessName(this.rtxtPName);
			//MessageBox.Show("Your Computer Name is " + WMI.GetName());
		}
	}
}
