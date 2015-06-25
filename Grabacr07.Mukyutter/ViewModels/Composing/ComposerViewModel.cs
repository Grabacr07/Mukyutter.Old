using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter.Composing;
using Grabacr07.Mukyutter.ViewModels.Messaging;
using Grabacr07.Utilities;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using NormalState = Grabacr07.Mukyutter.Models.Twitter.Composing.Normal;

namespace Grabacr07.Mukyutter.ViewModels.Composing
{
	internal class ComposerViewModel : ViewModelBase
	{
		public Composer Composer { get; private set; }
		public ReadOnlyDispatcherCollection<NewStatusViewModel> NewStatuses { get; private set; }

		#region CurrentStatus 変更通知プロパティ

		private NewStatusViewModel _CurrentStatus;

		public NewStatusViewModel CurrentStatus
		{
			get { return this._CurrentStatus; }
			set
			{
				if (this._CurrentStatus != value)
				{
					this._CurrentStatus.SafeDispose();
					this._CurrentStatus = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public string Footer
		{
			get { return this.Composer.Footer; }
			set { this.Composer.Footer = value; }
		}

		public bool FooterEnabled
		{
			get { return this.Composer.FooterEnabled; }
			set { this.Composer.FooterEnabled = value; }
		}


		public ComposerViewModel() : this(MukyutterClient.Current.Composer) { }

		public ComposerViewModel(Composer composer)
		{
			if (this.IsInDesignMode) return;

			this.Composer = composer;
			this.CurrentStatus = new NewStatusViewModel(this, composer.CurrentStatus);
			this.NewStatuses = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				composer.NewStatuses,
				status => new NewStatusViewModel(this, status),
				App.Current.Dispatcher);

			this.CompositeDisposable.Add(new PropertyChangedEventListener(composer)
			{
				{ "Footer", (sender, e) => this.RaisePropertyChanged(e.PropertyName) },
				{ "FooterEnabled", (sender, e) => this.RaisePropertyChanged(e.PropertyName) },
				{ "CurrentStatus", (sender, e) => this.CurrentStatus = new NewStatusViewModel(this, this.Composer.CurrentStatus) },
			});

			this.CompositeDisposable.Add(new EventListener<EventHandler>(
				h => this.Composer.FocusRequested += h,
				h => this.Composer.FocusRequested += h,
				(sender, e) => this.CurrentStatus.Focus()));
		}


		public void Update()
		{
			this.Composer.Update();
		}
	}
}
