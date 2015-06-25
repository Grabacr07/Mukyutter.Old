using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter.Composing
{
	public class Composer : NotificationObject
	{
		#region Account 変更通知プロパティ

		private TwitterAccount _Account;

		/// <summary>
		/// 投稿に使用するアカウントを取得または設定します。
		/// </summary>
		public TwitterAccount Account
		{
			get { return _Account; }
			set
			{
				if (this._Account != value)
				{
					this._Account = value;
					this._CurrentStatus.Account = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		/// <summary>
		/// 投稿を待っているツイートのキューを取得します。
		/// 通常、下書き、投稿中または投稿に失敗したツイートがキューイングされます。
		/// 入力中のツイート (CurrentStatus) は、このキューには含まれません。
		/// </summary>
		public ObservableSynchronizedCollection<NewStatus> NewStatuses { get; private set; }

		#region CurrentStatus 変更通知プロパティ

		private NewStatus _CurrentStatus;

		public NewStatus CurrentStatus
		{
			get { return this._CurrentStatus; }
			set
			{
				if (this._CurrentStatus != value)
				{
					this._CurrentStatus = value;
					this._CurrentStatus.Footer = this.Footer;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Footer 変更通知プロパティ

		private string _Footer;

		public string Footer
		{
			get { return this._Footer; }
			set
			{
				if (this._Footer != value)
				{
					this._Footer = value;
					this._CurrentStatus.Footer = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region FooterEnabled 変更通知プロパティ

		private bool _FooterEnabled = true;

		public bool FooterEnabled
		{
			get { return this._FooterEnabled; }
			set
			{
				if (this._FooterEnabled != value)
				{
					this._FooterEnabled = value;
					this.CurrentStatus.Footer = value ? this.Footer : "";
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region フォーカス要求イベント

		public event EventHandler FocusRequested;

		protected virtual void OnFocusRequested()
		{
			if (this.FocusRequested != null)
			{
				this.FocusRequested(this, new EventArgs());
			}
		}

		#endregion


		public Composer()
		{
			this.Account = null;
			this.CurrentStatus = new NewStatus(null);
			this.Footer = "";
			this.NewStatuses = new ObservableSynchronizedCollection<NewStatus>();
		}


		/// <summary>
		/// 現在入力中のツイートを投稿します。
		/// </summary>
		public void Update()
		{
			if (!this.CurrentStatus.CanUpdate) return;

			this.Update(this.CurrentStatus);
			this.CurrentStatus = new NewStatus(this.Account);
		}

		/// <summary>
		/// 指定したツイートを投稿します。
		/// </summary>
		public async void Update(NewStatus status)
		{
			if (!status.CanUpdate) return;

			if(!this.NewStatuses.Contains(status)) this.NewStatuses.Add(status);
			
			try
			{
				status.IsUpdating = true;
				status.Exception = null;
				await status.Account.CurrentToken.UpdateStatus(status);
			}
			catch (Exception ex)
			{
				ex.Write();

				// 投稿に失敗した場合、例外情報を持たせる
				status.Exception = ex;
				return;
			}
			finally
			{
				status.IsUpdating = false;
			}

			// 投稿に成功した場合、キューから削除し、インスタンス破棄
			this.NewStatuses.Remove(status);
			status.Dispose();
		}

		public void Focus()
		{
			this.OnFocusRequested();
		}
	}
}
