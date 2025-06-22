using Dissonance;
using Dissonance.Integrations.MirrorIgnorance;
using Dissonance.Networking;
using HarmonyLib;
namespace Audio.Pathes
{
    [HarmonyPatch(typeof(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit>), nameof(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit>.RunAsDedicatedServer))]
    internal static class RunAsHostFix
    {
        private static bool Prefix(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit> __instance)
        {
            __instance.RunAsHost(Unit.None, Unit.None);
            return false;
        }
    }
}