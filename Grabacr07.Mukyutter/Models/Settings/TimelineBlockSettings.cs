using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Settings
{
	[Serializable]
	public class TimelineBlockSettings : BlockSettings
	{
		public UserId Account { get; set; }

		public List<ListSettings> SubscribedLists { get; set; }

		public string FilterVersion { get; set; }
		public string FilterQuery { get; set; }

		public bool IsUnreadCountDisplaying { get; set; }
		public bool IsNotified { get; set; }
	}

	[Serializable]
	public class ListSettings
	{
		public ListId Id { get; set; }
		public UserId OwnerId { get; set; }
		public string Name { get; set; }
	}
}
