namespace scp228ruj
{
	using Exiled.API.Enums;
	using Exiled.API.Features;
	using System.Collections.Generic;
	using System.ComponentModel;

	using Exiled.API.Interfaces;
	using Exiled.Events;
	public class Plugin : Plugin<Config>
	{
		public static YamlConfig cfg;
		private EventHandlers EventHandlers;

		public static int harmonyCounter;


		public override PluginPriority Priority { get; } = PluginPriority.Medium;
        public override void OnEnabled()
        {
            base.OnEnabled();

            RegisterEvents();
        }
        public override void OnDisabled()
        {
            base.OnDisabled();

            UnregisterEvents();
        }
        private void RegisterEvents()
        {
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.WaitingForPlayers             += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted                  += EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.EndingRound                   += EventHandlers.OnCheckRoundEnd;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand     += EventHandlers.RunOnRACommandSent;

            Exiled.Events.Handlers.Player.Died                          += EventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Player.Hurting                       += EventHandlers.OnPlayerHurt;
            Exiled.Events.Handlers.Player.Escaping                      += EventHandlers.OnCheckEscape;
            Exiled.Events.Handlers.Player.ChangingRole                  += EventHandlers.OnSetClass;
            Exiled.Events.Handlers.Player.Left                          += EventHandlers.OnPlayerLeave;
            Exiled.Events.Handlers.Player.EnteringFemurBreaker          += EventHandlers.OnContain106;
            Exiled.Events.Handlers.Player.EnteringPocketDimension       += EventHandlers.OnPocketDimensionEnter;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension  += EventHandlers.OnPocketDimensionDie;
            Exiled.Events.Handlers.Player.EscapingPocketDimension       += EventHandlers.OnPocketDimensionExit;
            Exiled.Events.Handlers.Player.PickingUpItem                 += EventHandlers.OnPickupItem;
            Exiled.Events.Handlers.Player.TriggeringTesla               += EventHandlers.scpzeroninesixe;
            Exiled.Events.Handlers.Player.InteractingDoor               += EventHandlers.RunOnDoorOpen;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers             -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted                  -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.EndingRound                   -= EventHandlers.OnCheckRoundEnd;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand     -= EventHandlers.RunOnRACommandSent;

            Exiled.Events.Handlers.Player.Died                          -= EventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Player.Hurting                       -= EventHandlers.OnPlayerHurt;
            Exiled.Events.Handlers.Player.Escaping                      -= EventHandlers.OnCheckEscape;
            Exiled.Events.Handlers.Player.ChangingRole                  -= EventHandlers.OnSetClass;
            Exiled.Events.Handlers.Player.Left                          -= EventHandlers.OnPlayerLeave;
            Exiled.Events.Handlers.Player.EnteringFemurBreaker          -= EventHandlers.OnContain106;
            Exiled.Events.Handlers.Player.EnteringPocketDimension       -= EventHandlers.OnPocketDimensionEnter;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension  -= EventHandlers.OnPocketDimensionDie;
            Exiled.Events.Handlers.Player.EscapingPocketDimension       -= EventHandlers.OnPocketDimensionExit;
            Exiled.Events.Handlers.Player.PickingUpItem                 -= EventHandlers.OnPickupItem;
            Exiled.Events.Handlers.Player.TriggeringTesla               -= EventHandlers.scpzeroninesixe;
            Exiled.Events.Handlers.Player.InteractingDoor               -= EventHandlers.RunOnDoorOpen;
            EventHandlers = null;
        }
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}