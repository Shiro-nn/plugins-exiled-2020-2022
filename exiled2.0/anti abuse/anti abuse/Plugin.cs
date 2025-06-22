using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.IO;
namespace anti_abuse
{
    public class Plugin : Plugin<Config>
    {
        public static YamlConfig Cfg { get; set; }
        internal Events Events;
        #region override
        public static string ConfigsPath { get; internal set; }
        public override PluginPriority Priority { get; } = PluginPriority.Last;
        public override string Author { get; } = "fydne";
        public override void OnEnabled() => base.OnEnabled();
        public override void OnDisabled() => base.OnDisabled();
        public override void OnRegisteringCommands()
        {
            base.OnRegisteringCommands();
            ConfigsPath = Path.Combine(Paths.Configs, $"{Server.Port}-cfg.yml");
            if (!File.Exists(ConfigsPath))
            {
                File.Create(ConfigsPath).Close();
                File.WriteAllText(ConfigsPath, $"anti_abuse_roles: owner, admin\nanti_abuse_effect_cooldown: 3\nanti_abuse_owner_role: Server Owner");
            }
            Cfg = new YamlConfig(ConfigsPath);
            RegisterEvents();
        }
        public override void OnUnregisteringCommands()
        {
            base.OnUnregisteringCommands();
            UnregisterEvents();
        }
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Events = new Events(this);
            Exiled.Events.Handlers.Server.WaitingForPlayers += Events.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += Events.RoundStarted;
            Exiled.Events.Handlers.Scp106.Containing += Events.Contain;
            Exiled.Events.Handlers.Player.Died += Events.Dead;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += Events.Ra;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Events.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= Events.RoundStarted;
            Exiled.Events.Handlers.Scp106.Containing -= Events.Contain;
            Exiled.Events.Handlers.Player.Died -= Events.Dead;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= Events.Ra;
            Events = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}