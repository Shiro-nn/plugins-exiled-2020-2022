using System;
using System.Collections.Generic;

namespace MultiPlugin22.Collections.Chat
{
	public class Message
	{
		public Player Sender { get; set; }
		public List<Player> Targets { get; set; }
		public string Content { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
