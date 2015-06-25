using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter
{
	/// <summary>
	/// <see cref="N:Grabacr07.Mukyutter.Models.Twitter"/> 名前空間で使用される、API やコントロールの動作に関する静的プロパティおよび静的メソッドを提供します。このクラスは継承できません。
	/// </summary>
	public static class TwitterDefinitions
	{
		static TwitterDefinitions()
		{
			TwitterDefinitions.AtMarks = EnumerableEx.Return("@");
		}


		/// <summary>
		/// ツイートできる最大文字数を表します。このフィールドは定数です。
		/// </summary>
		public const int TweetMaxLength = 140;

		/// <summary>
		/// <see cref="M:System.DateTime.ParseExact"/> メソッドで使用する、Twitter API の日付のフォーマットを取得します。
		/// </summary>
		public static string[] DateTimeFormat
		{
			get { return TwitterDefinitions._dateTimeFormat; }
		}
		private static string[] _dateTimeFormat = new string[] { "ddd MMM d HH':'mm':'ss zzz yyyy" };


		/// <summary>
		/// Twitter のユーザー ID であることを表す、ID の先頭のアットマークとして使用する文字列のコレクションを取得または設定します。
		/// 規定値は、"@" のみを含むコレクションです。
		/// </summary>
		public static IEnumerable<string> AtMarks { get; set; }



		/// <summary>
		/// 正規表現に関する固有の値を定義します。このクラスは継承できません。
		/// </summary>
		public static class Regex
		{
			/// <summary>
			/// Twitter のユーザー ID を抽出可能な正規表現を取得します。
			/// </summary>
			public static string ScreenName
			{
				get { return @"[" + TwitterDefinitions.AtMarks.ToString("|") + @"][a-zA-Z0-9_]+"; }
			}

			/// <summary>
			/// 先頭のアットマークを除いた Twitter のユーザー ID を抽出可能な正規表現を取得します。
			/// </summary>
			public static string ScreenNameWithoutAtMark
			{
				get { return @"[a-z0-9_]+"; }
			}

			/// <summary>
			/// ハッシュタグを抽出可能な正規表現を取得します。
			/// </summary>
			public static string HashTag
			{
				get { return @"#[a-zA-Z0-9_\-]+"; }
			}

			/// <summary>
			/// 投稿に使用されたクライアント名と URL を抽出する正規表現を取得します。
			/// 正規表現検索に一致したグループから、"client" でクライアント名を、"url" で URL を取得できます。
			/// </summary>
			public static string Source
			{
				get { return @"<a href=\""(?<url>.*?)\"".*?>(?<client>.*?)</a>"; }
			}
		}


		/// <summary>
		/// HTTP レスポンス ヘッダへアクセスする際の文字列を定義します。このクラスは継承できません。
		/// </summary>
		public static class HttpResponse
		{
			/// <summary>
			/// HTTP レスポンス ヘッダに含まれる、単位時間あたりの API 実行可能回数を示す文字列を表します。
			/// </summary>
			public const string RateLimit = "X-Rate-Limit-Limit";

			/// <summary>
			/// HTTP レスポンス ヘッダに含まれる、単位時間あたりの残りの API 実行可能回数を示す文字列を表します。
			/// </summary>
			public const string RateLimitRemaining = "X-Rate-Limit-Remaining";

			/// <summary>
			/// HTTP レスポンス ヘッダに含まれる、API 実行回数制限の種類を示す文字列を表します。
			/// </summary>
			public const string RateLimitClass = "X-Rate-Limit-Class";

			/// <summary>
			/// HTTP レスポンス ヘッダに含まれる、現在の API 実行回数制限の期限を示す文字列を表します。
			/// </summary>
			public const string RateLimitReset = "X-Rate-Limit-Reset";
		}
	}
}
