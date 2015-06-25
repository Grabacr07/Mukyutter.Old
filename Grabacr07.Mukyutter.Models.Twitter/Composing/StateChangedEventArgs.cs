using System;

namespace Grabacr07.Mukyutter.Models.Twitter.Composing
{
	public class StateChangedEventArgs : EventArgs
	{
		public StateBase State { get; private set; }

		internal StateChangedEventArgs(StateBase state)
		{
			this.State = state;
		}
	}
}
