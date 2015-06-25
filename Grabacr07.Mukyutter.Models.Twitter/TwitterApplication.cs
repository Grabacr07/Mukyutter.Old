using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter
{
	[DebuggerDisplay("Name = {Name}, ConsumerKey = {ConsumerKey}, ConsumerSecret = {ConsumerSecret}")]
	public class TwitterApplication: NotificationObject
	{
		public Guid Id { get; private set; }
		public string ConsumerKey { get; private set; }
		public string ConsumerSecret { get; private set; }

		/// <summary>
		/// カスタム アプリケーション (ユーザーが追加した API) かどうかを示す値を取得または設定します。
		/// </summary>
		public bool IsCustom { get; private set; }

		#region Name 変更通知プロパティ

		private string _Name;

		public string Name
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


		public TwitterApplication(Guid id, string name, string consumerKey, string consumerSecret, bool isCustom)
		{
			this.Id = id;
			this.Name = name;
			this.ConsumerKey = consumerKey;
			this.ConsumerSecret = consumerSecret;
			this.IsCustom = isCustom;	
		}
	}
}
