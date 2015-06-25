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
using Codeplex.OAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Utilities.Reactive;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	partial class RestApi
	{
		public static IObservable<ICollection<List>> GetLists(this TwitterAccount account, UserId? userId = null)
		{
			const string endpoint = "lists/list";
			var client = account.ToOAuthClient(endpoint);
			client.Parameters.Add("user_id", userId ?? account.UserId);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(res => ListCollection.Parse(res, account.UserId))
				.OnErrorRetry(3)
				.WriteLine(endpoint, lists => lists.Count);
		}

		public static IObservable<List> ShowList(this TwitterAccount account, string owner, string slug)
		{
			const string endpoint = "lists/show";
			var client = account.ToOAuthClient(endpoint);
			client.Parameters.Add("slug", slug);
			client.Parameters.Add("owner_screen_name", owner);

			return ShowListCore(client, endpoint, account.UserId);
		}

		public static IObservable<List> ShowList(this TwitterAccount account, UserId owner, string slug)
		{
			const string endpoint = "lists/show";
			var client = account.ToOAuthClient(endpoint);
			client.Parameters.Add("slug", slug);
			client.Parameters.Add("owner_id", owner);

			return ShowListCore(client, endpoint, account.UserId);
		}

		public static IObservable<List> ShowList(this TwitterAccount account, ListId listId)
		{
			const string endpoint = "lists/show";
			var client = account.ToOAuthClient(endpoint);
			client.Parameters.Add("list_id", listId);

			return ShowListCore(client, endpoint, account.UserId);
		}

		private static IObservable<List> ShowListCore(OAuthClient client, string endpoint, UserId ownerId)
		{
			return Observable
				.Defer(() => client.GetResponseText())
				.Select(json => List.Parse(json, ownerId))
				.OnErrorRetry(3)
				.WriteLine(endpoint, list => list.FullName);
		}

		public static IObservable<StatusCollection> GetListStatuses(
			this TwitterAccount account,
			ListId listId,
			StatusId? sinceId = null,
			StatusId? maxId = null,
			int? count = null,
			bool? includeEntities = null,
			bool? includeRts = true)
		{
			const string endpoint = "lists/statuses";
			var client = account.ToOAuthClient(endpoint);

			client.Parameters.Add("list_id", listId);
			client.Parameters.Add("owner_id", account.UserId);
			if (sinceId.HasValue) client.Parameters.Add("since_id", sinceId.Value);
			if (maxId.HasValue) client.Parameters.Add("max_id", maxId.Value);
			if (count.HasValue) client.Parameters.Add("count", count.Value);
			if (includeEntities.HasValue) client.Parameters.Add("include_entities", includeEntities.Value);
			if (includeRts.HasValue) client.Parameters.Add("include_rts", includeRts.Value);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(json => StatusCollection.Parse(json))
				.OnErrorRetry(3)
				.WriteLine(endpoint);
		}

		public static IObservable<ICollection<User>> GetListMembers(this TwitterAccount account, ListId listId, bool? skipStatus = true)
		{
			const string endpoint = "lists/members";
			var cursor = -1L;

			return Observable
				.Defer(() =>
				{
					var client = account.ToOAuthClient(endpoint);
					client.Parameters.Add("cursor", cursor);
					client.Parameters.Add("list_id", listId);
					if (skipStatus.HasValue) client.Parameters.Add("skip_status", skipStatus.Value);

					return client.GetResponseText();
				})
				.Select(Collection.ParseUser)
				.Do(users => cursor = users.NextCursor)
				.DoWhile(() => cursor != -1 && cursor != 0)
				.Aggregate((user1, user2) => { user1.Objects.AddRange(user2.Objects); return user1; })
				.Select(users => users.Objects)
				.Do(users =>
				{
					var list = TwitterClient.Current.Lists[listId, account.UserId] ?? TwitterClient.Current.Lists.Add(listId, account.UserId);
					list.Members = new HashSet<UserId>(users.Select(u => u.Id));
					list.HasMembers = true;
				})
				.OnErrorRetry(3)
				.WriteLine(endpoint, ids => ids.Count + " users");
		}
	}
}
