using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Grabacr07.Mukyutter.Views.Twitter.Primitives
{
	/// <summary>
	/// UnreadCounter.xaml の相互作用ロジック
	/// </summary>
	public partial class UnreadCounter : UserControl
	{

		#region Count 依存関係プロパティ

		public int Count
		{
			get { return (int)this.GetValue(UnreadCounter.CountProperty); }
			set { this.SetValue(UnreadCounter.CountProperty, value); }
		}

		public static readonly DependencyProperty CountProperty =
			DependencyProperty.Register("Count", typeof(int), typeof(UnreadCounter), new UIPropertyMetadata(0, UnreadCounter.CountChangedCallback));

		private static void CountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var instance = (UnreadCounter)d;
			if (instance != null) instance.SetCount((int)e.NewValue);
		}

		#endregion


		public UnreadCounter()
		{
			this.InitializeComponent();
			this.SetCount(0);
		}

		private void SetCount(int value)
		{
			this.count.Text = value.ToString(CultureInfo.InvariantCulture);

			if (value == 0)
			{
				this.Visibility = Visibility.Hidden;
			}
			else
			{
				this.Visibility = Visibility.Visible;
				this.count.FontSize = (value < 10) ? 12 : 11;
			}
		}
	}
}
