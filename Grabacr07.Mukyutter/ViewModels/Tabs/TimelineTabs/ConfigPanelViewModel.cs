using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.ViewModels.Twitter.Accounts;
using Grabacr07.Utilities;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;

namespace Grabacr07.Mukyutter.ViewModels.Tabs.TimelineTabs
{
	public class ConfigPanelViewModel : ViewModelBase
	{
		private TimelineBlock block;

		#region IsEdit 変更通知プロパティ

		private bool _IsEdit;

		public bool IsEdit
		{
			get { return this._IsEdit; }
			set
			{
				if (this._IsEdit != value)
				{
					this._IsEdit = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region FilterQuery 変更通知プロパティ

		public string FilterQuery
		{
			get { return this.block.FilterQuery; }
			set { this.block.FilterQuery = value; }
		}

		#endregion

		#region FilterMessage 変更通知プロパティ

		public string FilterMessage
		{
			get { return this.block.FilterMessage; }
		}

		#endregion

		#region CanCreateFilter 変更通知プロパティ

		public bool CanCreateFilter
		{
			get { return this.block.CanCreateFilter; }
		}

		#endregion

		#region EditElementVisibility 変更通知プロパティ

		private Visibility _EditElementVisibility = Visibility.Collapsed;

		public Visibility EditElementVisibility
		{
			get { return this._EditElementVisibility; }
			set
			{
				if (this._EditElementVisibility != value)
				{
					this._EditElementVisibility = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsUnreadCountDisplaying 変更通知プロパティ

		public bool IsUnreadCountDisplaying
		{
			// 変更通知は TimelineBlock の同名のプロパティから
			get { return this.block.IsUnreadCountDisplaying; }
			set { this.block.IsUnreadCountDisplaying = value; }
		}

		#endregion

		#region IsNotified 変更通知プロパティ

		public bool IsNotified
		{
			// 変更通知は TimelineBlock の同名のプロパティから
			get { return this.block.IsNotified; }
			set { this.block.IsNotified = value; }
		}

		#endregion

		#region ReceivingSettings 変更通知プロパティ

		private string _ReceivingSettings;

		public string ReceivingSettings
		{
			get { return this._ReceivingSettings; }
			set
			{
				if (this._ReceivingSettings != value)
				{
					this._ReceivingSettings = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CurrentAccount 変更通知プロパティ

		private AccountViewModel _CurrentAccount;

		public AccountViewModel CurrentAccount
		{
			get { return this._CurrentAccount; }
			set
			{
				if (this._CurrentAccount != value && value.IsValid)
				{
					this._CurrentAccount = value;
					this.block.AccountId = value.Account.UserId;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public ConfigPanelViewModel(TimelineBlock block)
		{
			this.block = block;
			this.CompositeDisposable.Add(new PropertyChangedEventListener(block)
			{
				(sender, e) => this.RaisePropertyChanged(e.PropertyName),
			});

			this._CurrentAccount = AccountViewModel.Get(block.Account);
			this.CompositeDisposable.Add(new CollectionChangedEventListener(TwitterClient.Current.Accounts)
			{
				// アカウントのコレクションが更新されたとき、現在のアカウントが設定されていなければ、再取得する
				(sender, e) => { if (!this.CurrentAccount.IsValid) this.CurrentAccount = AccountViewModel.Get(block.Account); },
			});

			this.ReceivingSettings = block.IsReceivingAll
				? "受信したすべてのツイート"
				: block.Timeline.SubscribedLists.ToString(" ");
		}


		public void StartEditing()
		{
			this.IsEdit = true;
			this.EditElementVisibility = Visibility.Visible;
		}

		public void EndEditing()
		{
			this.IsEdit = false;
			this.block.ClearFilter();
			this.EditElementVisibility = Visibility.Collapsed;
		}

		public void ApplyFilter()
		{
			if (this.block.ApplyFilter())
			{
				this.EndEditing();
			}
		}

		public void ShowReceivingSettings()
		{
			var settings = new ReceivingSettingsWindowViewModel(this.block.IsReceivingAll);
			this.Messenger.Raise(new TransitionMessage(settings, TransitionMode.Modal, "Show/ReceivingSettings"));
			if (settings.IsOK)
			{
				block.SetLists(settings.IsList ? settings.Lists.ToList() : null);
				this.ReceivingSettings = settings.Result;
			}
		}
	}
}
