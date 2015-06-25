using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public sealed class UrlStore : StoreBase
	{
		private Dictionary<string, Uri> urls;

		internal UrlStore()
		{
			this.urls = new Dictionary<string, Uri>();
		}


	}
}
