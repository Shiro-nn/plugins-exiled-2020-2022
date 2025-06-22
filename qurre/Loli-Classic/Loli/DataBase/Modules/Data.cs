using Qurre.API.Attributes;
using Qurre.Events;
using System;
using System.Collections.Generic;

namespace Loli.DataBase.Modules
{
	static class Data
	{
		static internal readonly CustomDictionary<string, UserData> Users = new();
		static internal readonly Dictionary<string, DonateRA> Donates = new();
		static internal readonly Dictionary<string, DonateRoles> Roles = new();
		static internal readonly Dictionary<string, int> force = new();
		static internal readonly Dictionary<string, int> giveway = new();
		static internal readonly Dictionary<string, DateTime> effect = new();
		static internal readonly Dictionary<string, DateTime> gives = new();
		static internal readonly Dictionary<string, DateTime> forces = new();
		static internal readonly Dictionary<string, bool> giver = new();
		static internal readonly Dictionary<string, bool> forcer = new();
		static internal readonly Dictionary<string, bool> effecter = new();
		static internal readonly Dictionary<string, bool> scp_play = new();

		[EventMethod(RoundEvents.Waiting)]
		static void Waiting()
		{
			try { Users.Clear(); } catch { }
			try { Donates.Clear(); } catch { }
			try { giveway.Clear(); } catch { }
			try { force.Clear(); } catch { }
			try { effect.Clear(); } catch { }
			try { gives.Clear(); } catch { }
			try { giver.Clear(); } catch { }
			try { forces.Clear(); } catch { }
			try { forcer.Clear(); } catch { }
			try { effecter.Clear(); } catch { }
			try { scp_play.Clear(); } catch { }
			try { Module.Prefixs.Clear(); } catch { }
		}

		[EventMethod(RoundEvents.End)]
		static void RoundEnd()
		{
			giveway.Clear();
			force.Clear();
			effect.Clear();
			gives.Clear();
			forces.Clear();
		}
	}
}