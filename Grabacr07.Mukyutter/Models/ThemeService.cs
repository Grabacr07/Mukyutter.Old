using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grabacr07.Mukyutter.Models
{
	public enum Theme
	{
		DarkGray, LightGray
	}

	public class ThemeService
	{
		private App app;
		private Theme _current;
		public Theme Current { get { return this._current; } }

		public ThemeService(App app)
		{
			this.app = app;
			this.Change(Theme.DarkGray);
		}

		public void Change()
		{
			var theme = this.Current == Theme.DarkGray
				? Theme.LightGray
				: Theme.DarkGray;
			this.Change(theme);
		}
		public void Change(Theme theme)
		{
			var uri = new Uri(string.Format(@"pack://application:,,,/Themes/Mukyutter.{0}Color.xaml", theme.ToString()));
			var dic = new ResourceDictionary { Source = uri };

			dic.Keys.OfType<string>()
				.Where(key => this.app.Resources.Contains(key))
				.ForEach(key => this.app.Resources[key] = dic[key]);

			this._current = theme;
		}
	}
}
