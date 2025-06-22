using EXILED.Extensions;
using System.Collections.Generic;
using MultiPlugin22.Enums;
using MultiPlugin22.Extensions;
using MultiPlugin22.Interfaces;
using static MultiPlugin22.Database;

namespace MultiPlugin22.Commands.Console
{
	public class PrivateChat : Chat, ICommand
	{
		public PrivateChat() : base(ChatRoomType.Private, Configs.privateMessageColor)
		{ }

		public string Description => Configs.l23;

		public string Usage => $".chat_private [Nick/SteamID64/PlayerID] [{Configs.msg}]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(1), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][{Configs.privatet}]: {message}";

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return (Configs.l9.Replace("%player%", $"{target.GetNickname()}"), "red");
			else if (sender == target) return (Configs.l24, "red");
			else if (!Configs.canSpectatorSendMessagesToAlive && sender.GetTeam() == Team.RIP && target.GetTeam() != Team.RIP)
			{
				return (Configs.l25, "red");
			}

			if (Configs.saveChatToDatabase) SaveMessage(message, ChatPlayers[sender], new List<Collections.Chat.Player>() { ChatPlayers[sender] }, type);

			((CharacterClassManager)target.characterClassManager).TargetConsolePrint(((Mirror.NetworkBehaviour)target.scp079PlayerScript).connectionToClient, message, color);
			if (Configs.showPrivateMessageNotificationBroadcast)
			{
				target.ClearBroadcasts();
				target.Broadcast(10, $"<size=20><color={color}>{message}</color></size>", false);
			}

			return (message, color);
		}
	}
}
