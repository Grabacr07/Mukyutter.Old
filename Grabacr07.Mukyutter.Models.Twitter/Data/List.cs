using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Events;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	/// <summary>
	/// Twitter のリストを表します。
	/// </summary>
	/// <remarks>
	/// リストのインスタンスは、ID のみで成立します (Id プロパティのみ値を持ち、それ以外のメンバーが値を持たない状態は正常です)。
	/// これは、リストの ID さえ判明していれば、リストの詳細情報およびメンバーは後から取得できるためです。
	/// リストの詳細情報を取得したかどうかは、HasDetails プロパティの値で判別できます。
	/// 同様に、リストのメンバーを取得したかどうかを HasMembers プロパティの値で判別できます。
	/// </remarks>
	public class List : NotificationObject
	{
		public ListId Id { get; private set; }
		public UserId OwnerId { get; private set; }

		#region Owner 変更通知プロパティ

		private TwitterAccount _Owner;
		private IDisposable listener;

		/// <summary>
		/// アカウントが初めて設定されたとき (null の状態から、新しいインスタンスが設定されたとき) に 1 度だけ発生します。
		/// </summary>
		private event EventHandler<EventArgs<TwitterAccount>> AccountInitialized;

		/// <summary>
		/// このリストを所有しているアカウント (= 購読しているアカウント。リストの作者ではありません) を取得します。
		/// アカウントが読み込まれていない場合は null を返します。
		/// </summary>
		public virtual TwitterAccount Owner
		{
			get { return this._Owner; }
			private set
			{
				if (this._Owner != value)
				{
					this._Owner = value;
					if (this.listener != null && value != null)
					{
						this.listener.Dispose();
						this.listener = null;
					}
					if (this.AccountInitialized != null)
					{
						this.AccountInitialized(this, new EventArgs<TwitterAccount>(value));
					}
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CreatedAt 変更通知プロパティ

		private DateTime _CreatedAt;

		public DateTime CreatedAt
		{
			get { return this._CreatedAt; }
			set
			{
				if (this._CreatedAt != value)
				{
					this._CreatedAt = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region User 変更通知プロパティ

		private User _User;

		public User User
		{
			get { return this._User; }
			set
			{
				if (this._User != value)
				{
					this._User = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Slug 変更通知プロパティ

		private string _Slug;

		public string Slug
		{
			get { return this._Slug; }
			set
			{
				if (this._Slug != value)
				{
					this._Slug = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Name 変更通知プロパティ

		private string _Name;

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

		#region FullName 変更通知プロパティ

		private string _FullName;

		public string FullName
		{
			get { return this._FullName; }
			set
			{
				if (this._FullName != value)
				{
					this._FullName = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Description 変更通知プロパティ

		private string _Description;

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

		#region Uri 変更通知プロパティ

		private string _Uri;

		public string Uri
		{
			get { return this._Uri; }
			set
			{
				if (this._Uri != value)
				{
					this._Uri = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region SubscriberCount 変更通知プロパティ

		private int _SubscriberCount;

		public int SubscriberCount
		{
			get { return this._SubscriberCount; }
			set
			{
				if (this._SubscriberCount != value)
				{
					this._SubscriberCount = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region MemberCount 変更通知プロパティ

		private int _MemberCount;

		public int MemberCount
		{
			get { return this._MemberCount; }
			set
			{
				if (this._MemberCount != value)
				{
					this._MemberCount = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Mode 変更通知プロパティ

		private string _Mode;

		public string Mode
		{
			get { return this._Mode; }
			set
			{
				if (this._Mode != value)
				{
					this._Mode = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("IsPublic");
					this.RaisePropertyChanged("IsPrivate");
				}
			}
		}

		#endregion

		public bool IsPublic
		{
			get { return this.Mode.Compare("public"); }
		}

		public bool IsPrivate
		{
			get { return this.Mode.Compare("private"); }
		}

		#region Members 変更通知プロパティ

		private HashSet<UserId> _Members;
		private static readonly HashSet<UserId> empty = new HashSet<UserId>();

		public HashSet<UserId> Members
		{
			get { return this._Members ?? empty; }
			internal set
			{
				if (this._Members != value)
				{
					this._Members = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region HasDetails 変更通知プロパティ

		private bool _HasDetails;

		/// <summary>
		/// リストの詳細情報を取得済みかどうかを示す値を取得します。
		/// </summary>
		public bool HasDetails
		{
			get { return this._HasDetails; }
			internal set
			{
				if (this._HasDetails != value)
				{
					this._HasDetails = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region HasMembers 変更通知プロパティ

		private bool _HasMembers;

		/// <summary>
		/// リストのメンバー情報を取得済みかどうかを示す値を取得します。
		/// </summary>
		public bool HasMembers
		{
			get { return this._HasMembers; }
			internal set
			{
				if (this._HasMembers != value)
				{
					this._HasMembers = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public List(ListId id, UserId ownerId)
		{
			this.Id = id;
			this.OwnerId = ownerId;

			if (!this.TrySetAccount(ownerId))
			{
				this.listener = new CollectionChangedEventListener(
					TwitterClient.Current.Accounts,
					(sender, e) => this.TrySetAccount(ownerId));
			}
		}


		private bool TrySetAccount(UserId id)
		{
			if (this.Owner != null) return true;

			var owner = TwitterClient.Current.Accounts.FirstOrDefault(a => a.UserId == id);
			if (owner != null)
			{
				this.Owner = owner;
				return true;
			}
			return false;
		}


		/// <summary>
		/// リストの詳細情報を取得します。所有アカウントが読み込まれていない場合、アカウントの読み込み後に自動的に実行します。
		/// </summary>
		public Task<List> Update()
		{
			if (this.Owner == null)
			{
				return Observable.FromEventPattern<EventArgs<TwitterAccount>>(
					h => this.AccountInitialized += h,
					h => this.AccountInitialized -= h)
					.Take(1)
					.Timeout(TimeSpan.FromSeconds(60))
					.Select(e => e.EventArgs.Value)
					.SelectMany(account => account.ShowList(this.Id))
					.ToTask();
			}

			return this.Owner.ShowList(this.Id).ToTask();
		}

		/// <summary>
		/// リストのメンバーを取得します。所有アカウントが読み込まれていない場合、アカウントの読み込み後に自動的に実行します。
		/// </summary>
		public Task<ICollection<User>> UpdateMembers()
		{
			if (this.Owner == null)
			{
				return Observable.FromEventPattern<EventArgs<TwitterAccount>>(
					h => this.AccountInitialized += h,
					h => this.AccountInitialized -= h)
					.Take(1)
					.Timeout(TimeSpan.FromSeconds(60))
					.Select(e => e.EventArgs.Value)
					.SelectMany(account => account.GetListMembers(this.Id))
					.ToTask();
			}

			return this.Owner.GetListMembers(this.Id).ToTask();
		}


		/// <summary>
		/// 指定したユーザー ID のユーザーが、リストのメンバーに含まれているかどうかを確認します。
		/// </summary>
		public bool Contains(UserId userId)
		{
			return this.HasMembers && this.Members.Any(id => id == userId);
		}

		/// <summary>
		/// 指定したステータスが、リストのメンバーに含まれるユーザーのものかどうかを確認します。
		/// ステータスが Reply の場合、Reply 先のユーザーもリストに含まれているかどうかを確認します。
		/// </summary>
		public bool Contains(Status status)
		{
			if (this.HasMembers)
			{
				var contains = this.Members.Contains(status.User.Id);
				if (contains)
				{
					// Reply の場合、Reply 先のユーザーもリストに含まれているかどうかを確認する。
					var reply = status.Entities.UserMentions.FirstOrDefault(m => m.Indices.StartIndex == 0);
					return reply == null || this.Members.Contains(reply.Id);
				}
			}
			return false;
		}


		public override string ToString()
		{
			return this.FullName;
		}

		#region parse methods

		public static List Parse(string json, UserId ownerId)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			DynamicJsonHelper.ThrowIfError(djson);

			return ParseCore(djson, ownerId);
		}

		internal static List ParseCore(dynamic djson, UserId ownerId)
		{
			return TwitterClient.Current.Lists.Parse(djson, ownerId);
		}

		#endregion
	}
}
