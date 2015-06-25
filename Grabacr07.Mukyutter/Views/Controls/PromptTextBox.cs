using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grabacr07.Mukyutter.Views.Controls
{
	[TemplateVisualState(Name = "Empty", GroupName = "TextStates")]
	[TemplateVisualState(Name = "NotEmpty", GroupName = "TextStates")]
	public class PromptTextBox : TextBox
	{
		static PromptTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PromptTextBox), new FrameworkPropertyMetadata(typeof(PromptTextBox)));
		}

		public PromptTextBox()
		{
			this.UpdateTextStates(true);
			this.TextChanged += (sender, e) => this.UpdateTextStates(true);
			this.GotKeyboardFocus += (sender, e) => this.UpdateTextStates(true);
		}

		#region Prompt 依存関係プロパティ

		public string Prompt
		{
			get { return (string)this.GetValue(PromptTextBox.PromptProperty); }
			set { this.SetValue(PromptTextBox.PromptProperty, value); }
		}
		public static readonly DependencyProperty PromptProperty =
			DependencyProperty.Register("Prompt", typeof(string), typeof(PromptTextBox), new UIPropertyMetadata("", PromptChangedCallback));

		private static void PromptChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		#endregion

		#region PromptBrush 依存関係プロパティ

		public Brush PromptBrush
		{
			get { return (Brush)this.GetValue(PromptTextBox.PromptBrushProperty); }
			set { this.SetValue(PromptTextBox.PromptBrushProperty, value); }
		}
		public static readonly DependencyProperty PromptBrushProperty =
			DependencyProperty.Register("PromptBrush", typeof(Brush), typeof(PromptTextBox), new UIPropertyMetadata(Brushes.Gray));

		#endregion


		private void UpdateTextStates(bool useTransitions)
		{
			VisualStateManager.GoToState(this, string.IsNullOrEmpty(this.Text) ? "Empty" : "NotEmpty", useTransitions);
		}
	}
}
