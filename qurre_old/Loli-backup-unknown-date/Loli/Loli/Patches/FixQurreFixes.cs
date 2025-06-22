using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using Loli.Textures.Models.Rooms;
using Qurre.API;
using System.Reflection;
namespace Loli.Patches
{
	[HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.NetworkTargetState), MethodType.Setter)]
	internal static class OpenDoor
	{
		public static bool Prefix(DoorVariant __instance, ref bool value)
		{
			try
			{
				if (!Range.CustomDoors.TryFind(out var cd, x => x.Door == __instance)) return true;
				return cd.Interact(value);
			}
			catch (System.Exception e)
			{
				Qurre.Log.Error($"Patch [OpenDoor]:\n{e}\n{e.StackTrace}");
				return true;
			}
		}
	}
	[HarmonyPatch]
	internal static class FixQurreFixes
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("Qurre.Events.Modules.Etc");
			return AccessTools.Method(type, "FixFF");
		}
		internal static bool Prefix() => false;
	}
	[HarmonyPatch]
	internal static class FixQurreFixItems
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("Qurre.Events.Modules.Etc");
			return AccessTools.Method(type, "FixOneSerial");
		}
		internal static bool Prefix() => false;
	}
	[HarmonyPatch]
	internal static class FixQurreHideTag
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("Qurre.Patches.Events.player.HideBadge");
			return AccessTools.Method(type, "Prefix");
		}
		internal static bool Prefix(ref bool __result)
        {
			__result = true;
			return false;
		}
	}
	[HarmonyPatch]
	internal static class FixQurreShowTag
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("Qurre.Patches.Events.player.ShowBadge");
			return AccessTools.Method(type, "Prefix");
		}
		internal static bool Prefix(ref bool __result)
		{
			__result = true;
			return false;
		}
	}
}