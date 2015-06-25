using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Notifications;
using Grabacr07.Mukyutter.ViewModels.Common;
using Grabacr07.Mukyutter.ViewModels.Twitter.Accounts;
using Grabacr07.Mukyutter.ViewModels.Twitter.Events;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Tabs
{
	public class SystemTabViewModel : TabViewModel
	{
		public override string Name
		{
			get { return "Mukyutter"; }
		}

		public ReadOnlyDispatcherCollection<AccountViewModel> Accounts
		{
			get { return AccountViewModel.Accounts; }
		}

		#region AccountWizard 変更通知プロパティ

		private AccountWizardViewModel _AccountWizard;

		public AccountWizardViewModel AccountWizard
		{
			get { return this._AccountWizard; }
			set
			{
				if (this._AccountWizard != value)
				{
					this._AccountWizard = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region AccountWizardVisibility 変更通知プロパティ

		private Visibility _AccountWizardVisibility;

		public Visibility AccountWizardVisibility
		{
			get { return this._AccountWizardVisibility; }
			set
			{
				if (this._AccountWizardVisibility != value)
				{
					this._AccountWizardVisibility = value;
					this.AccountWizardButtonVisibility = value == Visibility.Visible
						? Visibility.Collapsed
						: Visibility.Visible;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region AccountWizardButtonVisibility 変更通知プロパティ

		private Visibility _AccountWizardButtonVisibility;

		public Visibility AccountWizardButtonVisibility
		{
			get { return this._AccountWizardButtonVisibility; }
			private set
			{
				if (this._AccountWizardButtonVisibility != value)
				{
					this._AccountWizardButtonVisibility = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public bool HasAccounts
		{
			get { return this.Accounts.Any(); }
		}

		public ReadOnlyDispatcherCollection<ErrorViewModel> Errors { get; private set; }

		public bool HasErrors
		{
			get { return this.Errors.Any(); }
		}

		public ReadOnlyDispatcherCollection<EventViewModel> Events { get; private set; }

		public bool HasEvents
		{
			get { return this.Events.Any(); }
		}


		public SystemTabViewModel()
			: base(new Block())
		{
			this.AccountWizardVisibility = Visibility.Collapsed;

			this.CompositeDisposable.Add(new PropertyChangedEventListener(this.Accounts)
			{
				{ "Count", (sender, e) => this.RaisePropertyChanged("HasAccounts") },
			});

			this.Errors = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				MukyutterClient.Current.Errors,
				error => new ErrorViewModel(error, () => MukyutterClient.Current.Errors.Remove(error)),
				DispatcherHelper.UIDispatcher);
			this.Errors.PropertyChanged += (sender, e) => this.RaisePropertyChanged("HasErrors");
			this.CompositeDisposable.Add(this.Errors);

			this.Events = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				MukyutterClient.Current.Events,
				EventViewModel.Select,
				DispatcherHelper.UIDispatcher);
			this.Events.PropertyChanged += (sender, e) => this.RaisePropertyChanged("HasEvents");
			this.CompositeDisposable.Add(this.Events);
		}


		public void AddAccount()
		{
			this.AccountWizardVisibility = Visibility.Visible;
			this.AccountWizard = new AccountWizardViewModel(() =>
			{
				this.AccountWizardVisibility = Visibility.Collapsed;
				this.AccountWizard.Dispose();
				this.AccountWizard = null;
			});
		}



		#region UsingProxy 変更通知プロパティ

		private bool _UsingProxy;

		public bool UsingProxy
		{
			get { return this._UsingProxy; }
			set
			{
				if (this._UsingProxy != value)
				{
					this._UsingProxy = value;
					if (value)
					{
						TwitterClient.Current.CurrentNetworkProfile = new Models.Twitter.Net.NetworkProfile
						{
							Name = "HISOL",
							Proxy = new Models.Twitter.Net.NetworkProxy
							{
								Address = "http://iproxy.intra.hitachi.co.jp:8080",
								UserName = "23925804",
								Password = new System.Security.SecureString()
							}
						};
						"h2solM25028kG!".ForEach(c => TwitterClient.Current.CurrentNetworkProfile.Proxy.Password.AppendChar(c));
					}
					else
					{
						TwitterClient.Current.CurrentNetworkProfile = null;
					}

					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

	}
}
