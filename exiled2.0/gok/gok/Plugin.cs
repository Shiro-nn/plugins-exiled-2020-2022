using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.Collections.Generic;
namespace gok
{
    public class Plugin : Plugin<Config>
    {
        internal Config config;
        public EventHandlers EventHandlers;
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.Low;
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
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.ra;
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.TeamRespawn;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.Ending;
            Exiled.Events.Handlers.Player.Verified += EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Died += EventHandlers.died;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.setrole;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.ra;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.TeamRespawn;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.Ending;
            Exiled.Events.Handlers.Player.Verified -= EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.died;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.setrole;
            EventHandlers = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string All_Spawn_bc { get; set; } = "<size=30%><color=red>Приехал отряд ГОК'а</color></size>";
        public ushort All_Spawn_bc_time { get; set; } = 10;
        public string Spawn_bc { get; set; } = "<size=30%><color=red>Вы -  ГОК</color></size>";
        public ushort Spawn_bc_time { get; set; } = 10;
        public int Max_players { get; set; } = 15;
        public int Hp { get; set; } = 150;
        public int Chance { get; set; } = 40;
        public List<int> SpawnItems { get; set; } = new List<int>() { 10, 20, 12, 14, 33, 25, 26, 15 };
    }
}