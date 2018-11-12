using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace NetSpeedMonitor.MyListView
{
	public abstract class ListViewCustomComparer : IComparer
	{
		protected readonly Dictionary<string, ListSortDirection> SortColumns = new Dictionary<string, ListSortDirection>();

		public void AddSort(string sortColumn, ListSortDirection dir)
		{
			//if (SortColumns.ContainsKey(sortColumn))
			//{
			//	SortColumns.Remove(sortColumn);
			//}
			ClearSort();
			SortColumns.Add(sortColumn, dir);
		}

		public void ClearSort()
		{
			SortColumns.Clear();
		}

		protected List<string> GetSortColumnList()
		{
			var result = new List<string>();
			var temp = new Stack<string>();

			foreach (var col in SortColumns.Keys)
			{
				temp.Push(col);
			}

			while (temp.Count > 0)
			{
				result.Add(temp.Pop());
			}

			return result;
		}

		public abstract int Compare(object x, object y);
	}

}
