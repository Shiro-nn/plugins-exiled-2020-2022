using Qurre.API;
using System;
using System.Linq;

namespace PlayerXP
{
	static class Extensions
	{
		static internal string GetGroupName(this UserGroup group)
			=> ServerStatic.GetPermissionsHandler().GetAllGroups().Where(p => p.Value == group).Select(p => p.Key).FirstOrDefault();

		static internal void SetRank(this Player player, string rank, string color = "default")
		{
			player.Administrative.RoleName = rank;
			player.Administrative.RoleColor = color;
		}
		static internal bool Adminsearch(this Player hub)
		{
			string group = hub.Administrative.Group.GetGroupName();
			return !string.IsNullOrEmpty(group);
		}

		static internal string Prefix(this int lvl)
		{
			string prfx = Cfg.Prefixs;
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