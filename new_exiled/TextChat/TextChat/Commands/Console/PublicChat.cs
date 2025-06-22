using EXILED.Extensions;
using System.Collections.Generic;
using System.Linq;
using MultiPlugin22.Enums;
using MultiPlugin22.Extensions;
using MultiPlugin22.Interfaces;
using static MultiPlugin22.Database;
namespace MultiPlugin22.Commands.Console
{
	public class PublicChat : Chat, ICommand
	{
		public PublicChat() : base(ChatRoomType.Public, Configs.generalChatColor)
		{ }

		public string Description => Configs.l22;

		public string Usage => $".chat [{Configs.msg}]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][{Configs.publict}]: {message}";

			IEnumerable<ReferenceHub> targets = Player.GetHubs().Where(target =>
			{
				return sender != target && (Configs.canSpectatorSendMessagesToAlive || sender.GetTeam() != Team.RIP || target.GetTeam() == Team.RIP);
			});

			List<Collections.Chat.Player> chatPlayers = targets.GetChatPlayers(ChatPlayers);

			if (chatPlayers.Count == 0) return (Configs.l21, "red");

			if (Configs.saveChatToDatabase) SaveMessage(message, ChatPlayers[sender], chatPlayers, type);
			Map.Broadcast($"<size=20><color={color}>{message}</color></size>", 5);
			targets.SendConsoleMessage(message, color);

			return (message, color);
		}
	}
}
