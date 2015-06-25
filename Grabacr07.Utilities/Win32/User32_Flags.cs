using System;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// user32.dll の GetAncestor 関数で取得する祖先の種類を示す識別子を定義します。
	/// </summary>
	public enum GetAncestorFlag : uint
	{
		/// <summary>
		/// 親ウィンドウを取得します。これには、GetParent 関数で取得されるような、オーナーウィンドウは含みません (0x1)。
		/// </summary>
		GA_PARENT = 0x1,

		/// <summary>
		/// 親ウィンドウのチェーンをたどってルートウィンドウを取得します (0x2)。
		/// </summary>
		GA_ROOT = 0x2,

		/// <summary>
		/// GetParent 関数が返す親ウィンドウのチェーンをたどって所有されているルートウィンドウを取得します (0x3)。
		/// </summary>
		GA_ROOTOWNER = 0x3,
	}


	/// <summary>
	/// SystemParametersInfo 関数のパラメーターです。
	/// </summary>
	public enum SPI : uint
	{
		/// <summary>
		/// プライマリモニタの作業領域のサイズを取得します。
		/// </summary>
		SPI_GETWORKAREA = 0x0030,
	}


	/// <summary>
	/// GetWindow 関数のパラメーターです。
	/// </summary>
	public enum GetWindowCommand : uint
	{
		/// <summary>
		/// 指定したウィンドウと同じ種類で最も高い Z オーダーを持つウィンドウのハンドルを取得します。
		/// </summary>
		GW_HWNDFIRST = 0,

		/// <summary>
		/// 指定したウィンドウと同じ種類で最も低い Z オーダーを持つウィンドウのハンドルを取得します。
		/// </summary>
		GW_HWNDLAST = 1,

		/// <summary>
		/// 指定したウィンドウより Z オーダーが 1 つ下のウィンドウのハンドルを取得します。
		/// </summary>
		GW_HWNDNEXT = 2,

		/// <summary>
		/// 指定したウィンドウより Z オーダーが 1 つ上のウィンドウのハンドルを取得します。
		/// </summary>
		GW_HWNDPREV = 3,

		/// <summary>
		/// 指定したウィンドウのオーナーウィンドウのハンドルを取得します。
		/// </summary>
		GW_OWNER = 4,

		/// <summary>
		/// 指定したウィンドウが親ウィンドウの場合は、Z オーダーが一番上の子ウィンドウのハンドルを取得します。
		/// </summary>
		GW_CHILD = 5,

		/// <summary>
		/// 定したウィンドウをオーナーとする有効なポップアップウィンドウのハンドルを取得します。
		/// </summary>
		GW_ENABLEDPOPUP = 6

	}


}
