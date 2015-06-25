using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Mukyutter.Models.Twitter.Internal;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Entity
{
	[Serializable]
	public class Entities
	{
		public Media[] Media { get; private set; }

		public Url[] Urls { get; private set; }

		public UserMention[] UserMentions { get; private set; }

		public Hashtag[] Hashtags { get; private set; }


		private static readonly Entities _default = new Entities
		{
			Media = new Media[0],
			Urls = new Url[0],
			UserMentions = new UserMention[0],
			Hashtags = new Hashtag[0]
		};
		public static Entities Default
		{
			get { return _default; }
		}

		internal static Entities ParseCore(dynamic djson)
		{
			try
			{
				var result = new Entities();

				if (djson.IsDefined("media"))
				{
					result.Media = ((object[])djson.media)
						.Select(media => (dynamic)media)
						.Select(media => new Media
						{
							Id = Convert.ToUInt64(media.id),
							MediaUrl = Helper.ToUri(media.media_url),
							MediaUrlHttps = Helper.ToUri(media.media_url_https),
							Url = Helper.ToUri(media.url),
							DisplayUrl = media.display_url,
							ExpandedUrl = Helper.ToUri(media.expanded_url),
							Indices = Indices.ParseCore(media.indices),
						})
						.ToArray();
				}
				else
				{
					result.Media = new Media[0];
				}

				if (djson.IsDefined("user_mentions"))
				{
					result.UserMentions = ((object[])djson.user_mentions)
						.Select(mention => (dynamic)mention)
						.Do(mention => User.ParseCore(mention))
						.Select(mention => new UserMention
						{
							Id = Convert.ToInt64(mention.id),
							Indices = Indices.ParseCore(mention.indices),
						})
						.ToArray();
				}
				else
				{
					result.UserMentions = new UserMention[0];
				}

				if (djson.IsDefined("urls"))
				{
					result.Urls = ((object[])djson.urls)
						.Select(url => (dynamic)url)
						.Select(url => new Url
						{
							EntityUrl = Helper.ToUri(url.url),
							DisplayUrl = url.IsDefined("display_url") ? url.display_url : null,
							ExpandedUrl = url.IsDefined("expanded_url") ? Helper.ToUri(url.expanded_url) : null,
							Indices = Indices.ParseCore(url.indices)
						})
						.ToArray();
				}
				else
				{
					result.Urls = new Url[0];
				}

				if (djson.IsDefined("hashtags"))
				{
					result.Hashtags = ((object[])djson.hashtags)
						.Select(hashtag => (dynamic)hashtag)
						.Select(hashtag => new Hashtag
						{
							Text = hashtag.text,
							Indices = Indices.ParseCore(hashtag.indices),
						})
						.ToArray();
				}
				else
				{
					result.Hashtags = new Hashtag[0];
				}

				return result;
			}
			catch (Exception ex)
			{
				throw new JsonParseException(djson, typeof(Entities), ex);
			}
		}
	}
}
