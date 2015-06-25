using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Twitter.Accounts
{
	public class TokenViewModel : ViewModelBase
	{
		public TwitterToken Token { get; private set; }

		#region AppName 変更通知プロパティ

		private string _AppName;

		public string AppName
		{
			get { return this._AppName; }
			set
			{
				if (this._AppName != value)
				{
					this._AppName = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public IEnumerable<EndpointViewModel> Endpoints { get; private set; }

		public ReadOnlyDispatcherCollection<AccountViewModel> Accounts
		{
			get { return AccountViewModel.Accounts; }
		}

		#region Fallback

		#region IsFallback 変更通知プロパティ

		private bool _IsFallback;

		public bool IsFallback
		{
			get { return this._IsFallback; }
			set
			{
				if (this._IsFallback != value)
				{
					this._IsFallback = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public IReadOnlyCollection<AccountTokenPair> FallbackTokens
		{
			get
			{
				return new ReadOnlyCollection<AccountTokenPair>(
					this.Accounts.SelectMany(a => a.Tokens.Select(t => new AccountTokenPair { Account = a, Token = t }))
						.Where(pair => pair.Token != this)
						.ToList());
			}
		}

		#region FallbackToken 変更通知プロパティ

		private AccountTokenPair _FallbackToken;

		public AccountTokenPair FallbackToken
		{
			get { return this._FallbackToken; }
			set
			{
				if (this._FallbackToken != value)
				{
					this._FallbackToken = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#endregion


		public TokenViewModel(TwitterToken token)
		{
			this.Token = token;

			this.AppName = token.Application.Name;
			this.CompositeDisposable.Add(new PropertyChangedEventListener(token.Application)
			{
				{ "Name", (sender, e) => this.AppName = token.Application.Name },
			});
			
			this.Endpoints = token.Endpoints.Select(kvp => new EndpointViewModel(kvp.Value)).ToList();
		}
	}
}
