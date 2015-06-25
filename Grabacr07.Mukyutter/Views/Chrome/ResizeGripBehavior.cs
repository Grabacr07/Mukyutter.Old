using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using Grabacr07.Utilities.Win32;

namespace Grabacr07.Mukyutter.Views.MetroChrome
{
	class ResizeGripBehavior : Behavior<FrameworkElement>
	{
		private bool isEnabled;
		private bool isInitialized;

		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.Loaded += this.Initialize;
		}

		protected override void OnDetaching()
		{
			this.AssociatedObject.Loaded -= this.Initialize;
			base.OnDetaching();
		}

		private void Initialize(object sender, RoutedEventArgs args)
		{
			if (isInitialized) return;

			var window = Window.GetWindow(this.AssociatedObject);
			if (window == null) return;
			window.StateChanged += (_, __) =>
			{
				this.isEnabled = window.WindowState == WindowState.Normal;
			};
			window.SourceInitialized += (_, __) =>
			{
				var source = (HwndSource)PresentationSource.FromVisual(window);
				if (source != null) source.AddHook(this.WndProc);
			};
			window.ContentRendered += (_, __) =>
			{
				this.isEnabled = window.WindowState == WindowState.Normal;
			};

			this.isInitialized = true;
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == (int)WM.NCHITTEST && this.isEnabled)
			{
				var dpiScaleFactor = this.AssociatedObject.GetDpiScaleFactor();
				var ptScreen = lParam.ToPoint().Multiplication(dpiScaleFactor);
				var ptClient = this.AssociatedObject.PointFromScreen(ptScreen);
				var rectTarget = new Rect(0, 0, this.AssociatedObject.ActualWidth, this.AssociatedObject.ActualHeight);

				if (rectTarget.Contains(ptClient))
				{
					handled = true;
					return (IntPtr)HitTestValues.HTBOTTOMRIGHT;
				}
			}

			return IntPtr.Zero;
		}
	}
}
