using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Grabacr07.Mukyutter.Views.Behaviors
{
	class WindowDragBehavior : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			this.AssociatedObject.MouseLeftButtonDown += (sender, e) =>
			{
				var window = Window.GetWindow(this.AssociatedObject);
				if (window != null) window.DragMove();
			};

			base.OnAttached();
		}
	}
}
