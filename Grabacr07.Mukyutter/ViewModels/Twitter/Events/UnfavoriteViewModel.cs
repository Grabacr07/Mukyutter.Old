using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Events;
using Grabacr07.Utilities;

namespace Grabacr07.Mukyutter.ViewModels.Twitter.Events
{
	public class UnfavoriteViewModel : EventViewModel
	{
		public string Text
		{
			get { return ((Favorite)this.Event).TargetObject.Text.Flatten(); }
		}

		public UnfavoriteViewModel(Favorite unfav) : base(unfav) { }
	}
}
