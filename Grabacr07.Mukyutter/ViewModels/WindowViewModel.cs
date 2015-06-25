using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Grabacr07.Mukyutter.Models.Settings;
using Grabacr07.Utilities.Development;
using Livet;
using Livet.Messaging;
using Livet.Messaging.Windows;

namespace Grabacr07.Mukyutter.ViewModels
{
	class WindowViewModel : ViewModelBase
	{

		#region Title 変更通知プロパティ

		private string _Title = "Window";

		public string Title
		{
			get { return this._Title; }
			set
			{
				if (this._Title != value)
				{
					this._Title = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region WindowState 変更通知プロパティ

		private WindowState _WindowState;

		public virtual WindowState WindowState
		{
			get { return this._WindowState; }
			set
			{
				if (this._WindowState != value)
				{
					this._WindowState = value;
					this.IsMaximized = value == WindowState.Maximized;
					this.CanNormalize = value == WindowState.Maximized;
					this.CanMaximize = value == WindowState.Normal;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsMaximized 変更通知プロパティ

		private bool _IsMaximized;

		public bool IsMaximized
		{
			get { return this._IsMaximized; }
			set
			{
				if (this._IsMaximized != value)
				{
					this._IsMaximized = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Left 変更通知プロパティ

		private double _Left;

		public double Left
		{
			get { return this._Left; }
			set
			{
				if (this._Left != value)
				{
					this._Left = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Top 変更通知プロパティ

		private double _Top;

		public double Top
		{
			get { return this._Top; }
			set
			{
				if (this._Top != value)
				{
					this._Top = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Width 変更通知プロパティ

		private double _Width;

		public double Width
		{
			get { return this._Width; }
			set
			{
				if (this._Width != value)
				{
					this._Width = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Height 変更通知プロパティ

		private double _Height;

		public double Height
		{
			get { return this._Height; }
			set
			{
				if (this._Height != value)
				{
					this._Height = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsActive 変更通知プロパティ

		private bool _IsActive;

		public bool IsActive
		{
			get { return this._IsActive; }
			set
			{
				if (this._IsActive != value)
				{
					this._IsActive = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CanMaximize 変更通知プロパティ

		private bool _CanMaximize = true;

		public bool CanMaximize
		{
			get { return this._CanMaximize; }
			set
			{
				if (this._CanMaximize != value)
				{
					this._CanMaximize = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CanMinimize 変更通知プロパティ

		private bool _CanMinimize = true;

		public bool CanMinimize
		{
			get { return this._CanMinimize; }
			set
			{
				if (this._CanMinimize != value)
				{
					this._CanMinimize = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CanNormalize 変更通知プロパティ

		private bool _CanNormalize = false;

		public bool CanNormalize
		{
			get { return this._CanNormalize; }
			set
			{
				if (this._CanNormalize != value)
				{
					this._CanNormalize = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		#region Opacity 変更通知プロパティ

		private double _Opacity;

		public double Opacity
		{
			get { return this._Opacity; }
			set
			{
				if (this._Opacity != value)
				{
					this._Opacity = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("OpacityPercent");
				}
			}
		}

		public int OpacityPercent
		{
			get { return (int)(this.Opacity * 100); }
		}

		#endregion

		#region Overlay 変更通知プロパティ

		private ImageSource _Overlay;

		public ImageSource Overlay
		{
			get { return this._Overlay; }
			set
			{
				if (this._Overlay != value)
				{
					this._Overlay = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
		


		protected WindowViewModel()
			: this(new WindowSettings()) { }

		protected WindowViewModel(WindowSettings settings)
		{
			this.WindowState = settings.State;
			this.Left = settings.Location.X;
			this.Top = settings.Location.Y;
			this.Width = settings.Size.Width;
			this.Height = settings.Size.Height;
			this.Opacity = settings.Opacity;
		}


		public void Activate()
		{
			this.Messenger.Raise(new WindowActionMessage(WindowAction.Active, "WindowAction"));
		}


		public virtual void Initialize()
		{
			this.Messenger.Raise(new InteractionMessage("UpdateToSource/IsActive"));
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}


		public virtual WindowSettings ToSettings()
		{
			return this.ToSettings<WindowSettings>();
		}

		public TSettings ToSettings<TSettings>() where TSettings : WindowSettings, new()
		{
			return new TSettings()
			{
				State = WindowState,
				Location = new Point(Left, Top),
				Size = new Size(Width, Height),
			};
		}
	}
}
