using System.Collections.ObjectModel;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using System.Collections.Generic;
using System.Linq;

namespace Grabacr07.Mukyutter.Models.Twitter.Composing
{
	public class MultiReply : StateBase
	{
		public TwitterAccount Sender { get; private set; }

		#region Destinations 変更通知プロパティ

		private IEnumerable<User> _Destinations;

		public IEnumerable<User> Destinations
		{
			get { return _Destinations; }
			set
			{
				if (!EqualityComparer<IEnumerable<User>>.Default.Equals(_Destinations, value))
				{
					_Destinations = value;
					RaisePropertyChanged("Destinations");
				}
			}
		}

		#endregion

		internal MultiReply(TwitterAccount sender)
		{
			this.Sender = sender;
			this.Destinations = Enumerable.Empty<User>();
		}
	}
}
