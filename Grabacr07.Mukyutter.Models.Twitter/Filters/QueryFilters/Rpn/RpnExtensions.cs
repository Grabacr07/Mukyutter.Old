using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters.Rpn
{
	internal static class RpnExtensions
	{
		/// <summary>
		/// 現在の文字が左丸かっこかどうかを確認します。
		/// </summary>
		public static bool IsBegginingParenthesis(this char c)
		{
			return c == '(';
		}
		/// <summary>
		/// 現在の文字が左丸かっこかどうかを確認します。
		/// </summary>
		public static bool IsBegginingParenthesis(this string s)
		{
			return s != null && s.Length == 1 && s[0].IsBegginingParenthesis();
		}

		/// <summary>
		/// 現在の文字が右丸かっこかどうかを確認します。
		/// </summary>
		public static bool IsClosingParenthesis(this char c)
		{
			return c == ')';
		}
		/// <summary>
		/// 現在の文字が右丸かっこかどうかを確認します。
		/// </summary>
		public static bool IsClosingParenthesis(this string s)
		{
			return s != null && s.Length == 1 && s[0].IsClosingParenthesis();
		}

		/// <summary>
		/// 現在の文字が丸かっこかどうかを確認します。
		/// </summary>
		public static bool IsParenthesis(this char c)
		{
			return c.IsBegginingParenthesis() || c.IsClosingParenthesis();
		}
		/// <summary>
		/// 現在の文字が丸かっこかどうかを確認します。
		/// </summary>
		public static bool IsParenthesis(this string s)
		{
			return s.IsBegginingParenthesis() || s.IsClosingParenthesis();
		}


		/// <summary>
		/// 現在の文字が論理積演算子 ('&amp;') かどうかを確認します。
		/// </summary>
		public static bool IsAnd(this char c)
		{
			return c == '&';
		}
		/// <summary>
		/// 現在の文字が論理積演算子 ('&amp;') かどうかを確認します。
		/// </summary>
		public static bool IsAnd(this string s)
		{
			return s != null && s.Length == 1 && s[0].IsAnd();
		}

		/// <summary>
		/// 現在の文字が論理和演算子 ('|') かどうかを確認します。
		/// </summary>
		public static bool IsOr(this char c)
		{
			return c == '|';
		}
		/// <summary>
		/// 現在の文字が論理和演算子 ('|') かどうかを確認します。
		/// </summary>
		public static bool IsOr(this string s)
		{
			return s != null && s.Length == 1 && s[0].IsOr();
		}

		/// <summary>
		/// 現在の文字が否定演算子 ('!') かどうかを確認します。
		/// </summary>
		public static bool IsNot(this char c)
		{
			return c == '!';
		}
		/// <summary>
		/// 現在の文字が否定演算子 ('!') かどうかを確認します。
		/// </summary>
		public static bool IsNot(this string s)
		{
			return s != null && s.Length == 1 && s[0].IsNot();
		}


		/// <summary>
		/// 現在の文字が演算子 ('&amp;' または '|') かどうかを確認します。
		/// </summary>
		public static bool IsOperator(this char c)
		{
			return c.IsAnd() || c.IsOr();
		}

		/// <summary>
		/// 現在の文字が演算子 ('&amp;' または '|') かどうかを確認します。
		/// </summary>
		public static bool IsOperator(this string s)
		{
			return s.IsAnd() || s.IsOr();
		}


		/// <summary>
		/// 現在の文字が予約された文字 (演算子または丸かっこ) かどうかを確認します。
		/// </summary>
		public static bool IsSymbol(this char c)
		{
			return c.IsOperator() || c.IsParenthesis() || c.IsNot();
		}

		/// <summary>
		/// 現在の文字が予約された文字 (演算子または丸かっこ) かどうかを確認します。
		/// </summary>
		public static bool IsSymbol(this string s)
		{
			return s.IsOperator() || s.IsParenthesis() || s.IsNot();
		}
	}
}
