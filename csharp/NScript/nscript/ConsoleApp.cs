using System;
using System.Runtime.InteropServices;
using NScript;

namespace NScript
{
	/// <summary>
	/// Summary description for ConsoleApp.
	/// </summary>
	public class ConsoleApp : BaseApp
	{
		public ConsoleApp()
		{
		}
		
		delegate bool CtrlHandlerRoutine(int eventType);

		[DllImport("kernel32.dll", SetLastError=true)]
        extern static bool SetConsoleCtrlHandler(CtrlHandlerRoutine handler);		
		
		private bool CtrlHandler(int eventType)
		{
			Console.Write(BaseApp.GetResourceString("CancelExecution"), "(Y/N)");
		
			if (String.Compare(Console.ReadLine(), "Y", true) == 0)
			{
				TerminateExecution();
			}
		
			return true;
		}

		protected override void TerminateExecutionLoop()
		{
			
		}

		protected override void ShowErrorMessage(string message)
		{
			Console.Error.WriteLine(message);
		}

		protected override void ExecutionLoop(System.IAsyncResult result)
		{
			result.AsyncWaitHandle.WaitOne();
		}

		public static void Main(string[] args)
		{
			ConsoleApp app = new ConsoleApp();
			CtrlHandlerRoutine handler = new CtrlHandlerRoutine(app.CtrlHandler);
			
			//Install a console ctrl handler so that we may unload the exceution domain
			//If the user wants to cancel by pressing Ctrl+C
			SetConsoleCtrlHandler(handler);
			app.Run(args);
			
			GC.KeepAlive(handler);
		}
	}
}
