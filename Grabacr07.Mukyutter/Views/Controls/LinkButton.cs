using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grabacr07.Mukyutter.Views.Controls
{
	public class LinkButton : Button
	{
		static LinkButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkButton), new FrameworkPropertyMetadata(typeof(LinkButton)));
		}

		#region Text 依存関係プロパティ

		public string Text
		{
			get { return (string)this.GetValue(LinkButton.TextProperty); }
			set { this.SetValue(LinkButton.TextProperty, value); }
		}
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(LinkButton), new UIPropertyMetadata(""));

		#endregion


		#region TextTrimming 依存関係プロパティ

		public TextTrimming TextTrimming
		{
			get { return (TextTrimming)this.GetValue(LinkButton.TextTrimmingProperty); }
			set { this.SetValue(LinkButton.TextTrimmingProperty, value); }
		}
		public static readonly DependencyProperty TextTrimmingProperty =
			DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(LinkButton), new UIPropertyMetadata(TextTrimming.CharacterEllipsis));

		#endregion

		#region TextWrapping 依存関係プロパティ

		public TextWrapping TextWrapping
		{
			get { return (TextWrapping)this.GetValue(LinkButton.TextWrappingProperty); }
			set { this.SetValue(LinkButton.TextWrappingProperty, value); }
		}
		public static readonly DependencyProperty TextWrappingProperty =
			DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(LinkButton), new UIPropertyMetadata(TextWrapping.NoWrap));

		#endregion

	}
}
