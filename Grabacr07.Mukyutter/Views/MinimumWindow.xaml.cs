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
using System.Windows.Shapes;

namespace Grabacr07.Mukyutter.Views
{
	/// <summary>
	/// MinimumWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MinimumWindow
	{
		public MinimumWindow()
		{
			InitializeComponent();

			this.Closed += (sender, e) => this.DataContext = null;
		}
	}
}
