using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Filters;
using Grabacr07.Mukyutter.Models.Twitter.Data.Stores;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	/// <summary>
	/// Twitter 上での会話ツリーを表します。
	/// </summary>
	public class Conversation : NotificationObject, IDisposable
	{
		private readonly IDisposable listener;

		#region Statuses 変更通知プロパティ

		private ObservableSynchronizedCollection<Status> _Statuses;

		public ObservableSynchronizedCollection<Status> Statuses
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

		#region IsReading 変更通知プロパティ

		private bool _IsReading;

		/// <summary>
		/// このタイムラインを読んでいるかどうかを取得または設定します。
		/// </summary>
		/// <remarks>
		/// このプロパティ値が false の間にタイムラインに追加されたステータスの数は UnreadCount プロパティから取得できます。
		/// このプロパティ値が true の間は、UnreadCount プロパティの値は常に 0 です。
		/// </remarks>
		public bool IsReading
		{
			get { return this._IsReading; }
			set
			{
				if (this._IsReading != value)
				{
					if (value) this.UnreadCount = 0;
					this._IsReading = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region UnreadCount 変更通知プロパティ

		private int _UnreadCount;
		private readonly object syncUnread = new object();

		/// <summary>
		/// 未読ステータス数を取得します。
		/// </summary>
		public int UnreadCount
		{
			get
			{
				lock (syncUnread)
				{
					return this._UnreadCount;
				}
			}
			private set
			{
				lock (syncUnread)
				{
					if (this._UnreadCount != value && !this.IsReading)
					{
						this._UnreadCount = value;
						this.RaisePropertyChanged();
					}
				}
			}
		}

		#endregion


		public Conversation()
		{
			this.Statuses = new ObservableSynchronizedCollection<Status>();

			this.listener = TwitterClient.Current.Statuses.Sequence
				.Where(x => !x.Source.HasFlag(StatusSource.Startup))
				.Where(x => !x.Source.HasFlag(StatusSource.Update))
				.Select(x => x.Status)
				.Subscribe(s => this.Add(true, s));
		}


		public Task Initialize(Status root)
		{
			this.Statuses = new ObservableSynchronizedCollection<Status> { root };
			this.UnreadCount = 1;

			return Task.Factory.StartNew(() => TwitterClient.Current.Statuses.DoReadLockAction(
				() => TwitterClient.Current.Statuses.Get(root)
								   .Where(s => s.Id != root.Id)
								   .ForEach(s => this.Add(false, s))));
		}

		private void Add(bool check, Status status)
		{
			// Predicate でチェックする場合は check = true, 
			// すべて素通りで追加する場合は check = false
			if (!check || this.Predicate(status))
			{
				this.Statuses.Add(status);
				this.UnreadCount++;
			}
		}

		private bool Predicate(Status target)
		{
			return this.Statuses.Any(source => PredicateCore(source, target));
		}

		private bool PredicateCore(Status source, Status target)
		{
			return (source.InReplyToStatusId.HasValue &&
					source.InReplyToStatusId.Value == target.Id) ||
				   (target.InReplyToStatusId.HasValue &&
					target.InReplyToStatusId.Value == source.Id) ||
				   source.ReplyFrom.Any(s => s.Id == target.Id) ||
				   target.ReplyFrom.Any(s => s.Id == source.Id);
		}


		#region Disposable pattern methods

		~Conversation()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Clean up all managed resources
				this.listener.SafeDispose();
			}

			// Clean up all native resources
		}

		#endregion
	}
}
