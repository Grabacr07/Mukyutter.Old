using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Notifications
{
	public class ClientError
	{
		public DateTime CreatedAt { get; set; }
		public string Message { get; private set; }
		public Exception Exception { get; private set; }
		public Action RetryAction { get; private set; }

		public ClientError(string message, Exception ex, Action retryAction)
		{
			this.CreatedAt = DateTime.Now;
			this.Message = message;
			this.Exception = ex;
			this.RetryAction = retryAction;
		}
	}
}
