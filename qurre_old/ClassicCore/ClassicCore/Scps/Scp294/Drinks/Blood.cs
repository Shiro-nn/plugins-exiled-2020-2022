using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Blood : IDrink
    {
        public string Name { get; } = "Кровь";

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
            pl.EnableEffect<CustomPlayerEffects.Bleeding>(10);
            pl.Hint(new(0, -4, "На вкус как вино", 5, true));
        }
    }
}