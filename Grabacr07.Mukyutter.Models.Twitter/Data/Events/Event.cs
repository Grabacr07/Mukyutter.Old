using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Events
{
	public abstract class Event
	{
		public User Source { get; internal set; }

		public User Target { get; internal set; }

		public DateTime CreatedAt { get; internal set; }

		internal Event()
		{
		}
	}
}
