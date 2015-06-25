﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grabacr07.Mukyutter.Views.Controls
{
	public class ExpanderButton : ToggleButton
	{
		static ExpanderButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpanderButton), new FrameworkPropertyMetadata(typeof(ExpanderButton)));
		}


		#region Direction 依存関係プロパティ

		public ExpandDirection Direction
		{
			get { return (ExpandDirection)this.GetValue(ExpanderButton.DirectionProperty); }
			set { this.SetValue(ExpanderButton.DirectionProperty, value); }
		}

		public static readonly DependencyProperty DirectionProperty =
			DependencyProperty.Register("Direction", typeof(ExpandDirection), typeof(ExpanderButton), new UIPropertyMetadata(ExpandDirection.Left, ExpanderButton.DirectionChangedCallback));

		private static void DirectionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var instance = (ExpanderButton)d;
		}

		#endregion

	}
}
