using NetSpeedMonitor.NetUtils;
using System.Windows;
using System.Windows.Input;

namespace NetSpeedMonitor
{
	/// <summary>
	/// SimpleWindow.xaml 的交互逻辑
	/// </summary>
	public partial class SimpleWindow
	{
		public SimpleWindow()
		{
			InitializeComponent();
			if (Application.Current is App app)
			{
				_ns = app.Ns;
			}

			MouseDown += Window_MouseDown;
		}

		private readonly NetFlowService _ns;

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
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
	}
}
