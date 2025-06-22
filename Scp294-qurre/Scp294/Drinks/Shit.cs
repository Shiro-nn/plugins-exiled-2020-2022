using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Shit : IDrink
    {
        public string Name { get; } = "Дерьмо";

        public string Description { get; } = "Ну и гадость";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player player)
        {
            player.ShowHint("Я не буду пить это.", 5f);
            return false;
        }

        public void OnDrank(Player _)
        {
        }
    }
}