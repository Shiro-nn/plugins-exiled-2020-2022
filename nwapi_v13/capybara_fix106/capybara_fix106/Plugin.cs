using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Events;

namespace capybara_fix106
{
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }

        [PluginConfig]
        public Config Config;

        [PluginEntryPoint("respawn of 106", "0.0.0", "", "")]
        public void Invoke()
        {
            Instance = this;
            EventManager.RegisterEvents<Events>(this);
            PluginHandler.Get(this).LoadConfig(this, nameof(PluginConfig));
        }
    }
}