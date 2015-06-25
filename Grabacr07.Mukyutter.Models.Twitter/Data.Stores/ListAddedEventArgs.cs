using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public class ListAddedEventArgs : EventArgs
	{
		public List List { get; private set; }

		public ListAddedEventArgs(List list)
		{
			this.List = list;
		}
	}
}
