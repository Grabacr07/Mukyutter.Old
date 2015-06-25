using System;
using System.Runtime.InteropServices;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// Shell API (Shlwapi.dll) へアクセスする機能を提供します。
	/// </summary>
	public static class Shlwapi
	{
		/// <summary>
		/// ファイルのサイズを成形してフォーマットします。
		/// </summary>
		/// <param name="nSize">変換するべき数値。</param>
		/// <param name="pBuffer">変換した書式化された文字列を受け取るバッファ。</param>
		/// <param name="nBufSize">バッファサイズ。</param>
		/// <returns>成功した場合、変換された文字列へのアドレス。失敗した場合は、IntPtr.Null を返します。</returns>
		/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/bb759971.aspx</remarks>
		[DllImport("shlwapi.dll", CharSet = CharSet.Ansi)]
		public static extern IntPtr StrFormatByteSize64A(Int64 nSize, byte[] pBuffer, uint nBufSize);
	}
}
