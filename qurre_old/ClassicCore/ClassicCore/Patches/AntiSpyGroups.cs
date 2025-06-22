using HarmonyLib;
namespace ClassicCore.Patches
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserCode_CmdRequestServerGroups))]
	internal class AntiSpyGroups
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			if (__instance.UserId.Contains("@northwood")) return true;
			__instance.TargetConsolePrint(__instance.Scp079.connectionToClient, "Зачем тебе это?", "red");
			return false;
		}
	}
}