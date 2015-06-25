using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Twitter.Composing
{
	public class Reply : StateBase
	{
		public TwitterAccount Sender { get; private set; }

		public Status InReplyTo { get; private set; }

		internal Reply(TwitterAccount sender, Status inReplyTo)
		{
			this.Sender = sender;
			this.InReplyTo = inReplyTo;
		}
	}
}
