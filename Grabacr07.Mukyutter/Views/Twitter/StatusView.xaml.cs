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

namespace Grabacr07.Mukyutter.Views.Twitter
{
	/// <summary>
	/// StatusView.xaml の相互作用ロジック
	/// </summary>
	public partial class StatusView : UserControl
	{
		public StatusView()
		{
			InitializeComponent();
		}


		#region CanControl 依存関係プロパティ

		public bool CanControl
		{
			get { return (bool)this.GetValue(StatusView.CanControlProperty); }
			set { this.SetValue(StatusView.CanControlProperty, value); }
		}
		public static readonly DependencyProperty CanControlProperty =
			DependencyProperty.Register("CanControl", typeof(bool), typeof(StatusView), new UIPropertyMetadata(false));

		#endregion

	}
}
