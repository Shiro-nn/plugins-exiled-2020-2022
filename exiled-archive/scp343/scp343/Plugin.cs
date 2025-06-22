using EXILED;
using Harmony;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace scp343
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
			Events.PlayerBannedEvent += EventHandlers.ban2;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.RoundRestartEvent += EventHandlers.OnRoundRestart;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.CheckRoundEndEvent += EventHandlers.OnCheckRoundEnd;
			Events.SetClassEvent += EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.TriggerTeslaEvent += EventHandlers.tesla;
			Events.CheckEscapeEvent += EventHandlers.OnCheckEscape;
			Events.PlayerHandcuffedEvent += EventHandlers.OnPlayerHandcuffed;
			Events.Scp106ContainEvent += EventHandlers.OnContain106;
			Events.PocketDimEnterEvent += EventHandlers.OnPocketDimensionEnter;
			Events.WarheadCancelledEvent += EventHandlers.OnStopCountdown;
			Events.RemoteAdminCommandEvent += EventHandlers.RunOnRACommandSent;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.PlayerSpawnEvent += EventHandlers.OnTeamRespawn;
			Events.Scp096EnrageEvent += EventHandlers.scpzeroninesixe;
			Events.PlayerBanEvent += EventHandlers.ban;
			Events.IntercomSpeakEvent += EventHandlers.intercom;
			Events.DropItemEvent += EventHandlers.OnDropItem;
			Events.WarheadCancelledEvent += EventHandlers.OnWarheadCancel;
			Events.FemurEnterEvent += EventHandlers.OnFemurEnter;
		}
		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerBannedEvent -= EventHandlers.ban2;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.TriggerTeslaEvent -= EventHandlers.tesla;
			Events.ShootEvent -= EventHandlers.OnShoot;
			Events.CheckRoundEndEvent -= EventHandlers.OnCheckRoundEnd;
			Events.SetClassEvent -= EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
			Events.DoorInteractEvent -= EventHandlers.RunOnDoorOpen;
			Events.CheckEscapeEvent -= EventHandlers.OnCheckEscape;
			Events.Scp106ContainEvent -= EventHandlers.OnContain106;
			Events.PocketDimEnterEvent -= EventHandlers.OnPocketDimensionEnter;
			Events.WarheadCancelledEvent -= EventHandlers.OnStopCountdown; 
			Events.RemoteAdminCommandEvent -= EventHandlers.RunOnRACommandSent;
			Events.PickupItemEvent -= EventHandlers.OnPickupItem;
			Events.PlayerSpawnEvent -= EventHandlers.OnTeamRespawn;
			Events.Scp096EnrageEvent -= EventHandlers.scpzeroninesixe;
			Events.PlayerBanEvent -= EventHandlers.ban;
			Events.IntercomSpeakEvent -= EventHandlers.intercom;
			Events.DropItemEvent -= EventHandlers.OnDropItem;
			Events.WarheadCancelledEvent -= EventHandlers.OnWarheadCancel;
			Events.PlayerHandcuffedEvent -= EventHandlers.OnPlayerHandcuffed;
			Events.FemurEnterEvent -= EventHandlers.OnFemurEnter;
			EventHandlers = null;
		}

		public override void OnReload() 
		{
		}

		public override string getName { get; } = "scp343";
	}
}
