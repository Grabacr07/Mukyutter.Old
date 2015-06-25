using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities;
using Livet;

namespace Grabacr07.Mukyutter.Models
{
	public class NotificationService : NotificationObject
	{
		private Subject<string> notifier;

		#region NotificationMessage 変更通知プロパティ

		private string _NotificationMessage;

		public string NotificationMessage
		{
			get { return this._NotificationMessage; }
			private set
			{
				if (this._NotificationMessage != value)
				{
					this._NotificationMessage = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		public NotificationService()
		{
			this.notifier = new Subject<string>();
			this.notifier
				.Do(s => this.NotificationMessage = s)
				.Throttle(TimeSpan.FromMilliseconds(5000))
				.Subscribe(_ => this.NotificationMessage = "");


			TwitterClient.Current.EventRaised += (sender, e) =>
			{
			};
			TwitterClient.Current.ErrorRaised += (sender, e) =>
			{
			};

			
		}

		public void Notify(string message)
		{
			this.notifier.OnNext(message);
		}

		public void Notify(string message, Status status)
		{
			this.notifier.OnNext(string.Format("{0} - @{1}: {2}", message, status.User.ScreenName, status.Text.Flatten()));
		}

		public void Notify(string message, Exception ex)
		{
			this.notifier.OnNext(string.Format("{0} - {1}: {2}", message, ex.GetType().Name, ex.Message));
		}

		public void Notify(string format, params object[] args)
		{
			this.notifier.OnNext(string.Format(format, args));
		}
	}
}
