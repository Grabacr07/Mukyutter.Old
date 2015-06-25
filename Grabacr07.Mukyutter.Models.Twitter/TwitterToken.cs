using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncOAuth;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter
{
	public class TwitterToken : NotificationObject
	{
		public TwitterApplication Application { get; private set; }
		public string TokenKey { get; private set; }
		public string TokenSecret { get; private set; }

		public IReadOnlyDictionary<string, TwitterEndpoint> Endpoints { get; private set; }

		#region FallbackToken 変更通知プロパティ

		public UserId FallbackUserId { get; private set; }
		public Guid FallbackApplicationId { get; private set; }

		private TwitterToken fallbackToken;

		/// <summary>
		/// このトークンで API 規制となったときに使用する、フォールバック先トークンを取得します。
		/// </summary>
		public TwitterToken FallbackToken
		{
			get
			{
				// fallbackToken が既に設定されていれば、それを返すように試みる。

				if (this.fallbackToken != null)
				{
					return this.fallbackToken;
				}
				else
				{
					var token = TwitterClient.Current.Accounts.Where(a => a.UserId == this.FallbackUserId)
						.SelectMany(a => a.Tokens.Where(t => t.Application.Id == this.FallbackApplicationId))
						.FirstOrDefault();
					if (token != null)
					{
						this.fallbackToken = token;
						return token;
					}
				}

				return null;
			}
		}

		#endregion


		public TwitterToken(TwitterApplication application, AccessToken token)
			: this(application, token.Key, token.Secret) { }

		public TwitterToken(TwitterApplication application, string tokenKey, string tokenSecret)
		{
			this.Application = application;
			this.TokenKey = tokenKey;
			this.TokenSecret = tokenSecret;

			this.Endpoints = RestApi.Endpoints
				.Select(def => new TwitterEndpoint(this, def))
				.ToDictionary(ep => ep.Definition.Name, ep => ep);
		}


		public void ChangeToken(AccessToken token)
		{
			this.TokenKey = token.Key;
			this.TokenSecret = token.Secret;
		}


		public void SetFallbackToken(UserId userId, Guid applicationId)
		{
			this.fallbackToken = null;
			this.FallbackUserId = userId;
			this.FallbackApplicationId = applicationId;

			this.RaisePropertyChanged("FallbackToken");
		}
	}
}
