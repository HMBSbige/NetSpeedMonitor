using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetSpeedMonitor.Utils
{
	public static class Util
	{
		public static string ToByteSize(long size)
		{
			var mStrSize = @"0";
			const double step = 1024.00;
			var factSize = size;
			if (factSize < step)
			{
				mStrSize = $@"{factSize:0.##} B";
			}
			else if (factSize >= step && factSize < 1048576)
			{
				mStrSize = $@"{factSize / step:0.##} KiB";
			}
			else if (factSize >= 1048576 && factSize < 1073741824)
			{
				mStrSize = $@"{factSize / step / step:0.##} MiB";
			}
			else if (factSize >= 1073741824 && factSize < 1099511627776)
			{
				mStrSize = $@"{factSize / step / step / step:0.##} GiB";
			}
			else if (factSize >= 1099511627776)
			{
				mStrSize = $@"{factSize / step / step / step / step:0.##} TiB";
			}

			return mStrSize;
		}

		public static long ToLong(string str)
		{
			if (str.EndsWith(@"/S"))
			{
				str = str.Substring(0, str.Length - 2);
			}

			if (!str.EndsWith(@"B"))
			{
				return 0;
			}

			var s = str.Split(' ');
			if (s.Length != 2)
			{
				throw new ArgumentException(@"Wrong Byte Size");
			}

			switch (s[1])
			{
				case @"TiB" when double.TryParse(s[0], out var res):
					return Convert.ToInt64(res * 1099511627776);
				case @"GiB" when double.TryParse(s[0], out var res):
					return Convert.ToInt64(res * 1073741824);
				case @"MiB" when double.TryParse(s[0], out var res):
					return Convert.ToInt64(res * 1048576);
				case @"KiB" when double.TryParse(s[0], out var res):
					return Convert.ToInt64(res * 1024);
				case @"B" when double.TryParse(s[0], out var res):
					return Convert.ToInt64(res);
				default:
					throw new ArgumentException(@"Wrong Byte Size");
			}
		}

		[DllImport(@"gdi32.dll", SetLastError = true)]
		private static extern bool DeleteObject(IntPtr hObject);

		public static ImageSource ToImageSource(this Icon icon)
		{
			if (icon == null)
			{
				return null;
			}
			var bitmap = icon.ToBitmap();
			var hBitmap = bitmap.GetHbitmap();

			ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
					hBitmap,
					IntPtr.Zero,
					Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());

			if (!DeleteObject(hBitmap))
			{
				throw new Win32Exception();
			}

			return wpfBitmap;
		}

		/// <summary>
		/// Remove the window from "Alt + TAB list".
		/// </summary>
		/// <param name="window">The window</param>
		/// <param name="NoActivate">No activate (get focus)</param>
		public static void WindowMissFromMission(Window window, bool NoActivate)
		{
			var helper = new WindowInteropHelper(window);
			var old = WinAPI.GetWindowLong(helper.Handle, WinAPI.GWL_EXSTYLE);
			old |= WinAPI.WS_EX_TOOLWINDOW;
			if (NoActivate)
			{
				old |= WinAPI.WS_EX_NOACTIVATE;
			}
			WinAPI.SetWindowLong(helper.Handle, WinAPI.GWL_EXSTYLE, (IntPtr)old);
		}

		/// <summary>
		/// Make sure the window is in the work area. (Make sure the window is in the screen and doesn't be covered by taskbar.)
		/// </summary>
		/// <param name="window">The window smaller than work area.</param>
		/// <param name="padding">Padding of the window</param>
		/// <returns>False if work area doesn't contain the window </returns>
		public static bool MoveWindowBackToWorkArea(Window window, Thickness padding)
		{
			var workArea = SystemParameters.WorkArea;
			var rect = new Rect(window.Left - padding.Left, window.Top - padding.Top, window.Width + padding.Left + padding.Right, window.Height + padding.Top + padding.Bottom);
			if (!workArea.Contains(rect))
			{
				var heightSpan = rect.Bottom - workArea.Bottom;
				if (heightSpan > 0)
				{
					window.Top = window.Top - heightSpan;
				}
				else
				{
					heightSpan = workArea.Top - rect.Top;
					if (heightSpan > 0)
					{
						window.Top = window.Top + heightSpan;
					}
				}

				var widthSpan = rect.Right - workArea.Right;
				if (widthSpan > 0)
				{
					window.Left = window.Left - widthSpan;
				}
				else
				{
					widthSpan = workArea.Left - rect.Left;
					if (widthSpan > 0)
					{
						window.Left = window.Left + widthSpan;
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
