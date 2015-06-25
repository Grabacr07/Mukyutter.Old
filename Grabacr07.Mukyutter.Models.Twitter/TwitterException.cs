using System;
using System.IO;
using System.Runtime.CompilerServices;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter
{
	public class TwitterException : Exception
	{
		public string CallerClassName { get; private set; }
		public string CallerMemberName { get; private set; }
		public int CallerLineNumber { get; private set; }

		internal TwitterException(
			string message = null,
			Exception innerException = null,
			[CallerFilePath]string path = "",
			[CallerMemberName]string member = "",
			[CallerLineNumber]int line = 0)
			: base(SelectMessage(message, innerException, member), innerException)
		{
			string filename = "";
			try
			{
				filename = Path.GetFileNameWithoutExtension(path);
			}
			catch (Exception ex)
			{
				ex.Write();
			}

			this.CallerClassName = filename;
			this.CallerMemberName = member;
			this.CallerLineNumber = line;
		}

		private static string SelectMessage(string message, Exception innerException, string member)
		{
			if (string.IsNullOrEmpty(message))
			{
				if (innerException != null)
				{
					return innerException.Message;
				}
				return "場所 '" + member + "' で Twitter 汎用例外が発生しました。";
			}
			else
			{
				return message;
			}
		}
	}
}
