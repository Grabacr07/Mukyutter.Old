using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities;
using Livet.Messaging.Windows;

namespace Grabacr07.Mukyutter.ViewModels.Tabs.TimelineTabs
{
	internal class ReceivingSettingsWindowViewModel : ViewModelBase
	{
		#region IsAll 変更通知プロパティ

		private bool _IsAll;

		public bool IsAll
		{
			get { return this._IsAll; }
			set
			{
				if (this._IsAll != value)
				{
					this._IsAll = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsList 変更通知プロパティ

		private bool _IsList;

		public bool IsList
		{
			get { return this._IsList; }
			set
			{
				if (this._IsList != value)
				{
					this._IsList = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public List<ListAccountViewModel> Accounts { get; private set; }

		#region SelectedAccount 変更通知プロパティ

		private ListAccountViewModel _SelectedAccount;

		public ListAccountViewModel SelectedAccount
		{
			get { return this._SelectedAccount; }
			set
			{
				if (this._SelectedAccount != value)
				{
					this._SelectedAccount = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public bool IsOK { get; private set; }

		public IEnumerable<List> Lists
		{
			get
			{
				return this.Accounts.SelectMany(a => a.Lists)
					.Where(l => l.IsChecked)
					.Select(l => l.List);
			}
		}

		public string Result
		{
			get { return this.IsList ? this.Lists.Select(l => l.FullName).ToString(", ") : "受信したすべてのツイート"; }
		}


		internal ReceivingSettingsWindowViewModel() { }

		public ReceivingSettingsWindowViewModel(bool isAll)
		{
			this.IsAll = isAll;
			this.IsList = !isAll;

			this.IsOK = false;
			this.Accounts = TwitterClient.Current.Accounts.Select(a => new ListAccountViewModel(a, this.Update)).ToList();
			this.SelectedAccount = this.Accounts.FirstOrDefault();
		}


		public void Update()
		{
			this.RaisePropertyChanged("Lists");
			this.RaisePropertyChanged("Result");
		}

		public void OK()
		{
			this.IsOK = true;
			this.Close();
		}

		public void Close()
		{
			this.Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
		}
	}
}
