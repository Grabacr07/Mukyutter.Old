using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Net;

namespace Grabacr07.Mukyutter.ViewModels.Extensions
{
	internal static class ViewModelExtensions
	{
		public static string ToDisplayName(this TwitterAccount account)
		{
			return account.User.ToDisplayName();
		}

		public static string ToDisplayName(this User user)
		{
			// ToDo: Name/ScreenName の選択機能を実装したら書き直す
			return user.ScreenName.Value;
		}


		/// <summary>
		/// 現在の日付を絶対表記の文字列形式に変換します。
		/// 現在時刻との差が 1 日未満の場合は "HH:mm:ss"、1 日以上 1 年未満の場合は "M/d"、1 年以上の場合は "yyyy/MM/dd" 形式になります。
		/// </summary>
		/// <param name="datetime"></param>
		/// <returns></returns>
		public static string ToAbsoluteShortString(this DateTime datetime)
		{
			var diff = DateTime.Now.Subtract(datetime);
			var format = diff.TotalDays < 1.0
				? "HH:mm:ss"
				: diff.TotalDays < 365.0
					? "M/d"
					: "yyyy/MM/dd";
			return datetime.ToString(format);
		}


		/// <summary>
		/// ステータスを配信するプロバイダーからの購読を開始し、受信したシーケンスを MukyutterClient の通知サービスに転送します。
		/// </summary>
		/// <param name="source"></param>
		/// <param name="success">プロバイダーから正常値が通知されたときのメッセージ。</param>
		/// <param name="failure">プロバイダーから例外がスローされたときのメッセージ。</param>
		public static IDisposable Subscribe(this IObservable<Status> source, string success, string failure)
		{
			return source.Subscribe(
				st => MukyutterClient.Current.NotificationService.Notify(success, st),
				ex => MukyutterClient.Current.NotificationService.Notify(failure, ex));
		}


		/// <summary>
		/// Twitter API で実行した処理結果を購読します。
		/// </summary>
		/// <typeparam name="T">Twitter API の処理結果として取得したデータの型。</typeparam>
		/// <param name="source"></param>
		/// <param name="success">API の処理が成功したとき、UI に通知するメッセージを生成するメソッド。</param>
		/// <param name="failure">API の処理が失敗したとき (または API の実行以前に失敗したとき) に UI に通知するメッセージの書式。書式内の {0} がエラー メッセージに置き換えられます。</param>
		/// <param name="retryAction">リトライ処理。</param>
		/// <returns></returns>
		public static IDisposable Subscribe<T>(this IObservable<T> source, Func<T, string> success, string failure, Action retryAction = null)
		{
			return source.Subscribe(
				data => MukyutterClient.Current.NotificationService.Notify(success(data)),
				ex => MukyutterClient.Current.NotificationService.Notify(failure, ex));
		}

	}
}
