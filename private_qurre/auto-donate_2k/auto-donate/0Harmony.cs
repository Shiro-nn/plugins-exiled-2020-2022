using HarmonyLib;
using Qurre.API;
namespace auto_donate
{
    [HarmonyPatch(typeof(Player), nameof(Player.Group), MethodType.Getter)]
    internal static class GetGroup
    {
        internal static bool Prefix(Player __instance, ref UserGroup __result)
        {
            if (!EventHandlers.Donates.TryGetValue(__instance.UserId, out var data)) return true;
            __result = ServerStatic.GetPermissionsHandler().GetGroup(data);
            return false;
        }
    }
}