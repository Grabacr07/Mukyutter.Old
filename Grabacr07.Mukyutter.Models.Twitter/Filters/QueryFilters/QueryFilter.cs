using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters
{
	public partial class QueryFilter : TimelineFilter
	{
		public string Query { get; private set; }
		private Func<Status, bool> predicate;

		public QueryFilter(string query, Func<Status, bool> predicate)
		{
			this.Query = query;
			this.predicate = predicate;
		}

		public override bool Predicate(Status status)
		{
			return this.predicate(status);
		}
	}
}
