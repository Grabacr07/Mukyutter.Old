using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	[DebuggerDisplay("{Remaining}/{Limit}, ~{ResetTime}")]
	public class RateLimit : NotificationObject
	{
		#region Limit 変更通知プロパティ

		private int? _Limit;

		public int? Limit
		{
			get { return this._Limit; }
			private set
			{
				if (this._Limit != value)
				{
					this._Limit = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Remaining 変更通知プロパティ

		private int? _Remaining;

		public int? Remaining
		{
			get { return this._Remaining; }
			private set
			{
				if (this._Remaining != value)
				{
					this._Remaining = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Reset 変更通知プロパティ

		private long? _Reset;

		public long? Reset
		{
			get { return this._Reset; }
			private set
			{
				if (this._Reset != value)
				{
					this._Reset = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("ResetTime");
				}
			}
		}

		#endregion

		#region ResetTime 変更通知プロパティ

		public DateTime? ResetTime
		{
			get
			{
				return this.Reset.HasValue
					? (DateTime?)(CommonDefinitions.UnixEpoch.AddSeconds(this.Reset.Value).ToLocalTime())
					: null;
			}
		}

		#endregion

		public void Set(int limit, int remaining, long reset)
		{
			this.Limit = limit;
			this.Remaining = remaining;
			this.Reset = reset;
		}

		public void Set(HttpResponseHeaders headers)
		{
			IEnumerable<string> values;

			if (headers.TryGetValues(TwitterDefinitions.HttpResponse.RateLimit, out values))
			{
				var limit = values.FirstOrDefault();

				if (headers.TryGetValues(TwitterDefinitions.HttpResponse.RateLimitRemaining, out values))
				{
					var remaining = values.FirstOrDefault();

					if (headers.TryGetValues(TwitterDefinitions.HttpResponse.RateLimitReset, out values))
					{
						var reset = values.FirstOrDefault();

						if (limit != null && remaining != null && reset != null)
						{
							try
							{
								this.Set(int.Parse(limit), int.Parse(remaining), long.Parse(reset));
							}
							catch (Exception ex)
							{
								ex.Write();
							}
						}
					}
				}
			}
		}

		public void Clear()
		{
			this.Limit = null;
			this.Remaining = null;
			this.Reset = null;
		}
	}
}
