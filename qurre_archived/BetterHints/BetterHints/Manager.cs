using Hints;
using MEC;
using Qurre.API;
using System.Collections.Generic;
using System.Linq;
namespace BetterHints
{
    internal static class Manager
    {
        internal static readonly Dictionary<Player, List<string>> FixHints = new();
        internal static void ShowHint(Player pl, string text, float dur)
        {
            if (!FixHints.ContainsKey(pl)) FixHints.Add(pl, new List<string>());
            var list = text.Trim().Split('\n');
            foreach (var str in list)
            {
                string _ = str.Replace("\n", "").Trim();
                if (_ == "") continue;
                if (!FixHints.TryGetValue(pl, out var _data)) continue;
                _data.Add(_);
                Timing.CallDelayed(dur, () => { if (_data.Contains(_)) _data.Remove(_); });
            }
        }
        internal static void Cycle()
        {
            static string tps(int strings) => $"\n<align=left><voffset={-20 + strings}em><b><color=red><size=80%><pos=10%>TPS: {Loader.TicksMinutes}" +
                $"</pos></size></color></b></voffset></align>";
            foreach (Player pl in Player.List)
            {
                try
                {
                    string str = "";
                    if (FixHints.TryGetValue(pl, out var fix))
                    {
                        for (int i = 0; fix.Count > i; i++)
                        {
                            if (fix[i] != "")
                            {
                                str += fix[i];
                                str += "\n";
                            }
                        }
                    }
                    if (Loader.ShowTps) str += tps(str.Split('\n').Length);
                    pl.HintDisplay.Show(new TextHint(str, new HintParameter[] { new StringHintParameter(Loader.HintToken) }, null, 1.1f));
                }
                catch { }
            }
        }
        internal static bool PatchYes(HintDisplay __instance, Hint hint)
        {
            if (hint is not TextHint _h) return true;
            if (_h.Parameters.Where(x => Yes(x)).Count() > 0) return true;
            var pls = Player.List.Where(x => x.NetworkIdentity.netId == __instance.netId);
            if (pls.Count() == 0) return false;
            var pl = pls.First();
            ShowHint(pl, _h.Text, _h.DurationScalar);
            return false;
            static bool Yes(HintParameter hp)
            {
                if (hp is not StringHintParameter shp) return false;
                return shp.Value.Contains(Loader.HintToken);
            }
        }
    }
}