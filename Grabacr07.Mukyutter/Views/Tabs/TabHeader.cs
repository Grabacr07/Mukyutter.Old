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

namespace Grabacr07.Mukyutter.Views.Tabs
{
	public class TabHeader : ListView
	{
		static TabHeader()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TabHeader), new FrameworkPropertyMetadata(typeof(TabHeader)));
		}

		#region AdditionalMenu 依存関係プロパティ

		public object AdditionalMenu
		{
			get { return this.GetValue(AdditionalMenuProperty); }
			set { this.SetValue(AdditionalMenuProperty, value); }
		}

		public static readonly DependencyProperty AdditionalMenuProperty =
			DependencyProperty.Register("AdditionalMenu", typeof(object), typeof(TabHeader), new UIPropertyMetadata(null));

		#endregion
	}
}
