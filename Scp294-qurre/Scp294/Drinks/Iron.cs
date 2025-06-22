using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Iron : IDrink
    {
        public string Name { get; } = "Железо";

        public string Description { get; } = "Жидкое железо";

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