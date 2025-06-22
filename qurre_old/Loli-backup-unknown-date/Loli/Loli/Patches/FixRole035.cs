using HarmonyLib;
using PlayerStatsSystem;
using Qurre.API;
using UnityEngine;
namespace Loli.Patches
{
    [HarmonyPatch(typeof(Player), nameof(Player.ChangeBody))]
    internal static class FixRole035
    {
        internal static bool Prefix(Player __instance, RoleType newRole, bool spawnRagdoll = false, Vector3 newPosition = default, Vector2 newRotation = default, string deathReason = "")
        {
            try
            {
                if (__instance.GetCustomRole() != Module.RoleType.Scp035) return true;
                if (spawnRagdoll) Qurre.API.Controllers.Ragdoll.Create(Scps.Scp035.CachedRole, __instance.Position, default, new CustomReasonDamageHandler(deathReason), __instance);
                if (newPosition == default) newPosition = __instance.Position;
                if (newRotation == default) newRotation = __instance.Rotation;
                __instance.ChangeModel(newRole);
                __instance.Position = newPosition;
                __instance.Rotation = newRotation;
                return false;
            }
            catch { return true; }
        }
    }
}