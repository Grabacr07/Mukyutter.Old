using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Filters;
using Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters;
using Grabacr07.Mukyutter.Models.Twitter.Data.Stores;
using Grabacr07.Utilities;
using Livet;
using Livet.EventListeners;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	public class Timeline : NotificationObject, IDisposable
	{
		private readonly IDisposable listener;

		public TimelineFilter Filter { get; private set; }

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

		#region SubscribedList 変更通知プロパティ

		private List<List> targets;
		private static readonly List<List> empty = new List<List>();

		/// <summary>
		/// このタイムラインが、すべてのツイートを受信する既定の動作を行う場合、このプロパティに null を指定してください (このとき、このプロパティは空のコレクションを返します)。
		/// 特定のリストからのツイートのみを受信する場合は、そのリストのコレクションを取得または設定します。
		/// </summary>
		public IReadOnlyCollection<List> SubscribedLists
		{
			get { return this.targets ?? empty; }
			set
			{
				this.targets = value == null ? null : value.ToList();
				this.RaisePropertyChanged();
			}
		}

		#endregion


		public Timeline()
		{
			this.SubscribedLists = null;
			this.Statuses = new ObservableSynchronizedCollection<Status>();

			this.listener = TwitterClient.Current.Statuses.Sequence.Subscribe(x => this.Add(x.Status, x.Source));
		}


		public Task Initialize(QueryFilter filter = null)
		{
			return Task.Factory.StartNew(() => TwitterClient.Current.Statuses.DoReadLockAction(() =>
			{
				// ロック中はステータスの追加が発生しないので、この間にコレクションのインスタンスを作り直す
				this.Filter = filter;
				this.Statuses = new ObservableSynchronizedCollection<Status>(
					TwitterClient.Current.Statuses.Get(this.Predicate));
				this.UnreadCount = this.Statuses.Count;
			}));
		}

		private void Add(Status status, StatusSource source)
		{
			// フィルター処理
			if (this.Predicate(status))
			{
				this.Statuses.Add(status);

				if (!status.User.IsSelf &&
					!source.HasFlag(StatusSource.Update) &&
					!source.HasFlag(StatusSource.Startup))
				{
					this.UnreadCount++;
				}
			}
		}

		private bool Predicate(Status status)
		{
			if (this.SubscribedLists.Any())
			{
				// 受信対象リストがある場合は、リスト対象ユーザーかどうかを確認し、更にフィルター処理
				return this.SubscribedLists.Any(list => list.Contains(status)) &&
					   (this.Filter == null || this.Filter.Predicate(status));
			}

			// 受信対象リストがない場合は、フィルター処理のみ
			return this.Filter == null || this.Filter.Predicate(status);
		}

		#region Disposable pattern methods

		~Timeline()
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
