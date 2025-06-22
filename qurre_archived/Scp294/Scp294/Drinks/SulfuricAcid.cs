using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class SulfuricAcid : IDrink
    {
        public string Name { get; } = "Серная кислота";

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
            player.Kill("");
        }
    }
}