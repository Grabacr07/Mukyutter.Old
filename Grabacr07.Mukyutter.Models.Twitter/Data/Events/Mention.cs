using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Events
{
	public class Mention : Event
	{
		public Status TargetObject { get; internal set; }
	}
}
