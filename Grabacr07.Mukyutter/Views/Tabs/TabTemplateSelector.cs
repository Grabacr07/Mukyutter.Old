using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Grabacr07.Mukyutter.ViewModels.Tabs;
using Grabacr07.Mukyutter.ViewModels.Tabs.TimelineTabs;

namespace Grabacr07.Mukyutter.Views.Tabs
{
	class TabTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is TimelineTabViewModel)
			{
				return ((FrameworkElement)container).FindResource("TimelineTabTemplateKey") as DataTemplate;
			}

			if (item is DevTabViewModel)
			{
				return ((FrameworkElement)container).FindResource("DevTabTemplateKey") as DataTemplate;
			}

			if (item is EventTabViewModel)
			{
				return ((FrameworkElement)container).FindResource("EventTabTemplateKey") as DataTemplate;
			}

			if (item is SystemTabViewModel)
			{
				return ((FrameworkElement)container).FindResource("SystemTabTemplateKey") as DataTemplate;
			}

			return null;
		}
	}
}
