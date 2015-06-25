using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Grabacr07.Mukyutter.Views.Twitter.Primitives
{
	/// <summary>
	/// StatusController.xaml の相互作用ロジック
	/// </summary>
	public partial class StatusController : UserControl
	{
		public StatusController()
		{
			this.InitializeComponent();
		}

		#region ButtonWidth 依存関係プロパティ

		public double ButtonWidth
		{
			get { return (double)this.GetValue(StatusController.ButtonWidthProperty); }
			set { this.SetValue(StatusController.ButtonWidthProperty, value); }
		}
		public static readonly DependencyProperty ButtonWidthProperty =
			DependencyProperty.Register("ButtonWidth", typeof(double), typeof(StatusController), new UIPropertyMetadata(32.0));

		#endregion

	}
}
