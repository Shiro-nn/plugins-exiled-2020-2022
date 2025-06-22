using Dissonance.Audio.Capture;
using HarmonyLib;
namespace Audio.Pathes
{
    [HarmonyPatch(typeof(CapturePipelineManager), nameof(CapturePipelineManager.RestartTransmissionPipeline))]
    internal static class RestartTransmissionPipelineFix
    {
        private static bool Prefix(string reason) => reason != "Detected a frame skip, forcing capture pipeline reset";
    }
}