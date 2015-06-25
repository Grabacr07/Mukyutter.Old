using System;
using System.Collections.Generic;
using System.Linq;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	/// <summary>
	/// ユーザー ID のコレクションを表します。
	/// </summary>
	public class UserIdCollection : HashSet<UserId>
	{
		/// <summary>
		/// <see cref="T:Grabacr07.Mukyutter.Models.Twitter.Model.UserIdCollection"/>
		/// クラスの新しいインスタンスを初期化します。
		/// </summary>
		public UserIdCollection()
			: base() { }

		/// <summary>
		/// 指定したコレクションからコピーした要素を格納し、コピーされる要素の数を格納できるだけの容量を備えた、<see cref="T:Grabacr07.Mukyutter.Models.Twitter.Model.UserIdCollection"/>
		/// クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="collection"></param>
		public UserIdCollection(IEnumerable<UserId> collection)
			: base(collection) { }


		public static UserIdCollection Parse(string json)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			DynamicJsonHelper.ThrowIfError(djson);

			return ParseCore(djson);
		}

		internal static UserIdCollection ParseCore(dynamic djson)
		{
			return new UserIdCollection(((object[])djson).Select(id => UserId.Parse(id.ToString())));
		}
	}
}
