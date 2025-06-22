using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
using Qurre.API.Objects;
using CustomPlayerEffects;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Vodka : IDrink
    {
        public string Name { get; } = "Водка";

        public string Description { get; } = "";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Кааааак х-хорош-шо", 5, true));
            if (pl.PlayerEffectsController.AllEffects.TryGetValue(typeof(Bleeding), out var effect))
            {
                if (effect is Bleeding blend)
                {
                    blend.minDamage = 0;
                    blend.maxDamage = 3;
                }
            }
            pl.EnableEffect<Bleeding>(10);
            pl.EnableEffect(EffectType.Burned, 30);
            pl.EnableEffect(EffectType.Invigorated, 30);
            if (pl.TryGetEffect(EffectType.MovementBoost, out PlayerEffect playerEffect))
            {
                playerEffect.Intensity = 7;
                pl.EnableEffect(playerEffect, 10);
            }
        }
    }
}