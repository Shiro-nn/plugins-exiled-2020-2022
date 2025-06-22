using EXILED;
using Harmony;

namespace scp228ruj
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;

		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;

		private bool enabled;

		public override void OnEnable()
		{
			enabled = Config.GetBool("scp228ruj_enabled", true);

			if (!enabled) return;

			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"scp228ruj{harmonyCounter}");
			harmonyInstance.PatchAll();

			EventHandlers = new EventHandlers(this);

			// Register events
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.RoundRestartEvent += EventHandlers.OnRoundRestart;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.CheckRoundEndEvent += EventHandlers.OnCheckRoundEnd;
			Events.CheckEscapeEvent += EventHandlers.OnCheckEscape;
			Events.RemoteAdminCommandEvent += EventHandlers.RunOnRACommandSent;
			Events.SetClassEvent += EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent += EventHandlers.OnContain106;
			Events.PocketDimDeathEvent += EventHandlers.OnPocketDimensionDie;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.PocketDimEscapedEvent += EventHandlers.OnPocketDimensionExit;
			Events.PocketDimEnterEvent += EventHandlers.OnPocketDimensionEnter;
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.Scp096EnrageEvent += EventHandlers.scpzeroninesixe;
		}

		public override void OnDisable()
		{
			// Unregister events
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.CheckRoundEndEvent -= EventHandlers.OnCheckRoundEnd;
			Events.CheckEscapeEvent -= EventHandlers.OnCheckEscape;
			Events.SetClassEvent -= EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent -= EventHandlers.OnContain106;
			Events.RemoteAdminCommandEvent -= EventHandlers.RunOnRACommandSent;
			Events.PocketDimDeathEvent -= EventHandlers.OnPocketDimensionDie;
			Events.PickupItemEvent -= EventHandlers.OnPickupItem;
			Events.PocketDimEscapedEvent -= EventHandlers.OnPocketDimensionExit;
			Events.PocketDimEnterEvent -= EventHandlers.OnPocketDimensionEnter; 
			Events.DoorInteractEvent -= EventHandlers.RunOnDoorOpen;
			Events.Scp096EnrageEvent -= EventHandlers.scpzeroninesixe;

			EventHandlers = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "SCP-228-RU-J";
	}
}
