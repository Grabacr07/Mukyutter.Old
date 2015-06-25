using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AsyncOAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	partial class RestApi
	{
		public static async Task<Configuration> GetConfiguration(this TwitterToken token)
		{
			var endpoint = token.Endpoints["help/configuration"];
			return await endpoint.GetAsync(token, null, null, Configuration.Parse, t => t.GetConfiguration());
		}
	}
}
