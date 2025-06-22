using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Linq;

namespace Loli.DataBase.Modules
{
	static class Admins
	{

		[EventMethod(ServerEvents.RequestPlayerListCommand)]
		static void Prefixs(RequestPlayerListCommandEvent ev)
		{
			bool gameplayData = Module.GD(ev.Sender) || CheckPerms(ev.Sender, PlayerPermissions.GameplayData);
			string text = "\n";
			foreach (Player pl in Player.List.Where(x => x != null).OrderBy(x => x.UserInfomation.Id))
			{
				string nick = pl.UserInfomation.Nickname.Replace("\n", string.Empty);
				string dn = pl.UserInfomation.DisplayName?.Trim();
				if (dn != null && dn != nick && dn != "") nick = $"{dn}<color=#855439>*</color> ({nick})";
				string nickname = $"({pl.UserInfomation.Id}) {nick}";
				if (gameplayData)
				{
					string color = "white";
					var team = pl.GetTeam();
					if (pl.GamePlay.Overwatch)
					{
						color = "#00d7ff";
						try { if (Patrol.List.Contains(pl.UserInfomation.UserId)) color = "white"; } catch { }
					}
					else if (pl.RoleInfomation.Role == RoleTypeId.ClassD) color = "#ff9900";
					else if (pl.RoleInfomation.Role == RoleTypeId.Scientist) color = "#e2e26d";
					else if (pl.RoleInfomation.Role == RoleTypeId.Tutorial) color = "#00ff00";
					else if (pl.RoleInfomation.Role == RoleTypeId.ChaosConscript) color = "#58be58";
					else if (pl.RoleInfomation.Role == RoleTypeId.ChaosMarauder) color = "#23be23";
					else if (pl.RoleInfomation.Role == RoleTypeId.ChaosRepressor) color = "#38ac38";
					else if (pl.RoleInfomation.Role == RoleTypeId.ChaosRifleman) color = "#1cac1c";
					else if (pl.RoleInfomation.Role == RoleTypeId.FacilityGuard) color = "#afafa1";
					else if (pl.RoleInfomation.Role == RoleTypeId.NtfPrivate) color = "#00a5ff";
					else if (pl.RoleInfomation.Role == RoleTypeId.NtfCaptain) color = "#0200ff";
					else if (pl.RoleInfomation.Role == RoleTypeId.NtfSergeant) color = "#0074ff";
					else if (pl.RoleInfomation.Role == RoleTypeId.NtfSpecialist) color = "#1f7fff";
					else if (team == Team.SCPs) color = "#ff0000";
					nickname = $"<color={color}>{nickname}</color>";
				}
				try
				{
					string prefix = Module.Prefix(pl);
					text += $"{prefix}{nickname}\n";
				}
				catch
				{
					text += $"{nickname}\n";
				}
			}
			ev.Sender.RaReply($"$0 {text}".Replace("RA_", string.Empty), true, false, string.Empty);
			ev.Allowed = false;
			static bool CheckPerms(CommandSender commandSender, PlayerPermissions perms)
			{
				return (ServerStatic.IsDedicated && commandSender.FullPermissions) ||
						PermissionsHandler.IsPermitted(commandSender.Permissions, perms);
			}
		}

		[EventMethod(PlayerEvents.Ban, int.MinValue)]
		static void Ban(BanEvent ev)
		{
			if (ev.Issuer.UserInfomation.Nickname != "Dedicated Server")
			{
				try
				{
					if (Data.Users.TryGetValue(ev.Issuer.UserInfomation.UserId, out var _data))
						Core.Socket.Emit("database.admin.ban", new object[] { _data.id, 1 });
				}
				catch { }
				try
				{
					if (Patrol.Verified.Contains(ev.Issuer.UserInfomation.UserId))
						return;
				}
				catch { }
				string reason = "";
				if (ev.Reason != "") reason = $"<color=#ff0000>Причина</color>: {ev.Reason}";
				Map.Broadcast($"<size=30%><color=#6f6f6f><color=#ff0000>{ev.Issuer.UserInfomation.Nickname}</color> забанил <color=#ff0000>{ev.Player.UserInfomation.Nickname}</color> " +
					$"до <color=#ff0000>{ev.Expires:dd.MM.yyyy HH:mm}</color>. {reason}</color></size>", 15);
			}
		}

		[EventMethod(PlayerEvents.Kick, int.MinValue)]
		static void Kick(KickEvent ev)
		{
			try
			{
				if (Data.Users.TryGetValue(ev.Issuer.UserInfomation.UserId, out var _data))
					Core.Socket.Emit("database.admin.kick", new object[] { _data.id, 1 });
			}
			catch { }
			try
			{
				if (Patrol.Verified.Contains(ev.Issuer.UserInfomation.UserId))
					return;
			}
			catch { }
			string reason = "";
			if (ev.Reason != "") reason = $"<color=#ff0000>Причина</color>: {ev.Reason}";
			Map.Broadcast($"<size=30%><color=#6f6f6f><color=#ff0000>{ev.Issuer.UserInfomation.Nickname}</color> кикнул <color=#ff0000>{ev.Player.UserInfomation.Nickname}</color>. {reason}</color></size>", 15);
		}
	}
}