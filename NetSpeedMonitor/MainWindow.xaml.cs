using NetSpeedMonitor.MyListView;
using NetSpeedMonitor.NetUtils;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Threading;
using Binding = System.Windows.Data.Binding;
using MessageBox = System.Windows.MessageBox;

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
			LoadIcon();
		}

		private void LoadIcon()
		{
			_nIcon.Icon = Properties.Resources.monitor;
			_nIcon.Visible = true;
			_nIcon.Click += (sender, e) =>
			{
				if (Visibility == Visibility.Visible)
				{
					Visibility = Visibility.Collapsed;
				}
				else
				{
					Visibility = Visibility.Visible;
					Dispatcher.BeginInvoke(DispatcherPriority.Background,
							new Action(delegate
							{
								WindowState = WindowState.Normal;
							})
					);
					Topmost = true;
					Topmost = false;
				}
			};
		}

		private readonly NetFlowService _ns = new NetFlowService();
		private readonly NotifyIcon _nIcon = new NotifyIcon();

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
					var log = new ConnectionWindow(p.NetConnectionInfoList, link.TargetName)
					{
						Owner = this
					};
					log.ShowDialog();
				}
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (!_ns.Start())
			{
				MessageBox.Show(@"Startup Failed", @"Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Exit(false);
			}

			AddSortBinding();

			_ns.NetProcessInfoList.CollectionChanged += ItemSource_CollectionChanged;
			_ns.DataChangeEvent += () => { ProcessesList.Dispatcher.Invoke(ListViewSorter.SortLast); };

			DownloadSpeedLabel.DataContext = _ns.NetFlow;
			UploadSpeedLabel.DataContext = _ns.NetFlow;

			ProcessesList.ItemsSource = _ns.NetProcessInfoList;
		}

		private void Exit(bool code = true)
		{
			_ns.Stop();
			_nIcon.Dispose();
			Environment.Exit(code ? 0 : 1);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Exit();
		}

		private void Window_StateChanged(object sender, System.EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
			{
				Visibility = Visibility.Collapsed;
			}
		}
	}
}
