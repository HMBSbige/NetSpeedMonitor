using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace NetSpeedMonitor.MyListView
{
	public static class ListViewSorter
	{
		public static DependencyProperty CustomSorterProperty = DependencyProperty.RegisterAttached(
		@"CustomSorter",
		typeof(IComparer),
		typeof(ListViewSorter));

		public static IComparer GetCustomSorter(DependencyObject obj)
		{
			return (IComparer)obj.GetValue(CustomSorterProperty);
		}

		public static void SetCustomSorter(DependencyObject obj, IComparer value)
		{
			obj.SetValue(CustomSorterProperty, value);
		}

		public static DependencyProperty SortBindingMemberProperty = DependencyProperty.RegisterAttached(
		@"SortBindingMember",
		typeof(BindingBase),
		typeof(ListViewSorter));

		public static BindingBase GetSortBindingMember(DependencyObject obj)
		{
			return (BindingBase)obj.GetValue(SortBindingMemberProperty);
		}

		public static void SetSortBindingMember(DependencyObject obj, BindingBase value)
		{
			obj.SetValue(SortBindingMemberProperty, value);
		}

		public static readonly DependencyProperty IsListViewSortableProperty = DependencyProperty.RegisterAttached(
		@"IsListViewSortable",
		typeof(bool),
		typeof(ListViewSorter),
		new FrameworkPropertyMetadata(false, OnRegisterSortableGrid));

		public static bool GetIsListViewSortable(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsListViewSortableProperty);
		}

		public static void SetIsListViewSortable(DependencyObject obj, bool value)
		{
			obj.SetValue(IsListViewSortableProperty, value);
		}

		private static GridViewColumnHeader _lastHeaderClicked;
		private static ListSortDirection _lastDirection = ListSortDirection.Descending;
		private static ListView _lv;

		private static void OnRegisterSortableGrid(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if (obj is ListView grid)
			{
				_lv = grid;
				RegisterSortableGridView(grid);
			}
		}

		private static void RegisterSortableGridView(ListView grid)
		{
			grid.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClickedHandler));
		}

		private static void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is GridViewColumnHeader headerClicked)
			{
				ListSortDirection direction;
				if (headerClicked != _lastHeaderClicked)
				{
					direction = ListSortDirection.Descending;
				}
				else
				{
					direction = _lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
				}

				var header = string.Empty;

				try
				{
					header = ((Binding)GetSortBindingMember(headerClicked.Column)).Path.Path;
				}
				catch
				{
					// ignored
				}

				if (header == string.Empty)
				{
					return;
				}

				Sort(header, direction);

				var resourceTemplateName = string.Empty;
				DataTemplate tmpTemplate;

				if (_lastHeaderClicked != null)
				{
					resourceTemplateName = @"HeaderTemplateSortNon";
					tmpTemplate = _lv.TryFindResource(resourceTemplateName) as DataTemplate;
					_lastHeaderClicked.Column.HeaderTemplate = tmpTemplate;
				}

				switch (direction)
				{
					case ListSortDirection.Ascending: resourceTemplateName = @"HeaderTemplateArrowUp"; break;
					case ListSortDirection.Descending: resourceTemplateName = @"HeaderTemplateArrowDown"; break;
				}

				tmpTemplate = _lv.TryFindResource(resourceTemplateName) as DataTemplate;
				if (tmpTemplate != null)
				{
					headerClicked.Column.HeaderTemplate = tmpTemplate;
				}

				_lastHeaderClicked = headerClicked;
				_lastDirection = direction;

			}

		}

		private static void Sort(string sortBy, ListSortDirection direction)
		{
			var view = (ListCollectionView)CollectionViewSource.GetDefaultView(_lv.ItemsSource);

			if (view != null)
			{
				try
				{
					var sorter = (ListViewCustomComparer)GetCustomSorter(_lv);
					if (sorter != null)
					{
						sorter.AddSort(sortBy, direction);
						view.CustomSort = sorter;
						_lv.Items.Refresh();
					}
					else
					{
						_lv.Items.SortDescriptions.Clear();
						var sd = new SortDescription(sortBy, direction);
						_lv.Items.SortDescriptions.Add(sd);
						_lv.Items.Refresh();
					}
				}
				catch
				{
					// ignored
				}
			}
		}

		public static void SortLast()
		{
			if (_lastHeaderClicked != null)
			{
				Sort(_lastHeaderClicked.Column.Header.ToString(), _lastDirection);
			}
		}
	}


}
