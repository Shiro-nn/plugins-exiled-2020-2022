using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace serpentshand
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
            Exiled.Events.Handlers.Player.Verified += EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Left += EventHandlers.Leave;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension += EventHandlers.OnPocketDimensionDie;
            Exiled.Events.Handlers.Player.EscapingPocketDimension += EventHandlers.OnPocketDimensionEscaping;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += EventHandlers.OnPocketDimensionEnter;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.hurt;
            Exiled.Events.Handlers.Player.Died += EventHandlers.died;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.setrole;
            Exiled.Events.Handlers.Scp096.Enraging += EventHandlers.scpzeroninesixe;
            Exiled.Events.Handlers.Scp096.AddingTarget += EventHandlers.scpzeroninesixeadd;
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.OnCheckRoundEnd;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.ra;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.TeamRespawn;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Player.Verified -= EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Left -= EventHandlers.Leave;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= EventHandlers.OnPocketDimensionDie;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= EventHandlers.OnPocketDimensionEscaping;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= EventHandlers.OnPocketDimensionEnter;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.hurt;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.died;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.setrole;
            Exiled.Events.Handlers.Scp096.Enraging -= EventHandlers.scpzeroninesixe;
            Exiled.Events.Handlers.Scp096.AddingTarget -= EventHandlers.scpzeroninesixeadd;
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.OnCheckRoundEnd;
            EventHandlers = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string Spawn_bc { get; set; } = "<size=30%><color=red>Вы</color>-<color=#15ff00>Длань змеи</color>\n<color=#00ffdc>Ваша задача убить всех, кроме <color=red>SCP</color></color></size>";
        public ushort Spawn_bc_time { get; set; } = 10;
        public string Map_Spawn_bc { get; set; } = "<size=30%><color=red>Внимание всему персоналу!</color>\n<color=#00ffff>Отряд <color=#15ff00>Длань Змеи</color></color> <color=#0089c7>замечен на территории комплекса</color></size>";
        public ushort Map_Spawn_bc_time { get; set; } = 10;
        public int Max_players { get; set; } = 15;
        public int Chance { get; set; } = 40;
        public int Hp { get; set; } = 150;
        public List<int> SpawnItems { get; set; } = new List<int>() { 10, 20, 12, 14, 33, 25, 26, 15 };
        public ushort ciBcTime { get; set; } = 10;
        public string ciBc { get; set; } = "Приехал отряд хаоса.";
    }
}