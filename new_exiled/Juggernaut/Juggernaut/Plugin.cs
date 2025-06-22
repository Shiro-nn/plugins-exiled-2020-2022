using EXILED;
using Harmony;

namespace Juggernaut
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;

		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;

		private bool enabled;

		public override void OnEnable()
		{
			enabled = Config.GetBool("j_enabled", true);

			if (!enabled) return;

			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"Juggernaut{harmonyCounter}");
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
			Events.SetClassEvent += EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent += EventHandlers.OnContain106;
			Events.PocketDimDeathEvent += EventHandlers.OnPocketDimensionDie;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.ShootEvent += EventHandlers.OnShoot;
			_ = EventHandlers.Main();
			Events.UseMedicalItemEvent += EventHandlers.OnUseMedicalItem;
			Events.PocketDimEscapedEvent += EventHandlers.OnPocketDimensionExit;
			Events.PocketDimEnterEvent += EventHandlers.OnPocketDimensionEnter;
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.Scp096EnrageEvent += EventHandlers.scpzeroninesixe;
			Events.RemoteAdminCommandEvent += EventHandlers.RemoteAdminCommand;
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
			Events.CheckEscapeEvent -= EventHandlers.OnCheckEscape;
			Events.SetClassEvent -= EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent -= EventHandlers.OnContain106;
			Events.PocketDimDeathEvent -= EventHandlers.OnPocketDimensionDie;
			Events.UseMedicalItemEvent -= EventHandlers.OnUseMedicalItem;
			Events.PocketDimEscapedEvent -= EventHandlers.OnPocketDimensionExit;
			Events.PocketDimEnterEvent -= EventHandlers.OnPocketDimensionEnter;
			Events.DoorInteractEvent -= EventHandlers.RunOnDoorOpen;
			Events.Scp096EnrageEvent -= EventHandlers.scpzeroninesixe;
			Events.RemoteAdminCommandEvent -= EventHandlers.RemoteAdminCommand;

			EventHandlers = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "Juggernaut";
	}
}