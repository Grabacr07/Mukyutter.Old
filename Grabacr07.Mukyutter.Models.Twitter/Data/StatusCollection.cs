using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	public class StatusCollection : HashSet<Status>
	{
		public StatusCollection() { }
		public StatusCollection(IEnumerable<Status> collection) : base(collection) { }


		/// <summary>
		/// 複数の Twitter ステータスを格納する json 文字列から、Status オブジェクトのコレクションに変換します。
		/// </summary>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Net.ApiException">Twitter API がエラーを返した場合。</exception>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Data.Json.JsonParseException">json から status のコレクションへの変換に失敗した場合。</exception>
		internal static StatusCollection Parse(string json, StatusSource source = StatusSource.RestApi)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			DynamicJsonHelper.ThrowIfError(djson);

			var list = new StatusCollection();
			try
			{
				foreach (var s in (object[])djson)
				{
					Status status;
					if (TwitterClient.Current.Statuses.TryAdd(s, source, out status)) list.Add(status);
				}
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json, typeof(StatusCollection), ex);
			}

			return list;
		}
	}
}
