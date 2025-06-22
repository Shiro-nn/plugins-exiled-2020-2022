using HarmonyLib;
namespace scp035.Patches
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRequestHideTag))]
	internal class HideTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			try
			{
				if (__instance.UserId == API.Scp035Data.GetScp035()?.characterClassManager?.UserId)
				{
					__instance.TargetConsolePrint(__instance.connectionToClient, "Зачем тебе убирать префикс?", "green");
					return false;
				}
				else
					return true;
			}
			catch
			{
				return true;
			}
		}
	}
}