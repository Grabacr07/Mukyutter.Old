using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grabacr07.Mukyutter.Views.Twitter.Primitives
{
	public class QuickStatusController : Control
	{
		static QuickStatusController()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(QuickStatusController), new FrameworkPropertyMetadata(typeof(QuickStatusController)));
		}


		#region Image 依存関係プロパティ

		public ImageSource Image
		{
			get { return (ImageSource)this.GetValue(QuickStatusController.ImageProperty); }
			set { this.SetValue(QuickStatusController.ImageProperty, value); }
		}
		public static readonly DependencyProperty ImageProperty =
			DependencyProperty.Register("Image", typeof(ImageSource), typeof(QuickStatusController), new UIPropertyMetadata(null));

		#endregion

		#region ImageSize 依存関係プロパティ

		public double ImageSize
		{
			get { return (double)this.GetValue(QuickStatusController.ImageSizeProperty); }
			set { this.SetValue(QuickStatusController.ImageSizeProperty, value); }
		}
		public static readonly DependencyProperty ImageSizeProperty =
			DependencyProperty.Register("ImageSize", typeof(double), typeof(QuickStatusController), new UIPropertyMetadata(36.0));

		#endregion

	}
}
