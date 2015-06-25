using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter.Notifications;

namespace Grabacr07.Mukyutter.ViewModels.Twitter.Events
{
	public class ErrorViewModel : ViewModelBase
	{
		private ClientError error;
		private Action removeAction;

		public long Id
		{
			get { return this.error.CreatedAt.Ticks; }
		}

		public string Message { get { return this.error.Message; } }
		public string Detail { get { return this.error.Exception.ToString(); } }
		public bool CanRetry { get { return this.error.RetryAction != null; } }

		public ErrorViewModel(ClientError error, Action removeAction)
		{
			this.error = error;
			this.removeAction = removeAction;
		}

		public void Retry()
		{
			if (this.error.RetryAction != null)
			{
				Task.Factory.StartNew(() =>
				{
					this.removeAction();
					this.error.RetryAction();
				});
			}

		}

		public void Remove()
		{
			this.removeAction();
		}
	}
}
