using Qurre.API;
using MEC;
namespace ClassicCore.BetterHints
{
    internal static class Sender
    {
        internal static void Hint(this Player pl, HintStruct hs)
        {
            if (!Manager.Hints.ContainsKey(pl)) Manager.Hints.Add(pl, new());
            var list = Manager.Hints[pl];
            list.Add(hs);
            Timing.CallDelayed(hs.Duration, () => list.Remove(hs));
        }
    }
}