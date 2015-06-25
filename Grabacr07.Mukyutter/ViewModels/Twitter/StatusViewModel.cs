using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Imaging;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Composing;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Mukyutter.ViewModels.Extensions;
using Grabacr07.Mukyutter.Views.Controls;
using Grabacr07.Utilities;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Twitter
{
	public class StatusViewModel : ViewModelBase
	{
		/// <summary>デザイナー用のコンストラクターです。通常は使用しないでください。</summary>
		internal StatusViewModel() { }

		private StatusViewModel(Status status)
		{
			this.Status = status;

			this.FavoriteUsers = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				status.FavoriteUsers,
				UserViewModel.Get,
				DispatcherHelper.UIDispatcher);
			this.CompositeDisposable.Add(this.FavoriteUsers);

			this.RetweetUsers = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				status.RetweetUsers,
				UserViewModel.Get,
				DispatcherHelper.UIDispatcher);
			this.CompositeDisposable.Add(this.RetweetUsers);

			this.CompositeDisposable.Add(new PropertyChangedEventListener(status)
			{
				(sender, e) => this.RaisePropertyChanged(e.PropertyName)
			});
		}


		public Status Status { get; private set; }

		public StatusId Id
		{
			get { return this.Status.Id; } // ソートのキーに使用
		}

		public UserViewModel User
		{
			get { return UserViewModel.Get(this.Status.User); }
		}

		public bool ValidAccount
		{
			get { return MukyutterClient.Current.CurrentAccount != null; }
		}

		#region State

		public bool IsSelf
		{
			get { return TwitterClient.Current.Accounts.Any(a => this.Status.User.Id == a.UserId); }
		}

		public bool IsMention
		{
			get { return TwitterClient.Current.Accounts.Any(a => this.Status.IsMention(a.User)); }
		}

		public bool IsDeleted
		{
			get { return this.Status.IsDeleted; }
		}

		#endregion

		#region Fav/RT

		public StatusViewModel DisplayStatus
		{
			get { return this.RetweetedStatus ?? this; }
		}

		public bool IsRetweet
		{
			get { return this.Status.IsRetweetStatus; }
		}

		private StatusViewModel _RetweetedStatus;

		public StatusViewModel RetweetedStatus
		{
			get
			{
				return this.IsRetweet
					? this._RetweetedStatus ?? (this._RetweetedStatus = Get(this.Status.RetweetedStatus))
					: null;
			}
		}

		public ReadOnlyDispatcherCollection<UserViewModel> FavoriteUsers { get; private set; }
		public ReadOnlyDispatcherCollection<UserViewModel> RetweetUsers { get; private set; }

		#endregion

		#region Text

		public string FlatText
		{
			get { return this.Status.Text.Flatten().DecodeCER(); }
		}

		public IEnumerable<RichText> RichText
		{
			get { return this.Status.ToRichText(); }
		}

		#endregion

		#region 日付と時刻

		public string Time
		{
			get { return this.Status.CreatedAt.TimeOfDay.ToString(); }
		}

		public string AbsoluteDateTime
		{
			get { return this.Status.CreatedAt.ToString(); }
		}

		public string AbsoluteShortDateTime
		{
			get { return this.Status.CreatedAt.ToAbsoluteShortString(); }
		}

		public string RelativeDateTime
		{
			get
			{
				double num;
				string unit;
				var deff = DateTime.Now.Subtract(this.Status.CreatedAt);

				var years = deff.TotalDays / 365;
				if (years >= 1)
				{
					num = years;
					unit = "year";
				}
				else if (deff.TotalDays >= 1)
				{
					num = deff.TotalDays;
					unit = "day";
				}
				else if (deff.TotalHours >= 1)
				{
					num = deff.TotalHours;
					unit = "hour";
				}
				else if (deff.TotalMinutes >= 1)
				{
					num = deff.TotalMinutes;
					unit = "minute";
				}
				else if (deff.TotalSeconds >= 1)
				{
					num = deff.TotalSeconds;
					unit = "second";
				}
				else
				{
					num = 0;
					unit = "second";
				}

				var result = string.Format("{0} {1}{2} ago", Math.Floor(num), unit, (num >= 2) ? "s" : "");
				return result;
			}
		}

		#endregion

		#region Source

		public string Client
		{
			get { return this.Status.Source.Client; }
		}

		public bool CanOpenClientPage
		{
			get { return this.Status.Source.Url != null; }
		}

		public void OpenClientPage()
		{
			Process.Start(this.Status.Source.Url.ToString());
		}

		#endregion

		#region Control

		#region CanControl 変更通知プロパティ

		private bool _CanControl;

		public bool CanControl
		{
			get { return this._CanControl; }
			set
			{
				if (this._CanControl != value)
				{
					this._CanControl = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public bool CanDelete
		{
			get { return this.IsSelf; }
		}

		public bool CanQuote
		{
			get { return !this.Status.User.Protected; }
		}

		public bool CanRetweet
		{
			get { return !this.IsSelf && !this.Status.User.Protected; }
		}

		public bool CanFavorite
		{
			get { return !this.IsSelf; }
		}

		public bool CanFavAndRetweet
		{
			get { return this.CanRetweet && this.CanFavorite; }
		}

		public void Reply()
		{
			MukyutterClient.Current.Composer.CurrentStatus.Mention(this.Status);
			//MukyutterClient.Current.Composer.Initialize(this.Status);
		}

		public void Quotation()
		{
			MukyutterClient.Current.Composer.CurrentStatus.Quote(this.Status, new QuotedTweetFormat());
		}

		public void Retweet()
		{
			if (MukyutterClient.Current.CurrentAccount == null) return;

			// リツイートを考慮 (このツイートがリツイートなら、リツイート元をリツイートする)
			MukyutterClient.Current.CurrentAccount.CurrentToken.RetweetStatus(this.Status.DisplayStatus.Id).Operation("リツイートしました", "リツイートに失敗しました");
		}

		public void Favorite()
		{
			if (MukyutterClient.Current.CurrentAccount == null) return;

			// リツイートを考慮 (このツイートがリツイートなら、リツイート元をお気に入りに追加する)
			MukyutterClient.Current.CurrentAccount.CreateFavorites(this.Status.DisplayStatus.Id).Subscribe("お気に入りに登録しました", "お気に入りの登録に失敗しました");
		}

		public void Delete()
		{
			// リツイートを考慮しない (このツイートがリツイートでも、このツイートを削除する)
			var account = TwitterClient.Current.Accounts.FirstOrDefault(a => this.Status.User.Id == a.UserId);
			if (account != null) account.CurrentToken.DestroyStatus(this.Status.Id).Operation("削除しました。", "削除に失敗しました。");
		}

		#endregion

		#region Media

		public IList<WeakReferenceBitmap> Images
		{
			get { return this.Status.Entities.Media.Select(m => new WeakReferenceBitmap(m.MediaUrlHttps)).ToList(); }
		}

		#endregion

		public void OpenStatusPage()
		{
			Process.Start(this.Status.GetPermalink().ToString());
		}

		public override string ToString()
		{
			return this.Status.ToString();
		}

		#region static members

		private static ConcurrentDictionary<StatusId, StatusViewModel> cache;
		private static readonly object syncCache = new object();

		static StatusViewModel()
		{
			lock (syncCache)
			{
				cache = new ConcurrentDictionary<StatusId, StatusViewModel>();
			}
		}

		public static StatusViewModel Get(Status status)
		{
			lock (syncCache)
			{
				StatusViewModel statusViewModel;
				return cache.TryGetValue(status.Id, out statusViewModel)
					? statusViewModel
					: cache.AddOrUpdate(status.Id, new StatusViewModel(status), (_, u) => u);
			}
		}

		#endregion
	}
}
