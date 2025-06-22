using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace gate3
{
    public class Plugin : Plugin<Config>
    {
        public Random Gen = new Random();
        public EventHandlers EventHandlers;
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
        public void RegisterEvents()
        {
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.ra;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.RestartingRound += EventHandlers.OnRoundRestart;


            Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning += EventHandlers.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnPlayerHurt;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension += EventHandlers.OnPocketDimensionDie;
            Exiled.Events.Handlers.Player.EscapingPocketDimension += EventHandlers.OnPocketDimensionEscaping;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.hurt;
            Exiled.Events.Handlers.Player.Died += EventHandlers.died;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.setrole;
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.OnCheckRoundEnd;
            Exiled.Events.Handlers.Player.InteractingElevator += EventHandlers.elevator;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.ra;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.RestartingRound -= EventHandlers.OnRoundRestart;


            Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning -= EventHandlers.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnPlayerHurt;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= EventHandlers.OnPocketDimensionDie;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= EventHandlers.OnPocketDimensionEscaping;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.hurt;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.died;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.setrole;
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.OnCheckRoundEnd;
            Exiled.Events.Handlers.Player.InteractingElevator -= EventHandlers.elevator;
            EventHandlers = null;
        }

    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}