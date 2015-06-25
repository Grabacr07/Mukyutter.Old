using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Twitter.Accounts
{
	public class EndpointViewModel : ViewModel
	{
		private readonly TwitterEndpoint endpoint;

		public string Name { get; private set; }

		#region ApiLimit プロパティ

		public string ApiLimit
		{
			get
			{
				var limit = this.endpoint.RateLimit.Limit;
				return limit.HasValue ? limit.Value.ToString() : "---";
			}
		}

		#endregion

		#region ApiCurrent プロパティ

		public string ApiCurrent
		{
			get
			{
				var current = this.endpoint.RateLimit.Remaining;
				return current.HasValue ? current.Value.ToString() : "---";
			}
		}

		#endregion

		#region ApiResetTime プロパティ

		public string ApiResetTime
		{
			get
			{
				var resetTime = this.endpoint.RateLimit.ResetTime;
				return resetTime.HasValue ? resetTime.Value.ToString() : "";
			}
		}

		#endregion


		public EndpointViewModel(TwitterEndpoint endpoint)
		{
			this.endpoint = endpoint;
			this.Name = endpoint.Definition.Name;

			this.CompositeDisposable.Add(new PropertyChangedEventListener(endpoint.RateLimit)
			{
				{ "Limit", (sender, e) => this.RaisePropertyChanged("ApiLimit") },
				{ "Remaining", (sender, e) => this.RaisePropertyChanged("ApiCurrent") },
				{ "ResetTime", (sender, e) => this.RaisePropertyChanged("ApiResetTime") },
			});
		}
	}
}
