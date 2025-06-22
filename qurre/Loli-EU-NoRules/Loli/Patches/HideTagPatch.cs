using HarmonyLib;
using Loli.DataBase;
using Loli.DataBase.Modules;
using Qurre.API;

namespace Loli.Patches
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserCode_CmdRequestHideTag))]
	static class HideTagPatch
	{
		[HarmonyPrefix]
		static bool Call(CharacterClassManager __instance)
		{
			if (__instance.UserId.Contains("@northwood"))
				return true;
			try
			{
				var pl = __instance.UserId.GetPlayer();
				if (pl.Administrative.ServerRoles.NetworkGlobalBadge != "")
				{
					pl.Administrative.ServerRoles.NetworkGlobalBadge = "";
					Levels.SetPrefix(pl);
					pl.Client.SendConsole("Успешно", "green");
				}
				else
				{
					if (Data.Users.TryGetValue(pl.UserInfomation.UserId, out var data) &&
						(data.administration.admin || data.administration.moderator || CustomDonates.ThisYt(data)))
					{
						data.anonym = true;
						Levels.SetPrefix(pl);
						pl.Client.SendConsole("Успешно", "green");
					}
					else pl.Client.SendConsole("Зачем тебе убирать префикс?", "red");
				}
			}
			catch
			{
				__instance.ConsolePrint("Зачем тебе убирать префикс?", "red");
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserCode_CmdRequestShowTag))]
	static class ShowTagPatch
	{
		[HarmonyPrefix]
		static bool Call(CharacterClassManager __instance)
		{
			if (__instance.UserId.Contains("@northwood"))
				return true;
			try
			{
				var pl = __instance.UserId.GetPlayer();
				if (Data.Users.TryGetValue(pl.UserInfomation.UserId, out var data) &&
					(data.administration.admin || data.administration.moderator || CustomDonates.ThisYt(data)))
				{
					data.anonym = false;
					Levels.SetPrefix(pl);
					pl.Client.SendConsole("Успешно", "green");
				}
				else pl.Client.SendConsole("Зачем тебе это?", "red");
			}
			catch
			{
				__instance.ConsolePrint("Зачем тебе это?", "red");
			}
			return false;
		}
	}
}