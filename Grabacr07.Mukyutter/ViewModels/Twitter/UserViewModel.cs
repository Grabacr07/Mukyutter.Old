using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Imaging;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.ViewModels.Internal;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Twitter
{
	public class UserViewModel : ViewModelBase
	{
		#region static members

		public static UserViewModel Empty
		{
			get { return cache[0]; }
		}

		private static ConcurrentDictionary<UserId, UserViewModel> cache;
		private static readonly object syncCache = new object();


		static UserViewModel()
		{
			var emptyUser = new User
			{
				ScreenName = new ScreenName("(empty)"),
				Name = "@-------",
				CreatedAt = DateTime.Now,
				Location = "--------, -----, Japan",
				Description = "--------------------.",
			};

			lock (syncCache)
			{
				cache = new ConcurrentDictionary<UserId, UserViewModel>();
				cache.AddOrUpdate(0, new UserViewModel(emptyUser), (_, u) => u);
			}
		}

		public static UserViewModel Get(User user)
		{
			if (user == null) return null;
			lock (syncCache)
			{
				UserViewModel userViewModel = null;
				return cache.TryGetValue(user.Id, out userViewModel)
					? userViewModel
					: cache.AddOrUpdate(user.Id, new UserViewModel(user), (_, u) => u);
			}
		}

		#endregion

		private User user;

		public UserId Id
		{
			get { return this.user.Id; }
		}

		public DateTime CreatedAt
		{
			get { return this.user.CreatedAt; }
		}

		public string Name
		{
			get { return this.user.Name.Replace('\n', ' '); }
		}

		public string ScreenName
		{
			get { return this.user.ScreenName.Value; }
		}

		public string ScreenNameWithAtmark
		{
			get { return this.user.ScreenName.ValueWithAtmark; }
		}

		public string Location
		{
			get { return this.user.Location; }
		}

		public string Description
		{
			get { return this.user.Description; }
		}

		public Uri ProfileImageUrl
		{
			get { return this.user.ProfileImageUrl; }
		}

		public string Url
		{
			get { return this.user.Url; }
		}

		public bool Protected
		{
			get { return this.user.Protected; }
		}

		public int FollowersCount
		{
			get { return this.user.FollowersCount; }
		}

		public int FriendsCount
		{
			get { return this.user.FriendsCount; }
		}

		public int FavoritesCount
		{
			get { return this.user.FavoritesCount; }
		}

		public long UtcOffset
		{
			get { return this.user.UtcOffset; }
		}

		public string TimeZone
		{
			get { return this.user.TimeZone; }
		}

		public bool Verified
		{
			get { return this.user.Verified; }
		}

		public int StatusesCount
		{
			get { return this.user.StatusesCount; }
		}

		public int ListedCount
		{
			get { return this.user.ListedCount; }
		}

		public Uri ReasonablyProfileImageUrl
		{
			get { return this.user.ReasonablyProfileImageUrl; }
		}

		public Uri HomeUrl
		{
			get { return this.user.HomeUrl; }
		}


		private WeakReferenceBitmap _Icon;
		public WeakReferenceBitmap Icon
		{
			get { return this._Icon ?? (this._Icon = new WeakReferenceBitmap(this.ProfileImageUrl)); }
		}

		/// <summary>デザイナー用のコンストラクターです。通常は使用しないでください。</summary>
		internal UserViewModel() { }

		private UserViewModel(User user)
		{
			this.user = user;

			this.CompositeDisposable.Add(
				new PropertyChangedEventListener(user)
				{
					(sender, e) => this.RaisePropertyChanged(e.PropertyName)
				});
		}

		public void OpenUserPage()
		{
			Process.Start(this.HomeUrl.ToString());
		}

		public override string ToString()
		{
			return this.user.ToString();
		}
	}
}
