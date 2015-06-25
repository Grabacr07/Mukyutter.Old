using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Imaging;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Twitter.Accounts
{
	/// <summary>
	/// Twitter アカウントに関する情報をビューに表示するためのデータ ストアを表します。
	/// このクラスのインスタンスは、1 つの Twitter アカウントと関連付けられます。
	///     <para>インスタンスをキャッシュするため、コンストラクターによる初期化はサポートされません。かわりに AccountViewModel.Get 静的メソッドを使用してください。</para>
	/// </summary>
	public class AccountViewModel : ViewModel
	{
		public TwitterAccount Account { get; private set; }

		public ReadOnlyDispatcherCollection<TokenViewModel> Tokens { get; private set; }

		#region CurrentToken 変更通知プロパティ

		private TokenViewModel _CurrentToken;

		public TokenViewModel CurrentToken
		{
			get { return this._CurrentToken; }
			set
			{
				if (this._CurrentToken != value)
				{
					this._CurrentToken = value;
					this.Account.CurrentToken = value.Token;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region DisplayToken 変更通知プロパティ

		private TokenViewModel _DisplayToken;

		public TokenViewModel DisplayToken
		{
			get { return this._DisplayToken; }
			set
			{
				if (this._DisplayToken != value)
				{
					this._DisplayToken = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsCurrentToken 変更通知プロパティ

		private bool _IsCurrentToken;

		public bool IsCurrentToken
		{
			get { return this._IsCurrentToken; }
			set
			{
				if (this._IsCurrentToken != value)
				{
					this._IsCurrentToken = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

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
				}
			}
		}

		#endregion

		#region Icon 読み取り専用プロパティ

		private WeakReferenceBitmap _Icon;

		public WeakReferenceBitmap Icon
		{
			get
			{
				return this._Icon ?? (this.User == null
					? WeakReferenceBitmap.Empty
					: this._Icon = new WeakReferenceBitmap(this.User.ReasonablyProfileImageUrl));
			}
		}

		#endregion

		public bool IsInitialized
		{
			get { return this.Account.IsInitialized; }
		}

		public virtual bool IsValid
		{
			get { return true; }
		}

		public UserStreamsViewModel UserStreams { get; private set; }


		protected AccountViewModel() { }

		private AccountViewModel(TwitterAccount account)
		{
			this.Account = account;
			this.User = account.User == null ? null : UserViewModel.Get(account.User);

			this.Tokens = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				account.Tokens,
				t => new TokenViewModel(t),
				DispatcherHelper.UIDispatcher);
			Action updateCurrentToken = 
				() => this.CurrentToken = this.Tokens.FirstOrDefault(t => t.Token.Application.Id == this.Account.CurrentToken.Application.Id);

			updateCurrentToken();
			this.DisplayToken = this.CurrentToken;

			this.CompositeDisposable.Add(new PropertyChangedEventListener(account)
			{
				{ "IsInitialized", (sender, e) => this.User = account.User == null ? null : UserViewModel.Get(account.User) },
				{ "CurrentToken", (sender, e) => updateCurrentToken() }
			});

			this.UserStreams = new UserStreamsViewModel(account.UserStreams);
			this.CompositeDisposable.Add(this.UserStreams);
		}


		#region static members

		private static readonly InvalidAccountViewModel invalid;
		private static readonly ConcurrentDictionary<UserId, AccountViewModel> cache;
		private static ReadOnlyDispatcherCollection<AccountViewModel> accounts;

		static AccountViewModel()
		{
			cache = new ConcurrentDictionary<UserId, AccountViewModel>();
			invalid = new InvalidAccountViewModel();
		}

		public static AccountViewModel Get(TwitterAccount account)
		{
			return account == null ? invalid : cache.GetOrAdd(account.UserId, new AccountViewModel(account));
		}

		public static ReadOnlyDispatcherCollection<AccountViewModel> Accounts
		{
			get { return accounts ?? (accounts = ViewModelHelper.CreateReadOnlyDispatcherCollection(TwitterClient.Current.Accounts, Get, DispatcherHelper.UIDispatcher)); }
		}

		public static InvalidAccountViewModel Invalid
		{
			get { return invalid; }
		}

		#endregion
	}

	public class InvalidAccountViewModel : AccountViewModel
	{
		public override bool IsValid
		{
			get { return false; }
		}
	}
}
