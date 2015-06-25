using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Twitter.Composing
{
	public class Normal : StateBase
	{
		public TwitterAccount Sender { get; private set; }

		internal Normal(TwitterAccount sender)
		{
			this.Sender = sender;
		}
	}
}
