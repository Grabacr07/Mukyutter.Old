using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters
{
	internal class EmptyFilter : TimelineFilter
	{
		public override bool Predicate(Status status)
		{
			return false;
		}
	}
}
