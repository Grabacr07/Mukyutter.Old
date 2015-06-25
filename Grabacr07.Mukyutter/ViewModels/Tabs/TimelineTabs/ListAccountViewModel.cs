using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Imaging;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Mukyutter.ViewModels.Twitter;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Tabs.TimelineTabs
{
	internal class ListAccountViewModel : ViewModelBase
	{
		private TwitterAccount account;
		private Action checkAction;

		#region User 変更通知プロパティ

		private UserViewModel _User;

		public UserViewModel User
		{
			get { return this._User; }
			set
			{
				if (this._User != value)
				{
					this._User = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("Icon");
					this.RaisePropertyChanged("Name");
				}
			}
		}

		#endregion

		private WeakReferenceBitmap _Icon;
		public WeakReferenceBitmap Icon
		{
			get
			{
				if (this.User == null) return WeakReferenceBitmap.Empty;
				return this._Icon ?? (this._Icon = new WeakReferenceBitmap(this.User.ReasonablyProfileImageUrl));
			}
		}

		public string Name
		{
			get { return this.User == null ? "user_id:" + this.account.UserId : this.User.ScreenNameWithAtmark; }
		}

		#region Lists 変更通知プロパティ

		private List<ListViewModel> _Lists;

		public List<ListViewModel> Lists
		{
			get { return this._Lists; }
			private set
			{
				if (this._Lists != value)
				{
					this._Lists = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CanDisplay 変更通知プロパティ

		private bool _CanDisplay;

		public bool CanDisplay
		{
			get { return this._CanDisplay; }
			set
			{
				if (this._CanDisplay != value)
				{
					this._CanDisplay = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Message 変更通知プロパティ

		private string _Message;

		public string Message
		{
			get { return this._Message; }
			set
			{
				if (this._Message != value)
				{
					this._Message = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public ListAccountViewModel(TwitterAccount account, Action checkAction)
		{
			this.account = account;
			this.checkAction = checkAction;
			this.User = account.User == null ? null : UserViewModel.Get(account.User);

			this.CompositeDisposable.Add(new PropertyChangedEventListener(account)
			{
				{ "IsInitialized", (sender, e) => this.User = account.User == null ? null : UserViewModel.Get(account.User) },
			});

			this.Lists = TwitterClient.Current.Lists.Get(account.UserId)
				.OrderBy(l => l.Name)
				.Select(l => new ListViewModel(l, checkAction))
				.ToList();
		}


		public async void Update()
		{
			this.Message = "取得しています...";
			try
			{
				var lists = await account.GetLists().ToTask();
				this.Lists = lists.OrderBy(l => l.FullName).Select(l => new ListViewModel(l, this.checkAction)).ToList();
				this.CanDisplay = true;
				this.Message = this.Lists.Any() ? "" : "リストはありません。";
			}
			catch (Exception ex)
			{
				this.Message = string.Format("リスト取得時にエラーが発生しました: {0}{1}", Environment.NewLine, ex.Message);
				this.CanDisplay = false;
			}
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
