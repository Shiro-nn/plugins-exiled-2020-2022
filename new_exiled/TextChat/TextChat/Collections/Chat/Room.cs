using LiteDB;
using MultiPlugin22.Enums;

namespace MultiPlugin22.Collections.Chat
{
	public class Room
	{
		public ObjectId Id { get; set; }
		public Message Message { get; set; }
		public ChatRoomType Type { get; set; }
	}
}
