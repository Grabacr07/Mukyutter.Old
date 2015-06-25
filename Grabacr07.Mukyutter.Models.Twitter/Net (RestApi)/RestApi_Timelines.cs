using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	partial class RestApi
	{
		public static async Task<StatusCollection> GetHomeTimeline(
			this TwitterToken token,
			int? count = null,
			StatusId? sinceId = null,
			StatusId? maxId = null,
			bool notify = true,
			bool isStartup = false)
		{
			var endpoint = token.Endpoints["statuses/home_timeline"];
			var source = isStartup
				? StatusSource.RestApi | StatusSource.Startup
				: StatusSource.RestApi;

			var param = new Dictionary<string, string>();
			if (count.HasValue) param.Add("count", count.Value.ToString(CultureInfo.InvariantCulture));
			if (sinceId.HasValue) param.Add("since_id", sinceId.Value.ToString());
			if (maxId.HasValue) param.Add("max_id", maxId.Value.ToString());

			return await endpoint.GetAsync(
				token, param, null, json => StatusCollection.Parse(json, source),
				t => t.GetHomeTimeline(count, sinceId, maxId));
		}


		public static IObservable<StatusCollection> GetHomeTimeline(
			this TwitterAccount account,
			int? count = null,
			StatusId? sinceId = null,
			StatusId? maxId = null,
			bool isStartup = false)
		{
			const string endpoint = "statuses/home_timeline";
			var source = isStartup
				? StatusSource.RestApi | StatusSource.Startup
				: StatusSource.RestApi;
			var client = account.ToOAuthClient(endpoint);

			if (count.HasValue) client.Parameters.Add("count", count.Value);
			if (sinceId.HasValue) client.Parameters.Add("since_id", sinceId.Value);
			if (maxId.HasValue) client.Parameters.Add("max_id", maxId.Value);

			return Observable
				.Defer(client.GetResponseText)
				.Select(json => StatusCollection.Parse(json, source))
				.OnErrorRetry(3)
				.WriteLine(endpoint);
		}


		public static IObservable<StatusCollection> GetUserTimeline(
			this TwitterAccount account,
			UserId? userId = null,
			int? count = null,
			StatusId? sinceId = null,
			StatusId? maxId = null,
			bool? trimUser = null,
			bool? excludeReplies = null,
			bool? contributorDetails = null,
			bool? includeRts = null,
			bool isStartup = false)
		{
			const string endpoint = "statuses/user_timeline";
			var source = isStartup
				? StatusSource.RestApi | StatusSource.Startup
				: StatusSource.RestApi;
			var client = account.ToOAuthClient(endpoint);

			if (userId.HasValue) client.Parameters.Add("user_id", userId.Value);
			if (count.HasValue) client.Parameters.Add("count", count.Value);
			if (sinceId.HasValue) client.Parameters.Add("since_id", sinceId.Value);
			if (maxId.HasValue) client.Parameters.Add("max_id", maxId.Value);
			if (trimUser.HasValue) client.Parameters.Add("trim_user", trimUser.Value);
			if (excludeReplies.HasValue) client.Parameters.Add("exclude_replies", excludeReplies.Value);
			if (contributorDetails.HasValue) client.Parameters.Add("contributor_details", contributorDetails.Value);
			if (includeRts.HasValue) client.Parameters.Add("include_rts", includeRts.Value);

			return Observable
				.Defer(client.GetResponseText)
				.Select(json => StatusCollection.Parse(json, source))
				.OnErrorRetry(3)
				.WriteLine(endpoint);
		}


		public static IObservable<StatusCollection> GetMentionsTimeline(
			this TwitterAccount account,
			int? count = null,
			StatusId? sinceId = null,
			StatusId? maxId = null,
			bool? trimUser = null,
			bool? contributorDetails = null,
			bool? includeEntities = null,
			bool isStartup = false)
		{
			const string endpoint = "statuses/mentions_timeline";
			var source = isStartup
				? StatusSource.RestApi | StatusSource.Startup
				: StatusSource.RestApi;
			var client = account.ToOAuthClient(endpoint);

			if (count.HasValue) client.Parameters.Add("count", count.Value);
			if (sinceId.HasValue) client.Parameters.Add("since_id", sinceId.Value);
			if (maxId.HasValue) client.Parameters.Add("max_id", maxId.Value);
			if (trimUser.HasValue) client.Parameters.Add("trim_user", trimUser.Value);
			if (contributorDetails.HasValue) client.Parameters.Add("contributor_details", contributorDetails.Value);
			if (includeEntities.HasValue) client.Parameters.Add("include_entities", includeEntities.Value);

			return Observable
				.Defer(client.GetResponseText)
				.Select(json => StatusCollection.Parse(json, source))
				.OnErrorRetry(3)
				.WriteLine(endpoint);
		}
	}
}
