﻿using NetSpeedMonitor.MyListView;
using NetSpeedMonitor.NetUtils;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Binding = System.Windows.Data.Binding;

namespace NetSpeedMonitor.Windows
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			if (Application.Current is App app)
			{
				_ns = app.Ns;
			}
		}

		private readonly NetFlowService _ns;

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
			AddSortBinding();

			_ns.NetProcessInfoList.CollectionChanged += ItemSource_CollectionChanged;
			_ns.DataChangeEvent += NsDataChangeEvent;

			DownloadSpeedLabel.DataContext = _ns.NetFlow;
			UploadSpeedLabel.DataContext = _ns.NetFlow;

			ProcessesList.ItemsSource = _ns.NetProcessInfoList;
		}

		private void Window_StateChanged(object sender, System.EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
			{
				Close();
			}
			else
			{
				if (Application.Current is App app)
				{
					app.DefaultState = WindowState;
				}
			}
		}

		private void NsDataChangeEvent()
		{
			try
			{
				Dispatcher.Invoke(() =>
				{
					if (Visibility == Visibility.Visible)
					{
						ListViewSorter.SortLast();
					}
				});
			}
			catch
			{
				// ignored
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Hide();
			e.Cancel = true;
		}
	}
}
