using HarmonyLib;
using UnityEngine;
namespace Audio.Pathes
{
    [HarmonyPatch(typeof(AudioSettings), nameof(AudioSettings.outputSampleRate), MethodType.Getter)]
    internal static class AudioSettingsOutputSamplerateFix
    {
        internal static bool Prefix(ref int __result)
        {
            __result = 48000;
            return false;
        }
    }
}