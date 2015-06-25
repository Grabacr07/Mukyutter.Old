using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters
{
	public abstract class TimelineFilter
	{
		public abstract bool Predicate(Status status);
	}
}
