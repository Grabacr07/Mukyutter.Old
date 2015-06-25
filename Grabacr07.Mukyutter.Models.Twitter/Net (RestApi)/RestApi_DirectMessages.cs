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
		public static IObservable<DirectMessage> SendDirectMessages(
			this TwitterAccount account,
			UserId userId,
			string text)
		{
			const string endpoint = "direct_messages/new";
			var client = account.ToOAuthClient(endpoint);

			client.Parameters.Add("user_id", userId);
			client.Parameters.Add("text", Helper.TrimIfOverLength(text));

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(json => DirectMessage.Parse(json))
				.OnErrorRetry(3)
				.WriteLine(endpoint, msg => msg.Text);
		}

		public static IObservable<DirectMessage> DestroyDirectMessages(
			this TwitterAccount account,
			UserId userId,
			bool? includeEntities = false)
		{
			const string endpoint = "direct_messages/destroy";
			var client = account.ToOAuthClient(endpoint);

			client.Parameters.Add("user_id", userId);
			if (includeEntities.HasValue) client.Parameters.Add("include_entities", includeEntities.Value);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(json => DirectMessage.Parse(json))
				.OnErrorRetry(3)
				.WriteLine(endpoint, msg => msg.Text);
		}

		public static IObservable<DirectMessage> ShowDirectMessages(
			this TwitterAccount account,
			UserId userId)
		{
			const string endpoint = "direct_messages/show";
			var client = account.ToOAuthClient(endpoint);

			client.Parameters.Add("user_id", userId);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(json => DirectMessage.Parse(json))
				.OnErrorRetry(3)
				.WriteLine(endpoint, msg => msg.Text);
		}

		/// <summary>
		/// 現在のアカウントが受信したダイレクト メッセージを取得します。
		/// </summary>
		public static IObservable<DirectMessageCollection> GetDirectMessagesBy(
			this TwitterAccount account,
			int? count = null,
			StatusId? sinceId = null,
			StatusId? maxId = null)
		{
			const string endpoint = "direct_messages";
			var client = account.ToOAuthClient(endpoint);

			if (count.HasValue) client.Parameters.Add("count", count.Value);
			if (sinceId.HasValue) client.Parameters.Add("since_id", sinceId.Value);
			if (maxId.HasValue) client.Parameters.Add("max_id", maxId.Value);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(json => DirectMessageCollection.Parse(json))
				.OnErrorRetry(3)
				.WriteLine(endpoint);
		}

		/// <summary>
		/// 現在のアカウントが送信したダイレクト メッセージを取得します。
		/// </summary>
		public static IObservable<DirectMessageCollection> GetDirectMessagesTo(
			this TwitterAccount account,
			int? count = null,
			StatusId? sinceId = null,
			StatusId? maxId = null)
		{
			const string endpoint = "direct_messages/sent";
			var client = account.ToOAuthClient(endpoint);

			if (count.HasValue) client.Parameters.Add("count", count.Value);
			if (sinceId.HasValue) client.Parameters.Add("since_id", sinceId.Value);
			if (maxId.HasValue) client.Parameters.Add("max_id", maxId.Value);

			return Observable
				.Defer(() => client.GetResponseText())
				.Select(json => DirectMessageCollection.Parse(json))
				.OnErrorRetry(3)
				.WriteLine(endpoint);
		}
	}
}
