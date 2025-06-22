using EXILED;
using Harmony;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace MultiPlugin
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
		internal static string StatFilePath =
			Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Plugins"),
				"PlayerXP");

		public override void OnEnable()
		{
			enabled = Config.GetBool("mp_enabled", true);

			if (!enabled) return;

			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"scp682{harmonyCounter}");
			harmonyInstance.PatchAll();

			EventHandlers = new EventHandlers(this);

			// Register events
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.RoundRestartEvent += EventHandlers.OnRoundRestart;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.PlayerBannedEvent += EventHandlers.ban2;
			Events.CheckRoundEndEvent += EventHandlers.OnCheckRoundEnd;
			Events.SetClassEvent += EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.ShootEvent += EventHandlers.Shoot;
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.CheckEscapeEvent += EventHandlers.OnCheckEscape;
			Events.Scp106ContainEvent += EventHandlers.OnContain106;
			Events.PocketDimEnterEvent += EventHandlers.OnPocketDimensionEnter;
			Events.WarheadCancelledEvent += EventHandlers.OnStopCountdown;
			Events.RemoteAdminCommandEvent += EventHandlers.RunOnRACommandSent;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.PlayerSpawnEvent += EventHandlers.OnTeamRespawn;
			Events.Scp096EnrageEvent += EventHandlers.scpzeroninesixe;
			Events.PlayerBanEvent += EventHandlers.ban;
			Events.IntercomSpeakEvent += EventHandlers.intercom;
			Events.ShootEvent += EventHandlers.Shot;
			Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
			Events.DropItemEvent += EventHandlers.OnDropItem;
			Events.WarheadCancelledEvent += EventHandlers.OnWarheadCancel;
			Events.PlayerHandcuffedEvent += EventHandlers.OnPlayerHandcuffed;
			Events.FemurEnterEvent += EventHandlers.OnFemurEnter;
			Events.ShootEvent += EventHandlers.Shit;
			premiumRoles = Config.GetStringList("premium").ToArray();
			vipRoles = Config.GetStringList("vip").ToArray();
			vipplusRoles = Config.GetStringList("vipplus").ToArray();
			staRoles = Config.GetStringList("sta").ToArray();
		}
		public override void OnDisable()
		{
			// Unregister events
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.PlayerBannedEvent -= EventHandlers.ban2;
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
			Events.ShootEvent -= EventHandlers.Shoot;
			Events.ShootEvent -= EventHandlers.Shot;
			Events.ShootEvent -= EventHandlers.Shit;
			Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;
			Events.DropItemEvent -= EventHandlers.OnDropItem;
			Events.WarheadCancelledEvent -= EventHandlers.OnWarheadCancel;
			Events.PlayerHandcuffedEvent -= EventHandlers.OnPlayerHandcuffed;
			Events.FemurEnterEvent -= EventHandlers.OnFemurEnter;
			EventHandlers = null;
		}

		public override void OnReload() 
		{
		}

		public override string getName { get; } = "Multi Plugin";
	}
}
