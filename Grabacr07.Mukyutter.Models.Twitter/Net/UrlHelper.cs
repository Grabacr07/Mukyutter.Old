using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public static class UrlHelper
	{
		/// <summary>
		/// 指定したユーザーのホーム画面の URL を取得します。
		/// </summary>
		/// <param name="screenName">ユーザー ID (先頭に "@" または "＠" が付加されている場合、取り除かれます)。</param>
		/// <returns>ユーザーのホーム画面の URL。</returns>
		/// <exception cref="System.ArgumentNullException">引数に NULL または空文字が指定された場合。</exception>
		/// <exception cref="System.ArgumentException">引数で指定されたユーザー ID が無効な場合。</exception>
		public static Uri GetUserHomeUrl(ScreenName screenName)
		{
			return new Uri("http://twitter.com/" + screenName.Value);
		}


		/// <summary>
		/// 検索ワードを指定し、Twitter 検索を行う URL を取得します。
		/// </summary>
		/// <param name="serchWords">検索するワード。</param>
		/// <returns>Twitter 検索を行う URL。</returns>
		/// <exception cref="System.ArgumentNullException">引数に NULL、空の配列、または空文字が指定された場合。</exception>
		public static Uri GetSerchUrl(params string[] serchWords)
		{
			if (serchWords == null || serchWords.Length < 1 || string.IsNullOrEmpty(serchWords[0]))
				throw new ArgumentNullException("serchWords");

			var result = serchWords[0];

			for (var i = 1; i < serchWords.Length; i++)
			{
				result += " " + serchWords[i];
			}

			return new Uri("http://twitter.com/search?q=" + result.UrlEncode());
		}

		/// <summary>
		/// ステータスのパーマリンクを取得します。
		/// </summary>
		/// <param name="status">パーマリンクを取得するステータス。</param>
		/// <returns>パーマリンクの URL。</returns>
		public static Uri GetPermalink(this Status status)
		{
			return new Uri(string.Format(@"https://twitter.com/{0}/statuses/{1}", status.User.ScreenName, status.Id));
		}
	}
}
