using HarmonyLib;
using Loli.Addons;
using Loli.Discord;
using Qurre.API;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Loli.Logs
{
	[HarmonyPatch]
	internal static class Fixes1
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "AlphaStart");
		}
		internal static bool Prefix()
		{
			if (OmegaWarhead.InProgress)
			{
				SCPDiscordLogs.Api.SendMessage(":radioactive: **Omega Warhead will explode in 3 minutes.**");
				return false;
			}
			else return true;
		}
	}

	[HarmonyPatch]
	internal static class Fixes2
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "Detonate");
		}
		internal static bool Prefix()
		{
			if (OmegaWarhead.InProgress)
			{
				SCPDiscordLogs.Api.SendMessage(":radioactive: **Omega Warhead successfully detonated.**");
				return false;
			}
			else return true;
		}
	}

	[HarmonyPatch]
	internal static class Fixes7
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "Ban");
		}
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
		internal static bool Prefix(KickEvent ev)
		{
			try
			{
				string target = SCPDiscordLogs.Api.PlayerInfo(ev.Player, false);
				string target_public = GetInfo(ev.Player);
				if (string.IsNullOrEmpty(target)) target = target_public;
				if (string.IsNullOrEmpty(target)) return false;
				string adminNick = ev.Issuer.UserInfomation.Nickname;
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
			if (thisBan) desc = $"Banned: {banned}\nBy: {banner}\nReason: {reason}\nExpires: {time}";
			else desc = $"Kicked: {banned}\nBy: {banner}\nReason: {reason}";

			new Webhook(hook).Send("", Core.ServerName, Avatar, false, embeds: new List<Embed>() {
				new()
				{
					Author = new()
					{
						Name = Core.ServerName,
						IconUrl = Avatar
					},
					Title = thisBan ? "Ban" : "Kick",
					Color = thisBan ? 16711680 : 16776960,
					Description = desc,
					TimeStamp = DateTime.Now
				}
			});
		}
	}
}