using MongoDB.Bson;
using Qurre.API;
using System;
using System.Collections.Generic;
namespace Loli.DataBase
{
	public static class Module
	{
		public static string Prefix(Player pl)
		{
			string userId = pl.UserId;
			if (Manager.Static.Data.Roles.TryGetValue(userId, out var roles))
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
			if (Manager.Static.Data.Users.TryGetValue(userId, out var main))
			{
				if (main.or || main.anonym)
				{
					return "";
				}
				if (main.gar)
				{
					return "<color=#ff0000>[MainAdmin]</color> ";
				}
				if (main.ar)
				{
					return "<color=#fdffbb>[Admin]</color> ";
				}
				if (main.ghr)
				{
					return "<color=#0089c7>[MainHelper]</color> ";
				}
				if (main.hr)
				{
					return "<color=#00ffff>[Helper]</color> ";
				}
				if (main.sr)
				{
					return "<color=#9bff00>[Trainee]</color> ";
				}
			}
			if (Modules.CustomDonates.TryGetRemoteAdminPrefix(main.id, out var adminPrefix))
			{
				return $"<color={adminPrefix.Color}>[{adminPrefix.Name}]</color> ";
			}
			if (pl.RemoteAdminAccess) return "[RA] ";
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
			if (Manager.Static.Data.Roles.TryGetValue(senderId, out var roles) && (roles.Sage || roles.Star)) result = true;
			return result;
		}
		public static Dictionary<string, ra_pref> Prefixs { get; set; } = new Dictionary<string, ra_pref>();
		[Serializable]
		public class ra_pref
		{
			public string prefix = "RA";
			public string color = "";
			public bool gameplay_data;
		}
	}
	public class EffectsSender : CommandSender
	{
		public override void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
		{
			Qurre.Log.Debug($"{Nickname}: {Command} ^ {text}");
		}

		public override void Print(string text)
		{
			Qurre.Log.Debug($"{Nickname}: {Command} ^ {text}");
		}

		public string Name;
		public string Command;
		public EffectsSender(string name, string com)
		{
			Name = name;
			Command = com;
		}
		public override string SenderId => "Effects Controller";
		public override string Nickname => Name;
		public override ulong Permissions => ServerStatic.GetPermissionsHandler().FullPerm;
		public override byte KickPower => byte.MinValue;
		public override bool FullPermissions => true;
	}
	[Serializable]
	public class UserData
	{
		public string UserId;
		public int money;
		public int xp;
		public int lvl;
		public int to;
		public bool ytr = false;
		public bool don = false;
		public bool sr = false;
		public bool hr = false;
		public bool ghr = false;
		public bool ar = false;
		public bool gar = false;
		public bool asr = false;
		public bool dcr = false;
		public bool or = false;

		public string prefix = "";
		public string clan = "";
		public bool find = false;
		public bool anonym = false;
		public int admintime = 0;
		public DateTime now;
		public string warns = "";
		public string name = "[data deleted]";
		public int id = 0;
		public string discord = "";
	}
	[Serializable]
	public class Ra_Cfg
	{
		public string UserId;
		public bool force = false;
		public bool give = false;
		public bool effects = false;
		public bool players_roles = false;
		public DateTime now = DateTime.Now;
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
	internal class User_Stats
	{
		public ObjectId _id { get; set; }
		public string steam { get; set; } = "";
		public string discord { get; set; } = "";
		public string[] ips { get; set; } = new string[0];
		public int money { get; set; } = 0;
		public int lvl { get; set; } = 1;
		public int xp { get; set; } = 0;
		public int to { get; set; } = 750;
		public long __v { get; set; } = 0;
	}
	public class BDDonateRoles
	{
		public int owner { get; set; } = 0;
		public int id { get; set; } = 0;
		public int server { get; set; } = 0;
		public bool freezed { get; set; } = false;

		public int sum { get; set; } = 0;
		public long expires { get; set; } = 0;
		public long freeze_start { get; set; } = 0;
		public string _id { get; set; } = "";
		public int __v { get; set; } = 0;
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