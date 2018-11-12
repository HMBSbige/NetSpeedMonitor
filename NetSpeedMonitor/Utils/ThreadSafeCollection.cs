using System.Collections.ObjectModel;
using System.Windows.Data;

namespace NetSpeedMonitor.Utils
{
	public class ThreadSafeCollection<T> : ObservableCollection<T>
	{
		private readonly object _lock = new object();

		public ThreadSafeCollection()
		{
			BindingOperations.EnableCollectionSynchronization(this, _lock);
		}
	}
}
