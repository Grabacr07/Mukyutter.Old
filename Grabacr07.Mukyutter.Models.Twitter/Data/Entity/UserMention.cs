using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Entity
{
	public class UserMention : IEntity
	{
		public UserId Id { get; internal set; }

		public User User
		{
			get { return TwitterClient.Current.Users[this.Id] ?? User.Empty; }
		}

		public Indices Indices { get; internal set; }
	}
}
