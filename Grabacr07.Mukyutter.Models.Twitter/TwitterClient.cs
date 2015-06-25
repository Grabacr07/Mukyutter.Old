using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AsyncOAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Events;
using Grabacr07.Mukyutter.Models.Twitter.Data.Stores;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Mukyutter.Models.Twitter.Notifications;
using Grabacr07.Mukyutter.Models.Twitter.Settings;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter
{
	public class TwitterClient : NotificationObject
	{
		#region static members

		public static void Initialize()
		{
			OAuthUtility.ComputeHash = (key, buffer) =>
			{
				using (var hmac = new HMACSHA1(key))
				{
					return hmac.ComputeHash(buffer);
				}
			};
		}


		private static TwitterClient _current = new TwitterClient();

		public static TwitterClient Current
		{
			get { return _current; }
		}

		#endregion

		public ObservableSynchronizedCollection<TwitterApplication> Applications { get; private set; }
		public ObservableSynchronizedCollection<TwitterAccount> Accounts { get; private set; }

		public StatusStore Statuses { get; private set; }
		public UserStore Users { get; private set; }
		public DirectMessageStore Messages { get; private set; }
		public SourceStore Sources { get; private set; }
		public ListStore Lists { get; private set; }

		public ObservableSynchronizedCollection<NetworkProfile> NetworkProfiles { get; private set; }
		public NetworkProfile CurrentNetworkProfile { get; set; }

		#region Configuration プロパティ

		private Configuration _Configuration = Configuration.Default;

		public Configuration Configuration
		{
			get { return this._Configuration; }
			set
			{
				if (this._Configuration != value)
				{
					this._Configuration = value;
					RestApi.Configuration = value;
				}
			}
		}

		#endregion

		#region raise error / event

		public event EventHandler<ErrorRaisedEventArgs> ErrorRaised;
		public event EventHandler<EventRaisedEventArgs> EventRaised;

		private void RaiseError(string message, Exception ex, Action retryAction = null)
		{
			if (this.ErrorRaised != null)
			{
				this.ErrorRaised(this, new ErrorRaisedEventArgs(message, ex, retryAction));
			}
		}

		private void RaiseEvent(Event @event)
		{
			if (this.EventRaised != null && !@event.Source.IsSelf)	// 自アカウントから発するイベントは通知しない
			{
				this.EventRaised(this, new EventRaisedEventArgs { Event = @event, });
			}
		}

		#endregion

		private TwitterClient()
		{
			this.Applications = new ObservableSynchronizedCollection<TwitterApplication>();
			this.Accounts = new ObservableSynchronizedCollection<TwitterAccount>();
			this.Statuses = new StatusStore(this);
			this.Users = new UserStore();
			this.Messages = new DirectMessageStore(this);
			this.Sources = new SourceStore();
			this.Lists = new ListStore(this);
		}

		#region Save / Load

		public async Task LoadAccounts()
		{
			await SettingsHelper.LoadAccounts(this.LoadAccount);
		}

		internal void LoadAccount(TwitterAccount account)
		{
			this.GetConfiguration(account);

			if (this.Accounts.All(a => a.UserId != account.UserId))
			{
				this.Accounts.Add(account);
				Helper.Operation(
					() => account.GetLists().ToTask(), "ユーザー '{0}' のリスト一覧を取得できませんでした。", account.UserId);
				Helper.Operation(
					() => account.GetHomeTimeline(200, isStartup: true).ToTask(), "ユーザー '{0}' のホーム タイムラインを取得できませんでした。", account.UserId);
				Helper.Operation(
					() => account.GetUserTimeline(count: 200, isStartup: true).ToTask(), "ユーザー '{0}' のユーザー タイムラインを取得できませんでした。", account.UserId);
				Helper.Operation(
					() => account.GetMentionsTimeline(200, isStartup: true).ToTask(), "ユーザー '{0}' のメンション タイムラインを取得できませんでした。", account.UserId);

				if (account.UserStreams.UseUserStreams)
				{
					Helper.Operation(() => account.UserStreams.Connect(), ex => "User streams 接続に失敗しました。");
				}
			}
		}

		private async void GetConfiguration(TwitterAccount account)
		{
			if (this.Configuration == Configuration.Default)
			{
				try
				{
					this.Configuration = await account.CurrentToken.GetConfiguration();
				}
				catch (Exception ex)
				{
					ex.Report("設定情報を取得できませんでした。", () => this.GetConfiguration(account));
				}
			}
		}

		public Task SaveAccounts()
		{
			return this.Accounts.Save();
		}

		#endregion



		/// <summary>
		/// シーケンス内のそれぞれのステータスが、現在のアカウントへのメンションまたはリツイートだった場合、通知イベントを発生させます。
		/// </summary>
		/// <param name="statuses"></param>
		internal void RaiseEventIfMatch(IEnumerable<Status> statuses)
		{
			statuses.ForEach(this.RaiseEventIfMatch);
		}

		/// <summary>
		/// ステータスが、現在のアカウントへのメンションまたはリツイートだった場合、通知イベントを発生させます。
		/// </summary>
		/// <param name="status"></param>
		internal void RaiseEventIfMatch(Status status)
		{
			if (status.IsRetweetStatus)
			{
				if (this.Accounts.Any(a => status.RetweetedStatus.User.Id == a.UserId))
				{
					var retweet = new Retweet
					{
						CreatedAt = status.CreatedAt,
						TargetObject = status,
						Source = status.User,
						Target = status.RetweetedStatus.User,
					};
					this.RaiseEvent(retweet);
				}
			}
			else
			{
				this.Accounts.Where(
					account => TwitterDefinitions.AtMarks.Any(
						atmark => status.Text.IndexOf(
							atmark + account.User.ScreenName,
							StringComparison.OrdinalIgnoreCase) >= 0))
					.Select(
						account => new Mention
						{
							CreatedAt = status.CreatedAt,
							TargetObject = status,
							Source = status.User,
							Target = account.User,
						})
					.ForEach(this.RaiseEvent);
			}
		}

		/// <summary>
		/// フォロー通知イベントを発生させます。
		/// </summary>
		/// <param name="follow"></param>
		internal void RaiseFollowEvent(Follow follow)
		{
			DebugMonitor.WriteLine("follow! {0} -> {1}", follow.Source.ScreenName, follow.Target.ScreenName);
			this.RaiseEvent(follow);
		}

		/// <summary>
		/// お気に入りに登録されたことを示す通知イベントを発生させます。
		/// </summary>
		/// <param name="favorite"></param>
		internal void RaiseFavoriteEvent(Favorite favorite)
		{
			DebugMonitor.WriteLine(
				"{0}favorite! {1} -> {2}, {3}",
				favorite.Unfavorite ? "un" : "",
				favorite.Source.ScreenName,
				favorite.Target.ScreenName,
				favorite.TargetObject.Text);

			if (favorite.Unfavorite)
			{
				favorite.TargetObject.FavoriteUsers.Remove(favorite.Source);
			}
			else
			{
				favorite.TargetObject.FavoriteUsers.Add(favorite.Source);
				this.RaiseEvent(favorite);
			}
		}

		internal void ReportException(string message, Exception ex, Action retryAction = null)
		{
			ex.Write("ReportExceptoin - " + message);
			this.RaiseError(message, ex, retryAction);
		}
	}
}
