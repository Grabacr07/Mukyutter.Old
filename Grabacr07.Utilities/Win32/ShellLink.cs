using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// シェルが保有するクラスを定義します。
	/// </summary>
	[ComImport]
	[ComVisible(false)]
	[Guid("00021401-0000-0000-C000-000000000046")]
	public class ShortcutClass { }

	/// <summary>
	/// ファイル情報を格納する構造体を定義します。
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct WIN32_FIND_DATAW
	{
		/// <summary>
		/// ファイル属性。
		/// </summary>
		public uint dwFileAttributes;

		/// <summary>
		/// 作成日時が世界標準時で格納されるFILETIME構造体。
		/// </summary>
		public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

		/// <summary>
		/// 最終アクセス日時が世界標準時で格納されるFILETIME構造体。
		/// </summary>
		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

		/// <summary>
		/// 最終更新日時が世界標準時で格納されるFILETIME構造体。
		/// </summary>
		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

		/// <summary>
		/// ファイルサイズの上位32ビット。
		/// </summary>
		public uint nFileSizeHigh;

		/// <summary>
		/// ファイルサイズの下位32ビット。
		/// </summary>
		public uint nFileSizeLow;

		/// <summary>
		/// dwFileAttributesにFILE_ATTRIBUTE_REPARSE_POINTが含まれる場合はリパースタグ、そうでない場合は0が格納される。
		/// </summary>
		public uint dwReserved0;

		/// <summary>
		/// 予約領域。
		/// </summary>
		public uint dwReserved1;

		/// <summary>
		/// ファイル名を表すNULLで終わる文字列。
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string cFileName;

		/// <summary>
		/// DOS形式の8.3ファイル名を表すNULLで終わる文字列。
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
		public string cAlternateFileName;
	}

	/// <summary>
	/// 受け取るパスのタイプを定義します。
	/// </summary>
	[Flags]
	public enum SLGP : uint
	{
		/// <summary>
		/// SHORTPATH
		/// </summary>
		SHORTPATH = 0x0001,

		/// <summary>
		/// UNCPRIORITY
		/// </summary>
		UNCPRIORITY = 0x0002,

		/// <summary>
		/// RAWPATH
		/// </summary>
		RAWPATH = 0x0004,
	};

	/// <summary>
	/// シェルリンクを作成、変更、決定するための機能を提供します。
	/// </summary>
	[ComImport]
	[Guid("000214F9-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IShellLinkW
	{
		/// <summary>
		/// シェルリンクオブジェクトのパスとファイル名を取得します。
		/// </summary>
		/// <param name="pszFile">シェルリンクオブジェクトのパスおよびファイル名を格納するバッファのアドレスを指定します。</param>
		/// <param name="cch">pszFile パラメータで表されるバッファのサイズを指定します。</param>
		/// <param name="pfd">シェルリンクオブジェクトの情報を格納する WIN32_FIND_DATA 構造体のアドレスを指定します。</param>
		/// <param name="fFlags">受け取るパスのタイプを指定します。</param>
		void GetPath(
			[Out][MarshalAs(UnmanagedType.LPWStr)] 
            StringBuilder pszFile,
			int cch,
			[MarshalAs(UnmanagedType.Struct)] ref WIN32_FIND_DATAW pfd,
			SLGP fFlags);

		/// <summary>
		/// GetIDList のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _1();

		/// <summary>
		/// SetIDList のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _2();

		/// <summary>
		/// GetDescription のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _3();

		/// <summary>
		/// SetDescription のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _4();

		/// <summary>
		/// GetWorkingDirectory のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _5();

		/// <summary>
		/// SetWorkingDirectory のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _6();

		/// <summary>
		/// GetArguments のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _7();

		/// <summary>
		/// SetArguments のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _8();

		/// <summary>
		/// GetHotkey のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _9();

		/// <summary>
		/// SetHotkey のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _10();

		/// <summary>
		/// GetShowCmd のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _11();

		/// <summary>
		/// SetShowCmd のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _12();

		/// <summary>
		/// GetIconLocation のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _13();

		/// <summary>
		/// SetIconLocation のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _14();

		/// <summary>
		/// SetRelativePath のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _15();

		/// <summary>
		/// Resolve のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _16();

		/// <summary>
		/// SetPath のダミーメソッドです。
		/// このメソッドをコールすることはできません。
		/// </summary>
		void _17();
	}
}
