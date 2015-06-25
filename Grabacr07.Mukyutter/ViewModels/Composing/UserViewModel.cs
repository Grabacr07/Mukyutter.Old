using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Grabacr07.Mukyutter.Models;
using Grabacr07.Mukyutter.Models.Imaging;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.ViewModels.Extensions;
using Livet;

namespace Grabacr07.Mukyutter.ViewModels.Composing
{
	public class UserViewModel : ViewModel
	{
		private readonly User user;

		public string Name
		{
			get { return user.ToDisplayName(); }
		}

		private WeakReferenceBitmap _Icon;
		public WeakReferenceBitmap Icon
		{
			get { return this._Icon ?? (this._Icon = new WeakReferenceBitmap(this.user.ProfileImageUrl)); }
		}

		public UserViewModel(User user)
		{
			this.user = user;
		}
	}
}
