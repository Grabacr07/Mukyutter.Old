using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters
{
	internal class FullMatch : Query
	{
		public FullMatch(string key, string value, FilterMode mode) : base(key, value, mode) { }

		/// <summary>
		/// 指定した文字列に、完全一致でフィルターを適用します。
		/// Equal モードの場合は <paramref name="target" /> とフィルター文字列が一致したとき、NotEqual モードの場合は <paramref name="target" /> とフィルター文字列が一致しなかったときに true を返します。
		/// フィルターが正規表現モードの場合は完全にマッチした場合のみ true を返します。
		/// </summary>
		public bool Match(string target, bool ignoreCase)
		{
			if (this.Regex != null)
			{
				return this.MatchRegex(target);
			}

			return this.Bool(string.Compare(this.Value, target, ignoreCase) == 0);
		}
	}

	internal class PartialMatch : Query
	{
		public PartialMatch(string key, string value, FilterMode mode) : base(key, value, mode) { }

		/// <summary>
		/// 指定した文字列に、部分一致でフィルターを適用します。
		/// Equal モードの場合は <paramref name="target" /> にフィルター文字列が含まれているとき、NotEqual モードの場合は <paramref name="target" /> にフィルター文字列が含まれていないときに true を返します。
		/// フィルターが正規表現モードの場合は完全にマッチした場合のみ true を返します。
		/// </summary>
		public bool Match(string target, bool ignoreCase)
		{
			if (this.Regex != null)
			{
				return this.MatchRegex(target);
			}

			var comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
			return this.Bool(target.IndexOf(this.Value, comparison) >= 0);
		}
	}
}
