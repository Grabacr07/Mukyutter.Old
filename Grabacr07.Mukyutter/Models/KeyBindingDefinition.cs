using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Livet;

namespace Grabacr07.Mukyutter.Models
{
	public class KeyBindingDefinition : NotificationObject
	{
		#region Key 変更通知プロパティ

		private Key _Key;

		public Key Key
		{
			get { return this._Key; }
			set
			{
				if (this._Key != value)
				{
					this._Key = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Modifier 変更通知プロパティ

		private ModifierKeys _Modifier;

		public ModifierKeys Modifier
		{
			get { return this._Modifier; }
			set
			{
				if (this._Modifier != value)
				{
					this._Modifier = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region HasSecondKey 変更通知プロパティ

		private bool _HasSecondKey;

		public bool HasSecondKey
		{
			get { return this._HasSecondKey; }
			set
			{
				if (this._HasSecondKey != value)
				{
					this._HasSecondKey = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region SecondKey 変更通知プロパティ

		private Key _SecondKey;

		public Key SecondKey
		{
			get { return this._SecondKey; }
			set
			{
				if (this._SecondKey != value)
				{
					this._SecondKey = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region SecondModifier 変更通知プロパティ

		private ModifierKeys _SecondModifier;

		public ModifierKeys SecondModifier
		{
			get { return this._SecondModifier; }
			set
			{
				if (this._SecondModifier != value)
				{
					this._SecondModifier = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		internal static Dictionary<string, KeyBindingDefinition> DefaultTable
		{
			get
			{
				var table = new Dictionary<string, KeyBindingDefinition>
				{
					{ "FocusNewStatusBox", new KeyBindingDefinition { Key = Key.I, Modifier = ModifierKeys.Control, } },
					{ "UpdateStatus", new KeyBindingDefinition { Key = Key.Enter, Modifier = ModifierKeys.Control, } },
					{ "ClearStatus", new KeyBindingDefinition { Key = Key.Escape, } },
					{ "OpenMedia", new KeyBindingDefinition { Key = Key.O, Modifier = ModifierKeys.Control, } },
				};
				return table;
			}
		}
	}
}
