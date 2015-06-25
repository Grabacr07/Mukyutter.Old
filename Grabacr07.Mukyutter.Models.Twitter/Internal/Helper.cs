using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Internal
{
	internal static class Helper
	{
		#region Operation

		/// <summary>
		/// リトライ可能な操作を実行します。例外はすべて捕捉され、指定したエラー メッセージを TwitterClient に通知します。
		/// </summary>
		public static void Operation(this Func<Task> operation, string errorMessage, params object[] args)
		{
			Operation(operation, _ => string.Format(errorMessage, args));
		}

		/// <summary>
		/// リトライ可能な操作を実行します。例外はすべて捕捉され、指定したエラー メッセージを TwitterClient に通知します。
		/// </summary>
		public static async void Operation(
			this Func<Task> operation,
			Func<Exception, string> errorMessage,
			bool isRetrying = true)
		{
			try
			{
				await operation();
			}
			catch (Exception ex)
			{
				ex.Report(errorMessage(ex), isRetrying ? () => Operation(operation, errorMessage) : (Action)null);
			}
		}

		/// <summary>
		/// リトライ可能な操作を実行します。例外はすべて捕捉され、指定したエラー メッセージを TwitterClient に通知します。
		/// </summary>
		public static void Operation<T>(Func<Task<T>> operation, string errorMessage, params object[] args)
		{
			Operation(operation, _ => string.Format(errorMessage, args));
		}

		/// <summary>
		/// リトライ可能な操作を実行します。例外はすべて捕捉され、指定したエラー メッセージを TwitterClient に通知します。
		/// </summary>
		public static async void Operation<T>(
			Func<Task<T>> operation,
			Func<Exception, string> errorMessage,
			bool isRetrying = true)
		{
			try
			{
				await operation();
			}
			catch (Exception ex)
			{
				ex.Report(errorMessage(ex), isRetrying ? () => Operation(operation, errorMessage) : (Action)null);
			}
		}

		#endregion

		/// <summary>
		/// 文字列がツイート可能な文字数をオーバーしていた場合、ツイート可能な文字数にカットします。
		/// </summary>
		public static string TrimIfOverLength(string text)
		{
			return text.Length <= TwitterDefinitions.TweetMaxLength ? text : text.Substring(0, TwitterDefinitions.TweetMaxLength);
		}

		/// <summary>
		/// 正規表現を使用して、指定した文字列からユーザー ID のコレクションを抽出します。
		/// </summary>
		/// <param name="text">ユーザー ID のコレクションを抽出する文字列。</param>
		/// <returns>抽出されたユーザー ID のコレクション。</returns>
		public static IEnumerable<ScreenName> GetScreenNames(string text)
		{
			try
			{
				return Regex.Matches(
					text,
					TwitterDefinitions.Regex.ScreenName,
					RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled)
					.OfType<Match>()
					.Select(m => m.Value)
					.Select(s => new ScreenName(s));
			}
			catch (Exception ex)
			{
				ex.Write();
				return Enumerable.Empty<ScreenName>();
			}
		}


		/// <summary>
		/// Twitter からの応答に含まれる日付の情報を表す文字列を <see cref="T:System.DateTime" /> 型に変換します。
		/// </summary>
		/// <param name="dateTimeString">日付の情報を表す文字列。</param>
		/// <returns>変換された日付。</returns>
		public static DateTime ToDateTime(string dateTimeString)
		{
			var result = CommonDefinitions.UnixEpoch;
			try
			{
				result = DateTime.ParseExact(
					dateTimeString,
					TwitterDefinitions.DateTimeFormat,
					DateTimeFormatInfo.InvariantInfo,
					DateTimeStyles.None
					);
			}
			catch (Exception e)
			{
				e.Write();
			}

			return result;
		}

		public static Uri ToUri(string uriString, UriKind uriKind = UriKind.Absolute)
		{
			if (String.IsNullOrEmpty(uriString)) return null;

			Uri uri;
			return Uri.TryCreate(uriString, uriKind, out uri) ? uri : null;
		}
	}
}
