using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters
{
	internal enum FilterMode
	{
		Equal,
		NotEqual,
		Regex,
	}

	static class FilterModeExtensions
	{
		public static FilterMode ToFilterMode(this string operation)
		{
			switch (operation)
			{
				case "!=":
					return FilterMode.NotEqual;
				case "?=":
					return FilterMode.Regex;
				default:
					return FilterMode.Equal;
			}
		}
	}
}
