using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace godoff
{
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }

        [PluginConfig]
        public Config Config;

        [PluginEntryPoint("godoff", "0.0.0", "", "")]
        public void Invoke()
        {
            Instance = this;
            EventManager.RegisterEvents<Events>(this);
            PluginHandler.Get(this).LoadConfig(this, nameof(PluginConfig));
        }
    }
}
