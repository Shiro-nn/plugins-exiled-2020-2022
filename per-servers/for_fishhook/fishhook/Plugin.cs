using EXILED;
using Harmony;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace fishhook
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;
		public Random Gen = new Random();
		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;
		private bool enabled;
		public static string[] owner;
		public override void OnEnable()
		{
			enabled = Config.GetBool("fishhook_enabled", true);
			if (!enabled) return;
			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"fishhook{harmonyCounter}");
			harmonyInstance.PatchAll();
			EventHandlers = new EventHandlers(this);
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.ShootEvent += EventHandlers.Shoot;
			Events.ShootEvent += EventHandlers.Shot;
			Events.RoundStartEvent += EventHandlers.RoundStart;
			Events.RoundEndEvent += EventHandlers.RoundEnd;
			Events.GrenadeThrownEvent += EventHandlers.f;
			Events.TeamRespawnEvent += EventHandlers.OnTeamRespawn;
			Events.UseMedicalItemEvent += EventHandlers.OnUseMedicalItem;
			Events.DropItemEvent += EventHandlers.OnDropItem;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
		}
		public override void OnDisable()
		{
			Events.ShootEvent -= EventHandlers.OnShoot;
			Events.ShootEvent -= EventHandlers.Shoot;
			Events.ShootEvent -= EventHandlers.Shot;
			Events.RoundStartEvent -= EventHandlers.RoundStart;
			Events.RoundEndEvent -= EventHandlers.RoundEnd;
			Events.GrenadeThrownEvent -= EventHandlers.f;
			Events.TeamRespawnEvent -= EventHandlers.OnTeamRespawn;
			Events.UseMedicalItemEvent -= EventHandlers.OnUseMedicalItem;
			Events.DropItemEvent -= EventHandlers.OnDropItem;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.PickupItemEvent -= EventHandlers.OnPickupItem;
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			EventHandlers = null;
		}

		public override void OnReload()
		{
		}

		public override string getName { get; } = "fishhook";
	}
}
