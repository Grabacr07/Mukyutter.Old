﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Views.Converters
{
	/// <summary>
	/// <see cref="T:System.Windows.WindowState"/> 値が
	/// <see cref="F:System.Windows.WindowState.Normal"/> のときに <see cref="F:System.Windows.Visibility.Visible"/>
	/// へ、<see cref="F:System.Windows.WindowState.Maximized"/> のときに <see cref="F:System.Windows.Visibility.Collapsed"/>
	/// へ変換するコンバーターを定義します。
	/// </summary>
	public class WindowStateToVisibilityConverter : IValueConverter
	{
		/// <summary>
		/// <see cref="T:System.Windows.WindowState"/> 値から <see cref="T:System.Windows.Visibility"/> 値へ変換します。
		/// </summary>
		/// <param name="value">変換元の <see cref="T:System.Windows.WindowState"/> 値。</param>
		/// <param name="targetType">この引数は使用されません。</param>
		/// <param name="parameter">パラメーター。文字列 "Reverse" を指定すると、変換結果が逆になります。それ以外の値を指定しても使用されません。</param>
		/// <param name="culture">この引数は使用されません。</param>
		/// <returns><see cref="T:System.Windows.WindowState"/> 値から <see cref="T:System.Windows.Visibility"/> 値への変換結果。</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is WindowState)) return Visibility.Collapsed;

			var state = (WindowState)value;
			var param = parameter as string;
			var isReverse = (param != null && param.Compare("Reverse"));

			return state == WindowState.Normal
				? isReverse ? Visibility.Collapsed : Visibility.Visible
				: isReverse ? Visibility.Visible : Visibility.Collapsed;
		}

		/// <summary>
		/// この実装は常に <see cref="T:System.NotImplementedException"/> をスローします。
		/// </summary>
		/// <param name="value">この引数は使用されません。</param>
		/// <param name="targetType">この引数は使用されません。</param>
		/// <param name="parameter">この引数は使用されません。</param>
		/// <param name="culture">この引数は使用されません。</param>
		/// <returns>値は返されません。</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
