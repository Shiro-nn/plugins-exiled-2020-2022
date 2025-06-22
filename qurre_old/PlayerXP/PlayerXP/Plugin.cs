using HarmonyLib;
using Qurre.API.Attributes;
using System.IO;

namespace PlayerXP
{
    [PluginInit("PlayerXP", "fydne", "1.2.0")]
    static public class Plugin
    {
        static Harmony hInstance;

        [PluginEnable]
        static void Enable()
        {
            Cfg.Reload();
            if (!Directory.Exists(Methods.StatFilePath))
                Directory.CreateDirectory(Methods.StatFilePath);
            hInstance = new Harmony("fydne.playerxp");
            hInstance.PatchAll();
        }

        [PluginDisable]
        static void Disable()
        {
            hInstance.UnpatchAll(null);
        }
    }
}