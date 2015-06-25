using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	public class ListCollection
	{
		private ListCollection() { }

		public static ICollection<List> Parse(string json, UserId ownerId)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			DynamicJsonHelper.ThrowIfError(djson);

			var lists = new List<List>();
			try
			{
				foreach (var l in (object[])djson)
				{
					List list;
					if (TwitterClient.Current.Lists.TryParse(l, ownerId, out list)) lists.Add(list);
				}
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json, typeof(StatusCollection), ex);
			}

			return lists;
		}
	}
}
