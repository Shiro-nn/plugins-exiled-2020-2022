using Loli.DataBase.Modules;
using Newtonsoft.Json;
using Qurre.API;
using System;
using System.Collections.Generic;

namespace Loli.DataBase
{
	static class Module
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
				if (main.id == 1) return "";
				if (main.maincontrol) return "<color=#ffe000>[Контроль Админов]</color> ";
				if (main.control) return "<color=#ef00ff>[Контроль MRP]</color> ";
				if (main.mainadmin) return "<color=#ff0000>[Глав Админ]</color> ";
				if (main.admin) return "<color=#fdffbb>[Админ]</color> ";
				if (main.mainhelper) return "<color=#0089c7>[Глав Хелпер]</color> ";
				if (main.helper) return "<color=#00ffff>[Хелпер]</color> ";
				if (main.trainee) return "<color=#9bff00>[Стажер]</color> ";
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
	class UserData
	{
		[JsonProperty("donater")]
		public bool donater = false;

		[JsonProperty("trainee")]
		public bool trainee = false;

		[JsonProperty("helper")]
		public bool helper = false;

		[JsonProperty("mainhelper")]
		public bool mainhelper = false;

		[JsonProperty("admin")]
		public bool admin = false;

		[JsonProperty("mainadmin")]
		public bool mainadmin = false;

		[JsonProperty("control")]
		public bool control = false;

		[JsonProperty("maincontrol")]
		public bool maincontrol = false;


		[JsonProperty("name")]
		public string name = "[data deleted]";

		[JsonProperty("id")]
		public int id = 0;

		[JsonProperty("discord")]
		public string discord = "";
	}
	class DonateRA
	{
		public bool Force { get; internal set; } = false;
		public bool Give { get; internal set; } = false;
		public bool Effects { get; internal set; } = false;
		public bool ViewRoles { get; internal set; } = false;
	}
#pragma warning disable IDE1006
	class BDDonateRA
	{
		[JsonProperty("force")]
		public bool force { get; set; } = false;

		[JsonProperty("give")]
		public bool give { get; set; } = false;

		[JsonProperty("effects")]
		public bool effects { get; set; } = false;

		[JsonProperty("players_roles")]
		public bool players_roles { get; set; } = false;

		[JsonProperty("prefix")]
		public string prefix { get; set; } = "";

		[JsonProperty("color")]
		public string color { get; set; } = "";
	}
	class BDDonateRoles
	{
		[JsonProperty("owner")]
		public int owner { get; set; } = 0;

		[JsonProperty("id")]
		public int id { get; set; } = 0;

		[JsonProperty("server")]
		public int server { get; set; } = 0;

		[JsonProperty("freezed")]
		public bool freezed { get; set; } = false;
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