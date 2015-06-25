using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grabacr07.Mukyutter.Models.Twitter.Data.Entity
{
	public class Hashtag : IEntity
	{
		public string Text { get; internal set; }

		public Indices Indices { get; internal set; }
	}
}
