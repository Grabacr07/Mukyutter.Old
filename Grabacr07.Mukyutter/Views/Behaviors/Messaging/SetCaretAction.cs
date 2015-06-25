using System.Windows.Controls;
using Grabacr07.Mukyutter.ViewModels.Messaging;
using Livet.Behaviors.Messaging;

namespace Grabacr07.Mukyutter.Views.Behaviors.Messaging
{
	public class SetCaretAction : InteractionMessageAction<TextBox>
	{
		protected override void InvokeAction(Livet.Messaging.InteractionMessage m)
		{
			var setCaretMsg = m as SetCaretMessage;

			this.AssociatedObject.Focus();
			this.AssociatedObject.CaretIndex = setCaretMsg.CaretIndex;
		}
	}
}
