using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Mukyutter.Models.Twitter.Data.Entity;
using Grabacr07.Mukyutter.Models.Twitter.Internal;
using Grabacr07.Mukyutter.Views.Controls;
using Grabacr07.Utilities;
using Ctrls = Grabacr07.Mukyutter.Views.Controls;

namespace Grabacr07.Mukyutter.Models.Twitter
{
	static class EntitiesExtensions
	{
		public static IEnumerable<RichText> ToRichText(this Status status)
		{
			var list = new List<RichText>();
			var current = 0;
			status.Entities.UserMentions.Select(m => new
				{
					m.Indices,
					RichText = (RichText)new Mention
					{
						Text = status.Text.Substring(m.Indices.StartIndex, m.Indices.Length),
						User = m.User
					}
				})
				.Concat(status.Entities.Hashtags.Select(h => new
				{
					h.Indices,
					RichText = (RichText)new Ctrls.Hashtag
					{
						Text = status.Text.Substring(h.Indices.StartIndex, h.Indices.Length)
					}
				}))
				.Concat(status.Entities.Urls.Select(u => new
				{
					u.Indices,
					RichText = (RichText)new Ctrls.Url
					{
						Text = u.DisplayUrl ?? status.Text.Substring(u.Indices.StartIndex, u.Indices.Length),
						Uri = u.ExpandedUrl ?? u.EntityUrl,
					}
				}))
				.Concat(status.Entities.Media.Select(m => new
				{
					m.Indices,
					RichText = (RichText)new Ctrls.Url
					{
						//Text = m.MediaUrlHttps != null
						//	? m.MediaUrlHttps.ToString()
						//	: m.DisplayUrl ?? status.Text.Substring(m.Indices.StartIndex, m.Indices.Length),
						Text = m.DisplayUrl ?? status.Text.Substring(m.Indices.StartIndex, m.Indices.Length),
						Uri = m.ExpandedUrl ?? m.MediaUrl
					}
				}))
				.OrderBy(a => a.Indices.StartIndex)
				.ForEach(e =>
				{
					if (e.Indices.StartIndex - current > 0)
					{
						list.Add(new Regular { Text = status.Text.Substring(current, e.Indices.StartIndex - current).DecodeCER() });
					}
					list.Add(e.RichText);
					current = e.Indices.EndIndex;
				});

			if (current < status.Text.Length)
			{
				list.Add(new Regular { Text = status.Text.Substring(current).DecodeCER() });
			}

			return list;
		}
	}
}
