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
	/// 一連のガード処理を実施する静的メソッドを公開します。
	/// </summary>
	public static class Guard
	{
		/// <summary>
		/// 指定した値が null の場合、<see cref="T:System.ArgumentNullException"/> をスローします。
		/// </summary>
		/// <typeparam name="T">任意の型。</typeparam>
		/// <param name="target">null かどうかを確認する値。</param>
		/// <param name="name">値の名前。</param>
		/// <exception cref="T:Sytem.ArgumentNullException"><paramref name="target"/> が null の場合。</exception>
		public static void ArgumentNull<T>(T target, string name = null)
		{
			if (target == null) throw new ArgumentNullException(name);
		}

		public static void ThrowArgumentOutOfRange<T>(this IEnumerable<T> collection, int index, string name = null)
		{
			if (index < 0 || index >= collection.Count()) throw new ArgumentOutOfRangeException(name);
		}
	}
}
