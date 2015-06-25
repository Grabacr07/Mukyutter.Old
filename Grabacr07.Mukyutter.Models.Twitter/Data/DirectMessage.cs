using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Entity;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Internal;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	public class DirectMessage : StatusBase
	{
		public User Recipient { get; set; }
		public User Sender { get; set; }


		public static DirectMessage Parse(string json)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			DynamicJsonHelper.ThrowIfError(json);

			return ParseCore(djson);
		}

		internal static DirectMessage ParseCore(dynamic djson)
		{
			return TwitterClient.Current.Messages.Parse(djson);
		}
	}
}
