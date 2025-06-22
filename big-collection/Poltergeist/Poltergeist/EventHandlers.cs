using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;
using hideandseek.API;
namespace Poltergeist
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		public static bool dr = false;
		public static bool dd = false;
		private static System.Random rand = new System.Random();
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

		private bool Tryhason()
		{
			return scp682Data.Gethason();
		}

		public void OnRoundStart()
		{
			players.Clear();
			foreach (CoroutineHandle coroutine in this.Coroutines)
				Timing.KillCoroutines(coroutine);
			if (Tryhason()) return;
			this.Coroutines.Add(Timing.RunCoroutine(this.plugin.RunBlackoutTimer()));
			dr = false; 
			dd = false;
		}

		public void OnRoundEnd()
		{
			foreach (CoroutineHandle coroutine in this.Coroutines)
				Timing.KillCoroutines(coroutine);
			dr = false;
			dd = false;
		}

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
			foreach (CoroutineHandle coroutine in this.Coroutines)
				Timing.KillCoroutines(coroutine);
			dr = false;
			dd = false;
		}

		public void OnRoundRestart()
		{
			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (dd)
			{
				dd = false;
				Map.Broadcast("<color=lime>Полтергейст получил свою жертву</color>\n<color=aqua>Т</color><color=red>е</color><color=aqua>п</color><color=red>е</color><color=aqua>рь он друж</color><color=red>е</color><color=aqua>любн</color><color=red>е</color><color=aqua>й</color>", 10);
			}
		}
		public void RunOnDoorOpen(ref DoorInteractionEvent doorInt)
		{
			if (doorInt.Door.DoorName == "012_BOTTOM")
			{
				doorInt.Allow = false;
				doorInt.Player.ClearBroadcasts();
				doorInt.Player.Broadcast(5, "<color=red>Это дом полтергейста, уходи!</color>", false);
				Extensions.Damage(doorInt.Player, 5, DamageTypes.Nuke);
				return;
			}
			if (dr)
			{
				if (rand.Next(1, 100) < 50)
				{
					doorInt.Allow = true;
					return;
				}
				if (rand.Next(1, 100) < 50)
				{
					doorInt.Allow = false;
					return;
				}
			}
		}
	}
}
