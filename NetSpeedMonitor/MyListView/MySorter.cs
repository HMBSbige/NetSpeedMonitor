using NetSpeedMonitor.NetUtils;
using System.ComponentModel;

namespace NetSpeedMonitor.MyListView
{
	internal class MySorter : ListViewCustomComparer
	{
		public override int Compare(object x, object y)
		{
			try
			{
				var a = (NetProcessInfo)x;
				var b = (NetProcessInfo)y;
				var result = 0;

				foreach (var sortColumn in GetSortColumnList())
				{
					if (sortColumn == @"Process Name")
					{
						var xStr = a.ProcessName;
						var yStr = b.ProcessName;
						result = string.CompareOrdinal(xStr, yStr);
					}
					else
					{
						long xInt = 0L, yInt = 0L;
						switch (sortColumn)
						{
							case @"Download Speed":
							{
								xInt = a.DownloadData;
								yInt = b.DownloadData;
								break;
							}
							case @"Download Count":
							{
								xInt = a.DownloadDataCount;
								yInt = b.DownloadDataCount;
								break;
							}
							case @"Upload Speed":
							{
								xInt = a.UploadData;
								yInt = b.UploadData;
								break;
							}
							case @"Upload Count":
							{
								xInt = a.UploadDataCount;
								yInt = b.UploadDataCount;
								break;
							}
						}

						if (xInt == yInt)
						{
							result = 0;
						}
						else
						{
							result = xInt > yInt ? 1 : -1;
						}
					}

					if (SortColumns[sortColumn] == ListSortDirection.Descending)
					{
						result = -1 * result;
					}

					if (result != 0)
					{
						break;
					}
				}

				return result;
			}
			catch
			{
				return 0;
			}
		}
	}
}
