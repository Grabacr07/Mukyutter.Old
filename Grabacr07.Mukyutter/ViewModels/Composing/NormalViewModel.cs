using Grabacr07.Mukyutter.Models.Twitter.Composing;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using NormalState = Grabacr07.Mukyutter.Models.Twitter.Composing.Normal;

namespace Grabacr07.Mukyutter.ViewModels.Composing
{
	public class NormalViewModel : StateViewModel
	{
		public UserViewModel Sender { get; private set; }

		public NormalViewModel(NormalState normal)
		{
			this.Sender = new UserViewModel(normal.Sender.User);
		}
	}
}
