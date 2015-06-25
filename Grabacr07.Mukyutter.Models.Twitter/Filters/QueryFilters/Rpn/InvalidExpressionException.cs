using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters.Rpn
{
	internal class InvalidExpressionException : Exception
	{
		public InvalidExpressionException() { }
		public InvalidExpressionException(string message) : base(message) { }
	}
}
