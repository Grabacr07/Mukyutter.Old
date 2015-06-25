using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter;
using Grabacr07.Mukyutter.ViewModels.Twitter.Accounts;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Tabs
{
	public abstract class TabViewModel : ViewModelBase
	{
		public Block Block { get; private set; }

		#region Name 変更通知プロパティ

		public virtual string Name
		{
			get { return this.Block.Name; }
			set { this.Block.Name = value; }
		}

		#endregion

		#region IsSelected 変更通知プロパティ

		public virtual bool IsSelected
		{
			get { return this.Block.IsSelected; }
			set { this.Block.IsSelected = value; }
		}

		#endregion

		#region Counter 変更通知プロパティ

		private int _Counter;

		public virtual int Counter
		{
			get { return this._Counter; }
			set
			{
				if (this._Counter != value)
				{
					this._Counter = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		protected TabViewModel(Block block)
		{
			if (this.IsInDesignMode)
			{
				this.Block = new Block();
				return;
			}

			this.Block = block;
			this.CompositeDisposable.Add(new PropertyChangedEventListener(block)
			{
				(sender, e) => this.RaisePropertyChanged(e.PropertyName),
			});
		}
	}
}
