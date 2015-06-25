using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities.Data.Xml;
using Livet;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Json
{
	public static class JsonMonitor
	{
		private static string location = Path.Combine(Path.GetDirectoryName(typeof(JsonMonitor).Assembly.Location), "dev-export");

		public static JsonCollection Statuses { get; private set; }
		public static JsonCollection Users { get; private set; }
		public static JsonCollection UserStreams { get; private set; }
		public static JsonCollection DirectMessages { get; private set; }
		public static JsonCollection Lists { get; private set; }

		static JsonMonitor()
		{
			Statuses = new JsonCollection { Path = Path.Combine(location, "statuses.xml") };
			Users = new JsonCollection { Path = Path.Combine(location, "users.xml") };
			UserStreams = new JsonCollection { Path = Path.Combine(location, "userstreams.xml") };
			DirectMessages = new JsonCollection { Path = Path.Combine(location, "dm.xml") };
			Lists = new JsonCollection { Path = Path.Combine(location, "lists.xml") };
		}

		public static Task Export(this JsonCollection collection)
		{
			return Task.Factory.StartNew(() =>
			{
				if (!collection.Data.IsEmpty()) collection.WriteXml(collection.Path);
			});
		}

		public static Task<IEnumerable<string>> Import(this JsonCollection source)
		{
			// これべつに source の内容を変更したりしないので注意。
			// JsonCollection 使ってるのは Path が欲しいためなので、呼び出し側で parse してあげてね。
			return Task.Factory.StartNew<IEnumerable<string>>(() =>
			{
				try
				{
					return XmlFileReader.ReadXml<JsonCollection>(source.Path).Data;
				}
				catch (Exception ex)
				{
					TwitterClient.Current.ReportException("サンプル データが読めなかったー。", ex, null);
					return Enumerable.Empty<string>();
				}
			});
		}
	}

	[Serializable]
	public class JsonCollection
	{
		[XmlIgnore]
		public string Path { get; set; }

		public ObservableSynchronizedCollection<string> Data { get; set; }

		public JsonCollection()
		{
			this.Data = new ObservableSynchronizedCollection<string>();
		}
	}
}
