using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// Windows User API Client DLL (User32.dll) で定義される関数へのアクセスを行う機能を提供します。このクラスは継承できません。
	/// </summary>
	public static class User32
	{
		/// <summary>
		/// 指定された文字列と一致するクラス名とウィンドウ名を持つトップレベルウィンドウ (親を持たないウィンドウ) のハンドルを返します。
		/// </summary>
		/// <remarks>
		/// Win32 API の FindWindow 関数を実行します。<para />
		/// この関数は、子ウィンドウは探しません。検索では、大文字小文字は区別されません。
		/// </remarks>
		/// <param name="lpClassName">クラス名。</param>
		/// <param name="lpWindowName">ウィンドウ名。</param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		/// <summary>
		/// <see cref="M:Grabacr07.Utilities.Win32.User32.EnumWindows"/>
		/// メソッドで使用される、EnumWindows 関数のコールバック メソッドを表します。
		/// </summary>
		/// <param name="hWnd">トップレベルウィンドウのハンドル。</param>
		/// <param name="lParam">EnumWindows 関数または EnumDesktopWindows 関数から渡されるアプリケーション定義の値。</param>
		/// <returns>列挙を停止する場合は 0、列挙を続行する場合は 0 以外。</returns>
		public delegate int EnumWindowsCallback(IntPtr hWnd, int lParam);

		/// <summary>
		/// 画面上のすべてのトップレベルウィンドウを列挙します。
		/// </summary>
		/// <remarks>
		/// Win32 API の EnumWindows 関数を実行します。<para />
		/// この関数を呼び出すと、各ウィンドウのハンドルが順々にアプリケーション定義のコールバック関数に渡されます。EnumWindows 関数は、すべてのトップレベルリンドウを列挙し終えるか、またはアプリケーション定義のコールバック関数から 0 (FALSE) が返されるまで処理を続けます。
		/// </remarks>
		/// <param name="lpEnumFunc">アプリケーション定義のコールバック関数へのポインタ。</param>
		/// <param name="lParam">コールバック関数に渡すアプリケーション定義の値。</param>
		/// <returns>関数の実行に成功した場合は 0 以外の値、それ以外は 0。</returns>
		[DllImport("user32.dll")]
		public static extern int EnumWindows(EnumWindowsCallback lpEnumFunc, int lParam);

		/// <summary>
		/// 指定された親ウィンドウに属する子ウィンドウを列挙します。
		/// </summary>
		/// <param name="hWnd">親ウィンドウのハンドルを指定します。</param>
		/// <param name="lpEnumFunc">コールバック関数へのポインタを指定します。</param>
		/// <param name="lParam">アプリケーション定義の値を指定します。</param>
		/// <returns>関数が成功すると、0 以外の値が返ります。関数が失敗すると、0 が返ります。拡張エラー情報を取得するには、SetError関数を使います。</returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int EnumChildWindows(IntPtr hWnd, EnumWindowsCallback lpEnumFunc, int lParam);



		/// <summary>
		/// 指定されたウィンドウに関する情報を取得します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <param name="nIndex">取得する値の 0 から始まるオフセットを指定します。</param>
		/// <returns>ウィンドウ　スタイル。</returns>
		public static WS GetWindowLong(this IntPtr hWnd)
		{
			return (WS)User32.GetWindowLong(hWnd, (int)GWL.STYLE);
		}

		/// <summary>
		/// 指定されたウィンドウに関する情報を取得します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <param name="nIndex">取得する値の 0 から始まるオフセットを指定します。</param>
		/// <returns>ウィンドウ　スタイル。</returns>
		public static WSEX GetWindowLongEx(this IntPtr hWnd)
		{
			return (WSEX)User32.GetWindowLong(hWnd, (int)GWL.EXSTYLE);
		}

		/// <summary>
		/// 指定されたウィンドウに関する情報を取得します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <param name="nIndex">取得する値の 0 から始まるオフセットを指定します。</param>
		/// <returns>ウィンドウ　スタイル。</returns>
		[DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);


		/// <summary>
		/// ウィンドウの属性を変更します。
		/// </summary>
		/// <param name="hWnd">ウィンドウ ハンドル。</param>
		/// <param name="nIndex">設定する値の 0 から始まるオフセット。</param>
		/// <param name="dwNewLong">新しく設定する値。</param>
		/// <returns>関数の実行に成功した場合は変更前のウィンドウ属性。それ以外は 0。</returns>
		public static WS SetWindowLong(this IntPtr hWnd, WS dwNewLong)
		{
			return (WS)User32.SetWindowLong(hWnd, (int)GWL.STYLE, (int)dwNewLong);
		}
		/// <summary>
		/// ウィンドウの属性を変更します。
		/// </summary>
		/// <param name="hWnd">ウィンドウ ハンドル。</param>
		/// <param name="nIndex">設定する値の 0 から始まるオフセット。</param>
		/// <param name="dwNewLong">新しく設定する値。</param>
		/// <returns>関数の実行に成功した場合は変更前のウィンドウ属性。それ以外は 0。</returns>
		public static WSEX SetWindowLongEx(this IntPtr hWnd, WSEX dwNewLong)
		{
			return (WSEX)User32.SetWindowLong(hWnd, (int)GWL.EXSTYLE, (int)dwNewLong);
		}

		/// <summary>
		/// 指定されたウィンドウの属性を変更します。
		/// </summary>
		/// <param name="hWnd">ウィンドウ ハンドル。</param>
		/// <param name="nIndex">設定する値の 0 から始まるオフセット。</param>
		/// <param name="dwNewLong">新しく設定する値。</param>
		/// <returns>関数の実行に成功した場合は変更前のウィンドウ属性。それ以外は 0。</returns>
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


		public static bool SetWindowPos(this IntPtr hWnd)
		{
			return User32.SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP.NOMOVE | SWP.NOSIZE | SWP.NOZORDER);
		}

		public static bool SetWindowPos(this IntPtr hWnd, int x, int y, int cx, int cy)
		{
			return User32.SetWindowPos(hWnd, IntPtr.Zero, x, y, cx, cy, SWP.NOMOVE | SWP.NOSIZE | SWP.NOZORDER);
		}

		/// <summary>
		/// 子ウィンドウ、ポップアップウィンドウ、またはトップレベルウィンドウのサイズ、位置、および Z オーダーを変更します。これらのウィンドウは、その画面上での表示に従って順序が決められます。最前面にあるウィンドウは最も高いランクを与えられ、Z オーダーの先頭に置かれます。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <param name="hWndInsertAfter">Z オーダーを決めるためのウィンドウハンドルを指定します。<paramref name="hWnd"/> で指定したウインドウは、このパラメータで指定したウィンドウの後ろに置かれます。</param>
		/// <param name="x">ウィンドウの左上端の新しい x 座標をクライアント座標で指定します。</param>
		/// <param name="y">ウィンドウの左上端の新しい y 座標をクライアント座標で指定します。</param>
		/// <param name="cx">ウィンドウの新しい幅をピクセル単位で指定します。</param>
		/// <param name="cy">ウィンドウの新しい高さをピクセル単位で指定します。</param>
		/// <param name="flags">ウィンドウのサイズと位置の変更に関するフラグを指定します。</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP flags);

		/// <summary>
		/// 指定された子ウィンドウの親ウィンドウまたはオーナーウィンドウのハンドルを返します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <returns>親ウィンドウまたはオーナーウィンドウのハンドルが返ります。</returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		/// <summary>
		/// 指定された子ウィンドウの親ウィンドウを変更します。
		/// </summary>
		/// <param name="hWndChild">子ウィンドウのハンドル。</param>
		/// <param name="hWndNewParent">新しい親ウィンドウ。NULL を指定すると、デスクトップウィンドウが新しい親ウィンドウになります。</param>
		/// <returns>関数が成功した場合、直前の親ウィンドウのハンドル。失敗した場合は null。</returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		/// <summary>
		/// 指定されたウィンドウハンドルを持つウィンドウが存在しているかどうかを調べます。
		/// </summary>
		/// <param name="hWnd">調査するウィンドウのハンドル。</param>
		/// <returns> 指定したウィンドウハンドルを持つウィンドウが存在している場合は、0 以外の値が返ります。<para />
		/// 指定したウィンドウハンドルを持つウィンドウが存在しない場合は、0 が返ります。</returns>
		[DllImport("user32.dll")]
		public static extern int IsWindow(IntPtr hWnd);

		/// <summary>
		/// 指定されたウィンドウの表示状態を調べます。
		/// </summary>
		/// <param name="hWnd">調査するウィンドウのハンドル。</param>
		/// <returns>指定されたウィンドウ、その親ウィンドウ、そのさらに上位の親ウィンドウのすべてが WS_VISIBLE スタイルを持つ場合は 0 以外の値。それ以外は 0。</returns>
		[DllImport("user32.dll")]
		public static extern int IsWindowVisible(IntPtr hWnd);

		/// <summary>
		/// 指定されたウィンドウのタイトルバーのテキストをバッファへコピーします。
		/// </summary>
		/// <remarks>
		/// Win32 API の GetWindowText 関数を実行します。<para />
		/// 指定されたウィンドウがコントロールの場合は、コントロールのテキストをコピーします。ただし、他のアプリケーションのコントロールのテキストを取得することはできません。
		/// </remarks>
		/// <param name="hWnd">ウィンドウまたはコントロールのハンドル。</param>
		/// <param name="lpString">テキスト バッファ。</param>
		/// <param name="nMaxCount">コピーする最大文字数。</param>
		/// <returns>関数が成功した場合、コピーされた文字列の文字数。タイトルバーやテキストがない場合、タイトルバーが空の場合、および hWnd パラメータに指定したウィンドウハンドルまたはコントロールハンドルが無効な場合は 0。</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		/// <summary>
		/// 指定されたウィンドウの位置とサイズを変更します。
		/// </summary>
		/// <remarks>
		/// Win32 API の MoveWindow 関数を実行します。<para />
		/// トップレベルウィンドウ (親を持たないウィンドウ)の場合は、画面左上端からの相対位置 (スクリーン座標) で位置とサイズを指定します。子ウィンドウの場合は、親ウィンドウのクライアント領域の左上端からの相対位置 (クライアント座標) で指定します。
		/// </remarks>
		/// <param name="hwnd">ウィンドウ ハンドル。</param>
		/// <param name="x">ウィンドウの左端の新しい位置。</param>
		/// <param name="y">ウィンドウの上端の新しい位置。</param>
		/// <param name="cx">ウィンドウの新しい幅。</param>
		/// <param name="cy">ウィンドウの新しい高さ。</param>
		/// <param name="repaint">ウィンドウを再描画するかどうかを表す値。</param>
		/// <returns>関数が成功した場合、0 以外の値。関数が失敗した場合は 0。</returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

		/// <summary>
		/// 指定されたウィンドウを作成したスレッドに関連付けられているメッセージキューに、1 つのメッセージをポストします (書き込みます)。
		/// </summary>
		/// <param name="hwnd">1 つのウィンドウのハンドルを指定します。</param>
		/// <param name="Msg">ポストするべきメッセージを指定します。</param>
		/// <param name="wParam">メッセージ特有の追加情報を指定します。</param>
		/// <param name="lParam">メッセージ特有の追加情報を指定します。</param>
		/// <returns>関数が成功すると、0 以外の値が返ります。</returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool PostMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// 指定されたウィンドウの左上端と右下端の座標をスクリーン座標で取得します。スクリーン座標は、表示画面の左上端が (0,0) となります。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <param name="rect">構造体へのポインタを指定します。この構造体の left メンバと top メンバに、スクリーン座標でのウィンドウ左上端の座標が入ります。right メンバと bottom メンバには、ウィンドウの右下端の座標が入ります。</param>
		/// <returns>成功した場合は 0 以外の値、それ以外の場合は 0。</returns>
		[DllImport("User32.Dll")]
		public static extern int GetWindowRect(IntPtr hWnd, out RECT rect);

		/// <summary>
		/// 指定された座標を含むウィンドウのハンドルを取得します。
		/// </summary>
		/// <param name="point">調査する座標が入った構造体を指定します。</param>
		/// <returns>関数が成功すると、指定した座標を含むウィンドウのハンドルが返ります。指定した座標にウィンドウがないときは、NULL が返ります。指定した座標がスタティックテキストコントロールに重なっていた場合は、そのスタティックテキストコントロールの下にあるウィンドウのハンドルが返ります。</returns>
		[DllImport("user32.dll")]
		public static extern IntPtr WindowFromPoint(POINT point);

		/// <summary>
		/// 指定された点を、クライアント座標からスクリーン座標へ変換します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。このウィンドウのクライアント領域を利用して変換を行います。</param>
		/// <param name="lpPoint">変換対象のクライアント座標を保持している、1 個の 構造体へのポインタを指定します。関数から制御が返り、関数が成功すると、この構造体にスクリーン座標が格納されます。</param>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		[DllImport("user32.dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

		/// <summary>
		/// 画面上にある指定された点を、スクリーン座標からクライアント座標へ変換します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。このウィンドウのクライアント領域を利用して変換を行います。</param>
		/// <param name="lpPoint">変換対象のスクリーン座標を保持している、1 個の 構造体へのポインタを指定します。関数から制御が返り、関数が成功すると、この構造体にクライアント座標が格納されます。</param>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		[DllImport("user32.dll")]
		public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

		/// <summary>
		/// 呼び出し側のスレッドに関連付けられているウィンドウの中から、キーボードフォーカスを持つウィンドウのハンドルを取得します。
		/// </summary>
		/// <returns>関数が成功すると、呼び出し側のスレッドに関連付けられている、キーボードフォーカスを持つウィンドウのハンドルが返ります。呼び出し側のスレッドのメッセージキューが、キーボードフォーカスを持つウィンドウを持たないときは、NULL が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc364641.aspx</remarks>
		[DllImport("user32.dll")]
		public static extern IntPtr GetFocus();

		/// <summary>
		/// 指定されたウィンドウにキーボードフォーカスを設定します。このウィンドウは、呼び出し側スレッドのメッセージキューにアタッチされているものでなければなりません。
		/// </summary>
		/// <param name="hWnd">キーボードフォーカスを設定したいウィンドウのハンドルを指定します。NULL を指定すると、キーストロークは無視されます。</param>
		/// <returns>関数が成功すると、以前にキーボードフォーカスを持っていたウィンドウのハンドルが返ります。hWnd パラメータが無効な場合や、指定のウィンドウが呼び出し側のスレッドのメッセージキューにアタッチされていない場合は、NULL が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc411074.aspx</remarks>
		[DllImport("user32.dll")]
		public static extern IntPtr SetFocus(IntPtr hWnd);

		/// <summary>
		/// 指定されたウィンドウの関連ウィンドウを取得します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <param name="nCmd">ウィンドウの取得情報を指定します。</param>
		/// <returns>関連ウィンドウのハンドルが返ります。</returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCommand nCmd);

		/// <summary>
		/// 指定したウィンドウの祖先のハンドルを取得します。
		/// </summary>
		/// <param name="hWnd">祖先を取得するウィンドウのハンドルを指定します。デスクトップウィンドウのハンドルを指定すると、NULL が返ります。</param>
		/// <param name="gaFlags">取得する祖先を指定します。</param>
		/// <returns>祖先のウィンドウのハンドルが返ります。</returns>
		[DllImport("user32.dll")]
		public static extern IntPtr GetAncestor(IntPtr hWnd, GetAncestorFlag gaFlags);

		/// <summary>
		/// フォアグラウンドウィンドウ (現在ユーザーが作業しているウィンドウ) のハンドルを返します。
		/// </summary>
		/// <returns>フォアグラウンドウィンドウのハンドルが返ります。</returns>
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		/// <summary>
		/// 指定されたウィンドウを作成したスレッドをフォアグラウンドにし、そのウィンドウをアクティブにします。
		/// </summary>
		/// <param name="hWnd">アクティブにし、フォアグラウンドにするウィンドウのハンドルを指定します。</param>
		/// <returns>ウィンドウがフォアグラウンドになったら、0 以外の値が返ります。</returns>
		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		/// 指定されたウィンドウの表示状態を設定します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <param name="nCmdShow">ウィンドウの表示方法を指定します。</param>
		/// <returns>ウィンドウが以前から表示されていた場合は、0 以外の値が返ります。</returns>
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		/// <summary>
		/// 別のスレッドによって作成されたウィンドウの表示状態を設定します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <param name="nCmdShow">ウィンドウの表示方法を指定します。</param>
		/// <returns>設定前にウィンドウが可視だった場合、0 以外の値が返ります。</returns>
		[DllImport("user32.dll")]
		public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

		/// <summary>
		/// 指定されたウィンドウが最小化 ( アイコン化) されているかどうかを調べます。
		/// </summary>
		/// <param name="hWnd">調査するウィンドウのハンドルを指定します。</param>
		/// <returns>ウィンドウが最小化されているときは、0 以外の値が返ります。</returns>
		[DllImport("user32.dll")]
		public static extern bool IsIconic(IntPtr hWnd);

		/// <summary>
		/// 特定のスレッドの入力処理機構を別のスレッドにアタッチします。
		/// </summary>
		/// <param name="idAttach">別のスレッドにアタッチするスレッドの識別子を指定します。システムスレッドをアタッチすることはできません。</param>
		/// <param name="idAttachTo">アタッチ先スレッドの識別子を指定します。システムスレッドは指定できません。スレッドをそれ自体にアタッチすることはできません。そのため、idAttachTo と idAttach を同じにすることはできません。</param>
		/// <param name="fAttach">スレッドをアタッチするかデタッチするか指定します。true に設定すると、2 つのスレッドがアタッチされます。false に設定すると、スレッドがデタッチされます。</param>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		[DllImport("user32.dll")]
		public static extern bool AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);

		/// <summary>
		/// 指定されたウィンドウを作成したスレッドの ID を取得します。必要であれば、ウィンドウを作成したプロセスの ID も取得できます。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
		/// <param name="ProcessId">プロセス ID を受け取る変数へのポインタを指定します。ポインタを指定すると、それが指す変数にプロセス ID がコピーされます。NULL を指定した場合は、プロセス ID の取得は行われません。</param>
		/// <returns>ウィンドウを作成したスレッドの ID。</returns>
		[DllImport("user32.dll")]
		public static extern int GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);


		/// <summary>
		/// 指定されたウィンドウのクライアント領域または画面全体を表すディスプレイデバイスコンテキストのハンドルを取得します。その後、GDI 関数を使って、返されたデバイスコンテキスト内で描画を行えます。
		/// </summary>
		/// <param name="hWnd">デバイスコンテキストの取得対象となるウィンドウのハンドルを指定します。NULL を指定すると、GetDC は画面全体を表すデバイスコンテキストを取得します。</param>
		/// <returns>関数が成功すると、指定したウィンドウのクライアント領域を表すデバイスコンテキストのハンドルが返ります。関数が失敗すると、NULL が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc428664.aspx</remarks>
		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);

		/// <summary>
		/// デバイスコンテキストを解放し、他のアプリケーションからつかえるようにします。ReleaseDC 関数の効果は、デバイスコンテキストのタイプによって異なります。この関数は、共通デバイスコンテキストとウィンドウデバイスコンテキストだけを解放します。クラスデバイスコンテキストやプライベートデバイスコンテキストには効果がありません。
		/// </summary>
		/// <param name="hWnd">解放対象のデバイスコンテキストに対応するウィンドウのハンドルを指定します。</param>
		/// <param name="hDC">解放対象のデバイスコンテキストのハンドルを指定します。</param>
		/// <returns>戻り値は、デバイスコンテキストを解放したかどうかを示します。デバイスコンテキストが解放された場合、1 が返ります。デバイスコンテキストが解放されなかった場合、0 が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc410542.aspx</remarks>
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		/// <summary>
		/// 複数の点を、あるウィンドウを基準とする座標空間から、他のウィンドウを基準とする座標空間へ変換（マップ）します。
		/// </summary>
		/// <param name="hWnd">変換対象の点を保持している（変換元）ウィンドウのハンドルを指定します。</param>
		/// <param name="hWndTo">変換後の点を保持する（変換先）ウィンドウのハンドルを指定します。</param>
		/// <param name="pt">変換対象の点の座標を保持している 構造体からなる 1 つの配列へのポインタを指定します。</param>
		/// <param name="cPoints">lpPoints パラメータで、複数の POINT 構造体からなる 1 つの配列へのポインタを指定した場合、配列内の POINT 構造体の数を指定します。</param>
		/// <returns>関数が成功すると、各点の移動距離を示す 32 ビット値が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc410437.aspx</remarks>
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(IntPtr hWnd, IntPtr hWndTo, ref POINT pt, int cPoints);

		/// <summary>
		/// 指定されたウィンドウまたはコントロールで、マウス入力とキーボード入力を有効または無効にします。
		/// </summary>
		/// <param name="handle">有効または無効にしたいウィンドウのハンドルを指定します。</param>
		/// <param name="bEnable">ウィンドウを有効にするか無効にするかを指定します。</param>
		/// <returns>ウィンドウが既に無効になっている場合、0 以外の値が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc410790.aspx</remarks>
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool EnableWindow(IntPtr handle, bool bEnable);

		/// <summary>
		/// システム全体に関するパラメータのいずれかを取得または設定します。この関数を使ってパラメータを設定する際に、ユーザープロファイルを更新することもできます。
		/// </summary>
		/// <param name="uiAction">取得または設定するべきシステムパラメータ</param>
		/// <param name="uiParam">実施するべき操作によって異なる</param>
		/// <param name="pvParam">実施するべき操作によって異なる</param>
		/// <param name="fWinIni">ユーザープロファイルの更新オプション</param>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc429946.aspx</remarks>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref RECT pvParam, uint fWinIni);


		/// <summary>
		/// マウスカーソル (マウスポインタ) の現在の位置に相当するスクリーン座標を取得します。
		/// </summary>
		/// <param name="lpPoint">マウスカーソルの位置に相当するスクリーン座標を受け取る 構造体へのポインタを指定します。</param>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		[DllImport("User32.dll")]
		public static extern bool GetCursorPos(ref POINT lpPoint);

		/// <summary>
		/// 指定されたウィンドウの更新リージョンが空ではない場合、ウィンドウへ メッセージを送信し、そのウィンドウのクライアント領域を更新します。この関数は、アプリケーションキューをとおさずに、指定されたウィンドウのウィンドウプロシージャへ直接 WM_PAINT メッセージを送信します。更新リージョンが空の場合、何もメッセージを送信しません。
		/// </summary>
		/// <param name="hWnd">更新したいウィンドウのハンドルを指定します。</param>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc428780.aspx</remarks>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		[DllImport("user32.dll")]
		public static extern bool UpdateWindow(IntPtr hWnd);

		/// <summary>
		/// デスクトップウィンドウのハンドルを取得します。
		/// </summary>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc364616.aspx</remarks>
		/// <returns>デスクトップウィンドウのハンドルが返ります。</returns>
		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		/// <summary>
		/// 指定されたウィンドウのプロパティリストからデータハンドルを取得します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドル</param>
		/// <param name="lpString">アトムまたは文字列</param>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc364724.aspx</remarks>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr GetProp(IntPtr hWnd, string lpString);

		/// <summary>
		/// 指定されたウィンドウのプロパティリストからプロパティエントリを削除します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドル</param>
		/// <param name="lpString">アトムまたは文字列</param>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc410987.aspx</remarks>
		/// <returns>指定された文字列が返ります。指定された文字列がプロパティリスト内で見つからなかった場合、NULL が返ります。</returns>
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr RemoveProp(IntPtr hWnd, string lpString);

		/// <summary>
		/// 指定されたウィンドウのプロパティリストに新しいエントリを追加するか、既存のエントリを変更します。指定された文字列がリスト内に存在しない場合、この関数は新しいエントリをリストに追加します。
		/// </summary>
		/// <param name="hWnd">ウィンドウのハンドル</param>
		/// <param name="lpString">アトムまたは文字列</param>
		/// <param name="handle">データのハンドル</param>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc411066.aspx</remarks>
		/// <returns>関数が成功してデータハンドルと文字列がプロパティリストに追加されると、0 以外の値が返ります。</returns>
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int SetProp(IntPtr hWnd, string lpString, IntPtr handle);

		/// <summary>
		/// 指定されたウィンドウが所有するポップアップウィンドウの中で最後にアクティブになったウィンドウを返します。
		/// </summary>
		/// <param name="hWnd">オーナーウィンドウのハンドル</param>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc364701.aspx</remarks>
		/// <returns>直前にアクティブであったポップアップウィンドウのハンドルが返ります。</returns>
		[DllImport("user32.dll")]
		public static extern IntPtr GetLastActivePopup(IntPtr hWnd);

		/// <summary>
		/// 1 つまたは複数のウィンドウへ、指定されたメッセージを送信します。この関数は、指定されたウィンドウのウィンドウプロシージャを呼び出し、そのウィンドウプロシージャがメッセージを処理し終わった後で、制御を返します。
		/// </summary>
		/// <param name="hWnd">送信先ウィンドウのハンドル</param>
		/// <param name="Msg">メッセージ</param>
		/// <param name="wParam">メッセージの最初のパラメータ</param>
		/// <param name="lParam">メッセージの 2 番目のパラメータ</param>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc411022.aspx</remarks>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// DDE の lParam 値を、プロセス間で DDE データを共有するときに使う内部構造にパックします。
		/// </summary>
		/// <param name="msg">ポストする DDE メッセージを指定します。</param>
		/// <param name="pLo">ポストしている DDE メッセージの lParam パラメータの、16 ビット Windows 上位ワードに対応する値を指定します。</param>
		/// <param name="pHi">ポストしている DDE メッセージの lParam パラメータの、16 ビット Windows 下位ワードに対応する値を指定します。</param>
		/// <returns>関数が成功すると、0 以外の値が返ります。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc430072.aspx</remarks>
		[DllImport("user32.dll")]
		public static extern IntPtr PackDDElParam(int msg, IntPtr pLo, IntPtr pHi);

		/// <summary>
		/// クリップボードビューアのチェインに、指定されたウィンドウを追加します。
		/// </summary>
		/// <param name="hWndNewViewer">クリップボードビューアウィンドウのハンドル</param>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc430089.aspx</remarks>
		/// <returns>関数が成功すると、クリップボードビューアのチェイン内で、追加したウィンドウの次に位置するウィンドウのハンドルが返ります。エラーが発生した場合、または、クリップボードビューアのチェイン内に他のウィンドウが存在しなかった場合は、NULL が返ります。拡張エラー情報を取得するには、 関数を使います。</returns>
		[DllImport("user32")]
		public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

		/// <summary>
		/// クリップボードビューアのチェインから、指定されたウィンドウを削除します。
		/// /// </summary>
		/// <param name="hWndRemove">削除したいウィンドウのハンドル</param>
		/// <param name="hWndNewNext">次のウィンドウのハンドル</param>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc429619.aspx</remarks>
		/// <returns>クリップボードビューアチェイン内のウィンドウに WM_CHANGECBCHAIN メッセージを渡した結果を示す値が返ります。</returns>
		[DllImport("user32")]
		public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
		
		/// <summary>
		/// 1 個のアイコンを破棄し、そのアイコンに割り当てられていたメモリを解放します。
		/// </summary>
		/// <param name="hIcon">破棄対象のアイコンのハンドルを指定します。使用中のアイコンを指定してはなりません。</param>
		/// <returns>関数が成功した場合は true、それ以外の場合は false。</returns>
		/// <remarks>http://msdn.microsoft.com/ja-jp/library/cc410764.aspx</remarks>
		[DllImport("user32")]
		public static extern bool DestroyIcon(IntPtr hIcon);


		/// <summary>
		/// オーナーウィンドウの Z オーダーを変更しません。
		/// </summary>
		public const int SWP_NOOWNERZORDER = 0x200;

		/// <summary>
		/// 変更結果を再描画しません。このフラグを指定すると、再描画は一切行われません。このフラグは、クライアント領域、非クライアント領域 ( タイトルバーおよびスクロールバーを含む) 、および親ウィンドウの、このウィンドウが移動した結果現れた部分のすべてに適用されます。このフラグをセットした場合、ウィンドウや親ウィンドウの再描画の必要な部分は、アプリケーションで明示的に無効化または再描画しなければなりません。
		/// </summary>
		public const int SWP_NOREDRAW = 0x8;

		/// <summary>
		/// 現在の Z オーダーを維持します (hWndInsertAfter パラメータを無視します)。
		/// </summary>
		public const int SWP_NOZORDER = 0x4;

		/// <summary>
		/// ウィンドウを表示します。
		/// </summary>
		public const int SWP_SHOWWINDOW = 0x0040;

		/// <summary>
		/// SetWindowLong 関数を使って新しいフレームスタイルの設定を適用します。ウィンドウサイズが変更されない場合にも、ウィンドウに WM_NCCALCSIZE メッセージを送ります。このフラグを指定しなかった場合、ウィンドウサイズが変更される場合にしか WM_NCCALCSIZE メッセージは送られません。
		/// </summary>
		public const int SWP_FRAMECHANGED = 0x20;

		/// <summary>
		/// ウィンドウをアクティブ化しません。このフラグをセットしなかった場合、ウィンドウはアクティブ化され、最前面ウィンドウまたは非最前面ウィンドウのどちらか (hWndInsertAfter パラメータの設定による) のグループの最上位に移動します。
		/// </summary>
		public const int SWP_NOACTIVATE = 0x10;

		/// <summary>
		/// ウィンドウに WM_WINDOWPOSCHANGING メッセージが送られないようにします。
		/// </summary>
		public const int SWP_NOSENDCHANGING = 0x0400;

		/// <summary>
		/// この関数を呼び出したスレッドとウィンドウを所有するスレッドが異なる入力キューに関連付けられている場合、ウィンドウを所有するスレッドへ要求が送られます。こうすると、要求を受け取ったスレッドが要求を処理している間も、関数を呼び出したスレッドの実行が止まってしまうことはありません。
		/// </summary>
		public const int SWP_ASYNCWINDOWPOS = 0x4000;

		/// <summary>
		/// 現在の位置を維持します (X パラメータと Y パラメータを無視します)。
		/// </summary>
		public const int SWP_NOMOVE = 0x2;

		/// <summary>
		/// 現在のサイズを維持します (cx パラメータと cy パラメータを無視します)。
		/// </summary>
		public const int SWP_NOSIZE = 0x1;

		/// <summary>
		/// 最前面ウィンドウを作成します。このウィンドウは、アクティブでないときにも他のウィンドウの前面に表示されます。このスタイルは、SetWindowPos 関数を使って有効にしたり無効にしたりできます。
		/// </summary>
		public const int WS_EX_TOPMOST = 0x00000008;

		/// <summary>
		/// MDI 子ウィンドウを作成します。
		/// </summary>
		public const int WS_EX_MDICHILD = 0x40;

		/// <summary>
		/// ツールウィンドウを作成します。
		/// </summary>
		public const int WS_EX_TOOLWINDOW = 0x00000080;

		/// <summary>
		/// 盛り上がった縁の境界線を持ちます。
		/// </summary>
		public const int WS_EX_WINDOWEDGE = 0x00000100;

		/// <summary>
		/// このスタイルで作成されたトップレベルウィンドウは、ユーザーがクリックしてもフォアグラウンドウィンドウになりません。ユーザーがフォアグラウンドウィンドウを最小化したり閉じたりしたときにも、システムがこのウィンドウをフォアグラウンドウィンドウにすることはありません。このウィンドウをアクティブにするには、SetActiveWindow 関数または SetForegroundWindow 関数を使います。既定では、このウィンドウはタスクバーには表示されません。ウィンドウがタスクバーに表示されるようにするには、WS_EX_APPWINDOW スタイルを指定します。
		/// </summary>
		public const int WS_EX_NOACTIVATE = 0x8000000;

		/// <summary>
		/// サイズ変更境界を持つウィンドウを作成します。
		/// </summary>
		public const int WS_SIZEBOX = 0x00040000;

		/// <summary>
		/// タイトル バーにコントロール メニュー ボックスを持つウィンドウを作成します。
		/// </summary>
		public const int WS_SYSMENU = 0x00080000;

		/// <summary>
		/// タイトル バーを持つウィンドウを作成します。
		/// </summary>
		public const int WS_CAPTION = 0x00C00000;

		/// <summary>
		/// 初期状態で表示されるウィンドウを作成します。
		/// </summary>
		public const int WS_VISIBLE = 0x10000000;

		/// <summary>
		/// 子ウィンドウを作成します。WS_POPUP スタイルと一緒に使うことはできません。
		/// </summary>
		public const int WS_CHILD = 0x40000000;

		/// <summary>
		/// 同じレベルで最前面のウィンドウを取得します。
		/// </summary>
		public const int GW_HWNDFIRST = 0;

		/// <summary>
		/// 背面のウィンドウを取得します。
		/// </summary>
		public const int GW_HWNDNEXT = 2;

		/// <summary>
		/// 前面のウィンドウを取得します。
		/// </summary>
		public const int GW_HWNDPREV = 3;

		/// <summary>
		/// オーナーウィンドウを取得します。
		/// </summary>
		public const int GW_OWNER = 4;

		/// <summary>
		/// ウィンドウスタイルを取得します。
		/// </summary>
		public const int GWL_STYLE = (-16);

		/// <summary>
		/// 拡張ウィンドウスタイルを取得します。
		/// </summary>
		public const int GWL_EXSTYLE = (-20);

		/// <summary>
		/// ウィンドウをTOPMOSTに移動します。
		/// </summary>
		public const int HWND_TOPMOST = (-1);

		/// <summary>
		/// ウィンドウを非表示にし、他のウィンドウをアクティブにします。
		/// </summary>
		public const int SW_HIDE = 0x0;

		/// <summary>
		/// ウィンドウをアクティブにして、最小化します。
		/// </summary>
		public const int SW_SHOWMINIMIZED = 0x2;

		/// <summary>
		/// ウィンドウをアクティブにして、最大化します。
		/// </summary>
		public const int SW_SHOWMAXIMIZED = 0x3;

		/// <summary>
		/// ウィンドウをアクティブにして、現在の位置とサイズで表示します。
		/// </summary>
		public const int SW_SHOW = 0x5;

		/// <summary>
		/// ウィンドウをアクティブにして表示します。最小化または最大化されていたウィンドウは、元の位置とサイズに戻ります。
		/// </summary>
		public const int SW_RESTORE = 0x9;
	}

}
