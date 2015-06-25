using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Utilities;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Stores
{
	public class SourceStore : StoreBase
	{
		private Dictionary<string, Source> sources;

		internal SourceStore()
		{
			this.sources = new Dictionary<string, Source>
			{
				{ "web", new Source("web", null) }
			};
		}

		#region parse source

		public Source Parse(string source)
		{
			// この辺のコードの意味は StatusStore に書いてあるのでそっち参照

			Source result;
			try
			{
				this.lockslim.EnterUpgradeableReadLock();

				var contains = this.sources.ContainsKey(source);
				if (contains)
				{
					result = this.sources[source];
				}
				else
				{
					try
					{
						this.lockslim.EnterWriteLock();
						result = this.ParseCore(source);
					}
					catch (Exception ex)
					{
						throw new JsonParseException(source, typeof(Source), ex);
					}
					finally
					{
						if (this.lockslim.IsWriteLockHeld) this.lockslim.ExitWriteLock();
					}
				}
			}
			finally
			{
				if (this.lockslim.IsUpgradeableReadLockHeld) this.lockslim.ExitUpgradeableReadLock();
			}

			return result;
		}

		private Source ParseCore(string source)
		{
			var result = Source.Default;

			if (!string.IsNullOrEmpty(source))
			{
				var regex = new Regex(
					TwitterDefinitions.Regex.Source,
					RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
				var matches = regex.Match(source);
				if (matches.Success)
				{
					var client = matches.Groups["client"].ToString();
					var url = matches.Groups["url"].ToString();
					Uri uri;

					result = new Source(client, Uri.TryCreate(url, UriKind.Absolute, out uri) ? uri : null);
					this.sources.Add(source, result);
				}
				else
				{
					DebugMonitor.WriteLine("unmatched source string: " + source);
				}

			}
			return result;
		}

		#endregion
	}
}
