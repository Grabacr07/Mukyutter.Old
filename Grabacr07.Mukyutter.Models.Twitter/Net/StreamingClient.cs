using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncOAuth;
using Codeplex.OAuth;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public class StreamingClient
	{
		private OAuthClient client;
		private TwitterEndpoint endpoint;

		public StreamingClient(TwitterToken token, string endpoint)
		{
			this.endpoint = token.Endpoints[endpoint];
			this.client = new OAuthClient(
				token.Application.ConsumerKey,
				token.Application.ConsumerSecret,
				token.TokenKey,
				token.TokenSecret)
			{
				Url = this.endpoint.Definition.Url,
				MethodType = this.endpoint.Definition.MethodType == HttpMethod.Get
					? MethodType.Get
					: MethodType.Post,
			};

			var profile = TwitterClient.Current.CurrentNetworkProfile;
			if (profile != null)
			{
				if (profile.Proxy != null)
				{
					this.client.ApplyBeforeRequest += req => req.Proxy = profile.Proxy.GetProxy();
				}
			}
		}

		public IObservable<string> Streaming()
		{
			return client.GetResponse().SelectMany(res =>
				Observable.Create<string>(observer => () =>
				{
					using (var stream = res.GetResponseStream())
					using (var reader = new StreamReader(stream))
					{
						try
						{
							while (!reader.EndOfStream)
							{
								var line = reader.ReadLine();
								observer.OnNext(line);
							}
						}
						catch (Exception ex)
						{
							observer.OnError(ex);
						}
						observer.OnCompleted();
					}
				}));
		}

	}
}
