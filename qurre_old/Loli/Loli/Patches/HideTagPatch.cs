using HarmonyLib;
using Loli.DataBase;
using Loli.DataBase.Modules;
using Qurre.API;
namespace Loli.Patches
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserCode_CmdRequestHideTag))]
	internal class HideTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			if (__instance.UserId.Contains("@northwood")) return true;
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
					if (Manager.Static.Data.Users.TryGetValue(pl.UserId, out var data) &&
						(data.trainee || data.helper || data.mainhelper || data.admin || data.mainadmin || data.owner || CustomDonates.ThisYt(data)))
					{
						data.anonym = true;
						Levels.SetPrefix(pl);
						pl.SendConsoleMessage("Успешно", "green");
					}
					else pl.SendConsoleMessage("Зачем тебе убирать префикс?", "red");
				}
			}
			catch
			{
				__instance.TargetConsolePrint(__instance.connectionToClient, "Зачем тебе убирать префикс?", "red");
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
				if (Manager.Static.Data.Users.TryGetValue(pl.UserId, out var data) &&
					(data.trainee || data.helper || data.mainhelper || data.admin || data.mainadmin || data.owner || CustomDonates.ThisYt(data)))
				{
					data.anonym = false;
					Levels.SetPrefix(pl);
					pl.SendConsoleMessage("Успешно", "green");
				}
				else pl.SendConsoleMessage("Зачем тебе это?", "red");
			}
			catch
			{
				__instance.TargetConsolePrint(__instance.connectionToClient, "Зачем тебе это?", "red");
			}
			return false;
		}
	}
}