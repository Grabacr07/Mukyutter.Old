using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models;

namespace Grabacr07.Mukyutter.ViewModels.Tabs
{
	class DevTabViewModel : TabViewModel
	{
		public override string Name
		{
			get { return "Debugger"; }
		}

		public DevTabViewModel()
			: base(new Block()) { }

	}
}
