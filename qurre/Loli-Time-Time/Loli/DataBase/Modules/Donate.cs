using Loli.Addons;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using Respawning;
using System;
using System.Linq;

namespace Loli.DataBase.Modules
{
	static class Donate
	{
		internal static int DonateLimint => 5;
		static Donate()
		{
			CommandsSystem.RegisterRemoteAdmin("hidetag", AntiHideTag);
		}
		static internal DateTime LastCall = DateTime.Now;
		static internal Faction LastFaction = Faction.Unclassified;
		static void AntiHideTag(RemoteAdminCommandEvent ev)
		{
			ev.Prefix = "HIDETAG";
			ev.Allowed = false;
			ev.Success = false;
			ev.Reply = "Недоступно";
		}
	}
}