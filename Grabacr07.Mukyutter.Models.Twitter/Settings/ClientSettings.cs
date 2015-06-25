using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Settings
{
	[Serializable]
	public class ClientSettings
	{
		public List<CustomApplicationSettings> CustomApplications { get; set; }
	}
}
