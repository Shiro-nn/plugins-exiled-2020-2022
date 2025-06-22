using MEC;
using Mirror;
using Qurre.API;
using Qurre.API.Events;
using RoundRestarting;
namespace Loli.Modules
{
    public partial class EventHandlers
    {
        public void ClearRam(RoundEndEvent ev)
        {
            /*Timing.CallDelayed(ev.ToRestart - 3f, () =>
            {
                foreach (Player pl in Player.List) pl.DimScreen();
                Timing.CallDelayed(1f, () => NetworkServer.SendToAll(new RoundRestartMessage(RoundRestartType.FullRestart,
                    Plugin.ServerID == 1 || Plugin.ServerID == 2 || Plugin.ServerID == 7 || Plugin.ServerID == 8 || Plugin.ServerID == 10 || Plugin.ServerID == 4 ? 15 : 20, 0, true, true)));
                Timing.CallDelayed(2.9f, () => Server.Restart());
            });*/
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
            catch { }
        }
    }
}