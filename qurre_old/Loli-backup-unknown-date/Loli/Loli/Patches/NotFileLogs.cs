using HarmonyLib;
namespace Loli.Patches
{
	[HarmonyPatch(typeof(ServerLogs), nameof(ServerLogs.StartLogging))]
	internal static class NotFileLogs1
	{
		internal static bool Prefix() => false;
	}
	[HarmonyPatch(typeof(ServerLogs), "AddLog")]
	internal static class NotFileLogs2
	{
		internal static bool Prefix(ServerLogs.Modules module, string msg, ServerLogs.ServerLogType type, bool init = false) => false;
	}
	[HarmonyPatch(typeof(ServerLogs), "AppendLog")]
	internal static class NotFileLogs3
	{
		internal static bool Prefix() => false;
	}
}