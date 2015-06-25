using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AsyncOAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public static partial class RestApi
	{
		static RestApi()
		{
			InitializeEndpoint();

			ServicePointManager.Expect100Continue = false;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
		}

		private static Configuration _Configuration;

		public static Configuration Configuration
		{
			get { return _Configuration ?? Configuration.Default; }
			set { _Configuration = value; }
		}


		public static async Task GetRateLimitStatus(this TwitterToken token, params string[] resources)
		{
			var endpoint = token.Endpoints["application/rate_limit_status"];
			var url = endpoint.Definition.CreateUrlWithParam(new Dictionary<string, string>
			{
				{ "resources", resources.ToString(",") },
			});

			await token.GetClient().GetAsyncEx(url, res => endpoint.RateLimit.Set(res.Headers), _ => _, null);
		}


		/// <summary>
		/// Twitter API エンドポイントとの HTTP 通信を実行するための <see cref="System.Net.Http.HttpClient" /> を生成します。
		/// </summary>
		/// <param name="token">Twitter トークン。</param>
		/// <param name="addRateLimitHeaders">Rate limit 情報を取得するためのヘッダーを付与する場合は true、それ以外の場合は false。</param>
		private static HttpClient GetClient(this TwitterToken token, bool addRateLimitHeaders = true)
		{
			var handler = new HttpClientHandler();
			var profile = TwitterClient.Current.CurrentNetworkProfile;
			if (profile != null && profile.Proxy != null && handler.SupportsProxy)
			{
				handler.Proxy = profile.Proxy.GetProxy();
			}

			var client = OAuthUtility.CreateOAuthClient(
				handler,
				token.Application.ConsumerKey,
				token.Application.ConsumerSecret,
				new AccessToken(token.TokenKey, token.TokenSecret));

			if (addRateLimitHeaders)
			{
				client.DefaultRequestHeaders.Add(TwitterDefinitions.HttpResponse.RateLimitClass, "");
				client.DefaultRequestHeaders.Add(TwitterDefinitions.HttpResponse.RateLimit, "");
				client.DefaultRequestHeaders.Add(TwitterDefinitions.HttpResponse.RateLimitRemaining, "");
				client.DefaultRequestHeaders.Add(TwitterDefinitions.HttpResponse.RateLimitReset, "");
			}

			return client;
		}


		/// <summary>
		/// TWitter API エンドポイントの URL を生成します。
		/// "statuses/retweet/:id" のようなエンドポイントを具体的な ID で置き換える場合に使用します。
		/// </summary>
		private static string CreateUrl(this EndpointDefinition endpoint, string paramName, string paramValue)
		{
			return endpoint.Url.Replace(paramName, paramValue);
		}

		/// <summary>
		/// GET 要求のパラメーターを付加し、Twitter API エンドポイントの URL を生成します。
		/// </summary>
		private static string CreateUrlWithParam(
			this EndpointDefinition endpoint,
			IDictionary<string, string> param)
		{
			var url = endpoint.Url;
			if (param.HasValue()) url += "?" + param.Select(kvp => kvp.Key + "=" + kvp.Value).ToString("&");

			return url;
		}


		/// <summary>
		/// 指定された URL に GET 要求を非同期操作として送信します。取得した json データは <typeparamref name="T" />
		/// 型へ変換され、API 制限時はフォールバックされます。
		/// </summary>
		/// <typeparam name="T">POST 要求によって取得するデータの型。</typeparam>
		/// <param name="client">HTTP 要求。</param>
		/// <param name="url">要求の送信先 URL。</param>
		/// <param name="responseHandler">受信した HTTP 応答メッセージを使用して追加の処理を行う場合、その処理を実行するメソッド。必要ない場合は null を指定できます。</param>
		/// <param name="parser">
		/// 取得した json データを <typeparamref name="T" /> 型へ変換するためのメソッド。
		/// </param>
		/// <param name="fallback">フォールバック処理。フォールバックしない場合は null を指定できます。</param>
		/// <returns></returns>
		private static async Task<T> GetAsyncEx<T>(
			this HttpClient client,
			string url,
			Action<HttpResponseMessage> responseHandler,
			Func<string, T> parser,
			Func<Task<T>> fallback)
		{
			try
			{
				var response = await client.GetAsync(url);
				if (responseHandler != null) responseHandler(response);

				var json = await response.Content.ReadAsStringAsync();
				return parser(json);
			}
			catch (ApiException ex)
			{
				if (ex.Errors.All(e => e.Code != 88) || fallback == null) throw;
			}

			return await fallback();
		}

		/// <summary>
		/// 指定された URL に POST 要求を非同期操作として送信します。取得した json データは <typeparamref name="T" />
		/// 型へ変換され、API 制限時はフォールバックされます。
		/// </summary>
		/// <typeparam name="T">POST 要求によって取得するデータの型。</typeparam>
		/// <param name="client">HTTP 要求。</param>
		/// <param name="url">要求の送信先 URL。</param>
		/// <param name="content">サーバーに送信される HTTP 要求の内容。</param>
		/// <param name="responseHandler">受信した HTTP 応答メッセージを使用して追加の処理を行う場合、その処理を実行するメソッド。必要ない場合は null を指定できます。</param>
		/// <param name="parser">
		/// 取得した json データを <typeparamref name="T" /> 型へ変換するためのメソッド。
		/// </param>
		/// <param name="fallback">フォールバック処理。フォールバックしない場合は null を指定できます。</param>
		/// <returns></returns>
		private static async Task<T> PostAsyncEx<T>(
			this HttpClient client,
			string url,
			HttpContent content,
			Action<HttpResponseMessage> responseHandler,
			Func<string, T> parser,
			Func<Task<T>> fallback)
		{
			try
			{
				var response = await client.PostAsync(url, content);
				if (responseHandler != null) responseHandler(response);

				var json = await response.Content.ReadAsStringAsync();
				return parser(json);
			}
			catch (ApiException ex)
			{
				if (ex.Errors.All(e => e.Code != 88) || fallback == null) throw;
			}

			return await fallback();
		}


		/// <summary>
		/// TWitter API エンドポイントの URL を生成します。
		/// </summary>
		/// <param name="endpoint">Twitter API エンドポイント。</param>
		/// <param name="param">GET 要求時に URL に付加するキーと値のペアのコレクション (&lt;url&gt;?key1=value1&amp;key2=value2&amp;...)。POST 要求時は null。</param>
		/// <param name="urlParam">Twitter API エンドポイントの URL パラメーター。"statuses/retweet/:id" のようなエンドポイントを具体的な ID で置き換える場合に使用します。不要な場合は null。</param>
		/// <returns>生成された URL を示す文字列。</returns>
		[Obsolete]
		private static string CreateUrl(
			this EndpointDefinition endpoint,
			IReadOnlyDictionary<string, string> param,
			IReadOnlyDictionary<string, string> urlParam)
		{
			var url = endpoint.Url;
			if (urlParam.HasValue()) urlParam.ForEach(kvp => url = url.Replace(kvp.Key, kvp.Value));
			if (param.HasValue()) url += "?" + param.Select(kvp => kvp.Key + "=" + kvp.Value).ToString("&");

			return url;
		}


		[Obsolete]
		private static async Task<string> GetStringAsync(
			this TwitterEndpoint endpoint, TwitterToken token,
			IReadOnlyDictionary<string, string> param, IReadOnlyDictionary<string, string> urlParam)
		{
			var client = token.GetClient(endpoint.Definition.MethodType == HttpMethod.Get);
			HttpResponseMessage response;

			if (endpoint.Definition.MethodType == HttpMethod.Get)
			{
				response = await client.GetAsync(endpoint.Definition.CreateUrl(param, urlParam));
				endpoint.RateLimit.Set(response.Headers);
			}
			else
			{
				response = await client.PostAsync(endpoint.Definition.CreateUrl(null, urlParam), new FormUrlEncodedContent(param));
			}

			var json = await response.Content.ReadAsStringAsync();
			return json;
		}


		/// <summary>
		/// エンドポイントへ GET または POST 要求を送信し、受信結果を <typeparamref name="T" /> 型に変換します。API 回数制限時は、指定されたトークンにフォールバックします。
		/// </summary>
		/// <typeparam name="T">取得した json を変換する型。</typeparam>
		/// <param name="endpoint">Twitter API エンドポイント。</param>
		/// <param name="token">Twitter API へのアクセスに使用するトークン。</param>
		/// <param name="param">GET または POST 要求のパラメーター。</param>
		/// <param name="urlParam">Twitter API エンドポイントの URL を置き換えるパラメーター。</param>
		/// <param name="parser">
		/// json から <typeparamref name="T" /> 型へ変換するメソッド。
		/// </param>
		/// <param name="fallback">API 制限時にフォールバックするメソッド。</param>
		/// <returns>エンドポイントから取得したデータ。</returns>
		[Obsolete]
		private static async Task<T> GetAsync<T>(
			this TwitterEndpoint endpoint, TwitterToken token,
			IReadOnlyDictionary<string, string> param, IReadOnlyDictionary<string, string> urlParam,
			Func<string, T> parser, Func<TwitterToken, Task<T>> fallback)
		{
			try
			{
				var json = await endpoint.GetStringAsync(token, param, urlParam);
				return parser(json);
			}
			catch (ApiException ex)
			{
				if (ex.Errors.Any(e => e.Code == 88) && token.FallbackToken != null) { }
				else throw;
			}

			return await fallback(token.FallbackToken);
		}
	}
}
