using HarmonyLib;
using InventorySystem.Configs;
using InventorySystem.Items.Armor;

namespace Loli.Patches
{
    [HarmonyPatch(typeof(InventoryLimits), nameof(InventoryLimits.GetAmmoLimit), new[] { typeof(BodyArmor), typeof(ItemType) })]
    static class AmmoLimits
    {
        [HarmonyPrefix]
        static bool Call(ref ushort __result)
        {
            __result = 999;
            return false;
        }
    }
}