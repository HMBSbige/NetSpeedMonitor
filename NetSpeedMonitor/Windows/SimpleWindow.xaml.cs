using System;
using System.Windows;
using System.Windows.Input;
using NetSpeedMonitor.NetUtils;

namespace NetSpeedMonitor.Windows
{
	/// <summary>
	/// SimpleWindow.xaml 的交互逻辑
	/// </summary>
	public partial class SimpleWindow
	{
		public SimpleWindow()
		{
			InitializeComponent();
			_app = Application.Current as App;
			if (_app != null)
			{
				_ns = _app.Ns;
			}
			else
			{
				throw new ApplicationException();
			}
		}

		private readonly App _app;
		private readonly NetFlowService _ns;
		private readonly Thickness _windowPadding = new Thickness(0, 0, 0, 0);

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Utils.Util.WindowMissFromMission(this, false);
			DownloadSpeedLabel.DataContext = _ns.NetFlow;
			UploadSpeedLabel.DataContext = _ns.NetFlow;
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}

		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			Utils.Util.MoveWindowBackToWorkArea(this, _windowPadding);
		}

		private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			_app.ShowMainWindow();
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			_app.Exit();
		}

		private void ShowDetails_Click(object sender, RoutedEventArgs e)
		{
			_app.ShowMainWindow();
			if (_app.mainWindow.Visibility == Visibility.Visible)
			{
				ShowDetailsMenu.Header = @"Hide Traffic Details";
			}
			else
			{
				ShowDetailsMenu.Header = @"Show Traffic Details";
			}
		}
	}
}
