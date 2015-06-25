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
		public static IObservable<StatusCollection> GetFavorites(
			this TwitterAccount account,
			UserId? userId = null,
			int? count = null,
			StatusId? sinceId = null,
			StatusId? maxId = null)
		{
			const string endpoint = "favorites/list";
			var client = account.ToOAuthClient(endpoint);

			if (userId.HasValue) client.Parameters.Add("user_id", userId.Value);
			if (count.HasValue) client.Parameters.Add("count", count.Value);
			if (sinceId.HasValue) client.Parameters.Add("since_id", sinceId.Value);
			if (maxId.HasValue) client.Parameters.Add("max_id", maxId.Value);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(json => StatusCollection.Parse(json))
				.OnErrorRetry(3)
				.WriteLine(endpoint, ids => ids.Count() + " statuses");
		}

		public static IObservable<Status> CreateFavorites(this TwitterAccount account, StatusId statusId)
		{
			const string endpoint = "favorites/create";
			var client = account.ToOAuthClient(endpoint);

			client.Parameters.Add("id", statusId);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(res => Status.Parse(res))
				.OnErrorRetry(3)
				.WriteLine(endpoint, s => s.Text);
		}

		public static IObservable<Status> DestroyFavorites(this TwitterAccount account, StatusId statusId)
		{
			const string endpoint = "favorites/destroy";
			var client = account.ToOAuthClient(endpoint);

			client.Parameters.Add("id", statusId);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(res => Status.Parse(res))
				.OnErrorRetry(3)
				.WriteLine(endpoint, s => s.Text);
		}
	}
}
