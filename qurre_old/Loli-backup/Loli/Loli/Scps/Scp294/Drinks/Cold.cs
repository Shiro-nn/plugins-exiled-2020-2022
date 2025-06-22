using Qurre.API;
using Qurre.API.Objects;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;
using CustomPlayerEffects;
namespace Loli.Scps.Scp294.Drinks
{
    public sealed class Cold : IDrink
    {
        public string Name { get; } = "Мороз";

        public string Description { get; } = "Холодное...";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Х-х-холодно..", 5, true));
            if (pl.TryGetEffect(EffectType.Hypothermia, out PlayerEffect pe))
            {
                Patches.LockHypothermia.Lock = true;
                pe.Intensity = 16;
                pl.EnableEffect(pe, 20);
                MEC.Timing.CallDelayed(20, () => Patches.LockHypothermia.Lock = false);
            }
        }
    }
}