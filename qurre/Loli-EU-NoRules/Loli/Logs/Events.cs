using HarmonyLib;
using Loli.DataBase.Modules;
using Qurre.API;
using Qurre.Events.Structs;
using System.Linq;
using System.Reflection;

namespace Loli.Logs
{
	[HarmonyPatch]
	internal static class RALogsPatch
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "RemoteAdminCommand");
		}
		internal static bool Prefix(RemoteAdminCommandEvent ev)
		{
			try
			{
				#region list
				if (ev.Name.ToLower() == "list")
				{
					ev.Allowed = false;
					string message = $"{Player.List.Count()}/{Core.MaxPlayers}\n";

					foreach (Player player in Player.List)
						message += $"{player.UserInfomation.Nickname} - {player.UserInfomation.UserId} " +
							$"({player.UserInfomation.Id}) [{player.RoleInfomation.Role}]\n";

					if (string.IsNullOrEmpty(message))
						message = $"No players online";

					ev.Sender.RaReply($"{message}", true, true, string.Empty);
					return false;
				}
				else if (ev.Name.ToLower() == "stafflist")
				{
					ev.Allowed = false;
					bool isStaff = false;
					string names = "";
					foreach (Player player in Player.List)
					{
						if (Data.Users.TryGetValue(player.UserInfomation.UserId, out var _main) &&
							(_main.administration.admin || _main.administration.moderator))
						{
							string role = "";
							if (_main.administration.admin) role = "- Admin";
							else if (_main.administration.moderator) role = "- Moderator";
							isStaff = true;
							names += $"{player.UserInfomation.Nickname} {role}\n";
						}
					}

					string response = isStaff ? names : $"No administration online";
					ev.Sender.RaReply($"{Player.List.Count()}/{Core.MaxPlayers}\n{response}", true, true, string.Empty);
					return false;
				}
				return true;
				#endregion
			}
			catch { return true; }
		}
	}
}