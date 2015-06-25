using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grabacr07.Utilities
{
	/// <summary>
	/// ユーティリティまたは参照するアプリケーションでの固有の値を定義します。このクラスは継承できません。
	/// </summary>
	public sealed class CommonDefinitions
	{
		private CommonDefinitions() { }

		/// <summary>
		/// 各種暗号化に使用するキーを取得します。
		/// </summary>
		internal static string EncryptKey
		{
			get { return "2ｰﾃ#3r98!ﾘKaa54Yｨc0rG027bMｨ"; }
		}


		/// <summary>
		/// UNIX Time の基準時間 (世界協定時 1970 年 1 月 1 日 0 時 0 分 0 秒) を表します。
		/// </summary>
		public readonly static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


		/// <summary>
		/// 正規表現に関する固有の値を定義します。このクラスは継承できません。
		/// </summary>
		public sealed class Regex
		{
			private Regex() { }

			/// <summary>
			/// HTTP URL を抽出可能な正規表現を取得します。
			/// </summary>
			public static string Url
			{
				get { return @"\b(?:https?|shttp)://(?:(?:[-_.!~*'()a-zA-Z0-9;:&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*@)?(?:(?:[a-zA-Z0-9](?:[-a-zA-Z0-9]*[a-zA-Z0-9])?\.)*[a-zA-Z](?:[-a-zA-Z0-9]*[a-zA-Z0-9])?\.?|[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)(?::[0-9]*)?(?:/(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*(?:;(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)*(?:/(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*(?:;(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)*)*)?(?:\?(?:[-_.!~*'()a-zA-Z0-9;/?:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)?(?:#(?:[-_.!~*'()a-zA-Z0-9;/?:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)?"; }
			}
		}
	}
}
