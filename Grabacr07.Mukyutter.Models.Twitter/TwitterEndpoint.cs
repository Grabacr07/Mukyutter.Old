using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter
{
	public class TwitterEndpoint : NotificationObject
	{
		private TwitterToken owner;

		public EndpointDefinition Definition { get; private set; }
		public RateLimit RateLimit { get; private set; }

		public TwitterEndpoint(TwitterToken owner, EndpointDefinition definition)
		{
			this.owner = owner;
			this.Definition = definition;
			this.RateLimit = new RateLimit();
		}
	}
}
