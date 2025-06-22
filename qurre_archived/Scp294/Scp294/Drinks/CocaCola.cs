using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class CocaCola : IDrink
    {
        public string Name { get; } = "Кока-Кола";

        public string Description { get; } = "Обычная газировка";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player player)
        {

        }
    }
}