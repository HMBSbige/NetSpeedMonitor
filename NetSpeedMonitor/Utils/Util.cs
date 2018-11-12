namespace NetSpeedMonitor.Utils
{
	public static class Util
	{
		public static string CountSize(long size)
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
	}
}
