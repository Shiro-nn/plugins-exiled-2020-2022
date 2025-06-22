using Qurre.API;
namespace Loli.Addons
{
    internal static class YtSAutoOW
    {
        internal static bool Event = false;
        private static bool Started = false;
        internal static void Update()
        {
            if (!Plugin.YouTubersServer) return;
            if (Event) return;
            if (!Started && Round.ElapsedTime.TotalMinutes > 20)
            {
                Started = true;
                Map.Broadcast("<size=25%><color=#6f6f6f>Совет О5 отдал <color=yellow>приказ</color> на <color=red>взрыв</color> <color=#0089c7>ОМЕГА Боеголовки</color></color></size>", 15, true);
                OmegaWarhead.Start();
            }
        }
    }
}