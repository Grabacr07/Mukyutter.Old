using System;

namespace Grabacr07.Utilities.Events
{
	public class UpdatedEventArgs<T> : EventArgs
	{
		public T NewValue { get; private set; }

		public T OldValue { get; private set; }

		public UpdatedEventArgs(T oldValue, T newValue)
		{
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}
	}
}
