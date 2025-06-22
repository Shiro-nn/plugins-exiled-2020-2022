using HarmonyLib;
using Loli.DataBase.Modules;
using Loli.Discord;
using Qurre.API;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Loli.Logs
{
	[HarmonyPatch]
	internal static class Fixes7
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "Ban");
		}

		[HarmonyPrefix]
		internal static bool Prefix(BannedEvent ev)
		{
			try
			{
				if (ev.Type == BanHandler.BanType.IP) return false;
				string target = SCPDiscordLogs.Api.PlayerInfo(ev.Player, false);
				string target_public = Fixes8.GetInfo(ev.Player);
				if (string.IsNullOrEmpty(target)) target = target_public;
				if (string.IsNullOrEmpty(target)) return false;
				string adminNick = ev.Details.Issuer;
				string time = $"<t:{new DateTimeOffset(new DateTime(ev.Details.Expires).AddHours((DateTime.Now - DateTime.UtcNow).TotalHours)).ToUnixTimeSeconds()}:f>";
				try
				{
					string[] IssuerPars = adminNick.Split('(');
					string Issuer = IssuerPars[IssuerPars.Length - 1];
					Issuer = Issuer.Remove(Issuer.Length - 1);
					if (Data.Users.TryGetValue(Issuer, out var _data))
						adminNick = $"<@!{_data.discord}> ({_data.name})";
				}
				catch { }
				SCPDiscordLogs.Api.SendBanOrKick(ev.Details.Reason, target, adminNick, time);
				try { Fixes8.SendHook(ev.Details.Reason, target_public, adminNick, time, true, Fixes8.Hook); } catch { }
				return false;
			}
			catch { return true; }
		}
	}

	[HarmonyPatch]
	internal static class Fixes8
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "Kick");
		}

		[HarmonyPrefix]
		internal static bool Prefix(KickEvent ev)
		{
			try
			{
				string target = SCPDiscordLogs.Api.PlayerInfo(ev.Player, false);
				string target_public = GetInfo(ev.Player);
				if (string.IsNullOrEmpty(target)) target = target_public;
				if (string.IsNullOrEmpty(target)) return false;
				string adminNick = ev.Issuer.UserInfomation.Nickname;
				if (Data.Users.TryGetValue(ev.Issuer.UserInfomation.UserId, out var _data))
					adminNick = $"<@!{_data.discord}> ({_data.name})";
				SCPDiscordLogs.Api.SendBanOrKick(ev.Reason, target, adminNick, "kick");
				try { SendHook(ev.Reason, target_public, adminNick, "kick", false, Hook); } catch { }
				return false;
			}
			catch { return true; }
		}
		internal static string GetInfo(Player pl)
		{
			if (pl == Server.Host) return $"{pl.UserInfomation.Nickname}";
			else
			{
				string nick = pl.UserInfomation.Nickname.Replace("_", "\\_").Replace("*", "\\*").Replace("|", "\\|").Replace("~", "\\~").Replace("`", "\\`")
					.Replace("<@", "\\<\\@").Replace("@", "\\@").Replace("@e", "@е").Replace("@he", "@hе");
				string userid = "";
				if (!pl.UserInfomation.DoNotTrack) userid = $" - {pl.UserInfomation.UserId}";
				return $"{nick}{userid}";
			}
		}
		internal const string Hook = "https://discord.com/api/webhooks/";
		internal const string Avatar = "https://cdn.scpsl.store/scpsl.store/img/etc/scpsl.png";
		internal static void SendHook(string reason, string banned, string banner, string time, bool thisBan, string hook)
		{
			string desc;
			if (thisBan) desc = $"Забанен: {banned}\nЗабанил: {banner}\nПричина: {reason}\nДо {time}";
			else desc = $"Кикнут: {banned}\nКикнул: {banner}\nПричина: {reason}";

			new Webhook(hook).Send("", Core.ServerName, Avatar, false, embeds: new List<Embed>() {
				new()
				{
					Author = new()
					{
						Name = Core.ServerName,
						IconUrl = Avatar
					},
					Title = thisBan ? "Бан" : "Кик",
					Color = thisBan ? 16711680 : 16776960,
					Description = desc,
					Footer = new() { Text = "© Qurre Team" },
					TimeStamp = DateTime.Now
				}
			});
		}
	}

	[HarmonyPatch]
	internal static class Fixes9
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "SendingConsole");
		}

		[HarmonyPrefix]
		internal static bool Prefix(GameConsoleCommandEvent ev) => ev.Name != "patrol";
	}
}