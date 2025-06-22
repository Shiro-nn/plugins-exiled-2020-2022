using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;

namespace Loli.Scps.Scp294.Drinks
{
    public sealed class SeaWater : IDrink
    {
        public string Name { get; } = "Морская вода";

        public string Description { get; } = "Просто морская вода";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Как же захотелось пить..", 5, true));
            pl.HealthInfomation.Stamina = 0;
        }
    }
}