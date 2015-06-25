
namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// ウィンドウ メッセージを示す識別子を定義します。
	/// </summary>
	public enum WindowMessage : int
	{
		/// <summary>
		/// 0x0201
		/// </summary>
		WM_LBUTTONDOWN = 0x0201,

		/// <summary>
		/// 0x0210
		/// </summary>
		WM_PARENTNOTIFY = 0x0210,

		/// <summary>
		/// 0x0047
		/// </summary>
		WM_WINDOWPOSCHANGED = 0x0047,

		/// <summary>
		/// 0x0082
		/// </summary>
		WM_NCDESTROY = 0x0082,

		/// <summary>
		/// 0x03E8
		/// </summary>
		WM_DDE_EXECUTE = 0x03E8,

		/// <summary>
		/// 0x03E0
		/// </summary>
		WM_DDE_INITIATE = 0x03E0,

		/// <summary>
		/// 0x03E1
		/// </summary>
		WM_DDE_TERMINATE = 0x03E1,

		/// <summary>
		/// 0x03E4
		/// </summary>
		WM_DDE_ACK = 0x03E4,

		/// <summary>
		/// 0x0308
		/// </summary>
		WM_DRAWCLIPBOARD = 0x0308,

		/// <summary>
		/// 0x030D
		/// </summary>
		WM_CHANGECBCHAIN = 0x030D,

		/// <summary>
		/// 0x007E
		/// </summary>
		WM_DISPLAYCHANGE = 0x007E,

		/// <summary>
		/// 0x001A
		/// </summary>
		WM_SETTINGCHANGE = 0x001A,

		/// <summary>
		/// 0x001D
		/// </summary>
		WM_FONTCHANGE = 0x001D,

		/// <summary>
		/// 0x001E
		/// </summary>
		WM_TIMECHANGE = 0x001E,

		/// <summary>
		/// 0x030F
		/// </summary>
		WM_QUERYNEWPALETTE = 0x030F,

		/// <summary>
		/// 0x0310
		/// </summary>
		WM_PALETTEISCHANGING = 0x0310,

		/// <summary>
		/// 0x0311
		/// </summary>
		WM_PALETTECHANGED = 0x0311,

		/// <summary>
		/// 0x02B1
		/// </summary>
		WM_WTSSESSION_CHANGE = 0x02B1,

		/// <summary>
		/// 0x0218
		/// </summary>
		WM_POWERBROADCAST = 0x0218,

		/// <summary>
		/// 0x031A
		/// </summary>
		WM_THEMECHANGED = 0x031A,

		/// <summary>
		/// 0x0104
		/// </summary>
		WM_SYSKEYDOWN = 0x0104,

		/// <summary>
		/// 0x0021
		/// </summary>
		WM_MOUSEACTIVATE = 0x0021,

		/// <summary>
		/// 0x0086
		/// </summary>
		WM_NCACTIVATE = 0x0086,

		/// <summary>
		/// 0x0084
		/// </summary>
		WM_NCHITTEST = 0x0084, 
	}
}
