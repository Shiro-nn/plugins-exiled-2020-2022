using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;
namespace Loli.Scps.Scp294.Drinks
{
    public sealed class Beer : IDrink
    {
        public string Name { get; } = "Пиво";

        public string Description { get; } = "";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            if (pl.PlayerEffectsController.AllEffects.TryGetValue(typeof(CustomPlayerEffects.Bleeding), out var effect))
            {
                if (effect is CustomPlayerEffects.Bleeding blend)
                {
                    blend.minDamage = 0;
                    blend.maxDamage = 3;
                }
            }
            pl.EnableEffect<CustomPlayerEffects.Bleeding>(5);
            pl.Hint(new(0, -4, "Сейчас бы рыбки...", 5, true));
        }
    }
}