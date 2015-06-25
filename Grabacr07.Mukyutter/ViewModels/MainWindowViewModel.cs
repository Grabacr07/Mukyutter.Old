using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Settings;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.ViewModels.Composing;
using Grabacr07.Mukyutter.ViewModels.Tabs;
using Grabacr07.Mukyutter.ViewModels.Tabs.TimelineTabs;
using Grabacr07.Mukyutter.ViewModels.Twitter;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.Windows;

namespace Grabacr07.Mukyutter.ViewModels
{
	internal class MainWindowViewModel : WindowViewModel
	{
		#region Mode 変更通知プロパティ

		private MainWindowMode _Mode = MainWindowMode.NormalWindow;

		public MainWindowMode Mode
		{
			get { return this._Mode; }
			private set
			{
				if (this._Mode != value)
				{
					this._Mode = value;
					if (value == MainWindowMode.NormalWindow)
					{
						this.Messenger.Raise(new TransitionMessage(this, TransitionMode.Normal, "Open/NormalWindow"));
						this.Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close/MinimumWindow"));
					}
					else if (value == MainWindowMode.MinimumWindow)
					{
						this.Messenger.Raise(new TransitionMessage(this, TransitionMode.Normal, "Open/MinimumWindow"));
						this.Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close/NormalWindow"));
					}
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region NotificationMessage 変更通知プロパティ

		private string _NotificationMessage;

		public string NotificationMessage
		{
			get { return this._NotificationMessage; }
			set
			{
				if (this._NotificationMessage != value)
				{
					this._NotificationMessage = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsNotificationOverlay 変更通知プロパティ

		private bool _IsNotificationOverlay;

		public bool IsNotificationOverlay
		{
			get { return this._IsNotificationOverlay; }
			set
			{
				if (this._IsNotificationOverlay != value)
				{
					this._IsNotificationOverlay = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public ComposerViewModel Composer { get; private set; }

		public ReadOnlyDispatcherCollection<TabViewModel> TabItems { get; private set; }
		public DispatcherCollection<TabViewModel> SysTabItems { get; private set; }

		#region SelectedTabItem 変更通知プロパティ

		private TabViewModel _SelectedTabItem;

		public TabViewModel SelectedTabItem
		{
			get { return this._SelectedTabItem; }
			set
			{
				if (this._SelectedTabItem != value)
				{
					this._SelectedTabItem = value;
					MukyutterClient.Current.CurrentBlock = value.Block;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		public MainWindowViewModel() : this(new MainWindowSettings()) { }

		public MainWindowViewModel(MainWindowSettings settings)
			: base(settings)
		{
			this.Title = "Mukyutter; Silent Selene (beta)";

			if (!this.IsInDesignMode)
			{
				this.CompositeDisposable.Add(new PropertyChangedEventListener(MukyutterClient.Current.NotificationService)
				{
					{ "NotificationMessage", (sender, e) => this.Notify(MukyutterClient.Current.NotificationService.NotificationMessage) },
				});

				this.Composer = new ComposerViewModel(MukyutterClient.Current.Composer);

				this.TabItems = ViewModelHelper.CreateReadOnlyDispatcherCollection(
					MukyutterClient.Current.BlockItems,
					tab => tab.ToTabViewModel(),
					DispatcherHelper.UIDispatcher);
				this.CompositeDisposable.Add(this.TabItems);

				this.SysTabItems = new DispatcherCollection<TabViewModel>(DispatcherHelper.UIDispatcher)
				{
					new SystemTabViewModel(),
					new EventTabViewModel(),
					#region DEBUG only
#if DEBUG
					new DevTabViewModel(),
#endif
					#endregion
				};

				this.SelectedTabItem = this.TabItems.FirstOrDefault();
				this.IsNotificationOverlay = true;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
		}


		public void AddTab()
		{
			var block = new TimelineBlock { Name = "新しいタブ" };
			MukyutterClient.Current.BlockItems.Add(block);
			var viewmodel = this.TabItems.FirstOrDefault(t => t.Block == block) as TimelineTabViewModel;
			if (viewmodel != null)
			{
				viewmodel.IsConfigMode = true;
				this.SelectedTabItem = viewmodel;
			}
		}

		public void NormalMode()
		{
			this.Mode = MainWindowMode.NormalWindow;
		}

		public void MinimumMode()
		{
			this.Mode = MainWindowMode.MinimumWindow;
		}

		private void Notify(string message)
		{
			this.NotificationMessage = message;
		}


		public new MainWindowSettings ToSettings()
		{
			var settings = this.ToSettings<MainWindowSettings>();
			return settings;
		}
	}
}
