using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters
{
	class BooleanQuery : Query
	{
		private bool b;

		public BooleanQuery(string key, string value, FilterMode mode) : base(key, value, mode)
		{
			if (mode != FilterMode.Equal)
			{
				throw new FilterException("クエリ '" + key + "' は '=' 演算子でのみ使用できます。");
			}

			if (!bool.TryParse(value, out this.b))
			{
				throw new FilterException("クエリ '" + key + "' の値は、'true' または 'false' で指定してください。");
			}
		}

		public bool Match(bool predicate)
		{
			return this.b ? predicate : !predicate;
		}
	}
}
