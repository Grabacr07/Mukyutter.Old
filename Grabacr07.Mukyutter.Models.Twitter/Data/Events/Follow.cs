using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grabacr07.Mukyutter.Models.Twitter.Internal;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Events
{
	public class Follow : Event
	{
		public Follow() { }


		internal static Follow ParseCore(dynamic djson)
		{
			var result = new Follow
			{
				Target = TwitterClient.Current.Users.Parse(djson.target),
				Source = TwitterClient.Current.Users.Parse(djson.source),
				CreatedAt = Helper.ToDateTime(djson.created_at),
			};

			return result;
		}
	}
}
