using HarmonyLib;
using InventorySystem.Configs;
using InventorySystem.Searching;
using Loli.BetterHints;
using Qurre.API;
using UnityEngine;

namespace Loli.Patches
{
    [HarmonyPatch(typeof(ItemSearchCompletor), nameof(ItemSearchCompletor.ValidateAny))]
    static class ItemLimitsHint
    {
        static FastInvokeHandler baseValidate;
        static bool Call(ItemSearchCompletor __instance, ref bool __result)
        {
            if(baseValidate is null)
                baseValidate = MethodInvoker.GetHandler(AccessTools.Method(typeof(SearchCompletor), "ValidateAny"));

            if (baseValidate(__instance, new object[] { }) is not bool answer || !answer)
            {
                __result = false;
                return false;
            }

            if (__instance.Hub.inventory.UserInventory.Items.Count >= 8)
            {
                Player.Get(__instance.Hub).Hint(new(0, 0, "Вы превысили лимит в 8 предметов", 5, true));
                __result = false;
                return false;
            }

            if (__instance._category != 0)
            {
                int num = Mathf.Abs(InventoryLimits.GetCategoryLimit(__instance._category, __instance.Hub));
                if (__instance.CategoryCount >= num)
                {
                    Player.Get(__instance.Hub).Hint(new(0, 0, $"Вы превысили лимит в {num} предметов данной категории", 5, true));
                    __result = false;
                    return false;
                }
            }

            __result = true;
            return false;
        }
    }
}
