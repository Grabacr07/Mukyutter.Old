using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	/// <summary>
	/// ツイートの投稿に使用されたクライアントの情報を表します。
	/// </summary>
	public class Source
	{
		public string Client { get; private set; }
		public Uri Url { get; private set; }

		internal Source(string client, Uri url)
		{
			this.Client = client;
			this.Url = url;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}", this.Client, this.Url == null ? "" : ": " + this.Url.ToString());
		}


		private static readonly Source _default = new Source("unknown", null);
		public static Source Default
		{
			get { return _default; }
		}

		public static Source Parse(string source)
		{
			return TwitterClient.Current.Sources.Parse(source);
		}
	}
}
