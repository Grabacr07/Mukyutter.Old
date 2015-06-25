using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// Microsoft OLE for Windows (ole32.dll) で定義される関数へのアクセスを行う機能を提供します。このクラスは継承できません。
	/// </summary>
	public static class Ole32
	{
		/// <summary>
		/// 指定されたクラス ID またはプログラム ID に関連付けられているクラスのオブジェクトを作成します。
		/// </summary>
		/// <param name="clsid">作成するオブジェクトの CLSID 。</param>
		/// <param name="inner">オブジェクトを集約するインタフェースポインタ。</param>
		/// <param name="context">オブジェクトが実行されるコンテキスト。</param>
		/// <param name="uuid">オブジェクトを操作するための識別子。</param>
		/// <param name="rReturnedComObject">uuid で指定したインタフェースへのポインタ。</param>
		/// <returns></returns>
		[DllImport("ole32.Dll")]
		public static extern uint CoCreateInstance(ref Guid clsid,
			[MarshalAs(UnmanagedType.IUnknown)] object inner,
			uint context,
			ref Guid uuid,
			[MarshalAs(UnmanagedType.IUnknown)] out object rReturnedComObject);

		/// <summary>
		/// Values that are used in activation calls to indicate the execution contexts in which an object is to be run.
		/// </summary>
		[Flags]
		public enum CLSCTX : uint
		{
			/// <summary>
			/// The code that creates and manages objects of this class is a DLL that runs in the same process as the caller of the function specifying the class context.
			/// </summary>
			CLSCTX_INPROC_SERVER = 0x1,

			/// <summary>
			/// The code that manages objects of this class is an in-process handler.
			/// </summary>
			CLSCTX_INPROC_HANDLER = 0x2,
			
			/// <summary>
			/// The EXE code that creates and manages objects of this class runs on same machine but is loaded in a separate process space.
			/// </summary>
			CLSCTX_LOCAL_SERVER = 0x4,

			/// <summary>
			/// Obsolete.
			/// </summary>
			CLSCTX_INPROC_SERVER16 = 0x8,

			/// <summary>
			/// A remote context.
			/// </summary>
			CLSCTX_REMOTE_SERVER = 0x10,

			/// <summary>
			/// Obsolete.
			/// </summary>
			CLSCTX_INPROC_HANDLER16 = 0x20,

			/// <summary>
			/// Reserved.
			/// </summary>
			CLSCTX_RESERVED1 = 0x40,

			/// <summary>
			/// Reserved.
			/// </summary>
			CLSCTX_RESERVED2 = 0x80,

			/// <summary>
			/// Reserved.
			/// </summary>
			CLSCTX_RESERVED3 = 0x100,

			/// <summary>
			/// Reserved.
			/// </summary>
			CLSCTX_RESERVED4 = 0x200,

			/// <summary>
			/// Disaables the downloading of code from the directory service or the Internet.
			/// </summary>
			CLSCTX_NO_CODE_DOWNLOAD = 0x400,

			/// <summary>
			/// Reserved.
			/// </summary>
			CLSCTX_RESERVED5 = 0x800,

			/// <summary>
			/// Specify if you want the activation to fail if it uses custom marshalling.
			/// </summary>
			CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,

			/// <summary>
			/// Enables the downloading of code from the directory service or the Internet.
			/// </summary>
			CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,

			/// <summary>
			/// The CLSCTX_NO_FAILURE_LOG can be used to override the logging of failures in CoCreateInstanceEx.
			/// </summary>
			CLSCTX_NO_FAILURE_LOG = 0x4000,

			/// <summary>
			/// Disables activate-as-activator (AAA) activations for this activation only.
			/// </summary>
			CLSCTX_DISABLE_AAA = 0x8000,

			/// <summary>
			/// Enables activate-as-activator (AAA) activations for this activation only.
			/// </summary>
			CLSCTX_ENABLE_AAA = 0x10000,

			/// <summary>
			/// Begin this activation from the default context of the current apartment.
			/// </summary>
			CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,

			/// <summary>
			/// Activate or connect to a 32-bit version of the server; fail if one is not registered.
			/// </summary>
			CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,

			/// <summary>
			/// Activate or connect to a 64 bit version of the server; fail if one is not registered.
			/// </summary>
			CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,

			/// <summary>
			/// When this flag is specified, COM uses the impersonation token of the thread, if one is present, for the activation request made by the thread.
			/// </summary>
			CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
		}
	}
}
