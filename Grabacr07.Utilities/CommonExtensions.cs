using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Grabacr07.Utilities
{
	/// <summary>
	/// 汎用的な拡張メソッドを定義します。
	/// </summary>
	public static class CommonExtensions
	{
		/// <summary>
		/// コレクションを展開し、メンバーの文字列表現を指定したセパレーターで連結した文字列を返します。
		/// </summary>
		/// <typeparam name="T">コレクションに含まれる任意の型。</typeparam>
		/// <param name="source">対象のコレクション。</param>
		/// <param name="separator">セパレーターとして使用する文字列。</param>
		/// <returns>コレクションの文字列表現を展開し、指定したセパレーターで連結した文字列。</returns>
		public static string ToString<T>(this IEnumerable<T> source, string separator)
		{
			return string.Join(separator, source);

			//var index = 0;

			//return source.Aggregate(
			//	new StringBuilder(),
			//	(sb, o) => index++ == 0
			//		? sb.Append(o)
			//		: sb.AppendFormat("{0}{1}", separator, o))
			//	.ToString();
		}

		/// <summary>
		/// シーケンスが null でなく、1 つ以上の要素を含んでいるかどうかを確認します。
		/// </summary>
		public static bool HasValue<T>(this IEnumerable<T> source)
		{
			return source != null && source.Any();
		}


		/// <summary>
		/// 現在の日時を UNIX 時間として取得します。
		/// </summary>
		/// <param name="target">日時。</param>
		/// <returns>UNIX 時間として表される整数値。</returns>
		public static long ToUnixTime(this DateTime target)
		{
			return Convert.ToInt64((target - CommonDefinitions.UnixEpoch).TotalSeconds);
		}


		public static void SafeDispose(this IDisposable resource)
		{
			if (resource != null) resource.Dispose();
		}


		/// <summary>
		/// 値が有効なインデックス範囲内かどうかを確認します。
		/// </summary>
		/// <param name="index">インデックス値。</param>
		/// <param name="count">コレクションの項目数。</param>
		/// <returns><paramref name="index"/> が有効なインデックス範囲内の場合は true、それ以外の場合は false。</returns>
		public static bool IsValidIndex(this int index, int count)
		{
			return index.IsValidIndex(0, count);
		}

		/// <summary>
		/// 値が有効なインデックス範囲内かどうかを確認します。
		/// </summary>
		/// <param name="index">インデックス値。</param>
		/// <param name="start">開始位置。</param>
		/// <param name="count">コレクションの項目数。</param>
		/// <returns><paramref name="index"/> が有効なインデックス範囲内の場合は true、それ以外の場合は false。</returns>
		public static bool IsValidIndex(this int index, int start, int count)
		{
			return start <= index && index < count;
		}

		public static string ToStringWithoutTaskAwaiter(this Exception exception)
		{
			const string message = @"
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)";

			return exception.ToString().Replace(message, "");
		}
	}
}
