using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Grabacr07.Mukyutter.Views.Chrome;

namespace Grabacr07.Mukyutter.Views.Behaviors
{
	/// <summary>
	/// ウィンドウ操作を実行します。
	/// </summary>
	internal class DirectWindowAction : TriggerAction<FrameworkElement>
	{
		#region WindowAction 依存関係プロパティ

		public WindowAction WindowAction
		{
			get { return (WindowAction)this.GetValue(DirectWindowAction.WindowActionProperty); }
			set { this.SetValue(DirectWindowAction.WindowActionProperty, value); }
		}
		public static readonly DependencyProperty WindowActionProperty =
			DependencyProperty.Register("WindowAction", typeof(WindowAction), typeof(DirectWindowAction), new UIPropertyMetadata(WindowAction.Active));

		#endregion

		protected override void Invoke(object parameter)
		{
			this.WindowAction.Invoke(this.AssociatedObject);
		}
	}
}
