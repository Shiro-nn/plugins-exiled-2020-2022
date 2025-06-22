using EXILED.Extensions;
using System;
using MultiPlugin22.Extensions;
using MultiPlugin22.Interfaces;
using static MultiPlugin22.Database;

namespace MultiPlugin22.Commands.RemoteAdmin
{
	public class Unmute : ICommand
	{
		public string Description => Configs.l6;

		public string Usage => ".chat_unmute [SteamID64/UserID/Nick]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (!sender.CheckPermission("tc.unmute")) return (Configs.l7, "red");

			if (args.Length != 1) return ($"{Configs.l8} {Usage}", "red");

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return (Configs.l9.Replace("%player%", $"{args[0]}"), "red");

			var mutedPlayer = LiteDatabase.GetCollection<Collections.Chat.Mute>().FindOne(mute => mute.Target.Id == target.GetRawUserId() && mute.Expire > DateTime.Now);

			if (mutedPlayer == null) return (Configs.l10.Replace("%player%", $"{target.GetNickname()}"), "red");

			mutedPlayer.Expire = DateTime.Now;

			LiteDatabase.GetCollection<Collections.Chat.Mute>().Update(mutedPlayer);

			target.Broadcast(10, Configs.l11, false);

			return (Configs.l12.Replace("%player%", $"{target.GetNickname()}"), "green");
		}
	}
}
