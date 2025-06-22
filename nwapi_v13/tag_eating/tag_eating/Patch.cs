using CommandSystem;
using HarmonyLib;

namespace tag_eating
{
    [HarmonyPatch(typeof(ICommand), nameof(ICommand.SanitizeResponse), MethodType.Getter)]
    internal class Patch
    {
        [HarmonyPrefix]
        bool Call(out bool __result)
        {
            __result = false;
            return false;
        }
    }
}
