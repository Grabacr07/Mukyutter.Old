using System;
using Grabacr07.Mukyutter.Models.Twitter.Composing;

namespace Grabacr07.Mukyutter.ViewModels.Composing
{
	public class ReplyViewModel : StateViewModel
	{
		public UserViewModel Sender { get; private set; }
		public UserViewModel Destination { get; private set; }
		public string ReplyToStatusText { get; private set; }

		public Action DeleteAction { get; set; }

		public ReplyViewModel(Reply reply)
		{
			this.Sender = new UserViewModel(reply.Sender.User);
			this.Destination = new UserViewModel(reply.InReplyTo.DisplayStatus.User);
			this.ReplyToStatusText = reply.InReplyTo.DisplayStatus.Text;
		}

		public void Delete()
		{
			if (this.DeleteAction != null) this.DeleteAction();
		}
	}
}
