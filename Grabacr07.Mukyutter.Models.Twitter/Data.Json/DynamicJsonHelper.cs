using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Data.Json;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Json
{
	internal static class DynamicJsonHelper
	{
		/// <summary>
		/// json 文字列から DynamicJson オブジェクトを生成します。
		/// </summary>
		/// <param name="json">json 文字列。</param>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Data.Json.JsonParseException">DynamicJson オブジェクトの生成に失敗した場合。</exception>
		/// <returns>DynamicJson オブジェクト。</returns>
		internal static dynamic ToDynamicJson(string json)
		{
			try
			{
				return DynamicJson.Parse(json);
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json, typeof(DynamicJson), ex);
			}
		}

		/// <summary>
		/// json がエラーを含む場合、TwitterApiException をスローします。
		/// </summary>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Net.ApiException"><paramref name="djson"/> が error を含む場合。</exception>
		/// <exception cref="Grabacr07.Mukyutter.Models.Twitter.Data.Json.JsonParseException"><paramref name="djson"/> が error を含んでいるが、エラー情報の取得に失敗した場合。</exception>
		internal static void ThrowIfError(dynamic djson)
		{
			try
			{
				if (djson.IsDefined("errors"))
				{
					var errors = Errors.ParseCore(djson);
					throw new ApiException(errors);
				}
			}
			catch (ApiException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new JsonParseException(djson, typeof(ApiException), ex);
			}
		}
	}
}
