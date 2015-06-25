using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grabacr07.Mukyutter.Models.Settings
{
	[Serializable]
	public class WindowSettings
	{
		public Point Location { get; set; }

		public Size Size { get; set; }

		public WindowState State { get; set; }

		public double Opacity { get; set; }

		public WindowSettings()
		{
			this.Location = new Point(double.NaN, double.NaN);
			this.Size = new Size(800, 800);
			this.State = WindowState.Normal;
			this.Opacity = 1.0;
		}
	}
}
