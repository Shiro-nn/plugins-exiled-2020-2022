using System;

namespace PlayerXP.gate3
{
    public class gate3p
    {
        internal readonly Plugin plugin;
        public gate3p(Plugin plugin) => this.plugin = plugin;
        public Random Gen = new Random();
        public EventHandlers EventHandlers;
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
        internal void UnregisterEvents()
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
}