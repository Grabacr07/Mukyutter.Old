using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters
{
	internal class ScreenNameQuery : Query
	{
		private ScreenName screenName;

		public ScreenNameQuery(string key, string value, FilterMode mode) : base(key, value, mode)
		{
			if (mode != FilterMode.Regex)
			{
				this.screenName = new ScreenName(value);
			}
		}

		public bool Match(ScreenName name)
		{
			if (this.Regex != null)
			{
				return this.MatchRegex(name.Value);
			}

			return this.Bool(this.screenName == name);
		}
	}
}
