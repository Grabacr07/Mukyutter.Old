using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.ViewModels.Twitter;

namespace Grabacr07.Mukyutter.SampleData
{
	internal class SampleUserViewModel : UserViewModel
	{
		public new UserId Id
		{
			get { return 12345; }
		}

		public new DateTime CreatedAt
		{
			get { return DateTime.Now; }
		}

		public new string Name
		{
			get { return "ぱちゅりー"; }
		}

		public new string ScreenName
		{
			get { return "patchouli"; }
		}

		public new string ScreenNameWithAtmark
		{
			get { return "@patchouli"; }
		}

		public new string Location
		{
			get { return "紅魔館大図書館"; }
		}

		public new string Description
		{
			get { return "七曜の魔女。"; }
		}

		public new Uri ProfileImageUrl
		{
			get { return new Uri("https://si0.twimg.com/profile_images/3401772361/55ca2ac14bc6628a717175b3c23e12a6.png"); }
		}

		public new string Url
		{
			get { return "http://grabacr.net"; }
		}

		public new bool Protected
		{
			get { return true; }
		}

		public new int FollowersCount
		{
			get { return 65536; }
		}

		public new int FriendsCount
		{
			get { return 512; }
		}

		public new int FavoritesCount
		{
			get { return 0; }
		}

		public new long UtcOffset
		{
			get { return 0; }
		}

		public new string TimeZone
		{
			get { return "Gensokyo"; }
		}

		public new bool Verified
		{
			get { return false; }
		}

		public new int StatusesCount
		{
			get { return 1677216; }
		}

		public new int ListedCount
		{
			get { return 1024; }
		}

		public new Uri ReasonablyProfileImageUrl
		{
			get { return new Uri("https://si0.twimg.com/profile_images/3608739910/a1af0cbcb9b5d1d2a8d77702aee71fc5_normal.png"); }
		}

		public new Uri HomeUrl
		{
			get { return new Uri("https://twitter.com/Grabacr07"); }
		}

		public new dynamic Icon
		{
			get { return new { Image = new BitmapImage(this.ReasonablyProfileImageUrl) }; }
		}
	}
}