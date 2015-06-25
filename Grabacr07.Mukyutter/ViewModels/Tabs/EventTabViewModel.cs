using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Twitter.Data.Events;
using Grabacr07.Mukyutter.ViewModels.Twitter;
using Grabacr07.Mukyutter.ViewModels.Twitter.Events;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Tabs
{
	public class EventTabViewModel : TabViewModel
	{
		public override string Name
		{
			get { return "Events"; }
		}

		public ReadOnlyDispatcherCollection<EventViewModel> Events { get; private set; }

		#region EventsCount 変更通知プロパティ

		private string _EventsCount;

		public string EventsCount
		{
			get { return this._EventsCount; }
			set
			{
				if (this._EventsCount != value)
				{
					this._EventsCount = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		public EventTabViewModel()
			: base(new Block())
		{
			this.Events = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				MukyutterClient.Current.Events,
				EventViewModel.Select,
				DispatcherHelper.UIDispatcher);
			this.CompositeDisposable.Add(this.Events);

			this.SetCount(0);
			this.CompositeDisposable.Add(new CollectionChangedEventListener(
				MukyutterClient.Current.Events,
				(sender, e) => this.SetCount(MukyutterClient.Current.Events.Count)));
		}

		public void Clear()
		{
			MukyutterClient.Current.Events.Clear();
		}

		private void SetCount(int count)
		{
			this.EventsCount = count + " 件のイベント";
		}
	}
}
