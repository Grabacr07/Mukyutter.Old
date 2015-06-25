using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Livet.Messaging;

namespace Grabacr07.Mukyutter.ViewModels.Tabs.TimelineTabs
{
	class ListViewModel : ViewModelBase
	{
		// チェック状態が変更されたときの動作を上から流し込めるようにする
		private Action checkAction;

		public List List { get; private set; }

		#region IsChecked 変更通知プロパティ

		private bool _IsChecked;

		public bool IsChecked
		{
			get { return this._IsChecked; }
			set
			{
				if (this._IsChecked != value)
				{
					this._IsChecked = value;
					this.RaisePropertyChanged();
					if (this.checkAction != null) this.checkAction();
				}
			}
		}

		#endregion

		public string Name
		{
			get { return this.List.FullName; }
		}


		public ListViewModel(List list, Action checkAction)
		{
			this.List = list;
			this.checkAction = checkAction;
		}

		public override string ToString()
		{
			return this.List.FullName;
		}
	}
}
