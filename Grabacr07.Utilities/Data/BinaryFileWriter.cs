using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Grabacr07.Utilities.Data
{
	public static class BinaryFileWriter
	{
		public static void ToBinaryFile<T>(this T target, string path)
		{
			using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
			{
				var bf = new BinaryFormatter();
				bf.Serialize(fs, target);
			}
		}

		public static T LoadBinaryFile<T>(this string path)
		{
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				var bf = new BinaryFormatter();
				var data = bf.Deserialize(fs);

				return data is T ? (T)data : default(T);
			}
		}
	}
}
