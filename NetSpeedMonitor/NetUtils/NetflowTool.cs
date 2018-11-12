using NetSpeedMonitor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace NetSpeedMonitor.NetUtils
{
	public class NetFlowTool : INotifyPropertyChanged
	{
		#region 上行数据流量

		private long _uploadData;

		public string UploadSpeed => $@"{Util.CountSize(UploadData)}/S";

		public long UploadData
		{
			get => _uploadData;
			private set
			{
				if (_uploadData != value)
				{
					_uploadData = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(UploadSpeed));
				}
			}
		}

		#endregion

		#region 上行数据总流量

		private long _uploadDataCount;

		public string UploadDataCountStr => Util.CountSize(UploadDataCount);

		public long UploadDataCount
		{
			get => _uploadDataCount;
			private set
			{
				if (_uploadDataCount != value)
				{
					_uploadDataCount = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(UploadDataCountStr));
				}
			}
		}

		#endregion

		#region 下行数据流量

		private long _downloadData;

		public string DownloadSpeed => $@"{Util.CountSize(DownloadData)}/S";

		public long DownloadData
		{
			get => _downloadData;
			private set
			{
				if (_downloadData != value)
				{
					_downloadData = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(DownloadSpeed));
				}
			}
		}

		#endregion

		#region 下行数据总流量

		private long _downloadDataCount;

		public string DownloadDataCountStr => Util.CountSize(DownloadDataCount);

		public long DownloadDataCount
		{
			get => _downloadDataCount;
			private set
			{
				if (_downloadDataCount != value)
				{
					_downloadDataCount = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(DownloadDataCountStr));
				}
			}
		}

		#endregion

		#region 私有

		private List<PerformanceCounter> _uploadCounter, _downloadCounter; //上行、下行流量计数器
		private int _dataCounterInterval = 1000; //数据流量计数器计数周期
		private bool _isStart = false;

		#endregion

		#region 公有

		public delegate void MonitorEvent(NetFlowTool n);
		public MonitorEvent DataMonitorEvent;
		public string[] Instances { get; private set; } //网卡名

		#endregion

		private bool Init()
		{
			Instances = NetCardInfoTool.GetInstanceNames();
			if (ListTool.HasElements(Instances))
			{
				_uploadCounter = new List<PerformanceCounter>();
				_downloadCounter = new List<PerformanceCounter>();
				foreach (var instance in Instances)
				{
					try
					{
						_uploadCounter.Add(new PerformanceCounter(@"Network Interface", @"Bytes Sent/sec", instance));
						_downloadCounter.Add(new PerformanceCounter(@"Network Interface", @"Bytes Received/sec", instance));
					}
					catch
					{
						// ignored
					}
				}
			}

			return ListTool.HasElements(_uploadCounter) && ListTool.HasElements(_downloadCounter);
		}

		public bool Start(int interval = 1000)
		{
			if (Init() && !_isStart)
			{
				_dataCounterInterval = interval;
				_isStart = true;

				Task.Run(() =>
				{
					while (_isStart)
					{
						DataMonitorEvent?.Invoke(this);
						try
						{
							UploadDataCount += UploadData;
							UploadData = 0;
							foreach (var uc in _uploadCounter)
							{
								UploadData += Convert.ToInt64(uc?.NextValue());
							}
							DownloadDataCount += DownloadData;
							DownloadData = 0;
							foreach (var dc in _downloadCounter)
							{
								DownloadData += Convert.ToInt64(dc?.NextValue());
							}
						}
						catch (Exception)
						{
							// ignored
						}

						Thread.Sleep(_dataCounterInterval);
					}
				});
				return true;
			}
			return false;
		}

		public void Restart()
		{
			if (_isStart)
			{
				foreach (var uc in _uploadCounter)
				{
					uc?.Close();
				}
				foreach (var dc in _downloadCounter)
				{
					dc?.Close();
				}
			}
			Init();
		}

		public void Reset()
		{
			UploadData = 0;
			UploadDataCount = 0;
			DownloadData = 0;
			DownloadDataCount = 0;
		}

		public void Stop()
		{
			if (_isStart)
			{
				_isStart = false;
				foreach (var uc in _uploadCounter)
				{
					uc?.Close();
				}
				foreach (var dc in _downloadCounter)
				{
					dc?.Close();
				}
			}
		}

		~NetFlowTool()
		{
			Stop();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = @"")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}