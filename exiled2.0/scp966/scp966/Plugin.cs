using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
namespace scp966
{
    public class Plugin : Plugin<Config>
    {
        internal Config config;
        public EventHandlers EventHandlers;
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override string Author { get; } = "fydne";
        public override void OnEnabled() => base.OnEnabled();
        public override void OnDisabled() => base.OnDisabled();
        public override void OnRegisteringCommands()
        {
            base.OnRegisteringCommands();
            cfg1();
            RegisterEvents();
        }
        public override void OnUnregisteringCommands()
        {
            base.OnUnregisteringCommands();
            UnregisterEvents();
        }
        #endregion
        #region cfg
        internal void cfg1() => config = base.Config;
        #endregion
        #region Events
        private void RegisterEvents()
        {
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.RoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers.Door;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.Hurt;
            Exiled.Events.Handlers.Scp914.UpgradingItems += EventHandlers.Scp914;
            Exiled.Events.Handlers.Player.Left += EventHandlers.Leave;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.SetClass;
            Exiled.Events.Handlers.Player.Died += EventHandlers.Died;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.Ra;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.RoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers.Door;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.Hurt;
            Exiled.Events.Handlers.Scp914.UpgradingItems -= EventHandlers.Scp914;
            Exiled.Events.Handlers.Player.Left -= EventHandlers.Leave;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.SetClass;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.Died;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.Ra;
            EventHandlers = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int Hp { get; set; } = 500;
        public string Bc { get; set; } = "Вы-SCP 966";
        public ushort Bc_time { get; set; } = 10;
        public int min_players_for_spawn { get; set; } = 5;
        public string Cassie_dead { get; set; } = "scp 9 6 6 containment minute";
    }
}