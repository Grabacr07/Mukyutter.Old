using System;

namespace Grabacr07.Utilities.Events
{
	public class EventArgs<T> : EventArgs
	{
		public T Value { get; private set; }

		public EventArgs(T value)
		{
			this.Value = value;
		}
	}
}
