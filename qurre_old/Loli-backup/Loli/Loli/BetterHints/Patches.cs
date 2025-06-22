using HarmonyLib;
using Hints;
using Qurre.API;
namespace Loli.BetterHints
{
    [HarmonyPatch(typeof(Player), nameof(Player.ShowHint), new[] { typeof(string), typeof(float), typeof(HintEffect[]) })]
    internal static class Patches
    {
        internal static bool Prefix(Player __instance, string text, float duration = 1f)
        {
            Manager.ShowHint(__instance, text, duration);
            return false;
        }
    }
}