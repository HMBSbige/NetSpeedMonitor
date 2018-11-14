using NetSpeedMonitor.Collections;

namespace NetSpeedMonitor
{
	/// <summary>
	/// ConnectionWindow.xaml 的交互逻辑
	/// </summary>
	public partial class ConnectionWindow
	{
		public ConnectionWindow(ThreadSafeCollection<NetConnectionInfo> info, string processName)
		{
			InitializeComponent();
			List1.ItemsSource = info;
			Title = processName;
		}
	}
}
