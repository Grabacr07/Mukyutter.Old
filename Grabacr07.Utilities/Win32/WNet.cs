using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// Windows ネットワーク (WNet) の機能を提供します。
	/// </summary>
	public static class WNet
	{
		/// <summary>
		/// ネットワーク資源に接続します。ローカルデバイスをネットワーク資源へリダイレクトできます。
		/// </summary>
		/// <param name="hwndOwner">ネットワーク資源のプロバイダがダイアログボックスのオーナーウィンドウとして利用できるウィンドウのハンドルを指定します。</param>
		/// <param name="lpNetResource">希望の接続の詳細（ ネットワーク資源、ローカルデバイス、ネットワーク資源のプロバイダの情報）を指定する 構造体へのポインタを指定します。</param>
		/// <param name="lpPassword">ネットワーク接続に使うパスワードを表す、NULL で終わる文字列へのポインタを指定します。</param>
		/// <param name="lpUsername">続に使うユーザー名を表す、NULL で終わる文字列へのポインタを指定します。</param>
		/// <param name="dwFlags">接続オプションを指定する 1 個の DWORD 値を指定します。</param>
		/// <returns>関数が成功すると、NO_ERROR が返ります。</returns>
		[DllImport("mpr.dll", EntryPoint = "WNetAddConnection3", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
		public static extern int WNetAddConnection3(IntPtr hwndOwner, ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, Int32 dwFlags);

		/// <summary>
		/// ローカル装置に対応するネットワーク資源の名前を取得します。
		/// </summary>
		/// <param name="localName">ネットワーク名が必要なローカル装置の名前を表す NULL で終わる文字列へのポインタを指定します。</param>
		/// <param name="remoteName">接続に使われているリモート名を表す NULL で終わる文字列を受け取るバッファへのポインタを指定します。</param>
		/// <param name="length">lpRemoteName パラメータが指すバッファのサイズ（ 文字数）が入った変数へのポインタを指定します。</param>
		/// <returns>関数が成功すると、NO_ERROR が返ります。</returns>
		[DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int WNetGetConnection([MarshalAs(UnmanagedType.LPTStr)] string localName, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName, ref int length);

		/// <summary>
		/// ネットワークリソースの列挙、または、現在の接続の列挙を開始します。
		/// </summary>
		/// <param name="dwScope">列挙したいリソースのスコープを指定します。</param>
		/// <param name="dwType">列挙したいリソースのタイプを指定します。</param>
		/// <param name="dwUsage">列挙したいリソースの用途を指定します。</param>
		/// <param name="lpNetResource">列挙したいコンテナを指定する 構造体へのポインタを指定します。</param>
		/// <param name="lphEnum">それ以降の WNetEnumResource の呼び出しに利用できる列挙ハンドルへのポインタを指定します。</param>
		/// <returns>関数が成功すると、NO_ERROR が返ります。</returns>
		[DllImport("mpr.dll", CharSet = CharSet.Auto)]
		public static extern int WNetOpenEnum(RESOURCE_SCOPE dwScope, RESOURCE_TYPE dwType, RESOURCE_USAGE dwUsage, [MarshalAs(UnmanagedType.AsAny)][In] Object lpNetResource, out IntPtr lphEnum);

		/// <summary>
		/// WNetOpenEnum 関数を呼び出して開始したネットワークリソースの列挙を継続します。
		/// </summary>
		/// <param name="hEnum">挙インスタンスを識別するハンドルを指定します。</param>
		/// <param name="lpcCount">要求したいエントリ数を指定する変数へのポインタを指定します。</param>
		/// <param name="lpBuffer">列挙の結果を受け取るバッファへのポインタを指定します。</param>
		/// <param name="lpBufferSize">lpBuffer パラメータで指定されたバッファのサイズ（ バイト単位）を保持する変数へのポインタを指定します。</param>
		/// <returns>関数が成功すると、NO_ERROR が返ります。</returns>
		[DllImport("mpr.dll", CharSet = CharSet.Auto)]
		public static extern int WNetEnumResource(IntPtr hEnum, ref int lpcCount, IntPtr lpBuffer, ref int lpBufferSize);

		/// <summary>
		/// WNetOpenEnum 関数を呼び出して開始したネットワークリソースの列挙を終了します。
		/// </summary>
		/// <param name="hEnum">列挙インスタンスを識別するハンドルを指定します。</param>
		/// <returns>関数が成功すると、NO_ERROR が返ります。</returns>
		[DllImport("mpr.dll", CharSet = CharSet.Auto)]
		public static extern int WNetCloseEnum(IntPtr hEnum);

		/// <summary>
		/// すべてのディスクリソースを示す値です。
		/// </summary>
		public const int RESOURCETYPE_DISK = 0x00000001;

		/// <summary>
		/// オペレーティングシステムが認証目的でユーザーと対話できます。
		/// </summary>
		public const Int32 CONNECT_INTERACTIVE = 0x00000008;

		/// <summary>
		/// この関数は、UNIVERSAL_NAME_INFO 構造体をバッファに格納します。
		/// </summary>
		public const int UNIVERSAL_NAME_INFO_LEVEL = 0x00000001;

		/// <summary>
		/// この操作を正しく終了しました。
		/// </summary>
		public const int NO_ERROR = 0x00000000;

		/// <summary>
		/// アクセスが拒否されました。
		/// </summary>
		public const int ERROR_ACCESS_DENIED = 0x00000005;

		/// <summary>
		/// ネットワーク パスが見つかりません。
		/// </summary>
		public const int ERROR_BAD_NETPATH = 0x00000035;

		/// <summary>
		/// ネットワーク名が見つかりません。
		/// </summary>
		public const int ERROR_BAD_NET_NAME = 0x00000043;

		/// <summary>
		/// データがさらにあります。
		/// </summary>
		public const int ERROR_MORE_DATA = 0x000000EA;

		/// <summary>
		/// 指定されたネットワーク パスワードが間違っています。
		/// </summary>
		public const int ERROR_INVALID_PASSWORD = 0x00000056;

		/// <summary>
		/// データはこれ以上ありません。
		/// </summary>
		public const int ERROR_NO_MORE_ITEMS = 0x00000103;

		/// <summary>
		/// 指定されたネットワーク パスはどのネットワーク プロバイダによっても受け付けられませんでした。
		/// </summary>
		public const int ERROR_NO_NET_OR_BAD_PATH = 0x000004B3;

		/// <summary>
		/// ネットワークが存在しないか、または起動されていません。
		/// </summary>
		public const int ERROR_NO_NETWORK = 0x000004C6;

		/// <summary>
		/// この操作はユーザーによって取り消されました。
		/// </summary>
		public const int ERROR_CANCELLED = 0x000004C7;

		/// <summary>
		/// パラメータが指す文字列が無効です。
		/// </summary>
		public const int ERROR_BAD_DEVICE = 0x000004B0;

		/// <summary>
		/// 装置は現在接続されていませんが、恒久的な接続として記憶されています。
		/// </summary>
		public const int ERROR_CONNECTION_UNAVAIL = 0x000004B1;

		/// <summary>
		/// ネットワーク固有のエラーが発生しました。
		/// </summary>
		public const int ERROR_EXTENDED_ERROR = 0x000004B8;

		/// <summary>
		/// この要求はサポートされていません。
		/// </summary>
		public const int ERROR_NOT_SUPPORTED = 0x00000032;

		/// <summary>
		/// パラメータで指定した装置がリダイレクトされていません。
		/// </summary>
		public const int ERROR_NOT_CONNECTED = 0x000008CA;

		/// <summary>
		/// 接続の詳細を定義します。
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct NETRESOURCE
		{
			/// <summary>
			/// WNetAddConnection3 では無視されます。
			/// </summary>
			public int dwScope;

			/// <summary>
			/// 接続先ネットワーク資源の種類を指定します。
			/// </summary>
			public int dwType;

			/// <summary>
			/// WNetAddConnection3 では無視されます。
			/// </summary>
			public int dwDisplayType;

			/// <summary>
			/// WNetAddConnection3 では無視されます。
			/// </summary>
			public int dwUsage;

			/// <summary>
			/// リダイレクトしたいローカルデバイスの名前を表す、NULL で終わる文字列へのポインタです。
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpLocalName;

			/// <summary>
			/// 接続先ネットワーク資源の名前を表す、NULL で終わる文字列へのポインタです。
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpRemoteName;

			/// <summary>
			/// WNetAddConnection3 では無視されます。
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpComment;

			/// <summary>
			/// 接続先ネットワークプロバイダの名前を表す、NULL で終わる文字列へのポインタです。
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpProvider;
		}

		/// <summary>
		/// 列挙したいリソースのスコープを指定します。
		/// </summary>
		public enum RESOURCE_SCOPE
		{
			/// <summary>
			/// 現在接続されているすべてのリソースを列挙します。
			/// </summary>
			RESOURCE_CONNECTED = 0x00000001,

			/// <summary>
			/// ネットワーク上のすべてのリソースを列挙します。
			/// </summary>
			RESOURCE_GLOBALNET = 0x00000002,

			/// <summary>
			/// 記憶されている（永続的な）すべての接続を列挙します。
			/// </summary>
			RESOURCE_REMEMBERED = 0x00000003,

			/// <summary>
			/// RESOURCE_RECENT
			/// </summary>
			RESOURCE_RECENT = 0x00000004,

			/// <summary>
			/// 呼び出し側のネットワークコンテキスト内のリソースだけを列挙します。
			/// </summary>
			RESOURCE_CONTEXT = 0x00000005
		}

		/// <summary>
		/// 列挙したいリソースのタイプを指定します。
		/// </summary>
		public enum RESOURCE_TYPE
		{
			/// <summary>
			/// すべてのリソース。
			/// </summary>
			RESOURCETYPE_ANY = 0x00000000,

			/// <summary>
			/// すべてのディスクリソース。
			/// </summary>
			RESOURCETYPE_DISK = 0x00000001,

			/// <summary>
			/// すべての印刷リソース。
			/// </summary>
			RESOURCETYPE_PRINT = 0x00000002,

			/// <summary>
			/// RESOURCETYPE_RESERVED
			/// </summary>
			RESOURCETYPE_RESERVED = 0x00000008,
		}

		/// <summary>
		/// 列挙したいリソースの用途を指定します。
		/// </summary>
		public enum RESOURCE_USAGE
		{
			/// <summary>
			/// 接続可能なすべてのリソース。
			/// </summary>
			RESOURCEUSAGE_CONNECTABLE = 0x00000001,

			/// <summary>
			/// すべてのコンテナリソース。
			/// </summary>
			RESOURCEUSAGE_CONTAINER = 0x00000002,

			/// <summary>
			/// RESOURCEUSAGE_NOLOCALDEVICE
			/// </summary>
			RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,

			/// <summary>
			/// RESOURCEUSAGE_SIBLING
			/// </summary>
			RESOURCEUSAGE_SIBLING = 0x00000008,

			/// <summary>
			/// この値をセットすると、ユーザーが認証されていない場合に WNetOpenEnum が失敗します。
			/// </summary>
			RESOURCEUSAGE_ATTACHED = 0x00000010,

			/// <summary>
			/// この値は、RESOURCEUSAGE_CONNECTABLE、RESOURCEUSAGE_CONTAINER、RESOURCEUSAGE_ATTACHED の組み合わせと同じことを意味します。
			/// </summary>
			RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
		}
	}
}
