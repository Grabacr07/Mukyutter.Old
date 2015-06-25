using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Grabacr07.Mukyutter.Views.Internal
{
	static class ViewExtensions
	{
		public static T GetParent<T>(this DependencyObject target) where T : DependencyObject
		{
			var self = target as T;
			if (self != null)
			{
				return self;
			}

			var parent = VisualTreeHelper.GetParent(target) as DependencyObject;
			if (parent != null)
			{
				return parent.GetParent<T>();
			}

			return null;
		}


		public static bool Contains(this ModifierKeys modifier, Key key)
		{
			switch (modifier)
			{
				case ModifierKeys.Control:
					return key == Key.LeftCtrl || key == Key.RightCtrl;
				case ModifierKeys.Alt:
					return key == Key.LeftAlt || key == Key.RightAlt;
				case ModifierKeys.Shift:
					return key == Key.LeftShift || key == Key.RightShift;
				case ModifierKeys.Windows:
					return key == Key.LWin || key == Key.RWin;
				default:
					return false;
			}
		}

		public static bool IsModifier(this Key key)
		{
			return key == Key.LeftCtrl || key == Key.RightCtrl ||
				   key == Key.LeftAlt || key == Key.RightAlt ||
				   key == Key.LeftShift || key == Key.RightShift ||
				   key == Key.LWin || key == Key.RWin ||
				   key == Key.System;
		}
	}
}
