using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Entity
{
	public class Indices
	{
		public int StartIndex { get; internal set; }

		public int EndIndex { get; internal set; }

		public int Length
		{
			get { return this.EndIndex - this.StartIndex; }
		}


		internal static Indices ParseCore(dynamic djson)
		{
			try
			{
				var result = new Indices
				{
					StartIndex = Convert.ToInt32(djson[0]),
					EndIndex = Convert.ToInt32(djson[1]),
				};

				return result;
			}
			catch (Exception ex)
			{
				throw new JsonParseException(djson, typeof(Indices), ex);
			}
		}
	}
}
