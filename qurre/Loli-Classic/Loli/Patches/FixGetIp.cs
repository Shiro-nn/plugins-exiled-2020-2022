using CommandSystem.Commands.Console;
using HarmonyLib;

namespace Loli.Patches
{
    [HarmonyPatch(typeof(IpCommand), nameof(IpCommand.Execute))]
    static class FixGetIp
    {
        [HarmonyPrefix]
        static bool Call(out string response, out bool __result)
        {
            __result = false;
            response = "Данная команда отключена";
            return false;
        }
    }
}