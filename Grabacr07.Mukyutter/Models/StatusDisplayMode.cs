using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models
{
	/// <summary>
	/// タイムライン内でステータスを詳細表示する方法を示す識別子を定義します。
	/// </summary>
	public enum StatusDisplayMode
	{
		/// <summary>
		/// 選択したステータスは、その場所でポップアップ表示されます。
		/// </summary>
		Popup,

		/// <summary>
		/// 選択したステータスは、タイムライン内で詳細表示されます。
		/// </summary>
		Expand,

		/// <summary>
		/// 選択したステータスは、タイムライン下部の詳細表示領域に表示されます。
		/// </summary>
		Bottom,

		/// <summary>
		/// すべてのステータスは、タイムライン内で常に詳細表示されます。
		/// </summary>
		Always,
	}
}
