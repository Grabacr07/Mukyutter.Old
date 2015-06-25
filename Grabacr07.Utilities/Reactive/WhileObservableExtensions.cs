using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Grabacr07.Utilities.Reactive
{
	public static class WhileObservableExtensions
	{
		public static IObservable<T> While<T>(this IObservable<T> source, Func<bool> condition)
		{
			return WhileCore(condition, source).Concat();
		}

		//public static IObservable<T> DoWhile<T>(this IObservable<T> source, Func<bool> condition)
		//{
		//	return source.Concat(source.While(condition));
		//}

		private static IEnumerable<IObservable<T>> WhileCore<T>(Func<bool> condition, IObservable<T> source)
		{
			while (condition())
			{
				yield return source;
			}
		}
	}
}
