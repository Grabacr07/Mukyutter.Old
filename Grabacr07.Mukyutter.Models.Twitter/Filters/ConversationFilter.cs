using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.Mukyutter.Models.Twitter.Data;

namespace Grabacr07.Mukyutter.Models.Twitter.Filters
{
	public class ConversationFilter : TimelineFilter
	{
		private Conversation conversation;

		public ConversationFilter(Conversation owner)
		{
			this.conversation = owner;
		}

		//public Func<IEnumerable<Status>> GetStatuses { get; set; }

		public override bool Predicate(Status status)
		{
			return this.conversation.Statuses.Any(s => Predicate(status, s));
		}

		private static bool Predicate(Status source, Status target)
		{
			return (source.InReplyToStatusId.HasValue &&
					source.InReplyToStatusId.Value == target.Id) ||
				   (target.InReplyToStatusId.HasValue &&
					target.InReplyToStatusId.Value == source.Id) ||
				   source.ReplyFrom.Any(s => s.Id == target.Id) ||
				   target.ReplyFrom.Any(s => s.Id == source.Id);
		}
	}
}
