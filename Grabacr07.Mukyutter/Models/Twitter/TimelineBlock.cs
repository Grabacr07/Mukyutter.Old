using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Settings;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Filters;
using Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.Models.Twitter
{
	public class TimelineBlock : UpdatableBlock, IDisposable
	{
		// ToDo: フィルター作成処理を外出し

		private List<List> targets; // 購読対象リストを保持しておくもの
		private IDisposable listener;
		private QueryFilter currentFilter;
		private readonly Subject<string> queryReader = new Subject<string>();

		public Timeline Timeline { get; private set; }

		public override bool IsSelected
		{
			set { base.IsSelected = this.Timeline.IsReading = value; }
		}

		/// <summary>
		/// すべてのツイートを受信するかどうかを示す値を取得します。受信対象リストが設定されている場合、このプロパティは false を返します。
		/// </summary>
		public bool IsReceivingAll
		{
			get { return this.targets == null || this.targets.IsEmpty(); }
		}

		#region FilterQuery 変更通知 (するかもしれない) プロパティ

		private string _FilterQuery;

		public string FilterQuery
		{
			get { return this._FilterQuery; }
			set
			{
				if (this._FilterQuery != value)
				{
					this._FilterQuery = value;
					this.queryReader.OnNext(value);
				}
			}
		}

		#endregion

		#region FilterMessage 変更通知プロパティ

		private string _FilterMessage;

		public string FilterMessage
		{
			get { return this._FilterMessage; }
			set
			{
				if (this._FilterMessage != value)
				{
					this._FilterMessage = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CanCreateFilter 変更通知プロパティ

		private bool _CanCreateFilter;

		public bool CanCreateFilter
		{
			get { return this._CanCreateFilter; }
			set
			{
				if (this._CanCreateFilter != value)
				{
					this._CanCreateFilter = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region UnreadCount 変更通知プロパティ

		public int UnreadCount
		{
			get { return this.IsUnreadCountDisplaying ? this.Timeline.UnreadCount : 0; }
		}

		#endregion

		#region IsUnreadCountDisplaying 変更通知プロパティ

		private bool _IsUnreadCountDisplaying;

		public bool IsUnreadCountDisplaying
		{
			get { return this._IsUnreadCountDisplaying; }
			set
			{
				if (this._IsUnreadCountDisplaying != value)
				{
					this._IsUnreadCountDisplaying = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("UnreadCount");
				}
			}
		}

		#endregion

		#region IsNotified 変更通知プロパティ

		private bool _IsNotified;

		public bool IsNotified
		{
			get { return this._IsNotified; }
			set
			{
				if (this._IsNotified != value)
				{
					this._IsNotified = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		public TimelineBlock()
		{
			// 主に、初回起動時、タブ手動追加時にこっちのコンストラクターが呼ばれる
			this.Initialize();
		}

		public TimelineBlock(TimelineBlockSettings settings)
			: base(settings.Account, settings)
		{
			this.IsUnreadCountDisplaying = settings.IsUnreadCountDisplaying;
			this.IsNotified = settings.IsNotified;

			// 主に、通常起動時にこっちのコンストラクターが呼ばれる
			this.Initialize(settings.FilterQuery,
				settings.SubscribedLists.Select(s => Tuple.Create(s.Id, s.OwnerId, s.Name)).ToList());
		}

		private void Initialize(string query = "", List<Tuple<ListId, UserId, string>> listIds = null)
		{
			if (listIds != null && listIds.Any())
			{
				this.SetLists(listIds.Select(key => TwitterClient.Current.Lists.Add(key.Item1, key.Item2, key.Item3)).ToList());
			}

			this.Timeline = new Timeline { SubscribedLists = this.targets };
			this.listener = new PropertyChangedEventListener(this.Timeline)
			{
				(sender, e) => this.RaisePropertyChanged(e.PropertyName),
			};

			this.ChangeFilter(this.CreateFilter(query));
			this._FilterQuery = query;

			this.queryReader
				.Do(q => this.FilterMessage = "")
				.Do(q => this.CanCreateFilter = string.IsNullOrWhiteSpace(q))
				.Throttle(TimeSpan.FromMilliseconds(1000))
				.Select(this.CreateFilter)
				.Where(f => f != null)
				.Do(_ => this.FilterMessage = "フィルターが作成されました。")
				.Do(_ => this.CanCreateFilter = true)
				.Subscribe(f => { });
		}

		#region Filter 関連メソッド

		public bool ApplyFilter()
		{
			if (string.IsNullOrWhiteSpace(this.FilterQuery))
			{
				this.ChangeFilter();
				return true;
			}

			var filter = this.CreateFilter(this.FilterQuery);
			if (filter != null)
			{
				this.ChangeFilter(filter);
				this.ClearFilter();
				return true;
			}
			return false;
		}

		public void ClearFilter()
		{
			this.FilterQuery = (this.currentFilter == null) ? "" : this.currentFilter.Query;
			this.RaisePropertyChanged("FilterQuery");
			this.CanCreateFilter = false;
			this.FilterMessage = "";
		}

		private QueryFilter CreateFilter(string query)
		{
			if (string.IsNullOrWhiteSpace(query)) return null;

			try
			{
				return QueryFilter.Create(query, this.Account);
			}
			catch (Exception ex)
			{
				this.FilterMessage = ex.Message;
				return null;
			}
		}

		public async void ChangeFilter(QueryFilter filter = null)
		{
			this.currentFilter = filter;
			await this.Timeline.Initialize(filter);
		}

		#endregion

		#region List 関連メソッド

		/// <summary>
		///     タイムラインに受信対象リストを設定します。
		/// </summary>
		/// <param name="lists">受信対象リストのコレクション。すべてのツイートを受信する設定に戻す場合は null。</param>
		public void SetLists(ICollection<List> lists)
		{
			if (lists == null || lists.IsEmpty())
			{
				this.targets = null;
				if (this.Timeline != null)
				{
					this.Timeline.SubscribedLists = null;
				}
			}
			else
			{
				this.targets = lists as List<List> ?? lists.ToList();
				this.targets.ForEach(
					l => Helper.Operation(l.UpdateMembers, "リスト '{0}' のメンバーを取得できませんでした。", l.HasDetails ? l.FullName : "id:" + l.Id));

				if (this.Timeline != null)
				{
					this.Timeline.SubscribedLists = this.targets;
				}
			}

			this.RaisePropertyChanged("IsReceivingAll");
		}

		#endregion

		public override BlockSettings ToSettings()
		{
			return new TimelineBlockSettings
			{
				Account = this.AccountId,
				Name = this.Name,
				FilterQuery = this.currentFilter == null ? "" : this.currentFilter.Query,
				SubscribedLists = this.Timeline.SubscribedLists
					.Select(l => new ListSettings { Id = l.Id, OwnerId = l.OwnerId, Name = l.FullName, })
					.ToList(),
				IsUnreadCountDisplaying = this.IsUnreadCountDisplaying,
				IsNotified = IsNotified,
			};
		}

		#region Disposable pattern methods

		~TimelineBlock()
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
				this.Timeline.SafeDispose();
			}

			// Clean up all native resources
		}

		#endregion
	}
}
