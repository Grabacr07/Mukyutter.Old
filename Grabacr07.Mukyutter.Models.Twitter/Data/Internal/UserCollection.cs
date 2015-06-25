using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Internal
{
	internal class Collection<T>
	{
		public long PreviousCursor { get; set; }

		public long NextCursor { get; set; }

		public List<T> Objects { get; set; }
	}

	internal static class Collection
	{
		public static Collection<UserId> ParseUserId(string json)
		{
			return Parse(json, djson => djson.ids, UserId.Parse);
		}

		public static Collection<User> ParseUser(string json)
		{
			return Parse(json, djson => djson.users, User.Parse);
		}

		public static Collection<T> Parse<T>(string json, Func<dynamic, dynamic> selector, Func<string, T> converter)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			DynamicJsonHelper.ThrowIfError(djson);

			try
			{
				return new Collection<T>
				{
					PreviousCursor = Convert.ToInt64(djson.previous_cursor),
					NextCursor = Convert.ToInt64(djson.next_cursor),
					Objects = ((object[])selector(djson)).Select(obj => converter(obj.ToString())).ToList(),
				};
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json, typeof(Collection<T>), ex);
			}
		}
	}
}
