using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;
using Qurre.API.Objects;
using CustomPlayerEffects;

namespace Loli.Scps.Scp294.Drinks
{
    public sealed class Juice : IDrink
    {
        public string Name { get; } = "Сок";

        public string Description { get; } = "Фруктовый сок";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "оооо, такооой ~ вкусный", 5, true));
            pl.HealthInfomation.Stamina = 100;
            if (pl.Effects.TryGet(EffectType.MovementBoost, out StatusEffectBase playerEffect))
            {
                playerEffect.Intensity = 7;
                pl.Effects.Enable(playerEffect, 10);
            }
        }
    }
}