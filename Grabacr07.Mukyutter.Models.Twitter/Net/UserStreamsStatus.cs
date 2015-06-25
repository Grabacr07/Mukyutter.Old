using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Net
{
	public enum UserStreamsStatus
	{
		/// <summary>
		/// User streams は使用されていません。
		/// </summary>
		Disabled,

		/// <summary>
		/// User streams に接続中です。
		/// </summary>
		Connecting,

		/// <summary>
		/// User streams 接続されています。
		/// </summary>
		Connected,

		/// <summary>
		/// User streams 接続から切断されました。
		/// </summary>
		Disconnected,

		/// <summary>
		/// インターネット接続がオフラインです。
		/// </summary>
		Offline,
	}

	public static class UserStreamsStatusExtensions
	{
		public static string Message(this UserStreamsStatus status)
		{
			var message = "";
			switch (status)
			{
				case UserStreamsStatus.Disabled:
					message = "User streams は使用されていません。";
					break;
				case UserStreamsStatus.Connecting:
					message = "User streams に接続しています...";
					break;
				case UserStreamsStatus.Connected:
					message = "User streams 接続されています。";
					break;
				case UserStreamsStatus.Disconnected:
					message = "User streams 接続されていません。";
					break;
				case UserStreamsStatus.Offline:
					message = "インターネット接続がオフラインです。";
					break;
				default:
					message = "不明な状態です。";
					break;
			}
			return message;
		}
	}
}
