using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.IO;

namespace lcz_ru
{
    public class Plugin : Plugin<Config>
    {
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.Last;
        public override string Author { get; } = "fydne";
        public override void OnEnabled() => base.OnEnabled();
        public override void OnDisabled() => base.OnDisabled();
        public override void OnRegisteringCommands()
        {
            base.OnRegisteringCommands();
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
            Exiled.Events.Handlers.Map.AnnouncingDecontamination += Announcing;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Map.AnnouncingDecontamination -= Announcing;
        }
        private void Announcing(Exiled.Events.EventArgs.AnnouncingDecontaminationEventArgs ev) =>
            GameCore.Console.singleton.TypeCommand($"/audio 1 {Path.Combine(Path.Combine(Paths.Plugins, "CustomEA"), $"lcz_{ev.Id}.raw")}");
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}
