using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Entity
{
	public class Url : IEntity
	{
		public Uri EntityUrl { get; internal set; }

		public string DisplayUrl { get; internal set; }

		public Uri ExpandedUrl { get; internal set; }

		public Indices Indices { get; internal set; }
	}
}
