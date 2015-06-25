using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace Grabacr07.Mukyutter.ViewModels.Common
{
	public class ObservableSynchronizedCollectionEx<T> : ObservableSynchronizedCollection<T>
	{
		public bool IsEmpty
		{
			get { return this.Count == 0; }
		}

		public ObservableSynchronizedCollectionEx()
		{
			this.PropertyChanged += (sender, e) => { if (e.PropertyName == "Count") this.OnPropertyChanged("IsEmpty"); };
		}
		public ObservableSynchronizedCollectionEx(IEnumerable<T> source)
			: base(source)
		{
			this.PropertyChanged += (sender, e) => { if (e.PropertyName == "Count") this.OnPropertyChanged("IsEmpty"); };
		}
	}
}
