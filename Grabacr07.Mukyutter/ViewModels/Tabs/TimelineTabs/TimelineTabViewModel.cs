using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Filters;
using Grabacr07.Mukyutter.ViewModels.Twitter;
using Grabacr07.Mukyutter.ViewModels.Twitter.Accounts;
using Grabacr07.Utilities;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;

namespace Grabacr07.Mukyutter.ViewModels.Tabs.TimelineTabs
{
	public class TimelineTabViewModel : TabViewModel
	{
		private readonly TimelineBlock block;

		#region Statuses 変更通知プロパティ

		private ReadOnlyDispatcherCollection<StatusViewModel> _Statuses;

		public ReadOnlyDispatcherCollection<StatusViewModel> Statuses
		{
			get { return this._Statuses; }
			set
			{
				if (this._Statuses != value)
				{
					this._Statuses = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region SelectedStatus 変更通知プロパティ

		private StatusViewModel _SelectedStatus;

		public StatusViewModel SelectedStatus
		{
			get { return this._SelectedStatus; }
			set
			{
				if (this._SelectedStatus != value)
				{
					this._SelectedStatus = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsConfigMode 変更通知プロパティ

		private bool _IsConfigMode;

		public bool IsConfigMode
		{
			get { return this._IsConfigMode; }
			set
			{
				if (this._IsConfigMode != value)
				{
					this._IsConfigMode = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("ContentOpacity");
				}
			}
		}

		#endregion

		#region SearchQuery 変更通知プロパティ

		private string _SearchQuery;

		public string SearchQuery
		{
			get { return this._SearchQuery; }
			set
			{
				if (this._SearchQuery != value)
				{
					this._SearchQuery = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public AccountViewModel Account
		{
			// block からの全プロパティ変更イベントをリレーして変更通知する
			get { return AccountViewModel.Get(block.Account); }
		}

		public ConfigPanelViewModel ConfigPanel { get; private set; }


		internal TimelineTabViewModel() : base(null) { }

		public TimelineTabViewModel(TimelineBlock block)
			: base(block)
		{
			if (this.IsInDesignMode) return;

			this.block = block;
			this.CompositeDisposable.Add(new PropertyChangedEventListener(block)
			{
				{ "Statuses", (sender, e) => this.InitializeStatuses() },
				{ "UnreadCount", (sender, e) => this.Counter = this.block.UnreadCount },
			});
			this.InitializeStatuses();

			this.IsConfigMode = false;
			this.ConfigPanel = new ConfigPanelViewModel(block);
		}

		private void InitializeStatuses()
		{
			this.Statuses.SafeDispose();
			this.Statuses = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				this.block.Timeline.Statuses,
				StatusViewModel.Get,
				DispatcherHelper.UIDispatcher);
		}
	}
}
