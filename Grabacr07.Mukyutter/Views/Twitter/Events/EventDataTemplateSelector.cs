using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Grabacr07.Mukyutter.ViewModels.Twitter.Events;

namespace Grabacr07.Mukyutter.Views.Twitter.Events
{
	class EventDataTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is FavoriteViewModel)
			{
				return ((FrameworkElement)container).FindResource("FavoriteTemplateKey") as DataTemplate;
			}

			if (item is UnfavoriteViewModel)
			{
				return ((FrameworkElement)container).FindResource("UnfavoriteTemplateKey") as DataTemplate;
			}

			if (item is RetweetViewModel)
			{
				return ((FrameworkElement)container).FindResource("RetweetTemplateKey") as DataTemplate;
			}

			if (item is FollowViewModel)
			{
				return ((FrameworkElement)container).FindResource("FollowTemplateKey") as DataTemplate;
			}

			if (item is MentionViewModel)
			{
				return ((FrameworkElement)container).FindResource("MentionTemplateKey") as DataTemplate;
			}

			return null;
		}
	}
}
