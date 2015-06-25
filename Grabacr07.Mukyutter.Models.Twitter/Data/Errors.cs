using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	public class Errors
	{
		private Errors() { }


		public static bool TryParse(string json, out Error[] errors)
		{
			try
			{
				errors = ParseCore(DynamicJsonHelper.ToDynamicJson(json));
			}
			catch (Exception ex)
			{
				ex.Write();
				errors = null;
			}

			return errors != null;
		}

		public static Error[] Parse(string json)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			try
			{
				return ParseCore(djson);
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json, typeof(Error[]), ex);
			}
		}

		internal static Error[] ParseCore(dynamic djson)
		{
			var list = new List<Error>();
			foreach (var e in (object[])djson.errors)
			{
				try
				{
					var error = Error.ParseCore(e);
					list.Add(error);
				}
				catch (Exception ex)
				{
					ex.Write();
				}
			}
			return list.ToArray();
		}
	}
}
