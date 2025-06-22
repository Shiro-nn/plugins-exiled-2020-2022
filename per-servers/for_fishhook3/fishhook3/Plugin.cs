using EXILED;
using Harmony;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace fishhook3
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
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.RoundStartEvent += EventHandlers.RoundStart;
			Events.PlayerHurtEvent += EventHandlers.PlayerHurtEvent;
			Events.ConsoleCommandEvent += EventHandlers.ConsoleCmd;
			Events.PlayerSpawnEvent += EventHandlers.PlayerSpawn;
			Events.UseMedicalItemEvent += EventHandlers.OnUseMedicalItem;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.DropItemEvent += EventHandlers.OnDropItem;
			Configs.ReloadConfig();
		}
		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.DoorInteractEvent -= EventHandlers.RunOnDoorOpen;
			Events.RoundStartEvent -= EventHandlers.RoundStart;
			Events.PlayerHurtEvent -= EventHandlers.PlayerHurtEvent;
			Events.ConsoleCommandEvent -= EventHandlers.ConsoleCmd;
			Events.PlayerSpawnEvent -= EventHandlers.PlayerSpawn;
			Events.UseMedicalItemEvent -= EventHandlers.OnUseMedicalItem;
			Events.PickupItemEvent -= EventHandlers.OnPickupItem;
			Events.DropItemEvent -= EventHandlers.OnDropItem;
			EventHandlers = null;
		}

		public override void OnReload()
		{
		}

		public override string getName { get; } = "fishhook";
	}
}
