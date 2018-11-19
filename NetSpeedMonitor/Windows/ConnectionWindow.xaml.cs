using NetSpeedMonitor.Collections;

namespace NetSpeedMonitor.Windows
{
	/// <summary>
	/// ConnectionWindow.xaml 的交互逻辑
	/// </summary>
	public partial class ConnectionWindow
	{
		public ConnectionWindow(ThreadSafeCollection<NetConnectionInfo> info, string processName)
		{
			InitializeComponent();
			Title = processName;
			List1.ItemsSource = info;
		}
	}
}
