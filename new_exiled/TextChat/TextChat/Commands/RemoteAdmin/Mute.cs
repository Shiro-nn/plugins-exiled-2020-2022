using EXILED.Extensions;
using System;
using System.Linq;
using MultiPlugin22.Extensions;
using MultiPlugin22.Interfaces;
using static MultiPlugin22.Database;

namespace MultiPlugin22.Commands.RemoteAdmin
{
	public class Mute : ICommand
	{
		public string Description => Configs.l13;

		public string Usage => $"chat_mute [PlayerID/SteamID64/Ник] [{Configs.min}] [{Configs.reason}]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (!sender.CheckPermission("tc.mute")) return (Configs.l7, "red");

			if (args.Length < 2) return ($"{Configs.l14} {Usage}", "red");

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return (Configs.l9.Replace("%player%", $"{args[0]}"), "red");

			if (!double.TryParse(args[1], out double duration) || duration < 1) return (Configs.l15.Replace("%time%", $"{args[1]}"), "red");

			string reason = string.Join(" ", args.Skip(2).Take(args.Length - 2));

			if (string.IsNullOrEmpty(reason)) return (Configs.l16, "red");

			if (target.IsChatMuted()) return (Configs.l17.Replace("%player%", $"{target.GetNickname()}"), "red");

			LiteDatabase.GetCollection<Collections.Chat.Mute>().Insert(new Collections.Chat.Mute()
			{
				Target = ChatPlayers[target],
				Issuer = ChatPlayers[sender],
				Reason = reason,
				Timestamp = DateTime.Now,
				Expire = DateTime.Now.AddMinutes(duration)
			});

			if (Configs.showChatMutedBroadcast)
			{
				target.ClearBroadcasts();
				target.Broadcast(Configs.chatMutedBroadcastDuration, string.Format(Configs.chatMutedBroadcast, duration, reason), true);
			}

			target.Broadcast(10, Configs.l18.Replace("%duration%", $"{duration} {Configs.min}{(duration != 1 ? Configs.sec : "")}").Replace("%reason%",$"{reason}"), false);

			return (Configs.l19.Replace("%player%", $"{target.GetNickname()}").Replace("%duration%", $"{duration} {Configs.min}{(duration != 1 ? Configs.sec : "")}").Replace("%reason%", $"{reason}"), "green");
		}
	}
}
