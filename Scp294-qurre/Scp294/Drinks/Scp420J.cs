using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Scp402J : IDrink
    {
        public string Name { get; } = "SCP-420-J";

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

        }
    }
}