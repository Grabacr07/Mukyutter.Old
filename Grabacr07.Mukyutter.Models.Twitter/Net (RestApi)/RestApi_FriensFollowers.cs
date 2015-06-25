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
		public static IObservable<UserIdCollection> GetFollowers(this TwitterAccount account, UserId? userId = null)
		{
			const string endpoint = "followers/ids";
			var cursor = -1L;

			return Observable
				.Defer(() =>
				{
					var client = account.ToOAuthClient(endpoint);

					client.Parameters.Add("cursor", cursor);
					if (userId.HasValue) client.Parameters.Add("user_id", userId.Value);

					return client.GetResponseText();
				})
				.Select(Collection.ParseUserId)
				.Do(ids => cursor = ids.NextCursor)
				.DoWhile(() => cursor != -1 && cursor != 0)
				.Aggregate((ids1, ids2) => { ids1.Objects.AddRange(ids2.Objects); return ids1; })
				.Select(ids => new UserIdCollection(ids.Objects))
				.OnErrorRetry(3)
				.WriteLine(endpoint, ids => ids.Count + " users");
		}


		public static IObservable<UserIdCollection> GetFriends(this TwitterAccount account, UserId? userId = null)
		{
			const string endpoint = "friends/ids";
			var cursor = -1L;

			return Observable
				.Defer(() =>
				{
					var client = account.ToOAuthClient(endpoint);

					client.Parameters.Add("cursor", cursor);
					if (userId.HasValue) client.Parameters.Add("user_id", userId.Value);

					return client.GetResponseText();
				})
				.Select(Collection.ParseUserId)
				.Do(ids => cursor = ids.NextCursor)
				.DoWhile(() => cursor != -1 && cursor != 0)
				.Aggregate((ids1, ids2) => { ids1.Objects.ToList().AddRange(ids2.Objects); return ids1; })
				.Select(ids => new UserIdCollection(ids.Objects))
				.OnErrorRetry(3)
				.WriteLine(endpoint, ids => ids.Count + " users");
		}

	}
}
