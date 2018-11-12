using NetSpeedMonitor.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NetSpeedMonitor.NetUtils
{
	public static class NetCardInfoTool
	{
		/// <summary>
		/// 获取网卡信息
		/// 【名称、描述、物理地址（Mac）、Ip地址、网关地址】
		/// </summary>
		/// <returns></returns>
		public static List<Tuple<string, string, string, string, string>> GetNetworkCardInfo()
		{
			try
			{
				var result = new List<Tuple<string, string, string, string, string>>();
				var adapters = NetworkInterface.GetAllNetworkInterfaces();
				foreach (var item in adapters)
				{
					if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet || item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
					{
						var name = item.Name.Trim();
						var desc = item.Description.Trim();
						var mac = item.GetPhysicalAddress().ToString();
						//测试获取数据
						var ip = item.GetIPProperties().UnicastAddresses.Count >= 1 ? item.GetIPProperties().UnicastAddresses[0].Address.ToString() : null;
						//更新IP为ipv4地址
						if (item.GetIPProperties().UnicastAddresses.Count > 0)
						{
							ip = item.GetIPProperties().UnicastAddresses[item.GetIPProperties().UnicastAddresses.Count - 1].Address.ToString();
						}

						var gateway = item.GetIPProperties().GatewayAddresses.Count >= 1 ? item.GetIPProperties().GatewayAddresses[0].Address.ToString() : null;
						result.Add(new Tuple<string, string, string, string, string>(name, desc, mac, ip, gateway));
					}
				}
				return result;
			}
			catch (NetworkInformationException)
			{
				return null;
			}
		}

		/// <summary>
		/// 获取网络连接操作状态
		/// </summary>
		/// <param name="mac"></param>
		/// <returns></returns>
		public static OperationalStatus GetOpStatus(string mac)
		{
			try
			{
				var adapters = NetworkInterface.GetAllNetworkInterfaces();
				foreach (var item in adapters)
				{
					var _mac = item.GetPhysicalAddress().ToString();
					if (string.Equals(_mac.ToUpper(), mac.ToUpper(), StringComparison.Ordinal))
					{
						return item.OperationalStatus;
					}
				}
			}
			catch
			{
				// ignored
			}

			return OperationalStatus.Unknown;
		}

		/// <summary>
		/// 获取网卡实例名称
		/// </summary>
		/// <returns></returns>
		public static string[] GetInstanceNames()
		{
			string[] instances = null;
			try
			{
				var performanceCounterCategory = new PerformanceCounterCategory(@"Network Interface");
				instances = performanceCounterCategory.GetInstanceNames();
			}
			catch
			{
				// ignored
			}

			return instances;
		}

		/// <summary>
		/// 获取本机IPv4的IP地址
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<IPAddress> GetIPv4Address()
		{
			var hosts = new List<IPAddress>();
			try
			{
				var temp = Dns.GetHostAddresses(Dns.GetHostName());
				if (ListTool.HasElements(temp))
				{
					hosts.AddRange(temp.Where(t => t.AddressFamily == AddressFamily.InterNetwork));
				}
			}
			catch (Exception)
			{
				// ignored
			}

			return hosts;
		}

		/// <summary>
		/// 获取本机IPv4的IP地址
		/// </summary>
		/// <returns></returns>
		public static List<string> GetAllIPv4Address()
		{
			var hosts = new List<string>();
			try
			{
				var temp = Dns.GetHostAddresses(Dns.GetHostName());
				if (ListTool.HasElements(temp))
				{
					hosts.AddRange(from t in temp where t.AddressFamily == AddressFamily.InterNetwork select t.ToString());
				}
			}
			catch (Exception)
			{
				// ignored
			}

			hosts.Add(@"0.0.0.0");
			hosts.Add(@"127.0.0.1");
			return hosts;
		}

	}
}
