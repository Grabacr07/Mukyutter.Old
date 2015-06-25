using System;
using System.Collections.Generic;
using System.Linq;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	/// <summary>
	/// Twitter のユーザー情報を表します。
	/// </summary>
	[Serializable]
	public class User : NotificationObject
	{
		/// <summary>
		/// ユーザー ID を取得または設定します。
		/// </summary>
		public UserId Id { get; set; }

		/// <summary>
		/// ユーザーのアカウント作成日時を取得または設定します。
		/// </summary>
		public DateTime CreatedAt { get; set; }

		#region Name 変更通知プロパティ

		private string _Name;

		/// <summary>
		/// ユーザー名を取得または設定します。
		/// </summary>
		public string Name
		{
			get { return this._Name; }
			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ScreenName 変更通知プロパティ

		private ScreenName _ScreenName;

		/// <summary>
		/// ユーザー表示名 (ログイン時のユーザー名) を取得または設定します。
		/// </summary>
		public ScreenName ScreenName
		{
			get { return this._ScreenName; }
			set
			{
				if (this._ScreenName != value)
				{
					this._ScreenName = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("HomeUrl");
				}
			}
		}

		#endregion

		#region Location 変更通知プロパティ

		private string _Location;

		/// <summary>
		/// ユーザーの現在地を取得または設定します。
		/// </summary>
		public string Location
		{
			get { return this._Location; }
			set
			{
				if (this._Location != value)
				{
					this._Location = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Description 変更通知プロパティ

		private string _Description;

		/// <summary>
		/// ユーザーの自己紹介文を取得または設定します。
		/// </summary>
		public string Description
		{
			get { return this._Description; }
			set
			{
				if (this._Description != value)
				{
					this._Description = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ProfileImageUrl 変更通知プロパティ

		private Uri _ProfileImageUrl;

		/// <summary>
		/// ユーザーの画像 URL を取得または設定します。
		/// </summary>
		public Uri ProfileImageUrl
		{
			get { return this._ProfileImageUrl; }
			set
			{
				if (this._ProfileImageUrl != value)
				{
					this._ProfileImageUrl = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("ReasonablyProfileImageUrl");
				}
			}
		}

		#endregion

		#region Url 変更通知プロパティ

		private string _Url;

		/// <summary>
		/// ユーザーの URL を取得または設定します。
		/// </summary>
		public string Url
		{
			get { return this._Url; }
			set
			{
				if (this._Url != value)
				{
					this._Url = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Protected 変更通知プロパティ

		private bool _Protected;

		/// <summary>
		/// ユーザーがツイートを非公開にしているかどうかを示す値を取得または設定します。
		/// </summary>
		public bool Protected
		{
			get { return this._Protected; }
			set
			{
				if (this._Protected != value)
				{
					this._Protected = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region FollowersCount 変更通知プロパティ

		private int _FollowersCount;

		/// <summary>
		/// ユーザーのフォロワー数を取得または設定します。
		/// </summary>
		public int FollowersCount
		{
			get { return this._FollowersCount; }
			set
			{
				if (this._FollowersCount != value)
				{
					this._FollowersCount = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region FriendsCount 変更通知プロパティ

		private int _FriendsCount;

		/// <summary>
		/// ユーザーのフォロー数を取得または設定します。
		/// </summary>
		public int FriendsCount
		{
			get { return this._FriendsCount; }
			set
			{
				if (this._FriendsCount != value)
				{
					this._FriendsCount = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region FavouritesCount 変更通知プロパティ

		private int _FavoritesCount;

		/// <summary>
		/// ユーザーのお気に入り数を取得または設定します。
		/// </summary>
		public int FavoritesCount
		{
			get { return this._FavoritesCount; }
			set
			{
				if (this._FavoritesCount != value)
				{
					this._FavoritesCount = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region UtcOffset 変更通知プロパティ

		private long _UtcOffset;

		/// <summary>
		/// ユーザーのタイムゾーンの UTC との差を秒数で取得または設定します。
		/// </summary>
		public long UtcOffset
		{
			get { return this._UtcOffset; }
			set
			{
				if (this._UtcOffset != value)
				{
					this._UtcOffset = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region TimeZone 変更通知プロパティ

		private string _TimeZone;

		/// <summary>
		/// ユーザーのタイムゾーンを取得または設定します。
		/// </summary>
		public string TimeZone
		{
			get { return this._TimeZone; }
			set
			{
				if (this._TimeZone != value)
				{
					this._TimeZone = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Verified 変更通知プロパティ

		private bool _Verified;

		/// <summary>
		/// 認証済みユーザーかどうかを示す値を取得または設定します。
		/// </summary>
		public bool Verified
		{
			get { return this._Verified; }
			set
			{
				if (this._Verified != value)
				{
					this._Verified = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region StatusesCount 変更通知プロパティ

		private int _StatusesCount;

		/// <summary>
		/// このユーザーのツイート数を取得または設定します。
		/// </summary>
		public int StatusesCount
		{
			get { return this._StatusesCount; }
			set
			{
				if (this._StatusesCount != value)
				{
					this._StatusesCount = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ListedCount 変更通知プロパティ

		private int _ListedCount;

		/// <summary>
		/// このユーザーをフォローしているリストの数を取得または設定します。
		/// </summary>
		public int ListedCount
		{
			get { return this._ListedCount; }
			set
			{
				if (this._ListedCount != value)
				{
					this._ListedCount = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsSelf 変更通知プロパティ

		private bool _IsSelf;

		/// <summary>
		/// このユーザーが、現在のクライアント利用ユーザーかどうかを示す値を取得します。
		/// </summary>
		public bool IsSelf
		{
			get { return this._IsSelf; }
			internal set
			{
				if (this._IsSelf != value)
				{
					this._IsSelf = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		/// <summary>
		/// ユーザーのプロフィール用画像 (128px x 128px) の URL を取得します。
		/// </summary>
		public Uri ReasonablyProfileImageUrl
		{
			get { return new Uri(this.ProfileImageUrl.ToString().Replace("_normal", "_reasonably_small")); }
		}

		/// <summary>
		/// このユーザーのホーム画面の URL を取得します。このプロパティが表す値は、Twitter API からの応答に含まれません (このアプリケーションの独自の機能です)。
		/// </summary>
		public Uri HomeUrl
		{
			get { return UrlHelper.GetUserHomeUrl(this.ScreenName); }
		}


		public override string ToString()
		{
			return "{{{0} ({1})}}".SafeFormatting(this.ScreenName.ValueWithAtmark, this.Name);
		}


		public static readonly User Empty = new User
		{
			Id = 0,
			ScreenName = new ScreenName("(empty)"),
			CreatedAt = CommonDefinitions.UnixEpoch,
		};


		/// <summary>
		/// Twitter のユーザー情報を格納する json 文字列から、User オブジェクトに変換します。
		/// </summary>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Net.ApiException">Twitter API がエラーを返した場合。</exception>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Data.Json.JsonParseException">json から user への変換に失敗した場合。</exception>
		public static User Parse(string json)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			DynamicJsonHelper.ThrowIfError(djson);

			return ParseCore(djson);
		}

		internal static User ParseCore(dynamic djson)
		{
			return TwitterClient.Current.Users.Parse(djson);
		}
	}
}
