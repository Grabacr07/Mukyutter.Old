using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Events;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Notifications;
using Grabacr07.Utilities.Development;
using Grabacr07.Utilities.Reactive;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public class _UserStreams : Connector
	{
		private const string endpoint = "userstream/user";

		private IDisposable connection;
		private bool isconnecting;

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
				else if (!this.UseUserStreams)
				{
					return UserStreamsStatus.Disabled;
				}
				else if (this.isconnecting)
				{
					return UserStreamsStatus.Connecting;
				}
				else if (!this.IsAvailable)
				{
					return UserStreamsStatus.Offline;
				}
				else
				{
					return UserStreamsStatus.Disconnected;
				}
			}
		}

		private void RaiseStatusChanged()
		{
			this.RaisePropertyChanged("Status");
		}

		#endregion

		public _UserStreams(TwitterAccount account)
			: base(account) { }


		public async Task Connect()
		{
			if (this.IsAvailable && this.UseUserStreams)
			{
				try
				{
					this.isconnecting = true;
					this.Disconnect(); // + RaiseStatusChanged()

					try
					{
						await this.Trial();
					}
					catch (Exception ex)
					{
						ex.Write();
						throw;
					}

					this.Streaming();
				}
				finally
				{
					this.isconnecting = false;
					this.RaiseStatusChanged();
				}
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
		}


		protected override async void ConnectCore()
		{
			base.ConnectCore();
			await this.Connect();
		}

		protected override void DisconnectCore()
		{
			base.DisconnectCore();
			this.Disconnect();
		}


		private void Streaming()
		{
			var client = this.Account.ToOAuthClient(endpoint);

			var connector = client.GetResponseLines()
				.Catch((Exception ex) => Observable.Throw<string>(new TwitterException("User streams 接続でエラーが発生しました。", ex)))
				.Where(json => !string.IsNullOrEmpty(json))
				.Select(json =>
				{
					#region Parse DynamicJson

					try
					{
						return DynamicJsonHelper.ToDynamicJson(json);
					}
					catch (Exception ex)
					{
						TwitterClient.Current.ReportException("User streams から認識できないデータを受信しました。", ex);
						return null;
					}

					#endregion
				})
				.Where(djson => djson != null)
				.Do(djson =>
				{
					DebugMonitor.WriteLine("stream: {0}", djson);
					#region DEBUG only

#if DEBUG
					JsonMonitor.UserStreams.Data.Add(djson.ToString());
#endif

					#endregion
				})
				.Publish();

			connector.Subscribe(
				_ => { },
				ex =>
				{
					TwitterClient.Current.ReportException("User streams 接続でエラーが発生しました。", ex);
					this.Disconnect();
				},
				this.Disconnect);

			connector.Take(1)
				.Select(djson => UserIdCollection.ParseCore(djson.friends))
				.Subscribe(friends => this.Account.Friends = friends);

			connector.Where(s => s.IsDefined("text") && s.IsDefined("user"))
				.Select(djson => TwitterClient.Current.Statuses.Add(djson))
				.OnErrorRetryFromStreaming("status")
				.Subscribe(status => TwitterClient.Current.AddStatus(status));

			connector.Where(djson => djson.IsDefined("delete"))
				.Select(djson => StatusId.Parse(djson.delete.status.id_str))
				.OnErrorRetryFromStreaming("delete")
				.Subscribe(id => TwitterClient.Current.RemoveStatus(id));

			connector.Where(djson => djson.IsDefined("direct_message"))
				.Select(djson => TwitterClient.Current.Messages.Parse(djson.direct_message))
				.OnErrorRetryFromStreaming("direct_message")
				.Subscribe();

			var eventConnector = connector.Where(djson => djson.IsDefined("event"))
				.Publish();

			eventConnector.Where(ev => (ev.@event == "favorite") || (ev.@event == "unfavorite"))
				.Select(fav => Favorite.ParseCore(fav))
				.OnErrorRetryFromStreaming("favorite")
				.Subscribe(fav => TwitterClient.Current.Favorite(fav));

			eventConnector.Where(ev => ev.@event == "follow")
				.Select(follow => Follow.ParseCore(follow))
				.OnErrorRetryFromStreaming("follow")
				.Subscribe(follow => TwitterClient.Current.Follow(follow));

			eventConnector.Connect();

			this.connection = connector.Connect();
		}
	}
}
