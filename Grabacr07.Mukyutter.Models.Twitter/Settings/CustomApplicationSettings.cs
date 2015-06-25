using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Settings
{
	/// <summary>
	/// カスタム アプリケーション (ユーザーが追加した API) の永続化可能な設定情報を表します。
	/// </summary>
	[Serializable]
	public class CustomApplicationSettings
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string ConsumerKey { get; set; }
		public string ConsumerSecret { get; set; }
	}
}
