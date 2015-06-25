using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data.Entity;
using Grabacr07.Mukyutter.Models.Twitter.Data.Json;
using Grabacr07.Utilities.Development;

namespace Grabacr07.Mukyutter.Models.Twitter.Data
{
	public class Configuration
	{
		public int CharactersReservedPerMedia { get; internal set; }
		public int MaxMediaPerUpload { get; internal set; }
		public int PhotoSizeLimit { get; internal set; }
		public IReadOnlyList<string> NonUsernamePaths { get; internal set; }
		public IReadOnlyDictionary<string, Size> PhotoSizes { get; internal set; }
		public int ShortUrlLengthHttps { get; internal set; }
		public int ShortUrlLength { get; internal set; }

		#region static members

		private static readonly Configuration _Default = new Configuration
		{
			CharactersReservedPerMedia = 23,
			MaxMediaPerUpload = 1,
			PhotoSizeLimit = 3145728,
			NonUsernamePaths = new List<string>
			{
				"about",
				"account",
				"accounts",
				"activity",
				"all",
				"announcements",
				"anywhere",
				"api_rules",
				"api_terms",
				"apirules",
				"apps",
				"auth",
				"badges",
				"blog",
				"business",
				"buttons",
				"contacts",
				"devices",
				"direct_messages",
				"download",
				"downloads",
				"edit_announcements",
				"faq",
				"favorites",
				"find_sources",
				"find_users",
				"followers",
				"following",
				"friend_request",
				"friendrequest",
				"friends",
				"goodies",
				"help",
				"home",
				"im_account",
				"inbox",
				"invitations",
				"invite",
				"jobs",
				"list",
				"login",
				"logo",
				"logout",
				"me",
				"mentions",
				"messages",
				"mockview",
				"newtwitter",
				"notifications",
				"nudge",
				"oauth",
				"phoenix_search",
				"positions",
				"privacy",
				"public_timeline",
				"related_tweets",
				"replies",
				"retweeted_of_mine",
				"retweets",
				"retweets_by_others",
				"rules",
				"saved_searches",
				"search",
				"sent",
				"settings",
				"share",
				"signup",
				"signin",
				"similar_to",
				"statistics",
				"terms",
				"tos",
				"translate",
				"trends",
				"tweetbutton",
				"twttr",
				"update_discoverability",
				"users",
				"welcome",
				"who_to_follow",
				"widgets",
				"zendesk_auth",
				"media_signup"
			},
			PhotoSizes = new Dictionary<string, Size>
			{
				{ "large", new Size { Width = 1024, Height = 2048, Resize = "fit" } },
				{ "medium", new Size { Width = 600, Height = 1200, Resize = "fit" } },
				{ "small", new Size { Width = 340, Height = 480, Resize = "fit" } },
				{ "thumb", new Size { Width = 150, Height = 150, Resize = "crop" } },
			},
			ShortUrlLengthHttps = 23,
			ShortUrlLength = 22,
		};

		public static Configuration Default
		{
			get { return _Default; }
		}

		#endregion

		#region parse methods

		public static bool TryParse(string json, out Configuration error)
		{
			try
			{
				error = ParseCore(DynamicJsonHelper.ToDynamicJson(json));
			}
			catch (Exception ex)
			{
				ex.Write();
				error = null;
			}

			return error != null;
		}

		public static Configuration Parse(string json)
		{
			var djson = DynamicJsonHelper.ToDynamicJson(json);

			try
			{
				return ParseCore(djson);
			}
			catch (Exception ex)
			{
				throw new JsonParseException(json, typeof(Configuration), ex);
			}
		}

		internal static Configuration ParseCore(dynamic djson)
		{
			var result = new Configuration
			{
				CharactersReservedPerMedia = (int)Convert.ToDouble(djson.characters_reserved_per_media),
				MaxMediaPerUpload = (int)Convert.ToDouble(djson.max_media_per_upload),
				NonUsernamePaths = ((object[])djson.non_username_paths).Select(_ => _.ToString()).ToList(),
				PhotoSizeLimit = (int)Convert.ToDouble(djson.photo_size_limit),
				ShortUrlLengthHttps = (int)Convert.ToDouble(djson.short_url_length_https),
				ShortUrlLength = (int)Convert.ToDouble(djson.short_url_length),
			};
			return result;
		}

		#endregion
	}
}
