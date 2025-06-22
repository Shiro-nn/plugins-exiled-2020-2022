using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Perfume : IDrink
    {
        public string Name { get; } = "Яд";

        public string Description { get; } = "";

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