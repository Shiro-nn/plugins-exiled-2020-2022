using HarmonyLib;
using System;
namespace Loli_Time
{
	[HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.OnDestroy))]
	internal static class Leave
	{
		static void Prefix(ReferenceHub __instance)
		{
			try
			{
				Events.Leave(__instance);
			}
			catch (Exception ex)
			{
				ServerConsole.AddLog($"{ex}", ConsoleColor.Red);
			}
		}
	}
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserId), MethodType.Setter)]
	internal static class Join
	{
		static void Postfix(CharacterClassManager __instance, string value)
		{
			try
			{
				Events.Join(__instance, value);
			}
			catch(Exception ex)
			{
				ServerConsole.AddLog($"{ex}", ConsoleColor.Red);
			}
		}
	}
}