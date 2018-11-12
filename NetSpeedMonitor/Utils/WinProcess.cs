using System;
using System.Diagnostics;
using System.IO;

namespace NetSpeedMonitor.Utils
{
	public static class WinProcess
	{
		public static void Stop(string processName)
		{
			var processes = Process.GetProcesses();
			foreach (var exe in processes)
			{
				if (string.Equals(exe.ProcessName, processName, StringComparison.CurrentCultureIgnoreCase))
				{
					exe.Kill();
					exe.WaitForExit();
					return;
				}
			}
		}

		public static void Start(string path, string arguments = @"")
		{
			var startInfo = new ProcessStartInfo
			{
				FileName = path,
				Arguments = arguments
			};
			Process.Start(startInfo);
		}

		public static void Restart(string path, string arguments = @"")
		{
			Stop(Path.GetFileNameWithoutExtension(path));
			Start(path, arguments);
		}
	}
}
