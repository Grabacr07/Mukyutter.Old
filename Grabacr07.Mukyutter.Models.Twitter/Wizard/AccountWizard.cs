using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using AsyncOAuth;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter.Wizard
{
	public class AccountWizard : NotificationObject
	{
		private RequestToken requestToken;
		private TwitterApplication application;


		public async Task GetRequestToken(TwitterApplication application)
		{
			this.application = application;
			var data = await TwitterClient.Current.GetRequestToken(this.application.ConsumerKey, this.application.ConsumerSecret).ToTask();
			this.requestToken = data.Token;
			Process.Start(data.AuthorizeUrl.ToString());
		}

		public async Task GetAccessToken(string pinCode)
		{
			var data = await TwitterClient.Current.GetAccessToken(
				this.application.ConsumerKey, 
				this.application.ConsumerSecret, 
				this.requestToken,
				pinCode).ToTask();

			var account = TwitterClient.Current.Accounts.FirstOrDefault(a => a.UserId == data.UserId);
			if (account == null)
			{
				account = new TwitterAccount(this.application, data.Token);
				await account.Initialize();
				TwitterClient.Current.LoadAccount(account);
			}
			else
			{
				account.AddToken(this.application, data.Token);
			}
		}
	}
}
