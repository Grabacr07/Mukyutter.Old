using System;

namespace Grabacr07.Mukyutter.Models.Twitter.Composing
{
	public class CaretIndexChangeRequestedEventArgs : EventArgs
	{
		public int CaretIndex { get; internal set; }
	}
}
