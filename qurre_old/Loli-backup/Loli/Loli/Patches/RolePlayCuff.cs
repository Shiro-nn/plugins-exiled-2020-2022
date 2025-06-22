using HarmonyLib;
using InventorySystem;
using InventorySystem.Disarming;
using InventorySystem.Items;
using static InventorySystem.Disarming.DisarmedPlayers;
namespace Loli.Patches
{
    [HarmonyPatch(typeof(DisarmedPlayers), nameof(DisarmedPlayers.CanDisarm))]
    internal static class RolePlayCuff
    {
        internal static bool Prefix(ReferenceHub disarmerHub, ReferenceHub targetHub, ref bool __result)
        {
            if (!Plugin.RolePlay) return true;
            if (!disarmerHub.characterClassManager.IsHuman() || !targetHub.characterClassManager.IsHuman())
            {
                __result = false;
                return false;
            }
            ItemBase curInstance = disarmerHub.inventory.CurInstance;
            IDisarmingItem disarmingItem;
            if (curInstance != null && (disarmingItem = (curInstance as IDisarmingItem)) != null)
            {
                __result = disarmingItem.AllowDisarming;
                return false;
            }
            __result = false;
            return false;
        }
    }
    [HarmonyPatch(typeof(DisarmedPlayers), nameof(DisarmedPlayers.ValidateEntry))]
    internal static class RolePlayCuff2
    {
        internal static bool Prefix(DisarmedEntry entry, ref bool __result)
        {
            if (!Plugin.RolePlay) return true;
            if (entry.Disarmer == 0)
            {
                __result = true;
                return false;
            }
            if (!ReferenceHub.TryGetHubNetID(entry.DisarmedPlayer, out ReferenceHub hub))
            {
                __result = false;
                return false;
            }
            if (!ReferenceHub.TryGetHubNetID(entry.Disarmer, out ReferenceHub hub2))
            {
                __result = false;
                return false;
            }
            if (!hub.characterClassManager.IsHuman() || !hub2.characterClassManager.IsHuman())
            {
                __result = false;
                return false;
            }
            if ((hub.transform.position - hub2.transform.position).sqrMagnitude > 8100f)
            {
                __result = false;
                return false;
            }
            hub.inventory.ServerDropEverything();
            __result = true;
            return false;
        }
    }
}