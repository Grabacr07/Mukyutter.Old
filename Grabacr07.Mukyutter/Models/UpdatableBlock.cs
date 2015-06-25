using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Settings;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities.Development;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.Models
{
	/// <summary>
	/// 投稿、その他 Twitter API に対するアクセスを行うことができるブロックを表します。
	/// このブロックは 1 つの Twitter アカウントと関連付けられます。
	/// このブロックが選択されたとき、アカウントの存在の有無に関わらず、MukyutterClient.Current.CurrentAccount プロパティをこのブロックのアカウントに設定します。
	/// </summary>
	public class UpdatableBlock : Block
	{
		/// <summary>
		/// このタイムラインを所有する AccountId から、アカウント情報を取得します。該当するアカウントが存在しない場合は null。
		/// </summary>
		public TwitterAccount Account
		{
			get { return TwitterClient.Current.Accounts.FirstOrDefault(a => a.UserId == this.AccountId); }
		}

		#region AccountId 変更通知プロパティ

		private UserId _AccountId;

		/// <summary>
		/// このブロックからの投稿、その他 Twitter API に対するアクセスに使用するアカウントのユーザー ID を取得または設定します。
		/// TwitterClient.Current.Accounts コレクションに存在する ID のみを設定できます。
		/// ただし、取得によって返される値は TwitterClient.Current.Accounts コレクションに存在するとは限りません (設定の読み込みの関係)。
		/// </summary>
		public UserId AccountId
		{
			get { return this._AccountId; }
			set
			{
				if (this._AccountId != value && TwitterClient.Current.Accounts.Any(a => a.UserId == value))
				{
					this._AccountId = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("Account");
					if (this.IsSelected) MukyutterClient.Current.CurrentAccountId = value;
				}
			}
		}

		#endregion

		/// <summary>
		/// クライアント UI 上でこのブロックが選択されている状態かどうかを示す値を取得または設定します。
		/// 通常、このブロックに対応するタブがアクティブな状態であることを示します。
		/// このブロックを選択すると
		/// </summary>
		public override bool IsSelected
		{
			get { return base.IsSelected; }
			set
			{
				base.IsSelected = value;
				if (value) MukyutterClient.Current.CurrentAccountId = this.AccountId;
			}
		}


		public UpdatableBlock() : this(MukyutterClient.Current.DefaultAccountId, new BlockSettings()) { }

		protected UpdatableBlock(UserId account, BlockSettings settings)
			: base(settings)
		{
			this._AccountId = account;
		}

		
		#region Disposable pattern methods

		//~UpdatableBlock()
		//{
		//	this.Dispose(false);
		//}

		//public void Dispose()
		//{
		//	this.Dispose(true);
		//	GC.SuppressFinalize(this);
		//}

		//protected virtual void Dispose(bool disposing)
		//{
		//	if (disposing)
		//	{
		//		// Clean up all managed resources
		//	}

		//	// Clean up all native resources
		//}

		#endregion
	}
}
