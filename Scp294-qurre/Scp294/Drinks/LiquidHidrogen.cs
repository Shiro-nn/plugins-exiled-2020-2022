using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class LiquidHidrogen : IDrink
    {
        public string Name { get; } = "Водород";

        public string Description { get; } = "Жидкий водород";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player player)
        {
            player.Kill();
        }
    }
}