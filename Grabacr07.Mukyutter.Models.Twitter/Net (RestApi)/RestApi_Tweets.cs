using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Threading.Tasks;
using AsyncOAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Data.Stores;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;
using Grabacr07.Utilities.Reactive;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Net.Http;
using Grabacr07.Mukyutter.Models.Twitter.Composing;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	partial class RestApi
	{
		/// <summary>
		/// ツイートを投稿します。
		/// </summary>
		public static Task<Status> UpdateStatus(this TwitterToken token, NewStatus status)
		{
			return status.MediaPaths.HasValue()
				? token.UpdateStatus(status.TextWithFooter, status.MediaPaths.ToArray(), status.InReplyTo == null ? null : new StatusId?(status.InReplyTo.Id))
				: token.UpdateStatus(status.TextWithFooter, status.InReplyTo == null ? null : new StatusId?(status.InReplyTo.Id));
		}

		/// <summary>
		/// ツイートを投稿します。
		/// </summary>
		public static async Task<Status> UpdateStatus(
			this TwitterToken token, string status, StatusId? inReplyToStatusId = null)
		{
			var endpoint = token.Endpoints["statuses/update"];

			var param = new Dictionary<string, string> { { "status", status } };
			if (inReplyToStatusId.HasValue) param.Add("in_reply_to_status_id", inReplyToStatusId.Value.ToString());

			using (var client = token.GetClient())
			using (var content = new FormUrlEncodedContent(param))
			{
				return await client.PostAsyncEx(
					endpoint.Definition.Url,
					content,
					res => endpoint.RateLimit.Set(res.Headers),
					json => Status.Parse(json, StatusSource.Update),
					() => token.FallbackToken.UpdateStatus(status, inReplyToStatusId));
			}
		}

		/// <summary>
		/// メディア付きのツイートを投稿します。
		/// </summary>
		public static async Task<Status> UpdateStatus(
			this TwitterToken token, string status, string[] mediaPaths, StatusId? inReplyToStatusId = null)
		{
			// media が空だったら通常の投稿に切り替える
			if (!mediaPaths.HasValue()) return await token.UpdateStatus(status, inReplyToStatusId);

			var endpoint = token.Endpoints["statuses/update_with_media"];

			using (var client = token.GetClient())
			using (var content = new MultipartFormDataContent { { new StringContent(status), "\"status\"" } })
			{
				if (inReplyToStatusId.HasValue)
				{
					content.Add(new StringContent(inReplyToStatusId.ToString()), "\"in_reply_to_status_id\"");
				}
				foreach (var path in mediaPaths)
				{
					var media = File.ReadAllBytes(path);
					var filename = Path.GetFileName(path);
					content.Add(new ByteArrayContent(media), "media[]", "\"" + filename + "\"");
				}

				return await client.PostAsyncEx(
					endpoint.Definition.Url,
					content,
					res => endpoint.RateLimit.Set(res.Headers),
					json => Status.Parse(json, StatusSource.Update),
					() => token.FallbackToken.UpdateStatus(status, mediaPaths, inReplyToStatusId));
			}
		}


		public static async Task<Status> RetweetStatus(this TwitterToken token, StatusId statusId, bool? trimUser = true)
		{
			var endpoint = token.Endpoints["statuses/retweet/:id"];
			var param = new Dictionary<string, string>();
			var urlParam = new Dictionary<string, string> { { ":id", statusId.ToString() }, };
			if (trimUser.HasValue) param.Add("trim_user", trimUser.Value.ToString());

			return await endpoint.GetAsync(token, param, urlParam, Status.Parse, t => t.RetweetStatus(statusId, trimUser));
		}

		public static async Task<Status> DestroyStatus(this TwitterToken token, StatusId statusId, bool? trimUser = true)
		{
			var endpoint = token.Endpoints["statuses/destroy/:id"];
			var param = new Dictionary<string, string>();
			var urlParam = new Dictionary<string, string> { { ":id", statusId.ToString() }, };
			if (trimUser.HasValue) param.Add("trim_user", trimUser.Value.ToString());

			return await endpoint.GetAsync(token, param, urlParam, Status.Parse, t => t.DestroyStatus(statusId, trimUser));
		}

		public static async Task<Status> ShowStatus(this TwitterToken token, StatusId statusId, bool? trimUser = true)
		{
			var endpoint = token.Endpoints["statuses/show/:id"];
			var param = new Dictionary<string, string>();
			var urlParam = new Dictionary<string, string> { { ":id", statusId.ToString() }, };
			if (trimUser.HasValue) param.Add("trim_user", trimUser.Value.ToString());

			return await endpoint.GetAsync(token, param, urlParam, Status.Parse, t => t.ShowStatus(statusId, trimUser));
		}
	}
}
