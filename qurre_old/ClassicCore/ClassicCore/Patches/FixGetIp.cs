using CommandSystem.Commands.Console;
using HarmonyLib;
namespace ClassicCore.Patches
{
    [HarmonyPatch(typeof(IpCommand), nameof(IpCommand.Execute))]
    internal static class FixGetIp
    {
        internal static bool Prefix(out string response, out bool __result)
        {
            __result = false;
            response = "Данная команда отключена";
            return false;
        }
    }
}