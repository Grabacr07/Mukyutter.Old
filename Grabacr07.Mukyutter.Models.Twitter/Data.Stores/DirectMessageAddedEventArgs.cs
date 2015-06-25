using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public class DirectMessageAddedEventArgs : EventArgs
	{
		public DirectMessage DirectMessage { get; private set; }

		internal DirectMessageAddedEventArgs(DirectMessage dm)
		{
			this.DirectMessage = dm;
		}
	}
}
