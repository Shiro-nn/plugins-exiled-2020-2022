using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;
using Qurre.API.Objects;
namespace Loli.Scps.Scp294.Drinks
{
    public sealed class Hot : IDrink
    {
        public string Name { get; } = "Жар";

        public string Description { get; } = "Горячее...";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Ай, ай, ай, горячо то как", 5, true));
            if (pl.PlayerEffectsController.AllEffects.TryGetValue(typeof(CustomPlayerEffects.Bleeding), out var effect))
            {
                if (effect is CustomPlayerEffects.Bleeding blend)
                {
                    blend.minDamage = 0;
                    blend.maxDamage = 3;
                }
            }
            pl.EnableEffect<CustomPlayerEffects.Bleeding>(10);
            pl.EnableEffect(EffectType.Burned, 30);
        }
    }
}