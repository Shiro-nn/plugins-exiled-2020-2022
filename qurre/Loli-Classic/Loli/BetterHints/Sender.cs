using Qurre.API;
using MEC;

namespace Loli.BetterHints
{
    static class Sender
    {
        static internal void Hint(this Player pl, HintStruct hs)
        {
            if (!Manager.Hints.ContainsKey(pl)) Manager.Hints.Add(pl, new());
            var list = Manager.Hints[pl];
            list.Add(hs);
            Timing.CallDelayed(hs.Duration, () => list.Remove(hs));
        }
    }
}