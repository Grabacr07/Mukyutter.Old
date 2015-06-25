using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Grabacr07.Mukyutter.Models.Twitter.Data.Entity;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Notifications;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public sealed class StatusStore : StoreBase
	{
		private static readonly IScheduler scheduler = new EventLoopScheduler();

		private readonly TwitterClient client;
		private readonly Dictionary<StatusId, Status> statuses;
		private readonly Subject<StatusStoreItem> statusSeq;

		/// <summary>
		/// ストアに Twitter ステータスが追加されると通知するプロバイダーを取得します。
		/// </summary>
		public IObservable<StatusStoreItem> Sequence
		{
			get { return this.statusSeq.ObserveOn(scheduler); }
		}

		public int Count { get; private set; }

		internal StatusStore(TwitterClient client)
		{
			this.client = client;
			this.statuses = new Dictionary<StatusId, Status>();
			this.statusSeq = new Subject<StatusStoreItem>();
		}

		#region read lock actions

		public Status this[StatusId id]
		{
			get { return this.DoReadLockAction(() => statuses.ContainsKey(id) ? statuses[id] : null); }
		}

		public bool Contains(StatusId id)
		{
			return this.DoReadLockAction(() => this.statuses.ContainsKey(id));
		}

		/// <summary>
		/// 指定した条件を満たすツイートのコレクションを取得します。
		/// </summary>
		/// <param name="predicate">ツイートが条件を満たすかどうかを確認するメソッド。すべてのツイートを取得する場合は null。</param>
		/// <returns>
		///     <paramref name="predicate" /> で指定した条件を満たすツイートのコレクション。
		/// </returns>
		public StatusCollection Get(Func<Status, bool> predicate)
		{
			return this.DoReadLockAction(() => new StatusCollection(predicate == null
				? this.statuses.Values
				: this.statuses.Values.Where(predicate)));
		}

		/// <summary>
		/// 指定したツイートを起点とした会話ツリーを取得します。
		/// </summary>
		/// <param name="root"></param>
		/// <returns></returns>
		public StatusCollection Get(Status root)
		{
			return this.DoReadLockAction(() =>
			{
				var list = new List<Status> { root };
				var current = root;

				// 起点より古いツイートの抽出 (in_reply_to を辿っていく)
				while (current.DisplayStatus.InReplyToStatusId.HasValue)
				{
					Status next;
					if (this.statuses.TryGetValue(current.DisplayStatus.InReplyToStatusId.Value, out next))
					{
						list.Add(next);
						current = next;
					}
					else break;
				}

				// 起点より新しいツイートの抽出 (ReplyFrom を使って逆方向へ辿る (再帰でツリーすべてをさらう感じ))
				Action<Status> recursion = null;
				recursion = status =>
				{
					status.DisplayStatus.ReplyFrom.ForEach(s => recursion(s));
					list.Add(status);
				};
				recursion(root);

				return new StatusCollection(list.OrderByDescending(s => s.Id));
			});
		}

		#endregion

		#region parse status

		/// <summary>
		/// json 形式の文字列から Status オブジェクトを追加することを試みます。
		/// </summary>
		/// <param name="json">追加するステータスの json 形式の文字列。</param>
		/// <param name="status">変換に成功した場合、変換された Status オブジェクトが格納されます。変換に失敗した場合は null が格納されます。</param>
		/// <param name="source">ステータスの取得先を示す識別子。</param>
		/// <returns>変換に成功した場合は true、それ以外の場合は false。</returns>
		internal bool TryAdd(dynamic json, StatusSource source, out Status status)
		{
			try
			{
				status = this.Add(json, source);
			}
			catch (TwitterException ex)
			{
				status = null;
				ex.Write();
			}

			return status != null;
		}

		/// <summary>
		/// json 形式の文字列から Status オブジェクトを追加します。
		/// </summary>
		/// <exception cref="JsonParseException">Status への変換に失敗した場合。</exception>
		internal Status Add(dynamic djson, StatusSource source)
		{
			var after = new List<Action>(); // ロック区間外で実行したいものたち
			Status result;

			var id = (StatusId)ParseId(djson);

			#region Test code

#if DEBUG
			if (id.ToString() != djson.id_str)
			{
				DebugMonitor.WriteLine("### Parsed status_id is aberrant value! ({0} -> {1})", id, djson.id_str);
			}
#endif

			#endregion

			try
			{
				this.lockslim.EnterUpgradeableReadLock();

				// アップグレード可能モードでロック
				// 読み取りモードでステータスが存在しているかチェック
				var contains = this.statuses.ContainsKey(id);
				if (contains)
				{
					// 既に取得済みのステータスの場合は、読み取りモードのままそのステータスを返す
					result = this.statuses[id];
				}
				else
				{
					// 未取得のステータスの場合、ロックを書き込みモードにアップグレード
					try
					{
						this.lockslim.EnterWriteLock();
						result = this.ParseCore(id, djson, source, after);
					}
					catch (Exception ex)
					{
						throw new JsonParseException(djson.ToString(), typeof(Status), ex);
					}
					finally
					{
						if (this.lockslim.IsWriteLockHeld) this.lockslim.ExitWriteLock();
					}
				}
			}
			finally
			{
				if (this.lockslim.IsUpgradeableReadLockHeld) this.lockslim.ExitUpgradeableReadLock();
			}

			after.ForEach(a => a());

			return result;
		}


		private static StatusId ParseId(dynamic djson)
		{
			try
			{
				return StatusId.Parse(djson.id_str);
			}
			catch (Exception ex)
			{
				throw new JsonParseException(djson.ToString(), typeof(StatusId), ex);
			}
		}


		/// <summary>
		/// json データを解析し Status オブジェクトを生成するコア メソッド。
		/// このメソッドの呼び出す前に、スレッドを書き込みモードでロックしてください。
		/// </summary>
		/// <remarks>
		/// このメソッド内での一切のロック操作 (またはロック操作を行うメソッドの呼び出し) を禁止します。
		/// </remarks>
		/// <returns></returns>
		private Status ParseCore(StatusId id, dynamic djson, StatusSource source, ICollection<Action> after)
		{
			#region DEBUG build only

#if DEBUG
			after.Add(() => JsonMonitor.Statuses.Data.Add(djson.ToString()));
#endif

			#endregion

			var result = new Status
			{
				Id = id,
				Text = (string)djson.text,
				Truncated = djson.truncated,
				CreatedAt = Helper.ToDateTime(djson.created_at),
				User = client.Users.Parse(djson.user),
			};

			#region source

			try
			{
				result.Source = Source.Parse(djson.source);
			}
			catch (JsonParseException ex)
			{
				after.Add(() => this.client.ReportException("ツイートの投稿に使用された source 情報を解析できませんでした。", ex));
				result.Source = Source.Default;
			}

			#endregion

			#region entities

			if (djson.IsDefined("entities"))
			{
				try
				{
					result.Entities = Entities.ParseCore(djson.entities);
				}
				catch (JsonParseException ex)
				{
					after.Add(() => this.client.ReportException("ツイートに含まれる entities を解析できませんでした。", ex));
					result.Entities = Entities.Default;
				}
			}
			else
			{
				result.Entities = Entities.Default;
			}

			#endregion

			#region retweeted_status

			if (djson.IsDefined("retweeted_status"))
			{
				var rtid = (StatusId)ParseId(djson.retweeted_status);
				var rtstatus = this.statuses.ContainsKey(rtid)
					? this.statuses[rtid]
					: (Status)this.ParseCore(rtid, djson.retweeted_status, source, after);

				rtstatus.RetweetUsers.Add(result.User);
				result.RetweetedStatus = rtstatus;
			}

			#endregion

			#region reply

			result.InReplyToStatusId = null;

			if (djson.IsDefined("in_reply_to_status_id_str") &&
				djson.in_reply_to_status_id_str != null)
			{
				var repId = StatusId.Parse(djson.in_reply_to_status_id_str);
				if (repId != 0)
				{
					result.InReplyToStatusId = repId;
					if (this.statuses.ContainsKey(repId))
					{
						this.statuses[repId].ReplyFrom.Add(result);
					}
				}
			}

			#endregion

			this.statuses.Add(result.Id, result);
			this.Count++;

			after.Add(() => this.statusSeq.OnNext(new StatusStoreItem { Status = result, Source = source }));

			return result;
		}

		#endregion
	}
}
