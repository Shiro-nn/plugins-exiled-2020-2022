using MapGeneration;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events.Structs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loli.Addons
{
    static class Pocket
	{
		static Pocket()
		{
			CommandsSystem.RegisterConsole("pocket", Console);
		}

		static void Console(GameConsoleCommandEvent ev)
		{
			if (ev.Name != "pocket")
				return;

			ev.Allowed = false;
			ev.Reply = "Еще не изобрели";
			ev.Color = "red";
		}
	}
}