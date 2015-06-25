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
	public class EditBox : PromptTextBox
	{
		static EditBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(EditBox), new FrameworkPropertyMetadata(typeof(EditBox)));
		}

		#region IsEdit 依存関係プロパティ

		public bool IsEdit
		{
			get { return (bool)this.GetValue(EditBox.IsEditProperty); }
			set { this.SetValue(EditBox.IsEditProperty, value); }
		}
		public static readonly DependencyProperty IsEditProperty =
			DependencyProperty.Register("IsEdit", typeof(bool), typeof(EditBox), new UIPropertyMetadata(false));

		#endregion

		#region EmptyText 依存関係プロパティ

		public string EmptyText
		{
			get { return (string)this.GetValue(EditBox.EmptyTextProperty); }
			set { this.SetValue(EditBox.EmptyTextProperty, value); }
		}
		public static readonly DependencyProperty EmptyTextProperty =
			DependencyProperty.Register("EmptyText", typeof(string), typeof(EditBox), new UIPropertyMetadata(""));

		#endregion

		#region EmptyTextBrush 依存関係プロパティ

		public Brush EmptyTextBrush
		{
			get { return (Brush)this.GetValue(EditBox.EmptyTextBrushProperty); }
			set { this.SetValue(EditBox.EmptyTextBrushProperty, value); }
		}
		public static readonly DependencyProperty EmptyTextBrushProperty =
			DependencyProperty.Register("EmptyTextBrush", typeof(Brush), typeof(EditBox), new UIPropertyMetadata(Brushes.Gray));

		#endregion


	}
}
