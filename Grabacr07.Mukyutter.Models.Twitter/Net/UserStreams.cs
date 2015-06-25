using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncOAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Events;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Data.Stores;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public class UserStreams : Connector
	{
		private const string endpoint = "userstream/user";

		private IDisposable connection;
		private bool isconnecting;
		private readonly Stopwatch stopwatch = new Stopwatch();

		#region UseUserStreams 変更通知プロパティ

		private bool _UseUserStreams;

		/// <summary>
		/// User streams 接続を使用するかどうかを示す値を取得または設定します。
		/// </summary>
		public bool UseUserStreams
		{
			get { return this._UseUserStreams; }
			set
			{
				if (this._UseUserStreams != value)
				{
					this._UseUserStreams = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Status 変更通知プロパティ

		public UserStreamsStatus Status
		{
			get
			{
				if (this.connection != null)
				{
					return UserStreamsStatus.Connected;
				}
				if (!this.UseUserStreams)
				{
					return UserStreamsStatus.Disabled;
				}
				if (this.isconnecting)
				{
					return UserStreamsStatus.Connecting;
				}
				if (!this.IsAvailable)
				{
					return UserStreamsStatus.Offline;
				}

				return UserStreamsStatus.Disconnected;
			}
		}

		private void RaiseStatusChanged()
		{
			this.RaisePropertyChanged("Status");
		}

		#endregion

		public UserStreams(TwitterAccount account)
			: base(account) { }


		#region Connect / Disconnect

		public async Task Connect()
		{
			if (this.IsAvailable && this.UseUserStreams)
			{
				try
				{
					this.isconnecting = true;
					this.Disconnect();

					await this.Trial();
					await Task.Factory.StartNew(this.GetStream);
				}
				finally
				{
					this.isconnecting = false;
					this.RaiseStatusChanged();
				}
			}
		}

		protected override async void ConnectCore()
		{
			base.ConnectCore();
			try
			{
				await this.Connect();
			}
			catch (Exception ex)
			{
				TwitterClient.Current.ReportException("UserStreams 接続に失敗しました。", ex, this.ConnectCore);
			}
		}

		public void Disconnect()
		{
			if (this.connection != null)
			{
				this.connection.Dispose();
				this.connection = null;
			}
			this.RaiseStatusChanged();
			this.stopwatch.Stop();
			this.stopwatch.Reset();
		}

		protected override void DisconnectCore()
		{
			base.DisconnectCore();
			this.Disconnect();
		}

		#endregion


		private void GetStream()
		{
			var connector = this.GetStreamCore()
				.Where(json => !string.IsNullOrEmpty(json))
				.Select(json =>
				{
					try
					{
						return DynamicJsonHelper.ToDynamicJson(json);
					}
					catch (Exception ex)
					{
						TwitterClient.Current.ReportException(
							"User streams から認識できないデータを受信しました。 (" + json + ")", ex);
						return null;
					}
				})
				.Where(djson => djson != null)
				.Catch((Exception ex) =>
				{
					TwitterClient.Current.ReportException(
						"User streams 接続がエラーにより切断されました (接続時間 " + this.stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss") + ")。",
						ex, this.ConnectCore);
					this.Disconnect();
					return Observable.Empty<string>();
				})
				.Publish();

			#region DEBUG
#if DEBUG
			connector.Subscribe(djson =>
			{
				DebugMonitor.WriteLine("stream: {0}", djson);
				JsonMonitor.UserStreams.Data.Add(djson.ToString());
			});
#endif
			#endregion

			connector.Take(1)
				.Select(djson => UserIdCollection.ParseCore(djson.friends))
				.Subscribe(friends => this.Account.Friends = friends);

			connector.Where(s => s.IsDefined("text") && s.IsDefined("user"))
				.Select(djson => TwitterClient.Current.Statuses.Add(djson, StatusSource.UserStreams))
				.OnErrorRetryFromStreaming("status")
				.Subscribe(status => TwitterClient.Current.RaiseEventIfMatch(status));

			connector.Where(djson => djson.IsDefined("delete"))
				.Select(djson => (StatusId)StatusId.Parse(djson.delete.status.id_str))
				.OnErrorRetryFromStreaming("delete")
				.Select(id => TwitterClient.Current.Statuses[id])
				.Where(status => status != null)
				.Subscribe(status => status.IsDeleted = true);

			connector.Where(djson => djson.IsDefined("direct_message"))
				.Select(djson => TwitterClient.Current.Messages.Parse(djson.direct_message))
				.OnErrorRetryFromStreaming("direct_message")
				.Subscribe();

			var eventConnector = connector.Where(djson => djson.IsDefined("event"))
				.Publish();

			eventConnector.Where(ev => (ev.@event == "favorite") || (ev.@event == "unfavorite"))
				.Select(fav => Favorite.ParseCore(fav))
				.OnErrorRetryFromStreaming("favorite")
				.Subscribe(fav => TwitterClient.Current.RaiseFavoriteEvent(fav));

			eventConnector.Where(ev => ev.@event == "follow")
				.Select(follow => Follow.ParseCore(follow))
				.OnErrorRetryFromStreaming("follow")
				.Subscribe(follow => TwitterClient.Current.RaiseFollowEvent(follow));

			eventConnector.Connect();

			this.stopwatch.Start();
			this.connection = connector.Connect();
		}

		private IObservable<string> GetStreamCore()
		{
			return Observable.Create<string>(async (observer, ct) =>
			{
				try
				{
					var token = this.Account.CurrentToken;
					var handler = new HttpClientHandler();
					var profile = TwitterClient.Current.CurrentNetworkProfile;
					if (profile != null && profile.Proxy != null) handler.Proxy = profile.Proxy.GetProxy();

					var client = OAuthUtility.CreateOAuthClient(
						handler,
						token.Application.ConsumerKey,
						token.Application.ConsumerSecret,
						new AccessToken(token.TokenKey, token.TokenSecret));
					client.Timeout = Timeout.InfiniteTimeSpan;
					var url = token.Endpoints[endpoint].Definition.Url;

					using (var stream = await client.GetStreamAsync(url))
					using (var sr = new StreamReader(stream))
					{
						while (!sr.EndOfStream && !ct.IsCancellationRequested)
						{
							var s = await sr.ReadLineAsync();
							observer.OnNext(s);
						}
					}
				}
				catch (Exception ex)
				{
					observer.OnError(ex);
					return;
				}
				if (!ct.IsCancellationRequested)
				{
					observer.OnCompleted();
				}
			});
		}
	}
}
