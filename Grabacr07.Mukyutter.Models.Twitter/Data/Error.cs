using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	public class Error
	{
		public int Code { get; private set; }
		public string Message { get; private set; }

		private Error(int code, string message)
		{
			this.Code = code;
			this.Message = message;
		}

		public override string ToString()
		{
			return "(" + this.Code + ") " + this.Message;
		}

		#region parse methods

		public static bool TryParse(string json, out Error error)
		{
			try
			{
				error = ParseCore(DynamicJsonHelper.ToDynamicJson(json));
			}
			catch (Exception ex)
			{
				ex.Write();
				error = null;
			}

			return error != null;
		}

		public static Error Parse(string json)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			try
			{
				return ParseCore(djson);
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json, typeof(Error), ex);
			}
		}

		internal static Error ParseCore(dynamic djson)
		{
			var result = new Error(Convert.ToInt32(djson.code), djson.message);
			return result;
		}

		#endregion

	}
}
