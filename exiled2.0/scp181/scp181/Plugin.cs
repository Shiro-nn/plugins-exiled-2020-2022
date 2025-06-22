using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.ComponentModel;
namespace scp181
{
    public class Plugin : Plugin<Config>
    {
        internal Config config;
        public EventHandlers EventHandlers;
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.Lower;
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
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.RoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers.door;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.hurt;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.RoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers.door;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.hurt;
            EventHandlers = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        [Description("Максимальное кол-во дверей, которое может открыть scp181")]
        public int MaxDoorTries { get; set; } = 5;
        [Description("Максимальное кол-во спасений от scp от scp181")]
        public int MaxTries { get; set; } = 5;
        [Description("bc о том, что scp181 не открыл дверь, т.к это оружейка")]
        public string dontopen { get; set; } = "\n<color=red>SCP 181 не любит войну</color>";
        public ushort dontopent { get; set; } = 10;
        [Description("bc о том, что scp181 открыл дверь")]
        public string open { get; set; } = "\n<color=#54ff00>SCP 181 открыл вам дверь</color>";
        public ushort opent { get; set; } = 10;
        [Description("bc жертве, что scp181 помог ему")]
        public string safe { get; set; } = "\n<color=#54ff00>Вам помог SCP 181</color>";
        public ushort safet { get; set; } = 10;
        [Description("bc убийце, что scp181 помог его жертве")]
        public string anti_safe { get; set; } = "\n<color=red>Этой жертве помог SCP 181</color>";
        public ushort anti_safet { get; set; } = 10;
    }
}