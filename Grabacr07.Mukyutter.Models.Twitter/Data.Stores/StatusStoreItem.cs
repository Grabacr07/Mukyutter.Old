using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public class StatusStoreItem
	{
		public Status Status { get; internal set; }
		public StatusSource Source { get; internal set; }
	}
}
