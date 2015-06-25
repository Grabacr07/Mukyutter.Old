using System;
using System.Collections.Generic;
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
using Codeplex.OAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Utilities.Reactive;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public static partial class RestApi
	{

		public class RequestTokenData
		{
			internal RequestTokenData() { }
			public RequestToken Token { get; internal set; }
			public Uri AuthorizeUrl { get; internal set; }
		}

		public static IObservable<RequestTokenData> GetRequestToken(this TwitterClient client, string consumerKey, string consumerSecret)
		{
			var authorizer = new OAuthAuthorizer(consumerKey, consumerSecret);
			if (client.CurrentNetworkProfile != null)
			{
				if (client.CurrentNetworkProfile.Proxy != null)
				{
					authorizer.ApplyBeforeRequest += req => req.Proxy = client.CurrentNetworkProfile.Proxy.GetProxy();
				}
			}

			return Observable
				.Defer(() => authorizer.GetRequestToken(RestApi.OAuthEndpoints["oauth/request_token"].Url))
				.OnErrorRetry(3)
				.Select(res => new RequestTokenData
				{
					Token = res.Token,
					AuthorizeUrl = new Uri(authorizer.BuildAuthorizeUrl(RestApi.OAuthEndpoints["oauth/authorize"].Url, res.Token)),
				});
		}



		public class AccessTokenData
		{
			internal AccessTokenData() { }
			public AccessToken Token { get; internal set; }
			public string ScreenName { get; internal set; }
			public long UserId { get; internal set; }
		}

		public static IObservable<AccessTokenData> GetAccessToken(this TwitterClient client, string consumerKey, string consumerSecret, RequestToken token, string pincode)
		{
			var authorizer = new OAuthAuthorizer(consumerKey, consumerSecret);
			if (client.CurrentNetworkProfile != null)
			{
				if (client.CurrentNetworkProfile.Proxy != null)
				{
					authorizer.ApplyBeforeRequest += req => req.Proxy = client.CurrentNetworkProfile.Proxy.GetProxy();
				}
			}

			return Observable
				.Defer(() => authorizer.GetAccessToken(RestApi.OAuthEndpoints["oauth/access_token"].Url, token, pincode))
				.OnErrorRetry(3)
				.Select(res => new AccessTokenData
				{
					Token = res.Token,
					ScreenName = res.ExtraData["screen_name"].FirstOrDefault(),
					UserId = Convert.ToInt64(res.ExtraData["user_id"].FirstOrDefault()),
				});
		}
	}
}
