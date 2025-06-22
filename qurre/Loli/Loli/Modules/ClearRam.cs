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
            });/*
            if (Plugin.ServerID == 4 || Plugin.ServerID == 5)
            {
                Timing.CallDelayed(ev.ToRestart - 3f, () =>
                {
                    foreach (Player pl in Player.List) pl.DimScreen();
                    Timing.CallDelayed(1f, () => Server.Restart());
                });
                return;
            }
            try
            {
                var url = "http://localhost:1337";
                var req = System.Net.WebRequest.Create(url);
                var resp = req.GetResponse();
                using var sr = new System.IO.StreamReader(resp.GetResponseStream());
                var response = sr.ReadToEnd().Trim();
                if (int.Parse(response) >= 85)
                {
                    Timing.CallDelayed(ev.ToRestart - 3f, () =>
                    {
                        foreach (Player pl in Player.List) pl.DimScreen();
                        Timing.CallDelayed(1f, () => NetworkServer.SendToAll(new RoundRestartMessage(RoundRestartType.FullRestart, 25, 0, true, true)));
                        Timing.CallDelayed(2.9f, () => Server.Restart());
                    });
                }
            }
            catch { }*/
        }
    }
}