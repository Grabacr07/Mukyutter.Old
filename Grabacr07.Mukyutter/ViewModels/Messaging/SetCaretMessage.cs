using System.Windows;
using Livet.Messaging;

namespace Grabacr07.Mukyutter.ViewModels.Messaging
{
	public class SetCaretMessage : InteractionMessage
	{
		//Viewでメッセージインスタンスを生成する時のためのコンストラクタ
		public SetCaretMessage() { }

		//ViewModelからMessenger経由での発信目的でメッセージインスタンスを生成するためのコンストラクタ
		public SetCaretMessage(string messageKey)
			: base(messageKey) { }


		public int CaretIndex
		{
			get { return (int)GetValue(CaretIndexProperty); }
			set { SetValue(CaretIndexProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CaretIndex.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CaretIndexProperty =
			DependencyProperty.Register("CaretIndex", typeof(int), typeof(SetCaretMessage), new UIPropertyMetadata(0));



		/// <summary>
		/// 派生クラスでは必ずオーバーライドしてください。Freezableオブジェクトとして必要な実装です。<br/>
		/// 通常このメソッドは、自身の新しいインスタンスを返すように実装します。
		/// </summary>
		/// <returns>自身の新しいインスタンス</returns>
		protected override Freezable CreateInstanceCore()
		{
			return new SetCaretMessage(this.MessageKey)
			{
				CaretIndex = this.CaretIndex,
			};
		}
	}
}
