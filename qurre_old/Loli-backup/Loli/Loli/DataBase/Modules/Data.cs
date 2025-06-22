using Qurre.API.Events;
using System;
using System.Collections.Generic;
namespace Loli.DataBase.Modules
{
	internal class Data
	{
		internal readonly Manager Manager;
		public Data(Manager manager) => Manager = manager;
		internal readonly static Dictionary<string, Clan> Clans = new();
		internal readonly Dictionary<string, UserData> Users = new();
		internal readonly Dictionary<string, DonateRA> Donates = new();
		internal readonly Dictionary<string, DonateRoles> Roles = new();
		internal readonly Dictionary<string, int> force = new();
		internal readonly Dictionary<string, int> giveway = new();
		internal readonly Dictionary<string, DateTime> effect = new();
		internal readonly Dictionary<string, DateTime> gives = new();
		internal readonly Dictionary<string, DateTime> forces = new();
		internal readonly Dictionary<string, bool> giver = new();
		internal readonly Dictionary<string, bool> forcer = new();
		internal readonly Dictionary<string, bool> effecter = new();
		internal readonly Dictionary<string, bool> scp_play = new();
		internal bool Contain { get; set; } = false;
		internal void Waiting()
		{
			Contain = false;
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
			Manager.Loader.LoadClans();
		}
		internal void RoundEnd(RoundEndEvent _)
		{
			Contain = false;
			giveway.Clear();
			force.Clear();
			effect.Clear();
			gives.Clear();
			forces.Clear();
		}
	}
}