using Qurre.API;
using Qurre.API.Objects;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Panic : IDrink
    {
        public string Name { get; } = "Паника";

        public string Description { get; } = "Жидкая паника";

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