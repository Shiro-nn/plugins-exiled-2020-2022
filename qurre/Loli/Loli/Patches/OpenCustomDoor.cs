using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Searching;
using Loli.Textures.Models.Rooms;
using Qurre.API;
using System;

namespace Loli.Patches
{
    [HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.NetworkTargetState), MethodType.Setter)]
    internal static class OpenDoor
    {
        public static bool Prefix(DoorVariant __instance, ref bool value)
        {
            try
            {
                if (!Range.CustomDoors.TryFind(out var cd, x => x.Door == __instance))
                    return true;

                return cd.Interact(value);
            }
            catch (Exception e)
            {
                Log.Error($"Patch [OpenDoor]:\n{e}\n{e.StackTrace}");
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(SearchCompletor), nameof(SearchCompletor.ValidateAny))]
    internal static class OpenCustomDoor
    {
        private static bool Prefix(SearchCompletor __instance)
        {
            try
            {
                {
                    if (Textures.Models.Panel.Panels.TryGetValue(__instance.TargetPickup, out var _data))
                    {
                        try { _data.Interact(__instance.Hub.GetPlayer()); } catch { }
                        return false;
                    }
                }
                {
                    if (Textures.Models.Server.Doors.TryGetValue(__instance.TargetPickup, out var _data))
                    {
                        _data.InteractDoor();
                        return false;
                    }
                }
                {
                    if (Servers.Doors.TryGetValue(__instance.TargetPickup, out var _data))
                    {
                        _data.InteractDoor();
                        return false;
                    }
                }
                {
                    if (Control.Buttons.TryGetValue(__instance.TargetPickup, out var _data))
                    {
                        try { _data.Interact(__instance.Hub.GetPlayer()); } catch { }
                        return false;
                    }
                }
                {
                    if (ServersManager.Buttons.Contains(__instance.TargetPickup))
                    {
                        try { ServersManager.InteractHack(__instance.Hub.GetPlayer()); } catch { }
                        return false;
                    }
                }
                {
                    if (Scps.Scp294.API.Scp294.Buttons.Contains(__instance.TargetPickup))
                    {
                        try { Scps.Scp294.Events.Interact(__instance.Hub.GetPlayer()); } catch { }
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error in patching [OpenCustomDoor]:\n{e}\n{e.StackTrace}");
            }
            return true;
        }
    }
}