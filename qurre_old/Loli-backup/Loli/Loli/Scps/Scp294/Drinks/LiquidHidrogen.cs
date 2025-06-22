using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
namespace Loli.Scps.Scp294.Drinks
{
    public sealed class LiquidHidrogen : IDrink
    {
        public string Name { get; } = "Водород";

        public string Description { get; } = "Жидкий водород";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Kill("Повреждение слизистой оболочки");
        }
    }
}