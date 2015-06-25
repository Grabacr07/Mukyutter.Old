using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Livet;

namespace Grabacr07.Mukyutter.ViewModels
{
	public class ViewModelBase : ViewModel
	{
		public bool IsInDesignMode
		{
			get { return DesignerProperties.GetIsInDesignMode(new DependencyObject()); }
		}
	}
}
