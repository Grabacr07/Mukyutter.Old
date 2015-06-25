using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Twitter.Accounts
{
	public class UserStreamsViewModel : ViewModel
	{
		private UserStreams userstreams;

		#region Message 変更通知プロパティ

		private string _Message;

		public string Message
		{
			get { return this._Message; }
			private set
			{
				if (this._Message != value)
				{
					this._Message = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		public UserStreamsViewModel(UserStreams userstreams)
		{
			this.userstreams = userstreams;
			this.Message = userstreams.Status.Message();
			this.CompositeDisposable.Add(new PropertyChangedEventListener(userstreams)
			{
				{ "Status", (sender, e) => this.Message = userstreams.Status.Message() }
			});
		}
	}
}
