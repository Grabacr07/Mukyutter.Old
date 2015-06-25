using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public class ListStore : StoreBase
	{
		private TwitterClient client;

		// リストは、リスト ID とユーザー ID の複合キーで識別することにします。
		private Dictionary<Tuple<ListId, UserId>, List> lists;

		#region Added event

		public event EventHandler<ListAddedEventArgs> Added;

		private void RaiseAdded(List list)
		{
			if (this.Added != null) this.Added(this, new ListAddedEventArgs(list));
		}

		#endregion

		internal ListStore(TwitterClient client)
		{
			this.client = client;
			this.lists = new Dictionary<Tuple<ListId, UserId>, List>();
		}

		#region read lock actions

		public List this[ListId id, UserId ownerId]
		{
			get
			{
				var key = Tuple.Create(id, ownerId);
				return this.DoReadLockAction(() => this.lists.ContainsKey(key) ? this.lists[key] : null);
			}
		}

		public List this[string fullName]
		{
			get { return this.DoReadLockAction(() => this.lists.Values.FirstOrDefault(list => fullName.Compare(list.FullName))); }
		}

		public List this[string owner, string slug]
		{
			get { return this[new ScreenName(owner).ValueWithAtmark + "/" + slug]; }
		}

		/// <summary>
		/// 指定したユーザーのリスト
		/// </summary>
		/// <param name="ownerId"></param>
		/// <returns></returns>
		public IEnumerable<List> Get(UserId ownerId)
		{
			return this.DoReadLockAction(() => this.lists.Values.Where(l => l.OwnerId == ownerId));
		}

		#endregion

		#region read and lock actions

		public List Add(ListId id, UserId ownerId, string name = "")
		{
			var key = Tuple.Create(id, ownerId);
			var after = new List<Action>(); // ロック区間外で実行したいものたち
			List result;

			try
			{
				this.lockslim.EnterUpgradeableReadLock();

				// アップグレード可能モードでロック
				// 読み取りモードでリストが存在しているかチェック
				var contains = this.lists.ContainsKey(key);
				if (contains)
				{
					// 既に取得済みのリストの場合は、読み取り専用モードのままそのリストを返す
					result = this.lists[key];
				}
				else
				{
					// 未取得のリストの場合、ロックを書き込みモードにアップグレード
					try
					{
						this.lockslim.EnterWriteLock();

						// 新しいリスト情報の場合、新しいオブジェクトを作成
						result = new List(id, ownerId) { FullName = name };
						this.lists.Add(key, result);
						after.Add(() => this.RaiseAdded(result));
					}
					finally
					{
						if (this.lockslim.IsWriteLockHeld) this.lockslim.ExitWriteLock();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("リストの追加に失敗しました (id: {0})。", id), ex);
			}
			finally
			{
				if (this.lockslim.IsUpgradeableReadLockHeld) this.lockslim.ExitUpgradeableReadLock();
			}

			after.ForEach(a => a());

			return result;
		}

		#endregion

		#region parse list

		internal bool TryParse(dynamic json, UserId ownerId, out List list)
		{
			try
			{
				list = this.Parse(json, ownerId);
			}
			catch (TwitterException ex)
			{
				list = null;
				ex.Write();
			}

			return list != null;
		}

		internal List Parse(dynamic json, UserId ownerId)
		{
			var after = new List<Action>(); // ロック区間外で実行したいものたち
			List result;

			var id = (ListId)this.ParseId(json);
			var key = Tuple.Create(id, ownerId);
			try
			{
				this.lockslim.EnterUpgradeableReadLock();

				// アップグレード可能モードでロック
				// 読み取りモードでリストが存在しているかチェック
				var contains = this.lists.ContainsKey(key);
				if (contains)
				{
					// 既に取得済みのリストの場合は、読み取り専用モードのままそのリストを返す
					result = this.lists[key];
				}
				else
				{
					#region DEBUG build only

#if DEBUG
					after.Add(() => JsonMonitor.Lists.Data.Add(json.ToString()));
#endif

					#endregion

					// 未取得のリストの場合、ロックを書き込みモードにアップグレード
					try
					{
						this.lockslim.EnterWriteLock();

						// 新しいリスト情報の場合、新しいオブジェクトを作成
						result = new List(id, ownerId);
						this.lists.Add(key, result);
						after.Add(() => this.RaiseAdded(result));
					}
					catch (Exception ex)
					{
						throw new JsonParseException(json.ToString(), typeof (User), ex);
					}
					finally
					{
						if (this.lockslim.IsWriteLockHeld) this.lockslim.ExitWriteLock();
					}
				}

				if (!result.HasDetails)
				{
					// 未取得の場合のみ更新する情報
					result.CreatedAt = Helper.ToDateTime(json.created_at);
					result.User = this.client.Users.Parse(json.user);
				}

				// 取得済みかどうかに関わらず更新する情報
				result.Slug = json.slug;
				result.Name = json.name;
				result.FullName = json.full_name;
				result.Description = json.description;
				result.Uri = json.uri;
				result.Mode = json.mode;
				result.SubscriberCount = Convert.ToInt32(json.subscriber_count);
				result.MemberCount = Convert.ToInt32(json.member_count);
				result.HasDetails = true;
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json.ToString(), typeof (List), ex);
			}
			finally
			{
				if (this.lockslim.IsUpgradeableReadLockHeld) this.lockslim.ExitUpgradeableReadLock();
			}

			after.ForEach(a => a());

			return result;
		}

		private ListId ParseId(dynamic djson)
		{
			try
			{
				return ListId.Parse(djson.id_str);
			}
			catch (Exception ex)
			{
				throw new JsonParseException(djson.ToString(), typeof (ListId), ex);
			}
		}

		#endregion
	}
}
