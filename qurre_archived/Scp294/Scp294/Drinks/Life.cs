using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Life : IDrink
    {
        public string Name { get; } = "Жизнь";

        public string Description { get; } = "Жидкая жизнь";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player player)
        {
            // TODO: full heal
            player.Heal(player.MaxHp - player.Hp, false);
        }
    }
}