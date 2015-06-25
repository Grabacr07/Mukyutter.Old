using System;
using System.Runtime.InteropServices;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// Windows GDI Client DLL (gdi32.dll) へアクセスする機能を提供します。
	/// </summary>
	public static class Gdi32
	{
		/// <summary>
		/// 指定されたデバイスコンテキストで、指定された 1 個のオブジェクトを選択します。新しいオブジェクトは、同じタイプの以前のオブジェクトを置き換えます。
		/// </summary>
		/// <param name="hdc">デバイスコンテキストのハンドルを指定します。</param>
		/// <param name="hgdiobj">選択対象のオブジェクトのハンドルを指定します。</param>
		/// <returns>リージョン以外のオブジェクトを指定した場合に関数が成功すると、置き換えが発生する前のオブジェクトのハンドルが返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc410576.aspx</remarks>
		[DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		/// <summary>
		/// 指定されたデバイスコンテキストに関連付けられているデバイスと互換性のある、ビットマップを作成します。
		/// </summary>
		/// <param name="hdc">デバイスコンテキストのハンドルを指定します。</param>
		/// <param name="nWidth">ビットマップの幅をピクセル単位で指定します。</param>
		/// <param name="nHeight">ビットマップの高さをピクセル単位で指定します。</param>
		/// <returns>関数が成功すると、ビットマップのハンドルが返ります。関数が失敗すると、NULL が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc428327.aspx</remarks>
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

		/// <summary>
		/// 指定された幅、高さ、色形式 (カラープレーン数と 1 ピクセル当たりの色数) を持つビットマップを作成します。モノクロームビットマップの作成に使います。
		/// </summary>
		/// <param name="nWidth">ビットマップの幅をピクセル単位で指定します。</param>
		/// <param name="nHeight">ビットマップの高さをピクセル単位で指定します。</param>
		/// <param name="cPlanes">デバイスが使っているカラープレーンの数を指定します。</param>
		/// <param name="cBitsPerPel">1 ピクセルの色を識別するのに必要なビット数を指定します。</param>
		/// <param name="lpvBits">複数のピクセルからなる長方形で、色を設定する複数の色データを保持している 1 個の配列へのポインタを指定します。長方形の各走査行は、ワード境界に整列されていなければなりません (ワード整列されていない走査行には 0 を追加しなければなりません)。NULL を指定すると、関数から制御が返ったとき、新しいビットマップの内容は不定になります。</param>
		/// <returns>関数が成功すると、ビットマップのハンドルが返ります。関数が失敗すると、NULL が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc428330.aspx</remarks>
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, IntPtr lpvBits);

		/// <summary>
		/// ペン、ブラシ、フォント、ビットマップ、リージョン、パレットのいずれかの論理オブジェクトを削除し、そのオブジェクトに関連付けられていたすべてのシステムリソースを解放します。オブジェクトを削除した後は、指定されたハンドルは無効になります。
		/// </summary>
		/// <param name="hObject">ペン、ブラシ、フォント、ビットマップ、リージョン、パレットのいずれかの論理オブジェクトのハンドルを指定します。</param>
		/// <returns>関数が成功すると、0 以外の値が返ります。指定したハンドルが有効でない場合、またはデバイスコンテキストでそのオブジェクトが選択されている場合は、0 が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc428362.aspx</remarks>
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		/// <summary>
		/// 指定されたデバイスと互換性のあるメモリデバイスコンテキストを作成します。
		/// </summary>
		/// <param name="hdc">既存のデバイスコンテキストのハンドルを指定します。NULL を指定すると、アプリケーションの現在の画面と互換性のあるメモリデバイスコンテキストが作成されます。</param>
		/// <returns>関数が成功すると、作成されたメモリデバイスコンテキストのハンドルが返ります。関数が失敗すると、NULL が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc428328.aspx</remarks>
		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		/// <summary>
		/// 指定されたデバイスコンテキストを削除します。
		/// </summary>
		/// <param name="hdc">デバイスコンテキストのハンドルを指定します。</param>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc428357.aspx</remarks>
		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hdc);


		/// <summary>
		/// ビットブロック転送を行います。コピー元からコピー先のデバイスコンテキストへ、指定された長方形内の各ピクセルの色データをコピーします。
		/// </summary>
		/// <param name="hdc">コピー先のデバイスコンテキストのハンドルを指定します。</param>
		/// <param name="nXDest">コピー先長方形の左上隅の x 座標を論理単位で指定します。</param>
		/// <param name="nYDest">コピー先長方形の左上隅の y 座標を論理単位で指定します。</param>
		/// <param name="nWidth">コピー先長方形の幅を論理単位で指定します。コピー元長方形のビットマップの幅でもあります。</param>
		/// <param name="nHeight">コピー先長方形の高さを論理単位で指定します。コピー元長方形のビットマップの高さでもあります。</param>
		/// <param name="hdcSrc">コピー元のデバイスコンテキストのハンドルを指定します。</param>
		/// <param name="nXSrc">コピー元長方形の左上隅の x 座標を論理単位で指定します。</param>
		/// <param name="nYSrc">コピー元長方形の左上隅の y 座標を論理単位で指定します。</param>
		/// <param name="dwRop">ラスタオペレーションコードを指定します。ラスタオペレーションコードは、最終的な色を決定するために、コピー元およびコピー先の色データを組み合わせる方法を定義します。</param>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc428307.aspx</remarks>
		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(
			IntPtr hdc,
			int nXDest,
			int nYDest,
			int nWidth,
			int nHeight,
			IntPtr hdcSrc,
			int nXSrc,
			int nYSrc,
			TernaryRasterOperations dwRop);

		/// <summary>
		/// ラスタオペレーションコードを示す識別子を定義します。
		/// </summary>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc428307.aspx</remarks>
		public enum TernaryRasterOperations : uint
		{
			/// <summary>
			/// 物理パレットのインデックス 0 に対応する色 (既定の物理パレットでは黒) で、コピー先の長方形を塗りつぶします。<para/>
			/// dest = source
			/// </summary>
			SRCCOPY = 0x00CC0020,

			/// <summary>
			/// 論理 OR 演算子を使って、コピー元の色とコピー先の色を組み合わせます。<para/>
			/// dest = source OR dest
			/// </summary>
			SRCPAINT = 0x00EE0086,

			/// <summary>
			/// 論理 AND 演算子を使って、コピー元の色とコピー先の色を組み合わせます。<para/>
			/// dest = source AND dest
			/// </summary>
			SRCAND = 0x008800C6,

			/// <summary>
			/// 論理 XOR 演算子を使って、コピー元の色とコピー先の色を組み合わせます。<para/>
			/// dest = source XOR dest
			/// </summary>
			SRCINVERT = 0x00660046,

			/// <summary>
			/// 論理 AND 演算子を使って、コピー先の色を反転した色と、コピー元の色を組み合わせます。<para/>
			/// dest = source AND (NOT dest)
			/// </summary>
			SRCERASE = 0x00440328,

			/// <summary>
			/// コピー元の色を反転して、コピー先へコピーします。<para/>
			/// dest = (NOT source)
			/// </summary>
			NOTSRCCOPY = 0x00330008,

			/// <summary>
			/// 論理 OR 演算子を使って、コピー元の色とコピー先の色を組み合わせ、さらに反転します。<para/>
			/// dest = (NOT src) AND (NOT dest)
			/// </summary>
			NOTSRCERASE = 0x001100A6,

			/// <summary>
			/// 論理 AND 演算子を使って、コピー元の色とコピー先の色を組み合わせます。<para/>
			/// dest = (source AND pattern)
			/// </summary>
			MERGECOPY = 0x00C000CA,

			/// <summary>
			/// 論理 OR 演算子を使って、コピー元の色を反転した色と、コピー先の色を組み合わせます。<para/>
			/// dest = (NOT source) OR dest
			/// </summary>
			MERGEPAINT = 0x00BB0226,

			/// <summary>
			/// 指定したパターンをコピー先へコピーします。<para/>
			/// dest = pattern
			/// </summary>
			PATCOPY = 0x00F00021,

			/// <summary>
			/// 論理 OR 演算子を使って、指定したパターンの色と、コピー元の色を反転した色を組み合わせます。さらに論理 OR 演算子を使って、その結果と、コピー先の色を組み合わせます。<para/>
			/// dest = DPSnoo
			/// </summary>
			PATPAINT = 0x00FB0A09,

			/// <summary>
			/// 論理 XOR 演算子を使って、指定したパターンの色と、コピー先の色を組み合わせます。<para/>
			/// dest = pattern XOR dest
			/// </summary>
			PATINVERT = 0x005A0049,

			/// <summary>
			/// コピー先長方形の色を反転します。<para/>
			/// dest = (NOT dest)
			/// </summary>
			DSTINVERT = 0x00550009,

			/// <summary>
			/// 物理パレットのインデックス 0 に対応する色 (既定の物理パレットでは黒) で、コピー先の長方形を塗りつぶします。<para/>
			/// dest = BLACK
			/// </summary>
			BLACKNESS = 0x00000042,

			/// <summary>
			/// 物理パレットのインデックス 1 に対応する色 (既定の物理パレットでは白) で、コピー先の長方形を塗りつぶします。<para/>
			/// dest = WHITE
			/// </summary>
			WHITENESS = 0x00FF0062,
		}
	}
}
