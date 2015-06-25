using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using AsyncOAuth;
using Codeplex.OAuth;
using Grabacr07.Utilities.Reactive;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using System.IO;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using System.Net.Http;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public class TwitterOAuthClient : OAuthClient
	{
		private TwitterEndpoint endpoint;

		public TwitterOAuthClient(TwitterToken token, string endpoint, IDictionary<string, object> param = null)
			: base(token.Application.ConsumerKey, token.Application.ConsumerSecret, token.TokenKey, token.TokenSecret)
		{
			this.endpoint = token.Endpoints[endpoint];

			var url = this.endpoint.Definition.Url;
			if (param != null) param.ForEach(kvp => url = url.Replace(kvp.Key, kvp.Value.ToString()));

			this.Url = url;
			this.MethodType = this.endpoint.Definition.MethodType == HttpMethod.Get
				? MethodType.Get
				: MethodType.Post;
		}

		protected override WebRequest CreateWebRequest()
		{
			var req = base.CreateWebRequest();

			req.Headers.Add(TwitterDefinitions.HttpResponse.RateLimitClass, "");
			req.Headers.Add(TwitterDefinitions.HttpResponse.RateLimit, "");
			req.Headers.Add(TwitterDefinitions.HttpResponse.RateLimitRemaining, "");
			req.Headers.Add(TwitterDefinitions.HttpResponse.RateLimitReset, "");

			return req;
		}

		public override IObservable<string> GetResponseText()
		{
			return this.GetResponse()
				//.Do(res => this.endpoint.RateLimit.Set(res.Headers))
				.Catch(
					(WebException ex) =>
					{
						Exception result = ex;

						if (ex.Response != null)
						{
							using (var res = ex.Response.GetResponseStream())
							using (var read = new StreamReader(res))
							{
//								var json = read.ReadToEnd();
//								Error[] errors = null;
//								if (Errors.TryParse(json, out errors)) result = new ApiException(errors, ex);
							}
						}

						return Observable.Throw<WebResponse>(result);
					})
				.SelectMany(res => res.DownloadStringAsync());
		}
	}
}
