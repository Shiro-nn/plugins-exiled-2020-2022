using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace scp714
{
    public class Plugin : Plugin<Config>
    {
        internal Config config;
        public EventHandlers EventHandlers;
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.First;
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
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.RoundStart;
            Exiled.Events.Handlers.Player.PickingUpItem += EventHandlers.Pickup;
            Exiled.Events.Handlers.Player.Dying += EventHandlers.Dying;
            MEC.Timing.RunCoroutine(EventHandlers.aok());
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.RoundStart;
            Exiled.Events.Handlers.Player.PickingUpItem -= EventHandlers.Pickup;
            Exiled.Events.Handlers.Player.Dying -= EventHandlers.Dying;
            EventHandlers = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string PickupBc { get; set; } = "<b><color=lime>Вы подобрали SCP714</color></b>";
        public ushort PickupBcTime { get; set; } = 5;
    }
}