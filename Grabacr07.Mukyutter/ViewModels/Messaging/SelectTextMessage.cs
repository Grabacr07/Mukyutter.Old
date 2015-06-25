using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet.Messaging;
using System.Windows;

namespace Grabacr07.Mukyutter.ViewModels.Messaging
{
	/*
	 * 戻り値のある相互作用メッセージはResponsiveInteractionMessage<T>を継承して作成します。
	 * Tは戻り値の型です。
	 * 戻り値のない相互作用メッセージはInteractionMessageを継承して作成します。
	 */
	public class SelectTextMessage : InteractionMessage
	{
		//Viewでメッセージインスタンスを生成する時のためのコンストラクタ
		public SelectTextMessage() { }

		//ViewModelからMessenger経由での発信目的でメッセージインスタンスを生成するためのコンストラクタ
		public SelectTextMessage(string messageKey)
			: base(messageKey) { }


		public int Start
		{
			get { return (int)GetValue(StartProperty); }
			set { SetValue(StartProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Start.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StartProperty =
			DependencyProperty.Register("Start", typeof(int), typeof(SelectTextMessage), new UIPropertyMetadata(0));


		public int Length
		{
			get { return (int)GetValue(LengthProperty); }
			set { SetValue(LengthProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Length.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty LengthProperty =
			DependencyProperty.Register("Length", typeof(int), typeof(SelectTextMessage), new UIPropertyMetadata(0));



		/// <summary>
		/// 派生クラスでは必ずオーバーライドしてください。Freezableオブジェクトとして必要な実装です。<br/>
		/// 通常このメソッドは、自身の新しいインスタンスを返すように実装します。
		/// </summary>
		/// <returns>自身の新しいインスタンス</returns>
		protected override System.Windows.Freezable CreateInstanceCore()
		{
			return new SelectTextMessage(this.MessageKey)
			{
				Start = this.Start,
				Length = this.Length,
			};
		}
	}
}
