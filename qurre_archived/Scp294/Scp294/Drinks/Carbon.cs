using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Carbon : IDrink
    {
        public string Name { get; } = "Углерод";

        public string Description { get; } = "Жидкий углерод";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player player)
        {
            player.Kill("Ожоги третьей степени.");
        }
    }
}