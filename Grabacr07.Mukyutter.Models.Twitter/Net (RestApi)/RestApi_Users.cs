using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Joins;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading;
using System.Text;
using AsyncOAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Utilities.Reactive;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	partial class RestApi
	{
		public static IObservable<User> GetUser(this TwitterAccount account, UserId userId)
		{
			const string endpoint = "users/show";
			var client = account.ToOAuthClient(endpoint);
			client.Parameters.Add("user_id", userId);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(res => User.Parse(res))
				.OnErrorRetry(3)
				.WriteLine(endpoint, u => u.ScreenName + " / " + u.Name);
		}

	}
}
