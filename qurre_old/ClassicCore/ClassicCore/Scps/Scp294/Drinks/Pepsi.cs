using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Pepsi : IDrink
    {
        public string Name { get; } = "Пепси";

        public string Description { get; } = "Обычная газировка";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "А что не кола?)", 5, true));
            pl.ResetStamina();
        }
    }
}