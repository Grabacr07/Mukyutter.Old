using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters
{
	public class FilterException : TwitterException
	{
		internal FilterException(
			string message = null,
			Exception innnerException = null,
			[CallerFilePath]string path = "",
			[CallerMemberName]string member = "",
			[CallerLineNumber]int line = 0)
			: base(message ?? "フィルターの作成に失敗しました。", innnerException, path, member, line) { }
	}
}
