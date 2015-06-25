using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Internal;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Events
{
	public class Favorite : Event
	{
		public bool Unfavorite { get; internal set; }

		public Status TargetObject { get; internal set; }

		internal Favorite() { }


		internal static Favorite ParseCore(dynamic djson)
		{
			var result = new Favorite
			{
				Target = TwitterClient.Current.Users.Parse(djson.target),
				Source = TwitterClient.Current.Users.Parse(djson.source),
				CreatedAt = Helper.ToDateTime(djson.created_at),
				TargetObject = TwitterClient.Current.Statuses.Add(djson.target_object, StatusSource.RestApi),
				Unfavorite = djson.@event == "unfavorite",
			};

			return result;
		}
	}
}
