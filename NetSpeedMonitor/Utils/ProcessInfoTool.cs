using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NetSpeedMonitor.Utils
{
	public static class ProcessInfoTool
	{
		#region 程序集信息
		private struct SHFILEINFO
		{
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.LPStr)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.LPStr)]
			public string szTypeName;
			public SHFILEINFO(bool b)
			{
				hIcon = IntPtr.Zero;
				iIcon = 0;
				dwAttributes = 0u;
				szDisplayName = string.Empty;
				szTypeName = string.Empty;
			}
		}

		private enum SHGFI
		{
			SmallIcon = 1,
			LargeIcon = 0,
			Icon = 256,
			DisplayName = 512,
			Typename = 1024,
			SysIconIndex = 16384,
			UseFileAttributes = 16
		}

		#endregion

		[DllImport(@"Shell32.dll")]
		private static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);

		private static Icon GetIcon(string file, bool small)
		{
			try
			{
				var sHFILEINFO = new SHFILEINFO(true);
				var cbfileInfo = Marshal.SizeOf(sHFILEINFO);
				SHGFI uFlags;
				if (small)
				{
					uFlags = (SHGFI)273;
				}
				else
				{
					uFlags = (SHGFI)272;
				}
				SHGetFileInfo(file, 256u, out sHFILEINFO, (uint)cbfileInfo, uFlags);
				return Icon.FromHandle(sHFILEINFO.hIcon);
			}
			catch
			{
				// ignored
			}

			return null;
		}

		public static Icon GetIcon(Process p, bool small)
		{
			try
			{
				var fileName = p.MainModule.FileName;
				return GetIcon(fileName, small);
			}
			catch
			{
				// ignored
			}

			return null;
		}
	}
}
