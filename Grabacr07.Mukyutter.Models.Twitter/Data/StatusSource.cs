using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	/// <summary>
	/// ステータスの取得先を識別します。
	/// </summary>
	[Flags]
	public enum StatusSource
	{
		/// <summary>
		/// タイムライン取得系の REST API で取得したツイートです。
		/// </summary>
		RestApi = 1,

		/// <summary>
		/// User streams 接続によって取得したツイートです。
		/// </summary>
		UserStreams = 2,

		/// <summary>
		/// 投稿したツイートです。
		/// </summary>
		Update = 4,

		/// <summary>
		/// 起動時に最初に取得されたステータスです。
		/// </summary>
		Startup = 8,
	}
}
