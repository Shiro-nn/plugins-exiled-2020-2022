using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Piss : IDrink
    {
        public string Name { get; } = "Моча";

        public string Description { get; } = "";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player player)
        {
            player.ShowHint("Я не буду пить это.", 4);
            return false;
        }

        public void OnDrank(Player _)
        {
        }
    }
}