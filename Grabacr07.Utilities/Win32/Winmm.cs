using System;
using System.Runtime.InteropServices;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// Windows Multimedia API (winmm.dll) で定義される関数へのアクセスを行う機能を提供します。このクラスは継承できません。
	/// </summary>
	public static class Winmm
	{
		/// <summary>
		/// システムに存在するミキサーデバイスの数を取得します。
		/// </summary>
		/// <returns>
		/// 関数が成功すると、ミキサーデバイス数が返ります。
		/// 利用可能なミキサーデバイスがない場合は 0 が返ります。
		/// </returns>
		[DllImport("winmm.dll")]
		public extern static uint mixerGetNumDevs();

		/// <summary>
		/// 指定されたミキサーデバイスをオープンし、アプリケーションがハンドルをクローズするまでデバイスが削除されないようにします。
		/// </summary>
		/// <param name="phmx">
		/// オープンしたミキサーデバイスを識別するハンドルを受け取る、変数のアドレスを指定します。
		/// このハンドルは、ほかのオーディオミキサー関数を呼び出すときに、デバイスを識別するために使います。
		/// このパラメータを NULL にすることはできません。
		/// </param>
		/// <param name="uMxId">
		/// オープンするミキサーデバイスの識別子を指定します。
		/// 有効なデバイス識別子、またはいずれかの HMIXEROBJ 構造体を使います(ミキサーオブジェクトハンドルの説明は、mixerGetID 関数を参照してください）。
		/// オーディオミキサーデバイスの“マッパー”は現在存在しないため、ミキサーデバイス識別子に -1 を指定すると無効になります。
		/// </param>
		/// <param name="dwCallback">
		/// オープンするデバイスと関連付けられたオーディオラインまたはコントロール、あるいはその両方の状態が変化するときに呼び出されるウィンドウのハンドルを指定します。
		/// コールバック機構を使用しない場合は、このパラメータに 0 を指定します。
		/// </param>
		/// <param name="dwInstance">
		/// コールバック関数に渡されるユーザーインスタンスデータが入ります。
		/// このパラメータはウィンドウコールバック関数では使われません。
		/// </param>
		/// <param name="fdwOpen">
		/// デバイスをオープンするためのフラグを指定します。
		/// </param>
		/// <returns>関数が成功すると、MMSYSERR_NOERROR が返ります。</returns>
		[DllImport("winmm.dll")]
		public static extern uint mixerOpen(ref uint phmx, uint uMxId, uint dwCallback, uint dwInstance, uint fdwOpen);

		/// <summary>
		/// 指定されたミキサーデバイスを照会して、ミキサーデバイスの性能を調べます。
		/// </summary>
		/// <param name="uMxID">
		/// オープンしているミキサーデバイスの識別子またはハンドルを指定します。
		/// </param>
		/// <param name="pmxcaps">
		/// デバイスの性能に関する情報を受け取る MIXERCAPS 構造体のアドレスを指定します。
		/// </param>
		/// <param name="cbmxcaps">
		/// MIXERCAPS 構造体のサイズをバイト単位で指定します。
		/// </param>
		/// <returns>
		/// 関数が成功すると、MMSYSERR_NOERROR が返ります。
		/// </returns>
		[DllImport("winmm.dll")]
		public static extern uint mixerGetDevCaps(uint uMxID, ref MIXERCAPS pmxcaps, uint cbmxcaps);

		/// <summary>
		/// 指定されたミキサーデバイスをクローズします。
		/// </summary>
		/// <param name="hmx">
		/// ミキサーデバイスのハンドルを指定します。このハンドルは、成功した mixerOpen 関数から返されたものでなければなりません。
		/// mixerClose 関数が成功すると、hmx は無効になります。
		/// </param>
		/// <returns>
		/// 関数が成功すると、MMSYSERR_NOERROR が返ります。
		/// </returns>
		[DllImport("winmm.dll")]
		public static extern uint mixerClose(uint hmx);

		/// <summary>
		/// ミキサーデバイスの特定のラインに関する情報を取得します。
		/// </summary>
		/// <param name="hmxobj">
		/// 特定のオーディオラインを制御するミキサーデバイスオブジェクトのハンドルを指定します。
		/// </param>
		/// <param name="pmxl">
		/// MIXERLINE 構造体のアドレスを指定します。
		/// この構造体には、ミキサーデバイスのオーディオラインに関する情報が入ります。
		/// cbStruct メンバは、常に MIXERLINE 構造体のサイズ(バイト単位）に初期化する必要があります。
		/// </param>
		/// <param name="fdwInfo">
		/// オーディオラインに関する情報を取得するためのフラグを指定します。
		/// </param>
		/// <returns>
		/// 関数が成功すると、MMSYSERR_NOERROR が返ります。
		/// </returns>
		[DllImport("winmm.dll")]
		public extern static uint mixerGetLineInfo(uint hmxobj, ref MIXERLINE pmxl, uint fdwInfo);

		/// <summary>
		/// オーディオラインと関連付けられている 1 つ以上のコントロールを取得します。
		/// </summary>
		/// <param name="hmxobj">
		/// 照会するミキサーデバイスオブジェクトのハンドルを指定します。
		/// </param>
		/// <param name="pmxlc">
		/// MIXERLINECONTROLS 構造体のアドレスを指定します。
		/// この構造体は、オーディオラインと関連付けられたコントロールに関する情報を格納する、1 つ以上の MIXERCONTROL 構造体を参照するために使われます。
		/// MIXERLINECONTROLS 構造体の cbStruct メンバは、常に MIXERLINECONTROLS 構造体のサイズ(バイト単位）に初期化してください。
		/// </param>
		/// <param name="fdwControls">
		/// オーディオラインと関連付けられた、1 つ以上のコントロールに関する情報を取得するためのフラグを指定します。
		/// </param>
		/// <returns>
		/// 関数が成功すると、MMSYSERR_NOERROR が返ります。
		/// </returns>
		[DllImport("winmm.dll")]
		public extern static uint mixerGetLineControls(uint hmxobj, ref MIXERLINECONTROLS pmxlc, uint fdwControls);

		/// <summary>
		/// オーディオラインと関連付けられた、1 つのコントロールに関する詳細を取得します。
		/// </summary>
		/// <param name="hmxobj">
		/// 照会するミキサーデバイスオブジェクトのハンドルを指定します。 
		/// </param>
		/// <param name="pmxcd">
		/// MIXERCONTROLDETAILS 構造体のアドレスを指定します。
		/// この構造体には、コントロールに関する状態情報が入ります。</param>
		/// <param name="fdwDetails">
		/// コントロールの詳細を取得するためのフラグを指定します。
		/// </param>
		/// <returns>
		/// 関数が成功すると、MMSYSERR_NOERROR が返ります。
		/// </returns>
		[DllImport("winmm.dll")]
		public static extern uint mixerGetControlDetails(uint hmxobj, ref MIXERCONTROLDETAILS pmxcd, uint fdwDetails);

		/// <summary>
		/// Windows Multimedia APIで使用する定数。
		/// </summary>
		public const uint MIXER_OBJECTF_MIXER = 0x00000000U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint CALLBACK_WINDOW = 0x00010000U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERLINE_COMPONENTTYPE_DST_SPEAKERS = 0x00000004U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXER_OBJECTF_HANDLE = 0x80000000U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXER_OBJECTF_HMIXER = MIXER_OBJECTF_HANDLE | MIXER_OBJECTF_MIXER;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXER_GETLINEINFOF_COMPONENTTYPE = 0x00000003U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXER_GETLINECONTROLSF_ONEBYTYPE = 0x00000002U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERCONTROL_CT_CLASS_SWITCH = 0x20000000U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERCONTROL_CT_SC_SWITCH_BOOLEAN = 0x00000000U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERCONTROL_CT_UNITS_BOOLEAN = 0x00010000U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERCONTROL_CONTROLTYPE_BOOLEAN = (MIXERCONTROL_CT_CLASS_SWITCH | MIXERCONTROL_CT_SC_SWITCH_BOOLEAN | MIXERCONTROL_CT_UNITS_BOOLEAN);

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERCONTROL_CONTROLTYPE_MUTE = (MIXERCONTROL_CONTROLTYPE_BOOLEAN + 2);

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERCONTROL_CT_CLASS_FADER = 0x50000000U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERCONTROL_CT_UNITS_UNSIGNED = 0x00030000U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERCONTROL_CONTROLTYPE_FADER = (MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED);

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXERCONTROL_CONTROLTYPE_VOLUME = (MIXERCONTROL_CONTROLTYPE_FADER + 1);

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MIXER_GETCONTROLDETAILSF_VALUE = 0x00000000U;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MMSYSERR_NOERROR = 0;

		/// <summary>
		/// Windows Multimedia API で使用する定数。
		/// </summary>
		public const uint MM_MIXM_CONTROL_CHANGE = 0x3D1;

		/// <summary>
		/// 製品名を格納する最大文字数。
		/// </summary>
		private const uint MAXPNAMELEN = 32;

		/// <summary>
		/// Describes the capabilities of a mixer device.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct MIXERCAPS
		{
			/// <summary>
			/// A manufacturer identifier for the mixer device driver.
			/// </summary>
			public ushort wMid;

			/// <summary>
			/// A product identifier for the mixer device driver.
			/// </summary>
			public ushort wPid;

			/// <summary>
			/// Version number of the mixer device driver.
			/// </summary>
			public uint vDriverVersion;

			/// <summary>
			/// Name of the product.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)MAXPNAMELEN)]
			public string szPname;

			/// <summary>
			/// Various support information for the mixer device driver.
			/// </summary>
			public uint fdwSupport;

			/// <summary>
			/// The number of audio line destinations available through the mixer device.
			/// </summary>
			public uint cDestinations;
		}

		/// <summary>
		/// ソースライン名（ショート）の制限文字数
		/// </summary>
		private const uint MIXER_SHORT_NAME_CHARS = 16U;

		/// <summary>
		/// ソースライン名（ロング）の制限文字数
		/// </summary>
		private const uint MIXER_LONG_NAME_CHARS = 64U;

		/// <summary>
		/// Describes the state and metrics of an audio line.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct MIXERLINE
		{
			/// <summary>
			/// Size, in bytes, of the MIXERLINE structure.
			/// </summary>
			public uint cbStruct;

			/// <summary>
			/// Destination line index.
			/// </summary>
			public uint dwDestination;

			/// <summary>
			/// Index for the audio source line associated with the dwDestination member.
			/// </summary>
			public int dwSource;
			
			/// <summary>
			/// An identifier defined by the mixer device that uniquely refers to the audio line described by the MIXERLINE structure.
			/// </summary>
			public uint dwLineID;

			/// <summary>
			/// Status and support flags for the audio line.
			/// </summary>
			public uint fdwLine;

			/// <summary>
			/// Instance data defined by the audio device for the line.
			/// </summary>
			public uint dwUser;

			/// <summary>
			/// Component type for this audio line.
			/// </summary>
			public uint dwComponentType;

			/// <summary>
			/// Maximum number of separate channels that can be manipulated independently for the audio line.
			/// </summary>
			public uint cChannels;

			/// <summary>
			/// Number of connections that are associated with the audio line.
			/// </summary>
			public uint cConnections;

			/// <summary>
			/// Number of controls associated with the audio line.
			/// </summary>
			public uint cControls;

			/// <summary>
			/// Short string that describes the audio mixer line specified in the dwLineID member.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)MIXER_SHORT_NAME_CHARS)]
			public string szShortName;

			/// <summary>
			/// String that describes the audio mixer line specified in the dwLineID member.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)MIXER_LONG_NAME_CHARS)]
			public string szName;

			/// <summary>
			/// Target media device type associated with the audio line described in the MIXERLINE structure.
			/// </summary>
			public uint Target_dwType;

			/// <summary>
			/// Current device identifier of the target media device when the dwType member is a target type other than MIXERLINE_TARGETTYPE_UNDEFINED.
			/// </summary>
			public uint Target_dwDeviceID;

			/// <summary>
			/// Manufacturer identifier of the target media device when the dwType member is a target type other than MIXERLINE_TARGETTYPE_UNDEFINED.
			/// </summary>
			public ushort Target_wMid;

			/// <summary>
			/// Product identifier of the target media device when the dwType member is a target type other than MIXERLINE_TARGETTYPE_UNDEFINED.
			/// </summary>
			public ushort Target_wPid;

			/// <summary>
			/// Driver version of the target media device when the dwType member is a target type other than MIXERLINE_TARGETTYPE_UNDEFINED.
			/// </summary>
			public uint Target_vDriverVersion;

			/// <summary>
			/// Product name of the target media device when the dwType member is a target type other than MIXERLINE_TARGETTYPE_UNDEFINED.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)MAXPNAMELEN)]
			public string Target_szPname;
		}

		/// <summary>
		/// Contains information about the controls of an audio line.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		public struct MIXERLINECONTROLS
		{
			/// <summary>
			/// Size, in bytes, of the MIXERLINECONTROLS structure.
			/// </summary>
			[FieldOffset(0)]
			public uint cbStruct;

			/// <summary>
			/// Line identifier for which controls are being queried.
			/// </summary>
			[FieldOffset(4)]
			public uint dwLineID;

			/// <summary>
			/// Control identifier of the desired control.
			/// </summary>
			[FieldOffset(8)]
			public uint dwControlID;

			/// <summary>
			/// Class of the desired Control Types.
			/// </summary>
			[FieldOffset(8)]
			public uint dwControlType;

			/// <summary>
			/// Number of MIXERCONTROL structure elements to retrieve.
			/// </summary>
			[FieldOffset(12)]
			public uint cControls;

			/// <summary>
			/// Size, in bytes, of a single MIXERCONTROL structure.
			/// </summary>
			[FieldOffset(16)]
			public uint cbmxctrl;

			/// <summary>
			/// Pointer to one or more MIXERCONTROL structures to receive the properties of the requested audio line controls.
			/// </summary>
			[FieldOffset(20)]
			public IntPtr pamxctrl;
		}

		/// <summary>
		/// Describes the state and metrics of a single control for an audio line.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		public struct MIXERCONTROL
		{
			/// <summary>
			/// Size, in bytes, of the MIXERCONTROL structure.
			/// </summary>
			[FieldOffset(0)]
			public uint cbStruct;

			/// <summary>
			/// Audio mixer-defined identifier that uniquely refers to the control described by the MIXERCONTROL structure.
			/// </summary>
			[FieldOffset(4)]
			public uint dwControlID;

			/// <summary>
			/// Class of the control for which the identifier is specified in dwControlID.
			/// </summary>
			[FieldOffset(8)]
			public uint dwControlType;

			/// <summary>
			/// Status and support flags for the audio line control.
			/// </summary>
			[FieldOffset(12)]
			public uint fdwControl;

			/// <summary>
			/// Number of items per channel that make up a MIXERCONTROL_CONTROLF_MULTIPLE control.
			/// </summary>
			[FieldOffset(16)]
			public uint cMultipleItems;

			/// <summary>
			/// Short string that describes the audio line control specified by dwControlID.
			/// </summary>
			[FieldOffset(20)]
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)MIXER_SHORT_NAME_CHARS)]
			public string szShortName;

			/// <summary>
			/// String that describes the audio line control specified by dwControlID.
			/// </summary>
			[FieldOffset(36)]
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)MIXER_LONG_NAME_CHARS)]
			public string szName;

			/// <summary>
			/// Minimum signed value for a control that has a signed boundary nature.
			/// </summary>
			[FieldOffset(100)]
			public int Bounds_lMinimum;

			/// <summary>
			/// Maximum signed value for a control that has a signed boundary nature.
			/// </summary>
			[FieldOffset(104)]
			public int Bounds_lMaximum;

			/// <summary>
			/// Minimum unsigned value for a control that has an unsigned boundary nature.
			/// </summary>
			[FieldOffset(100)]
			public uint Bounds_dwMinimum;

			/// <summary>
			/// Maximum unsigned value for a control that has an unsigned boundary nature.
			/// </summary>
			[FieldOffset(104)]
			public uint Bounds_dwMaximum;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(100)]
			public uint Bounds_dwReserved_1;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(104)]
			public uint Bounds_dwReserved_2;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(108)]
			public uint Bounds_dwReserved_3;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(112)]
			public uint Bounds_dwReserved_4;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(116)]
			public uint Bounds_dwReserved_5;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(120)]
			public uint Bounds_dwReserved_6;

			/// <summary>
			/// Number of discrete ranges within the union specified for a control specified by the Bounds member.
			/// </summary>
			[FieldOffset(124)]
			public uint Metrics_cSteps;

			/// <summary>
			/// Size, in bytes, required to contain the state of a custom control class.
			/// </summary>
			[FieldOffset(124)]
			public uint Metrics_cbCustomData;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(124)]
			public uint Metrics_dwReserved_1;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(128)]
			public uint Metrics_dwReserved_2;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(132)]
			public uint Metrics_dwReserved_3;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(136)]
			public uint Metrics_dwReserved_4;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(140)]
			public uint Metrics_dwReserved_5;

			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			[FieldOffset(144)]
			public uint Metrics_dwReserved_6;
		}

		/// <summary>
		/// Refers to control-detail structures, retrieving or setting state information of an audio mixer control.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		public struct MIXERCONTROLDETAILS {

			/// <summary>
			/// Size, in bytes, of the MIXERCONTROLDETAILS structure.
			/// </summary>
			[FieldOffset(0)]
			public uint cbStruct;

			/// <summary>
			/// Control identifier on which to get or set properties.
			/// </summary>
			[FieldOffset(4)]
			public uint dwControlID;

			/// <summary>
			/// Number of channels on which to get or set control properties.
			/// </summary>
			[FieldOffset(8)]
			public uint cChannels;

			/// <summary>
			/// Handle to the window that owns a custom dialog box for a mixer control.
			/// </summary>
			[FieldOffset(12)]
			public IntPtr hwndOwner;

			/// <summary>
			/// Number of multiple items per channel on which to get or set properties.
			/// </summary>
			[FieldOffset(16)]
			public uint cMultipleItems;

			/// <summary>
			/// Size, in bytes, of one of the following details structures being used.
			/// </summary>
			[FieldOffset(16)]
			public uint cbDetails;

			/// <summary>
			/// Pointer to an array of one or more structures in which properties for the specified control are retrieved or set.
			/// </summary>
			[FieldOffset(20)]
			public IntPtr paDetails;
		}

		/// <summary>
		/// Retrieves and sets Boolean control properties for an audio mixer control.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct MIXERCONTROLDETAILS_BOOLEAN
		{
			/// <summary>
			/// Boolean value for a single item or channel.
			/// </summary>
			public uint fValue;
		}

		/// <summary>
		/// Retrieves and sets unsigned type control properties for an audio mixer control. 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct MIXERCONTROLDETAILS_UNSIGNED
		{
			/// <summary>
			/// Unsigned integer value for a single item or channel.
			/// </summary>
			public uint dwValue;
		}
	}
}
