using System.Collections.Generic;
using System.Linq;

namespace NetSpeedMonitor.Utils
{
	/// <summary>
	/// 元素列表工具类
	/// </summary>
	public static class ListTool
	{
		/// <summary>
		/// 列表为空（null 或 count 等于 0）
		/// </summary>
		/// <typeparam name="T">元素类型</typeparam>
		/// <param name="list">元素列表</param>
		/// <returns></returns>
		public static bool IsNullOrEmpty<T>(IEnumerable<T> list)
		{
			if (list != null && list.Any())
				return false;
			return true;
		}

		/// <summary>
		/// 列表至少有一个元素
		/// </summary>
		/// <typeparam name="T">元素类型</typeparam>
		/// <param name="list">元素列表</param>
		/// <returns></returns>
		public static bool HasElements<T>(IEnumerable<T> list)
		{
			return !IsNullOrEmpty(list);
		}
	}
}
