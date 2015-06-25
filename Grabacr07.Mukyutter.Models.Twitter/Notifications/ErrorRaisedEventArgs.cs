using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter.Notifications
{
	public class ErrorRaisedEventArgs : EventArgs
	{
		public ClientError Error { get; private set; }

		public ErrorRaisedEventArgs(string message, Exception ex, Action retryAction)
		{
			this.Error = new ClientError(message, ex, retryAction);
		}
	}
}
