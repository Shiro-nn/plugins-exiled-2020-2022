using Qurre.API;
using System;
using System.Linq;
namespace PlayerXP
{
	public static class Extensions
	{
		internal static void SetRank(this Player player, string rank, string color = "default")
		{
			player.ReferenceHub.serverRoles.Network_myText = rank;
			player.ReferenceHub.serverRoles.Network_myColor = color;
		}
		internal static UserGroup GetGroup(this Player pl)
        {
			if (pl.AdminSearch()) return pl.Group;
			if (!auto_donate.EventHandlers.Donates.TryGetValue(pl.UserId, out var _data)) return null;
			return ServerStatic.GetPermissionsHandler().GetGroup(_data);
		}
		internal static string GetGroupName(this UserGroup group) => ServerStatic.GetPermissionsHandler().GetAllGroups().Where(p => p.Value == group).Select(p => p.Key).FirstOrDefault();
		internal static bool AdminSearch(this Player hub)
		{
			string group = hub.Group.GetGroupName();
			return !string.IsNullOrEmpty(group);
		}
		internal static string Prefix(this int lvl)
		{
			string prfx = Configs.Prefixs;
			if (prfx.Contains(','))
			{
				var array = prfx.Split(',');
				foreach (string txt in array.Where(x => x.Contains(':')))
				{
					try
					{
						var array2 = txt.Split(':');
						if (Convert.ToInt32(array2[0]) == lvl) return $" | {array2[1]}";
					}
					catch { }
				}
			}
			else if (prfx.Contains(':'))
			{
				try
				{
					var array2 = prfx.Split(':');
					if (Convert.ToInt32(array2[0]) == lvl) return $" | {array2[1]}";
				}
				catch { }
			}
			return "";
		}
	}
}