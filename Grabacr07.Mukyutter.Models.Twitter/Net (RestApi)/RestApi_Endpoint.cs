using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AsyncOAuth;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	partial class RestApi
	{
		/// <summary>
		/// エンド ポイント定義のコレクションを取得します。
		/// </summary>
		public static IReadOnlyCollection<EndpointDefinition> Endpoints { get; private set; }

		public static IReadOnlyDictionary<string, EndpointDefinition> OAuthEndpoints { get; private set; }

		public static void InitializeEndpoint()
		{
			RestApi.Endpoints = new List<EndpointDefinition>
			{
				new EndpointDefinition("statuses/update", "https://api.twitter.com/1.1/statuses/update.json", HttpMethod.Post),
				new EndpointDefinition("statuses/update_with_media", "https://api.twitter.com/1.1/statuses/update_with_media.json", HttpMethod.Post),
				new EndpointDefinition("statuses/home_timeline", "https://api.twitter.com/1.1/statuses/home_timeline.json", HttpMethod.Get),
				new EndpointDefinition("statuses/user_timeline", "https://api.twitter.com/1.1/statuses/user_timeline.json", HttpMethod.Get),
				new EndpointDefinition("statuses/mentions_timeline", "https://api.twitter.com/1.1/statuses/mentions_timeline.json", HttpMethod.Get),
				new EndpointDefinition("statuses/show/:id", "https://api.twitter.com/1.1/statuses/show.json", HttpMethod.Get),
				new EndpointDefinition("statuses/destroy/:id", "https://api.twitter.com/1.1/statuses/destroy/:id.json", HttpMethod.Post),
				new EndpointDefinition("statuses/retweet/:id", "https://api.twitter.com/1.1/statuses/retweet/:id.json", HttpMethod.Post),
				new EndpointDefinition("favorites/list", "https://api.twitter.com/1.1/favorites/list.json", HttpMethod.Get),
				new EndpointDefinition("favorites/create", "https://api.twitter.com/1.1/favorites/create.json", HttpMethod.Post),
				new EndpointDefinition("favorites/destroy", "https://api.twitter.com/1.1/favorites/destroy.json", HttpMethod.Post),
				new EndpointDefinition("followers/ids", "https://api.twitter.com/1.1/followers/ids.json", HttpMethod.Get),
				new EndpointDefinition("friends/ids", "https://api.twitter.com/1.1/friends/ids.json", HttpMethod.Get),
				new EndpointDefinition("users/show", "https://api.twitter.com/1.1/users/show.json", HttpMethod.Get),
				new EndpointDefinition("userstream/user", "https://userstream.twitter.com/1.1/user.json", HttpMethod.Get),
				new EndpointDefinition("lists/list", "http://api.twitter.com/1.1/lists/list.json", HttpMethod.Get),
				new EndpointDefinition("lists/statuses", "https://api.twitter.com/1.1/lists/statuses.json", HttpMethod.Get),
				new EndpointDefinition("lists/show", "https://api.twitter.com/1.1/lists/show.json", HttpMethod.Get),
				new EndpointDefinition("lists/members", "https://api.twitter.com/1.1/lists/members.json", HttpMethod.Get),
				new EndpointDefinition("direct_messages", "https://api.twitter.com/1.1/direct_messages.json", HttpMethod.Get),
				new EndpointDefinition("direct_messages/sent", "https://api.twitter.com/1.1/direct_messages/sent.json", HttpMethod.Get),
				new EndpointDefinition("direct_messages/new", "https://api.twitter.com/1.1/direct_messages/new.json", HttpMethod.Post),
				new EndpointDefinition("direct_messages/destroy", "https://api.twitter.com/1.1/direct_messages/destroy.json", HttpMethod.Post),
				new EndpointDefinition("direct_messages/show", "https://api.twitter.com/1.1/direct_messages/show.json", HttpMethod.Get),
				//new EndpointDefinition("", "", HttpMethod.Get),
				//new EndpointDefinition("", "", HttpMethod.Get),
				//new EndpointDefinition("", "", HttpMethod.Get),
				//new EndpointDefinition("", "", HttpMethod.Get),
				//new EndpointDefinition("", "", HttpMethod.Get),
				//new EndpointDefinition("", "", HttpMethod.Get),
				//new EndpointDefinition("", "", HttpMethod.Get),
				//new EndpointDefinition("", "", HttpMethod.Get),
				//new EndpointDefinition("", "", HttpMethod.Get),
				new EndpointDefinition("help/configuration", "https://api.twitter.com/1.1/help/configuration.json", HttpMethod.Get),
				new EndpointDefinition("application/rate_limit_status", "https://api.twitter.com/1.1/application/rate_limit_status.json", HttpMethod.Get),
			};

			var oauthEndpoints = new List<EndpointDefinition>
			{
				new EndpointDefinition("oauth/request_token", "https://api.twitter.com/oauth/request_token", HttpMethod.Post),
				new EndpointDefinition("oauth/authorize", "https://api.twitter.com/oauth/authorize", HttpMethod.Post),
				new EndpointDefinition("oauth/access_token", "https://api.twitter.com/oauth/access_token", HttpMethod.Post),
			};
			RestApi.OAuthEndpoints = oauthEndpoints.ToDictionary(ed => ed.Name, ed => ed);
		}

	}
}
