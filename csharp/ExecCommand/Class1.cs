using System;
using System.Diagnostics;

namespace ExecCommand
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				Process notepad = new Process();
				notepad.StartInfo.FileName = @"c:\windows\notepad";
//				notepad.StartInfo.Arguments = @"\\acstools\toolbox\MOMBvtSetup\readme.txt";
				notepad.StartInfo.UseShellExecute = true;
				notepad.StartInfo.WorkingDirectory = @"C:\";
				notepad.Start();
				notepad.WaitForExit;
			}
			catch(System.ComponentModel.Win32Exception)
			{
				Console.WriteLine("Caught exception, executable file not found.");
				Debug.Write("[MyProgram] - Caught exception, executable file not found.");
			}
		}
	}
}

