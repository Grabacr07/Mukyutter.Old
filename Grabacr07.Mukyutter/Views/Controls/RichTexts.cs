using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Models.Twitter.Net;

namespace Grabacr07.Mukyutter.Views.Controls
{
	public abstract class RichText
	{
		public string Text { get; set; }
	}

	public abstract class Link : RichText
	{
		public abstract void Click();
	}



	public class Regular : RichText
	{
	}

	public class Mention : Link
	{
		public User User { get; set; }

		public override void Click()
		{
			if (this.User != null)
			{
				Process.Start(UrlHelper.GetUserHomeUrl(this.User.ScreenName).ToString());
			}
		}
	}

	public class Url : Link
	{
		public Uri Uri { get; set; }

		public override void Click()
		{
			if (this.Uri != null)
			{
				Process.Start(this.Uri.ToString());
			}
		}
	}

	public class Hashtag : Link
	{
		public override void Click()
		{
			if (!string.IsNullOrEmpty(this.Text))
			{
				Process.Start(UrlHelper.GetSerchUrl(this.Text).ToString());
			}
		}
	}
}
