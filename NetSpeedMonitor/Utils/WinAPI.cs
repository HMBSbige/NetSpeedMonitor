using System;
using System.Runtime.InteropServices;

namespace NetSpeedMonitor.Utils
{
	public static class WinAPI
	{
		public const int GWL_EXSTYLE = -20;
		public const int WS_EX_TOOLWINDOW = 0x80;
		public const int WS_EX_NOACTIVATE = 0x08000000;

		[DllImport(@"user32.dll")]
		public static extern long SetWindowLong(IntPtr handle, int oldStyle, IntPtr newStyle);

		[DllImport(@"user32.dll")]
		public static extern long GetWindowLong(IntPtr handle, int style);
	}
}
