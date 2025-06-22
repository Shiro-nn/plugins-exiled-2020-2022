using HarmonyLib;

namespace Loli.Patches
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserCode_CmdRequestServerGroups))]
	static class AntiSpyGroups
	{
		[HarmonyPrefix]
		static bool Call(CharacterClassManager __instance)
		{
			if (__instance.UserId.Contains("@northwood"))
				return true;

			__instance.ConsolePrint("Зачем тебе это?", "red");
			return false;
		}
	}
}