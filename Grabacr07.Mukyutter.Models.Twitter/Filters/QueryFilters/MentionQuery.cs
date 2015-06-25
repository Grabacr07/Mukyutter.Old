using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters
{
	internal class MentionQuery : Query
	{
		private bool allaccount;
		private ScreenName screenName;

		public MentionQuery(string key, string value, FilterMode mode) : base(key, value, mode)
		{
			if (value == "*")
			{
				if (mode != FilterMode.Equal)
				{
					throw new FilterException("クエリ '" + key + "' の値 '*' は、 '=' 演算子でのみ使用できます。");
				}

				this.allaccount = true;
			}
			else if (this.Mode != FilterMode.Regex)
			{
				this.screenName = new ScreenName(value);
			}
		}

		public bool Match(Status status)
		{
			if (this.allaccount)
			{
				return TwitterClient.Current.Accounts
					.Where(a => a.IsInitialized)
					.Select(a => a.User)
					.Any(status.IsMention);
			}

			if (this.Regex != null)
			{
				if (status.Entities != null && status.Entities.UserMentions != null)
				{
					return status.Entities.UserMentions.Any(um => this.MatchRegex(um.User.ScreenName.Value));
				}

				return false;
			}

			return this.Bool(status.IsMention(this.screenName));
		}
	}
}
