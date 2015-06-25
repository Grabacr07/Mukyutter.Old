using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Wizard;
using Grabacr07.Mukyutter.Models.Twitter.Net;
using Grabacr07.Utilities.Reactive;
using Livet;

namespace Grabacr07.Mukyutter.ViewModels.Twitter.Accounts
{
	public class AccountWizardViewModel : ViewModel
	{
		private AccountWizard wizard;
		private Action close;

		public ReadOnlyDispatcherCollection<ApplicationViewMoel> Applications { get; private set; }

		#region SelectedApplication 変更通知プロパティ

		private ApplicationViewMoel _SelectedApplication;

		public ApplicationViewMoel SelectedApplication
		{
			get { return this._SelectedApplication; }
			set
			{
				if (this._SelectedApplication != value)
				{
					this._SelectedApplication = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		#region PinCode 変更通知プロパティ

		private string _PinCode;

		public string PinCode
		{
			get { return this._PinCode; }
			set
			{
				if (this._PinCode != value)
				{
					this._PinCode = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region UserName 変更通知プロパティ

		private object _UserName;

		public object UserName
		{
			get { return this._UserName; }
			set
			{
				if (this._UserName != value)
				{
					this._UserName = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ErrorMessage 変更通知プロパティ

		private string _ErrorMessage;

		public string ErrorMessage
		{
			get { return this._ErrorMessage; }
			set
			{
				if (this._ErrorMessage != value)
				{
					this._ErrorMessage = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsInteractive 変更通知プロパティ

		private bool _IsInteractive;

		public bool IsInteractive
		{
			get { return this._IsInteractive; }
			set
			{
				if (this._IsInteractive != value)
				{
					this._IsInteractive = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("CanAccessToken");
				}
			}
		}

		#endregion

		#region CanAccessToken 変更通知プロパティ

		private bool _CanAccessToken;

		public bool CanAccessToken
		{
			get { return this._CanAccessToken && this.IsInteractive; }
			set
			{
				if (this._CanAccessToken != value)
				{
					this._CanAccessToken = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		public AccountWizardViewModel(Action close)
		{
			this.wizard = new AccountWizard();
			this.close = close;

			this.Applications = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				TwitterClient.Current.Applications,
				app => new ApplicationViewMoel(app),
				DispatcherHelper.UIDispatcher);
			this.CompositeDisposable.Add(this.Applications);

			this.SelectedApplication = this.Applications.FirstOrDefault();
			this.IsInteractive = true;
			this.CanAccessToken = false;
		}


		public async void GetRequestToken()
		{
			this.ErrorMessage = "";
			this.IsInteractive = false;

			try
			{
				await this.wizard.GetRequestToken(this.SelectedApplication.Application);
				this.CanAccessToken = true;
			}
			catch (Exception ex)
			{
				this.ErrorMessage = ex.Message;
			}

			this.IsInteractive = true;
		}

		public async void GetAccessToken()
		{
			this.ErrorMessage = "";
			this.IsInteractive = false;

			try
			{
				await this.wizard.GetAccessToken(this.PinCode);
				this.close();
			}
			catch (Exception ex)
			{
				this.ErrorMessage = ex.Message;
			}

			this.IsInteractive = true;
		}

		public void Cancel()
		{
			this.close();
		}
	}
}
