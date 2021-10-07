using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Microsoft.PaulMc
{
	/// <summary>
	/// Summary description for DPAPI.
	/// </summary>
	public class DataProtection 
	{
		#region Native Definitions
		private const int ERROR_CALL_NOT_IMPLEMENTED = 120;

		public enum CRYPTPROTECT_FLAGS
		{
			CRYPTPROTECT_UI_FORBIDDEN		= 0x1,
			CRYPTPROTECT_LOCAL_MACHINE		= 0x4,
			CRYPTPROTECT_CRED_SYNC          = 0x8,
			CRYPTPROTECT_AUDIT              = 0x10,
			CRYPTPROTECT_NO_RECOVERY        = 0x20,
			CRYPTPROTECT_VERIFY_PROTECTION  = 0x40,
			CRYPTPROTECT_CRED_REGENERATE    = 0x80
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct DATA_BLOB
		{
			public int cbData;
			public IntPtr pbData;

			public DATA_BLOB(int cb, IntPtr pb)
			{
				cbData = cb;
				pbData = pb;
			}
		} 

		[DllImport("crypt32.dll", SetLastError=true)]
		private static extern bool CryptProtectData(
			ref DATA_BLOB pDataIn, 
			[MarshalAs(UnmanagedType.LPWStr)]
			string szDataDescr, 
			uint pOptionalEntropy, 
			uint pvReserved, 
			uint pPromptStruct, 
			CRYPTPROTECT_FLAGS dwFlags, 
			ref DATA_BLOB pDataOut 
			);

		[DllImport("crypt32.dll", SetLastError=true)]
		private static extern bool CryptUnprotectData(
			ref DATA_BLOB pDataIn, 
			[MarshalAs(UnmanagedType.LPWStr)]
			out string ppszDataDescr, 
			uint pOptionalEntropy, 
			uint pvReserved, 
			uint pPromptStruct, 
			CRYPTPROTECT_FLAGS dwFlags, 
			ref DATA_BLOB pDataOut 
			);

		[DllImport("kernel32.dll", SetLastError=true)]
		private static extern bool LocalFree(
			IntPtr hLocal);			
		#endregion

//		//Protect string and return base64-encoded data.
//		public static string ProtectData(string data,
//			string name,
//			CRYPTPROTECT_FLAGS flags)
//		{
//			byte[] dataIn = Encoding.Unicode.GetBytes(data);
//			byte[] dataOut = ProtectData(dataIn, name, flags);
//			return (null != dataOut) ? Convert.ToBase64String(dataOut) : null;
//		}
//
//		//Unprotect base64-encoded data and return string.
//		public static string UnprotectData(string data)
//		{
//			byte[] dataIn = Convert.FromBase64String(data);
//			byte[] dataOut = UnprotectData(dataIn, CRYPTPROTECT_FLAGS.CRYPTPROTECT_UI_FORBIDDEN);
//			return (null != dataOut) ? Encoding.Unicode.GetString(dataOut) : null;
//		}
//
		//Protect string and return an encrypted byte array
		public static byte[] ProtectData(string data,
			string name,
			CRYPTPROTECT_FLAGS flags)
		{
			byte[] dataIn = Encoding.Unicode.GetBytes(data);
			return ProtectData(dataIn, name, flags);
		}

		//Unprotect a byte array data and return string.
		public static string UnprotectData(byte[] data)
		{
			byte[] dataOut = UnprotectData(data, CRYPTPROTECT_FLAGS.CRYPTPROTECT_UI_FORBIDDEN);
			return (null != dataOut) ? Encoding.Unicode.GetString(dataOut) : null;
		}

		public static bool PlatformSupportsDataProtection()
		{
			if (!platformTested)
			{
				supportsDataProtection = true;
				try
				{
					byte[] dummyData = { 1 };
					ProtectData(dummyData, "test", CRYPTPROTECT_FLAGS.CRYPTPROTECT_UI_FORBIDDEN);
				}
				catch (System.DllNotFoundException)
				{
					supportsDataProtection = false;
				}
				catch (System.EntryPointNotFoundException)
				{
					supportsDataProtection = false;
				}
				catch (Exception e)
				{
					if (Marshal.GetLastWin32Error() == ERROR_CALL_NOT_IMPLEMENTED)
					{
						supportsDataProtection = false;
					}
					else 
					{
						throw e;
					}
				}

				platformTested = true;
			}

			return supportsDataProtection;
		}

		////////////////////////
		//Internal functions //
		////////////////////////
		protected static bool platformTested = false;
		protected static bool supportsDataProtection;

		public static byte[] ProtectData(byte[] data,
			string name,
			CRYPTPROTECT_FLAGS dwFlags)
		{
			byte[] cipherText = null;

			//Copy data into unmanaged memory.
			DATA_BLOB din = new DATA_BLOB();
			din.cbData = data.Length;
			din.pbData = Marshal.AllocHGlobal(din.cbData);
			Marshal.Copy(data, 0, din.pbData, din.cbData);
			DATA_BLOB dout = new DATA_BLOB();
//			CRYPTPROTECT_PROMPTSTRUCT ps = new CRYPTPROTECT_PROMPTSTRUCT();

			//Fill the DPAPI prompt structure.
//			InitPromptstruct(ref ps);
			try 
			{
				bool ret =
					CryptProtectData(
					ref din,
					name,
					0,
					0,
					0,
					dwFlags, ref dout);
				if (ret)
				{
					cipherText = new byte [dout.cbData ];
					Marshal.Copy(dout.pbData, cipherText, 0, dout.cbData);
					LocalFree(dout.pbData);
				}
				else 
				{
					throw new Exception("Encryption error: CryptProtectData failed");
				}
			}
			finally 
			{
				if (din.pbData != IntPtr.Zero)
					Marshal.FreeHGlobal(din.pbData);
			}
			return cipherText;
		}

		public static byte[] UnprotectData(byte[] data, CRYPTPROTECT_FLAGS dwFlags)
		{
			byte[] clearText = null;

			//Copy data into unmanaged memory.
			DATA_BLOB din = new DATA_BLOB();
			din.cbData = data.Length;
			din.pbData = Marshal.AllocHGlobal(din.cbData);
			Marshal.Copy(data, 0, din.pbData, din.cbData);
//			CRYPTPROTECT_PROMPTSTRUCT ps = new CRYPTPROTECT_PROMPTSTRUCT();
//			InitPromptstruct(ref ps);
			DATA_BLOB dout = new DATA_BLOB();
			try 
			{
				string description;
				bool ret =
					CryptUnprotectData(
					ref din,
					out description,
					0,
					0,
					0,
					dwFlags,
					ref dout);
				if (ret)
				{
					clearText = new byte [dout.cbData ];
					Marshal.Copy(dout.pbData, clearText, 0, dout.cbData);
					LocalFree(dout.pbData);
				}
				else 
				{
					throw new Exception("Encryption error: CryptUnprotectData failed");
				}
			}
			finally 
			{
				if (din.pbData != IntPtr.Zero )
					Marshal.FreeHGlobal(din.pbData);
			}
			return clearText;
		}

//		static internal void InitPromptstruct(ref CRYPTPROTECT_PROMPTSTRUCT ps)
//		{
//			ps.cbSize = Marshal.SizeOf(typeof(CRYPTPROTECT_PROMPTSTRUCT));
//			ps.dwPromptFlags = 0;
//			ps.hwndApp = NullPtr;
//			ps.szPrompt = null;
//		}
	}
}
