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
				if (roles.Star) return "<color=#f700ff>[Star]</color> ";
				if (roles.Sage) return "<color=#dc143c>[Sage]</color> ";
				if (roles.Mage) return "<color=#98ff98>[Mage]</color> ";
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
				if (main.anonym) return "";
				if (main.administration.owner) return "";
				if (main.administration.admin) return "<color=#fdffbb>[Admin]</color> ";
				if (main.administration.moderator) return "<color=#00ffff>[Moderator]</color> ";
			}
			if (CustomDonates.TryGetRemoteAdminPrefix(main.steam, out var adminPrefix))
			{
				if (main.anonym) return "";
				return $"<color={adminPrefix.Color}>[{adminPrefix.Name}]</color> ";
			}
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
		public string steam;
		public string prefix;

		public Administration administration;

		public int money;
		public int lvl;
		public int xp;
		public int to;

        [Newtonsoft.Json.JsonIgnore]
		public bool donater = false;
		[Newtonsoft.Json.JsonIgnore]
		public bool anonym = false;
	}
	[Serializable]
	public class Administration
	{
		public bool owner;
		public bool admin;
		public bool moderator;
	}
	public class DonateRA
	{
		public bool Force { get; internal set; } = false;
		public bool Give { get; internal set; } = false;
		public bool Effects { get; internal set; } = false;
		public bool ViewRoles { get; internal set; } = false;
	}
#pragma warning disable IDE1006
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
		public string owner { get; set; } = "";
		public int id { get; set; } = 0;
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
		internal bool Mage => _mages > 0;
		internal bool Sage => _sages > 0;
		internal bool Star => _stars > 0;

		internal int _rainbows { get; set; } = 0;
		internal int _primes { get; set; } = 0;
		internal int _mages { get; set; } = 0;
		internal int _sages { get; set; } = 0;
		internal int _stars { get; set; } = 0;
	}
#pragma warning restore IDE1006
}