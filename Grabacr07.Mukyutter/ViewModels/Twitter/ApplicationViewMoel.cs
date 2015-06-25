using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.Mukyutter.ViewModels.Twitter
{
	public class ApplicationViewMoel : ViewModel
	{
		public TwitterApplication Application { get; private set; }

		public string Name
		{
			get { return this.Application.Name; }
			set { this.Application.Name = value; }
		}

		public ApplicationViewMoel(TwitterApplication application)
		{
			this.Application = application;
			var listener = new PropertyChangedEventListener(application)
			{
				{ "Name", (sender, e) => this.RaisePropertyChanged("Name") },
			};
			this.CompositeDisposable.Add(listener);
		}
	}
}
