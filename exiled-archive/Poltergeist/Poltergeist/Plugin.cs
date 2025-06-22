using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace Poltergeist
{
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
            Exiled.Events.Handlers.Warhead.Starting += EventHandlers.OnWarheadStart;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            EventHandlers = null;
        }
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}