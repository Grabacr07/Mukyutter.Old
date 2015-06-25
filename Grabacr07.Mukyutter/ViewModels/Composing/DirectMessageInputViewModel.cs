using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.ViewModels.Composing
{
	public class DirectMessageInputViewModel : StateViewModel
	{
		public UserViewModel Sender { get; private set; }
		public UserViewModel Destination { get; private set; }

		public DirectMessageInputViewModel()
		{
			// ToDo: 作りかけ
		}
	}
}
