using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Grabacr07.Utilities.Reactive
{
	public static class SubscribeUIExtensions
	{
		public static IDisposable SubscribeUI<T>(this IObservable<T> source, Action<T> onNext)
		{
			if (onNext != null) onNext += _ => CommandManager.InvalidateRequerySuggested();

			return source
				.ObserveOnDispatcher()
				.Subscribe(onNext);
		}

		public static IDisposable SubscribeUI<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError)
		{
			if (onNext != null) onNext += _ => CommandManager.InvalidateRequerySuggested();
			if (onError != null) onError += _ => CommandManager.InvalidateRequerySuggested();

			return source
				.ObserveOnDispatcher()
				.Subscribe(onNext, onError);
		}

		public static IDisposable SubscribeUI<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
		{
			if (onNext != null) onNext += _ => CommandManager.InvalidateRequerySuggested();
			if (onError != null) onError += _ => CommandManager.InvalidateRequerySuggested();
			if (onCompleted != null) onCompleted += () => CommandManager.InvalidateRequerySuggested();

			return source
				.ObserveOnDispatcher()
				.Subscribe(onNext, onError, onCompleted);
		}
	}
}
