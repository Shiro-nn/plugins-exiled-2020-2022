using HarmonyLib;

namespace MongoDB.Patches
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRequestHideTag))]
	class HideTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			__instance.TargetConsolePrint(__instance.connectionToClient, "Зачем тебе убирать префикс?", "green");
			return false;
		}
	}
}
