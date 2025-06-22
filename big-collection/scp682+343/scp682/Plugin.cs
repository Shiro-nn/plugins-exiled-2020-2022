using EXILED;
using Harmony;
using System;
namespace scp682343
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
			enabled = Config.GetBool("scp682_enabled", true);

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
			Events.CheckRoundEndEvent += EventHandlers.OnCheckRoundEnd;
			Events.SetClassEvent += EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.CheckEscapeEvent += EventHandlers.OnCheckEscape;
			Events.Scp106ContainEvent += EventHandlers.OnContain106;
			Events.PocketDimEnterEvent += EventHandlers.OnPocketDimensionEnter;
			Events.WarheadCancelledEvent += EventHandlers.OnStopCountdown;
			Events.RemoteAdminCommandEvent += EventHandlers.RunOnRACommandSent;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.PlayerSpawnEvent += EventHandlers.OnTeamRespawn;
		}
		public override void OnDisable()
		{
			// Unregister events
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
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

			EventHandlers = null;
		}

		public override void OnReload() 
		{
		}

		public override string getName { get; } = "scp682";
	}
}
