using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
namespace ClassicCore.Scps.Scp294.Drinks
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
            pl.Hint(new(0, -4, "Тааак освежает", 5, true));
            pl.Heal(20, false);
        }
    }
}