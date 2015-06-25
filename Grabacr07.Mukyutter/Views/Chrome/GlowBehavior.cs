using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Grabacr07.Mukyutter.Views.MetroChrome
{
	class GlowBehavior : Behavior<Window>
	{
		MukyutterGlowWindow left;
		MukyutterGlowWindow right;
		MukyutterGlowWindow top;
		MukyutterGlowWindow bottom;

		#region Opacity 依存関係プロパティ

		public double Opacity
		{
			get { return (double)this.GetValue(GlowBehavior.OpacityProperty); }
			set { this.SetValue(GlowBehavior.OpacityProperty, value); }
		}
		public static readonly DependencyProperty OpacityProperty =
			DependencyProperty.Register("Opacity", typeof(double), typeof(GlowBehavior), new UIPropertyMetadata(1.0, GlowBehavior.OpacityPropertyChangedCallback));

		private static void OpacityPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var source = (GlowBehavior)d;

			if (source.left != null) source.left.Opacity = (double)e.NewValue;
			if (source.right != null) source.right.Opacity = (double)e.NewValue;
			if (source.top != null) source.top.Opacity = (double)e.NewValue;
			if (source.bottom != null) source.bottom.Opacity = (double)e.NewValue;
		}

		#endregion

		#region CanHorizontalResize 依存関係プロパティ

		public bool CanHorizontalResize
		{
			get { return (bool)this.GetValue(GlowBehavior.CanHorizontalResizeProperty); }
			set { this.SetValue(GlowBehavior.CanHorizontalResizeProperty, value); }
		}
		public static readonly DependencyProperty CanHorizontalResizeProperty =
			DependencyProperty.Register("CanHorizontalResize", typeof(bool), typeof(GlowBehavior), new UIPropertyMetadata(true));

		#endregion

		#region CanVerticalResize 依存関係プロパティ

		public bool CanVerticalResize
		{
			get { return (bool)this.GetValue(GlowBehavior.CanVerticalResizeProperty); }
			set { this.SetValue(GlowBehavior.CanVerticalResizeProperty, value); }
		}
		public static readonly DependencyProperty CanVerticalResizeProperty =
			DependencyProperty.Register("CanVerticalResize", typeof(bool), typeof(GlowBehavior), new UIPropertyMetadata(true));

		#endregion


		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.Loaded += (sender, e) =>
			{
				this.left = new MukyutterGlowWindow(this.AssociatedObject, GlowDirection.Left);
				this.right = new MukyutterGlowWindow(this.AssociatedObject, GlowDirection.Right);
				this.top = new MukyutterGlowWindow(this.AssociatedObject, GlowDirection.Top);
				this.bottom = new MukyutterGlowWindow(this.AssociatedObject, GlowDirection.Bottom);

				this.left.Opacity = this.Opacity;
				this.right.Opacity = this.Opacity;
				this.top.Opacity = this.Opacity;
				this.bottom.Opacity = this.Opacity;

				this.left.Show();
				this.right.Show();
				this.top.Show();
				this.bottom.Show();

				this.left.Update();
				this.right.Update();
				this.top.Update();
				this.bottom.Update();
			};
		}
	}
}
