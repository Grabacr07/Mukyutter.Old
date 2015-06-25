using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Grabacr07.Mukyutter.Models.Twitter.Data;
using Grabacr07.Utilities.Security;

namespace Grabacr07.Mukyutter.Models.Twitter.Settings
{
	[Serializable]
	public class TokenSettings
	{
		public Guid ApplicationId { get; set; }

		public UserId FallbackUserId { get; set; }
		public Guid FallbackApplicationId { get; set; }

		public string TokenKey { get; set; }

		[NonSerialized]
		private string _tokenSecret;

		[XmlIgnore]
		public string TokenSecret
		{
			get { return this._tokenSecret; }
			set
			{
				this._tokenSecret = value;
				this._encryptTokenSecret = value.Encrypt(TokenKey);
			}
		}

		private string _encryptTokenSecret;

		public string EncryptTokenSecret
		{
			get { return this._encryptTokenSecret; }
			set
			{
				this._encryptTokenSecret = value;
				this._tokenSecret = value.Decrypt(TokenKey);
			}
		}
	}
}
