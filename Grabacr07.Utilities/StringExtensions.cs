using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grabacr07.Utilities.Development;
using Microsoft.VisualBasic;

namespace Grabacr07.Utilities
{
	/// <summary>
	/// .NET Framework 標準の文字列型に対する拡張メソッドを定義します。
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// 現在の文字列と、指定した文字列を比較します。大文字と小文字は区別されません。
		/// </summary>
		public static bool Compare(this string strA, string strB)
		{
			return string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase) == 0;
		}

		/// <summary>
		/// 現在の文字列の改行コードを置き換え、フラットな文字列を作成します。
		/// </summary>
		/// <param name="str">元の文字列。</param>
		/// <param name="replace">改行コードを置き換える文字列。既定値は半角スペースです。</param>
		public static string Flatten(this string str, string replace = " ")
		{
			return str.Replace("\r", "").Replace("\n", replace);
		}

		/// <summary>
		/// 現在の文字列の前後に、指定したラップ文字列を追加した文字列を返します。
		/// </summary>
		/// <param name="input">対象の文字列。</param>
		/// <param name="wrapper">ラップ文字列。</param>
		/// <returns>対象の文字列の前後にラップ文字列を追加した文字列。</returns>
		public static string Wrap(this string input, string wrapper)
		{
			return wrapper + input + wrapper;
		}


		/// <summary>
		/// 現在の文字列内の文字実体参照 (CER:Character Entity Reference) を実体に変換します。
		/// </summary>
		/// <param name="text">対象の文字列。</param>
		public static string DecodeCER(this string text)
		{
			return text == null ? null : text
				.Replace(@"&lt;", "<")
				.Replace(@"&gt;", ">")
				.Replace(@"&quot;", "\"")
				.Replace(@"&apos;", "'")
				.Replace(@"&nbsp;", " ")
				.Replace(@"&amp;", "&");
		}


		/// <summary>
		/// 現在のテキスト内の特定のパラメータを指定した文字列に置換した新しい文字列を返します。
		/// </summary>
		/// <param name="resourceStr">元のリソース文字列。</param>
		/// <param name="paramAndReplaceStrs">パラメータ (Key) と置き換える文字列 (Value) の組み合わせのコレクション。</param>
		/// <returns>リソース文字列内のパラメータが置換された文字列。</returns>
		public static string SetParameter(this string resourceStr, Dictionary<string, string> paramAndReplaceStrs)
		{
			return paramAndReplaceStrs.Aggregate(
				resourceStr, 
				(current, paramAndReplaceStr) => current.SetParameter(paramAndReplaceStr.Key, paramAndReplaceStr.Value));
		}


		/// <summary>
		/// 現在のテキスト内の特定のパラメータを指定した文字列に置換した新しい文字列を返します。
		/// </summary>
		/// <param name="resourceStr">元のリソース文字列。</param>
		/// <param name="param">置換元のパラメータ。</param>
		/// <param name="replaceStr">パラメータに対する置換後の文字列。</param>
		/// <returns>リソース文字列内のパラメータが置換された文字列。</returns>
		/// <remarks>
		/// 使用例
		/// <para>resourceStr:"ようこそ、%UserName% さん！"</para>
		/// <para>param:"UserName"</para>
		/// <para>replaceStr:"Grabacr07"</para>
		/// <para>戻り値:"ようこそ、Grabacr07 さん！"</para>
		/// </remarks>
		public static string SetParameter(this string resourceStr, string param, string replaceStr)
		{
			return resourceStr.Replace("%" + param + "%", replaceStr);
		}


		/// <summary>
		/// 現在のテキストが null または System.String.Empty 文字列であるかどうかを示します。
		/// </summary>
		/// <param name="targetStr">現在のテキスト。</param>
		/// <returns>現在のテキストが null または System.String.Empty 文字列の場合は true、それ以外は false。</returns>
		public static bool IsNullOrEmpty(this string targetStr)
		{
			return string.IsNullOrEmpty(targetStr);
		}

		/// <summary>
		/// 現在のテキストが null または System.String.Empty 文字列以外の文字列であるかどうかを示します。
		/// </summary>
		/// <param name="targetStr">現在のテキスト。</param>
		/// <returns>現在のテキストが null または System.String.Empty 文字列でない場合は true、それ以外は false。</returns>
		public static bool IsNotNullOrEmpty(this string targetStr)
		{
			return !string.IsNullOrEmpty(targetStr);
		}


		/// <summary>
		/// 2 バイト文字も考慮した URI エンコードを行います。
		/// </summary>
		/// <param name="stringToEncode">エンコードする文字列。</param>
		/// <returns>エンコードの結果。</returns>
		public static string UrlEncode(this string stringToEncode)
		{
			const string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
			var sb = new StringBuilder();
			var bytes = Encoding.UTF8.GetBytes(stringToEncode);

			foreach (byte b in bytes)
			{
				if (unreservedChars.IndexOf(Strings.Chr(b)) != -1)
				{
					sb.Append(Strings.Chr(b));
				}
				else
				{
					sb.AppendFormat("%{0:X2}", b);
				}
			}

			return sb.ToString();
		}


		/// <summary>
		/// 例外をスローさせずに、現在の文字列を使用して string.Format メソッドを呼び出します。呼び出しで例外がスローされた場合、このメソッドは例外をスローする代わりに元の文字列をそのまま返します。
		/// </summary>
		public static string SafeFormatting(this string source, params object[] args)
		{
			try
			{
				return string.Format(source, args);
			}
			catch (Exception ex)
			{
				ex.Write();
				return source;
			}
		}
	}
}
