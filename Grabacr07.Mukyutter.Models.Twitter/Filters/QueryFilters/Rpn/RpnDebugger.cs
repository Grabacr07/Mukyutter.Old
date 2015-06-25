using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters.Rpn
{
	public class QueryFilterDebugger
	{
		public static void Run()
		{
			var expressions = new[] {
				@"user=""grabacr"" & (text = ""hoge"" | text = ""foo"")",
				//@"user=""grabacr"" & !(text = ""hoge"" | text = ""foo"")",
				//@"!(user=""grabacr"") & (text = ""hoge"" | text = ""foo"")",
			};

			expressions.ForEach(exp =>
			{
				var filter = QueryFilter.Create(exp, null);
			});
		}
	}
}
