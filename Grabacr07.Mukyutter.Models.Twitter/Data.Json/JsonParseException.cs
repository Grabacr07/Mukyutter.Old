using System;
using System.Runtime.CompilerServices;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Json
{
	public class JsonParseException : TwitterException
	{
		/// <summary>
		/// 変換に失敗した json 文字列を取得します。
		/// </summary>
		public string Json { get; private set; }

		/// <summary>
		/// 変換先の型を取得します。
		/// </summary>
		public Type Type { get; private set; }


		internal JsonParseException(dynamic djson, Type type, Exception innerException,
			[CallerFilePath]string path = "", [CallerMemberName]string member = "", [CallerLineNumber]int line = 0)
			: this((string)djson.ToString(), type, innerException, path, member, line) { }

		internal JsonParseException(string json, Type type, Exception innerException,
			[CallerFilePath]string path = "", [CallerMemberName]string member = "", [CallerLineNumber]int line = 0)
			: base(string.Format("json から {0} 型への変換に失敗しました。{1}", type.Name, json), innerException, path, member, line)
		{
			this.Json = json;
			this.Type = type;
		}
	}
}
