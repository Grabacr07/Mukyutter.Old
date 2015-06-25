using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Grabacr07.Utilities.Win32
{
	/// <summary>
	/// Core Audio API で定義される関数へのアクセスを行う機能を提供します。このクラスは継承できません。
	/// </summary>
	public static class CoreAudioApi
	{
		#region DEVICE_STATE

		/// <summary>
		/// The audio endpoint device is active.
		/// </summary>
		public static readonly int DEVICE_STATE_ACTIVE = 0x00000001;

		/// <summary>
		/// The audio endpoint device is disabled.
		/// </summary>
		public static readonly int DEVICE_STATE_DISABLE = 0x00000002;

		/// <summary>
		/// The audio endpoint device is not present because the audio adapter that connects to the endpoint device has been removed or disabled.
		/// </summary>
		public static readonly int DEVICE_STATE_NOTPRESENT = 0x00000004;

		/// <summary>
		/// The audio endpoint device is unplugged.
		/// </summary>
		public static readonly int DEVICE_STATE_UNPLUGGED = 0x00000008;

		/// <summary>
		/// Includes audio endpoint devices in all states.
		/// </summary>
		public static readonly int DEVICE_STATEMASK_ALL = 0x0000000f;

		#endregion

		#region STGM

		/// <summary>
		/// Indicates that the object is read-only, meaning that modifications cannot be made.
		/// </summary>
		public static readonly int STGM_READ = 0x00000000;

		/// <summary>
		/// Enables you to save changes to the object, but does not permit access to its data.
		/// </summary>
		public static readonly int STGM_WRITE = 0x00000001;

		/// <summary>
		/// Enables access and modification of object data.
		/// </summary>
		public static readonly int STGM_READWRITE = 0x00000002;

		#endregion

		/// <summary>
		/// Represents a collection of audio devices.
		/// </summary>
		[Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMMDeviceCollection
		{
			/// <summary>
			/// Retrieves a count of the devices in the device collection.
			/// </summary>
			int GetCount(ref uint pcDevices);

			/// <summary>
			/// Retrieves a pointer to the specified item in the device collection.
			/// </summary>
			int Item(uint nDevice, ref IntPtr ppDevice);
		}

		/// <summary>
		/// Provides methods for enumerating multimedia device resources.
		/// </summary>
		[Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMMDeviceEnumerator
		{
			/// <summary>
			/// Generates a collection of audio endpoint devices that meet the specified criteria.
			/// </summary>
			int EnumAudioEndpoints(EDataFlow dataFlow, int dwStateMask, ref IntPtr ppDevices);

			/// <summary>
			/// Retrieves the default audio endpoint for the specified data-flow direction and role.
			/// </summary>
			int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, ref IntPtr ppEndpoint);

			/// <summary>
			/// Retrieves an endpoint device that is specified by an endpoint device-identification string.
			/// </summary>
			int GetDevice(string pwstrId, ref IntPtr ppDevice);

			/// <summary>
			/// Registers a client's notification callback interface.
			/// </summary>
			int RegisterEndpointNotificationCallback(IntPtr pClient);

			/// <summary>
			/// Deletes the registration of a notification interface that the client registered in a previous call to the IMMDeviceEnumerator::RegisterEndpointNotificationCallback method.
			/// </summary>
			int UnregisterEndpointNotificationCallback(IntPtr pClient);
		}

		/// <summary>
		/// Encapsulates the generic features of a multimedia device resource.
		/// </summary>
		[Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMMDevice
		{
			/// <summary>
			/// Creates a COM object with the specified interface.
			/// </summary>
			int Activate(ref Guid iid, uint dwClsCtx, IntPtr pActivationParams, ref IntPtr ppInterface);

			/// <summary>
			/// Gets an interface to the device's property store.
			/// </summary>
			int OpenPropertyStore(int stgmAccess, ref IntPtr ppProperties);

			/// <summary>
			/// Gets a string that identifies the device.
			/// </summary>
			[PreserveSig]
			int GetId([Out] [MarshalAs(UnmanagedType.LPWStr)] out string ppstrId);

			/// <summary>
			/// Gets the current state of the device.
			/// </summary>
			int GetState(ref int pdwState);
		}

		/// <summary>
		/// Represents the volume controls on the audio stream to or from an audio endpoint device.
		/// </summary>
		[Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioEndpointVolume
		{
			/// <summary>
			/// Registers a client's notification callback interface.
			/// </summary>
			int RegisterControlChangeNotify(IAudioEndpointVolumeCallback pNotify);

			/// <summary>
			/// Deletes the registration of a client's notification callback interface.
			/// </summary>
			int UnregisterControlChangeNotify(IAudioEndpointVolumeCallback pNotify);
			
			/// <summary>
			/// Gets a count of the channels in the audio stream.
			/// </summary>
			int GetChannelCount(ref uint pnChannelCount);

			/// <summary>
			/// Sets the master volume level of the audio stream, in decibels.
			/// </summary>
			int SetMasterVolumeLevel(float fLevelDB, Guid pguidEventContext);

			/// <summary>
			/// Sets the master volume level, expressed as a normalized, audio-tapered value.
			/// </summary>
			int SetMasterVolumeLevelScalar(float fLevel, Guid pguidEventContext);

			/// <summary>
			/// Gets the master volume level of the audio stream, in decibels.
			/// </summary>
			int GetMasterVolumeLevel(ref float pfLevelDB);

			/// <summary>
			/// Gets the master volume level, expressed as a normalized, audio-tapered value.
			/// </summary>
			int GetMasterVolumeLevelScalar(ref float pfLevel);

			/// <summary>
			/// Sets the volume level, in decibels, of the specified channel of the audio stream.
			/// </summary>
			int SetChannelVolumeLevel(uint nChannel, float fLevelDB, Guid pguidEventContext);

			/// <summary>
			/// Sets the normalized, audio-tapered volume level of the specified channel in the audio stream.
			/// </summary>
			int SetChannelVolumeLevelScalar(uint nChannel, float fLevel, Guid pguidEventContext);

			/// <summary>
			/// Gets the volume level, in decibels, of the specified channel in the audio stream.
			/// </summary>
			int GetChannelVolumeLevel(uint nChannel, ref float pfLevelDB);

			/// <summary>
			/// Gets the normalized, audio-tapered volume level of the specified channel of the audio stream.
			/// </summary>
			int GetChannelVolumeLevelScalar(uint nChannel, ref float pfLevel);

			/// <summary>
			/// Sets the muting state of the audio stream.
			/// </summary>
			int SetMute(bool bMute, Guid pguidEventContext);

			/// <summary>
			/// Gets the muting state of the audio stream.
			/// </summary>
			int GetMute(ref bool pbMute);

			/// <summary>
			/// Gets information about the current step in the volume range.
			/// </summary>
			int GetVolumeStepInfo(ref uint pnStep, ref uint pnStepCount);

			/// <summary>
			/// Increases the volume level by one step.
			/// </summary>
			int VolumeStepUp(Guid pguidEventContext);

			/// <summary>
			/// Decreases the volume level by one step.
			/// </summary>
			int VolumeStepDown(Guid pguidEventContext);

			/// <summary>
			/// Queries the audio endpoint device for its hardware-supported functions.
			/// </summary>
			int QueryHardwareSupport(ref uint pdwHardwareSupportMask);

			/// <summary>
			/// Gets the volume range of the audio stream, in decibels.
			/// </summary>
			int GetVolumeRange(ref float pflVolumeMindB, ref float pflVolumeMaxdB, ref float pflVolumeIncrementdB);
		}

		/// <summary>
		/// Exposes methods for enumerating, getting, and setting property values.
		/// </summary>
		/// <remarks>
		/// MSDN Reference: http://msdn.microsoft.com/en-us/library/bb761474.aspx
		/// Note: This item is external to CoreAudio API, and is defined in the Windows Property System API.
		/// </remarks>
		[Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPropertyStore
		{
			/// <summary>
			/// Gets the number of properties attached to the file.
			/// </summary>
			/// <param name="propertyCount">Receives the property count.</param>
			/// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
			int GetCount(out int propertyCount);

			/// <summary>
			/// Gets a property key from an item's array of properties.
			/// </summary>
			/// <param name="propertyIndex">The index of the property key in the array of <see cref="PROPERTYKEY"/> structures.</param>
			/// <param name="propertyKey">The unique identifier for a property.</param>
			/// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
			int GetAt(int propertyIndex, out PROPERTYKEY propertyKey);

			/// <summary>
			/// Gets data for a specific property.
			/// </summary>
			/// <param name="propertyKey">A <see cref="PROPERTYKEY"/> structure containing a unique identifier for the property in question.</param>
			/// <param name="value">Receives a <see cref="PROPVARIANT"/> structure that contains the property data.</param>
			/// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
			[PreserveSig]
			int GetValue([In] ref PROPERTYKEY propertyKey, [Out] out PROPVARIANT value);

			/// <summary>
			/// Sets a new property value, or replaces or removes an existing value.
			/// </summary>
			/// <param name="propertyKey">A <see cref="PROPERTYKEY"/> structure containing a unique identifier for the property in question.</param>
			/// <param name="value">A <see cref="PROPVARIANT"/> structure that contains the new property data.</param>
			/// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
			[PreserveSig]
			int SetValue([In] ref PROPERTYKEY propertyKey, [In] ref PROPVARIANT value);

			/// <summary>
			/// Saves a property change.
			/// </summary>
			/// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
			[PreserveSig]
			int Commit();
		}

		/// <summary>
		/// Specifies the FMTID/PID identifier that programmatically identifies a property.
		/// </summary>
		/// <remarks>
		/// MSDN Reference: http://msdn.microsoft.com/en-us/library/bb773381.aspx
		/// Note: This item is external to CoreAudio API, and is defined in the Windows Property System API.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct PROPERTYKEY
		{
			/// <summary>
			/// A unique GUID for the property.
			/// </summary>
			public Guid fmtid;

			/// <summary>
			/// A property identifier (PID).
			/// </summary>
			public UIntPtr pid;
		}

		/// <summary>
		/// from Propidl.h.
		/// http://msdn.microsoft.com/en-us/library/aa380072(VS.85).aspx
		/// contains a union so we have to do an explicit layout
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		public struct PROPVARIANT
		{
			[FieldOffset(0)]
			short vt;
			[FieldOffset(2)]
			short wReserved1;
			[FieldOffset(4)]
			short wReserved2;
			[FieldOffset(6)]
			short wReserved3;
			[FieldOffset(8)]
			sbyte cVal;
			[FieldOffset(8)]
			byte bVal;
			[FieldOffset(8)]
			short iVal;
			[FieldOffset(8)]
			ushort uiVal;
			[FieldOffset(8)]
			int lVal;
			[FieldOffset(8)]
			uint ulVal;
			[FieldOffset(8)]
			int intVal;
			[FieldOffset(8)]
			uint uintVal;
			[FieldOffset(8)]
			long hVal;
			[FieldOffset(8)]
			long uhVal;
			[FieldOffset(8)]
			float fltVal;
			[FieldOffset(8)]
			double dblVal;
			[FieldOffset(8)]
			bool boolVal;
			[FieldOffset(8)]
			int scode;
			//CY cyVal;
			[FieldOffset(8)]
			DateTime date;
			[FieldOffset(8)]
			System.Runtime.InteropServices.ComTypes.FILETIME filetime;
			//CLSID* puuid;
			//CLIPDATA* pclipdata;
			//BSTR bstrVal;
			//BSTRBLOB bstrblobVal;
			[FieldOffset(8)]
			Blob blobVal;			
			//LPSTR pszVal;
			[FieldOffset(8)]
			IntPtr pwszVal; //LPWSTR 
			//IUnknown* punkVal;
			/*IDispatch* pdispVal;
			IStream* pStream;
			IStorage* pStorage;
			LPVERSIONEDSTREAM pVersionedStream;
			LPSAFEARRAY parray;
			CAC cac;
			CAUB caub;
			CAI cai;
			CAUI caui;
			CAL cal;
			CAUL caul;
			CAH cah;
			CAUH cauh;
			CAFLT caflt;
			CADBL cadbl;
			CABOOL cabool;
			CASCODE cascode;
			CACY cacy;
			CADATE cadate;
			CAFILETIME cafiletime;
			CACLSID cauuid;
			CACLIPDATA caclipdata;
			CABSTR cabstr;
			CABSTRBLOB cabstrblob;
			CALPSTR calpstr;
			CALPWSTR calpwstr;
			CAPROPVARIANT capropvar;
			CHAR* pcVal;
			UCHAR* pbVal;
			SHORT* piVal;
			USHORT* puiVal;
			LONG* plVal;
			ULONG* pulVal;
			INT* pintVal;
			UINT* puintVal;
			FLOAT* pfltVal;
			DOUBLE* pdblVal;
			VARIANT_BOOL* pboolVal;
			DECIMAL* pdecVal;
			SCODE* pscode;
			CY* pcyVal;
			DATE* pdate;
			BSTR* pbstrVal;
			IUnknown** ppunkVal;
			IDispatch** ppdispVal;
			LPSAFEARRAY* pparray;
			PROPVARIANT* pvarVal;
			*/

			/// <summary>
			/// Helper method to gets blob data
			/// </summary>
			byte[] GetBlob()
			{
				byte[] Result = new byte[blobVal.Length];
				Marshal.Copy(blobVal.Data, Result, 0, Result.Length);
				return Result;
			}

			/// <summary>
			/// Property value
			/// </summary>
			public object Value
			{
				get
				{
					VarEnum ve = (VarEnum)vt;
					switch (ve)
					{
						case VarEnum.VT_I1:
							return bVal;
						case VarEnum.VT_I2:
							return iVal;
						case VarEnum.VT_I4:
							return lVal;
						case VarEnum.VT_I8:
							return hVal;
						case VarEnum.VT_INT:
							return iVal;
						case VarEnum.VT_UI4:
							return ulVal;
						case VarEnum.VT_LPWSTR:
							return Marshal.PtrToStringUni(pwszVal);
						case VarEnum.VT_BLOB:
							return GetBlob();
					}
					throw new NotImplementedException("PropVariant " + ve.ToString());
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		internal struct Blob
		{
			public int Length;
			public IntPtr Data;

			//Code Should Compile at warning level4 without any warnings, 
			//However this struct will give us Warning CS0649: Field [Fieldname] 
			//is never assigned to, and will always have its default value
			//You can disable CS0649 in the project options but that will disable
			//the warning for the whole project, it's a nice warning and we do want 
			//it in other places so we make a nice dummy function to keep the compiler
			//happy.
			private void FixCS0649()
			{
				Length = 0;
				Data = IntPtr.Zero;
			}
		}

		/// <summary>
		/// Provides notifications of changes in the volume level and muting state of an audio endpoint device.
		/// </summary>
		[Guid("657804FA-D6AD-4496-8A60-352752AF4F89"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAudioEndpointVolumeCallback
		{
			/// <summary>
			/// Notifies the client that the volume level or muting state of the audio endpoint device has changed.
			/// </summary>
			[PreserveSig]
			int OnNotify(IntPtr pNotifyData);
		}

		/// <summary>
		/// Defines constants that indicate the direction in which audio data flows between an audio endpoint device and an application.
		/// </summary>
		public enum EDataFlow
		{
			/// <summary>
			/// Audio rendering stream. Audio data flows from the application to the audio endpoint device, which renders the stream.
			/// </summary>
			eRender,

			/// <summary>
			/// Audio capture stream. Audio data flows from the audio endpoint device that captures the stream, to the application.
			/// </summary>
			eCapture,

			/// <summary>
			/// Audio rendering or capture stream. Audio data can flow either from the application to the audio endpoint device, or from the audio endpoint device to the application.
			/// </summary>
			eAll,

			/// <summary>
			/// The number of members in the EDataFlow enumeration (not counting the EDataFlow_enum_count member).
			/// </summary>
			EDataFlow_enum_count
		}

		/// <summary>
		/// Defines constants that indicate the role that the system has assigned to an audio endpoint device.
		/// </summary>
		public enum ERole
		{
			/// <summary>
			/// Games, system notification sounds, and voice commands.
			/// </summary>
			eConsole,

			/// <summary>
			/// Music, movies, narration, and live music recording.
			/// </summary>
			eMultimedia,

			/// <summary>
			/// Voice communications (talking to another person).
			/// </summary>
			eCommunications,

			/// <summary>
			/// The number of members in the ERole enumeration (not counting the ERole_enum_count member).
			/// </summary>
			ERole_enum_count
		}

		/// <summary>
		/// Describes a change in the volume level or muting state of an audio endpoint device.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct AUDIO_VOLUME_NOTIFICATION_DATA
		{
			/// <summary>
			/// Context value for the IAudioEndpointVolumeCallback::OnNotify method.
			/// </summary>
			public Guid guidEventContext;

			/// <summary>
			/// Specifies whether the audio stream is currently muted.
			/// </summary>
			public bool bMuted;

			/// <summary>
			/// Specifies the current master volume level of the audio stream.
			/// </summary>
			public float fMasterVolume;

			/// <summary>
			/// Specifies the number of channels in the audio stream, which is also the number of elements in the afChannelVolumes array.
			/// </summary>
			public uint nChannels;

			/// <summary>
			/// The first element in an array of channel volumes.
			/// </summary>
			public float afChannelVolumes;
		}
	}
}
