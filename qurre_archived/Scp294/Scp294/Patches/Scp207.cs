using HarmonyLib;
using InventorySystem.Items.Usables;

namespace Scp294.Patches
{
    [HarmonyPatch(typeof(Scp207), nameof(Scp207.OnEffectsActivated))]
    internal static class Scp207ItemBase
    {
        internal static bool Prefix(Scp207 __instance)
        {
            return !__instance.TryGetDrink(out _);
        }
    }
}