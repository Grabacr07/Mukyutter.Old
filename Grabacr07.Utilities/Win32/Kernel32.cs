using System;
using System.Runtime.InteropServices;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// Microsoft Windows Kernel Process (kernel32.dll) で定義される関数へのアクセスの機能を提供します。
	/// </summary>
	public static class Kernel32
	{
		/// <summary>
		/// 指定されたプロセスのプロセス識別子を取得します。
		/// </summary>
		/// <param name="hProcess">プロセス ハンドル。</param>
		/// <returns>プロセス ID。</returns>
		[DllImport("kernel32.dll")]
		public static extern int GetProcessId(IntPtr hProcess);

		/// <summary>
		/// 開いているオブジェクトハンドルを閉じます。
		/// </summary>
		/// <param name="hObject">開いているオブジェクトのハンドル。</param>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr hObject);

		/// <summary>
		/// アプリケーションで使用する DLL を探索するフォルダーを追加します。
		/// </summary>
		/// <param name="lpPathName">追加するフォルダーのパス。空文字 ("") を指定した場合、追加されているフォルダーを削除します。</param>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetDllDirectory(string lpPathName);

		/// <summary>
		/// 64 ビット形式のファイル時刻を、システム日時形式へ変換します。
		/// </summary>
		/// <param name="lpFileTime">64 ビット形式のファイル時刻を保持している、1 個の 構造体へのポインタを指定します。</param>
		/// <param name="lpSystemTime">1 個の 構造体へのポインタを指定します。</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
		public static extern bool FileTimeToSystemTime([In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpFileTime, out SYSTEMTIME lpSystemTime);

		/// <summary>
		/// オブジェクトを作成するか開き、そのオブジェクトをアクセスするために利用できるハンドルを返します。
		/// </summary>
		/// <param name="lpFileName">作成または開く対象のオブジェクトの名前を保持している、NULL で終わる文字列へのポインタを指定します。</param>
		/// <param name="dwDesiredAccess">オブジェクトへのアクセスのタイプを指定します。</param>
		/// <param name="dwShareMode">オブジェクトの共有方法を指定します。</param>
		/// <param name="SecurityAttributes">取得したハンドルを子プロセスへ継承することを許可するかどうかを決定する、1 個の 構造体へのポインタを指定します。</param>
		/// <param name="dwCreationDisposition">ファイルが存在する場合、または存在しない場合のファイルの扱い方を指定します。</param>
		/// <param name="dwFlagsAndAttributes">ファイルの属性とフラグを指定します。</param>
		/// <param name="hTemplateFile">テンプレートファイルに対して GENERIC_READ アクセス権を備えているハンドルを指定します。</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr SecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		/// <summary>
		/// 指定されたデバイスドライバへ制御コードを直接送信し、対応するデバイスに対応する動作をさせます。
		/// </summary>
		/// <param name="hDevice">動作を実行するデバイスのハンドルを指定します。</param>
		/// <param name="dwIoControlCode">動作を表す制御コードを指定します。</param>
		/// <param name="lpInBuffer">動作を実行するために必要なデータを保持する 1 つのバッファへのポインタを指定します。</param>
		/// <param name="nInBufferSize">lpInBuffer が指すバッファのバイト単位のサイズです。</param>
		/// <param name="lpOutBuffer">動作の出力データを受け取る 1 つのバッファへのポインタを指定します。</param>
		/// <param name="nOutBufferSize">lpOutBuffer が指すバッファのバイト単位のサイズです。</param>
		/// <param name="lpBytesReturned">lpOutBuffer が指すバッファへ格納されるデータのバイト単位のサイズを受け取る、変数へのポインタを指定します。</param>
		/// <param name="lpOverlapped">1 つの 構造体へのポインタを指定します。</param>
		/// <returns>関数が成功すると、0 以外の値が返ります。</returns>
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
		public static extern uint DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

		/// <summary>
		/// 文字列をグローバルアトムテーブルに格納し、その文字列を識別する一意の値（ アトム）を返します。
		/// </summary>
		/// <param name="atomName">グローバルアトムテーブルに格納したい、NULL で終わる文字列へのポインタを指定します。</param>
		/// <returns>関数が成功すると、新しく作成されたアトムが返ります。</returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern short GlobalAddAtom(string atomName);

		/// <summary>
		/// グローバル文字列アトムの参照カウントをデクリメントします。参照カウントが 0 になったときは、アトムに関連付けられている文字列を、グローバルアトムテーブルから削除します。
		/// </summary>
		/// <param name="atom">削除したいアトム</param>
		/// <returns>関数が成功すると、0 が返ります。</returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GlobalDeleteAtom(short atom);

		/// <summary>
		/// グローバルメモリオブジェクトをロックし、メモリブロックの最初の 1 バイトへのポインタを返します。
		/// </summary>
		/// <param name="hMem">グローバルメモリオブジェクトのハンドルを指定します。</param>
		/// <returns>関数が成功すると、メモリブロックの最初の 1 バイトへのポインタが返ります。</returns>
		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalLock(IntPtr hMem);

		/// <summary>
		/// GMEM_MOVEABLE を指定して割り当てたグローバルメモリオブジェクトのロックカウントを減らします。
		/// </summary>
		/// <param name="hMem">グローバルメモリオブジェクトのハンドルを指定します。</param>
		/// <returns>ロックカウントを減らした後もメモリオブジェクトが依然としてロックされている場合、0 以外の値が返ります。</returns>
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GlobalUnlock(IntPtr hMem);

		/// <summary>
		/// 指定されたプロセスの最小ワーキングセットサイズと最大ワーキングセットサイズを設定します。
		/// </summary>
		/// <param name="hProcess">操作対象プロセスの開いているハンドル</param>
		/// <param name="dwMinimumWorkingSetSize">最小ワーキングセットサイズ</param>
		/// <param name="dwMaximumWorkingSetSize">最大ワーキングセットサイズ</param>
		/// <returns>関数が成功すると、0 以外の値が返ります。</returns>
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetProcessWorkingSetSize(IntPtr hProcess, UIntPtr dwMinimumWorkingSetSize, UIntPtr dwMaximumWorkingSetSize);

		/// <summary>
		/// 月、日、年、曜日、時、分、秒、ミリ秒の各メンバーを使用し日付と時間を表します。
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct SYSTEMTIME
		{
			/// <summary>
			/// 年を表します。
			/// </summary>
			[MarshalAs(UnmanagedType.U2)]
			public short Year;

			/// <summary>
			/// 月を表します。
			/// </summary>
			[MarshalAs(UnmanagedType.U2)]
			public short Month;

			/// <summary>
			/// 日を表します。
			/// </summary>
			[MarshalAs(UnmanagedType.U2)]
			public short DayOfWeek;

			/// <summary>
			/// 曜日を表します。
			/// </summary>
			[MarshalAs(UnmanagedType.U2)]
			public short Day;

			/// <summary>
			/// 時を表します。
			/// </summary>
			[MarshalAs(UnmanagedType.U2)]
			public short Hour;

			/// <summary>
			/// 分を表します。
			/// </summary>
			[MarshalAs(UnmanagedType.U2)]
			public short Minute;

			/// <summary>
			/// 秒を表します。
			/// </summary>
			[MarshalAs(UnmanagedType.U2)]
			public short Second;

			/// <summary>
			/// ミリ秒を表します。
			/// </summary>
			[MarshalAs(UnmanagedType.U2)]
			public short Milliseconds;

			/// <summary>
			/// <see cref="T:Grabacr07.Utilities.Win32.SYSTEMTIME"/> を初期化します。
			/// </summary>
			/// <param name="dt"></param>
			public SYSTEMTIME(DateTime dt)
			{
				dt = dt.ToUniversalTime();  // SetSystemTime expects the SYSTEMTIME in UTC
				Year = (short)dt.Year;
				Month = (short)dt.Month;
				DayOfWeek = (short)dt.DayOfWeek;
				Day = (short)dt.Day;
				Hour = (short)dt.Hour;
				Minute = (short)dt.Minute;
				Second = (short)dt.Second;
				Milliseconds = (short)dt.Millisecond;
			}
		}

		/// <summary>
		/// The REPARSE_DATA_BUFFER structure contains reparse point data for a Microsoft reparse point.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct REPARSE_GUID_DATA_BUFFER
		{
			/// <summary>
			/// Reparse point tag. Must be a Microsoft reparse point.
			/// </summary>
			public uint ReparseTag;

			/// <summary>
			/// Size, in bytes, of the reparse data in the DataBuffer member.
			/// </summary>
			public ushort ReparseDataLength;

			/// <summary>
			/// Length, in bytes, of the unparsed portion of the file name pointed to by the FileName member of the associated file object. For more information about the FileName member, see FILE_OBJECT.
			/// </summary>
			ushort Reserved;

			/// <summary>
			/// Offset, in bytes, of the substitute name string in the PathBuffer array.
			/// </summary>
			public ushort SubstituteNameOffset;

			/// <summary>
			/// Length, in bytes, of the substitute name string.
			/// </summary>
			public ushort SubstituteNameLength;

			/// <summary>
			/// Offset, in bytes, of the print name string in the PathBuffer array.
			/// </summary>
			public ushort PrintNameOffset;

			/// <summary>
			/// Length, in bytes, of the print name string.
			/// </summary>
			public ushort PrintNameLength;

			/// <summary>
			/// First character of the path string.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3FF0)]
			public char[] PathBuffer;
		}

		/// <summary>
		/// ファイルを開きます。指定したファイルが存在していない場合、この関数は失敗します。
		/// </summary>
		public const uint OPEN_EXISTING = 3;

		/// <summary>
		/// このフラグを指定すると、NTFS の再解析ポイントで再解析を行うことを禁止します。
		/// </summary>
		public const uint FILE_FLAG_OPEN_REPARSE_POINT = 0x200000;

		/// <summary>
		/// バックアップまたは復元操作の目的で、ファイルを開くまたは作成するようシステムに指示します。
		/// </summary>
		public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;

		/// <summary>
		/// ファイルまたはディレクトリの再解析ポイントデータを返します。
		/// </summary>
		public const uint FSCTL_GET_REPARSE_POINT = 0x900A8;
	}
}
