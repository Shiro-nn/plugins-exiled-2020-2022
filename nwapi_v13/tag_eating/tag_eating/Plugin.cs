using HarmonyLib;
using PluginAPI.Core.Attributes;

namespace tag_eating
{
    public class Plugin
    {
        [PluginEntryPoint("tag_eating", "0.0.0", "", "")]
        public void Invoke()
        {
            Harmony harmony = new Harmony("tag_eating");
            harmony.PatchAll();
        }
    }
}
