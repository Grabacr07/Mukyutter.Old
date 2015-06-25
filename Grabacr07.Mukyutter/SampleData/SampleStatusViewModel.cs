using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.ViewModels.Twitter;
using Grabacr07.Mukyutter.Views.Controls;

namespace Grabacr07.Mukyutter.SampleData
{
	internal class SampleStatusViewModel : StatusViewModel
	{
		public new StatusId Id
		{
			get { return 1234567890; }
		}

		public new UserViewModel User
		{
			get { return new SampleUserViewModel(); }
		}

		public new StatusViewModel DisplayStatus
		{
			get { return this; }
		}

		public new bool IsSelf
		{
			get { return false; }
		}

		public new bool IsRetweet
		{
			get { return true; }
		}

		public new StatusViewModel RetweetedStatus
		{
			get { return this; }
		}

		public new string FlatText
		{
			get { return "画面は開発中のものです。実際のパチュリーはもっと可愛い可能性があります。"; }
		}

		public new IEnumerable<RichText> RichText
		{
			get
			{
				return new List<RichText>
				{
					new Regular { Text = "画面は開発中のものです。" },
					new Url { Text = "実際のパチュリー", Uri = new Uri("https://www.google.co.jp/search?hl=ja&q=%E3%83%91%E3%83%81%E3%83%A5%E3%83%AA%E3%83%BC&lr=lang_ja&um=1&ie=UTF-8&tbm=isch&source=og&sa=N&tab=wi&ei=jxZfUenYDoWAiQfkpoGACA&biw=1184&bih=1132&sei=kBZfUbXmL4nYigeMmICAAg#um=1&hl=ja&lr=lang_ja&tbs=lr:lang_1ja&tbm=isch&sa=1&q=%E3%83%91%E3%83%81%E3%83%A5%E3%83%AA%E3%83%BC%E3%83%BB%E3%83%8E%E3%83%BC%E3%83%AC%E3%83%83%E3%82%B8&oq=%E3%83%91%E3%83%81%E3%83%A5%E3%83%AA%E3%83%BC%E3%83%BB%E3%83%8E%E3%83%BC%E3%83%AC%E3%83%83%E3%82%B8&gs_l=img.3...104.2135.0.2596.10.10.0.0.0.1.123.783.8j2.10.0...0.0...1c.4.8.img.eu419iNVSno&bav=on.2,or.r_cp.r_qf.&bvm=bv.44770516,d.aGc&fp=b89ecf106d55acf8&biw=1184&bih=1132")},
					new Regular { Text = "はもっと可愛い可能性があります。" },
				};
			}
		}

		public new string Time
		{
			get { return DateTime.Now.TimeOfDay.ToString(); }
		}

		public new string AbsoluteDateTime
		{
			get { return DateTime.Now.ToString(); }
		}

		public new string AbsoluteShortDateTime
		{
			get { return DateTime.Now.Ticks % 2 == 0 ? "12:34:56" : "05/01"; }
		}

		public new string RelativeDateTime
		{
			get { return "10 minutes ago"; }
		}

		public new string Client
		{
			get { return "Mukyutter; Sample"; }
		}

		public new bool CanOpenClientPage
		{
			get { return true; }
		}
	}
}
