using System.Windows;
using Livet.Behaviors.Messaging;
using Livet.Messaging;

namespace Grabacr07.Mukyutter.Views.Behaviors.Messaging
{
	//Tはこのアクションがアタッチできる型を表します。
	//この場合はこのアクションはFrameworkElementにアタッチできます。
	public class FocusAction : InteractionMessageAction<FrameworkElement>
	{
		protected override void InvokeAction(InteractionMessage m)
		{
			this.AssociatedObject.Focus();
		}
	}
}
