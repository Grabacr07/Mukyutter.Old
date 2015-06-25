using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Grabacr07.Utilities.Data.Xml;
using Grabacr07.Utilities.Security;

namespace Grabacr07.Mukyutter.Models.Twitter.Settings
{
	[Serializable]
	public class AccountSettings
	{
		public string AccountName { get; set; }

		public List<TokenSettings> Tokens { get; set; }

		public Guid CurrentApplicationId { get; set; }

		public bool UseUserStreams { get; set; }


		public AccountSettings()
		{
			this.Tokens = new List<TokenSettings>();
			this.UseUserStreams = true;
		}
	}
}
