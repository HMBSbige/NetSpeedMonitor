using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;

namespace NetSpeedMonitor.Collections
{
	public class NetConnectionInfo : INotifyPropertyChanged
	{
		private string _protocolName;
		private string _localIp;
		private int _localPort;
		private string _remoteIp = IPAddress.Any.ToString();
		private int _remotePort;
		private ConnectionState _status;
		private DateTime _lastUpdateTime;

		public string ProtocolName
		{
			get => _protocolName;
			set
			{
				if (_protocolName != value)
				{
					_protocolName = value;
					NotifyPropertyChanged();
				}
			}
		}

		public string LocalIP
		{
			get => _localIp;
			set
			{
				if (_localIp != value)
				{
					_localIp = value;
					NotifyPropertyChanged();
				}
			}
		}

		public int LocalPort
		{
			get => _localPort;
			set
			{
				if (_localPort != value)
				{
					_localPort = value;
					NotifyPropertyChanged();
				}
			}
		}

		public string RemoteIP
		{
			get => _remoteIp;
			set
			{
				if (_remoteIp != value)
				{
					_remoteIp = value;
					NotifyPropertyChanged();
				}
			}
		}

		public int RemotePort
		{
			get => _remotePort;
			set
			{
				if (_remotePort != value)
				{
					_remotePort = value;
					NotifyPropertyChanged();
				}
			}
		}

		public ConnectionState Status
		{
			get => _status;
			set
			{
				if (_status != value)
				{
					_status = value;
					NotifyPropertyChanged();
				}
			}
		}

		public DateTime LastUpdateTime
		{
			get => _lastUpdateTime;
			set
			{
				if (_lastUpdateTime != value)
				{
					_lastUpdateTime = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(LastUpdateTimeStr));
				}
			}
		}

		public string LastUpdateTimeStr => LastUpdateTime.ToString(CultureInfo.CurrentCulture);

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = @"")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
