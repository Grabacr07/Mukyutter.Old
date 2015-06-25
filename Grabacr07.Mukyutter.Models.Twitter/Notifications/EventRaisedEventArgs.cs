using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Events;

namespace Grabacr07.Mukyutter.Models.Twitter.Notifications
{
	public class EventRaisedEventArgs : EventArgs
	{
		public Event Event { get; internal set; }
	}
}
