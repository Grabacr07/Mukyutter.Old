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

namespace Grabacr07.Mukyutter.Views.Twitter
{
	public class TwitterIcon : Control
	{
		static TwitterIcon()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TwitterIcon), new FrameworkPropertyMetadata(typeof(TwitterIcon)));
		}


		#region Type 依存関係プロパティ

		public TwitterIconType Type
		{
			get { return (TwitterIconType)this.GetValue(TwitterIcon.TypeProperty); }
			set { this.SetValue(TwitterIcon.TypeProperty, value); }
		}
		public static readonly DependencyProperty TypeProperty =
			DependencyProperty.Register("Type", typeof(TwitterIconType), typeof(TwitterIcon), new UIPropertyMetadata(TwitterIconType.TwitterBird));

		#endregion

	}
}
