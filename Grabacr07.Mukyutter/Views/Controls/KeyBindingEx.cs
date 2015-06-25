//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;
//using System.Windows.Markup;

//namespace Grabacr07.Mukyutter.Views.Controls
//{
//	class KeyBindingEx : KeyBinding
//	{

//		//----------------------------------------------------- 
//		//
//		//  Constructors 
//		// 
//		//-----------------------------------------------------
//		#region Constructor 
//		/// <summary>
//		/// Constructor
//		/// </summary>
//		public KeyBindingEx()
//			: base() 
//		{
//		} 
 
//		/// <summary>
//		/// Constructor 
//		/// </summary>
//		/// <param name="command">Command associated</param>
//		/// <param name="gesture">KeyGesture associated</param>
//		public KeyBindingEx(ICommand command, KeyGesture gesture) : base(command, gesture) 
//		{
//			SynchronizePropertiesFromGesture(gesture); 
//		} 

//		/// <summary> 
//		/// Constructor
//		/// </summary>
//		/// <param name="command"></param>
//		/// <param name="modifiers">modifiers</param> 
//		/// <param name="key">key</param>
//		public KeyBindingEx(ICommand command, Key key, ModifierKeys modifiers) : 
//							this(command, new KeyGesture(key, modifiers)) 
//		{
//		} 
//		#endregion Constructor

//		//------------------------------------------------------
//		// 
//		//  Public Methods
//		// 
//		//----------------------------------------------------- 
//		#region Public Methods
//		/// <summary> 
//		/// KeyGesture Override, to ensure type-safety and provide a
//		///  TypeConverter for KeyGesture
//		/// </summary>
//		[TypeConverter(typeof(KeyGestureConverter))] 
//		[ValueSerializer(typeof(KeyGestureValueSerializer))]
//		public override InputGesture Gesture 
//		{ 
//			get
//			{ 
//				return base.Gesture as KeyGesture;
//			}
//			set
//			{ 
//				KeyGesture keyGesture = value as KeyGesture;
//				if (keyGesture != null) 
//				{ 
//					base.Gesture = value;
//					SynchronizePropertiesFromGesture(keyGesture); 
//				}
//				else
//				{
//					throw new ArgumentException(); 
//				}
//			} 
//		} 

//		/// <summary> 
//		///     Dependency Property for Modifiers
//		/// </summary>
//		public static readonly DependencyProperty ModifiersProperty =
//			DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(KeyBinding), new UIPropertyMetadata(ModifierKeys.None, new PropertyChangedCallback(OnModifiersPropertyChanged))); 

//		/// <summary> 
//		///     Modifiers 
//		/// </summary>
//		public ModifierKeys Modifiers 
//		{
//			get
//			{
//				return (ModifierKeys)GetValue(ModifiersProperty); 
//			}
//			set 
//			{ 
//				SetValue(ModifiersProperty, value);
//			} 
//		}

//		private static void OnModifiersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//		{
//			var keyBinding = (KeyBindingEx)d;
//			keyBinding.SynchronizeGestureFromProperties(keyBinding.Key, (ModifierKeys)(e.NewValue)); 
//		} 

//		/// <summary> 
//		///     Dependency Property for Key
//		/// </summary>
//		public static readonly DependencyProperty KeyProperty =
//			DependencyProperty.Register("Key", typeof(Key), typeof(KeyBinding), new UIPropertyMetadata(Key.None, new PropertyChangedCallback(OnKeyPropertyChanged))); 

//		/// <summary> 
//		///     Key 
//		/// </summary>
//		public Key Key 
//		{
//			get
//			{
//				return (Key)GetValue(KeyProperty); 
//			}
//			set 
//			{ 
//				SetValue(KeyProperty, value);
//			} 
//		}

//		private static void OnKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//		{
//			var keyBinding = (KeyBindingEx)d;
//			keyBinding.SynchronizeGestureFromProperties((Key)(e.NewValue), keyBinding.Modifiers); 
//		} 

//		#endregion Public Methods 

//		#region Freezable

//		protected override Freezable CreateInstanceCore() 
//		{
//			return new KeyBinding(); 
//		} 

//		#endregion 

//		#region Private Methods

//		/// <summary> 
//		///     Synchronized Properties from Gesture
//		/// </summary> 
//		private void SynchronizePropertiesFromGesture(KeyGesture keyGesture) 
//		{
//			if (!_settingGesture) 
//			{
//				_settingGesture = true;
//				try
//				{ 
//					Key = keyGesture.Key;
//					Modifiers = keyGesture.Modifiers; 
//				} 
//				finally
//				{ 
//					_settingGesture = false;
//				}
//			}
//		} 

//		/// <summary> 
//		///     Synchronized Gesture from properties 
//		/// </summary>
//		private void SynchronizeGestureFromProperties(Key key, ModifierKeys modifiers) 
//		{
//			if (!_settingGesture)
//			{
//				_settingGesture = true; 
//				try
//				{ 
//					Gesture = new KeyGesture(key, modifiers); 
//				}
//				finally 
//				{
//					_settingGesture = false;
//				}
//			} 
//		}
 
//		#endregion 
//		//------------------------------------------------------
//		// 
//		//   Private Fields
//		//
//		//------------------------------------------------------
//		#region Data 

//		private bool _settingGesture = false; 
 
//		#endregion
//	}
//}
