using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.Models.Twitter.Notifications
{
	public class Message
	{
		public string BaseMessage { get; private set; }
		public object AdditionalData1 { get; private set; }
		public object AdditionalData2 { get; private set; }

		public Message(string message)
			: this(message, null, null) { }

		public Message(string format, params object[] args)
			: this(string.Format(format, args), null, null) { }

		public Message(string message, object data1)
			: this(message, data1, null) { }

		public Message(string message, object data1, object data2)
		{
			this.BaseMessage = message;
			this.AdditionalData1 = data1;
			this.AdditionalData2 = data2;
		}

		public override string ToString()
		{
			return this.BaseMessage;
		}

		public static readonly Message Empty = new Message("");
	}

	public static class MessageExtensions
	{
		public static bool IsEmpty(this Message message)
		{
			return message == null || string.IsNullOrEmpty(message.BaseMessage);
		}
	}
}
