using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters.QueryFilters.Rpn
{
	public class ParameterVisitor : ExpressionVisitor
	{
		private readonly IDictionary<Tuple<Type, string>, ParameterExpression> _Parameters;

		public ICollection<ParameterExpression> Parameters
		{
			get { return this._Parameters.Values; }
		}

		public ParameterVisitor(IEnumerable<ParameterExpression> parameters)
		{
			this._Parameters = parameters.ToDictionary(p => Tuple.Create(p.Type, p.Name));
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			var key = Tuple.Create(node.Type, node.Name);
			return this._Parameters.ContainsKey(key) ? this._Parameters[key] : node;
		}
	}
}