using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs;
namespace kill
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "fydne";
        public override void OnEnabled() => base.OnEnabled();
        public override void OnDisabled() => base.OnDisabled();
        public override void OnRegisteringCommands() => Exiled.Events.Handlers.Server.SendingConsoleCommand += console;
        public override void OnUnregisteringCommands() => Exiled.Events.Handlers.Server.SendingConsoleCommand -= console;
        private void console(SendingConsoleCommandEventArgs ev)
        {
            if (ev.Name == "kill")
            {
                ev.IsAllowed = false;
                ev.Player.ReferenceHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(666666, "WORLD", DamageTypes.Bleeding, ev.Player.ReferenceHub.queryProcessor.PlayerId), ev.Player.ReferenceHub.gameObject);
            }
        }
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}