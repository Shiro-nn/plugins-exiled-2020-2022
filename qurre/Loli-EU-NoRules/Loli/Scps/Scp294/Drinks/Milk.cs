using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;

namespace Loli.Scps.Scp294.Drinks
{
    public sealed class Milk : IDrink
    {
        public string Name { get; } = "Молоко";

        public string Description { get; } = "Обычное молоко";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Тааак ~ освежает", 5, true));
            pl.HealthInfomation.Heal(20, false);
        }
    }
}