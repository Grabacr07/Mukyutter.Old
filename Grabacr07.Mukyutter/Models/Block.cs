using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Settings;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities.Events;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.Models
{
	/// <summary>
	/// ブロックを表します。ブロックは、1 つの画面で完結する機能の単位です。
	/// </summary>
	public class Block : NotificationObject
	{
		#region Name 変更通知プロパティ

		private string _Name;

		public virtual string Name
		{
			get { return this._Name; }
			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsSelected 変更通知プロパティ

		private bool _IsSelected;

		/// <summary>
		/// クライアント UI 上でこのブロックが選択されている状態かどうかを示す値を取得または設定します。
		/// 通常、このブロックに対応するタブがアクティブな状態であることを示します。
		/// </summary>
		public virtual bool IsSelected
		{
			get { return this._IsSelected; }
			set
			{
				if (this._IsSelected != value)
				{
					this._IsSelected = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public Block() { }

		protected Block(BlockSettings settings)
		{
			this._Name = settings.Name;
		}

		public virtual BlockSettings ToSettings()
		{
			return new BlockSettings
			{
				Name = this.Name,
			};
		}
	}
}
