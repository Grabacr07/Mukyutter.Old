using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Utilities.Data.Json;
using Grabacr07.Utilities.Reactive;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public static partial class RestApi
	{
		private static IObservable<TSource> OnErrorRetry<TSource>(
			this IObservable<TSource> source, int retryCount)
		{
			return source.OnErrorRetry((Exception ex) => ex.Write(), retryCount);
		}

	}
}
