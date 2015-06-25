using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Entity;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public class DirectMessageStore : StoreBase
	{
		private TwitterClient client;
		private Dictionary<StatusId, DirectMessage> statuses;

		#region Added event

		public event EventHandler<DirectMessageAddedEventArgs> Added;

		private void RaiseAdded(DirectMessage dm)
		{
			if (this.Added != null) this.Added(this, new DirectMessageAddedEventArgs(dm));
		}

		#endregion


		internal DirectMessageStore(TwitterClient client)
		{
			this.client = client;
			this.statuses = new Dictionary<StatusId, DirectMessage>();
		}


		#region read lock actions

		public DirectMessage this[StatusId id]
		{
			get { return this.DoReadLockAction(() => statuses.ContainsKey(id) ? statuses[id] : null); }
		}

		public bool Contains(StatusId id)
		{
			return this.DoReadLockAction(() => this.statuses.ContainsKey(id));
		}

		/// <summary>
		/// 指定した条件を満たすダイレクト メッセージのコレクションを取得します。
		/// </summary>
		/// <param name="predicate">ツイートが条件を満たすかどうかを確認するメソッド。</param>
		/// <returns><paramref name="predicate"/> で指定した条件を満たすツイートのコレクション。</returns>
		public DirectMessageCollection Get(Func<DirectMessage, bool> predicate)
		{
			if (predicate == null)
			{
				return this.DoReadLockAction(() => new DirectMessageCollection(this.statuses.Values));
			}
			else
			{
				return this.DoReadLockAction(() => new DirectMessageCollection(this.statuses.Values.Where(predicate)));
			}
		}

		#endregion

		#region parse status

		/// <summary>
		/// json 形式の文字列を Status オブジェクトに変換することを試みます。
		/// </summary>
		/// <param name="json">変換する json 形式の文字列。</param>
		/// <param name="status">変換に成功した場合、変換された Status オブジェクトが格納されます。変換に失敗した場合は null が格納されます。</param>
		/// <returns>変換に成功した場合は true、それ以外の場合は false。</returns>
		internal bool TryParse(dynamic json, out DirectMessage status)
		{
			try
			{
				status = this.Parse(json);
			}
			catch (TwitterException ex)
			{
				status = null;
				ex.Write();
			}

			return status != null;
		}

		/// <summary>
		/// json 形式の文字列を Status オブジェクトに変換します。
		/// </summary>
		/// <exception cref="JsonParseExceptoin">Status への変換に失敗した場合。</exception>
		internal DirectMessage Parse(dynamic djson)
		{
			var after = new List<Action>();	// ロック区間外で実行したいものたち
			DirectMessage result;

			var id = (StatusId)this.ParseId(djson);
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
						result = this.ParseCore(id, djson, after);
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


		private StatusId ParseId(dynamic djson)
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


		private DirectMessage ParseCore(StatusId id, dynamic djson, List<Action> after)
		{
			#region DEBUG build only
#if DEBUG
			after.Add(() => JsonMonitor.DirectMessages.Data.Add(djson.ToString()));
#endif
			#endregion

			// ============================================================================================
			// || このメソッド内での一切のロック操作 (またはロック操作を行うメソッドの呼び出し) を禁止します。 ||
			// ============================================================================================

			var result = new DirectMessage
			{
				Id = id,
				Text = djson.text,
				CreatedAt = Helper.ToDateTime(djson.created_at),
			};

			// ユーザー情報なし版の json に対応する？ めんどうだから後だ！！
			result.Recipient = TwitterClient.Current.Users.Parse(djson.recipient);
			result.Sender = TwitterClient.Current.Users.Parse(djson.sender);

			if (djson.IsDefined("entities"))
			{
				result.Entities = Entities.ParseCore(djson.entities);
			}

			this.statuses.Add(result.Id, result);

			after.Add(() => this.RaiseAdded(result));

			return result;
		}

		#endregion
	}
}
