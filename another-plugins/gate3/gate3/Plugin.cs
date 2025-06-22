using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace gate3
{
    public class Plugin : Plugin<Config>
    {
        public Random Gen = new Random();
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
            if(ServerConsole.Ip == "95.181.152.154")
            {
                Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
                Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
                Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
                Exiled.Events.Handlers.Server.RestartingRound += EventHandlers.OnRoundRestart;


                Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;
                Exiled.Events.Handlers.Player.Spawning += EventHandlers.OnPlayerSpawn;
                Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers.RunOnDoorOpen;
                Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnPlayerHurt;
            }
            else
            {
                Log.Info("IP не вафли дафли, плагин не запущен");
            }
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.RestartingRound -= EventHandlers.OnRoundRestart;


            Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning -= EventHandlers.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnPlayerHurt;
        }

    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}