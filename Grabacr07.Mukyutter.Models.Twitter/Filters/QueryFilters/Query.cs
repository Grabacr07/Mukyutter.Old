using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Internal;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters
{
	internal abstract class Query
	{
		public string Key { get; private set; }
		public string Value { get; private set; }
		public FilterMode Mode { get; private set; }
		protected Regex Regex { get; set; }

		protected Query(string key, string value, FilterMode mode)
		{
			this.Key = key;
			this.Value = value;
			this.Mode = mode;

			if (this.Mode == FilterMode.Regex)
			{
				// ReSharper disable DoNotCallOverridableMethodsInConstructor
				this.Regex = this.CreateRegex();
				// ReSharper restore DoNotCallOverridableMethodsInConstructor
			}
		}

		protected bool Bool(bool value)
		{
			if (this.Mode == FilterMode.Equal) return value;
			if (this.Mode == FilterMode.NotEqual) return !value;
			return value;
		}

		protected virtual Regex CreateRegex()
		{
			try
			{
				return new Regex(this.Value, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
			}
			catch (Exception ex)
			{
				throw new FilterException("クエリ '" + this.Key + "' の値 '" + this.Value + "' を正規表現に変換できません。", ex);
			}
		}

		protected virtual bool MatchRegex(string value)
		{
			try
			{
				return this.Regex.Match(value).Success;
			}
			catch (Exception ex)
			{
				ex.Report("クエリ '" + this.Key + "' の正規表現 '" + this.Value + "' の検索に失敗しました。");
			}

			return false;
		}
	}
}
