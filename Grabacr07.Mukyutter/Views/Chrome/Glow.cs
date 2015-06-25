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

namespace Grabacr07.Mukyutter.Views.MetroChrome
{
	public class Glow : Control
	{
		static Glow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Glow), new FrameworkPropertyMetadata(typeof(Glow)));
		}


		#region Orientation 依存関係プロパティ

		public Orientation Orientation
		{
			get { return (Orientation)this.GetValue(Glow.OrientationProperty); }
			set { this.SetValue(Glow.OrientationProperty, value); }
		}
		public static readonly DependencyProperty OrientationProperty =
			DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Glow), new UIPropertyMetadata(Orientation.Vertical));

		#endregion

		#region IsGlow 依存関係プロパティ

		public bool IsGlow
		{
			get { return (bool)this.GetValue(Glow.IsGlowProperty); }
			set { this.SetValue(Glow.IsGlowProperty, value); }
		}
		public static readonly DependencyProperty IsGlowProperty =
			DependencyProperty.Register("IsGlow", typeof(bool), typeof(Glow), new UIPropertyMetadata(false));

		#endregion

	}
}
