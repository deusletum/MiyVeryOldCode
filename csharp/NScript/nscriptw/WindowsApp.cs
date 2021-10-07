using System;
using System.Windows.Forms;

namespace NScript
{
	/// <summary>
	/// Summary description for WindowsApp.
	/// </summary>
	public class WindowsApp : BaseApp
	{
		private NotifyIcon icon;
		private Timer timer;
		private System.Drawing.Icon [] icons;
		private int currentIconIndex = 0;

		public WindowsApp()
		{
		}

		protected override void ShowErrorMessage(string message)
		{
			MessageBox.Show(message, EntryAssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		protected override void ExecutionLoop(System.IAsyncResult result)
		{
			icon = new NotifyIcon();
			timer = new Timer();
			icons = new System.Drawing.Icon[4];
			
			for(int i = 0; i < 4; i++)
			{
				icons[i] = (System.Drawing.Icon)GetResourceObject("AnimationIcon" + (i + 1).ToString());
			}

			icon.Icon = icons[currentIconIndex];
			icon.Text = GetResourceString("IconTip");
			icon.Visible = true;
			icon.DoubleClick += new EventHandler(this.OnIconDoubleClick);

			timer.Tick += new EventHandler(this.OnTimerTick);
			timer.Interval = 100;
			timer.Start();

			Application.Run();
			
			icon.Dispose();
			timer.Dispose();
		}

		protected override void TerminateExecutionLoop()
		{
			Application.Exit();
		}
		
		private void OnIconDoubleClick(object sender, EventArgs e)
		{
			if (MessageBox.Show(String.Format(GetResourceString("CancelExecution"), ""), EntryAssemblyName, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				TerminateExecution();
			}
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			//Change the icon
			currentIconIndex++;
			
			if (currentIconIndex == 4)
				currentIconIndex = 0;

			icon.Icon = icons[currentIconIndex];
			icon.Visible = true;
		}

		[STAThread]
		public static void Main(string[] args)
		{
			new WindowsApp().Run(args);
		}
	}
}
