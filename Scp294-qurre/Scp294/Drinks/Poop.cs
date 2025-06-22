using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Poop : IDrink
    {
        public string Name { get; } = "Фекалии";

        public string Description { get; } = "Жидкие фекалии";

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