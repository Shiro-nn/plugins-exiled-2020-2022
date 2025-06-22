using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Chocolate : IDrink
    {
        public string Name { get; } = "Шоколад";

        public string Description { get; } = "Горячий шоколад";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "ммм, вкусно", 5, true));
        }
    }
}