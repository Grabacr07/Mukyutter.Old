using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public class StatusAddedEventArgs : EventArgs
	{
		public Status Status { get; private set; }

		public StatusAddedEventArgs(Status status)
		{
			this.Status = status;
		}
	}
}
