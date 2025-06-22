using HarmonyLib;
using InventorySystem.Searching;
using Qurre.API;
using System;
namespace Loli.Patches
{
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
                        try { _data.Interact(Player.Get(__instance.Hub)); } catch { }
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
                    if (Textures.Models.Rooms.Servers.Doors.TryGetValue(__instance.TargetPickup, out var _data))
                    {
                        _data.InteractDoor();
                        return false;
                    }
                }
                {
                    if (Textures.Models.Rooms.Control.Buttons.TryGetValue(__instance.TargetPickup, out var _data))
                    {
                        try { _data.Interact(Player.Get(__instance.Hub)); } catch { }
                        return false;
                    }
                }
                {
                    if (Textures.Models.Rooms.ServersManager.Buttons.Contains(__instance.TargetPickup))
                    {
                        try { Textures.Models.Rooms.ServersManager.InteractHack(Player.Get(__instance.Hub)); } catch { }
                        return false;
                    }
                }
                {
                    if (Scps.Scp294.API.Scp294.Buttons.Contains(__instance.TargetPickup))
                    {
                        try { Scps.Scp294.Events.Interact(Player.Get(__instance.Hub)); } catch { }
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Qurre.Log.Error($"umm, error in patching [OpenCustomDoor]:\n{e}\n{e.StackTrace}");
            }
            return true;
        }
    }
}