using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter
{
	internal static class Helper
	{
		public static async void Operation<T>(this Task<T> task, string success, string failure)
		{
			try
			{
				var result = await task;
				MukyutterClient.Current.NotificationService.Notify(success + ": " + result);
			}
			catch (Exception ex)
			{
				MukyutterClient.Current.NotificationService.Notify(failure, ex);
			}
		}

		public static async void Operation(this Task<Status> task, string success, string failure)
		{
			try
			{
				var result = await task;
				MukyutterClient.Current.NotificationService.Notify(success, result);
			}
			catch (Exception ex)
			{
				MukyutterClient.Current.NotificationService.Notify(failure, ex);
			}
		}


		/// <summary>
		/// リトライ可能な操作を実行します。例外はすべて捕捉され、指定したエラーメッセージを MukyutterClient に通知します。
		/// </summary>
		public static void Operation(Action operation, Func<Exception, string> errorMessage, bool isRetrying = true)
		{
			try
			{
				operation();
			}
			catch (Exception ex)
			{
				ex.Report(errorMessage(ex), isRetrying ? () => Operation(operation, errorMessage) : (Action)null);
			}
		}

		/// <summary>
		/// リトライ可能な操作を実行します。例外はすべて捕捉され、指定したエラー メッセージを MukyutterClient に通知します。
		/// </summary>
		public static void Operation<T>(this IObservable<T> operation, string errorMessage, params object[] args)
		{
			Operation(operation, _ => string.Format(errorMessage, args));
		}

		/// <summary>
		/// リトライ可能な操作を実行します。例外はすべて捕捉され、指定したエラー メッセージを MukyutterClient に通知します。
		/// </summary>
		public static async void Operation<T>(this IObservable<T> operation, Func<Exception, string> errorMessage, bool isRetrying = true)
		{
			try
			{
				await operation.ToTask();
			}
			catch (Exception ex)
			{
				ex.Report(errorMessage(ex), isRetrying ? () => Operation(operation, errorMessage) : (Action)null);
			}
		}

		/// <summary>
		/// リトライ可能な操作を実行します。例外はすべて捕捉され、指定したエラー メッセージを MukyutterClient に通知します。
		/// </summary>
		public static void Operation<T>(Func<Task<T>> operation, string errorMessage, params object[] args)
		{
			Operation(operation, _ => string.Format(errorMessage, args));
		}

		/// <summary>
		/// リトライ可能な操作を実行します。例外はすべて捕捉され、指定したエラー メッセージを MukyutterClient に通知します。
		/// </summary>
		public static async void Operation<T>(Func<Task<T>> operation, Func<Exception, string> errorMessage, bool isRetrying = true)
		{
			try
			{
				await operation();
			}
			catch (Exception ex)
			{
				ex.Report(errorMessage(ex), isRetrying ? () => Operation(operation, errorMessage) : (Action)null);
			}
		}


		internal static void Report(this Exception ex, string message, Action retryAction = null)
		{
			MukyutterClient.Current.ReportException(message, ex, retryAction);
		}
	}
}
