using HarmonyLib;
using InventorySystem.Items.Usables;

namespace Loli.Scps.Scp294
{
    [HarmonyPatch(typeof(Scp207), nameof(Scp207.OnEffectsActivated))]
    static class Patch
    {
        [HarmonyPrefix]
        static bool Call(Scp207 __instance)
        {
            return !__instance.TryGetDrink(out _);
        }
    }
}