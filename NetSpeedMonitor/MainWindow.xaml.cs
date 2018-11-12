using NetSpeedMonitor.NetUtils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetSpeedMonitor
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private readonly NetFlowService _ns = new NetFlowService();

		private readonly ObservableCollection<NetProcessInfo> Table = new ObservableCollection<NetProcessInfo>();

		private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			_ns.Start();

			DownloadSpeedLabel.DataContext = _ns.NetFlow;
			UploadSpeedLabel.DataContext = _ns.NetFlow;

			ProcessesList.ItemsSource = _ns.NetProcessInfoList;
		}

		private void ToShowTable(IEnumerable<NetProcessInfo> table)
		{
			foreach (var item in table)
			{
				if (Table.Any(x => x.ProcessId == item.ProcessId))
				{
					//foreach (var x in Table)
					//{
					//	if (item.Index == x.Index)
					//	{
					//		x.Latency = item.Latency;
					//	}
					//}
				}
				else
				{
					Table.Add(item);
				}
			}
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			//DownloadSpeedLabel.Content = @"999.9 GiB/S";
			//UploadSpeedLabel.Content = @"999.9 GiB/S";
			//ToShowTable(_ns.NetProcessInfoList);
		}
	}
}
