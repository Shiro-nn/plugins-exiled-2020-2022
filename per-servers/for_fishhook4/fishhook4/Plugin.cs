using EXILED;
using Harmony;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace fishhook4
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;
		public Random Gen = new Random();
		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;
		private bool enabled;
		public override void OnEnable()
		{
			enabled = Config.GetBool("fishhook_enabled", true);
			if (!enabled) return;
			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"fishhook{harmonyCounter}");
			harmonyInstance.PatchAll();
			EventHandlers = new EventHandlers(this);
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.PickupItemEvent += EventHandlers.OnPickupEvent;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.ShootEvent += EventHandlers.Shoot;
			Events.WarheadCancelledEvent += EventHandlers.WarheadCancel;
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundStartEvent += EventHandlers.RoundStart;
			Events.RoundEndEvent += EventHandlers.RoundEnd;
			Events.CheckRoundEndEvent += EventHandlers.CheckRoundEnd;
			Events.PlayerHurtEvent += EventHandlers.hurt;
			Configs.ReloadConfig();
		}
		public override void OnDisable()
		{
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.DoorInteractEvent -= EventHandlers.RunOnDoorOpen;
			Events.PickupItemEvent -= EventHandlers.OnPickupEvent;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.ShootEvent -= EventHandlers.OnShoot;
			Events.ShootEvent -= EventHandlers.Shoot;
			Events.WarheadCancelledEvent -= EventHandlers.WarheadCancel;
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundStartEvent -= EventHandlers.RoundStart;
			Events.RoundEndEvent -= EventHandlers.RoundEnd;
			Events.CheckRoundEndEvent -= EventHandlers.CheckRoundEnd;
			Events.PlayerHurtEvent -= EventHandlers.hurt;
			EventHandlers = null;
		}

		public override void OnReload()
		{
		}

		public override string getName { get; } = "fishhook";
	}
}
