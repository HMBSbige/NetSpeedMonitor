#pragma warning disable CS0649, CS0169
using System;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;

namespace NetSpeedMonitor.NetUtils
{
	public static class NetProcessTool
	{
		#region 枚举 读取连接表选项
		enum TCP_TABLE_CLASS
		{
			TCP_TABLE_BASIC_LISTENER,
			TCP_TABLE_BASIC_CONNECTIONS,
			TCP_TABLE_BASIC_ALL,
			TCP_TABLE_OWNER_PID_LISTENER,
			TCP_TABLE_OWNER_PID_CONNECTIONS,
			TCP_TABLE_OWNER_PID_ALL,
			TCP_TABLE_OWNER_MODULE_LISTENER,
			TCP_TABLE_OWNER_MODULE_CONNECTIONS,
			TCP_TABLE_OWNER_MODULE_ALL
		}
		enum UDP_TABLE_CLASS
		{
			UDP_TABLE_BASIC,
			UDP_TABLE_OWNER_PID,
			UDP_TABLE_OWNER_MODULE
		}
		#endregion

		#region UDP 和 TCP 列表 结构
		struct UdpTable
		{
			public uint dwNumEntries;
			private UdpRow table;
		}
		struct TcpTable
		{
			public uint dwNumEntries;
			private TcpRow table;
		}
		#endregion

		#region UDP 和 TCP 行记录 结构
		public struct UdpRow
		{
			private uint localAddr;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			private byte[] localPort;

			public IPAddress LocalIP => new IPAddress(localAddr);

			public ushort LocalPort => BitConverter.ToUInt16(new[]
			{
					localPort[1],
					localPort[0]
			}, 0);

			public int ProcessId { get; }
		}

		public struct TcpRow
		{
			private ConnectionState state;
			private uint localAddr;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			private byte[] localPort;
			private uint remoteAddr;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			private byte[] remotePort;
			public ConnectionState State => state;

			public IPAddress LocalIP => new IPAddress(localAddr);

			public ushort LocalPort => BitConverter.ToUInt16(new[]
			{
					localPort[1],
					localPort[0]
			}, 0);

			public IPAddress RemoteIP => new IPAddress(remoteAddr);

			public ushort RemotePort => BitConverter.ToUInt16(new[]
			{
					remotePort[1],
					remotePort[0]
			}, 0);

			public int ProcessId { get; }
		}
		#endregion

		[DllImport(@"iphlpapi.dll", SetLastError = true)]
		static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool sort, int ipVersion, TCP_TABLE_CLASS tblClass, uint reserved = 0u);
		[DllImport(@"iphlpapi.dll", SetLastError = true)]
		static extern uint GetExtendedUdpTable(IntPtr pUdpTable, ref int dwOutBufLen, bool sort, int ipVersion, UDP_TABLE_CLASS tblClass, uint reserved = 0u);

		public static TcpRow[] GetTcpConnection()
		{
			TcpRow[] array = null;
			const int ipVersion = 2;
			var cb = 0;
			GetExtendedTcpTable(IntPtr.Zero, ref cb, true, ipVersion, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);
			var intPtr = Marshal.AllocHGlobal(cb);
			try
			{
				var extendedTcpTable = GetExtendedTcpTable(intPtr, ref cb, true, ipVersion, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);
				var flag = extendedTcpTable > 0u;
				if (flag)
				{
					return null;
				}

				var tcpTable = (TcpTable)Marshal.PtrToStructure(intPtr, typeof(TcpTable));
				var intPtr2 = (IntPtr)((long)intPtr + Marshal.SizeOf(tcpTable.dwNumEntries));
				array = new TcpRow[tcpTable.dwNumEntries];
				var num = 0;
				while (num < tcpTable.dwNumEntries)
				{
					var tcpRow = (TcpRow)Marshal.PtrToStructure(intPtr2, typeof(TcpRow));
					array[num] = tcpRow;
					intPtr2 = (IntPtr)((long)intPtr2 + Marshal.SizeOf(tcpRow));
					num++;
				}
			}
			catch (Exception)
			{
				// ignored
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return array;
		}

		public static UdpRow[] GetUdpConnection()
		{
			UdpRow[] array = null;
			const int ipVersion = 2;
			var cb = 0;
			GetExtendedUdpTable(IntPtr.Zero, ref cb, true, ipVersion, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID);
			var intPtr = Marshal.AllocHGlobal(cb);
			try
			{
				var extendedUdpTable = GetExtendedUdpTable(intPtr, ref cb, true, ipVersion, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID);
				var flag = extendedUdpTable > 0u;
				if (flag)
				{
					return null;
				}

				var udpTable = (UdpTable)Marshal.PtrToStructure(intPtr, typeof(UdpTable));
				var intPtr2 = (IntPtr)((long)intPtr + Marshal.SizeOf(udpTable.dwNumEntries));
				array = new UdpRow[udpTable.dwNumEntries];
				var num = 0;
				while (num < udpTable.dwNumEntries)
				{
					var udpRow = (UdpRow)Marshal.PtrToStructure(intPtr2, typeof(UdpRow));
					array[num] = udpRow;
					intPtr2 = (IntPtr)((long)intPtr2 + Marshal.SizeOf(udpRow));
					num++;
				}
			}
			catch (Exception)
			{
				// ignored
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return array;
		}
	}
}
