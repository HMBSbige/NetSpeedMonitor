using NetSpeedMonitor.NetUtils;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;

namespace NetSpeedMonitor
{
	/// <summary>
	/// App.xaml 的交互逻辑
	/// </summary>
	public partial class App
	{
		public readonly NetFlowService Ns = new NetFlowService();
		public WindowState DefaultState = WindowState.Normal;
		private readonly NotifyIcon notifyIcon = new NotifyIcon();
		private MainWindow mainWindow;
		private SimpleWindow simpleWindow;

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			LoadIcon();
			simpleWindow = new SimpleWindow();
			Task.Run(() =>
			{
				if (!Ns.Start())
				{
					MessageBox.Show(@"Startup Failed", @"Error", MessageBoxButton.OK, MessageBoxImage.Error);
					Exit(false);
				}

				Current.Dispatcher.Invoke(() =>
				{
					simpleWindow.Show();
				});
			});
		}

		private void LoadIcon()
		{
			notifyIcon.Icon = new Icon(GetResourceStream(new Uri(@"pack://application:,,,/Resources/monitor.ico")).Stream);

			notifyIcon.Click += (sender, e) =>
			{
				if (e is MouseEventArgs me && me.Button == MouseButtons.Left)
				{
					if (mainWindow == null)
					{
						mainWindow = new MainWindow();
						mainWindow.Closed += MainWindow_Closed;
						mainWindow.Show();
					}
					else
					{
						if (mainWindow.Visibility == Visibility.Collapsed)
						{
							mainWindow.Visibility = Visibility.Visible;
							Dispatcher.BeginInvoke(DispatcherPriority.Background,
									new Action(delegate { mainWindow.WindowState = DefaultState; }));
							mainWindow.Topmost = true;
							mainWindow.Topmost = false;
						}
						else
						{
							mainWindow.Visibility = Visibility.Collapsed;
						}
					}
				}
			};

			var menuExit = new MenuItem(@"Exit", (sender, args) => { Exit(); });

			var menu = new ContextMenu(new[] { menuExit });
			notifyIcon.ContextMenu = menu;

			notifyIcon.Visible = true;
		}

		private void MainWindow_Closed(object sender, EventArgs e)
		{
			Exit();
		}

		private new void Exit(bool code = true)
		{
			Ns.Stop();
			notifyIcon.Dispose();
			Shutdown(code ? 0 : 1);
		}
	}
}
