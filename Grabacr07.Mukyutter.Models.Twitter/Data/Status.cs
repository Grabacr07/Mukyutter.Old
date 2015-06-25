using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Data.Stores;
using Grabacr07.Utilities;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	/// <summary>
	/// Twitter ステータスを表します。
	/// </summary>
	[Serializable]
	[DebuggerDisplay("[ID:{Id}, CreateAt:{CreatedAt}, Text:{Text}, User:{User}]")]
	public class Status : StatusBase, IComparable<Status>
	{
		/// <summary>
		///     <see cref="T:Grabacr07.Mukyutter.Models.Twitter.Data.Status" />
		/// クラスの新しいインスタンスを初期化します。
		/// </summary>
		public Status()
		{
			this.RetweetUsers = new ObservableSynchronizedCollection<User>();
			this.FavoriteUsers = new ObservableSynchronizedCollection<User>();
			this.ReplyFrom = new ObservableSynchronizedCollection<Status>();
		}

		/// <summary>
		/// ツイートしたユーザーを取得します。
		/// </summary>
		public User User { get; set; }

		/// <summary>
		/// 投稿に使用されたクライアントを取得または設定します。
		/// </summary>
		public Source Source { get; set; }

		/// <summary>
		/// ツイートが (140 字を超えているため) 省略されているかどうかを示す値を取得または設定します。
		/// </summary>
		public bool Truncated { get; set; }

		#region In reply to

		/// <summary>
		/// 返信元のツイートのステータス ID を取得または設定します。
		/// </summary>
		public StatusId? InReplyToStatusId { get; set; }

		//public UserId? InReplyToUserId { get; set; }
		//dpublic string InReplyToScreenName { get; set; }
		/// <summary>
		/// 返信元のユーザーのユーザー ID を取得または設定します。
		/// </summary>
		/// <summary>
		/// 返信元のユーザー名を取得または設定します。
		/// </summary>
		/// <summary>
		/// このツイートへの返信であるツイートのコレクションを取得します。
		/// </summary>
		public ObservableSynchronizedCollection<Status> ReplyFrom { get; private set; }

		#endregion

		#region Retweet

		/// <summary>
		/// このツイートが公式リツイートされたツイートかどうかを示す値を取得します。
		/// </summary>
		public bool IsRetweetStatus
		{
			get { return this.RetweetedStatus != null; }
		}

		/// <summary>
		/// リツイート元のステータスを取得します。
		/// </summary>
		/// <value>
		/// このツイートが公式リツイートである場合、リツイート元のステータスを格納します。
		/// それ以外の場合は null となります。
		/// </value>
		public Status RetweetedStatus { get; set; }

		/// <summary>
		/// 画面に表示されるべきステータスを取得します。
		/// </summary>
		/// <value>
		/// このツイートが公式リツイートである場合は <see cref="Grabacr07.Mukyutter.Models.Twitter.Data.Status.RetweetedStatus" />。それ以外の場合は、このインスタンス。
		/// </value>
		public Status DisplayStatus
		{
			get { return this.RetweetedStatus ?? this; }
		}

		#endregion

		#region Fav/RT

		/// <summary>
		/// お気に入りに設定されているかどうかを示す値を取得または設定します。
		/// </summary>
		public bool Favorited { get; set; }

		/// <summary>
		/// このツイートをお気に入りに追加しているユーザーのコレクションを取得します。
		/// </summary>
		public ObservableSynchronizedCollection<User> FavoriteUsers { get; private set; }

		/// <summary>
		/// このツイートをリツイートしたユーザーのコレクションを取得します。
		/// </summary>
		public ObservableSynchronizedCollection<User> RetweetUsers { get; private set; }

		#endregion

		#region IsDeleted 変更通知プロパティ

		private bool _IsDeleted;

		public bool IsDeleted
		{
			get { return this._IsDeleted; }
			set
			{
				if (this._IsDeleted != value)
				{
					this._IsDeleted = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		/// <summary>
		/// ステータスに、指定したユーザーへのメンションが含まれているかどうかを確認します。
		/// </summary>
		public bool IsMention(ScreenName screenName)
		{
			if (this.Entities != null && this.Entities.UserMentions != null)
			{
				return this.Entities.UserMentions.Any(um => um.User.ScreenName == screenName);
			}
			return false;
		}

		/// <summary>
		/// ステータスに、指定したユーザーへのメンションが含まれているかどうかを確認します。
		/// </summary>
		public bool IsMention(User user)
		{
			if (this.Entities != null && this.Entities.UserMentions != null)
			{
				return this.Entities.UserMentions.Any(um => um.Id == user.Id);
			}
			return false;
		}

		/// <summary>
		/// 指定したステータスとこのインスタンスを比較し、ステータス ID を元にしたこれらの相対値を示す値を返します。
		/// </summary>
		/// <param name="status">比較するオブジェクト。</param>
		/// <returns>ステータス ID を元にした相対値。</returns>
		public int CompareTo(Status status)
		{
			return this.Id.CompareTo(status.Id);
		}

		public override string ToString()
		{
			return "{{@{0}: {1}}}".SafeFormatting(this.User.ScreenName, this.Text);
		}

		#region parse methods

		/// <summary>
		/// Twitter ステータスを格納する json 文字列から、Status オブジェクトに変換します。
		/// </summary>
		/// <param name="json">ステータス情報を格納した json 文字列。</param>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Net.ApiException">Twitter API がエラーを返した場合。</exception>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Data.Json.JsonParseException">json から status への変換に失敗した場合。</exception>
		public static Status Parse(string json)
		{
			return Parse(json, StatusSource.RestApi);
		}

		/// <summary>
		/// Twitter ステータスを格納する json 文字列から、Status オブジェクトに変換します。
		/// </summary>
		/// <param name="json">ステータス情報を格納した json 文字列。</param>
		/// <param name="source">ステータスの取得先を示す識別子。既定値は StatusSource.RestApi です。</param>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Net.ApiException">Twitter API がエラーを返した場合。</exception>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Data.Json.JsonParseException">json から status への変換に失敗した場合。</exception>
		internal static Status Parse(string json, StatusSource source)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			DynamicJsonHelper.ThrowIfError(djson);

			return ParseCore(djson, source);
		}

		private static Status ParseCore(dynamic djson, StatusSource source)
		{
			return TwitterClient.Current.Statuses.Add(djson, source);
		}

		#endregion
	}
}
