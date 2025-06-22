using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class SeaWater : IDrink
    {
        public string Name { get; } = "Морская вода";

        public string Description { get; } = "Просто морская вода";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player _)
        {
        }
    }
}