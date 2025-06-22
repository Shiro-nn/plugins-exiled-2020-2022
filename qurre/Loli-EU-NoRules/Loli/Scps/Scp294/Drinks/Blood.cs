using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;
using CustomPlayerEffects;

namespace Loli.Scps.Scp294.Drinks
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
            if (pl.Effects.Controller.TryGetEffect(out Bleeding blend))
            {
                blend.minDamage = 0;
                blend.maxDamage = 3;
            }
            pl.Effects.Enable<Bleeding>(10);
            pl.Hint(new(0, -4, "Мммм~, я могу подсесть на это ❤️", 5, true));
        }
    }
}