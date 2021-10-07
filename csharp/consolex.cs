//////////////////////////////////////////////////////////
// 

// MSN Search
//
//  File:   ConsoleX.cs
//
//  Summary:  ConsoleX class - extends standard Console  with CRT _kbhit() and _getch() methods 
//            and provides colored console output.
//
//  Author: V.Pinich (VladiP)
//  10/23/2001
//	
//  11/02/2001  Special thanks to Keith Stutler and Yann Christensen for useful advices
//
//////////////////////////////////////////////////////////

/* usage example

namespace ConsoleXtest
{  
  	using System;
	using ConsoleX;

	class App
	{
		static void Main()
		{
			int   i = 1234;
			float f = 23.263f;
			char  c = ' ';

			ConsoleX.WriteLineX(CC.YELLOW, " ConsoleX output examples\n");

			ConsoleX.WriteLineX(" Simple output without {} brackets:   int: ", i, "  float: ", f);
			ConsoleX.WriteLineX(CC.BLUE, " Simple colored output: ", CC.GREEN, " int: ",i, "  float: ", f);

			ConsoleX.WriteX(CC.WHITE, "\n Formatted output: ", CC.GREEN, "f= ", f.ToString("000.0"));
			ConsoleX.WriteX("   Inversed default color: ", CC.INV, "i= ", i, " 0x", i.ToString("x") );

			ConsoleX.WriteX( CC.WHITE, "\n\n Default ", CC.GREEN, "background ", CC.RED, "output \n"); 
			ConsoleX.WriteX( CC.ABS, CC.B_WHITE, CC.BGND, " White   ", CC.RED, "background ", CC.BLUE, "output \n"); 
			ConsoleX.WriteX( CC.ABS, (byte)(CC.B_GRAY | CC.WHITE), "       white on gray       \n"); 
			ConsoleX.WriteX( CC.ABS, (byte)(CC.B_GREEN | CC.GREEN),"       green bar           \n\n"); 
		
			byte OldColor = ConsoleX.WriteX("Set global color:\n", CC.ABS, (byte)(CC.B_BLUE|CC.GRAY), CC.GLOBAL, " Same call\n");
			ConsoleX.WriteX(" New call \n");
			ConsoleX.WriteX(CC.WHITE," Restoring old console color: ", CC.ABS, OldColor, CC.GLOBAL, " here it is !!\n");

			ConsoleX.WriteLineX(CC.YELLOW, "\n _kbhit() + _getch() example, hit any key to loop, hit Esc to exit\n" );
		
			while (c != 27)
			{
				if (ConsoleX._kbhit())
				{
					c = ConsoleX._getch();
					ConsoleX.WriteLineX(CC.GREEN,"Key: ", c, "  code: ", CC.WHITE, ((int)c).ToString("d") );
				}

				System.Threading.Thread.Sleep(200);
			}
		}
	}
}
*/ 

using System;
using System.Runtime.InteropServices;  // Must have to gain access to Win32 API attributes.

namespace ConsoleX
{
	/// <summary>
	/// <p>class ConsoleX: Extends standard Console  with _kbhit() and _getch() methods and colored output.</p>
	/// 
	/// <p>uses MSVCR70.DLL Runtime library for _kbhit() and _getch()</p>
	/// <p>uses kernel32.dll and Win32 Console API for colored output</p>
	///  
	/// contacts: VladiP
	/// </summary>
	/// <example>
	/// <code>
	/// // write colored output for arg list without brackets {},  see comments in CC class  for color output details
	/// ConsoleX.WriteLineX(CC.BLUE, " Simple colored output: ", CC.GREEN, " int: ",i, "  float: ", f);
	/// ConsoleX.WriteX(CC.WHITE, "\n Formatted output: ", CC.GREEN, "f= ", f.ToString("000.0")); 
	/// 
	/// // _kbhit example
	/// while (27 != c)
	/// {
	///		if (ConsoleX._kbhit())
	///		{
	///			c = ConsoleX._getch();
	///			ConsoleX.WriteLineX(CC.GREEN,"Key: ", c, "  code: ", CC.WHITE, ((int)c).ToString("d") );
	///		}
	///	}
	/// </code>
	/// </example>
	/// 
	public class ConsoleX
	{
		const short STD_INPUT_HANDLE  = -10;
		const short STD_OUTPUT_HANDLE = -11;
		const short STD_ERROR_HANDLE  = -12;

		[DllImport("kernel32")]
		static extern int GetStdHandle(int hStdHandle);

		[DllImport("kernel32")]
		static extern int SetConsoleTextAttribute( int hConsole, ushort Attribute); 

		[DllImport("kernel32")]
		static extern int GetConsoleScreenBufferInfo( int hConsole, ref CONSOLE_SCREEN_BUFFER_INFO cbInfo ); 


		[DllImport("kernel32", CharSet=CharSet.Unicode, EntryPoint = "WriteConsoleW")]
		static extern int WriteConsole( int hConsole,
			String  pText,	int nTextLength, int nWritten, int nReserved);

		[DllImport("MSVCR70.DLL")]
		public static extern bool _kbhit();

		[DllImport("MSVCR70.DLL")]
		public static extern char _getch();

		private ConsoleX() {}		// to prevent from instantiate the class


		/// <summary>
		/// writes arg list to console, parameters with byte type will set color options for the next output items
		/// see comments in CC class  for color output details
		/// </summary>
		/// <param name="olist"> parameter list with optional byte type color modifiers </param>
		/// <returns> byte value - old default console color </returns>
		public static byte WriteX( params object[] olist  )
		{
			lock(typeof(ConsoleX))
			{
				CONSOLE_SCREEN_BUFFER_INFO cbInfo = new CONSOLE_SCREEN_BUFFER_INFO();		// Console information 
				int  hConsoleOut = GetStdHandle( STD_OUTPUT_HANDLE );					    // Handle to the console 

				GetConsoleScreenBufferInfo( hConsoleOut,  ref cbInfo );

				byte colorDEF  =  (byte) cbInfo.wAttributes; 
				byte colorLAST =  colorDEF;
				byte colorMODE =  CC.DEF;								// default color mode - related to default color
				bool SetGlobal =  false;								// indicates color restoring on return 

				foreach(object o in olist)
				{
					if (o is Byte)		// use byte value for color change -- was: if(o.GetType().Equals(typeof(System.Byte)))
					{
						byte colorNEW = (byte) o;			
						if( CC.ABS == colorNEW )			// use absolute color
						{
							colorMODE = CC.ABS;
							continue;
						}
						else if ( CC.BGND == colorNEW )		// use previous background
						{
							colorMODE = CC.BGND;
							continue;
						}
						else if ( CC.GLOBAL == colorNEW )	// use current color as global 
						{
							SetGlobal = true;
							continue;
						}
						else if ( CC.DEF == colorNEW )		// use default console color
						{
							colorMODE = CC.DEF;
							colorNEW  = colorDEF;
						}
						else if ( CC.INV == colorNEW )
						{
							if (CC.DEF == colorMODE)
								colorNEW  = (byte) (colorDEF ^ 0xFF);    // for default we invert default color
							else
								colorNEW  = (byte) (colorLAST ^ 0xFF);   // for absolute we invert last color
						}
						else if ( CC.DEF == colorMODE )
						{
							colorNEW  = (byte) ( (colorDEF & 0xF0) | (colorNEW & 0x0F) );
						}
						else if ( CC.BGND == colorMODE )
							colorNEW  = (byte) ( (colorLAST & 0xF0) | (colorNEW & 0x0F) );

						colorLAST =  colorNEW;
						SetConsoleTextAttribute(hConsoleOut, colorNEW );
					}
					else
						Console.Write("{0}", o);
				}

				if (!SetGlobal)
					SetConsoleTextAttribute(hConsoleOut, cbInfo.wAttributes);		// restore old color

				return (byte)cbInfo.wAttributes;									// return old default color
			}
		}


		/// <summary>
		/// WriteLineX: works as WriteX with new line at the end of parameter list
		/// </summary>
		/// <param name="olist"></param>
		/// <returns> byte value - old default console color </returns>
		public static byte WriteLineX( params object[] olist  )
		{
			lock( typeof(ConsoleX) )	
			{
				byte ret = WriteX( olist );
				Console.Write("\n");
			}
			return ret;
		}

	}


	/// <summary>
	/// class CC - contains color constants to be used with ConsoleX output methods.
	/// Thanks to Keith Stutler (KeithSt) -- he suggested to put all constants into public class
	/// </summary>
	public class CC
	{
		public const byte DEF    = 0;				// use default console foreground and background colors for output
		public const byte ABS    = 0x8;				// use absolute coloring - not related to curent or default background
		public const byte INV    = 0x80;			// use inverted console color for output
		public const byte BGND   = 0x88;			// use current background for next output
		public const byte GLOBAL = 0xff;			// use current color or last color for this call as global console color

		public const byte F_BLUE      = 0x1;		//  text color contains blue.
		public const byte F_GREEN     = 0x2;		//  text color contains green.
		public const byte F_RED       = 0x4;		//  text color contains red.
		public const byte F_INTENSITY = 0x8;		//  text color is intensified.
		public const byte B_BLUE      = 0x10;		//  background color contains blue.
		public const byte B_GREEN     = 0x20;		//  background color contains green.
		public const byte B_RED       = 0x40;		//  background color contains red.
		public const byte B_INTENSITY = 0x80;		//  background color is intensified.
		public const byte B_GRAY      = 0x70;		//  background gray color.
		public const byte B_WHITE     = 0xF0;		//  background white color.

		public const byte BLUE    = F_INTENSITY | F_BLUE;				// 0x9
		public const byte GREEN   = F_INTENSITY | F_GREEN;				// 0xA
		public const byte RED     = F_INTENSITY | F_RED;				// 0xC

		public const byte YELLOW  = F_INTENSITY | F_RED   | F_GREEN;	
		public const byte PINK    = F_INTENSITY | F_RED   | F_BLUE;	
		public const byte AQUA    = F_INTENSITY | F_GREEN | F_BLUE;	
		public const byte GRAY    = 0x7;
		public const byte WHITE   = 0xF;

		public const byte DARK_BLUE    = F_BLUE;		// 0x1
		public const byte DARK_GREEN   = F_GREEN;		// 0x2
		public const byte DARK_RED     = F_RED;			// 0x4

		public const byte INFO    = GREEN;				// console color for info msgs
		public const byte ERROR   = RED;				// console color for error msgs
		public const byte WARNING = YELLOW;				// console color for warning msgs

		private CC() {}		// to prevent from instantiate the class
	}

	/// <summary>
	/// Win32 API struct
	/// </summary>
	struct COORD 
	{ 
		public short X; 
		public short Y; 
	}

	/// <summary>
	/// Win32 API struct
	/// </summary>
	struct SMALL_RECT 
	{
		public short Left; 
		public short Top; 
		public short Right; 
		public short Bottom; 
	} 

	/// <summary>
	/// Win32 API struct
	/// </summary>
	struct CONSOLE_SCREEN_BUFFER_INFO 
	{ 
		public COORD      dwSize; 
		public COORD      dwCursorPosition; 
		public ushort     wAttributes; 
		public SMALL_RECT srWindow; 
		public COORD      dwMaximumWindowSize; 
	} 
}
