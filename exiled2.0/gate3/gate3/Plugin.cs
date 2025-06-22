using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using MEC;
namespace gate3
{
    public class Plugin : Plugin<Config>
    {
        internal Config config;
        public EventHandlers EventHandlers;
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.Last;
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
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.Waiting;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.RoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers.DoorOpen;
            Exiled.Events.Handlers.Player.Verified += EventHandlers.PlayerJoin;
            Exiled.Events.Handlers.Warhead.Detonated += EventHandlers.Detonated;
            Timing.RunCoroutine(EventHandlers.etc());
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.Waiting;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.RoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers.DoorOpen;
            Exiled.Events.Handlers.Player.Verified -= EventHandlers.PlayerJoin;
            Exiled.Events.Handlers.Warhead.Detonated -= EventHandlers.Detonated;
            EventHandlers = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}
