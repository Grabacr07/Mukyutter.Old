using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	public class DummyUser : User
	{
		public DummyUser(ScreenName screenName)
		{
			this.ScreenName = screenName;
		}
	}
}
