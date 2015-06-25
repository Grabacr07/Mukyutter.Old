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

namespace Grabacr07.Mukyutter.Views.Twitter.Events
{
	public class EventCounter : ListBox
	{
		static EventCounter()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(EventCounter), new FrameworkPropertyMetadata(typeof(EventCounter)));
		}


		#region Icon 依存関係プロパティ

		public TwitterIconType Icon
		{
			get { return (TwitterIconType)this.GetValue(EventCounter.IconProperty); }
			set { this.SetValue(EventCounter.IconProperty, value); }
		}
		public static readonly DependencyProperty IconProperty =
			DependencyProperty.Register("Icon", typeof(TwitterIconType), typeof(EventCounter), new UIPropertyMetadata(TwitterIconType.Favorite));

		#endregion

	}
}
