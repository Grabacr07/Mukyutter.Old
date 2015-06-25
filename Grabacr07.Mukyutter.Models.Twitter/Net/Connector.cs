using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Grabacr07.Utilities.Development;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public abstract class Connector : NotificationObject, IDisposable
	{
		#region static members

#if DEBUG
		static Connector()
		{
			NetworkChange.NetworkAvailabilityChanged += (sender, e) =>
			{
				DebugMonitor.WriteLine("NetworkAvailability: {0}", e.IsAvailable);
			};
		}
#endif

		#endregion

		private LivetCompositeDisposable compositeDisposable = new LivetCompositeDisposable();

		public TwitterAccount Account { get; private set; }

		#region IsAvailable 変更通知プロパティ

		private bool _IsAvailable;

		/// <summary>
		/// ネットワーク接続が使用可能かどうかを示す値を取得します。
		/// </summary>
		public bool IsAvailable
		{
			get { return this._IsAvailable; }
			private set
			{
				if (this._IsAvailable != value)
				{
					this._IsAvailable = value;
					if (value) this.ConnectCore();
					else this.DisconnectCore();
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		protected Connector(TwitterAccount account)
		{
			this.Account = account;
			this._IsAvailable = NetworkInterface.GetIsNetworkAvailable();

			this.compositeDisposable.Add(
				new EventListener<NetworkAvailabilityChangedEventHandler>(
					h => NetworkChange.NetworkAvailabilityChanged += h,
					h => NetworkChange.NetworkAvailabilityChanged -= h,
					(sender, e) => this.IsAvailable = e.IsAvailable));
		}


		public Task Trial()
		{
			return this.Account.CurrentToken.GetRateLimitStatus();
		}


		/// <summary>
		/// 派生クラスでオーバーライドされると、ネットワークが利用可能になったときに接続処理を実行します。
		/// </summary>
		protected virtual void ConnectCore() { }

		/// <summary>
		/// 派生クラスでオーバーライドされると、ネットワークが利用できなくなったときに切断処理を実行します。
		/// </summary>
		protected virtual void DisconnectCore() { }

		#region IDisposable pattern

		~Connector()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Clean up all managed resources
				this.compositeDisposable.Dispose();
			}

			// Clean up all native resources
		}

		#endregion
	}
}
