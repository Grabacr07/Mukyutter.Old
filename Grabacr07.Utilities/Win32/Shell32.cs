using System;
using System.Runtime.InteropServices;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// Shell32.dll で定義される関数へのアクセスを行う機能を提供します。このクラスは継承できません。
	/// </summary>
	public static class Shell32
	{
		/// <summary>
		/// パスとして使用できる文字列の最大長。
		/// </summary>
		public const int MAX_PATH = 260;

		/// <summary>
		/// Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
		/// </summary>
		/// <param name="pszPath">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file name. Both absolute and relative paths are valid. </param>
		/// <param name="dwFileAttributes">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
		/// <param name="psfi">Pointer to a SHFILEINFO structure to receive the file information.</param>
		/// <param name="cbSizeFileInfo">The size, in bytes, of the SHFILEINFO structure pointed to by the psfi parameter.</param>
		/// <param name="uFlags">The flags that specify the file information to retrieve. This parameter can be a combination of the following values.</param>
		/// <returns>Returns a value whose meaning depends on the uFlags parameter. </returns>
		/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/bb762179(v=vs.85).aspx</remarks>
		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(
			string pszPath,
			uint dwFileAttributes,
			ref SHFILEINFO psfi,
			uint cbSizeFileInfo,
			SHGFI uFlags);

		/// <summary>
		/// 指定された実行可能ファイル、ダイナミックリンクライブラリ（DLL）、アイコンファイルのいずれかから、大きいアイコンまたは小さいアイコンを取得し、それら複数のアイコンのハンドルからなる 1 つの配列を作成します。
		/// </summary>
		/// <param name="lpszFile">実行可能ファイル、DLL、アイコンファイルのいずれかのファイル名を保持している、NULL で終わる文字列へのポインタを指定します。ここで指定したファイルからアイコンを取得します。</param>
		/// <param name="nIconIndex">取得対象のアイコンのインデックスを 0 ベースで指定します。たとえば、0 を指定すると、ファイルの最初のアイコンから、nIcons パラメータで指定した数だけアイコンを取得します。 nIconIndex パラメータで -1 を指定し、phiconLarge と phiconSmall の両方のパラメータで NULL を指定すると、指定されたファイル内のアイコンの総数が返ります。実行可能ファイルまたは DLL を指定した場合、RT_GROUP_ICON リソースの数が返ります。.ICO ファイルの場合、1 が返ります。 </param>
		/// <param name="phiconLarge">1 個の配列へのポインタを指定します。関数から制御が返ると、この配列に、ファイルから取得した複数の大きいアイコンのハンドルが格納されます。このような配列が必要ない場合、NULL を指定します。</param>
		/// <param name="phiconSmall">1 個の配列へのポインタを指定します。関数から制御が返ると、この配列に、ファイルから取得した複数の小さいアイコンのハンドルが格納されます。このような配列が必要ない場合、NULL を指定します。</param>
		/// <param name="nIcons">ファイルから取得するべきアイコンの数を指定します。</param>
		/// <returns>nIconIndex パラメータで -1 を指定し、phiconLarge と phiconSmall の両方のパラメータで NULL を指定すると、指定されたファイル内のアイコンの総数が返ります。それ以外の場合、指定されたファイルから取得することに成功したアイコンの数が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc410843.aspx</remarks>
		[DllImport("shell32.dll", EntryPoint = "ExtractIconEx", CharSet = CharSet.Auto)]
		public static extern int ExtractIconEx(
			[MarshalAs(UnmanagedType.LPTStr)] string lpszFile,
			int nIconIndex,
			out IntPtr phiconLarge,
			out IntPtr phiconSmall,
			uint nIcons);
	}


	/// <summary>
	/// Contains information about a file object.
	/// </summary>
	/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/bb759792(v=vs.85).aspx</remarks>
	public struct SHFILEINFO
	{
		/// <summary>
		/// A handle to the icon that represents the file. You are responsible for destroying this handle with DestroyIcon when you no longer need it.
		/// </summary>
		public IntPtr hIcon;

		/// <summary>
		/// The index of the icon image within the system image list.
		/// </summary>
		public int iIcon;

		/// <summary>
		/// An array of values that indicates the attributes of the file object. For information about these values, see the IShellFolder::GetAttributesOf method.
		/// </summary>
		public uint dwAttributes;

		/// <summary>
		/// A string that contains the name of the file as it appears in the Windows Shell, or the path and file name of the file that contains the icon representing the file.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Shell32.MAX_PATH)]
		public string szDisplayName;

		/// <summary>
		/// A string that describes the type of file.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string szTypeName;
	}


	/// <summary>
	/// shell32.dll の SHGetFileInfo 関数で使用するフラグを定義します。
	/// </summary>
	/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/bb762179(v=vs.85).aspx</remarks>
	[Flags]
	public enum SHGFI : uint
	{
		/// <summary>
		/// Retrieve the handle to the icon that represents the file and the index of the icon within the system image list. The handle is copied to the hIcon member of the structure specified by psfi, and the index is copied to the iIcon member.
		/// </summary>
		SHGFI_ICON = 0x100,				// アイコン・リソースの取得

		/// <summary>
		/// Modify SHGFI_ICON, causing the function to retrieve the file's large icon. The SHGFI_ICON flag must also be set.
		/// </summary>
		SHGFI_LARGEICON = 0x0,			// 大きいアイコン

		/// <summary>
		/// Modify SHGFI_ICON, causing the function to retrieve the file's small icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that contains small icon images. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
		/// </summary>
		SHGFI_SMALLICON = 0x1,			// 小さいアイコン

		/// <summary>
		/// Indicates that the function should not attempt to access the file specified by pszPath. Rather, it should act as if the file specified by pszPath exists with the file attributes passed in dwFileAttributes. This flag cannot be combined with the SHGFI_ATTRIBUTES, SHGFI_EXETYPE, or SHGFI_PIDL flags.
		/// </summary>
		SHGFI_USEFILEATTRIBUTES = 0x10,	// 拡張子のみも取得できるようにする

		/// <summary>
		/// Retrieve the string that describes the file's type. The string is copied to the szTypeName member of the structure specified in psfi.
		/// </summary>
		SHGFI_TYPENAME = 0x400,			// ファイルの種類
	}
}
