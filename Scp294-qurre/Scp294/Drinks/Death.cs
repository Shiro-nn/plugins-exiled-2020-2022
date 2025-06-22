using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Death : IDrink
    {
        public string Name { get; } = "Смерть";

        public string Description { get; } = "Жидкая смерть";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player player)
        {
            player.Kill("");
        }
    }
}