using EXILED;
using Harmony;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace fishhook2
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
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.RoundStart;
			Events.RoundEndEvent += EventHandlers.RoundEnd;
			Events.TeamRespawnEvent += EventHandlers.OnTeamRespawn;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.SetClassEvent += EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
		}
		public override void OnDisable()
		{
			Events.RoundStartEvent -= EventHandlers.RoundStart;
			Events.RoundEndEvent -= EventHandlers.RoundEnd;
			Events.TeamRespawnEvent -= EventHandlers.OnTeamRespawn;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.DoorInteractEvent -= EventHandlers.RunOnDoorOpen;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.SetClassEvent -= EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
			Events.PickupItemEvent -= EventHandlers.OnPickupItem;
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			EventHandlers = null;
		}

		public override void OnReload()
		{
		}

		public override string getName { get; } = "fishhook";
	}
}
