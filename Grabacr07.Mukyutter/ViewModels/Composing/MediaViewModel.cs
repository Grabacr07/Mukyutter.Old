using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.ViewModels.Composing
{
	public class MediaViewModel : ViewModelBase
	{
		private Action deleteAction;
		public string Path { get; private set; }
		public ImageSource Thumb { get; private set; }

		public MediaViewModel(string path, Action deleteAction)
		{
			this.deleteAction = deleteAction;
			this.Path = path;

			try
			{
				this.Thumb = new BitmapImage(new Uri(path));
			}
			catch (Exception ex)
			{
				ex.Write();
				this.Thumb = new BitmapImage(new Uri("/Mukyutter;component/Images/UserIcon/DummyUser25.png", UriKind.Relative));
			}
		}

		public void Delete()
		{
			this.deleteAction();
		}
	}
}
