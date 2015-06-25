using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.ViewModels.Tabs.TimelineTabs;

namespace Grabacr07.Mukyutter.ViewModels.Tabs
{
	static class TabTypeConverter
	{
		public static TabViewModel ToTabViewModel(this Block block)
		{
			if (block is TimelineBlock)
			{
				var vm = new TimelineTabViewModel((TimelineBlock)block);
				return vm;
			}

			return null;
		}
	}
}
