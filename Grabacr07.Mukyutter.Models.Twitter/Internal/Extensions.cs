using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities.Development;
using Grabacr07.Utilities.Reactive;

namespace Grabacr07.Mukyutter.Models.Twitter.Internal
{
	internal static class Extensions
	{
		/// <summary>
		/// Twitter のユーザー トークンから user id を抽出します。
		/// </summary>
		public static UserId ToUserId(this TwitterToken token)
		{
			try
			{
				return Int64.Parse(token.TokenKey.Split('-')[0]);
			}
			catch (Exception ex)
			{
				TwitterClient.Current.ReportException(string.Format("Token ({0}) からユーザー ID を抽出できませんでした。", token.TokenKey), ex);
				return 0;
			}
		}


		public static TwitterOAuthClient ToOAuthClient(this TwitterAccount account, string endpoint, StatusId paramId)
		{
			return account.ToOAuthClient(endpoint, param: new Dictionary<string, object> { { ":id", paramId } });
		}

		public static TwitterOAuthClient ToOAuthClient(this TwitterAccount account, string endpoint,
			NetworkProfile profile = null, IDictionary<string, object> param = null)
		{
			var oauthClient = new TwitterOAuthClient(account.CurrentToken, endpoint, param);

			profile = profile ?? TwitterClient.Current.CurrentNetworkProfile;
			if (profile != null)
			{
				if (profile.Proxy != null)
				{
					oauthClient.ApplyBeforeRequest += req => req.Proxy = profile.Proxy.GetProxy();
				}
			}

			return oauthClient;
		}


		internal static IObservable<T> WriteLine<T, TValue>(this IObservable<T> source, string name, Func<T, TValue> getValue)
		{
#if DEBUG
			return source.Do(data => DebugMonitor.WriteLine("{0}: {1}", name, getValue(data)));
#else
			return source;
#endif
		}

		internal static IObservable<StatusCollection> WriteLine(this IObservable<StatusCollection> source, string endpoint)
		{
#if DEBUG
			return source.WriteLine(endpoint, statuses => statuses.Count() + " statuses");
#else
			return source;
#endif
		}

		internal static IObservable<DirectMessageCollection> WriteLine(this IObservable<DirectMessageCollection> source,
			string endpoint)
		{
#if DEBUG
			return source.WriteLine(endpoint, statuses => statuses.Count() + " messages");
#else
			return source;
#endif
		}

		internal static IObservable<T> OnErrorRetryFromStreaming<T>(this IObservable<T> source, string name)
		{
			return
				source.OnErrorRetry(
					(Exception ex) =>
						TwitterClient.Current.ReportException(string.Format("User streams から配信された {0} の解析に失敗しました", name), ex));
		}


		internal static void Report(this Exception ex, string message, Action retryAction = null)
		{
			TwitterClient.Current.ReportException(message, ex, retryAction);
		}
	}
}
