using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using AsyncOAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;
using Grabacr07.Utilities.Events;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter
{
	public class TwitterAccount : NotificationObject
	{
		/// <summary>
		/// このアカウントが使用する API キーとトークンの組み合わせのコレクションを取得します。
		/// </summary>
		public ObservableSynchronizedCollection<TwitterToken> Tokens { get; private set; }

		#region CurrentToken 変更通知プロパティ

		private TwitterToken _CurrentToken;

		/// <summary>
		/// Twitter API エンド ポイントへのアクセスに使用するトークンを取得または設定します。
		/// </summary>
		public TwitterToken CurrentToken
		{
			get { return this._CurrentToken; }
			set
			{
				if (this._CurrentToken != value && value != null)
				{
					this._CurrentToken = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public UserId UserId { get; set; }
		public User User { get; private set; }

		#region Friends 変更通知プロパティ

		private UserIdCollection _Friends;

		public UserIdCollection Friends
		{
			get { return this._Friends; }
			set
			{
				if (this._Friends != value)
				{
					this._Friends = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Followers 変更通知プロパティ

		private UserIdCollection _Followers;

		public UserIdCollection Followers
		{
			get { return this._Followers; }
			set
			{
				if (this._Followers != value)
				{
					this._Followers = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsInitialized 変更通知プロパティ

		private bool _IsInitialized = false;

		/// <summary>
		/// アカウント情報 (ユーザー情報、Friends, Followers) の取得が完了しているかどうかを示す値を取得します。
		/// </summary>
		public bool IsInitialized
		{
			get { return this._IsInitialized; }
			private set
			{
				if (this._IsInitialized != value)
				{
					this._IsInitialized = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public UserStreams UserStreams { get; private set; }


		public TwitterAccount(TwitterApplication application, AccessToken token)
			: this(EnumerableEx.Return(new TwitterToken(application, token))) { }

		public TwitterAccount(IEnumerable<TwitterToken> tokens)
		{
			// ReSharper disable PossibleMultipleEnumeration
			Guard.ArgumentNull(tokens, "tokens");
			this.Tokens = new ObservableSynchronizedCollection<TwitterToken>(tokens);
			// ReSharper restore PossibleMultipleEnumeration
			this.CurrentToken = this.Tokens.FirstOrDefault();

			this.UserId = this.CurrentToken.ToUserId();
			this.UserStreams = new UserStreams(this);
		}


		public void AddToken(TwitterApplication application, AccessToken token)
		{
			Guard.ArgumentNull(application, "application");
			Guard.ArgumentNull(token, "token");

			// 同じアプリケーションのトークンが登録されていないかどうかを確認
			var duplicate = this.Tokens.SingleOrDefault(t => t.Application.Id == application.Id);
			if (duplicate == null)
			{
				this.Tokens.Add(new TwitterToken(application, token));
			}
			else
			{
				duplicate.ChangeToken(token);
			}
		}

		public async Task Initialize()
		{
			if (this.User == null)
			{
				try
				{
					this.User = await this.GetUser(this.UserId).ToTask();
					this.User.IsSelf = true;
				}
				catch (Exception ex)
				{
					TwitterClient.Current.ReportException(
						string.Format("アカウント {0} のユーザー情報取得に失敗しました。", this.UserId),
						ex, async () => await Initialize());
				}
			}
			if (this.Friends == null)
			{
				try
				{
					this.Friends = await this.GetFriends().ToTask();
				}
				catch (Exception ex)
				{
					TwitterClient.Current.ReportException(
						string.Format("アカウント {0} の friends 取得に失敗しました。", this.UserId),
						ex, async () => await Initialize());
				}
			}
			if (this.Followers == null)
			{
				try
				{
					this.Followers = await this.GetFollowers().ToTask();
				}
				catch (Exception ex)
				{
					TwitterClient.Current.ReportException(
						string.Format("アカウント {0} の followers 取得に失敗しました。", this.UserId),
						ex, async () => await Initialize());
				}
			}

			if (this.User != null && this.Friends != null && this.Followers != null)
			{
				this.IsInitialized = true;
			}
		}
	}
}
