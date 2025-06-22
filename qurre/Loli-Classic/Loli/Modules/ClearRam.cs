using MEC;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Loli.Modules
{
    static class ClearRam
    {
        [EventMethod(RoundEvents.End)]
        static void RouneEnd(RoundEndEvent ev)
        {
            Timing.CallDelayed(ev.ToRestart - 3f, () =>
            {
                foreach (Player pl in Player.List)
                    pl.Client.DimScreen();
                Timing.CallDelayed(1f, () => Server.Restart());
            });
        }
    }
}