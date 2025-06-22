using Exiled.Events.Handlers;
namespace MongoDB.scp035
{
	public class Main035
	{
		internal EventHandlers EventHandlers;
		internal readonly MongoDB.Plugin plugin;
		public Main035(MongoDB.Plugin plugin) => this.plugin = plugin;

		public void RegisterEvents()
		{
			EventHandlers = new EventHandlers(this);
			Server.RoundStarted += EventHandlers.OnRoundStart;
			Player.PickingUpItem += EventHandlers.OnPickupItem;
			Server.RoundEnded += EventHandlers.OnRoundEnd;
			Server.RestartingRound += EventHandlers.OnRoundRestart;
			Player.Died += EventHandlers.OnPlayerDie;
			Player.Died += EventHandlers.OnPlayerDied;
			Player.Hurting += EventHandlers.OnPlayerHurt;
			Player.EnteringPocketDimension += EventHandlers.OnPocketDimensionEnter;
			Player.EnteringFemurBreaker += EventHandlers.OnFemurBreaker;
			Player.Escaping += EventHandlers.OnCheckEscape;
			Player.ChangingRole += EventHandlers.OnSetClass;
			Player.Left += EventHandlers.OnPlayerLeave;
			Scp106.Containing += EventHandlers.OnContain106;
			Player.Handcuffing += EventHandlers.OnPlayerHandcuffed;
			Player.InsertingGeneratorTablet += EventHandlers.OnInsertTablet;
			Player.EjectingGeneratorTablet += EventHandlers.OnEjectTablet;
			Player.FailingEscapePocketDimension += EventHandlers.OnPocketDimensionDie;
			Player.Shooting += EventHandlers.OnShoot;
			Server.SendingRemoteAdminCommand += EventHandlers.RunOnRACommandSent;
			Scp096.Enraging += EventHandlers.scpzeroninesixe;
			Scp096.AddingTarget += EventHandlers.scpzeroninesixeadd;
		}

		public void UnregisterEvents()
		{
			Server.RoundStarted -= EventHandlers.OnRoundStart;
			Player.PickingUpItem -= EventHandlers.OnPickupItem;
			Server.RoundEnded -= EventHandlers.OnRoundEnd;
			Server.RestartingRound -= EventHandlers.OnRoundRestart;
			Player.Died -= EventHandlers.OnPlayerDie;
			Player.Died -= EventHandlers.OnPlayerDied;
			Player.Hurting -= EventHandlers.OnPlayerHurt;
			Player.EnteringPocketDimension -= EventHandlers.OnPocketDimensionEnter;
			Player.EnteringFemurBreaker -= EventHandlers.OnFemurBreaker;
			Player.Escaping -= EventHandlers.OnCheckEscape;
			Player.ChangingRole -= EventHandlers.OnSetClass;
			Player.Left -= EventHandlers.OnPlayerLeave;
			Scp106.Containing -= EventHandlers.OnContain106;
			Player.Handcuffing -= EventHandlers.OnPlayerHandcuffed;
			Player.InsertingGeneratorTablet -= EventHandlers.OnInsertTablet;
			Player.EjectingGeneratorTablet -= EventHandlers.OnEjectTablet;
			Player.FailingEscapePocketDimension -= EventHandlers.OnPocketDimensionDie;
			Player.Shooting -= EventHandlers.OnShoot;
			Server.SendingRemoteAdminCommand -= EventHandlers.RunOnRACommandSent;
			Scp096.Enraging -= EventHandlers.scpzeroninesixe;
			Scp096.AddingTarget -= EventHandlers.scpzeroninesixeadd;

			EventHandlers = null;
		}
	}
}
