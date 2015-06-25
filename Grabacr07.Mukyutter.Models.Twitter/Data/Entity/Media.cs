using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Entity
{
	public class Media : IEntity
	{
		public ulong Id { get; internal set; }

		public Uri MediaUrl { get; internal set; }

		public Uri MediaUrlHttps { get; internal set; }

		public Uri Url { get; internal set; }

		public string DisplayUrl { get; internal set; }

		public Uri ExpandedUrl { get; internal set; }

		public string Type { get; internal set; }

		public Dictionary<string, Size> Sizes { get; internal set; }

		public Indices Indices { get; internal set; }
	}
}
