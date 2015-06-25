using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Grabacr07.Mukyutter.Views.Twitter
{
	class TimelineDataTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			return ((FrameworkElement)container).FindResource("TimelineViewStatusTemplateKey") as DataTemplate;
		}
	}
}
