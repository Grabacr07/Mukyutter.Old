using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Notifications;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public sealed class UserStore : StoreBase
	{
		private Dictionary<UserId, User> users;


		internal UserStore()
		{
			this.users = new Dictionary<UserId, User>();
		}

		#region read lock actions

		/// <summary>
		/// 指定した User ID に関連付けられているユーザー情報を取得または設定します。
		/// </summary>
		/// <param name="id">取得または設定するユーザー情報の User ID。</param>
		public User this[UserId id]
		{
			get { return this.DoReadLockAction(() => this.users.ContainsKey(id) ? this.users[id] : null); }
		}

		/// <summary>
		/// 指定したユーザー表示名に関連付けられているユーザー情報を取得します。
		/// </summary>
		/// <param name="screenName">取得するユーザーのユーザー表示名。</param>
		/// <returns></returns>
		public User this[ScreenName screenName]
		{
			get { return this.DoReadLockAction(() => this.users.Values.FirstOrDefault(user => user.ScreenName == screenName)); }
		}


		/// <summary>
		/// 指定した User ID のデータがキャッシュに含まれているかどうかを判断します。
		/// </summary>
		/// <param name="id">キャッシュ内で検索される User ID。</param>
		/// <returns>指定した User ID がキャッシュ内に格納されている場合は true、それ以外は false。</returns>
		public bool Contains(UserId id)
		{
			return this.DoReadLockAction(() => this.users.ContainsKey(id));
		}


		/// <summary>
		/// 指定した文字列を含むユーザーのコレクションを返します。
		/// </summary>
		/// <param name="searchStr">検索するユーザー名の一部または全部。</param>
		/// <returns>
		///     <paramref name="searchStr" /> を含むユーザー名のコレクション。
		/// </returns>
		public IEnumerable<User> Search(string searchStr)
		{
			return this.DoReadLockAction(() => this.users.Values.Where(user => user.ScreenName.Value.Contains(searchStr)));
		}

		#endregion

		#region parse user

		/// <summary>
		/// json 形式の文字列を User オブジェクトに変換することを試みます。
		/// </summary>
		/// <param name="json">変換する json 形式の文字列。</param>
		/// <param name="user">変換に成功した場合、変換された User オブジェクトが格納されます。変換に失敗した場合は null が格納されます。</param>
		/// <returns>変換に成功した場合は true、それ以外の場合は false。</returns>
		internal bool TryParse(dynamic json, out User user)
		{
			try
			{
				user = this.Parse(json);
			}
			catch (TwitterException ex)
			{
				user = null;
				ex.Write();
			}

			return user != null;
		}

		/// <summary>
		/// json 形式の文字列を User オブジェクトに変換します。
		/// </summary>
		/// <exception cref="JsonParseException">Status への変換に失敗した場合。</exception>
		internal User Parse(dynamic json)
		{
			var after = new List<Action>(); // ロック区間外で実行したいものたち
			User result;

			var id = (UserId)this.ParseId(json);
			try
			{
				this.lockslim.EnterUpgradeableReadLock();

				// アップグレード可能モードでロック
				// 読み取りモードでユーザーが存在しているかチェック
				var contains = this.users.ContainsKey(id);
				if (contains)
				{
					// 既に取得済みのユーザーの場合は、読み取り専用モードのままそのユーザーを返す
					result = this.users[id];
				}
				else
				{
					#region DEBUG build only

#if DEBUG
					after.Add(() => JsonMonitor.Users.Data.Add(json.ToString()));
#endif

					#endregion

					// 未取得のユーザーの場合、ロックを書き込みモードにアップグレード
					try
					{
						this.lockslim.EnterWriteLock();

						// 新しいユーザー情報の場合、新しいオブジェクトを作成
						result = new User { Id = id };

						this.users.Add(result.Id, result);
					}
					catch (Exception ex)
					{
						throw new JsonParseException(json.ToString(), typeof(User), ex);
					}
					finally
					{
						if (this.lockslim.IsWriteLockHeld) this.lockslim.ExitWriteLock();
					}
				}

				// trim=true で取得した json は id しか入っていないので、
				// name が定義されていなければ飛ばす
				if (json.IsDefined("name"))
				{
					result.Name = ((string)json.name).DecodeCER();
					result.ScreenName = new ScreenName(json.screen_name);

					// 更に、user_mention から取得した json には name と screen_name しか入っていないので、
					// created_at が定義されていなければ以降は飛ばす
					if (json.IsDefined("created_at"))
					{
						result.CreatedAt = Helper.ToDateTime(json.created_at);
						result.Location = json.location;
						result.Verified = json.verified;
						result.UtcOffset = Convert.ToInt64(json.utc_offset);
						result.Description = ((string)json.description).DecodeCER();
						result.Protected = json.@protected;
						result.ProfileImageUrl = Helper.ToUri(json.profile_image_url);
						result.StatusesCount = Convert.ToInt32(json.statuses_count);
						result.FriendsCount = Convert.ToInt32(json.friends_count);
						result.FollowersCount = Convert.ToInt32(json.followers_count);
						result.FavoritesCount = Convert.ToInt32(json.favourites_count);
						result.ListedCount = Convert.ToInt32(json.listed_count);
						result.Url = json.url;
					}
				}
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json.ToString(), typeof(User), ex);
			}
			finally
			{
				if (this.lockslim.IsUpgradeableReadLockHeld) this.lockslim.ExitUpgradeableReadLock();
			}

			after.ForEach(a => a());

			return result;
		}

		private UserId ParseId(dynamic djson)
		{
			try
			{
				return UserId.Parse(djson.id_str);
			}
			catch (Exception ex)
			{
				throw new JsonParseException(djson.ToString(), typeof(UserId), ex);
			}
		}

		#endregion
	}
}
