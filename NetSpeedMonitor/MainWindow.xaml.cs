using NetSpeedMonitor.MyListView;
using NetSpeedMonitor.NetUtils;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

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

		#region 列表排序

		private void AddSortBinding()
		{
			var gv = (GridView)ProcessesList.View;

			foreach (var col in gv.Columns)
			{
				ListViewSorter.SetSortBindingMember(col, new Binding(col.Header.ToString()));
			}

			ListViewSorter.SetCustomSorter(ProcessesList, new MySorter());
		}

		private void ItemSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			try
			{
				//增加或删除
				if (e.Action != NotifyCollectionChangedAction.Replace)
				{
					var dataView = CollectionViewSource.GetDefaultView(ProcessesList.ItemsSource);
					dataView.Refresh();
				}
			}
			catch
			{
				// ignored
			}
		}

		#endregion

		private void ShowConnectionList(object sender, RoutedEventArgs e)
		{
			if (e.Source is Hyperlink link)
			{
				var p = _ns.NetProcessInfoList.FirstOrDefault(x => x.ProcessName == link.TargetName);
				if (p != null && p.NetConnectionInfoList.Count > 0)
				{
					var log = new ConnectionWindow(p.NetConnectionInfoList, link.TargetName);
					log.ShowDialog();
				}
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_ns.Start();

			AddSortBinding();

			_ns.NetProcessInfoList.CollectionChanged += ItemSource_CollectionChanged;
			_ns.DataChangeEvent += () => { ProcessesList.Dispatcher.Invoke(ListViewSorter.SortLast); };

			DownloadSpeedLabel.DataContext = _ns.NetFlow;
			UploadSpeedLabel.DataContext = _ns.NetFlow;

			ProcessesList.ItemsSource = _ns.NetProcessInfoList;
		}


		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//DownloadSpeedLabel.Content = @"999.9 GiB/S";
			//UploadSpeedLabel.Content = @"999.9 GiB/S";
			//_ns.Stop();
			//ProcessesList.Dispatcher.Invoke(ListViewSorter.SortLast);
		}
	}
}
