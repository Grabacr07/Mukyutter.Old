using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Settings
{
	[Serializable]
	[XmlInclude(typeof(TimelineBlockSettings))]
	public class BlockSettings
	{
		public string Name { get; set; }
	}
}
