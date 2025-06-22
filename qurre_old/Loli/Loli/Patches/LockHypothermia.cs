using InventorySystem.Items.Usables.Scp244.Hypothermia;
using HarmonyLib;
namespace Loli.Patches
{
    [HarmonyPatch(typeof(Hypothermia), nameof(Hypothermia.Update))]
    static internal class LockHypothermia
    {
        static internal bool Lock { get; set; } = false;
        static internal bool Prefix(Hypothermia __instance)
        {
            if (Lock)
            {
                __instance.ParamsActive = true;
                __instance.MuteSoundtrack = true;
                __instance.DisableSprint = true;
                return false;
            }
            return true;
        }
    }
}