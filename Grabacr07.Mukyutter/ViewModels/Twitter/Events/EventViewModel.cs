using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Events;
using Grabacr07.Mukyutter.ViewModels.Extensions;

namespace Grabacr07.Mukyutter.ViewModels.Twitter.Events
{
	public abstract class EventViewModel : ViewModelBase
	{
		protected Event Event { get; set; }

		public long Id
		{
			get { return this.Event.CreatedAt.Ticks; }
		}

		public UserViewModel Source
		{
			get { return UserViewModel.Get(this.Event.Source); }
		}

		public UserViewModel Target
		{
			get { return UserViewModel.Get(this.Event.Target); }
		}

		public string CreatedAt
		{
			get { return this.Event.CreatedAt.ToAbsoluteShortString(); }
		}

		protected EventViewModel(Event ev)
		{
			this.Event = ev;
		}

		#region static members

		public static EventViewModel Select(Event ev)
		{
			var favorite = ev as Favorite;
			if (favorite != null)
			{
				var fav = favorite;
				return fav.Unfavorite
					? new UnfavoriteViewModel(fav)
					: new FavoriteViewModel(fav) as EventViewModel;
			}

			var retweet = ev as Retweet;
			if (retweet != null)
			{
				return new RetweetViewModel(retweet);
			}

			var follow = ev as Follow;
			if (follow != null)
			{
				return new FollowViewModel(follow);
			}

			var mention = ev as Mention;
			if (mention != null)
			{
				return new MentionViewModel(mention);
			}

			return new UnknownViewModel(ev);
		}

		#endregion
	}
}
