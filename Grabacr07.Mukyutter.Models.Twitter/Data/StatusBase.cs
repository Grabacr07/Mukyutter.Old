using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Entity;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	[Serializable]
	public abstract class StatusBase : NotificationObject, IComparable<StatusBase>
	{
		public StatusId Id { get; set; }
		public DateTime CreatedAt { get; set; }

		public string Text { get; set; }
		public Entities Entities { get; set; }

		/// <summary>
		/// 指定したステータスとこのインスタンスを比較し、ステータス ID を元にしたこれらの相対値を示す値を返します。
		/// </summary>
		/// <param name="status">比較するオブジェクト。</param>
		/// <returns>ステータス ID を元にした相対値。</returns>
		public int CompareTo(StatusBase status)
		{
			return this.Id.CompareTo(status.Id);
		}
	}
}
