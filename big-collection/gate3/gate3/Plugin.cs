using EXILED;
using Harmony;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace gate3
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;
		public Random Gen = new Random();
		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;
		private bool enabled;
		public static string[] premiumRoles;
		public static string[] vipRoles;
		public static string[] vipplusRoles;
		public static string[] staRoles;

		public override void OnEnable()
		{
			enabled = Config.GetBool("scp343_enabled", true);

			if (!enabled) return;

			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"scp343{harmonyCounter}");
			harmonyInstance.PatchAll();

			EventHandlers = new EventHandlers(this);

			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.RoundRestartEvent += EventHandlers.OnRoundRestart;
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
			Events.PlayerSpawnEvent += EventHandlers.OnPlayerSpawn;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.ShootEvent += EventHandlers.Shit;
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.ShootEvent += EventHandlers.Shoot;
			Events.ShootEvent += EventHandlers.Shot;
		}
		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.RoundRestartEvent -= EventHandlers.OnRoundRestart;
			Events.DoorInteractEvent -= EventHandlers.RunOnDoorOpen;
			Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;
			Events.PlayerSpawnEvent -= EventHandlers.OnPlayerSpawn;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.ShootEvent -= EventHandlers.Shit;
			Events.ShootEvent -= EventHandlers.OnShoot;
			Events.ShootEvent -= EventHandlers.Shoot;
			Events.ShootEvent -= EventHandlers.Shot;
			EventHandlers = null;
		}

		public override void OnReload() 
		{
		}

		public override string getName { get; } = "gate3";
	}
}
