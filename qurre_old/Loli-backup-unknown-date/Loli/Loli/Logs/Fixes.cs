using HarmonyLib;
using Loli.Addons;
using Loli.Discord;
using Qurre.API;
using Qurre.API.Events;
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
				SCPDiscordLogs.Api.SendMessage(":radioactive: **Омега-Боеголовка взорвется через 3 минуты.**");
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
			return AccessTools.Method(type, "Detonation");
		}
		internal static bool Prefix()
		{
			if (OmegaWarhead.InProgress)
			{
				SCPDiscordLogs.Api.SendMessage(":radioactive: **Омега-Боеголовка успешно взорвана.**");
				return false;
			}
			else return true;
		}
	}
	[HarmonyPatch]
	internal static class Fixes4
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "Ally");
		}
		internal static bool Prefix(ref bool __result, Player pl, Player target)
		{
			try
			{
				var targetTeam = target.Team;
				if (Fixes6.Cached.TryGetValue(target, out var role)) targetTeam = role.GetTeam();
				if (targetTeam == pl.GetTeam()) __result = true;
				else if ((targetTeam == Team.SCP || targetTeam == Team.TUT) && (pl.GetTeam() == Team.SCP || pl.GetTeam() == Team.TUT)) __result = true;
				else if ((targetTeam == Team.MTF || targetTeam == Team.RSC) && (pl.GetTeam() == Team.MTF || pl.GetTeam() == Team.RSC)) __result = true;
				else if ((targetTeam == Team.CHI || targetTeam == Team.CDP) && (pl.GetTeam() == Team.CHI || pl.GetTeam() == Team.CDP)) __result = true;
				else __result = false;
				return false;
			}
			catch { return true; }
		}
	}
	[HarmonyPatch(typeof(SCPDiscordLogs.Api), "PlayerInfo")]
	internal static class Fixes5
	{
		internal static bool Prefix(Player pl, bool role, ref string __result)
		{
			try
			{
				if (pl == Server.Host) __result = $"{pl.Nickname}";
				else
				{
					string nick = pl.Nickname.Replace("_", "\\_").Replace("*", "\\*").Replace("|", "\\|").Replace("~", "\\~").Replace("`", "\\`")
						.Replace("<@", "\\<\\@").Replace("@", "\\@").Replace("@e", "@е").Replace("@he", "@hе");
					if (role)
					{
						try
						{
							var _role = pl.GetCustomRole();
							if ((_role == Module.RoleType.Spectator || _role == Module.RoleType.None) && Fixes6.Cached.TryGetValue(pl, out var __role)) _role = __role;
							__result = $"{nick} - {pl.UserId} ({_role})";
						}
						catch { __result = $"{nick} - {pl.UserId}"; }
					}
					else __result = $"{nick} - {pl.UserId}";
				}
				return false;
			}
			catch { return false; }
		}
	}
	[HarmonyPatch]
	internal static class Fixes6
	{
		internal static Dictionary<Player, Module.RoleType> Cached = new();
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "Dies");
		}
		internal static bool Prefix(DiesEvent ev)
		{
			try
			{
				if (Cached.ContainsKey(ev.Target)) Cached.Remove(ev.Target);
				Cached.Add(ev.Target, ev.Target.GetCustomRole());
				return false;
			}
			catch { return true; }
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
				string target = SCPDiscordLogs.Api.PlayerInfo(ev.Player, false);
				string target_public = Fixes8.GetInfo(ev.Player);
				if (string.IsNullOrEmpty(target)) target = target_public;
				if (string.IsNullOrEmpty(target)) return false;
				string adminNick = ev.Details.Issuer;
				try
				{
					string[] IssuerPars = adminNick.Split('(');
					string Issuer = IssuerPars[IssuerPars.Length - 1];
					Issuer = Issuer.Remove(Issuer.Length - 1);
					if (DataBase.Manager.Static.Data.Users.TryGetValue(Issuer, out var _data)) adminNick = $"<@!{_data.discord}> ({_data.name})";
				}
				catch { }
				string time = new DateTime(ev.Details.Expires).AddHours((DateTime.Now - DateTime.UtcNow).TotalHours).ToString("dd.MM.yyyy HH:mm");
				SCPDiscordLogs.Api.SendBanOrKick(ev.Details.Reason, target, adminNick, time);
				try { Fixes8.SendHook(ev.Details.Reason, target_public, adminNick, time, true); } catch { }
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
				string target = SCPDiscordLogs.Api.PlayerInfo(ev.Target, false);
				string target_public = GetInfo(ev.Target);
				if (string.IsNullOrEmpty(target)) target = target_public;
				if (string.IsNullOrEmpty(target)) return false;
				string adminNick = ev.Issuer.Nickname;
				if (DataBase.Manager.Static.Data.Users.TryGetValue(ev.Issuer.UserId, out var _data)) adminNick = $"<@!{_data.discord}> ({_data.name})";
				SCPDiscordLogs.Api.SendBanOrKick(ev.Reason, target, adminNick, "kick");
                try { SendHook(ev.Reason, target_public, adminNick, "kick", false); } catch { }
				return false;
			}
			catch { return true; }
		}
		internal static string GetInfo(Player pl)
		{
			if (pl == Server.Host) return $"{pl.Nickname}";
			else
			{
				string nick = pl.Nickname.Replace("_", "\\_").Replace("*", "\\*").Replace("|", "\\|").Replace("~", "\\~").Replace("`", "\\`")
					.Replace("<@", "\\<\\@").Replace("@", "\\@").Replace("@e", "@е").Replace("@he", "@hе");
				string userid = "";
				if (!pl.DoNotTrack) userid = $" - {pl.UserId}";
				return $"{nick}{userid}";
			}
		}
		internal const string Hook = "https://discord.com/api/webhooks";
		internal static void SendHook(string reason, string banned, string banner, string time, bool thisBan)
		{
			Webhook webhk = new(Hook);
			List<Embed> listEmbed = new();

			EmbedAuthor author = new();
			author.Name = Plugin.ServerName;
			author.IconUrl = "https://cdn.scpsl.store/scpsl.store/img/etc/scpsl.png";

            string desc;
            if (thisBan) desc = $"Забанен: {banned}\nЗабанил: {banner}\nПричина: {reason}\nДо {time}";
			else desc = $"Кикнут: {banned}\nКикнул: {banner}\nПричина: {reason}";

			Embed embed = new();
			embed.Author = author;
			embed.Title = thisBan ? "Бан" : "Кик";
			embed.Color = thisBan ? 16711680 : 16776960;
			embed.Description = desc;
			embed.Footer = new() { Text = "© Qurre Team" };
			embed.TimeStamp = DateTime.Now;

			listEmbed.Add(embed);

			webhk.Send("", Plugin.ServerName, author.IconUrl, false, embeds: listEmbed);
		}
	}
}