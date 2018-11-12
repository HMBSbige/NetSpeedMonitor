using System;
using System.Data;

namespace NetSpeedMonitor.NetUtils
{
	public class NetConnectionInfo
	{
		public string ProtocolName { get; set; }
		public string LocalIP { get; set; }
		public int LocalPort { get; set; }
		public string RemoteIP { get; set; }
		public int RemotePort { get; set; }
		public ConnectionState Status { get; set; }
		public DateTime LastUpdateTime { get; set; }
	}
}
