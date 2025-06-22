using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;

namespace Loli.Scps.Scp294.Drinks
{
    public sealed class CocaCola : IDrink
    {
        public string Name { get; } = "Кока-Кола";

        public string Description { get; } = "Обычная газировка";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Это же не SCP-207, я надеюсь?", 5, true));
            pl.HealthInfomation.Stamina = 100;
        }
    }
}