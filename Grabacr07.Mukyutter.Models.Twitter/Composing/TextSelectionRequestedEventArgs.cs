using System;

namespace Grabacr07.Mukyutter.Models.Twitter.Composing
{
	public class TextSelectionRequestedEventArgs : EventArgs
	{
		public int Start { get; internal set; }
		public int Length { get; internal set; }
	}
}
