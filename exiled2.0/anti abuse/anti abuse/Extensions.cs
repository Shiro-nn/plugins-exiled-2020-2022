using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
namespace anti_abuse
{
	public static class Extensions
	{
		public static Team GetTeam(this RoleType roleType)
		{
			switch (roleType)
			{
				case RoleType.ChaosInsurgency:
					return Team.CHI;
				case RoleType.Scientist:
					return Team.RSC;
				case RoleType.ClassD:
					return Team.CDP;
				case RoleType.Scp049:
				case RoleType.Scp93953:
				case RoleType.Scp93989:
				case RoleType.Scp0492:
				case RoleType.Scp079:
				case RoleType.Scp096:
				case RoleType.Scp106:
				case RoleType.Scp173:
					return Team.SCP;
				case RoleType.Spectator:
					return Team.RIP;
				case RoleType.FacilityGuard:
				case RoleType.NtfCadet:
				case RoleType.NtfLieutenant:
				case RoleType.NtfCommander:
				case RoleType.NtfScientist:
					return Team.MTF;
				case RoleType.Tutorial:
					return Team.TUT;
				default:
					return Team.RIP;
			}
		}
		public static bool CheckRole(this Player pl)
		{
			bool _ = false;
			if (Events.WhiteList.Contains(pl.UserId)) return false;
			try { _ = Roles().Contains(pl.Group.BadgeText); } catch { }
			return _;
		}
		public static List<string> Roles()
		{
			string _ = Plugin.Cfg.GetString("anti_abuse_roles", "");
			string[] str = _.Split(',');
			List<string> strl = new List<string>();
			foreach (string st in str) strl.Add(st.Trim());
			return strl;
		}
	}
}