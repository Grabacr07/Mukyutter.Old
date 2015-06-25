using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	public class DirectMessageCollection : HashSet<DirectMessage>
	{
		public DirectMessageCollection() : base() { }
		public DirectMessageCollection(IEnumerable<DirectMessage> collection) : base(collection) { }


		/// <summary>
		/// 複数のダイレクト メッセージを格納する json 文字列から、DirectMessage オブジェクトのコレクションに変換します。
		/// </summary>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Net.ApiException">Twitter API がエラーを返した場合。</exception>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Data.Json.JsonParseException">json から status のコレクションへの変換に失敗した場合。</exception>
		public static DirectMessageCollection Parse(string json)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			DynamicJsonHelper.ThrowIfError(djson);

			var list = new DirectMessageCollection();
			try
			{
				foreach (var s in (object[])djson)
				{
					DirectMessage message = null;
					if (TwitterClient.Current.Messages.TryParse(s, out message)) list.Add(message);
				}
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json, typeof(List<Status>), ex);
			}

			return list;
		}
	}
}
