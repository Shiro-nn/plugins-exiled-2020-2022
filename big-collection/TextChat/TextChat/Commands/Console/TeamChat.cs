
using EXILED.Extensions;
using System.Collections.Generic;
using System.Linq;
using TextChat.Enums;
using TextChat.Extensions;
using TextChat.Interfaces;
using static TextChat.Database;

namespace TextChat.Commands.Console
{
	public class TeamChat : Chat, ICommand
	{
		public TeamChat() : base(ChatRoomType.Team)
		{ }

		public string Description => "Отправляет сообщение в чат вашей команде.";

		public string Usage => ".chat_team [Сообщение]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][TEAM ({sender.GetRole().ToString().ToUpper()})]: {message}";

			IEnumerable<ReferenceHub> targets = Player.GetHubs().Where(chatPlayer => chatPlayer != sender && chatPlayer.GetTeam() == sender.GetTeam());
			List<Collections.Chat.Player> chatTargets = targets.GetChatPlayers(ChatPlayers);

			if (chatTargets.Count == 0) return ("Нет доступных игроков для общения!", "red");

			color = sender.GetColor();

			if (Configs.saveChatToDatabase) SaveMessage(message, ChatPlayers[sender], chatTargets, type);
			if (sender.GetTeam() == Team.MTF)
			{
				ReferenceHub se = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfScientist).FirstOrDefault();
				ReferenceHub sew = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfLieutenant).FirstOrDefault();
				ReferenceHub sea = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfCommander).FirstOrDefault();
				ReferenceHub ses = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfCadet).FirstOrDefault();
				ReferenceHub sed = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.FacilityGuard).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sew.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sea.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				ses.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sed.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.CHI)
			{
				ReferenceHub se = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ChaosInsurgency).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.CDP)
			{
				ReferenceHub se = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.RSC)
			{
				ReferenceHub se = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scientist).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.TUT)
			{
				ReferenceHub se = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp173).FirstOrDefault();
				ReferenceHub sew = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
				ReferenceHub sea = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp049).FirstOrDefault();
				ReferenceHub ses = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp079).FirstOrDefault();
				ReferenceHub sed = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp096).FirstOrDefault();
				ReferenceHub seq = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Tutorial).FirstOrDefault();
				ReferenceHub sez = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp0492).FirstOrDefault();
				ReferenceHub sex = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93953).FirstOrDefault();
				ReferenceHub sec = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93989).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sew.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sea.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				ses.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sed.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				seq.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sez.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sex.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sec.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.SCP)
			{
				ReferenceHub se = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp173).FirstOrDefault();
				ReferenceHub sew = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
				ReferenceHub sea = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp049).FirstOrDefault();
				ReferenceHub ses = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp079).FirstOrDefault();
				ReferenceHub sed = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp096).FirstOrDefault();
				ReferenceHub seq = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Tutorial).FirstOrDefault();
				ReferenceHub sez = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp0492).FirstOrDefault();
				ReferenceHub sex = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93953).FirstOrDefault();
				ReferenceHub sec = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93989).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sew.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sea.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				ses.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sed.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				seq.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sez.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sex.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sec.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetRole() == RoleType.Spectator)
			{
				ReferenceHub se = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>",  false);
			}

			targets.SendConsoleMessage(message, color);

			return (message, color);
		}
	}
}
