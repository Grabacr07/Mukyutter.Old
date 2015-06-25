using System;
using System.Runtime.InteropServices;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// HTML ヘルプ API (hhctrl.ocx) へのアクセスを行う機能を提供します。このクラスは継承できません。
	/// </summary>
	public static class HHInterop
	{
		/// <summary>
		/// ヘルプ ウィンドウを表示します。
		/// </summary>
		/// <param name="hwndCaller">アプリケーション ウィンドウへのハンドル。このウィンドウ ハンドルは、使用方法によって HTML Help の親、所有者、またはメッセージの受信者として使用されます。</param>
		/// <param name="pszFile">表示するファイル。</param>
		/// <param name="uCommand">実行するアクション。</param>
		/// <param name="dwData"><paramref name="uCommand"/> の値に従った追加のデータ。</param>
		/// <returns>ヘルプを表示した場合はそのハンドル。それ以外の場合は <see cref="T:System.IntPtr.Zero"/>。</returns>
		[DllImport("hhctrl.ocx", CharSet = CharSet.Unicode, EntryPoint = "HtmlHelpW")]
		public static extern IntPtr HtmlHelp(IntPtr hwndCaller, string pszFile, Commands uCommand, uint dwData);


		/// <summary>
		/// <see cref="M:Grabacr07.Utilities.Win32.HHInterop.HtmlHelp"/> メソッドの uCommand 引数に割り当てられるアクションを識別する定数を定義します。
		/// </summary>
		public enum Commands : uint
		{
			/// <summary>
			/// トピックが含まれる HTML ファイルの名前を dwData 引数として渡し、ヘルプ トピックを表示します。
			/// </summary>
			HH_DISPLAY_TOPIC = 0x0,

			/// <summary>
			/// 目次が開かれたヘルプ トピックを表示します。
			/// </summary>
			HH_DISPLAY_TOC = 0x1,

			/// <summary>
			/// 索引ページが開かれたヘルプ トピックを表示します。
			/// </summary>
			HH_DISPLAY_INDEX = 0x2,

			/// <summary>
			/// 検索ページが開かれたヘルプ トピックを表示します。
			/// </summary>
			HH_DISPLAY_SEARCH = 0x3,

			/// <summary>
			/// トピックのマップされたコンテキスト番号を dwData 引数として渡し、ヘルプ トピックを表示します。
			/// </summary>
			HH_HELP_CONTEXT = 0xF,
		}

	}
}
