using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
namespace cassie
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
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.WaitingForPlayers;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += EventHandlers.AnnouncingMTF;
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.TeamRespawn;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.WaitingForPlayers;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= EventHandlers.AnnouncingMTF;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.TeamRespawn;
            EventHandlers = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string MTFCassie { get; set; } = "XMAS_EPSILON11 %UnitName% %UnitNumber% XMAS_HASENTERED %ScpsLeft% XMAS_SCPSUBJECTS";
        public string CICassie { get; set; } = ".g1";
    }
}