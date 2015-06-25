using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Events;

namespace Grabacr07.Mukyutter.ViewModels.Twitter.Events
{
	public class UnknownViewModel : EventViewModel
	{
		public UnknownViewModel(Event ev) : base(ev) { }

		public override string ToString()
		{
			return "Unknown event: " + this.Event.GetType().FullName;
		}
	}
}
