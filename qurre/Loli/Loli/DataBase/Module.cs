using Loli.DataBase.Modules;
using Qurre.API;
using System;
using System.Collections.Generic;

namespace Loli.DataBase
{
	public static class Module
	{
		public static string Prefix(Player pl)
		{
			string userId = pl.UserInfomation.UserId;
			if (Data.Roles.TryGetValue(userId, out var roles))
			{
				if (roles.Priest) return "<color=#ff9898>[Священник]</color> ";
				if (roles.Star) return "<color=#f700ff>[Звездочка]</color> ";
				if (roles.Sage) return "<color=#dc143c>[Мудрец]</color> ";
				if (roles.Mage) return "<color=#98ff98>[Маг]</color> ";
			}
			if (Prefixs.ContainsKey(userId))
			{
				if (Prefixs[userId].color == "")
				{
					return $"[{Prefixs[userId].prefix}] ";
				}
				return $"<color={Prefixs[userId].color}>[{Prefixs[userId].prefix}]</color> ";
			}
			if (Data.Users.TryGetValue(userId, out var main))
			{
				if (main.id == 1 || main.anonym) return "";
				if (main.owner) return "<color=#ffe000>[Контроль Админов]</color> ";
				if (main.mainadmin) return "<color=#ff0000>[Глав Админ]</color> ";
				if (main.admin) return "<color=#fdffbb>[Админ]</color> ";
				if (main.mainhelper) return "<color=#0089c7>[Глав Хелпер]</color> ";
				if (main.helper) return "<color=#00ffff>[Хелпер]</color> ";
				if (main.trainee) return "<color=#9bff00>[Стажер]</color> ";
			}
			if (CustomDonates.TryGetRemoteAdminPrefix(main.id, out var adminPrefix))
			{
				return $"<color={adminPrefix.Color}>[{adminPrefix.Name}]</color> ";
			}
			try { if (Patrol.List.Contains(pl.UserInfomation.UserId)) return ""; } catch { }
			if (pl.Administrative.RemoteAdmin) return "[RA] ";
			return "";
		}
		public static bool GD(CommandSender cs)
		{
			bool result = false;
			string senderId = cs.SenderId;
			if (Prefixs.ContainsKey(senderId))
			{
				result = Prefixs[senderId].gameplay_data;
			}
			if (Data.Roles.TryGetValue(senderId, out var roles) && (roles.Sage || roles.Star)) result = true;
			return result;
		}
		public static Dictionary<string, RaPrefix> Prefixs { get; } = new();
		[Serializable]
		public class RaPrefix
		{
			public string prefix = "RA";
			public string color = "";
			public bool gameplay_data;
		}
	}
	[Serializable]
	public class UserData
	{
		public int money;
		public int xp;
		public int lvl;
		public int to;

		public bool donater = false;
		public bool trainee = false;
		public bool helper = false;
		public bool mainhelper = false;
		public bool admin = false;
		public bool mainadmin = false;
		public bool selection = false;
		public bool owner = false;
		public int warnings = 0;

		public string prefix = "";
		public string clan = "";
		public bool found = false;
		[Newtonsoft.Json.JsonIgnore]
		public bool anonym = false;
		[Newtonsoft.Json.JsonIgnore]
		public DateTime entered = DateTime.Now;
		public string name = "[data deleted]";
		public int id = 0;
		public string discord = "";
		public string login = "";
	}
	public class DonateRA
	{
		public bool Force { get; internal set; } = false;
		public bool Give { get; internal set; } = false;
		public bool Effects { get; internal set; } = false;
		public bool ViewRoles { get; internal set; } = false;
	}
	[Serializable]
	public class Clan
	{
		public string Tag = "";
		public List<int> Users;
		public List<int> Boosts;
	}
#pragma warning disable IDE1006
	public class ClanUsersJson
	{
		public int[] lvl1 { get; set; }
		public int[] lvl2 { get; set; }
		public int[] lvl3 { get; set; }
		public int[] lvl4 { get; set; }
		public int[] lvl5 { get; set; }
	}
	public class ClanBoostsJson
	{
		public int[] boosts { get; set; }
		public int[] available { get; set; }
	}
	public class BDDonateRA
	{
		public bool force { get; set; } = false;
		public bool give { get; set; } = false;
		public bool effects { get; set; } = false;
		public bool players_roles { get; set; } = false;
		public string prefix { get; set; } = "";
		public string color { get; set; } = "";
	}
	public class BDDonateRoles
	{
		public int owner { get; set; } = 0;
		public int id { get; set; } = 0;
		public int server { get; set; } = 0;
		public bool freezed { get; set; } = false;
	}
	public class SocketStatsData
	{
		public int xp { get; set; } = 0;
		public int lvl { get; set; } = 0;
		public int to { get; set; } = 0;
		public int money { get; set; } = 0;
	}
	internal class DonateRoles
	{
		internal bool Rainbow => _rainbows > 0;
		internal bool Prime => _primes > 0;
		internal bool Priest => _priests > 0;
		internal bool Mage => _mages > 0;
		internal bool Sage => _sages > 0;
		internal bool Star => _stars > 0;

		internal int _rainbows { get; set; } = 0;
		internal int _primes { get; set; } = 0;
		internal int _priests { get; set; } = 0;
		internal int _mages { get; set; } = 0;
		internal int _sages { get; set; } = 0;
		internal int _stars { get; set; } = 0;
	}
#pragma warning restore IDE1006
}