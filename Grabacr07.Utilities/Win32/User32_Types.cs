using System.Runtime.InteropServices;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// The POINT structure defines the x- and y- coordinates of a point.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		/// <summary>
		/// The x-coordinate of the point.
		/// </summary>
		public int X;

		/// <summary>
		/// The y-coordinate of the point.
		/// </summary>
		public int Y;

		/// <summary>
		/// X および Y 座標を指定して、<see cref="T:Grabacr07.Utilities.Win32.POINT"/>
		/// 構造体の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="x">The x-coordinate of the point.</param>
		/// <param name="y">The y-coordinate of the point.</param>
		public POINT(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Point 構造体から <see cref="System.Drawing.Point"/> に変換します。
		/// </summary>
		/// <param name="p">Point 構造体</param>
		/// <returns><see cref="System.Drawing.Point"/> のインスタンス</returns>
		public static implicit operator System.Drawing.Point(POINT p)
		{
			return new System.Drawing.Point(p.X, p.Y);
		}

		/// <summary>
		/// Point 構造体から <see cref="System.Windows.Point"/> に変換します。
		/// </summary>
		/// <param name="p">Point 構造体</param>
		/// <returns><see cref="System.Windows.Point"/> のインスタンス</returns>
		public static implicit operator System.Windows.Point(POINT p)
		{
			return new System.Windows.Point(p.X, p.Y);
		}

		/// <summary>
		/// <see cref="System.Drawing.Point"/> から Point 構造体に変換します。
		/// </summary>
		/// <param name="p"><see cref="System.Drawing.Point"/> のインスタンス</param>
		/// <returns>Point 構造体</returns>
		public static implicit operator POINT(System.Drawing.Point p)
		{
			return new POINT(p.X, p.Y);
		}
	}


	/// <summary>
	/// 四角形の左上隅と右下隅の座標を定義します。
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RECT
	{
		/// <summary>
		/// 四角形の左上隅の x 座標を指定します。
		/// </summary>
		public int left;

		/// <summary>
		/// 四角形の左上隅の y 座標を指定します。
		/// </summary>
		public int top;

		/// <summary>
		/// 四角形の右下隅の x 座標を指定します。
		/// </summary>
		public int right;

		/// <summary>
		/// 四角形の右下隅の y 座標を指定します。
		/// </summary>
		public int bottom;
	}
}
