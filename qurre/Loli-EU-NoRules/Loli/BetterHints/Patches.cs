using HarmonyLib;
using Hints;
using Qurre.API;
using Qurre.API.Classification.Player;

namespace Loli.BetterHints
{
    [HarmonyPatch(typeof(Client), nameof(Client.ShowHint), new[] { typeof(string), typeof(float), typeof(HintEffect[]) })]
    internal static class Patches
    {
        internal static bool Prefix(Client __instance, string text, float duration = 1f)
        {
            Manager.ShowHint(Traverse.Create(__instance).Field("_player").GetValue() as Player, text, duration);
            return false;
        }
    }
}