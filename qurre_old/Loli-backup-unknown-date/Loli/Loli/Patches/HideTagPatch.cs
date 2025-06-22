using HarmonyLib;
using Loli.DataBase;
using Qurre.API;
namespace Loli.Patches
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserCode_CmdRequestHideTag))]
	internal class HideTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			if(__instance.UserId.Contains("@northwood")) return true;
            try
            {
				var pl = Player.Get(__instance.UserId);
				if (pl.ServerRoles.NetworkGlobalBadge != "")
                {
					pl.ServerRoles.NetworkGlobalBadge = "";
					Levels.SetPrefix(pl);
					pl.SendConsoleMessage("Успешно", "green");
				}
                else
                {
					var data = Manager.Static.Data.Users[pl.UserId];
					if(data.sr || data.hr || data.ghr || data.ar || data.gar)
                    {
						data.anonym = true;
						Levels.SetPrefix(pl);
						pl.SendConsoleMessage("Успешно", "green");
					}
					else pl.SendConsoleMessage("Зачем тебе убирать префикс?", "green");
				}
			}
            catch
			{
				__instance.TargetConsolePrint(__instance.connectionToClient, "Зачем тебе убирать префикс?", "green");
			}
			return false;
		}
	}
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserCode_CmdRequestShowTag))]
	internal class ShowTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			if (__instance.UserId.Contains("@northwood")) return true;
			try
			{
				var pl = Player.Get(__instance.UserId);
				var data = Manager.Static.Data.Users[pl.UserId];
				if (data.sr || data.hr || data.ghr || data.ar || data.gar)
				{
					data.anonym = false;
					Levels.SetPrefix(pl);
					pl.SendConsoleMessage("Успешно", "green");
				}
				else pl.SendConsoleMessage("Зачем тебе это?", "green");
			}
			catch
			{
				__instance.TargetConsolePrint(__instance.connectionToClient, "Зачем тебе это?", "green");
			}
			return false;
		}
	}
}