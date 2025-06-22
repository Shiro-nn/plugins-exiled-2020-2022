using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace PlayerXP
{
	using Exiled.API.Enums;
	using Exiled.API.Features;
	using System.Collections.Generic;
	using System.ComponentModel;

	using Exiled.API.Interfaces;
    using Exiled.Events;
    public class Plugin : Plugin<Configs>
    {
        public static YamlConfig cfg;
        public EventHandlers EventHandlers;

        internal static Regex[] regices = new Regex[0];
        internal static string StatFilePath =
            Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Plugins"),
                "PlayerXP");

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
            if (!Directory.Exists(StatFilePath))
                Directory.CreateDirectory(StatFilePath);

            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers.OnConsoleCommand;

            Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning += EventHandlers.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.Escaping += EventHandlers.OnCheckEscape;
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension += EventHandlers.OnPocketDimensionDie;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers.OnConsoleCommand;

            Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning -= EventHandlers.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.Escaping -= EventHandlers.OnCheckEscape;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= EventHandlers.OnPocketDimensionDie;
            EventHandlers = null;
        }
    }
}