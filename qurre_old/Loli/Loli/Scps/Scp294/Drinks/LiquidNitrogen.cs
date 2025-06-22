using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Qurre.API.Objects;
using CustomPlayerEffects;
using Loli.BetterHints;
namespace Loli.Scps.Scp294.Drinks
{
    public sealed class LiquidNitrogen : IDrink
    {
        public string Name { get; } = "Жидкий азот";

        public string Description { get; } = "";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "З-зачем я э-эт-то с-сделал", 5, true));
            if (pl.TryGetEffect(EffectType.Hypothermia, out PlayerEffect playerEffect))
            {
                Patches.LockHypothermia.Lock = true;
                playerEffect.Intensity = 11;
                pl.EnableEffect(playerEffect, 30);
                MEC.Timing.CallDelayed(30, () => Patches.LockHypothermia.Lock = false);
            }
        }
    }
}