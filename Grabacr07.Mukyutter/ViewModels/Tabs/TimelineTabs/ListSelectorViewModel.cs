using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.Windows;

namespace Grabacr07.Mukyutter.ViewModels.Tabs.TimelineTabs
{
	internal class ListSelectorViewModel : ViewModelBase
	{
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

		public IEnumerable<List> Result
		{
			get
			{
				return this.Accounts.SelectMany(a => a.Lists)
				           .Where(l => l.IsChecked)
				           .Select(l => l.List);
			}
		}

		public string ResultMessage
		{
			get { return this.Result.Select(l => l.FullName).ToString(", "); }
		}

		public bool IsOK { get; private set; }


		public ListSelectorViewModel()
		{
			if (!this.IsInDesignMode)
			{
				this.IsOK = false;
				this.Accounts = TwitterClient.Current.Accounts.Select(a => new ListAccountViewModel(a, this.Update)).ToList();
				this.SelectedAccount = this.Accounts.FirstOrDefault();
			}
		}


		public void Update()
		{
			this.RaisePropertyChanged("Result");
			this.RaisePropertyChanged("ResultMessage");
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
