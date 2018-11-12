using NetSpeedMonitor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace NetSpeedMonitor.NetUtils
{
	public class NetProcessInfo : INotifyPropertyChanged
	{
		private int _processId;
		private string _processName;
		private Icon _processIcon;
		private DateTime _lastUpdateTime;
		private long _uploadData;
		private long _downloadData;
		private long _uploadDataCount;
		private long _downloadDataCount;
		private long _uploadBag;
		private long _downloadBag;
		private long _uploadBagCount;
		private long _downloadBagCount;

		public int ProcessId
		{
			get => _processId;
			set
			{
				if (_processId != value)
				{
					_processId = value;
					NotifyPropertyChanged();
				}
			}
		}

		public string ProcessName
		{
			get => _processName;
			set
			{
				if (_processName != value)
				{
					_processName = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Icon ProcessIcon
		{
			get => _processIcon;
			set
			{
				if (_processIcon != value)
				{
					_processIcon = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(ProcessImage));
				}
			}
		}
		public ImageSource ProcessImage => ProcessIcon.ToImageSource();

		public DateTime LastUpdateTime
		{
			get => _lastUpdateTime;
			set
			{
				if (_lastUpdateTime != value)
				{
					_lastUpdateTime = value;
					NotifyPropertyChanged();
				}
			}
		}

		public long UploadData
		{
			get => _uploadData;
			set
			{
				if (_uploadData != value)
				{
					_uploadData = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(UploadSpeed));
				}
			}
		}
		public string UploadSpeed => $@"{Util.ToByteSize(UploadData)}/S";

		public long DownloadData
		{
			get => _downloadData;
			set
			{
				if (_downloadData != value)
				{
					_downloadData = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(DownloadSpeed));
				}
			}
		}
		public string DownloadSpeed => $@"{Util.ToByteSize(DownloadData)}/S";

		public long UploadDataCount
		{
			get => _uploadDataCount;
			set
			{
				if (_uploadDataCount != value)
				{
					_uploadDataCount = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(UploadDataCountStr));
				}
			}
		}
		public string UploadDataCountStr => Util.ToByteSize(UploadDataCount);

		public long DownloadDataCount
		{
			get => _downloadDataCount;
			set
			{
				if (_downloadDataCount != value)
				{
					_downloadDataCount = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(DownloadDataCountStr));
				}
			}
		}
		public string DownloadDataCountStr => Util.ToByteSize(DownloadDataCount);

		public long UploadBag
		{
			get => _uploadBag;
			set
			{
				if (_uploadBag != value)
				{
					_uploadBag = value;
					NotifyPropertyChanged();
				}
			}
		}

		public long DownloadBag
		{
			get => _downloadBag;
			set
			{
				if (_downloadBag != value)
				{
					_downloadBag = value;
					NotifyPropertyChanged();
				}
			}
		}

		public long UploadBagCount
		{
			get => _uploadBagCount;
			set
			{
				if (_uploadBagCount != value)
				{
					_uploadBagCount = value;
					NotifyPropertyChanged();
				}
			}
		}

		public long DownloadBagCount
		{
			get => _downloadBagCount;
			set
			{
				if (_downloadBagCount != value)
				{
					_downloadBagCount = value;
					NotifyPropertyChanged();
				}
			}
		}

		//TODO
		public List<NetConnectionInfo> NetConnectionInfoList { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = @"")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
