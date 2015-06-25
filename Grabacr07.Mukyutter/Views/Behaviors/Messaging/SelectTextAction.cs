using System.Windows.Controls;
using Grabacr07.Mukyutter.ViewModels.Messaging;
using Livet.Behaviors.Messaging;
using Livet.Messaging;

namespace Grabacr07.Mukyutter.Views.Behaviors.Messaging
{
	//Tはこのアクションがアタッチできる型を表します。
	//この場合はこのアクションはFrameworkElementにアタッチできます。
	public class SelectTextAction : InteractionMessageAction<TextBox>
	{
		protected override void InvokeAction(InteractionMessage m)
		{
			var selectMessage = m as SelectTextMessage;

			this.AssociatedObject.Focus();
			this.AssociatedObject.Select(selectMessage.Start, selectMessage.Length);
		}
	}
}
