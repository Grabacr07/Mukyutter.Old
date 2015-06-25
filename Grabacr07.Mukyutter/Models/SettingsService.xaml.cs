using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grabacr07.Mukyutter.Models
{
	public class SettingsService
	{
		private App app;

		public SettingsService(App app)
		{
			this.app = app;
		}

		public T Get<T>(object key)
		{
			if (this.app.Resources.Contains(key))
			{
				var value = this.app.Resources[key];
				if (value is T)
				{
					return (T)value;
				}
			}
			return default(T);
		}

		public void Set(object key, object value)
		{
			if (this.app.Resources.Contains(key))
			{
				this.app.Resources[key] = value;
			}
		}
	}
}
