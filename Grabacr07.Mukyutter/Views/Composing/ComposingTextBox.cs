using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using Grabacr07.Mukyutter.Views.Controls;

namespace Grabacr07.Mukyutter.Views.Composing
{
	public class ComposingTextBox : PromptTextBox
	{
		static ComposingTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ComposingTextBox), new FrameworkPropertyMetadata(typeof(ComposingTextBox)));
		}

		#region RemainingCharactersNum 依存関係プロパティ

		public string RemainingCharactersNum
		{
			get { return (string)this.GetValue(RemainingCharactersNumProperty); }
			set { this.SetValue(RemainingCharactersNumProperty, value); }
		}

		public static readonly DependencyProperty RemainingCharactersNumProperty =
			DependencyProperty.Register("RemainingCharactersNum", typeof(string), typeof(ComposingTextBox), new UIPropertyMetadata("140"));

		#endregion

		#region MediaThumbs 依存関係プロパティ

		public IEnumerable MediaThumbs
		{
			get { return (IEnumerable)this.GetValue(MediaThumbsProperty); }
			set { this.SetValue(MediaThumbsProperty, value); }
		}

		public static readonly DependencyProperty MediaThumbsProperty =
			DependencyProperty.Register("MediaThumbs", typeof(IEnumerable), typeof(ComposingTextBox), new UIPropertyMetadata(null));

		#endregion

		#region MediaThumbWidth 依存関係プロパティ

		public double MediaThumbWidth
		{
			get { return (double)this.GetValue(MediaThumbWidthProperty); }
			set { this.SetValue(MediaThumbWidthProperty, value); }
		}

		public static readonly DependencyProperty MediaThumbWidthProperty =
			DependencyProperty.Register("MediaThumbWidth", typeof(double), typeof(ComposingTextBox), new UIPropertyMetadata(64.0));

		#endregion

		#region ClearRequested ルーティング イベント

		public event RoutedEventHandler ClearRequested
		{
			add { this.AddHandler(ClearRequestedEvent, value); }
			remove { this.RemoveHandler(ClearRequestedEvent, value); }
		}

		public static readonly RoutedEvent ClearRequestedEvent = EventManager.RegisterRoutedEvent(
			"ClearRequested", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ComposingTextBox));

		#endregion

		public void RequestClearing()
		{
			var args = new RoutedEventArgs(ClearRequestedEvent);
			this.RaiseEvent(args);
		}
	}
}
