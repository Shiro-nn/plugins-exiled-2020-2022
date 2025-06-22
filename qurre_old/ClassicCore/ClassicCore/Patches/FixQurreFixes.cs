using HarmonyLib;
using System.Reflection;
namespace ClassicCore.Patches
{
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
			var type = AccessTools.TypeByName("Qurre.Patches.Events.Player.HideBadge");
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
			var type = AccessTools.TypeByName("Qurre.Patches.Events.Player.ShowBadge");
			return AccessTools.Method(type, "Prefix");
		}
		internal static bool Prefix(ref bool __result)
		{
			__result = true;
			return false;
		}
	}
}