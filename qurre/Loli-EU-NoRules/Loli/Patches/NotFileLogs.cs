using HarmonyLib;

namespace Loli.Patches
{
	[HarmonyPatch(typeof(ServerLogs), nameof(ServerLogs.StartLogging))]
	internal static class NotFileLogs1
	{
		[HarmonyPrefix]
		internal static bool Prefix() => false;
	}

	[HarmonyPatch(typeof(ServerLogs), "AddLog")]
	internal static class NotFileLogs2
	{
		[HarmonyPrefix]
		internal static bool Prefix() => false;
	}

	[HarmonyPatch(typeof(ServerLogs), "AppendLog")]
	internal static class NotFileLogs3
	{
		[HarmonyPrefix]
		internal static bool Prefix() => false;
	}
}