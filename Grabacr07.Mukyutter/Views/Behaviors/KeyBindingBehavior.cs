using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Views.Internal;
using Livet.Behaviors;

namespace Grabacr07.Mukyutter.Views.Behaviors
{
	internal class KeyBindingBehavior : Behavior<FrameworkElement>
	{
		private KeyBindingDefinition keyBinding;
		private bool waitSecond;
		private KeyEventHandler handler;

		private readonly MethodBinder methodBinder = new MethodBinder();
		private readonly MethodBinderWithArgument methodBinderWithArg = new MethodBinderWithArgument();
		private bool hasParameter;

		#region BindingKey 依存関係プロパティ

		public string BindingKey
		{
			get { return (string)this.GetValue(KeyBindingBehavior.BindingKeyProperty); }
			set { this.SetValue(KeyBindingBehavior.BindingKeyProperty, value); }
		}

		public static readonly DependencyProperty BindingKeyProperty =
			DependencyProperty.Register("BindingKey", typeof(string), typeof(KeyBindingBehavior), new UIPropertyMetadata(null));

		#endregion

		#region MethodTarget 依存関係プロパティ

		public object MethodTarget
		{
			get { return (object)this.GetValue(KeyBindingBehavior.MethodTargetProperty); }
			set { this.SetValue(KeyBindingBehavior.MethodTargetProperty, value); }
		}

		public static readonly DependencyProperty MethodTargetProperty =
			DependencyProperty.Register(
				"MethodTarget", typeof(object), typeof(KeyBindingBehavior), new UIPropertyMetadata(null));

		#endregion

		#region MethodName 依存関係プロパティ

		public string MethodName
		{
			get { return (string)this.GetValue(KeyBindingBehavior.MethodNameProperty); }
			set { this.SetValue(KeyBindingBehavior.MethodNameProperty, value); }
		}

		public static readonly DependencyProperty MethodNameProperty =
			DependencyProperty.Register("MethodName", typeof(string), typeof(KeyBindingBehavior), new UIPropertyMetadata(null));

		#endregion

		#region MethodParameter 依存関係プロパティ

		public object MethodParameter
		{
			get { return (object)this.GetValue(KeyBindingBehavior.MethodParameterProperty); }
			set { this.SetValue(KeyBindingBehavior.MethodParameterProperty, value); }
		}

		public static readonly DependencyProperty MethodParameterProperty =
			DependencyProperty.Register(
				"MethodParameter",
				typeof(object),
				typeof(KeyBindingBehavior),
				new UIPropertyMetadata(null, KeyBindingBehavior.MethodParameterChangedCallback));

		private static void MethodParameterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var instance = (KeyBindingBehavior)d;
			instance.hasParameter = true;
		}

		#endregion

		#region IsEnabled 依存関係プロパティ

		public bool IsEnabled
		{
			get { return (bool)this.GetValue(KeyBindingBehavior.IsEnabledProperty); }
			set { this.SetValue(KeyBindingBehavior.IsEnabledProperty, value); }
		}

		public static readonly DependencyProperty IsEnabledProperty =
			DependencyProperty.Register("IsEnabled", typeof(bool), typeof(KeyBindingBehavior), new UIPropertyMetadata(true));

		#endregion

		#region IsHandle 依存関係プロパティ

		public bool IsHandle
		{
			get { return (bool)this.GetValue(KeyBindingBehavior.IsHandleProperty); }
			set { this.SetValue(KeyBindingBehavior.IsHandleProperty, value); }
		}
		public static readonly DependencyProperty IsHandleProperty =
			DependencyProperty.Register("IsHandle", typeof(bool), typeof(KeyBindingBehavior), new UIPropertyMetadata(true));

		#endregion


		public KeyBindingBehavior()
		{
			this.handler = this.HandleKeyDown;
		}

		#region attached / detaching

		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.AddHandler(UIElement.PreviewKeyDownEvent, this.handler, true);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.AssociatedObject.RemoveHandler(UIElement.PreviewKeyDownEvent, this.handler);
		}

		#endregion

		private void HandleKeyDown(object sender, KeyEventArgs e)
		{
			// キーバインドのテーブルから、対象のキーの定義を持ってくる
			if (this.keyBinding == null)
			{
				if (!MukyutterClient.Current.KeyBindings.TryGetValue(this.BindingKey, out this.keyBinding)) return;
			}

			if (this.waitSecond)
			{
				// 第 2 キーを待機している場合、修飾キーはすべて無視する
				if (e.Key.IsModifier()) return;

				// 第 2 キーの待機を開始してから、修飾キー以外の最初のキーが押された時点で、
				// 定義の第 2 キーと一致しているかどうかをチェック
				if (e.Key == this.keyBinding.SecondKey && e.KeyboardDevice.Modifiers.HasFlag(this.keyBinding.SecondModifier))
				{
					// 一致していたらキーバインドに対応する処理を実行
					this.Invoke();
					if (this.IsHandle) e.Handled = true;
				}

				this.waitSecond = false;
			}
			else
			{
				if (e.Key == this.keyBinding.Key && e.KeyboardDevice.Modifiers.HasFlag(this.keyBinding.Modifier))
				{
					// 第 1 キーに一致するキー入力を検出

					// 第 2 キーが定義されている場合、第 2 キーの待機状態に入る
					if (this.keyBinding.HasSecondKey) this.waitSecond = true;

					// 定義されていない場合、キーバインドに対応する処理を実行
					else
					{
						this.Invoke();
						if (this.IsHandle) e.Handled = true;
					}
				}
			}
		}

		private void Invoke()
		{
			if (this.IsEnabled && this.MethodTarget != null && this.MethodName != null)
			{
				if (this.hasParameter)
				{
					this.methodBinderWithArg.Invoke(this.MethodTarget, this.MethodName, this.MethodParameter);
				}
				else
				{
					this.methodBinder.Invoke(this.MethodTarget, this.MethodName);
				}
			}
		}
	}
}
