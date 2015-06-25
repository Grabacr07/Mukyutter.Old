using System.Linq;
using Grabacr07.Mukyutter.Models.Twitter.Composing;
using Livet;
using System.Collections.Generic;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.ViewModels.Composing
{
	public class MultiReplyViewModel : StateViewModel
	{
		public UserViewModel Sender { get; private set; }

		#region Destinations 変更通知プロパティ

		private IEnumerable<UserViewModel> _Destinations;

		public IEnumerable<UserViewModel> Destinations
		{
			get { return _Destinations; }
			set
			{
				if (!EqualityComparer<IEnumerable<UserViewModel>>.Default.Equals(_Destinations, value))
				{
					_Destinations = value;
					RaisePropertyChanged("Destinations");
					RaisePropertyChanged("TargetCount");
				}
			}
		}

		#endregion

		public int TargetCount
		{
			get { return this.Destinations.Count(); }
		}


		public MultiReplyViewModel(MultiReply multiReply)
		{
			this.Sender = new UserViewModel(multiReply.Sender.User);
			this.UpdateDestinations(multiReply.Destinations);

			multiReply.PropertyChanged += (sender, e) => this.UpdateDestinations(((MultiReply)sender).Destinations);
		}

		private void UpdateDestinations(IEnumerable<User> destinations)
		{
			this.Destinations = destinations.Select(u => new UserViewModel(u));
		}
	}
}
