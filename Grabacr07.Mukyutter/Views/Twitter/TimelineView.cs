using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Grabacr07.Mukyutter.Models.Twitter.Filters;
using Grabacr07.Mukyutter.ViewModels.Twitter;
using Grabacr07.Utilities.Events;

namespace Grabacr07.Mukyutter.Views.Twitter
{
	class TimelineView : Control
	{
		static TimelineView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineView), new FrameworkPropertyMetadata(typeof(TimelineView)));
		}

		public TimelineView()
		{
		}


		#region ItemsSource 依存関係プロパティ

		public IEnumerable<StatusViewModel> ItemsSource
		{
			get { return (IEnumerable<StatusViewModel>)this.GetValue(TimelineView.ItemsSourceProperty); }
			set { this.SetValue(TimelineView.ItemsSourceProperty, value); }
		}
		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(IEnumerable<StatusViewModel>), typeof(TimelineView), new UIPropertyMetadata(null, TimelineView.ItemsSourcePropertyChangedCallback));

		private static void ItemsSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var source = (TimelineView)d;
			source.UpdateView();
			source.UpdateFilteringView();
		}

		#endregion

		#region SortDirection 依存関係プロパティ

		public ListSortDirection SortDirection
		{
			get { return (ListSortDirection)this.GetValue(TimelineView.SortDirectionProperty); }
			set { this.SetValue(TimelineView.SortDirectionProperty, value); }
		}
		public static readonly DependencyProperty SortDirectionProperty =
			DependencyProperty.Register("SortDirection", typeof(ListSortDirection), typeof(TimelineView), new UIPropertyMetadata(ListSortDirection.Descending, TimelineView.SortDirectionPropertyChangedCallback));

		private static void SortDirectionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var source = (TimelineView)d;
			source.UpdateView();
			source.UpdateFilteringView();
		}

		#endregion

		#region CollectionView 依存関係プロパティ

		public CollectionView CollectionView
		{
			get { return (CollectionView)this.GetValue(TimelineView.CollectionViewProperty); }
			private set { this.SetValue(TimelineView.CollectionViewProperty, value); }
		}
		public static readonly DependencyProperty CollectionViewProperty =
			DependencyProperty.Register("CollectionView", typeof(CollectionView), typeof(TimelineView), new UIPropertyMetadata(null, TimelineView.CollectionViewPropertyChangedCallback));

		private static void CollectionViewPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var source = (TimelineView)d;
		}

		#endregion


		#region Filter 依存関係プロパティ

		public Predicate<StatusViewModel> Filter
		{
			get { return (Predicate<StatusViewModel>)this.GetValue(TimelineView.FilterProperty); }
			set { this.SetValue(TimelineView.FilterProperty, value); }
		}
		public static readonly DependencyProperty FilterProperty =
			DependencyProperty.Register("Filter", typeof(Predicate<StatusViewModel>), typeof(TimelineView), new UIPropertyMetadata(null, TimelineView.FilterPropertyChangedCallback));

		private static void FilterPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var source = (TimelineView)d;
			source.UpdateFilteringView();
		}

		#endregion

		#region IsFiltering 依存関係プロパティ

		public bool IsFiltering
		{
			get { return (bool)this.GetValue(TimelineView.IsFilteringProperty); }
			private set { this.SetValue(TimelineView.IsFilteringProperty, value); }
		}
		public static readonly DependencyProperty IsFilteringProperty =
			DependencyProperty.Register("IsFiltering", typeof(bool), typeof(TimelineView), new UIPropertyMetadata(false, TimelineView.IsFilteringPropertyChangedCallback));

		private static void IsFilteringPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var source = (TimelineView)d;
		}

		#endregion

		#region FilteringCollectionView 依存関係プロパティ

		public CollectionView FilteringCollectionView
		{
			get { return (CollectionView)this.GetValue(TimelineView.FilteringCollectionViewProperty); }
			private set { this.SetValue(TimelineView.FilteringCollectionViewProperty, value); }
		}
		public static readonly DependencyProperty FilteringCollectionViewProperty =
			DependencyProperty.Register("FilteringCollectionView", typeof(CollectionView), typeof(TimelineView), new UIPropertyMetadata(null, TimelineView.FilteringCollectionViewPropertyChangedCallback));

		private static void FilteringCollectionViewPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var source = (TimelineView)d;
		}

		#endregion


		#region SelectedStatus 依存関係プロパティ

		public object SelectedStatus
		{
			get { return (object)this.GetValue(TimelineView.SelectedStatusProperty); }
			set { this.SetValue(TimelineView.SelectedStatusProperty, value); }
		}

		public static readonly DependencyProperty SelectedStatusProperty =
			DependencyProperty.Register("SelectedStatus", typeof(object), typeof(TimelineView), new UIPropertyMetadata(null, TimelineView.SelectedStatusChangedCallback));

		private static void SelectedStatusChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var instance = (TimelineView)d;

		}

		#endregion


		private void UpdateView()
		{
			var view = CollectionViewSource.GetDefaultView(this.ItemsSource) as CollectionView;
			if (view != null)
			{
				view.SortDescriptions.Add(new SortDescription("Id", this.SortDirection));
				this.CollectionView = view;
			}
		}

		private void UpdateFilteringView()
		{
			CollectionView view = null;

			if (this.Filter != null)
			{
				view = CollectionViewSource.GetDefaultView(this.ItemsSource) as CollectionView;
				if (view != null)
				{
					view.SortDescriptions.Add(new SortDescription("Id", this.SortDirection));
					view.Filter += item =>
					{
						var status = item as StatusViewModel;
						if (status == null) return false;
						else return this.Filter(status);
					};
				}
			}

			if (view == null)
			{
				this.IsFiltering = false;
				this.FilteringCollectionView = null;
			}
			else
			{
				this.FilteringCollectionView = view;
				this.IsFiltering = true;
			}
		}
	}
}
